using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Hukiry.UI
{

    //监测面板贴图预览
    internal class SpritePreview
	{

		public Color color = Color.white;

		Vector4 m_Border;

		bool m_EnableBorderEdit = false;


		public Sprite sprite
		{
			get{ return m_Sprite; }
			set
			{
				if (m_Sprite != value)
				{
					m_Sprite = value;
					m_Border = m_Sprite ? m_Sprite.border : Vector4.zero;
				}
			}
		}
		public System.Action onApplyBorder;

		Sprite m_Sprite;


		MethodInfo miDrawSprite = System.Type.GetType("UnityEditor.UI.SpriteDrawUtility, UnityEditor.UI")
			.GetMethod("DrawSprite",
			                          BindingFlags.NonPublic | BindingFlags.Static,
			                          null,
			                          new System.Type[]{ typeof(Texture), typeof(Rect), typeof(Vector4), typeof(Rect), typeof(Rect), typeof(Rect), typeof(Color), typeof(Material) },
			                          null
		                          );

		MethodInfo mCreateCheckerTex = System.Type.GetType("UnityEditor.UI.SpriteDrawUtility, UnityEditor.UI").GetMethod("CreateCheckerTex",
					  BindingFlags.NonPublic | BindingFlags.Static,
					  null,
					  new System.Type[] { typeof(Color), typeof(Color) },
					  null
				  );

		FieldInfo m_ContrastTex = System.Type.GetType("UnityEditor.UI.SpriteDrawUtility, UnityEditor.UI").GetField("s_ContrastTex", BindingFlags.NonPublic | BindingFlags.Static);


		public AtlasImage m_AtlasImage;
        private bool m_EnableCommondEdit;

        public GUIContent GetPreviewTitle()
		{
			return new GUIContent(sprite ? sprite.name : "-");
		}

		public void DrawSprite(Rect drawArea, Vector4 border)
		{
			if (sprite == null)
				return;

			Texture2D tex = sprite.texture;
			if (tex == null)
				return;

			m_ContrastTex?.SetValue(null, mCreateCheckerTex?.Invoke(null, new object[] { new Color(0f, 0f, 0f, 0.3f), new Color(0, 1f, 0, 0.7f) }));
			Rect outer = sprite.rect;
			Rect inner = outer;
			inner.xMin += border.x;
			inner.yMin += border.y;
			inner.xMax -= border.z;
			inner.yMax -= border.w;

			Vector4 uv4 = UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
			Rect uv = new Rect(uv4.x, uv4.y, uv4.z - uv4.x, uv4.w - uv4.y);
			Vector4 padding = UnityEngine.Sprites.DataUtility.GetPadding(sprite);
			padding.x /= outer.width;
			padding.y /= outer.height;
			padding.z /= outer.width;
			padding.w /= outer.height;

			if ((m_AtlasImage!=null&&!m_AtlasImage.IsGray) || Application.isPlaying)
			{
				miDrawSprite.Invoke(null, new object[] { tex, drawArea, padding, outer, inner, uv, color, null });
			}
			else
			{
				miDrawSprite.Invoke(null, new object[] { tex, drawArea, padding, outer, inner, uv, color, m_AtlasImage?.material });
			}
            m_ContrastTex?.SetValue(null, mCreateCheckerTex?.Invoke(null, new object[] { new Color(0f, 0f, 0f, 0.5f), new Color(1f, 1f, 1f, 0.5f) }));
        }


		public void OnPreviewGUI(Rect rect, bool isDraw = true)
		{
			DrawSprite(rect, m_Border);
			DrawContextEditWindowContext(rect);
			if (isDraw)
			{
				bool isClick = GUI.Button(new Rect(rect.x + rect.width - 21, rect.y + rect.height - 21, 20, 20), 
					new GUIContent(Hukiry.HukiryUtilEditor.GetTexture2D("sprite.tga")), GUIStyle.none);
				if (isClick)
				{
					m_EnableBorderEdit = !m_EnableBorderEdit;
				}
				DrawBorderEditWindow(rect);
			}
		}

		void DrawContextEditWindowContext(Rect rect)
		{
			if (!m_EnableCommondEdit)
				return;
			Rect boxRect = new Rect(rect.x + rect.width - 120, rect.y - 3, 124, 104);//90
			GUI.Box(boxRect, "", "helpbox");

			float labelWidth = EditorGUIUtility.labelWidth;
			int fontSize = EditorStyles.toolbarButton.fontSize;
			float fixedHeight = EditorStyles.toolbarButton.fixedHeight;
			{

				EditorGUIUtility.labelWidth = 40;

				if (m_AtlasImage)
				{

					EditorStyles.toolbarButton.fontSize = 12;
					EditorStyles.toolbarButton.fixedHeight = 24;
					EditorStyles.toolbarButton.fontStyle = FontStyle.Bold;
					Rect elementRect = new Rect(boxRect.x + 2, boxRect.y + 3, boxRect.width - 6, 24);
					elementRect = MiniButton(elementRect, "拷贝 SpriteName", () =>
					{
						Hukiry.HukiryUtilEditor.InvokeInstance(m_AtlasImage, "CopySpriteName");
						m_EnableCommondEdit = false;
					});
					elementRect = MiniButton(elementRect, "粘贴 SpriteName", () =>
					{
						Hukiry.HukiryUtilEditor.InvokeInstance(m_AtlasImage, "PasteSpriteName");
						m_EnableCommondEdit = false;
					});
					elementRect = MiniButton(elementRect, "定位 SpriteAtlas", () =>
					{
						Hukiry.HukiryUtilEditor.InvokeInstance(m_AtlasImage, "PingAtlas");
						m_EnableCommondEdit = false;
					});
					MiniButton(elementRect, "打包 SpriteAtlas", () =>
					{
						Hukiry.HukiryUtilEditor.InvokeInstance(m_AtlasImage, "PackingAtlas");
						m_EnableCommondEdit = false;
					});
				}

			}
			EditorStyles.toolbarButton.fixedHeight = fixedHeight;
			EditorStyles.toolbarButton.fontStyle = FontStyle.Normal;
			EditorStyles.toolbarButton.fontSize = fontSize;
			EditorGUIUtility.labelWidth = labelWidth;
		}

		Rect MiniButton(Rect rect, string label, System.Action action)
		{
			if (GUI.Button(rect, label, EditorStyles.toolbarButton))
			{
				action?.Invoke();
			}
			rect.y += rect.height + 1;
			return rect;
		}

        void DrawBorderEditWindow(Rect rect)
        {
            if (!m_EnableBorderEdit)
                return;

            Rect boxRect = new Rect(rect.x + rect.width - 70, rect.y + rect.height - 103, 70, 80);
            GUI.Box(boxRect, "", "helpbox");

            float labelWidth = EditorGUIUtility.labelWidth;
            int fontSize = EditorStyles.label.fontSize;
            {

                EditorGUIUtility.labelWidth = 40;
                EditorStyles.label.fontSize = 9;
                Rect elementRect = new Rect(boxRect.x + 2, boxRect.y + 3, boxRect.width - 6, 14);

                elementRect = MiniIntField(elementRect, "Left", ref m_Border.x);
                elementRect = MiniIntField(elementRect, "Right", ref m_Border.z);
                elementRect = MiniIntField(elementRect, "Top", ref m_Border.w);
                elementRect = MiniIntField(elementRect, "Bottom", ref m_Border.y);

                if (GUI.Button(elementRect, "Apply", "minibutton"))
                {
                    m_EnableBorderEdit = false;
                    ApplyBorder();
                }

            }
            EditorStyles.label.fontSize = fontSize;
            EditorGUIUtility.labelWidth = labelWidth;
        }


        Rect MiniIntField(Rect rect, string label, ref float value)
        {
            value = Mathf.Max(0, EditorGUI.IntField(rect, label, (int)value, EditorStyles.miniTextField));
            rect.y += rect.height + 1;
            return rect;
        }

        void ApplyBorder()
		{
			bool isDirty = false;
			TextureImporter t = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(m_Sprite)) as TextureImporter;

			switch (t.spriteImportMode) {
			case SpriteImportMode.Single:
				t.spriteBorder = m_Border;
				isDirty = true;
				break;
			case SpriteImportMode.Multiple:
				SpriteMetaData[] spritesheet = t.spritesheet;
				for (int i = 0; i < spritesheet.Length; i++)
				{
					if (spritesheet[i].name == m_Sprite.name)
					{
						spritesheet[i].border = m_Border;
						isDirty = true;
					}
				}
				t.spritesheet = spritesheet;
				break;
			}

			if (isDirty) {
				EditorUtility.SetDirty(t);
				t.SaveAndReimport();

				if (onApplyBorder != null) {
					onApplyBorder();
				}
			}
		}

		public string GetInfoString()
		{
			return m_Sprite ? string.Format("{0} : {1}x{2}", m_Sprite.name, Mathf.RoundToInt(m_Sprite.rect.width), Mathf.RoundToInt(m_Sprite.rect.height)) : "";
		}

		public void OnPreviewSettings()
		{
			m_EnableCommondEdit = GUILayout.Toggle(m_EnableCommondEdit, " Command   ", "PreButton");
		}

		public Vector4 Border { get { return m_Border; } set { m_Border = value; } }
		public void ApplySprite()
		{
			this.ApplyBorder();
		}
	}
}