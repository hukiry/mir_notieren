using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

//贴图导出，和分割贴图纹理
public class Texture2DExportEditor
{
    //public static void ExoportSpriteAtlasUvs()
    //{
    //    EditorApplication.isPlaying = true;

    //    if (EditorApplication.isPlaying)
    //    {
    //        var sprite_Uv_Datas = MapMeshGridGlobalAsset.Instance.sprite_Uv_Datas;
    //        sprite_Uv_Datas.Clear();
    //        string path = "Assets/ResourcesEx/AtlasScene";
    //        string[] files = Directory.GetFiles(path, "*.spriteatlas", SearchOption.AllDirectories);
    //        int length = files.Length;
    //        for (int i = 0; i < length; i++)
    //        {
    //            string fileName = Path.GetFileNameWithoutExtension(files[i]);
    //            if (fileName.ToLower().Contains("mesh") || fileName.StartsWith("Item"))
    //            {
    //                SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(files[i]);
    //                Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
    //                spriteAtlas.GetSprites(sprites);
    //                foreach (var item in sprites)
    //                {
    //                    string spriteName = item.name.Replace("(Clone)", "");
    //                    sprite_uv_data uv_Data = new sprite_uv_data();
    //                    uv_Data.spriteName = spriteName;
    //                    uv_Data.SetUVs(UnityEngine.Sprites.DataUtility.GetOuterUV(item));
    //                    if (sprite_Uv_Datas.Count == 0)
    //                    {
    //                        sprite_Uv_Datas.Add(uv_Data);
    //                    }
    //                    else
    //                    {
    //                        int index = sprite_Uv_Datas.FindIndex(p => p.spriteName == spriteName);
    //                        if (index >= 0)
    //                        {
    //                            sprite_Uv_Datas[i] = uv_Data;
    //                        }
    //                        else
    //                        {
    //                            sprite_Uv_Datas.Add(uv_Data);
    //                        }
    //                    }

    //                }
    //                EditorUtility.DisplayProgressBar("导出图集数据", spriteAtlas.name, i / (float)length);
    //            }
    //        }
    //        MapMeshGridGlobalAsset.Instance.sprite_Uv_Datas = sprite_Uv_Datas;
    //        MapMeshGridGlobalAsset.Instance.SaveAssets();
    //        EditorUtility.ClearProgressBar();
    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();
    //    }
    //}

    /// <summary>
    /// 生成图集数据
    /// </summary>
    /// <param name="spriteAtlas"></param>
    public static void ExportCurrentSpriteAtlasUV(SpriteAtlas spriteAtlas, bool isSplit = true)
    {
        string texturePath = Path.ChangeExtension(AssetDatabase.GetAssetPath(spriteAtlas), ".asset");
        SpriteAtlasAsset spriteAtlasAsset = AssetDatabase.LoadAssetAtPath<SpriteAtlasAsset>(texturePath);
        var temp = GetSpriteAtlasAsset(spriteAtlas);
        if (spriteAtlasAsset == null)
        {
            spriteAtlasAsset = new SpriteAtlasAsset();
            spriteAtlasAsset.spriteAtlasName = spriteAtlas.name;
            spriteAtlasAsset.spriteDatas = temp;
            UnityEditor.AssetDatabase.CreateAsset(spriteAtlasAsset, texturePath);
        }
        else
        {
            if (spriteAtlasAsset.spriteAtlasName == spriteAtlas.name)
            {
                spriteAtlasAsset.spriteDatas = temp;
                EditorUtility.SetDirty(spriteAtlasAsset);
            }
        }

        if (isSplit)
            SplitTexture(Path.ChangeExtension(texturePath,".png"), spriteAtlasAsset);
    }

    public static void ExportCustomSpriteAtlasUV(SpriteAtlas spriteAtlas, string jsonFilePath)
    {
        SpriteAtlasAsset spriteAtlasAsset = new SpriteAtlasAsset();
        spriteAtlasAsset.spriteAtlasName = spriteAtlas.name;
        spriteAtlasAsset.spriteDatas = GetSpriteAtlasAsset(spriteAtlas);
        string jsonStr = JsonUtility.ToJson(spriteAtlasAsset);
        File.WriteAllText(jsonFilePath, jsonStr);
    }

    private static List<SpriteData> GetSpriteAtlasAsset(SpriteAtlas spriteAtlas)
    {
        Sprite[] sprites = GetSprites(spriteAtlas);

        List<SpriteData> temp = new List<SpriteData>();
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite sprite = sprites[i];
            SpriteData spriteData = new SpriteData();
            spriteData.spriteName = sprite.name.Replace("(Clone)", "");
            spriteData.x = sprite.textureRect.x;
            spriteData.y = sprite.textureRect.y;
            spriteData.height = sprite.textureRect.height;
            spriteData.width = sprite.textureRect.width;
            spriteData.border = sprite.border;
            spriteData.SetUVs(UnityEngine.Sprites.DataUtility.GetOuterUV(sprite));
            temp.Add(spriteData);
        }
        return temp;
    }

    private static Sprite[] GetSprites(SpriteAtlas spriteAtlas)
    {
        Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(sprites);
        return sprites;
    }

    public static void SplitTexture(string assetPath, SpriteAtlasAsset spriteAtlasAsset)
    {
        AssetDatabase.Refresh();
        TextureImporter importer = TextureImporter.GetAtPath(assetPath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        List<SpriteMetaData> spritesheet = new List<SpriteMetaData>();
        for (int index = 0; index < spriteAtlasAsset.spriteDatas.Count; ++index)
        {
            SpriteData data = spriteAtlasAsset.spriteDatas[index];
            SpriteMetaData spriteMetaData = new SpriteMetaData();
            spriteMetaData.name = data.spriteName;
            spriteMetaData.rect = data.GetRect();
            spriteMetaData.pivot = new Vector2(0.5f, 0.5f);
            spriteMetaData.border = data.border;
            spritesheet.Add(spriteMetaData);
        }

        importer.spritesheet = spritesheet.ToArray();
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }
}