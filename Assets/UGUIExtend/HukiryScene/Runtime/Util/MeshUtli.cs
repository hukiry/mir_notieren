using UnityEngine;

/// <summary>
/// 编辑工具生成位置计算
/// </summary>
public class MeshUtli
{
	public static float m_width = 2.56f;
	public static float m_height = 1.56F;

	//世界坐标转换索引坐标
	public static Vector2Int WorldPointToCoord(Vector2 worldPoint)
	{
		int _x = Mathf.RoundToInt(worldPoint.x / m_width + worldPoint.y / m_height);
		int _y = -Mathf.RoundToInt(worldPoint.x / m_width - worldPoint.y / m_height);
		return new Vector2Int(_x, _y);
	}

	public static Vector2Int CoordToWorldPoint(float x, float y)
	{
		int _x = Mathf.RoundToInt(x / m_width + y / m_height);
		int _y = -Mathf.RoundToInt(x / m_width - y / m_height);
		return new Vector2Int(_x, _y);
	}

	//索引坐标转换世界坐标
	public static Vector3 CoordToWorldPoint(Vector2Int coord)
	{
		float halfX = m_width / 2;
		float halfY = m_height / 2;
		float _x = (coord.x - coord.y) * halfX;
		float _y = (coord.x + coord.y) * halfY;
		return new Vector3(_x, _y, 0);
	}

	public static Vector3 CoordToWorldPoint(int _x, int _y)
	{
		float halfX = m_width / 2;
		float halfY = m_height / 2;
		float x = (_x - _y) * halfX;
		float y = (_x + _y) * halfY;
		return new Vector3(x, y);
	}

	public static bool IsOverRange(Vector2Int coord, int size)
	{
		return coord.x > size || coord.x < -size || coord.y < -size || coord.y > size;
	}
}
