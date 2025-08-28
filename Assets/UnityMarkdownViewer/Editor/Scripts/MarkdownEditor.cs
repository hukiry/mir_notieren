using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Hukiry.MDV
{
    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownEditor : Editor
    {
        public GUISkin SkinLight;
        public GUISkin SkinDark;

        MarkdownViewer mViewer;

        private static List<string> mExtensions = new List<string> { ".md", ".markdown" };

        protected void OnEnable()
        {
            var content = ( target as TextAsset ).text;
            var path    = AssetDatabase.GetAssetPath( target );

            var ext = Path.GetExtension( path ).ToLower();

            // 未知原因可能导致丢失样式，手动赋值下
            SkinLight = GetAssetObject<GUISkin>("MarkdownViewerSkin");
            SkinDark = GetAssetObject<GUISkin>("MarkdownSkinQS"); 
            if ( mExtensions.Contains( ext ) )
            {
                mViewer = new MarkdownViewer( Preferences.DarkSkin ? SkinDark : SkinLight, path, content );
                EditorApplication.update += UpdateRequests;
            }
        }

        public static T GetAssetObject<T>(string fileName) where T : UnityEngine.Object
        {
            var assetPathArray = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x));
            foreach (var path in assetPathArray)
            {
                var assetName = Path.GetFileNameWithoutExtension(path);
                if (assetName == fileName)
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                }
            }
            return default;
        }

        protected void OnDisable()
        {
            if( mViewer != null )
            {
                EditorApplication.update -= UpdateRequests;
                mViewer = null;
            }
        }

        void UpdateRequests()
        {
            if( mViewer != null && mViewer.Update() )
            {
                Repaint();
            }
        }


        //------------------------------------------------------------------------------

        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected override void OnHeaderGUI()
        {
#if UNITY_2019_2_OR_NEWER && !UNITY_2020_1_OR_NEWER
            // TODO: workaround for bug in 2019.2
            // https://forum.unity.com/threads/oninspectorgui-not-being-called-on-defaultasset-in-2019-2-0f1.724328/
            DrawEditor();
#endif
        }

        public override void OnInspectorGUI()
        {
#if !UNITY_2019_2_OR_NEWER || UNITY_2020_1_OR_NEWER
            DrawEditor();
#endif
        }


            //------------------------------------------------------------------------------

        private Editor mDefaultEditor;

        void DrawEditor()
        {
            if( mViewer != null )
            {
                mViewer.Draw();
            }
            else
            {
                DrawDefaultEditor();
            }
        }

        void DrawDefaultEditor()
        {
            if( mDefaultEditor == null )
            {
                mDefaultEditor = CreateEditor( target, Type.GetType( "UnityEditor.TextAssetInspector, UnityEditor" ) );
            }

            if( mDefaultEditor != null )
            {
                mDefaultEditor.OnInspectorGUI();
            }
        }
    }
}
