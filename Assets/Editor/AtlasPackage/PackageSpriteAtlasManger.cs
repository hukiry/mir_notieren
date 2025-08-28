
using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
#endif
using System.IO;

namespace Hukiry
{
    [CreateAssetMenu(fileName = "PackageSpriteAtlasMangerAsset",menuName = "Hukiry/SpriteAtlasManger Asset", order =-1)]
    public class PackageSpriteAtlasManger : CommonAssets<PackageSpriteAtlasManger>
    {
        public PackageAtlasType packageAtlasType;
        /// <summary>
        /// 输入的图集
        /// </summary>
        public SpriteAtlas inputSpriteAtlas;
        public List<Object> objList=new List<Object> ();
        //打包图集的目录文件
        public string dirPath = "";
        //创建图鉴的目录路径
        public string creatSpriteAtlasDirPath = "";
        /// <summary>
        /// UV目录数据
        /// </summary>
      [HideInInspector]  public string uvDataDirPath;
        /// <summary>
        /// 根据文件目录创建图集
        /// </summary>
        public void PackageSpriteAtlasByDirectory()
        {
#if UNITY_EDITOR
            //图集纹理设置
            SpriteAtlasTextureSettings spriteAtlasTextureSettings = new SpriteAtlasTextureSettings
            {
                filterMode = FilterMode.Bilinear,
                sRGB = false,
                generateMipMaps = false,
                readable = false,
                anisoLevel = 10
            };

            //打包图集设置
            SpriteAtlasPackingSettings spriteAtlasPackingSettings = new SpriteAtlasPackingSettings
            {
                enableRotation = false,
                enableTightPacking = false,
                padding = 2
            };

            string[] dirs = Directory.GetDirectories(dirPath);
            int len = dirs.Length;
            for (int i = 0; i < len; i++)
            {
                DirectoryInfo di = new DirectoryInfo(dirs[i]);
                string filePath = creatSpriteAtlasDirPath + "/" + di.Name + ".spriteatlas";
                if (!File.Exists(filePath))
                {
                    SpriteAtlas spriteAtlas = new SpriteAtlas();
                    spriteAtlas.SetIncludeInBuild(true);
                    spriteAtlas.SetTextureSettings(spriteAtlasTextureSettings);
                    spriteAtlas.SetPackingSettings(spriteAtlasPackingSettings);
                    spriteAtlas.name = di.Name;
                    //打包的目录资源图片
                    string objPath = di.FullName.Replace('\\', '/').Replace(Application.dataPath, "Assets");
                    Object obj = AssetDatabase.LoadAssetAtPath<Object>(objPath);
                    spriteAtlas.Add(new Object[] { obj });
                    //创建图集文件
                    AssetDatabase.CreateAsset(spriteAtlas, filePath);
                    //打包图集
                    SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
                }

                EditorUtility.DisplayProgressBar("Package SpriteAtlas By Directory", di.Name, i / (float)len);
            }
            EditorUtility.ClearProgressBar();
            //保存并刷新
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void PackageSpriteAtlas()
        {
#if UNITY_EDITOR
            var objs = inputSpriteAtlas.GetPackables();
            inputSpriteAtlas.Remove(objs);
            inputSpriteAtlas.Add(objList.ToArray());
            SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { inputSpriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
#endif
        }

    }


    public enum PackageAtlasType
    {
        /// <summary>
        /// 每个目录一个图集
        /// </summary>
        DirectoryToSpriteAtlas,
        /// <summary>
        /// 多个目录配置到图集
        /// </summary>
        MulDirToSpriteAtlas
    }
}
