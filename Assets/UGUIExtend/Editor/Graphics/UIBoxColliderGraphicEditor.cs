using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIBoxCollider)), CanEditMultipleObjects]
public class UIBoxColliderGraphicEditor : Editor
{
	UIBoxCollider m_uGUIEventGraphic;
	RectTransform m_RectTransform;
	Color drawColor;
	Vector3[] m_HandlePoints = new Vector3[4];
	private SerializedProperty m_RaycastTarget;

    protected void OnEnable()
	{
        m_uGUIEventGraphic = target as UIBoxCollider;
		m_RaycastTarget = serializedObject.FindProperty("m_RaycastTarget");
		m_RectTransform = m_uGUIEventGraphic.rectTransform;
        ColorUtility.TryParseHtmlString("#3DE64B", out drawColor);
	}

	public override void OnInspectorGUI()
	{
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Collider"), base.target);

        EditorGUILayout.PropertyField(m_RaycastTarget);
		serializedObject.ApplyModifiedProperties();
		if (Tools.current == Tool.Rotate &&GUILayout.Button(target.name))
		{
			Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("UIBoxCollider");
			EditorGUIUtility.PingObject(target.GetInstanceID());
		}

        //下拉菜单，用于场景编辑时，下拉菜单使用
        //var pos =EditorGUIUtility.ScreenToGUIPoint(Event.current.mousePosition);
        //EditorUtility.DisplayPopupMenu(new Rect(pos.x, pos.y, 0, 0), "GameObject", new MenuCommand(target));


    }



	private void OnSceneGUI()
	{
        if (!EditorApplication.isCompiling)
            DrawRectTransform();

    }

	private void DrawRectTransform()
	{
		if (m_uGUIEventGraphic.raycastTarget)
		{
			m_RectTransform.GetWorldCorners(m_HandlePoints);
			Vector3 lossyScale = m_RectTransform.lossyScale;
			//color one is the full fill,color one is the line fill
			Handles.DrawSolidRectangleWithOutline(m_HandlePoints, new Color32(255, 255, 255, 0), m_uGUIEventGraphic.enabled ? drawColor : new Color(0, 1, 1, 0.1F));
			Handles.color = Color.green;

			// Draw & process FreeMoveHandles
			Vector2 size = m_RectTransform.sizeDelta;
			Vector2 offset = m_RectTransform.anchoredPosition;
			// 左
			Vector3 oldLeft = (m_HandlePoints[0] + m_HandlePoints[1]) * 0.5f;
			Vector3 newLeft = Handles.FreeMoveHandle(oldLeft, Quaternion.identity, HandleUtility.GetHandleSize(m_RectTransform.position) * 0.05f, Vector3.zero, Handles.DotHandleCap);
			bool hasChanged = false;
			if (oldLeft != newLeft)
			{
				float delta = oldLeft.x - newLeft.x;
				size.x += delta / lossyScale.x;
				offset.x -= (delta / 2) / lossyScale.x;
				hasChanged = true;
			}

			// 上
			Vector3 oldTop = (m_HandlePoints[1] + m_HandlePoints[2]) * 0.5f;
			Vector3 newTop = Handles.FreeMoveHandle(oldTop, Quaternion.identity, HandleUtility.GetHandleSize(m_RectTransform.position) * 0.05f, Vector3.zero, Handles.DotHandleCap);
			if (oldTop != newTop)
			{
				float delta = oldTop.y - newTop.y;
				size.y -= delta / lossyScale.y;
				offset.y -= (delta / 2) / lossyScale.y;
				hasChanged = true;
			}

			// 右
			Vector3 oldRight = (m_HandlePoints[2] + m_HandlePoints[3]) * 0.5f;
			Vector3 newRight = Handles.FreeMoveHandle(oldRight, Quaternion.identity, HandleUtility.GetHandleSize(m_RectTransform.position) * 0.05f, Vector3.zero, Handles.DotHandleCap);
			if (oldRight != newRight)
			{
				float delta = oldRight.x - newRight.x;
				size.x -= delta / lossyScale.x;
				offset.x -= (delta / 2) / lossyScale.x;
				hasChanged = true;
			}

			// 下
			Vector3 oldBottom = (m_HandlePoints[3] + m_HandlePoints[0]) * 0.5f;
			Vector3 newBottom = Handles.FreeMoveHandle(oldBottom, Quaternion.identity, HandleUtility.GetHandleSize(m_RectTransform.position) * 0.05f, Vector3.zero, Handles.DotHandleCap);
			if (oldBottom != newBottom)
			{
				float delta = oldBottom.y - newBottom.y;
				size.y += delta / lossyScale.y;
				offset.y -= (delta / 2) / lossyScale.y;
				hasChanged = true;
			}

			if (hasChanged)
			{
				m_RectTransform.sizeDelta = size;
				m_RectTransform.anchoredPosition = offset;
				Undo.RecordObjects(new Object[] { m_RectTransform, m_uGUIEventGraphic }, "Rect Transform Changes");
				EditorUtility.SetDirty(target);
			}
		}
	}
}
