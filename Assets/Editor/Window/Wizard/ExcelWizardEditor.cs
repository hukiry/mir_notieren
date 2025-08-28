using UnityEditor;
using UnityEngine;

public class ExcelWizardEditor : ScriptableWizard
{

    [SerializeField] private string excelText = string.Empty;
    [SerializeField] private string ChangeText = string.Empty;
    [SerializeField] private int selectIndex = 0;
    [SerializeField] private string[] array = new string[] { ";", "|", "-", "=" };
    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Excel文本编号分割，输入分割的文本点击按钮即可", MessageType.Info);
        excelText = EditorGUILayout.TextField(  "输入需要分割的文本",excelText);
        selectIndex = EditorGUILayout.Popup("字符分割符", selectIndex, array);
        ChangeText = EditorGUILayout.TextField("转换后的文本：", ChangeText);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("转换"))
        {
            string[] temp = excelText.Split('\n', ' ');
            string s = string.Empty;
            for (int i = 0; i < temp.Length; i++)
            {
                if (!string.IsNullOrEmpty(temp[i].Trim()))

                {
                    s += temp[i].Trim() + array[selectIndex];
                }

            }
            ChangeText = s.Substring(0, s.Length - 1);
        }

        if (GUILayout.Button("拷贝"))
        {
            UnityEngine.GUIUtility.systemCopyBuffer = ChangeText;
        }
        GUILayout.EndHorizontal();
    }
}