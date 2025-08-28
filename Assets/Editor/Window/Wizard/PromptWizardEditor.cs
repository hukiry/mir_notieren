using UnityEditor;
using UnityEngine;

public class PromptWizardEditor : ScriptableWizard
{
    public string identifier;
     public string productName;
    public int appVersion;
    private void OnEnable()
    {
        identifier = PackingWinConfigAssets.Instance.identifier;
        productName = PackingWinConfigAssets.Instance.productName;
        appVersion = PackingWinConfigAssets.Instance.appVersion;
    }

    private void OnDisable()
    {
        this.position = new Rect(this.position.x, this.position.y, 450, 220);
    }

    private void OnGUI()
    {
        Color color = GUI.color;
        int fontSize = GUI.skin.label.fontSize;
        this.titleContent = new GUIContent($"保存");
        if (EditorApplication.isCompiling)
        {
            EditorGUILayout.HelpBox($"请耐心等待！正在编译中...", MessageType.Warning);
            return;
        }

        if (EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox($"正在运行中... 不可操作", MessageType.Error);
            if (GUILayout.Button("停止播放"))
            {
                EditorApplication.isPlaying = false;
            }
            return;
        }

        GUI.skin.label.fontSize = 18;
        GUILayout.Label("游戏设置");
        GUI.skin.label.fontSize = fontSize;

        GUILayout.Space(15);
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            identifier = EditorGUILayout.TextField("包名：", identifier);
            GUILayout.Space(20);
        }
        GUILayout.Space(10);
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            productName = EditorGUILayout.TextField("游戏名称：", productName);
            GUILayout.Space(20);
        }
        GUILayout.Space(15);

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            appVersion = EditorGUILayout.IntField("App版本号：", appVersion);
            GUILayout.Space(20);
        }
        GUILayout.Space(10);

        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            GUI.color = Color.green;
            if (GUILayout.Button("Save", GUILayout.Height(50)))
            {
                PackingWinConfigAssets.Instance.identifier = identifier;
                PackingWinConfigAssets.Instance.productName = productName;
                PackingWinConfigAssets.Instance.SaveAssets();
                AssetDatabase.Refresh();
                this.Close();
            }

            GUI.color = color;
            GUILayout.Space(20);
            if (GUILayout.Button("Cancel", GUILayout.Height(50)))
            {
                this.Close();
            }
            GUILayout.Space(20);
            GUI.color = color;
        }
    }
}
