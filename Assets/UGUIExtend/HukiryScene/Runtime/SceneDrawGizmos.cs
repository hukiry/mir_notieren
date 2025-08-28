#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
public class SceneDrawGizmos
{
	private Vector3[] arrowPos = new Vector3[4];
	private Quaternion[] arrowQuaternion = new Quaternion[4];
	Vector2Int[] vector2Ints = new Vector2Int[4] {
		new Vector2Int (-1,0),
		new Vector2Int (1,0),
		new Vector2Int (0,-1),
		new Vector2Int (0,1),
	};
	private Vector3[] arrowText = new Vector3[4];
	private string[] arrowTextString = new string[4];

	private int size = 4;
	private Vector3 centerPos;
	private Vector2Int selectIndex;
	float viewZ = 0;

	public void SetIndexData(Vector2Int selectIndex)
	{
		this.selectIndex = selectIndex;
		this.centerPos = MeshUtli.CoordToWorldPoint(selectIndex);

	}

	public void DrawGrid(int size, bool isPreviewGrid)
	{
		this.size = size;

		if (isPreviewGrid)
		{
			Gizmos.color = Color.blue;
			float halfX = MeshUtli.m_width / 2;
			float halfY = MeshUtli.m_height / 2;
			for (int i = -size, k = 0; i <= size + 1; i++, k++)
			{
				Gizmos.DrawLine(new Vector3(-(size - i) * halfX - halfX, -k * halfY, viewZ), new Vector3(k * halfX, (size - i) * halfY + halfY, viewZ));
				Gizmos.DrawLine(new Vector3(-(size - i) * halfX - halfX, k * halfY, viewZ), new Vector3(k * halfX, (i - size) * halfY - halfY, viewZ));
			}
		}
		this.DrawSelectGrid(this.viewZ);
		GUI.color = Color.white;
	}


	private void DrawSelectGrid(float z)
	{
		Gizmos.color = Color.red;

		float x = MeshUtli.m_width / 2;
		float y = MeshUtli.m_height / 2;
		Vector3 left = new Vector3(centerPos.x - x, centerPos.y, z);
		Vector3 down = new Vector3(centerPos.x, centerPos.y - y, z);
		Vector3 right = new Vector3(centerPos.x + x, centerPos.y, z);
		Vector3 up = new Vector3(centerPos.x, centerPos.y + y, z);
		Gizmos.DrawLine(left, down);
		Gizmos.DrawLine(down, right);
		Gizmos.DrawLine(right, up);
		Gizmos.DrawLine(up, left);
		Gizmos.color = new Color(0, 1, 0, 0.5f);
		Gizmos.DrawSphere(new Vector3(centerPos.x, centerPos.y + 0.2f, centerPos.z), 0.05f);

		//绘制箭头
		UnityEditor.Handles.BeginGUI();
		GUI.color = new Color(255, 0, 0, 255);
		UnityEditor.Handles.Label(centerPos, string.Format("{0},{1}", selectIndex.x, selectIndex.y));
		this.DrawArrowMark();
		for (int i = 0; i < 4; i++)
		{
			UnityEditor.Handles.color = i >= 2 ? Color.red : Color.green;
			UnityEditor.Handles.ArrowHandleCap(1, arrowPos[i], arrowQuaternion[i], 2F, EventType.Repaint);
			GUI.color = i >= 2 ? Color.red : Color.green;
			UnityEditor.Handles.Label(arrowText[i], arrowTextString[i]);
		}
		GUI.color = Color.white;
		Handles.EndGUI();
	}

	private void DrawArrowMark(bool isDraw = false)
	{
		float x = MeshUtli.m_width / 2;
		float y = MeshUtli.m_height / 2;
		if (Time.frameCount % 120 == 0 || isDraw)
		{
			for (int i = 0; i < vector2Ints.Length; i++)
			{
				arrowPos[i] = MeshUtli.CoordToWorldPoint(new Vector2Int(vector2Ints[i].x * size, vector2Ints[i].y * size));
				if (i < 2)
				{
					arrowPos[i].x = arrowPos[i].x + x / 2 * vector2Ints[i].x;
					arrowPos[i].y = arrowPos[i].y + y / 2 * vector2Ints[i].x;
				}
				else
				{
					arrowPos[i].x = arrowPos[i].x + (-x / 2 * vector2Ints[i].y);
					arrowPos[i].y = arrowPos[i].y + y / 2 * vector2Ints[i].y;
				}

				arrowQuaternion[i] = Quaternion.LookRotation(arrowPos[i], Vector3.up);
				arrowText[i] = MeshUtli.CoordToWorldPoint(new Vector2Int(vector2Ints[i].x * (size + 3), vector2Ints[i].y * (size + 3)));
			}

			arrowTextString = new string[4]
			{
				"-X坐标轴",
				"X坐标轴",
				"-Y坐标轴",
				"Y坐标轴"
			};
		}
	}

}

#endif