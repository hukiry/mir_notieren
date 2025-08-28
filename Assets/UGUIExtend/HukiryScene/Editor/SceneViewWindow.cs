using UnityEditor;
using UnityEngine;

public class SceneViewWindow
{
	private float Width = 100, Height = 16;
	private const int SPACE = 4;
	private GUIContent title = new GUIContent("选择");
	private Rect windowFloatRect;
	MeshGridTool meshGridTool;
	//绘制窗口 
	public void DrawWindow(SceneView sceneView)
	{
		var viewSize = sceneView.position.size;
		Rect rect = new Rect(SPACE, SPACE * 2 + Height, Width, viewSize.y - SPACE * 3 - Height);
		windowFloatRect = GUILayout.Window(title.text.GetHashCode(), rect, WindowFloat, title);
	}

	private Rect LeftRect(float y) => new Rect(SPACE, SPACE * 2 + Height, Width, y - SPACE * 3 - Height);
	private Rect RightDownRect(float x, float y)
	{
		float width = Width;
		float height = Width * 2;
		Rect rect = new Rect(x - SPACE - width, y - SPACE - height, Width, height);
		return rect;
	}
	private void WindowFloat(int id)
	{
		Vector2Int iconSize = new Vector2Int(65, 60);

		//meshGridTool.DrawList(windowFloatRect, iconSize, spriteName =>
		//{
		//    MapMeshGridGlobalAsset.Instance.selectSpriteName = spriteName;
		//    mapMeshGrid.SelectSprite();
		//}, mapMeshGrid?.GetMeshSpriteAtlas());
		//meshGridTool.DrawEraser();
		//if (GUI.changed)
		//{
		//    mapMeshGrid.SetSelectGameObject(!MapMeshGridGlobalAsset.Instance.isDelete);
		//}
		GUI.DragWindow();//可以拖动
	}
}
