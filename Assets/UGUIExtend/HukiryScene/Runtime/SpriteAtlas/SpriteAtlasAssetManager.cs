using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasAssetManager
{
    public static SpriteAtlasAssetManager Instance { get; set; } = new SpriteAtlasAssetManager();
    private Dictionary<string, SpriteAtlasAsset> m_spriteDic = new Dictionary<string, SpriteAtlasAsset>();

    private Dictionary<string, SpriteAtlas> m_spriteAtlas = new Dictionary<string, SpriteAtlas>();
    public SpriteAtlasAsset GetSpriteAtlasInfo(string texName)
    {
        if (m_spriteDic.ContainsKey(texName))
        {
            if (m_spriteDic[texName] == null)
            {
                var sAsset = GetSpriteAtlasAsset(texName);
                sAsset.InitDictionary();
                m_spriteDic[texName] = sAsset;
            }
            return m_spriteDic[texName];
        }
        else
        {
            var sAsset = GetSpriteAtlasAsset(texName);
            sAsset.InitDictionary();
            m_spriteDic.Add(texName, sAsset);
            return sAsset;
        }

    }

    private SpriteAtlasAsset GetSpriteAtlasAsset(string assetName)
    {
#if UNITY_EDITOR
        return Hukiry.HukiryUtilEditor.FindAssetObject<SpriteAtlasAsset>(assetName);
#else
        //后期修改
        //return LoadAsset<SpriteAtlasAsset>("tp", assetName);
        return null;
#endif

    }

    public Sprite GetSprite(string spriteName)
    {
        return GetSprite("Texture", spriteName);
    }

    private Dictionary<string, UnityEngine.Object[]> m_spriteAtlasObject = new Dictionary<string, UnityEngine.Object[]>();
    public Sprite GetSprite(string atlasName, string spriteName)
    {
#if UNITY_EDITOR
        if (!m_spriteAtlasObject.ContainsKey(atlasName))
        {
            string assetPath = Hukiry.HukiryUtilEditor.FindAssetPath<Texture2D>(atlasName);
            UnityEngine.Object[] objArray = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
            if (objArray != null && objArray.Length > 0)
            {
                m_spriteAtlasObject[atlasName] = objArray;
            }
        }

        for (int i = 0; i < m_spriteAtlasObject[atlasName].Length; i++)
        {
            if (m_spriteAtlasObject[atlasName][i].name == spriteName)
            {
                return m_spriteAtlasObject[atlasName][i] as Sprite;
            }
        }
        return Hukiry.HukiryUtilEditor.FindAssetObject<Sprite>(spriteName);
#endif
        return null;
    }

    public Vector2[] GetSpriteUV(string atlasName,string spriteName)
    {
        SpriteAtlasAsset atlasAsset = this.GetSpriteAtlasInfo(atlasName);
        SpriteData? spriteData = atlasAsset.GetSpriteData(spriteName);
        if (spriteData == null)
        {
            Vector2[] temp = new Vector2[4];
            return temp; 
        }
        else
        {
            return spriteData.Value.GetUvs();
        }
    }

    public void ClearData()
    {
        m_spriteDic.Clear();
        m_spriteAtlas.Clear();
    }
}
