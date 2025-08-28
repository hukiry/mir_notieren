using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 线性渲染绘制
/// </summary>
[ExecuteAlways]
public class DrawRendererGrid : MonoBehaviour
{
	[SerializeField]
	private float sprite_width = 1.4f;//等于ui 140像素
	//[Range(2, 10)]
	[FormerlySerializedAs("size")]
	//[SerializeField]
	private int size = 4;
	[Header("选择的索引")]
	[SerializeField]
	private Vector2Int selectIndex;

	[SerializeField]
	private LineRenderer lineSelect;
	//线条渲染
	private LineRenderer lineMap;
	[Range(0.01F, 0.05F)]
	[SerializeField]
	private float widthLine = 0.02F;
	private Color colorLine = Color.red;
	private const float z = 0;
	private void OnEnable()
	{
		Material material = new Material(Shader.Find("UI/Default"));
		this.lineMap = GetComponent<LineRenderer>();
		this.lineMap.sharedMaterial = material;
		if (lineSelect == null)
		{
			this.lineSelect = this.transform.GetChild(0).GetComponent<LineRenderer>();
		}

		this.lineSelect.startWidth = widthLine + 0.01F;
		this.lineSelect.endWidth = widthLine + 0.01F;
		this.lineSelect.startColor = Color.green;
		this.lineSelect.endColor = Color.green;
		this.lineSelect.sharedMaterial = material;

		this.StartDraw();

	}

	private void FixedUpdate()
	{
		this.StartDraw();
	}

	private void StartDraw()
	{
		//每20帧绘制一次
		if (Time.frameCount % 60 == 0)
		{
            try
            {
                this.DrawGrid();
                this.DrawSelectGrid();
            }catch{}
		}
	}

	//支持打包
	private void DrawGrid()
	{
		this.lineMap.startWidth = widthLine;
		this.lineMap.endWidth = widthLine;
		this.lineMap.startColor = colorLine;
		this.lineMap.endColor = colorLine;

		float halfX = sprite_width / 2;
		float halfY = sprite_width / 2;
		List<Vector3> posHorizontalList = new List<Vector3>();//水平坐标
		List<Vector3> posVerticalList = new List<Vector3>();//垂直坐标
		for (int i = -size, k = 0; i <= size + 1; i++, k++)
		{
			var xStart = new Vector3(size * sprite_width + halfX, i * sprite_width - halfY, z);
			var xEnd = new Vector3(-size * sprite_width - halfX, i * sprite_width - halfY, z);
			posHorizontalList.Add(i % 2 == 0 ? xStart : xEnd);
			posHorizontalList.Add(i % 2 == 0 ? xEnd : xStart);

			var yStart = new Vector3(i * sprite_width - halfX, size * sprite_width + halfY, z);
			var yEnd = new Vector3(i * sprite_width - halfX, -size * sprite_width - halfY, z);
			posVerticalList.Add(i % 2 == 0 ? yStart : yEnd);
			posVerticalList.Add(i % 2 == 0 ? yEnd : yStart);
		}
		posHorizontalList.AddRange(posVerticalList);//将垂直坐标附加到水平坐标上
		this.lineMap.positionCount = posHorizontalList.Count;
		this.lineMap.SetPositions(posHorizontalList.ToArray());
	}

	private void DrawSelectGrid()
	{
		float x = sprite_width / 2;
		float y = sprite_width / 2;
		var centerPos = MeshUtliMatch.CoordToWorldPoint(this.selectIndex);
		List<Vector3> posList = new List<Vector3>();
		posList.Add(new Vector3(centerPos.x - x, centerPos.y - y, z));
		posList.Add(new Vector3(centerPos.x + x, centerPos.y - y, z));
		posList.Add(new Vector3(centerPos.x + x, centerPos.y + y, z));
		posList.Add(new Vector3(centerPos.x - x, centerPos.y + y, z));
		//最后一个点和第一个点相同
		posList.Add(new Vector3(centerPos.x - x, centerPos.y - y, z));
		this.lineSelect.positionCount = 5;
		this.lineSelect.SetPositions(posList.ToArray());
	}

	/// <summary>
	/// 选择格子的位置
	/// </summary>
	public void SetPositionTile(int x, int y)
	{
		this.selectIndex = new Vector2Int(x, y);
		this.DrawSelectGrid();
	}
}
