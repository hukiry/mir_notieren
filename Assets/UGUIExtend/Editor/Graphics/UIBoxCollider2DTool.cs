using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
[EditorTool("Edit Box Collider 2D", typeof(UIBoxCollider))]
public class UIBoxCollider2DTool : EditorTool
{
	private readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();
    private static GUIContent s_EditModeButton;

    public override GUIContent toolbarIcon => editModeButton;
	static Color s_ColliderHandleColor = new Color(145f, 244f, 139f, 210f) / 255f;
	 static Color s_ColliderHandleColorDisabled = new Color(84f, 200f, 77f, 140f) / 255f;

	private void OnEnable()
	{
		m_BoundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
	}

	public override void OnToolGUI(EditorWindow window)
	{
		foreach (Object target in base.targets)
		{
			BoxCollider2D boxCollider2D = target as BoxCollider2D;
			if (boxCollider2D == null || Mathf.Approximately(boxCollider2D.transform.lossyScale.sqrMagnitude, 0f))
			{
				continue;
			}
			Matrix4x4 matrix4x = boxCollider2D.transform.localToWorldMatrix;
			matrix4x.SetRow(0, Vector4.Scale(matrix4x.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
			matrix4x.SetRow(1, Vector4.Scale(matrix4x.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
			matrix4x.SetRow(2, new Vector4(0f, 0f, 1f, boxCollider2D.transform.position.z));
			if (boxCollider2D.usedByComposite && boxCollider2D.composite != null)
			{
				Vector3 pos = boxCollider2D.composite.transform.rotation * boxCollider2D.composite.offset;
				pos.z = 0f;
				matrix4x = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one) * matrix4x;
			}
			using (new Handles.DrawingScope(matrix4x))
			{
				m_BoundsHandle.center = boxCollider2D.offset;
				m_BoundsHandle.size = boxCollider2D.size;
				m_BoundsHandle.SetColor(boxCollider2D.enabled ? s_ColliderHandleColor : s_ColliderHandleColorDisabled);
				EditorGUI.BeginChangeCheck();
				m_BoundsHandle.DrawHandle();
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(boxCollider2D, $"Modify {ObjectNames.NicifyVariableName(boxCollider2D.GetType().Name)}");
					Vector2 size = boxCollider2D.size;
					boxCollider2D.size = m_BoundsHandle.size;
					if (boxCollider2D.size != size)
					{
						boxCollider2D.offset = m_BoundsHandle.center;
					}
				}
			}
		}
	}

	internal static GUIContent editModeButton
	{
		get
		{
			if (s_EditModeButton == null)
			{
				s_EditModeButton = new GUIContent(EditorGUIUtility.IconContent("EditCollider").image, EditorGUIUtility.TrTextContent("Edit bounding volume.\n\n - Hold Alt after clicking control handle to pin center in place.\n - Hold Shift after clicking control handle to scale uniformly.").text);
			}
			return s_EditModeButton;
		}
	}
}
