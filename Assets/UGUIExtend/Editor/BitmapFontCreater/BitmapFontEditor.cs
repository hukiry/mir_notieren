using Hukiry.UI;
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(Font))]
public class BitmapFontEditor : Editor
{
    Font m_font;
    private Vector2 scroll;
    CharacterInfo[] characterInfo;
    private AnimBool animShowType;

    void OnEnable()
    {
        m_font = target as Font;
        characterInfo = m_font?.characterInfo;

        animShowType = new AnimBool(true);
        animShowType.valueChanged.AddListener(new UnityAction(base.Repaint));

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        int fontSize = GUI.skin.label.fontSize;
        Color col = GUI.contentColor;
    
        if (AtlasImageUtility.DrawHeader("扩展编辑")){
            if (characterInfo!=null && characterInfo.Length > 0)
            {
                int index = 0;
                scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(300));
                do
                {
                  
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        bool isChange = index< m_font.characterInfo.Length && m_font.characterInfo[index].index == characterInfo[index].index;
                        GUI.contentColor = isChange ? col : new Color(255,255,255,255);
                        char ch = (char)characterInfo[index].index;
                        GUI.skin.label.fontSize = 30;
                        EditorGUILayout.LabelField("字符：", GUILayout.Width(50));
                        
                        string txt = EditorGUILayout.TextField(ch.ToString());
                        if (!string.IsNullOrEmpty(txt))
                        {
                            characterInfo[index].index = txt[0];
                        }

                        GUI.contentColor = isChange ? col : Color.green;
                        using (new EditorGUI.DisabledGroupScope(isChange))
                        {
                            if (index >= m_font.characterInfo.Length)
                            {
                                if (EditorGUILayout.BeginFadeGroup(animShowType.faded))
                                if (GUILayout.Button("取消"))
                                {
                                    var newTemp = new CharacterInfo[characterInfo.Length - 1];
                                    Array.Copy(characterInfo, 0, newTemp, 0, newTemp.Length);
                                    characterInfo = newTemp;
                                    Hukiry.HukiryUtilEditor.UndoRecordObject(m_font, "Font_cancle");
                                    EditorUtility.SetDirty(m_font);
                                }
                                EditorGUILayout.EndFadeGroup();
                            }

                            if (EditorGUILayout.BeginFadeGroup(animShowType.faded))
                            if (GUILayout.Button("确认"))
                            {
                                m_font.characterInfo = characterInfo;
                                Hukiry.HukiryUtilEditor.UndoRecordObject(m_font, "Font_ok");
                                EditorUtility.SetDirty(m_font);
                            }
                            EditorGUILayout.EndFadeGroup();
                        }
                        GUI.contentColor = col;
                        index++;
                    }
                }
                while (index < characterInfo.Length);

                if (GUILayout.Button("添加字符"))
                {
                    var newTemp = new CharacterInfo[characterInfo.Length + 1];
                    Array.Copy(characterInfo, 0, newTemp, 0, characterInfo.Length);
                    characterInfo = newTemp;
                }
                EditorGUILayout.EndScrollView();
            }
        }
        GUI.skin.label.fontSize = fontSize;
        GUI.contentColor = col;

        EditorGUILayout.Space();
        Hukiry.HukiryUtilEditor.DrawLine(Color.green, Color.gray, 4, 4);
        EditorGUILayout.Space();
    }

    [MenuItem("CONTEXT/TrueTypeFontImporter/CreateFontAsset", false)]
    static void CreateFontAsset(MenuCommand command)
    {
        TrueTypeFontImporter importer = command.context as TrueTypeFontImporter;
    }
}
