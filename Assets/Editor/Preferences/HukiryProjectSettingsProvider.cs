
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Hukiry.SDK;

namespace Hukiry.Project
{
    public partial class HukiryProjectSettingsProvider: SettingsProvider
	{
		public static HukiryProjectSettingsProvider ins { get; } = new HukiryProjectSettingsProvider("Project/Hukiry Configuration", SettingsScope.Project);
		public HukiryProjectSettingsProvider(string path, SettingsScope scopes):base(path, scopes)
		{
			this.label = "Hukiry Configuration";
			this.keywords = new HashSet<string>(new[] { "Hukiry", "Android", "Config" });
			this.guiHandler = (searchContext) =>
			{
				this.HukiryAndroidSettings();
			};

			this.InitData();
		}

		[SettingsProvider]
		public static SettingsProvider CreateSpineSettingsProvider() => ins;

		public class ResValue
		{
			public string keyName;
			public string value;
			public bool isChange;
			internal bool isEditor;

			public ResValue Init(string value)
			{
				var aar = value.Split(',');
				this.keyName = aar[0];
				this.value = aar[1];
				return this;
			}

			public string GetResValue()
			{
				return $"resValue \"string\",\"{keyName}\",\"{value}\"";
			}

			public override string ToString()
			{
				return $"{keyName},{value}";
			}
		}
	}

	public partial class HukiryProjectSettingsProvider
	{
		public string sdkHukiryPath, ndkHukiryPath, gradleHukiryVersion;
		public List<string> keepCodeList = new List<string>();
		public string GetProguard => string.Join("\n", keepCodeList);
		public int selectIndex = -1;
		[Tooltip("#IMPLEMENTATION#")]
		public List<string> dependList = new List<string>();
		public int selectIndexDepend = -1;
		private const string unityLibraryJar = "compileOnly files('../unityLibrary/libs/unity-classes.jar')";
		public string GetImplementation { get
			{
				if(dependList.Contains(unityLibraryJar))
					dependList.Remove(unityLibraryJar);
				dependList.Add(unityLibraryJar);
				return string.Join("\n", dependList.ToArray());
			} 
		}

		[Tooltip("#RESVALUE#")]
		private List<ResValue> resValueXML = new List<ResValue>();
		public string GetResValue => string.Join("\n", resValueXML.ConvertAll(p=>p.GetResValue()));

		[Tooltip("#GOOGLESERVICES#")]
		public bool enableGoogleServer;
		//classpath 'com.google.gms:google-services:4.3.15'
		public string googleClasspathVersion = "4.3.15";
		public string googleApplyPlugin = "apply plugin: 'com.google.gms.google-services'";

		[Tooltip("#DEBUGSYMBOL#")]
		public bool enableDebugSymbol;
		public string apkSymbol = "ndk{\n\t\t\t\t debugSymbolLevel 'FULL' \n\t\t\t}";

		public bool enableMinify;
		public string apkMinify = "minifyEnabled true";

		[Tooltip("#MULTIDEXENABLED#")]
		public bool enableMultiDex;
		public string apkMultiDex = "multiDexEnabled true";

		public string GetNdkVersion()
		{
			string ndkPath = $"{ndkHukiryPath}/source.properties";
			string[] lines = File.ReadAllLines(ndkPath);
			foreach (var item in lines)
			{
				if (item.StartsWith("Pkg.Revision"))
				{
					return $"ndkVersion '{item.Split('=')[1]}'";
				}
			}
			return string.Empty;
		}

		private void HukiryAndroidSettings()
		{
			int androidInt = 0, iosInt = 0;
			using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
			{
				var (isAndroid, a1) = this.SelectPage("d_BuildSettings.Android", "配置Android", 0);
				
				var (isIOS, a2) = this.SelectPage("d_BuildSettings.iPhone", "配置iOS", 0);
                //var (isw, a3) = this.SelectPage("d_BuildSettings.Standalone", "配置Window", 1);
                androidInt = a1; iosInt = a2;
				if (isIOS || isAndroid)
				{
					if (isAndroid)
					{
						PlayerPrefs.SetInt("配置iOS", 0);
					}
					else
					{
						PlayerPrefs.SetInt("配置Android", 0);
					}

					AssetDatabase.Refresh();
				}

				Hukiry.HukiryUtilEditor.LocationMonoScript(nameof(HukiryProjectSettingsProvider));
			}

			if (androidInt == 1)
			{
				this.DrawAndroid();
			}
			else if (iosInt == 1)
			{
				this.DrawIOS();
			}
			else
			{
				this.DrawAndroid();
			}
		}


		private void DrawIOS()
		{
			var value = HorizontalLayout("App Public Key", SdkConfiguration.AppPublicKey, null, false,100);
			if (value != SdkConfiguration.AppPublicKey)
			{
				EditorPrefs.SetString(nameof(SdkConfiguration.AppPublicKey), SdkConfiguration.AppPublicKey = value);

				string resDir = "Assets/Resources/";
				if (Directory.Exists(resDir)) Directory.CreateDirectory(resDir);
				File.WriteAllText($"{resDir}{nameof(SdkConfiguration.AppPublicKey)}.json", value);
			}
		}

		private void DrawAndroid()
		{
			//=======================路径和版本部分============================================================
			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				var fontStyle = GUI.skin.label.fontStyle;
				GUI.skin.label.fontStyle = FontStyle.Bold;
				GUILayout.Label("Android Folder Path Or Version");
				GUI.skin.label.fontStyle = fontStyle;
				//SDK路径
				this.HorizontalLayout("SDK ", sdkHukiryPath, dir =>
				{
					string filePath = Path.Combine(dir, "tools/source.properties");
					if (File.Exists(filePath))
						if (File.ReadAllText(filePath).Contains("Android SDK"))
							EditorPrefs.SetString(nameof(sdkHukiryPath), dir);
					return false;
				});

				//NDK路径 **NDKVERSION**
				this.HorizontalLayout("NDK ", ndkHukiryPath, dir =>
				{
					string filePath = Path.Combine(dir, "source.properties");
					if (File.Exists(filePath))
						if (File.ReadAllText(filePath).Contains("Android NDK"))
							EditorPrefs.SetString(nameof(ndkHukiryPath), dir);
					return false;
				});


				//Gradle 版本
				string gradleVersion = HorizontalLayout("Gradle", gradleHukiryVersion, null, false);
				if (gradleHukiryVersion != gradleVersion)
				{
					gradleHukiryVersion = gradleVersion;
					EditorPrefs.SetString(nameof(gradleHukiryVersion), gradleHukiryVersion);
				}
			}

			//=======================配置部分============================================================
			selectIndex = this.DrawList("Proguard (不混淆的代码)", keepCodeList, nameof(keepCodeList), selectIndex, "-keep", null,
@"example:
-keep interface com.yiliu.gamelibrary.UnityInterfaceProxy {{ *; }}
-keep class com.yiliu.gamelibrary.* {{ *; }}");

			selectIndexDepend = this.DrawList("Setting Dependencies", dependList, nameof(dependList), selectIndexDepend, "implementation", "compileOnly",
@"example:
implementation 'androidx.multidex:multidex:2.0.1'
compileOnly files('..\unityLibrary\libs\unity-classes.jar')");

			this.DrawResValue("Configuration String.xml", nameof(resValueXML), resValueXML,
@"example:
resValue 'string', 'facebook_app_id', '294899665027169'
resValue 'string', 'fb_login_protocol_scheme', 'fb294899665027169'
resValue 'string', 'google_ads', 'ca-app-pub-2660511620851048~1084547114'");

			EditorGUILayout.Separator();
			//=======================启动部分============================================================
			using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
			{
				enableDebugSymbol = this.DrawToggleLeft("启动 Symbol", enableDebugSymbol, nameof(enableDebugSymbol));
				enableMinify = this.DrawToggleLeft("启动混淆 Minify", enableMinify, nameof(enableMinify));
				enableMultiDex = this.DrawToggleLeft("启动多个Dex优化", enableMultiDex, nameof(enableMultiDex));
			}

			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				enableGoogleServer = EditorGUILayout.ToggleLeft("启动 Google Server", enableGoogleServer);
				if (enableGoogleServer)
				{
					googleClasspathVersion = this.DrawText("	Classpath Version ", googleClasspathVersion, nameof(googleClasspathVersion));
				}
			}
		}
		private int DrawList(string label, List<string> valueList, string key, int index, string preValue, string preValue2 = null, string helpInfo = "")
		{
			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				GUI.color = Color.green;
				if (GUILayout.Button(label))
				{
					index = -1;
					PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0) == 1 ? 0 : 1);
				}
				GUI.color = Color.white;

				if (PlayerPrefs.GetInt(key, 0) == 1)
				{
					int count = valueList.Count;
					for (int i = 0; i < valueList.Count; i++)
					{
						GUI.color = index == i ? Color.white * 1.2F : Color.white * 0.8F;
						using (new EditorGUILayout.HorizontalScope())
						{

							int k = i;
							var tule = ButtonLayout(i, new GUIContent("☲"))();
							if (tule.isOk)
							{
								index = tule.index;
							}

							string reslut = EditorGUILayout.TextField(valueList[i]);
							bool isOk = preValue2 != null && reslut.StartsWith(preValue2);
							if ((reslut.StartsWith(preValue) || isOk) && valueList[i] != reslut)
							{
								valueList[i] = reslut;
								EditorPrefs.SetString(key, string.Join("|", valueList.ToArray()));
							}

							if (i > 0)
							{	
								var move = ButtonLayout(i, new GUIContent("^","^移动"))();
								if (move.isOk)
								{
									var temp = valueList[move.index];
									valueList[move.index] = valueList[move.index - 1];
									valueList[move.index - 1] = temp;
									break;
								}
							}
						}
						GUI.color = Color.white;
					}

					using (new EditorGUILayout.HorizontalScope())
					{
						GUILayout.FlexibleSpace();
						if (index >= 0)
						{
							if (GUILayout.Button("-", GUI.skin.verticalScrollbar, GUILayout.Width(20)))
							{
								valueList.RemoveAt(index);
								index = valueList.Count - 1;
								EditorPrefs.SetString(key, string.Join("|", valueList.ToArray()));
							}
						}
						if (GUILayout.Button("+", GUI.skin.verticalScrollbar, GUILayout.Width(20)))
						{
							PlayerPrefs.SetInt(key, 1);
							valueList.Add("");
						}
					}

					using (new EditorGUILayout.HorizontalScope())
					{
						var align = GUI.skin.box.alignment;
						GUI.skin.box.alignment = TextAnchor.MiddleLeft;
						EditorGUILayout.HelpBox(helpInfo, MessageType.None);
						if (GUILayout.Button("Copy", GUILayout.Height(40), GUILayout.Width(40))) GUIUtility.systemCopyBuffer = helpInfo;
						GUI.skin.box.alignment = align;
					}
				}
			}
			return index;
		}

		private (bool, int) SelectPage(string platformIcon,string label,int defaultValue)
		{
			int page1 = PlayerPrefs.GetInt(label, defaultValue);
			bool isPress = false;
			var align = GUI.skin.label.alignment;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			var guiContent = new GUIContent(EditorGUIUtility.Load(platformIcon) as Texture2D, label);
			if (GUILayout.Button(guiContent, page1 == 0? "flow node hex 0" : "flow node hex 0 on"))
			{
				isPress = true;
				page1 = 1;
				PlayerPrefs.SetInt(label, page1);
			}
			GUI.skin.label.alignment = align;
			return (isPress, page1);
		}

		private void DrawResValue(string label, string key, List<ResValue> valueList, string helpInfo="")
		{
			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				GUI.color = Color.green;
				if (GUILayout.Button(label))
				{
					PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0) == 1 ? 0 : 1);
				}
				GUI.color = Color.white;

				if (PlayerPrefs.GetInt(key, 0) == 1) 
				{
					int count = valueList.Count;
					for (int i = 0; i < valueList.Count; i++)
					{

						using (new EditorGUILayout.HorizontalScope())
						{
							if (valueList[i].isEditor)
							{
								var reslut1 = EditorGUILayout.TextField(valueList[i].keyName);
								var reslut2 = EditorGUILayout.TextField(valueList[i].value);
								if (!valueList[i].isChange)
								{
									valueList[i].isChange = reslut1 != valueList[i].keyName || reslut2 != valueList[i].value;
								}
								else
								{
									valueList[i].keyName = reslut1;
									valueList[i].value = reslut2;
								}
							}
							else
							{
								EditorGUILayout.LabelField(valueList[i].keyName, GUILayout.Width(200));
								EditorGUILayout.TextField(valueList[i].value);
							}

							if (valueList[i].isChange)
							{
								
								var save = ButtonLayout(i, new GUIContent("save", "保存"), 50)();
								if (save.isOk)
								{
									valueList[i].isEditor = false;
									valueList[i].isChange = false;
									EditorPrefs.SetString(key, string.Join("|", valueList));
								}
							}
							else if (i > 0)
							{
								var move = ButtonLayout(i, new GUIContent("^", "^移动"))();
								if (move.isOk)
								{
									var temp = valueList[move.index];
									valueList[move.index] = valueList[move.index - 1];
									valueList[move.index - 1] = temp;
									break;
								}
							}

							var delte = ButtonLayout(i, new GUIContent("x", "刪除"))();
							if (delte.isOk)
							{
								valueList.RemoveAt(delte.index);
								EditorPrefs.SetString(key, string.Join("|", valueList));
								break;
							}
						}
					}

					using (new EditorGUILayout.HorizontalScope())
					{
						GUILayout.FlexibleSpace();

						if (GUILayout.Button("+", GUI.skin.verticalScrollbar, GUILayout.Width(20)))
						{
							PlayerPrefs.SetInt(key, 1);
							valueList.Add(new ResValue() { isEditor=true});
						}
					}
					using (new EditorGUILayout.HorizontalScope())
					{
						var align = GUI.skin.box.alignment;
						GUI.skin.box.alignment = TextAnchor.MiddleLeft;
						EditorGUILayout.HelpBox(helpInfo, MessageType.None);
						if (GUILayout.Button("Copy",GUILayout.Height(50), GUILayout.Width(40))) GUIUtility.systemCopyBuffer = helpInfo;
						GUI.skin.box.alignment = align;
					}
				}
			}
		}
		private Func<(int index, bool isOk)> ButtonLayout(int index, GUIContent label, int width=30)
		{
			return new Func<(int, bool)>(() => (index, GUILayout.Button(label, GUILayout.Width(width))));
		}
		private string DrawText(string label, string value, string key)
		{
			string reslut = EditorGUILayout.TextField(label, value);
			if (reslut != value)
			{
				value = reslut;
				EditorPrefs.SetString(key, value);
			}
			return value;
		}
		private bool DrawToggleLeft(string label, bool value, string key)
		{
			bool reslut = EditorGUILayout.ToggleLeft(label, value, GUILayout.Width(150));
			if (reslut != value)
			{
				value = reslut;
				EditorPrefs.SetBool(key, value);
			}
			return value;
		}
		private string HorizontalLayout(string label, string value, Func<string, bool> complete, bool isHasButton = true, int width = 50)
		{
			EditorGUILayout.Separator();
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label(label, GUILayout.Width(width));
				value = EditorGUILayout.TextField(value);
				if (isHasButton)
				{
					if (GUILayout.Button("Browse"))
					{
						string folderPath = EditorUtility.OpenFolderPanel($"Open {label} Path",
							!string.IsNullOrWhiteSpace(value) ? value : System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "");
						if (complete(folderPath))
						{
							value = folderPath;
						}
					}
				}
				return value;
			}
		}
		private void InitData()
		{
			this.enableGoogleServer = EditorPrefs.GetBool(nameof(this.enableGoogleServer), this.enableGoogleServer);
			this.enableDebugSymbol = EditorPrefs.GetBool(nameof(this.enableDebugSymbol), this.enableDebugSymbol);
			this.enableMinify = EditorPrefs.GetBool(nameof(this.enableMinify), this.enableMinify);
			this.enableMultiDex = EditorPrefs.GetBool(nameof(this.enableMultiDex), this.enableMultiDex);

			this.googleClasspathVersion = EditorPrefs.GetString(nameof(this.googleClasspathVersion), this.googleClasspathVersion);
			this.sdkHukiryPath = EditorPrefs.GetString(nameof(this.sdkHukiryPath), this.sdkHukiryPath);
			this.ndkHukiryPath = EditorPrefs.GetString(nameof(this.ndkHukiryPath), this.ndkHukiryPath);
			this.gradleHukiryVersion = EditorPrefs.GetString(nameof(this.gradleHukiryVersion), this.gradleHukiryVersion);

			keepCodeList = InitList(keepCodeList, nameof(keepCodeList));

			dependList = InitList(dependList, nameof(dependList));

			SdkConfiguration.AppPublicKey = EditorPrefs.GetString(nameof(SdkConfiguration.AppPublicKey), SdkConfiguration.AppPublicKey);


			if (resValueXML.Count <= 0)
			{
				string keepStr = EditorPrefs.GetString(nameof(resValueXML), "");
				if (!string.IsNullOrWhiteSpace(keepStr))
				{
					resValueXML = keepStr.Split('|')?.Where(p=>!string.IsNullOrEmpty(p)).ToList().ConvertAll(p=> new ResValue().Init(p));
				}
			}
		}

		private List<string> InitList(List<string> tempList, string key)
		{
			if (tempList.Count <= 0)
			{
				string keepStr = EditorPrefs.GetString(key, "");
				if (!string.IsNullOrWhiteSpace(keepStr))
				{
					tempList = keepStr.Split('|')?.ToList();
				}
			}
			return tempList;
		}
	}
}

//firebase事件上报
//implementation 'com.google.firebase:firebase-analytics'
//implementation platform('com.google.firebase:firebase-bom:32.4.0')
//支付
//implementation 'com.android.billingclient:billing:5.1.0'
//Google登录
//implementation 'com.google.android.gms:play-services-auth:20.6.0'
//Facebook
//implementation 'com.facebook.android:facebook-android-sdk:7.1.0'
//google评价功能
//implementation 'com.google.android.play:core:1.10.3'
//分包混编
//implementation 'androidx.multidex:multidex:2.0.1'
//kotlin
//implementation 'androidx.appcompat:appcompat:1.6.1'
//广告部分
// implementation 'com.applovin:applovin-sdk:9.13.4' 
// implementation 'com.facebook.android:audience-network-sdk:5.11.0' 
// implementation 'com.google.android.gms:play-services-ads:19.3.0'
// implementation 'com.google.android.gms:play-services-ads-identifier:17.0.0' 
// implementation 'com.google.android.gms:play-services-basement:17.2.1' 
// implementation 'com.ironsource.adapters:admobadapter:4.3.13' 
// implementation 'com.ironsource.adapters:applovinadapter:4.3.16' 
// implementation 'com.ironsource.adapters:facebookadapter:4.3.19' 
// implementation 'com.ironsource.adapters:unityadsadapter:4.3.6' 
// implementation 'com.ironsource.sdk:mediationsdk:7.0.1.1' 
// implementation 'com.unity3d.ads:unity-ads:3.4.8'"


/*
apply plugin: 'com.android.application'
# GOOGLESERVICES#

dependencies {
    implementation project(':unityLibrary')
# IMPLEMENTATION#
}

android {
    compileSdkVersion **APIVERSION**
    buildToolsVersion '**BUILDTOOLS**'
# NDKVERSION#

    compileOptions {
        sourceCompatibility JavaVersion.VERSION_1_8
        targetCompatibility JavaVersion.VERSION_1_8
    }

    defaultConfig {
        minSdkVersion **MINSDKVERSION**
        targetSdkVersion **TARGETSDKVERSION**
        applicationId '**APPLICATIONID**'
        ndk {
            abiFilters **ABIFILTERS**
        }
        versionCode **VERSIONCODE**
        versionName '**VERSIONNAME**'
# MULTIDEXENABLED#
    }

    aaptOptions {
        noCompress = ['.unity3d', '.ress', '.resource', '.obb'**STREAMING_ASSETS**]
        ignoreAssetsPattern = "!.svn:!.git:!.ds_store:!*.scc:.*:!CVS:!thumbs.db:!picasa.ini:!*~"
    }**SIGN**

    lintOptions {
        abortOnError false
        checkReleaseBuilds false
    }

    buildTypes {
        debug {
            minifyEnabled **MINIFY_DEBUG**
            proguardFiles getDefaultProguardFile('proguard-android.txt')**SIGNCONFIG**
            jniDebuggable true
            // 修改 strings.xml 文件中的值
            // resValue "string", "facebook_app_id", "294899665027169"
            // resValue "string", "fb_login_protocol_scheme", "fb294899665027169"
            // resValue "string", "google_ads", "ca-app-pub-2660511620851048~1084547114"
        }
        release {
# DEBUGSYMBOL#
            minifyEnabled **MINIFY_DEBUG**
            zipAlignEnabled true
            proguardFiles getDefaultProguardFile('proguard-android.txt')**SIGNCONFIG**
            // 修改 strings.xml 文件中的值
            // resValue "string", "facebook_app_id", "294899665027169"
            // resValue "string", "fb_login_protocol_scheme", "fb294899665027169"
            // resValue "string", "google_ads", "ca-app-pub-2660511620851048~1084547114"
        }
    }**PACKAGING_OPTIONS****PLAY_ASSET_PACKS****SPLITS**
**BUILT_APK_LOCATION**
    bundle {
        language {
            enableSplit = false
        }
        density {
            enableSplit = false
        }
        abi {
            enableSplit = true
        }
    }
}**SPLITS_VERSION_CODE****LAUNCHER_SOURCE_BUILD_SETUP**
 */

// 修改 strings.xml 文件中的值
// resValue "string", "facebook_app_id", "294899665027169"
// resValue "string", "fb_login_protocol_scheme", "fb294899665027169"
// resValue "string", "google_ads", "ca-app-pub-2660511620851048~1084547114"