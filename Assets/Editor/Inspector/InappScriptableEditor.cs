using Hukiry.SDK;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(InappScriptableData))]
public class InappScriptableEditor : Editor
{
    InappScriptableData data;
    private Vector2 scrollPosition;
    private Vector2 scrollPositionShow;
    private void OnEnable()
    {
        data = target as InappScriptableData;
    }

    public override void OnInspectorGUI()
    {
        if (EditorApplication.isCompiling) return;
        if (data == null || data.details == null)
        {
            GUILayout.Box("数据异常，需要检查");
            return;
        }
        var fontSize = GUI.skin.label.fontSize;
        GUI.skin.label.fontSize = 16;
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box, GUILayout.Height(Screen.height-500));
        {
            for (int i = 0; i < data.details.Count; i++)
            {
                var p = data.details[i];
                using (new GUILayout.VerticalScope(GUI.skin.box))
                {
                    int.TryParse(p.amount, out int result);
                    var color = GUI.color;
                    GUI.color = p.isEnable?Color.green: Color.white;
                    p.isEnable = EditorGUILayout.Toggle($"${(result / 100F).ToString("f2")}", p.isEnable);
                    GUI.color = color;
                    if (EditorGUILayout.Foldout(p.isEnable, "   _________________________________________________________________________________________________________________________", GUIStyle.none))
                    {
                        p.amount = EditorGUILayout.IntField("   价格", result).ToString();
                        EditorGUILayout.LabelField("   id", $"{PackingWinConfigAssets.Instance.identifier}{p.amount}");
                        p.product_id = PackingWinConfigAssets.Instance.identifier;
                    }
                    EditorGUILayout.Space(2);
                }
            }
        }
        GUILayout.EndScrollView();
        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            var count = data.details.Where(p => p.isEnable).Count();
            EditorGUILayout.LabelField("启动内购总档次：", $"{count}");
            var height = Mathf.Clamp(count * 16, 20, 96);
            scrollPositionShow = GUILayout.BeginScrollView(scrollPositionShow, GUI.skin.box, GUILayout.Height(height));
            {
                GUILayout.TextArea(string.Join("\n", data.GetProductList()));
            }
            GUILayout.EndScrollView();
        }

        if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            if (data)
            {
                EditorUtility.SetDirty(data);
                AssetDatabase.SaveAssets();
            }
        }
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("需要将数据挂载到脚本中，或者拷贝内购id 到Lua中", MessageType.Info);


        GUI.skin.label.fontSize = fontSize;
    }
}
