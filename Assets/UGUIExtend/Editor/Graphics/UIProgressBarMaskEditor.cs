using Hukiry.UI;
using UnityEditor;

[CustomEditor(typeof(UIProgressbarMask), true), CanEditMultipleObjects]
public class UIProgressBarMaskEditor : Editor
{
	private SerializedProperty m_fillAmount;
	private SerializedProperty m_fillMethod;
	private SerializedProperty m_Softness;
	private SerializedProperty m_handle;
	private SerializedProperty m_OnValueChanged;
	private SerializedProperty m_handlePercent;
	private SerializedProperty m_percentText;
	private SerializedProperty m_PercentMethod;
	protected void OnEnable()
	{
		m_fillAmount = serializedObject.FindProperty("m_fillAmount");
		m_fillMethod = serializedObject.FindProperty("m_fillMethod");
		m_Softness = serializedObject.FindProperty("m_Softness");
		m_handle = serializedObject.FindProperty("m_handle");
		m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");
		m_handlePercent = serializedObject.FindProperty("m_handlePercent");
		m_percentText = serializedObject.FindProperty("m_percentText");
		m_PercentMethod = serializedObject.FindProperty("m_PercentMethod");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(m_fillMethod);
		EditorGUILayout.PropertyField(m_fillAmount);
		EditorGUILayout.PropertyField(m_handle);
		EditorGUILayout.PropertyField(m_percentText);

		var obj = Hukiry.HukiryUtilEditor.FieldInstance<UIProgressbarMask, UnityEngine.Object>(target as UIProgressbarMask, "m_percentText");
		if (obj)
			EditorGUILayout.PropertyField(m_PercentMethod);

		EditorGUILayout.PropertyField(m_Softness);
		EditorGUILayout.Space(5);
		EditorGUILayout.PropertyField(m_OnValueChanged);


		if (m_OnValueChanged.serializedObject.targetObject)
		{
			int count = Hukiry.HukiryUtilEditor.InvokeInstance<UIProgressbarMask, int>(target as UIProgressbarMask, "GetPersistentEventCount");
			if (count > 0)
			{
				var objTarget = Hukiry.HukiryUtilEditor.FieldInstance<UIProgressbarMask, UnityEngine.Object>(target as UIProgressbarMask, "m_percentText");
				if (objTarget == null)
				{
					EditorGUILayout.PropertyField(m_PercentMethod);
				}
				EditorGUILayout.PropertyField(m_handlePercent);
			}
		}
		serializedObject.ApplyModifiedProperties();
	}

}
