using System.Collections.Generic;

namespace Hukiry
{
	/// <summary>
	/// AssetBundle管理器，负责资源的加载路径和更新资源索引
	/// </summary>
	public class AssetBundleList
	{
		public static AssetBundleList ins { get; } = new AssetBundleList();
		// 本地ab文件清单
		private Dictionary<string, HotAssetInfo> manifestDatas = new Dictionary<string, HotAssetInfo>();

		// 查找AssetBundle，如果找到就会存在这里，减少File.IO.Exists的调用
		static Dictionary<string, string> s_AssetBundleNameCache = new Dictionary<string, string>();

		/// <summary>
		/// 游戏初始化时，初始化ManifestData数据
		/// </summary>
		public void RecoverManifestDataFromLocal(string text)
		{
			List<HotAssetInfo> temp = JsonUtil.ToObject<List<HotAssetInfo>>(text);
			Log("初始化 'ManifestData.ab' 成功");
			temp.ForEach(abData => manifestDatas[abData.ab] = abData);
		}

		/// <summary>
		/// 根据引用路径获取AssetBundleData的数据：打包之后，ab路径加载
		/// </summary>
		public HotAssetInfo GetAssetBundleDataAtPath(string abName)
		{
			if (string.IsNullOrEmpty(abName))
				return null;

			// todo other

			if (manifestDatas.ContainsKey(abName))
			{
				return manifestDatas[abName];
			}
			return null;
		}

		/// <summary>
		/// 转换路径
		/// </summary>
		/// <param name="assetName"></param>
		/// <returns></returns>

		public static string SwitchingPath(string assetName)
		{
			string temp = AssetBundleConifg.Prefabs + assetName;
#if !UNITY_EDITOR || ASSETBUNDLE_TEST
            temp = GetAssetBundleName(temp);
#endif
			return temp;
		}


		/// <summary>
		/// 根据加载的名字，获取assetBundle名字
		/// </summary>
		/// <param name="sabname"></param>
		/// <returns></returns>
		private static string GetAssetBundleName(string sabname)
		{
			string fullpath = null;
			s_AssetBundleNameCache.TryGetValue(sabname, out fullpath);
			if (fullpath == null)
			{
				string abName = sabname;

				fullpath = abName + AssetBundleConifg.AbSuffix;
				s_AssetBundleNameCache.Add(sabname, fullpath);
				return fullpath;
			}
			return fullpath;
		}

		/// <summary>
		/// 根据引用路径判断是否存在该数据
		/// </summary>
		public bool Contains(string path)
		{
			if (string.IsNullOrEmpty(path))
				return false;
			return manifestDatas.ContainsKey(path);
		}

		public void Log(params object[] param)
		{
			UnityEngine.LogManager.Log(param);
		}

		public void LogError(params object[] param)
		{
			UnityEngine.LogManager.LogError(param);
		}

		public void LogColor(string color, params object[] param)
		{
			UnityEngine.LogManager.LogColor(color,param);
		}

		public void LogException(System.Exception e)
		{
			UnityEngine.LogManager.LogException(e);
		}

		public static void AddTimer<T, U>(float delay, float interval, System.Action<T, U> callback, T arg1, U arg2)
		{
			TimerUnit.AddTimer(delay, interval, callback, arg1, arg2);
		}
	}


	[System.Serializable]
	public class HotAssetInfo
	{
		public string ab;
		public string md5;
		public long size;
		public List<string> ds;//当前文件的依赖文件

		private bool isWebRequest = false;//是否边下载边加载
		public void SetWebRequest(bool isRemote)=> this.isWebRequest = isRemote;
		public bool IsWebRequest() => isWebRequest;
		public string GetAbFullPath() => ab;
		/// <summary>
		/// 优先加载缓存包： AssetBundle ： 路径前不需要加 "file：//"
		/// </summary>
		public string GetPath()
		{
			//新包时，已经删除缓存
			string cachePath = AssetBundleConifg.AppCachePath + ab;//缓存路径
			string packetPath = AssetBundleConifg.PacketAbPath + ab;//包内路径
#if ASSETBUNDLE_TEST//编辑模式 ab测试
            return packetPath;
#elif !UNITY_EDITOR
            return File.Exists(cachePath)?cachePath:packetPath;
#else//编辑模式
			return cachePath;
#endif
		}
	}
}

