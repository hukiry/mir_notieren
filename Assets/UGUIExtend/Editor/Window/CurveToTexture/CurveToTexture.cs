namespace Hukiry.CurveTexture
{
    using UnityEditor;
    using UnityEngine;

    public class CurveToTexture : EditorWindow, IHasCustomMenu
    {
       
        public static void ShowCurveToTexture()
        {
            // Get existing open window or if none, make a new one:
            CurveToTexture window = (CurveToTexture)EditorWindow.GetWindow(typeof(CurveToTexture));
            window.minSize = new Vector2(300, 400);
            window.titleContent = new GUIContent("Shader Texture");
            window.Show();
        }

        CurveScriptObject m_cso;
        CurveScriptObject cso
        {
            get
            {
                if (m_cso == null)
                {
                    m_cso = CreateInstance<CurveScriptObject>();
                }
                return m_cso;
            }
            set
            {
                m_cso = value;
            }
        }

        CurveScriptObjectEditor cttEditor;
        void OnGUI()
        {
            if (cso != null)
            {
                if (cttEditor == null)
                {
                    cttEditor = (CurveScriptObjectEditor)Editor.CreateEditor(cso, typeof(CurveScriptObjectEditor));
                    cttEditor.OnEnable();
                }
            }

            cttEditor.OnInspectorGUI();
            Rect lastTect = GUILayoutUtility.GetLastRect();
            Rect previewRect = position;
            previewRect.height -= lastTect.height;
            cttEditor.DrawCustomPreview(previewRect);
        }


        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(m_SaveSettings, false, MenuItemm_SaveSettings);
            menu.AddItem(new GUIContent("定位脚本"), false, () => {
                Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("CurveToTexture");
            });
        }
        private GUIContent m_SaveSettings = new GUIContent("Save Settings");
        private void MenuItemm_SaveSettings()
        {
            string savePath = EditorUtility.SaveFilePanelInProject("Save Sattings", "CurveToTextureSettings", "asset", "Please enter a file name to save the Settings");
            if (savePath != string.Empty)
            {
                cso = Instantiate(cso);
                AssetDatabase.CreateAsset(cso, savePath);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = cso;
            }
        }
    }
}