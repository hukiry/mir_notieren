using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[CustomEditor(typeof(SceneViewGrid))]
public class SceneViewGridEditor : Editor
{
	private float Width = 100, Height = 40;
	private const int SPACE = 4;
	SceneViewGrid sceneViewGrid;
	private Rect windowFloatRect;
	private bool isCanDrag = false;

	SceneViewWindow sceneViewWindow = new SceneViewWindow();

	private void OnEnable()
	{
		sceneViewGrid = target as SceneViewGrid;

#if UNITY_2019_4_OR_NEWER
		SceneView.duringSceneGui -= DrawSceneGUI;
		SceneView.duringSceneGui += DrawSceneGUI;
#else
		SceneView.onSceneGUIDelegate = DrawSceneGUI;
#endif

		EditorSceneManager.activeSceneChangedInEditMode -= EditorActiveSceneChanged;
		EditorSceneManager.activeSceneChangedInEditMode += EditorActiveSceneChanged;

	}

	private void EditorActiveSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
	{
		if (arg1.name != "MeshGrid")
		{
#if UNITY_2019_4_OR_NEWER
			SceneView.duringSceneGui -= DrawSceneGUI;
#else
			SceneView.onSceneGUIDelegate = null;
#endif
		}
		else
		{
			//动态选择对象
			Selection.SetActiveObjectWithContext(GameObject.FindObjectOfType<SceneViewGrid>().gameObject, this);
		}
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox("拷贝（Shift+C），粘贴（Shift+V），删除（Shift+D）", MessageType.Info);
		base.OnInspectorGUI();
	}

	private void OnDisable()
	{
		//mapMeshGrid.SetSelectGameObject(false);
	}

	//绘制场景
	private void DrawSceneGUI(SceneView sceneView)
	{
		if (sceneViewGrid.isCancelEditor)
		{
			return;
		}

		EventType eventType = Event.current.type;
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			if (eventType != EventType.Repaint && eventType != EventType.Layout)
			{
				//添加输入的点
				ProcessBezierPathInput(Event.current);
			}
			// Don't allow clicking over empty space to deselect the object
			sceneViewWindow?.DrawWindow(sceneView);

			if (eventType == EventType.Layout)
			{
				HandleUtility.AddDefaultControl(0);
			}
			if (check.changed)
			{
				EditorApplication.QueuePlayerLoopUpdate();
			}
		}
		//Selection.activeTransform = sceneViewGrid.transform;
	}

	private void ProcessBezierPathInput(Event e)
	{
		if (sceneViewGrid == null) return;
		if (e.type == EventType.MouseMove)
		{
			var origin = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
			sceneViewGrid?.UpdateIndex(origin);
		}
		//拷贝资源
		if (e.shift && e.keyCode == KeyCode.C)
		{
			Undo.RecordObject(target, "Copy_SpriteName");
			return;
		}
		//粘贴资源
		if (e.shift && e.keyCode == KeyCode.V)
		{
			Undo.RecordObject(target, "Paste_SpriteName");
			return;
		}

		//删除资源
		if (e.shift && e.keyCode == KeyCode.D)
		{
			Undo.RecordObject(target, "Delete_SpriteName");

			return;
		}

		//鼠标按下
		if ((e.type == EventType.MouseDown && e.button == 0))
		{
			Undo.RecordObject(target, "Place");
			isCanDrag = true;
		}

		//鼠标拖动
		if (isCanDrag && e.type == EventType.MouseDrag && e.button == 0)
		{
			Undo.RecordObject(target, "Delete");
			var origin = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
		}

		//鼠标抬起
		if (e.type == EventType.MouseUp)
		{
			isCanDrag = false;
		}

		//鼠标右键菜单
		if (e.type == EventType.MouseUp && e.button == 1)
		{
			var origin = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
			//DrawDropMenu();
		}
	}

}
