using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hukiry
{
	public class EditorStyleViewer : EditorWindow, IHasCustomMenu
	{
		const int CELL = 4;
		Vector2 scrollPosition = new Vector2(0, 0);
		string search = "";
		const int SpaceSize = 16;

		public static void ShowEditorStyleViewer()
		{
			EditorStyleViewer window = (EditorStyleViewer)EditorWindow.GetWindow(typeof(EditorStyleViewer));
			window.titleContent = new GUIContent(" Editor Style", Hukiry.HukiryUtilEditor.GetTexture2D("d_UnityLogo"));
		}

		List<ResourcesIcon> m_styleIcon = new List<ResourcesIcon>();
        private string lastSearch;

        private List<ResourcesIcon> GetUnityEditorStyle()
        {
			List<ResourcesIcon> temp = new List<ResourcesIcon>();

			foreach (GUIStyle style in GUI.skin.customStyles)
			{
				if (style.name.ToLower().IndexOf(search.ToLower())>=0)
				{
					temp.Add(new ResourcesIcon() { style = style, iconName = style.name });
				}
			}

            temp.Sort();
            Resources.UnloadUnusedAssets();
			System.GC.Collect();
			Repaint();
			return temp;
		}

        void OnGUI()
		{
			if (m_styleIcon.Count == 0|| lastSearch != search)
			{
				lastSearch = search;
				m_styleIcon = GetUnityEditorStyle();
			}

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Space(84f);
				search = EditorGUILayout.TextField("", search, "SearchTextField");
				if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f)))
				{
					search = "";
					GUIUtility.keyboardControl = 0;
				}
				GUILayout.Space(84f);
			}

			scrollPosition = GUILayout.BeginScrollView(scrollPosition, "gridList");
			//内置图标
			for (int i = 0; i < m_styleIcon.Count; i += CELL)
			{
				GUILayout.Space(SpaceSize);
				using (new GUILayout.HorizontalScope())
				{
					for (int j = 0; j < CELL; j++)
					{
						int index = i + j;
						if (index < m_styleIcon.Count)
						{
							GUILayout.Space(SpaceSize);
							if (GUILayout.Button(m_styleIcon[index].iconName, m_styleIcon[index].style))
							{
								EditorGUIUtility.systemCopyBuffer = m_styleIcon[index].iconName;
								this.ShowNotification(new GUIContent( m_styleIcon[index].iconName));
							}
							GUILayout.Space(SpaceSize);
						}
					}
				}
				GUILayout.Space(SpaceSize);
			}
			GUILayout.EndScrollView();
		}

        public void AddItemsToMenu(GenericMenu menu)
        {
			menu.AddItem(new GUIContent("定位脚本"), false, () => {
				Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("EditorStyleViewer");
			});
		}
    }
}