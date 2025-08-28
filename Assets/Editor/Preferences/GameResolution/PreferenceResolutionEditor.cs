using Hukiry.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PreferenceResolutionEditor : SettingsProvider
{
	//Preferences/Hukiry
	private static PreferenceResolutionEditor ins { get; } = new PreferenceResolutionEditor("Preferences/Hukiry/Resolution", SettingsScope.User);
	[SettingsProvider]
	private static SettingsProvider CreateSpineSettingsProvider() => ins;
	private PreferenceResolutionEditor(string path, SettingsScope scopes) : base(path, scopes)
	{
		this.guiHandler = this.PreferenceHukiry;
		this.keywords = new HashSet<string>(new[] { "Hukiry", "Resolution" });
	}

	private string projectPath = Application.dataPath.Replace("Assets", "");

	private bool m_isChange = false;
    private int indexResolution;

    private void PreferenceHukiry(string searchContext)
	{
		GUI.contentColor = Color.green;
		if (GUILayout.Button(new GUIContent("  Game Resolution Configuration：", Hukiry.HukiryUtilEditor.GetTexture2D("Arrow.tga"), ""), GUI.skin.textArea))
		{
			Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("ResolutionSetting");
			GUI.changed = false;
		}
		GUI.contentColor = Color.white;


		ResolutionSetting settings = ResolutionSetting.Instance;
		settings.OnStart();
		indexResolution = DrawList("游戏分辨率配置", settings.m_Custom, nameof(settings.m_Custom), indexResolution);

		if (GUI.changed) m_isChange = true;
		if (m_isChange)
		{
			if (GUILayout.Button("Apply Resolution", "Button"))
			{
				m_isChange = false;
				settings.SaveAssets();
			}
		}
	}


	private int DrawList(string label, List<GameResolutionSize> valueList, string key, int index)
	{
		using (new EditorGUILayout.VerticalScope(GUI.skin.box))
		{
			GUI.color = Color.green;
			if (GUILayout.Button(label))
			{
				index = -1;
				PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0) == 1 ? 0 : 1);
				GUI.changed = false;
			}
			GUI.color = Color.white;

			if (PlayerPrefs.GetInt(key, 0) == 1)
			{
				int count = valueList.Count;
				for (int i = 0; i < valueList.Count; i++)
				{
					GUI.color = index == i ? Color.white * 1.2F : Color.white * 0.8F;
					var p = valueList[i];
					using (new EditorGUILayout.HorizontalScope())
					{

						int k = i;
						var tule = ButtonLayout(i, new GUIContent(Hukiry.HukiryUtilEditor.GetTexture2D("AssetLabelIcon")))();
						if (tule.isOk)
						{
							index = index != tule.index?tule.index:-1;
							GUI.changed = false;
						}

						using (new EditorGUILayout.VerticalScope())
						{
							
							using (new EditorGUILayout.HorizontalScope())
							{
								EditorGUILayout.LabelField("Label", GUILayout.Width(100));
								p.baseText = EditorGUILayout.TextField(p.baseText);
							}
							using (new EditorGUILayout.HorizontalScope())
							{
								EditorGUILayout.LabelField("Width & Height",GUILayout.Width(100));
								p.width = EditorGUILayout.IntField(p.width);
								p.height = EditorGUILayout.IntField(p.height);
							}	
						}
							
					}
					GUI.color = Color.white;
				}

				using (new EditorGUILayout.HorizontalScope())
				{
					GUILayout.FlexibleSpace();
					if (index >= 0)
					{
						if (GUILayout.Button(Hukiry.HukiryUtilEditor.GetTexture2D("d_ol_minus"),  GUILayout.Height(20)))
						{
							valueList.RemoveAt(index);
							ResolutionSetting.Instance.Remove(index);
							index = valueList.Count - 1;
						}
					}
					if (GUILayout.Button(Hukiry.HukiryUtilEditor.GetTexture2D( "d_ol_plus"), GUILayout.Height(20)))
					{
						valueList.Add(new GameResolutionSize());
					}
				}
			}
		}
		return index;
	}
	private Func<(int index, bool isOk)> ButtonLayout(int index, GUIContent label, int width = 30)
	{
		return new Func<(int, bool)>(() => (index, GUILayout.Button(label, GUILayout.Width(width))));
	}

}

