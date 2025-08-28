using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.IO;
using System;

public class BuildPipelineAB : Editor {


    /// <summary>
    /// assets文件夹下的目录
    /// </summary>
    
	//[MenuItem("Hukiry/AssetBundle/Set AssetBundle Assets Name", false ,-1)]
    static void SetAndroidAssetsName()
    {
      
        BuildTarget target = BuildTarget.Android;
#if UNITY_ANDROID
        target = BuildTarget.Android;
#elif UNITY_IOS
         target=BuildTarget.iOS;
#else
         target=BuildTarget.StandaloneWindows;
#endif
        string outPath = Application.dataPath.Replace("Assets", target.ToString());
        

    }

    public static void BuildAndroidAssets()
    {
       
        BuildTarget target = BuildTarget.Android;
        
#if UNITY_ANDROID
        target=BuildTarget.Android;
#elif UNITY_IOS
         target=BuildTarget.iOS;
#else
         target=BuildTarget.StandaloneWindows;
#endif
		string outPath = Application.dataPath + "/../OutUpdate/" + target.ToString();
		if (!Directory.Exists(outPath))
		{
			Directory.CreateDirectory(outPath);
			AssetDatabase.Refresh();
		}

		BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression, target);

		Application.OpenURL(outPath);
	}


	public static void BuildAndroidAssetsTestHotUpate()
    {

		BuildTarget target = BuildTarget.Android;

#if UNITY_ANDROID
		        target = BuildTarget.Android;
#elif UNITY_IOS
		         target=BuildTarget.iOS;
#else
		target = BuildTarget.StandaloneWindows;
#endif
		string outPath = Application.dataPath + "/../OutUpdate/" + target.ToString();
		if (!Directory.Exists(outPath))
		{
			Directory.CreateDirectory(outPath);
			AssetDatabase.Refresh();
		}

		BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle, target);

		Application.OpenURL(outPath);
	}

	private static void CollectDirectoryFile(List<string> pathList, string originDir)
	{
		string[] dir = Directory.GetDirectories(originDir);
		string[] file = Directory.GetFiles(originDir);
		if (file != null || file.Length > 0)
		{
			foreach (var item in file)
			{
				if (Path.GetExtension(item).ToLower() != ".meta")
				{
					pathList.Add(item.Replace('\\', '/').Replace(Application.dataPath, "Assets"));
				}
			}
		}

		if (dir != null || dir.Length > 0)
		{
			foreach (var item in dir)
			{
				CollectDirectoryFile(pathList, item);
			}
		}
	}
}

