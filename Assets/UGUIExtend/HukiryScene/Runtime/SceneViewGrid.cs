#if UNITY_EDITOR
using UnityEngine;

[ExecuteInEditMode]
//[ExecuteAlways]
public class SceneViewGrid : MonoBehaviour
{
	[Range(4, 40)] [SerializeField]
	public int size = 5;
	[Header("选择的索引")]
	[SerializeField]
	private Vector2Int selectIndex;
	private Vector3 centerPos;
	[Header("预览效果")]
	[SerializeField]  private bool isPreviewGrid = true;
	[Header("取消编辑")]
	[SerializeField]public bool isCancelEditor;


    SceneDrawGizmos sceneDrawGizmos = new SceneDrawGizmos();
    //跟随放置的精灵
    public void UpdateIndex(Vector2 origin)
	{
		var index = MeshUtli.WorldPointToCoord(origin);
		index.x = Mathf.Clamp(index.x, -size, size);
		index.y = Mathf.Clamp(index.y, -size, size);
		if (index != selectIndex)
		{
			this.selectIndex = index;
			this.centerPos = MeshUtli.CoordToWorldPoint(index);
		}
		var pos = new Vector3(origin.x + MeshUtli.m_width / 4, origin.y - MeshUtli.m_height);
        sceneDrawGizmos.SetIndexData(this.selectIndex);

    }


    private void OnDrawGizmos()
	{
        sceneDrawGizmos.DrawGrid(this.size, this.isPreviewGrid);
    }



	private void OnValidate()
	{

	}
}
#endif
