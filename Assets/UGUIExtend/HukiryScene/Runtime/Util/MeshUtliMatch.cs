using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.U2D;

/// <summary>
/// 编辑工具生成位置计算
/// </summary>
public class MeshUtliMatch
{
	private static float m_x = 1.4f;
	private static float m_y = 1.4f;

	//世界坐标转换索引坐标
	public static Vector2Int WorldPointToCoord(Vector2 worldPoint)
	{
		int _x = Mathf.RoundToInt(worldPoint.x / m_y);
		int _y = Mathf.RoundToInt(worldPoint.y / m_y);
		return new Vector2Int(_x, _y);
	}

	//索引坐标转换世界坐标
	public static Vector3 CoordToWorldPoint(Vector2Int coord)
	{
		float _x = coord.x * m_x;
		float _y = coord.y * m_y;
		return new Vector3(_x, _y, 0);
	}

	public static Vector3 CoordToWorldPoint(int _x, int _y)
	{
		float x = _x * m_x;
		float y = _y * m_y;
		return new Vector3(x, y);
	}

	public static bool IsOverRange(Vector2Int coord, int size)
	{
		return coord.x > size || coord.x < -size || coord.y < -size || coord.y > size;
	}

	/// <summary>
	/// 创建网格
	/// </summary>
	/// <param name="spriteAtlas">图集</param>
	/// <param name="spriteInfoList">排序后的精灵信息集合</param>
	/// <returns>网格</returns>
	public static Mesh CreateGeometryMesh(SpriteAtlas spriteAtlas, List<SpriteMeshInfo> spriteInfoList)
	{
		Mesh mesh = new Mesh();
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<Color> colos = new List<Color>();

		var count = spriteInfoList.Count;

		float half = 0;
		for (int i = 0; i < count; i++)
		{
			var data = spriteInfoList[i];
			Vector2 pos = data.centerPos;
			//世界网格顶点位置计算
			Vector2[] spriteUvs = data.GetSpriteUvs(spriteAtlas, out float x, out float y);
			verts.Add(new Vector3(pos.x - x, pos.y - y + half, 0));
			verts.Add(new Vector3(pos.x - x, pos.y + y + half, 0));
			verts.Add(new Vector3(pos.x + x, pos.y + y + half, 0));
			verts.Add(new Vector3(pos.x + x, pos.y - y + half, 0));
			//世界纹理UV
			uvs.AddRange(spriteUvs);
			//顶点的颜色
			colos.Add(Color.white);
			colos.Add(Color.white);
			colos.Add(Color.white);
			colos.Add(Color.white);
			data.index = i;
			spriteInfoList[i] = data;
		}

		int[] mindexs = GenerateCachedIndexBuffer(verts.Count, (verts.Count >> 1) * 3);
		mesh.SetVertices(verts);//设置顶点位置
		mesh.SetUVs(0, uvs);//设置uv
		mesh.SetTriangles(mindexs, 0, false);//设置三角形索引
		mesh.SetColors(colos);//设置顶点颜色
		return mesh;
	}

	/// <summary>
	/// 保存网格
	/// </summary>
	/// <param name="mesh"></param>
	/// <param name="Path">文件后缀.asset</param>
	public static void SaveMesh(Mesh mesh, string Path)
	{
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.CreateAsset(mesh, Path);
#endif
	}

	public static void SetMeshFilter(Transform transform, Mesh mesh)
	{
		MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>()?? transform.gameObject.AddComponent<MeshRenderer>();
		if (meshRenderer)
		{
			meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			meshRenderer.receiveShadows = false;
			meshRenderer.sortingOrder = 100;
		}
		MeshFilter meshFilter = transform.GetComponent<MeshFilter>() ?? transform.gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
	}

	#region 三角形索引
	const int maxIndexBufferCache = 10;
    static List<int[]> mIndexCache = new List<int[]>(maxIndexBufferCache);
    static int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
    {
        int Count = mIndexCache.Count;
        for (int i = 0; i < Count; ++i)
        {
            int[] ids = mIndexCache[i];
            if (ids != null && ids.Length == indexCount)
                return ids;
        }

        int[] indexTriangles = new int[indexCount];
        int index = 0;
		//每个网格，有4个UV坐标，2个三角形，6个顶点索引
        for (int i = 0; i < vertexCount; i += 4)
        {
            //右三角形：顺序旋转，左上角为0开始
            indexTriangles[index++] = i;
            indexTriangles[index++] = i + 1;
            indexTriangles[index++] = i + 2;
            //左三角形
            indexTriangles[index++] = i + 2;
            indexTriangles[index++] = i + 3;
            indexTriangles[index++] = i;
        }

        if (mIndexCache.Count > maxIndexBufferCache) mIndexCache.RemoveAt(0);
        mIndexCache.Add(indexTriangles);
        return indexTriangles;
    } 
    #endregion
}

[System.Serializable]
public struct SpriteMeshInfo
{
	/// <summary>
	/// 中心点世界位置
	/// </summary>
    public Vector2 centerPos;
	/// <summary>
	/// 索引网格顺序
	/// </summary>
	public int index;
	/// <summary>
	/// 排序
	/// </summary>
	public int sort;

	/// <summary>
	/// 精灵名称
	/// </summary>
	public string spriteName;

	/// <summary>
	/// 根据图集获取精灵名信息
	/// </summary>
	/// <param name="spriteAtlas"></param>
	/// <param name="halfWidth">输出精灵一半宽</param>
	/// <param name="halfHeight">输出精灵一半高</param>
	/// <returns>UV坐标</returns>
	public Vector2[] GetSpriteUvs(SpriteAtlas spriteAtlas, out float halfWidth, out float halfHeight)
	{
		var sp = spriteAtlas.GetSprite(this.spriteName);
		var outerUV = DataUtility.GetOuterUV(sp);
		var spriteUV = new Vector2[4] {
				new Vector2(outerUV.x, outerUV.y),
				new Vector2(outerUV.x, outerUV.w),
				new Vector2(outerUV.z, outerUV.w),
				new Vector2(outerUV.z, outerUV.y),
			};

		halfWidth = sp.rect.width / 200F;
		halfHeight = sp.rect.height / 200F;
		return spriteUV;
	}
}
