using System.IO;
using UnityEditor;
using UnityEngine;

public class FontWindowEditor : CreateWindowEditor<FontWindowEditor>
{
    [System.Serializable]
    public class FontData
    {
        [FieldName("图集贴图")]
        public Texture2D texture;
        [FieldName("创建的字体文件名")]
        public string CreateFileName="New Font";
        [FieldName("字体行间距")]
        public float lineSpace=1;
    }

    [SerializeField]
    FontData m_fontData=new FontData ();
    public override void DrawGUI()
    {
        m_fontData = Hukiry.Editor.HukiryGUIEditor.DrawType(m_fontData);

        
        if (m_fontData != null && m_fontData.texture != null)
        {
            m_fontData.CreateFileName = m_fontData.texture.name;
            if (GUILayout.Button("创建字体"))
            {
                string assetPath = AssetDatabase.GetAssetPath(m_fontData.texture.GetInstanceID());
                string saveFilePath = EditorUtility.SaveFilePanel(
                      "保存字体",
                      Path.GetDirectoryName(assetPath).Replace("atlas","font"),
                      m_fontData.CreateFileName,
                      "fontsettings");

                if (!string.IsNullOrEmpty(saveFilePath))
                {
                    saveFilePath = saveFilePath.Replace(Application.dataPath, "Assets");
                    saveFilePath = saveFilePath.Split('.')[0];
                    //FontOfCreateSprite.CreateFontSprite(assetPath, saveFilePath, m_fontData.lineSpace);
                }
            }
        }
    }

    public override Color TitleColor()
    {
        return new Color(255, 255, 255, 255);
    }
}
