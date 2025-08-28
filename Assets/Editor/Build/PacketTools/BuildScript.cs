using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hukiry;
using Hukiry.Pack;

/**
 * 打包脚本
 * @author Hukiry
 */
namespace Hukiry.Pack
{
	public class BuildScript {
		private static string _outPackageDirPath;

		/// <summary>
		/// 出包目录路径，打包打开指定的路径
		/// </summary>
		public static string outPackageDirPath
		{
			get
			{
#if UNITY_ANDROID
				_outPackageDirPath = "Android";
#elif UNITY_IOS || UNITY_IPHONE
				_outPackageDirPath = "IOS";
#else
				_outPackageDirPath = "Window";
#endif
				if (!Directory.Exists(_outPackageDirPath)) Directory.CreateDirectory(_outPackageDirPath);
				return _outPackageDirPath;
			}
		}

		public static string GetOutFileName(PackageType packageType, string versionsInfo = null, string WorkMode = "")
		{
			var dateTimePackage = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
			string extension = packageType == PackageType.exportProject ? string.Empty : "." + packageType.ToString();
			string fileName = WorkMode + "_" + versionsInfo + extension;
			return dateTimePackage + "_" + fileName;
		}

		/// <summary>
		/// 游戏打包   只打包
		/// </summary>
		/// <param name="target"></param>
		public static void BuildPlayer(PackageType packageType, VersionsInfo versionsInfo)
		{
			BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

			string outPath = $"{outPackageDirPath}/{GetOutFileName(packageType, versionsInfo.GetVersionName(), PackingWinConfigAssets.Instance.WorkMode)}";
			BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.CompressWithLz4);
			AssetDatabase.SaveAssets();
			Debug.Log($"打包完成:{outPath}");

#if UNITY_ANDROID
			if (packageType == PackageType.exportProject)
			{
				Hukiry.Editor.UnityToAndroidStudioExport.CopyExportUnityProject(outPath, versionsInfo.GetVersionName(), versionsInfo.GetVersionCode());
			}
			else
			{
				Application.OpenURL(outPackageDirPath);
			}
#elif UNITY_IOS || UNITY_IPHONE
            string curPath = "file://" + Path.GetFullPath(outPath);
            Application.OpenURL(curPath);
#endif
        }

		/// <summary>
		/// 删除Lua文件
		/// </summary>
		/// <param name="target"></param>
		public static void BuildDeleteLuaFiles(BuildTarget target) {
			EditorUtility.DisplayProgressBar("准备数据", "删除Lua文件", 0.0f);

			var buildPath = string.Format("{0}/{1}", Application.dataPath, AssetBundleConifg.LuaOutFile);
			if (Directory.Exists(buildPath)) {
				FileHelper.DeleteDirectory(buildPath, true);
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			Debug.Log("删除Lua脚本成功");
		}

		/// <summary>
		/// 复制Lua的文件
		/// </summary>
		/// <param name="target"></param>
		public static void BuildCopyLuaFiles(BuildTarget target) {
			EditorUtility.DisplayProgressBar("拷贝Lua脚本", "拷贝开始", 0.0f);
			var clientPath = Application.dataPath.Replace("Assets", "");
			var localPath = string.Format("{0}LuaScript", clientPath);
			var buildPath = string.Format("{0}/{1}", Application.dataPath, AssetBundleConifg.LuaOutFile);
			int len = AssetBundleConifg.DirLuaNames.Length;
			for (int i = 0; i < len; i++)
			{
				string curLuaDir = buildPath + "/" + AssetBundleConifg.DirLuaNames[i];
				FileHelper.CreateDirectory(curLuaDir);
				FileHelper.CopyDirectoryAndRenameLua(localPath + "/" + AssetBundleConifg.DirLuaNames[i], curLuaDir, ".lua", new string[] { ".svn" }, ".bytes");
				EditorUtility.DisplayProgressBar("拷贝Lua脚本", curLuaDir, i / (float)len);
			}

			//处理调试代码
			string curLuaMain = buildPath + "/Game/Main.bytes";
			string[] lines = File.ReadAllLines(curLuaMain);
			for (int i = 0; i < 3; i++)
			{
				if (!lines[i].StartsWith("--"))
					lines[i] = "--" + lines[i];
			}
			File.WriteAllLines(curLuaMain, lines);

			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
		}

		public const string luaCodeFile = "LuaCode.txt";
		/// <summary>
		/// 打包lua完成后加密文件,将加密的文件拷贝一份到热更目录
		/// </summary>
		public static void LuaKeyCodeByAB(VersionsInfo info)
		{
			string appDataPath = AssetBundleConifg.PacketAbPath;
			AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(appDataPath, AssetBundleConifg.AbFile));
			AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest"); //获取总的AssetBundleManifest
			bundle.Unload(false);
			//收集包里的
			Dictionary<string, Lua_CodeData> localHash = new Dictionary<string, Lua_CodeData>();
			Dictionary<string, Lua_CodeData> temp = new Dictionary<string, Lua_CodeData>();
			string[] assetBundles = manifest.GetAllAssetBundles();
			for (int i = 0; i < assetBundles.Length; i++)
			{
				string abPath = assetBundles[i];
				string hash = manifest.GetAssetBundleHash(abPath).ToString();
				localHash[abPath] = new Lua_CodeData()
				{
					abpath = abPath,
					md5 = hash,
				};
			}

			if (!File.Exists(luaCodeFile))
			{
				temp = localHash;
				string str = JsonUtil.ToJsonUnity(temp.Values.ToList());
				File.WriteAllText(luaCodeFile, str);
			}
			else
			{
				var tempLast = JsonUtil.ToArrayUnity<Lua_CodeData>(File.ReadAllText(luaCodeFile));
				Dictionary<string, Lua_CodeData> last = tempLast?.ToDictionary(P=>P.abpath);
				if (last != null)
				{
					localHash.Values.ToList().ForEach(v =>
					{
						if (last.ContainsKey(v.abpath))
						{
							if (v.md5 != last[v.abpath].md5)
							{
								temp[v.abpath] = v;
							}
						}
						else
						{
							temp[v.abpath] = v;
						}
					});
				}

				string str = JsonUtil.ToJsonUnity(localHash.Values.ToList());
				File.WriteAllText(luaCodeFile, str);
			}

			var buildPath = string.Format("{0}/{1}", AssetBundleConifg.AbFile, AssetBundleConifg.LuaOutFile);
			string filePath = Application.streamingAssetsPath + "/" + buildPath;
			int len = AssetBundleConifg.DirLuaNames.Length;
			for (int i = 0; i < len; i++)
			{
				string abPath = AssetBundleConifg.LuaOutFile + "/" + AssetBundleConifg.DirLuaNames[i].ToLower() + AssetBundleConifg.AbSuffix;
				if (temp.ContainsKey(abPath))
				{
					string curLuaDir = filePath + "/" + AssetBundleConifg.DirLuaNames[i].ToLower() + AssetBundleConifg.AbSuffix;
					var buffer = File.ReadAllBytes(curLuaDir);
					buffer = Hukiry.HukiryUtil.CodeByte(buffer);
					File.WriteAllBytes(curLuaDir, buffer);

					string destFileName = Path.Combine(BuildPackConfig.HotUpdateAbFolder, AssetBundleConifg.LuaOutFile + "/" + AssetBundleConifg.DirLuaNames[i].ToLower() + AssetBundleConifg.AbSuffix);
					FileHelper.CopyFile(curLuaDir, destFileName, true);
				}
			}
			LogManager.LogColor("yellow", "写入Lua加密完成");
			AssetDatabase.Refresh();

			////////////////////////////////////////拷贝一份//////////////////////////////////////////////////////
			FileHelper.CopyDirectory(BuildPackConfig.HotUpdateAbFolder, BuildPackConfig.HotUpdatePath + "v" + info.GetLargeVersions());
		}

		public static string[] GetLevelsFromBuildSettings() {
			List<string> levels = new List<string>();
			for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i) {
				if (EditorBuildSettings.scenes[i].enabled)
					levels.Add(EditorBuildSettings.scenes[i].path);
			}

			return levels.ToArray();
		}
		[Serializable]
		public struct Lua_CodeData
		{
			public string abpath;
			public string md5;
		}
	}
}