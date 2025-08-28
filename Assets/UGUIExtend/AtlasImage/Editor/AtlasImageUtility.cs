using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hukiry.UI
{
    //九宫格数字设置
    class AtlasImageUtility
	{
		static Color blueColor = new Color(0f, 0.7f, 1f, 1f);
		static Color greenColor = new Color(0.4f, 1f, 0f, 1f);
		private static bool m_ischangeGUI;
		static public bool DrawBorder(SpritePreview preview, System.Action OpenWin, System.Action<string> actionInput)
		{
			return DrawBorder(preview, OpenWin, actionInput, null);
		}

		static public bool DrawBorder(SpritePreview preview, System.Action OpenWin, System.Action<string> actionInput, string keyValue)
		{

			EditorGUILayout.Separator();
			bool isDraw = DrawHeader(keyValue != null ? keyValue : "Sprite Border");
			if (isDraw)
			{

				using (new EditorGUILayout.VerticalScope(GUI.skin.box))
				{
					using (new EditorGUILayout.HorizontalScope())
					{
						GUI.backgroundColor = Color.green;
						string spriteName = EditorGUILayout.TextField(preview.sprite?.name);
						if (spriteName != preview.sprite?.name)
						{
							actionInput?.Invoke(spriteName);
						}

						GUI.backgroundColor = Color.white;
						if (GUILayout.Button("Select Sprite"))
						{
							OpenWin?.Invoke();
						}
					}

					EditorGUILayout.Separator();
					GUI.changed = false;
					GUI.backgroundColor = greenColor;
					GUI.backgroundColor = blueColor;
					Vector2Int borderA = IntPair("Border", "Left", "Right", (int)preview.Border.x, (int)preview.Border.z);
					Vector2Int borderB = IntPair(null, "Top", "Bottom", (int)preview.Border.w, (int)preview.Border.y);
					GUI.backgroundColor = Color.white;
					if (GUI.changed)
					{
						RegisterUndo("Atlas Change", preview.sprite as Object);
						preview.Border = new Vector4(borderA.x, borderB.y, borderA.y, borderB.x);
						m_ischangeGUI = true;
					}
					EditorGUILayout.Separator();

					using (new EditorGUILayout.HorizontalScope())
					{
						if (keyValue == null)
							DrawSaveAsTexture(preview);

						
						GUI.contentColor = m_ischangeGUI ? Color.yellow : Color.white;
						if (m_ischangeGUI|| keyValue == null)
						{
							if (GUILayout.Button("Apply"))
							{
								m_ischangeGUI = false;
								preview.ApplySprite();
								GUIUtility.ExitGUI();
							}
						}
						GUI.contentColor = Color.white;
					}
				}
			}
			return isDraw;
		}

		static void DrawSaveAsTexture(SpritePreview preview)
		{
			if (GUILayout.Button("Save As..."))
			{
				if (!preview.sprite.texture.isReadable) LogManager.LogColor("red", preview.sprite, (object)$"请将精灵 {preview.sprite?.name} 修改为可读！");
				string path = EditorUtility.SaveFilePanel("Save As", Application.dataPath + "/../", preview.sprite?.name + ".png", "png");
				if (!string.IsNullOrEmpty(path))
				{
					var bytes = preview.sprite.texture.EncodeToPNG();
					File.WriteAllBytes(path, bytes);
				}
			}
		}

		static void RegisterUndo(string name, Object obj) { if (obj != null) UnityEditor.Undo.RecordObject(obj, name); }

		static Vector2Int IntPair(string prefix, string leftCaption, string rightCaption, int x, int y)
		{
			GUILayout.BeginHorizontal();

			if (string.IsNullOrEmpty(prefix))
			{
				GUILayout.Space(80f);
			}
			else
			{
				GUILayout.Label(prefix, GUILayout.Width(74f));
			}

			EditorGUIUtility.labelWidth = 48f;

			Vector2Int retVal = Vector2Int.zero;
			retVal.x = EditorGUILayout.IntField(leftCaption, x, GUILayout.MinWidth(30f));
			retVal.y = EditorGUILayout.IntField(rightCaption, y, GUILayout.MinWidth(30f));

			EditorGUIUtility.labelWidth = 80f;

			GUILayout.EndHorizontal();
			return retVal;
		}

		public static bool DrawHeader(string text)
		{
			return DrawHeader(text, text, false, false);
		}
		static  bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
		{
			bool state = EditorPrefs.GetBool(key, true);

			if (!minimalistic) GUILayout.Space(3f);
			if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
			else GUI.backgroundColor = new Color(1.1f, 1.1f, 1.1f);

			GUILayout.BeginHorizontal();
			GUI.changed = false;

			if (minimalistic)
			{
				if (state) text = "\u25BC" + (char)0x200a + text;
				else text = "\u25BA" + (char)0x200a + text;

				GUILayout.BeginHorizontal();
				GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
				if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;//"PreToolbar2"
				GUI.contentColor = Color.white;
				GUILayout.EndHorizontal();
			}
			else
			{
				text = "<b><size=11>" + text + "</size></b>";
				if (state) text = "\u25BC " + text;
				else text = "\u25BA " + text;
				if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
			}

			if (GUI.changed) EditorPrefs.SetBool(key, state);

			if (!minimalistic) GUILayout.Space(2f);
			GUILayout.EndHorizontal();
			GUI.backgroundColor = Color.white;
			if (!forceOn && !state) GUILayout.Space(3f);
			return state;
		}
	}
}
