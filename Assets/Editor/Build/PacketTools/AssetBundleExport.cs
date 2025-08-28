using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Hukiry.Pack
{
    /// <summary>
    /// 清楚ab名，根据配置打包AB文件
    /// </summary>
    public class AssetBundleExport {

		/// <summary>
		/// 记录资源处理热更资源:生成文件清单
		/// </summary>
		public static void GeneralBetaRecordData(bool isNewBuild)
		{
			//DeleteDir(PathConifg.HotUpdateAbFolder);
			//复制所有需要分包的资源到hotUpdate
			string appDataPath = AssetBundleConifg.PacketAbPath;
			AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(appDataPath, AssetBundleConifg.AbFile));
			AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest"); //获取总的AssetBundleManifest
			bundle.Unload(false);
			//收集包里的
			Dictionary<string, HotAssetInfo> localHash = new Dictionary<string, HotAssetInfo>();
			string[] assetBundles = manifest.GetAllAssetBundles();
			for (int i = 0; i < assetBundles.Length; i++)
			{
				string abPath = assetBundles[i];
				string hash = manifest.GetAssetBundleHash(abPath).ToString();
				localHash[hash] = new HotAssetInfo()
				{
					ab = abPath,
					md5 = hash,
					size = new FileInfo(appDataPath + abPath).Length
				};
				string[] ds = manifest.GetDirectDependencies(abPath);

				localHash[hash].ds = new List<string>(ds);
			}

			//改变的
			List<HotAssetInfo> hotUpdateList = new List<HotAssetInfo>();

			string lastAsetPath = Path.Combine(BuildPackConfig.HotUpdatePath, BuildPackConfig.LastFileList);
			string hashmapHotAsetPath = Path.Combine(BuildPackConfig.HotUpdateAbFolder, AssetBundleConifg.HashmapFileName);//热更的清单
			string hashmapPackAsetPath = Path.Combine(AssetBundleConifg.PacketAbPath, AssetBundleConifg.HashmapFileName);//包里的清单
			FileHelper.CreateDirectory(BuildPackConfig.HotUpdateAbFolder);
			FileHelper.CreateDirectory(AssetBundleConifg.PacketAbPath);


			//记录更新资源：上次文件存在，并且不是新apk打包
			if (File.Exists(lastAsetPath) && isNewBuild == false)
			{
				string strs = File.ReadAllText(lastAsetPath);
				Dictionary<string, HotAssetInfo> lastListHash = JsonUtil.ToObject<List<HotAssetInfo>>(strs).ToDictionary(p => p.md5);
				//比较不同 记录起来
				foreach (var item in localHash.Values)
				{
					if (!lastListHash.ContainsKey(item.md5))
					{
						hotUpdateList.Add(item);
					}
				}
			}
			else
			{
				hotUpdateList = localHash.Values.ToList();
			}

			//移动热更文件
			for (int i = 0; i < hotUpdateList.Count; i++)
			{
				string sourceFileName = Path.Combine(AssetBundleConifg.PacketAbPath, hotUpdateList[i].ab);
				string destFileName = Path.Combine(BuildPackConfig.HotUpdateAbFolder, hotUpdateList[i].ab);
				FileHelper.CopyFile(sourceFileName, destFileName, true);
			}

			FileHelper.WriteFile(Path.Combine(BuildPackConfig.HotUpdatePath, "Z_resfile_Update.txt"), JsonUtil.ToJson(hotUpdateList));
			//写入清单文件
			FileHelper.WriteFile(hashmapHotAsetPath, JsonUtil.ToJson(localHash.Values.ToList()));
			FileHelper.WriteFile(hashmapPackAsetPath, JsonUtil.ToJson(localHash.Values.ToList()));

			//保存最新的
			FileHelper.WriteFile(lastAsetPath, JsonUtil.ToJson(localHash.Values.ToList()));
			Debug.Log("需要更新的数量：" + hotUpdateList.Count);
		}

		/// <summary>  
		/// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包  
		/// 只要设置了AssetBundleName的，都会进行打包，不论在什么目录下  
		/// </summary>  
		public static void ClearAssetBundlesName() {
			string[] oldAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
			for (int j = 0; j < oldAssetBundleNames.Length; j++) {
				try {
					AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
				} catch {

				}
			}
			AssetDatabase.Refresh();
			Debug.Log("清除AssetBundleName成功");
		}

		/// <summary>
		/// 清除所有资源文件
		/// </summary>
		public static void ClearAllAssetBundles() {
			FileHelper.DeleteDirectoryInExtends(AssetBundleConifg.PacketAbPath, new string[2] { $"*{AssetBundleConifg.AbSuffix}", "*.txt" });
			FileHelper.DeleteDirectory(AssetBundleConifg.PacketAbPath, true);
			FileHelper.DeleteFile(BuildScript.luaCodeFile);

			AssetDatabase.Refresh();
			Debug.Log("清除StreamingAssets成功");
		}
		/// <summary>
		/// 获取某个目录下所有符合扩展名的文件
		/// </summary>
		/// <param name="path">要查找的目录</param>
		/// <param name="extend">要查找的文件扩展名</param>
		static FileInfo[] FindFiles(string path, string extend) {
			DirectoryInfo dic = new DirectoryInfo(path);
			return dic.GetFiles(extend, SearchOption.AllDirectories);
		}

		/// <summary>
		/// 删除某个目录下所有符合扩展名的文件
		/// </summary>
		/// <param name="path">要查找的目录</param>
		/// <param name="extend">要查找的文件扩展名</param>
		static void DeleteFiles(string path, string extend) {
			DirectoryInfo dic = new DirectoryInfo(path);
			FileInfo[] files = dic.GetFiles("*" + extend, SearchOption.AllDirectories);
			files.ToList().ForEach(fileInfo => File.Delete(fileInfo.FullName));
		}

		static void DeleteDir(string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo dic = new DirectoryInfo(path);
				DirectoryInfo[] files = dic.GetDirectories();
				files.ToList().ForEach(fileInfo => Directory.Delete(fileInfo.FullName, true));
			}
		}
	}
}
