using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 精灵图集缓存,运行时编辑
/// </summary>
public class SpriteAtlasAsset : ScriptableObject
{
    public string spriteAtlasName;
    public List<SpriteData> spriteDatas = new List<SpriteData>();

    private Dictionary<string, SpriteData> m_spriteDic = new Dictionary<string, SpriteData>();
    public SpriteData? GetSpriteData(string spriteName)
    {
        if (m_spriteDic.ContainsKey(spriteName))
        {
            return m_spriteDic[spriteName];
        }

        Debug.LogError($"图集中{spriteAtlasName} 不存在此精灵名：{spriteName}");
        return null;
    }

    public void InitDictionary()
    {
        foreach (var item in spriteDatas)
        {
            m_spriteDic[item.spriteName] = item;
        }
    }

    /// <summary>
    /// 仅仅刷新网格UV坐标
    /// </summary>
    /// <param name="uvs">uv坐标</param>
    /// <param name="isFilp">是否有翻转</param>
    /// <returns></returns>
    public string GetSpriteNameByUV(Vector2[] uvs, ref bool isFilp)
    {
        foreach (var item in m_spriteDic)
        {
            if (item.Value.IsEqual(uvs, ref isFilp))
            {
                return item.Key;
            }
        }
        return null;
    }
}

[Serializable]
public struct SpriteData
{
    public string spriteName;
    public float x;
    public float y;
    public float width;
    public float height;
    public Vector2[] uvs;
    public Vector4 border;

    public Vector2[] GetUvs() => uvs;

    public void SetUVs(Vector4 outerUV)
    {
        this.uvs = new Vector2[4] {
            new Vector2(outerUV.x, outerUV.y),
            new Vector2(outerUV.x, outerUV.w),
            new Vector2(outerUV.z, outerUV.w),
            new Vector2(outerUV.z, outerUV.y),
        };
    }

    //获取精灵分割Rect
    public Rect GetRect()
    {
        return new Rect((int)x, (int)y, (int)width, (int)height);
    }

    //反uv索引对比
    public bool IsEqual(Vector2[] uvs, ref bool isFilp)
    {
        if (this.IsFlipH(uvs))
        {
            isFilp = true;
            return true;
        }
        return this.uvs[0] == uvs[0] && this.uvs[1] == uvs[1] && this.uvs[2] == uvs[2] && this.uvs[3] == uvs[3];
    }

    //网格UV翻转
    private bool IsFlipH(Vector2[] uvs)
    {
        var uvsDefault = new Vector2[4];
        (float x_sh, float y_sh, float z_sh, float w_sh) = (this.uvs[0].x, this.uvs[0].y, this.uvs[2].x, this.uvs[2].y);
        uvsDefault[0] = new Vector2(z_sh, y_sh);
        uvsDefault[1] = new Vector2(z_sh, w_sh);
        uvsDefault[2] = new Vector2(x_sh, w_sh);
        uvsDefault[3] = new Vector2(x_sh, y_sh);
        return uvsDefault[0] == uvs[0] && uvsDefault[1] == uvs[1] && uvsDefault[2] == uvs[2] && uvsDefault[3] == uvs[3];
    }
}
