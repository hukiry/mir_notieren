#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class DrawGridMatch : MonoBehaviour
{
	public float sprite_width = 1.4f;
	[Range(2, 10)] [FormerlySerializedAs("size")] [SerializeField]
	public int size = 4;

	[HideInInspector]
	[SerializeField]
	public int sortingLayer;

	[Header("选择的索引")]
	[SerializeField]
	private Vector2Int selectIndex;
	[SerializeField] [HideInInspector] public bool isLightSelectMesh = false;
	[SerializeField] [Description("预览效果")] private bool isPreviewGrid = true;

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
    private Vector3 centerPos;

    private void OnDrawGizmos()
	{
		this.DrawGrid();
	}

	private void DrawGrid()
	{
		float z = 0;
		if (isPreviewGrid)
		{
			Gizmos.color = Color.red;
			float halfX = sprite_width / 2;
			float halfY = sprite_width / 2;
			for (int i = -size, k = 0; i <= size + 1; i++, k++)
			{
                Gizmos.DrawLine(new Vector3(size * sprite_width + halfX, i* sprite_width - halfY, z), new Vector3( -size * sprite_width - halfX, i * sprite_width - halfY, z));
                Gizmos.DrawLine(new Vector3(i * sprite_width - halfX, size * sprite_width + halfY, z), new Vector3(i * sprite_width - halfX, -size * sprite_width - halfY, z));
            }
		}
        this.DrawSelectGrid(z);
        GUI.color = Color.white;
	}

	private void DrawSelectGrid(float z)
	{
		Gizmos.color = Color.green;

		float x = sprite_width / 2;
		float y = sprite_width / 2;

		centerPos = MeshUtliMatch.CoordToWorldPoint(selectIndex);

		Vector3 left = new Vector3(centerPos.x - x, centerPos.y-y, z);
		Vector3 down = new Vector3(centerPos.x+x, centerPos.y - y, z);
		Vector3 right = new Vector3(centerPos.x + x, centerPos.y+y, z);
		Vector3 up = new Vector3(centerPos.x-x, centerPos.y + y, z);
		Gizmos.DrawLine(left, down);
		Gizmos.DrawLine(down, right);
		Gizmos.DrawLine(right, up);
		Gizmos.DrawLine(up, left);
		Gizmos.color = new Color(0, 1, 0, 0.5f);
		Gizmos.DrawSphere(new Vector3(centerPos.x, centerPos.y, centerPos.z), 0.05f);

		//绘制箭头
		UnityEditor.Handles.BeginGUI();
		GUI.color = new Color(255,0,0,255);
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
		float x = sprite_width / 2;
		float y = sprite_width / 2;
		if (Time.frameCount % 120 == 0 || isDraw)
		{
			for (int i = 0; i < vector2Ints.Length; i++)
			{
                arrowPos[i] = MeshUtliMatch.CoordToWorldPoint(new Vector2Int(vector2Ints[i].x * size, vector2Ints[i].y * size));
                if (i < 2)
                {
                    arrowPos[i].x = arrowPos[i].x + x * vector2Ints[i].x;
                }
                else
                {
                    arrowPos[i].y = arrowPos[i].y + y * vector2Ints[i].y;
                }

                arrowQuaternion[i] = Quaternion.LookRotation(arrowPos[i], Vector3.up);
                arrowText[i] = MeshUtliMatch.CoordToWorldPoint(new Vector2Int(vector2Ints[i].x * (size + 2), vector2Ints[i].y * (size + 2)));
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
