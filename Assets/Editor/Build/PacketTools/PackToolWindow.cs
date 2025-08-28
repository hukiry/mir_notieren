using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using Hukiry;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Hukiry.Socket;
using HukiryInitialize;
/********************************************************************
author:	Hukiry
date: 2021-9-15
desc: 导出AssetBundle可视面板
资源更新
版本更新
正式和测试包，打包
宏定义
*********************************************************************/

namespace Hukiry.Pack {


	public class PackToolWindow : EditorWindow,IHasCustomMenu {
		bool isStartBuild = false;
		private string versionLabel = "1.0.0";
		private VersionsInfo curVerInfo;

		bool isDeleteLuaScript = false;
		bool isClearAssetBundleName = false;
		bool isDeleteCacheAsset = false;
		bool isProgressiveVer = false;
		bool isDeleteCacheGame = false;
		bool isCopySourceRes = false;

		static PackToolWindow window;
        private GUIStyle mButtonDef;
        private static GUIStyle mBoxStyle;

		private static string SystemPlatform=>
#if UNITY_ANDROID
            "d_BuildSettings.Android.Small";
#elif UNITY_WEBGL
            "d_BuildSettings.WebGL";
#elif UNITY_STANDALONE || UNITY_WEBPLAYER
            "d_BuildSettings.Standalone";
#else
			"d_BuildSettings.iPhone.Small";
#endif
		public static void ShowWindow() {
			window = GetWindow<PackToolWindow>();
			window.titleContent = new GUIContent("打包资源",Hukiry.HukiryUtilEditor.GetTexture2D(SystemPlatform));
			window.minSize = new Vector2(540, 600);
			window.maxSize = new Vector2(540, 600);
			window.Show();
		}

		private EBuildSystem buildSyste = EBuildSystem.Gradle;
		private ScriptingImplementation m_scriptI = ScriptingImplementation.IL2CPP;
		private EWorkMode m_WorkMode = EWorkMode.Debug;
		private PackageType packageType = PackageType.apk;
		private GUIStyle mBoldLabelDef2;

        void OnEnable() {		
			packageType = PackingWinConfigAssets.Instance.packageType;
			Enum.TryParse(PackingWinConfigAssets.Instance.WorkMode, out EWorkMode result);
			m_WorkMode = result;

			InitVersion();
		}

		void OnGUI() {

            mBoldLabelDef2 = new GUIStyle(GUI.skin.label);
            mBoldLabelDef2.fontSize = 20;
			mButtonDef = new GUIStyle(GUI.skin.button);
            mBoxStyle = new GUIStyle(GUI.skin.box);
			mBoxStyle.padding = new RectOffset(6, 6, 6, 6);
			using (new GUILayout.VerticalScope("FrameBox"))
			{
				BuildTool();
			}

            if (isStartBuild) {
				StartPack();
			}

			//删除缓存lua脚本
			if (isDeleteLuaScript) {
				BuildScript.BuildDeleteLuaFiles(EditorUserBuildSettings.activeBuildTarget);
			}

            //清除所有AssetBundleName
            if (isClearAssetBundleName)
            {
                AssetBundleExport.ClearAssetBundlesName();
            }

            //删除streamAsset缓存ab资源
            if (isDeleteCacheAsset) {
				AssetBundleExport.ClearAllAssetBundles();
			}

			if (isDeleteCacheGame)
			{
				//BinaryDataMgr.instance.DeleteGameCache();
				PlayerPrefs.DeleteAll();
			}

			if (isCopySourceRes)
			{
				this.CopySourceRes();//手动拷贝
			}

		}

		private void BuildTool()
		{
			var ins = PackingWinConfigAssets.Instance;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			int fontSize = GUI.skin.label.fontSize;
			EditorGUILayout.Space();
			//资源
			using (new GUILayout.VerticalScope(mBoxStyle))
			{
				EditorGUILayout.LabelField("资源", mBoldLabelDef2, GUILayout.Height(30));

				using (new GUILayout.VerticalScope(mBoxStyle))
				{
					using (new GUILayout.HorizontalScope())
					{
						isDeleteCacheGame = GUILayout.Button("清除游戏缓存", mButtonDef);
						isDeleteLuaScript = GUILayout.Button("删除Lua缓存代码", mButtonDef);
						isClearAssetBundleName = GUILayout.Button("清除资源ab名", mButtonDef);
						isDeleteCacheAsset = GUILayout.Button("清除包内ab文件", mButtonDef);
					}

					EditorGUILayout.Space();
					using (new GUILayout.HorizontalScope())
					{
						
						isCopySourceRes = GUILayout.Button("拷贝底包资源", mButtonDef);
						
					}
				}

				using (new GUILayout.HorizontalScope(mBoxStyle))
				{
					DrawToggleGUI(nameof(ins.Lua), true);
                    //DrawToggleGUI("Res", true);

                    GUI.color = Color.green;
					if (GUILayout.Button("打开热更资源目录"))
					{
						if (!Directory.Exists(BuildPackConfig.HotUpdatePath)) Directory.CreateDirectory(BuildPackConfig.HotUpdatePath);
						Application.OpenURL(BuildPackConfig.HotUpdatePath);
					}

					if (GUILayout.Button("打开包所在目录"))
					{
						if (!Directory.Exists(BuildScript.outPackageDirPath)) Directory.CreateDirectory(BuildScript.outPackageDirPath);
						Application.OpenURL(BuildScript.outPackageDirPath);
					}
					GUI.color = Color.white;
				}
			}

			EditorGUILayout.Space();//版本
			using (new GUILayout.VerticalScope(mBoxStyle))
			{
				EditorGUILayout.LabelField("版本", mBoldLabelDef2, GUILayout.Height(30));

				var workMode = (EWorkMode)EditorGUILayout.EnumPopup("打包模式", m_WorkMode);
				EditorGUILayout.Space();

				if (m_WorkMode != workMode)
				{
					m_WorkMode = workMode;
					PackingWinConfigAssets.Instance.WorkMode = workMode.ToString();
					InitVersion();
				}

				using (new GUILayout.HorizontalScope(mBoxStyle))
				{
					EditorGUILayout.LabelField(string.Format("当前版本号：{0}", this.curVerInfo.GetVersionName()));
					GUI.color = Color.cyan;
					EditorGUILayout.LabelField($"新版本号：{versionLabel}");
					GUI.color = Color.white;
					if (GUILayout.Button("递增版本号", mButtonDef, GUILayout.Width(80)))//
					{
						isProgressiveVer = true;
					}
				}

				using (new GUILayout.HorizontalScope())
				{
					DrawToggleGUI(nameof(ins.BuildPlayer));

					if (GUILayout.Button(string.Empty, GUI.skin.label, GUILayout.Width(100), GUILayout.Height(20)))
					{
						GenericMenu menu = new GenericMenu();
						menu.AddItem(new GUIContent("Hukiry Configuration"), false, () => SettingsService.OpenProjectSettings("Project/Hukiry Configuration"));
						menu.AddItem(new GUIContent("Hukiry Symbol"), false, () => SettingsService.OpenUserPreferences("Preferences/Hukiry/Symbol"));
						menu.AddItem(new GUIContent("Hukiry Path"), false, () => SettingsService.OpenUserPreferences("Preferences/Hukiry/Path"));
						menu.AddItem(new GUIContent("External Tools"), false, () => SettingsService.OpenUserPreferences("Preferences/External Tools"));
						menu.ShowAsContext();
					}

					if (GUILayout.Button("Project Settings", GUI.skin.box, GUILayout.Width(120), GUILayout.Height(20)))
					{
						SettingsService.OpenProjectSettings("Project/Player");
					}
				}
			}

			this.SetVersion();

			EditorGUILayout.Space();
			string bulidStartTxt = "开始";
			if (PackingWinConfigAssets.Instance.BuildPlayer)//包
			{
				using (new GUILayout.VerticalScope(mBoxStyle))
				{
					EditorGUILayout.LabelField("编译包", mBoldLabelDef2, GUILayout.Height(30));

					string productName = PackingWinConfigAssets.Instance.productName; // "Come Match";
#if UNITY_ANDROID
					string identifier = "com.yiliu.android.HappyMatch";
#elif UNITY_IOS || UNITY_IPHONE
					this.EnableiOSSetting();
					string identifier = PackingWinConfigAssets.Instance.identifier;// "com.yiliu.HappyMatch";
#else
					string identifier = "com.win.sdk";
#endif
					if (m_WorkMode != EWorkMode.Release && m_WorkMode != EWorkMode.Develop)
					{
						productName = $"Come Match";
					}
					UnityEditor.PlayerSettings.companyName = "yiliu";
					UnityEditor.PlayerSettings.productName = productName;
					UnityEditor.PlayerSettings.applicationIdentifier = identifier;
					GUI.skin.label.fontSize = 18;
					GUI.skin.label.alignment = TextAnchor.MiddleLeft;
					GUILayout.Label($"出包:      {BuildScript.GetOutFileName(packageType, versionLabel, m_WorkMode.ToString())}");
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Label($"包名:      {identifier}");
						if (GUILayout.Button(string.Empty, GUI.skin.label, GUILayout.Width(200), GUILayout.Height(20)))
						{
							GenericMenu menu = new GenericMenu();
							menu.AddItem(new GUIContent("设置包名"), false, () => ScriptableWizard.DisplayWizard<PromptWizardEditor>(string.Empty));
							menu.ShowAsContext();
						}
					}
					GUILayout.Label($"版本:      {versionLabel}");
				
					GUI.skin.label.fontSize = fontSize;
					GUI.skin.label.alignment = TextAnchor.MiddleCenter;
					using (new GUILayout.HorizontalScope())
					{
						DrawToggleGUI(nameof(ins.Enable_SDK), true);
						if (m_WorkMode != EWorkMode.Release)
						{
							DrawToggleGUI(nameof(ins.Fps), true);
							DrawToggleGUI(nameof(ins.Battery), true);
						}
					}
					Hukiry.HukiryUtilEditor.DrawLine(Color.green*0.6f, Color.white*0.2f);
					packageType = (PackageType)EditorGUILayout.EnumPopup("包类型", packageType);
					m_scriptI = (ScriptingImplementation)EditorGUILayout.EnumPopup("ScriptingBackend", m_scriptI);
#if UNITY_ANDROID
					if (packageType == PackageType.aab) buildSyste = EBuildSystem.Gradle;
					using (new GUILayout.HorizontalScope())
					{
						DrawToggleGUI("Armv7");
						DrawToggleGUI("Arm64");
					}

					if (m_scriptI == ScriptingImplementation.IL2CPP)
					{
						buildSyste = (EBuildSystem)EditorGUILayout.EnumPopup("BuildSystem", buildSyste);
						PackingWinConfigAssets.Instance.Arm64 = true;
					}

					bulidStartTxt = packageType == PackageType.exportProject ? "导出" : "开始";
#else
					packageType = PackageType.exportProject;
					m_scriptI = ScriptingImplementation.IL2CPP;
					bulidStartTxt = "导出";
#endif
				}
			}

			GUI.color = Color.green;
			isStartBuild = GUILayout.Button(bulidStartTxt, mButtonDef, GUILayout.Height(30));
			GUI.color = Color.white;
		}

		private void EnableiOSSetting()
		{
			//隐藏home
			PlayerSettings.iOS.hideHomeButton = true;
			//允许http下载
			PlayerSettings.iOS.allowHTTPDownload = true;
			//必须设置项
			PlayerSettings.iOS.cameraUsageDescription = "No Use Camera";
			PlayerSettings.iOS.microphoneUsageDescription = "No Use microphone";
			PlayerSettings.iOS.locationUsageDescription = "No Use location";
			//最低为12.2，正式包体会变小
			PlayerSettings.iOS.targetOSVersionString = "12.2";
			PlayerSettings.iOS.appleEnableAutomaticSigning = true;
			PlayerSettings.muteOtherAudioSources = true;
			PlayerSettings.iOS.showActivityIndicatorOnLoading = iOSShowActivityIndicatorOnLoading.DontShow;
		}

		//设置版本号
		public void SetVersion()
		{
			if (isProgressiveVer)
			{
				if (PackingWinConfigAssets.Instance.BuildPlayer)
				{
					versionLabel = this.curVerInfo.AddOversizeLargeVersions();
				}
				else
				{
					versionLabel = this.curVerInfo.AddOversizeVersions();
				}
			}
		}

		public void CopySourceRes()
		{
			string targetPath = BuildPackConfig.HotUpdatePath + "StreamingAssets";
			if (Directory.Exists(targetPath)) Directory.Delete(targetPath, true);
			FileHelper.CopyDirectory(Application.streamingAssetsPath, targetPath, null, new string[] {
					".meta",".manifest","resfile"
				});
		}

#region GUI方法
		void DrawToggleGUI(string keyName, bool isLeft = false)
		{
			var ins = PackingWinConfigAssets.Instance;
			var info = ins.GetType().GetField(keyName, BindingFlags.Public | BindingFlags.Instance);
			DescriptionAttribute desc = info.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute;
			if (isLeft)
				info.SetValue(ins, EditorGUILayout.ToggleLeft(desc.name, (bool)info.GetValue(ins), GUILayout.Width(150)));
			else
				info.SetValue(ins, EditorGUILayout.Toggle(desc.name, (bool)info.GetValue(ins)));

		}
#endregion

#region 开始打包
		public void StartPack() {
#if UNITY_STANDALONE_WIN
			EditorUtility.DisplayDialog("警告", "当前是PC，请切换平台,Steam 请修改脚本来适配", "确定");
			return;
#endif
			long lastTime = UtilCommon.GetTotalSeconds();
			string buildCode = "101";
			//设置本地版本号
			this.curVerInfo = new VersionsInfo(versionLabel);
			//设置bundle
			PlayerSettings.bundleVersion = versionLabel;
			buildCode = string.Format("{0}{1}", this.curVerInfo.firstVer, this.curVerInfo.middleVer);
#if UNITY_ANDROID
			PlayerSettings.Android.bundleVersionCode = int.Parse(buildCode);
#elif UNITY_IPHONE
			PlayerSettings.iOS.buildNumber = buildCode;
#endif
			//拷贝Lua代码
			if (PackingWinConfigAssets.Instance.Lua)
			{
				BuildScript.BuildCopyLuaFiles(EditorUserBuildSettings.activeBuildTarget);
			}

			//打AssetBundle文件 mark and build ab
			FileHelper.CreateDirectory(AssetBundleConifg.PacketAbPath);
			BuildPipeline.BuildAssetBundles(AssetBundleConifg.PacketAbPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
			AssetDatabase.Refresh();
			//记录资源处理热更资源:生成文件清单，并且写入版本号
			AssetBundleExport.GeneralBetaRecordData(PackingWinConfigAssets.Instance.BuildPlayer);
			BuildScript.LuaKeyCodeByAB(this.curVerInfo);//lua加密并拷贝到热更目录
#if UNITY_ANDROID
			//IL2CPP
			PlayerSettings.SetScriptingBackend(BuildPackConfig.buildTargetGroup, m_scriptI);
			AndroidArchitecture aac = AndroidArchitecture.None;
			if (PackingWinConfigAssets.Instance.Armv7) aac |= AndroidArchitecture.ARMv7;
			if (PackingWinConfigAssets.Instance.Arm64) aac |= AndroidArchitecture.ARM64;
			PlayerSettings.Android.targetArchitectures = aac;

			if (buildSyste == EBuildSystem.Gradle) {
				EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
			} else {
				EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
			}
#endif
			
			//编译包
			if (PackingWinConfigAssets.Instance.BuildPlayer) {
				#region 设置宏
				string macro = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildPackConfig.buildTargetGroup);
				List<string> macroArr = macro.Split(';').ToList();
				SetMacro(ref macroArr, m_WorkMode == EWorkMode.Debug, UnitySymbol.DEBUG);
				SetMacro(ref macroArr, m_WorkMode == EWorkMode.Develop, UnitySymbol.DEVELOP);
				SetMacro(ref macroArr, m_WorkMode == EWorkMode.Release, UnitySymbol.RELEASE);

				if (m_WorkMode != EWorkMode.Release)
				{
					SetMacro(ref macroArr, PackingWinConfigAssets.Instance.Fps, UnitySymbol.ENABLE_FPS);
					SetMacro(ref macroArr, PackingWinConfigAssets.Instance.Battery, UnitySymbol.SYSTEM_INFO);
				}
				else
				{
					SetMacro(ref macroArr, false, UnitySymbol.ENABLE_FPS);
					SetMacro(ref macroArr, false, UnitySymbol.SYSTEM_INFO);
				}
				SetMacro(ref macroArr, PackingWinConfigAssets.Instance.Enable_SDK, UnitySymbol.ENABLE_SDK);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPackConfig.buildTargetGroup, string.Join(";", macroArr.ToArray()));
				AssetDatabase.Refresh();
				#endregion
#if UNITY_ANDROID
				EditorUserBuildSettings.buildAppBundle = false;
				EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
				switch (packageType)
				{
					case PackageType.aab:
						EditorUserBuildSettings.buildAppBundle = true;
						EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
						break;
					case PackageType.exportProject:
						EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
						EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
						break;
				}
#endif
				//打包时，设置热更目录
				PackingWinConfigAssets.Instance.WorkMode = m_WorkMode.ToString();
				PackingWinConfigAssets.Instance.packageType = packageType;
				PackingWinConfigAssets.Instance.appVersion++;
				PackingWinConfigAssets.Instance.SaveAssetsBefore();


				AssetDatabase.Refresh();
				//开始打包
				BuildScript.BuildPlayer(packageType, this.curVerInfo);
			}

			this.SaveVersion(m_WorkMode);
			isProgressiveVer = false;
			//保存版本号
			long time = UtilCommon.GetTotalSeconds() - lastTime;
			if (EditorUtility.DisplayDialog("提示", string.Format("任务完成，本次用时：{0}", UtilCommon.ToTimeStringMS(time)), "确定"))
			{
				Application.OpenURL(BuildPackConfig.HotUpdatePath);
				InitVersion();
			}
			
			AssetDatabase.Refresh();
		}


#endregion

#region 辅助方法
		/// <summary>
		/// 设置预处理编译
		/// </summary>
		/// <param name="macroArr"></param>
		/// <param name="key"></param>
		/// <param name="macro"></param>
		private void SetMacro(ref List<string> macroArr, bool key, string macro) {
			if (key) {
				if (!macroArr.Contains(macro)) {
					macroArr.Add(macro);
				}
			} else {
				if (macroArr.Contains(macro)) {
					macroArr.Remove(macro);
				}
			}
		}

		//初始化版本:启动和切换模式时更新对应版本
		private void InitVersion()
		{
			GameVersion gameVer = ReadVersion();
			this.curVerInfo = new VersionsInfo(gameVer.version);
			versionLabel = this.curVerInfo.GetVersionName();
			PackingWinConfigAssets.Instance.appVersion = gameVer.appVersion;
		}

		private void SaveVersion(EWorkMode workMode)
		{
			if (FindObjectOfType<MainGame>() == null)
			{
				throw new Exception("请切换到打包场景");
			}
			FindObjectOfType<MainGame>().WorkMode = workMode;
			string packPath = Path.Combine(AssetBundleConifg.AppPacketPath, AssetBundleConifg.VersionName);
			string verPath = Path.Combine(BuildPackConfig.HotUpdatePath, AssetBundleConifg.VersionName);
			GameVersion gameVer = ReadVersion();
			gameVer.version = this.curVerInfo.GetVersionName();
			gameVer.workMode = (int)workMode;

#if UNITY_ANDROID
			gameVer.strongerUrl = "";
#else
			string appID = "6449088011";
			//https://apps.apple.com/us/app/id{appID}  自动打开浏览器
			gameVer.strongerUrl = $"itms-apps://itunes.apple.com/app/id{appID}";// 自动打开app store
#endif
			gameVer.jsonUrl = string.Empty;
			switch (workMode)
            {
                case EWorkMode.Debug:
					gameVer.webUrl = "http://match-oss.calf66.top/web";
					gameVer.workMode = 1;
					break;
                case EWorkMode.Develop:
#if UNITY_ANDROID
					gameVer.webUrl = "http://43.139.57.32:8201/web";
#else
					gameVer.webUrl = "http://match-oss.calf66.top/web";
#endif
					break;
                case EWorkMode.Release:
					//match-three.oss-us-west-1.aliyuncs.com
					gameVer.webUrl = "http://match-oss.calf66.top/web";
					break;
                default:
                    break;
            }
			gameVer.appVersion = PackingWinConfigAssets.Instance.appVersion;
			string jsonStr = "{\n" +
					$"  \"version\": \"{ gameVer.version}\",\n" +
					$"  \"webUrl\": \"{ gameVer.webUrl}\",\n" +
					$"  \"jsonUrl\": \"{ gameVer.jsonUrl}\",\n" +
					$"  \"workMode\": { gameVer.workMode},\n" +
					$"  \"strongerUrl\": \"{ gameVer.strongerUrl}\",\n" +
					$"  \"appVersion\": { gameVer.appVersion}"
					+ "\n}";
			File.WriteAllText(packPath, jsonStr);
			File.WriteAllText(verPath, jsonStr);
		}

		private GameVersion ReadVersion()
		{
			BuildPackConfig.HotUpdateWorkModeFolder = m_WorkMode.ToString() + "/";
			string verPath = Path.Combine(BuildPackConfig.HotUpdatePath, AssetBundleConifg.VersionName);
			GameVersion gameVer = new GameVersion();
			if (File.Exists(verPath))
			{
				gameVer = JsonUtil.ToObject<GameVersion>(File.ReadAllText(verPath));
			}
			else
			{
				verPath = Path.Combine(AssetBundleConifg.AppPacketPath, AssetBundleConifg.VersionName);
				if (File.Exists(verPath))
				{
					gameVer = JsonUtil.ToObject<GameVersion>(File.ReadAllText(verPath));
				}
			}
			return gameVer;
		}

		public void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(new GUIContent("定位脚本"), false, () =>
			{
				HukiryUtilEditor.LocationObject<MonoScript>("PackToolWindow");
			});

			menu.AddItem(new GUIContent("定位标记ab目录"), false, () =>
			{
				HukiryUtilEditor.LocationObject<DefaultAsset>("ResourcesEx");
			});

			menu.AddItem(new GUIContent("定位配置"), false, () =>
			{
				HukiryUtilEditor.LocationObject<PackingWinConfigAssets>("PackingWinConfigAssets");
			});


			var data = HukiryUtilEditor.FindAssetObject<SDK.InappScriptableData>("InappScriptableData");
			if (data == null)
			{
				menu.AddItem(new GUIContent("创建内购"), false, () =>
				{
					SDK.InappScriptableData.CreateAppStoreSettingsAsset();
				});
			}
			else
			{
				menu.AddItem(new GUIContent("定位内购"), false, () =>
				{
					HukiryUtilEditor.LocationObject<SDK.InappScriptableData>("InappScriptableData");
				});
			}

		}
#endregion
	}
}