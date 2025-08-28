using UnityEngine;
namespace Hukiry
{
	public class AssetBundleConifg
	{
		public const string TempDownload = "tempDownload.txt";
		/// <summary>
		/// 临时缓存
		/// </summary>
		public static string AppTempCachePath => AppCachePath + "/tempFile/";

		/// <summary>
		/// 版本号文件名
		/// </summary>
		public const string VersionName = "version.json";
		/// <summary>
		/// 包内资源路径
		/// </summary>
		public static string AppPacketPath=> Application.streamingAssetsPath + "/";

		//////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// LuaAB文件名
		/// </summary>
		public const string LuaOutFile = "luascript";

		public static readonly string[] DirLuaNames = { "ToLua", "Game", "LuaConfig", "Network","Library" };

		/// <summary>
		/// 上次记录的所有文件hash码 :用于下次比较是否需要热更
		/// </summary>
		public const string HashmapFileName = "Hashmap.json";
		/// <summary>
		/// 正式服资源: 配置小写
		/// </summary>
		public const string Prefabs = "resourcesex/";
		/// <summary>
		/// 导出的assetBundle文件名
		/// </summary>
		public const string AbFile = "resfile";
		/// <summary>
		/// ab文件后缀名
		/// </summary>
		public const string AbSuffix = ".ab";

		/// <summary>
		/// 资源路径：获取缓存-可读可写
		/// </summary>
		public static string CacheAbPath => AppCachePath + AbFile + "/";
		/// <summary>
		///  包内资源路径：获取包路径-只读
		/// </summary>
		public static string PacketAbPath => Application.streamingAssetsPath + "/" + AbFile + "/";

		/// <summary>
		/// 缓存父目录路径:上次更新的缓存文件
		/// </summary>
		/// <returns></returns>
		public static string AppCachePath
		{
			get
			{
#if ASSETBUNDLE_TEST
			return Application.streamingAssetsPath + "/";
#elif UNITY_EDITOR
				return "Cache";
#else
			return AppPersistentDataPath + "Cache/";
#endif
			}
		}
		/// <summary>
		/// UnityWebRequest文件前缀：包里使用
		/// </summary>
		public static readonly string WWWPrefix =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		@"";
#elif UNITY_ANDROID
		"";
#elif UNITY_IPHONE
		"file://";
#endif
		/// <summary>
		/// 本地缓存目录：可读可写
		/// </summary>
		public static readonly string AppPersistentDataPath =
#if UNITY_EDITOR
		System.Environment.CurrentDirectory + "/";
#elif UNITY_IPHONE || UNITY_STANDALONE_OSX
		Application.persistentDataPath + "/";
#elif UNITY_ANDROID
		Application.persistentDataPath + "/";
#else
		Application.persistentDataPath + "/";
#endif

		public static string buildTarget
		{
			get
			{
#if UNITY_ANDROID
		return "Android";
#elif UNITY_IPHONE || UNITY_STANDALONE_OSX
		return "iOS";
#elif UNITY_STANDALONE_WIN
				return "Windows";
#endif
			}
		}
	}
}