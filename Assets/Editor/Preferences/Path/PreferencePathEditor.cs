using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PreferencePathEditor : SettingsProvider
{
	//Preferences/Hukiry
	private static PreferencePathEditor ins { get; } = new PreferencePathEditor("Preferences/Hukiry/Path", SettingsScope.User);
	[SettingsProvider]
	private static SettingsProvider CreateSpineSettingsProvider() => ins;
	private PreferencePathEditor(string path, SettingsScope scopes) : base(path, scopes)
	{
		this.guiHandler = this.PreferenceHukiry;
		this.keywords = new HashSet<string>(new[] { "Hukiry", "Path" });
	}

	private string projectPath = Application.dataPath.Replace("Assets", "");

	private bool m_isChange = false;
	private void PreferenceHukiry(string searchContext)
	{
		GUI.contentColor = Color.green;
		if (GUILayout.Button(new GUIContent("  路径配置：", Hukiry.HukiryUtilEditor.GetTexture2D("Arrow.tga"), ""), GUI.skin.textArea))
		{
			Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("PathSettings");
			GUI.changed = false;
		}
		GUI.contentColor = Color.white;

		EditorGUILayout.LabelField($"{projectPath}");


		PathSettings settings = PathSettings.Instance;
		settings.LuaCodeDirPath = this.TextField(nameof(settings.LuaCodeDirPath), "Lua目录路径", settings.LuaCodeDirPath);
		settings.CharpCodeDirPath = this.TextField(nameof(settings.CharpCodeDirPath), "C#目录路径", settings.CharpCodeDirPath);


		if (GUI.changed) m_isChange = true;
		if (m_isChange)
		{
			if (GUILayout.Button("Apply Path", "Button"))
			{
				m_isChange = false;
				settings.SaveAssets();
			}
		}
	}

	private string GetTooltip(string fieldKey)
	{
		PathSettings settings = PathSettings.Instance;
		var field = settings.GetType().GetField(fieldKey);

		return field.GetCustomAttribute<TooltipAttribute>()?.tooltip;
	}

	private string TextField(string fieldKey, string tooltip, string value)
	{
		var label = this.GetTooltip(fieldKey);
		GUIContent uIContent = new GUIContent(label, tooltip);
		EditorGUILayout.LabelField(uIContent);
		using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
		{
			string result = EditorGUILayout.TextField(value);
			if (GUILayout.Button("Browse", GUILayout.Width(100)))
			{
				string folder = Application.dataPath;
				if (!string.IsNullOrEmpty(value))
				{
					folder = value.Replace(projectPath, "");
				}

				string dirPath = EditorUtility.OpenFolderPanel("Unity", folder, "");
				if (!string.IsNullOrEmpty(dirPath))
				{
					result = dirPath;
				}
				else
				{
					GUI.changed = false;
				}
			}
			return result;
		}
	}
}
