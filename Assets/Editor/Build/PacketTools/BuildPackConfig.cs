using UnityEngine;
namespace Hukiry.Pack
{
    public class BuildPackConfig
    {
		/// <summary>
		/// 父目录：存储热更AB文件
		/// </summary>
		public static string HotUpdatePath => Application.dataPath + "/../HotUpdateFile/" + AssetBundleConifg.buildTarget + "/" + HotUpdateWorkModeFolder;
		/// <summary>
		/// 存储热更AB文件
		/// </summary>
		public static string HotUpdateAbFolder => Application.dataPath + "/../HotUpdateFile/" + AssetBundleConifg.buildTarget + "/" + HotUpdateWorkModeFolder + AssetBundleConifg.AbFile + "_Update/";
		/// <summary>
		/// 热更子文件夹
		/// </summary>
		public static string HotUpdateWorkModeFolder = "";

		public static readonly UnityEditor.BuildTargetGroup buildTargetGroup =
#if UNITY_ANDROID
		UnityEditor.BuildTargetGroup.Android;
#elif UNITY_IPHONE || UNITY_STANDALONE_OSX
		UnityEditor.BuildTargetGroup.iOS;
#elif UNITY_STANDALONE_WIN
		UnityEditor.BuildTargetGroup.Standalone;
#endif

		/// <summary>
		/// 上次记录的所有文件版本比对文件，用于热更新时比对需要更新的资源
		/// </summary>
		public const string LastFileList = "Z_LastFileList.json";

		public static string LuaOutFile =>AssetBundleConifg.LuaOutFile;
	}
}
