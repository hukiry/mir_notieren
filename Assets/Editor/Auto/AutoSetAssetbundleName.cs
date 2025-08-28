/************************** 
     * 文件名:AutoSetAssetbundleName.cs; 
     * 文件描述:自动设置Assetbundle名字为文件夹名_文件名.unity3d; 
     * 创建日期:2016/06/26; 
     * Author:胡雄坤; 
     ***************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Hukiry;
using Hukiry.Pack;
/// <summary>
/// 图片不打ab名
/// 预制件 按文件夹打ab名
/// 其他，单独文件打ab名
/// </summary>
public class AutoSetAssetbundleName : AssetPostprocessor
{
    const string resPath = "Assets/ResourcesEx/";
    const string folderNames = "1prefab";
    const string filterDirNames = "AtlasImages";
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var str in importedAssets)                             //导入
        {
            if (str.StartsWith("Assets/StreamingAssets") || str.EndsWith(".cs"))
            {
                continue;
            }
            SetAbName(str);
        }

        foreach (var str in movedAssets)                                   //移动
        {
            if (str.StartsWith("Assets/StreamingAssets") || str.EndsWith(".cs"))
            {
                continue;
            }
            SetAbName(str);
        }

        foreach (var str in deletedAssets)
        {
            if (str.StartsWith("Assets/StreamingAssets") || str.EndsWith(".cs"))
            {
                continue;
            }
            LogManager.LogWarning("Delete Asset: ", str);
        }
    }

    public static void MarkALLAbFile(string[] filePaths)
    {
        int len = filePaths.Length;
        for (int i = 0; i < len; i++)
        {
            SetAbName(filePaths[i].Replace('\\', '/').Replace(Application.dataPath, "Assets"));
            if (i % 5 == 0)
                EditorUtility.DisplayProgressBar("设置ab名", Path.GetFileName(filePaths[i]), i / (float)len);
        }
        
        EditorUtility.ClearProgressBar();
    }

    public static void ClearAllAb(string[] filePaths)
    {
        string[] oldAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            try
            {
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
                if (j % 5 == 0)
                    EditorUtility.DisplayProgressBar("清除ab名", oldAssetBundleNames[j], j / (float)oldAssetBundleNames.Length);
            }
            catch{}
        }
        EditorUtility.ClearProgressBar();
    }

    private static void SetAbName(string assetPath)
    {
        if (assetPath.EndsWith(".meta") || Directory.Exists(assetPath))
        {
            return;
        }

        AssetImporter importer = AssetImporter.GetAtPath(assetPath);

        string filterResPath = resPath + filterDirNames;
        string luaPath = "Assets/" + BuildPackConfig.LuaOutFile + "/";
        if (importer.assetPath.StartsWith(luaPath))
        {
            if (importer.assetPath.EndsWith(".bytes"))
            {
                string luaShort = importer.assetPath.Replace(luaPath, "");
                string assetBundleName = luaShort.Substring(0, luaShort.IndexOf('/')) + AssetBundleConifg.AbSuffix;
                importer.assetBundleName = BuildPackConfig.LuaOutFile + "/" + assetBundleName;
            }
            return;
        }

        if (importer.assetPath.StartsWith(filterResPath))
        {
            importer.assetBundleName = null;
            return;
        }

        if (importer.assetPath.StartsWith(resPath + folderNames))//目录命名
        {
            if (importer.assetPath.EndsWith(".prefab"))
            {
                string fileName = Path.GetFileName(importer.assetPath);
                importer.assetBundleName = assetPath.Replace("Assets/", "").Replace("/" + fileName, AssetBundleConifg.AbSuffix);
            }
        }
        else if(importer.assetPath.StartsWith(resPath)|| importer.assetPath.StartsWith("Assets/luascript"))
        {
            importer.assetBundleName = assetPath.Replace("Assets/", "").Split('.')[0] + AssetBundleConifg.AbSuffix;
        }
    }

    /// <summary> 模型导入 不导入材质 </summary>
    void OnPreprocessModel()
    {

    }

}