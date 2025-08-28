using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Hukiry;

[CustomEditor(typeof(HukirySupperText), true)]
[CanEditMultipleObjects]
public class HukirySupperTextEditor : GraphicEditor
{
    SerializedProperty m_Text;
    SerializedProperty m_FontData;
    SerializedProperty characterspacing;
    SerializedProperty applyGradient, gradientTop, gradientBottom;
    GUIContent _inputGUIContent, gradientColor,shadowEffect, guiUnderLine;

    SerializedProperty m_Style, effectColor, m_EffectDistance, m_UseGraphicAlpha, m_isEnableUnderLine;

    protected override void OnEnable()
    {
        base.OnEnable();
        _inputGUIContent = new GUIContent("Text");
        gradientColor = new GUIContent("启动颜色渐变");
        shadowEffect = new GUIContent("选择阴影样式");
        guiUnderLine = new GUIContent("显示下划线");

        m_Text = serializedObject.FindProperty("m_Text");
        m_FontData = serializedObject.FindProperty("m_FontData");
        characterspacing = serializedObject.FindProperty("characterspacing");

        applyGradient = serializedObject.FindProperty("applyGradient");
        gradientTop = serializedObject.FindProperty("m_gradientTop");
        gradientBottom = serializedObject.FindProperty("m_gradientBottom");

        m_Style = serializedObject.FindProperty("m_Style");
        effectColor = serializedObject.FindProperty("m_effectColor");
        m_EffectDistance = serializedObject.FindProperty("m_EffectDistance");
        m_UseGraphicAlpha = serializedObject.FindProperty("m_UseGraphicAlpha");
        m_isEnableUnderLine = serializedObject.FindProperty("m_isEnableUnderLine");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Text, _inputGUIContent);
        //字符间距
        EditorGUILayout.PropertyField(characterspacing);

        EditorGUILayout.PropertyField(m_FontData);
        AppearanceControlsGUI();
        RaycastControlsGUI();
        GUILayout.Space(5);

        
        Hukiry.HukiryUtilEditor.DrawLine(Color.green * 255F, Color.white * 0.2F);
        EditorGUILayout.PropertyField(m_isEnableUnderLine, guiUnderLine);

        //颜色渐变
        EditorGUILayout.PropertyField(applyGradient, gradientColor);
        if (applyGradient.boolValue)
        {
            HorizontalScope(gradientTop);
            HorizontalScope(gradientBottom);
            Hukiry.HukiryUtilEditor.DrawLine(Color.green * 255F, Color.white * 0.2F);
        }

        //特效
        EditorGUILayout.PropertyField(m_Style, shadowEffect);
        if (m_Style.enumValueIndex > 0)
        {
            HorizontalScope(effectColor);
            HorizontalScope(m_EffectDistance);
            HorizontalScope(m_UseGraphicAlpha);
        }

        if (HukirySupperText.m_EnableHelp)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUIUtility.fieldWidth, 16f, 78, EditorStyles.numberField);
            if (GUI.Button(rect, "取消")) HukirySupperText.m_EnableHelp = false;
            EditorGUILayout.HelpBox("超链接配置如下：\n" +
                "1，[-1#文本显示#00FFFC22]\n" +
                "2，[1#文本显示#00FFFC]\n" +
                "3，[22#文本显示#00FF]\n" +
                "4，[33#文本显示#00F]\n" +
                "5，[任意字符#文本显示]\n", MessageType.Info);
            GUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }


    private void HorizontalScope(SerializedProperty serializedProperty, GUIContent content= null)
    {
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(15);
            EditorGUILayout.PropertyField(serializedProperty, content);
        }
    }
}
