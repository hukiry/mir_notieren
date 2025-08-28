using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq;
using Hukiry;

public class PreferenceSymbolEditor : SettingsProvider
{
	//Preferences/Hukiry
	private static PreferenceSymbolEditor ins { get; } = new PreferenceSymbolEditor("Preferences/Hukiry/Symbol", SettingsScope.User);
	private PreferenceSymbolEditor(string path, SettingsScope scopes) : base(path, scopes)
	{
		//this.label = "宏定义";
		this.guiHandler = this.PreferenceHukiry;
		// Populate the search keywords to enable smart search filtering and label highlighting:
		this.keywords = new HashSet<string>(new[] { "Hukiry", "ShaowHierarchyIcon", "systeminfo", "Symbols" });
	}

	[SettingsProvider]
	private static SettingsProvider CreateSpineSettingsProvider() => ins;


    private bool m_isChange = false;
	private void PreferenceHukiry(string searchContext)
	{
		GUI.contentColor = Color.green;
		if (GUILayout.Button(new GUIContent("  宏定义：", Hukiry.HukiryUtilEditor.GetTexture2D("Arrow.tga"), ""), GUI.skin.textArea))
		{
			Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("SymbolSetting");
			GUI.changed = false;
		}
		GUI.contentColor = Color.white;
		

		SymbolSetting settings = SymbolSetting.Instance;
		EditorGUIUtility.labelWidth = 300;
		
		settings.isShaowHierarchyIcon = this.Toggle(nameof(settings.isShaowHierarchyIcon), string.Empty, settings.isShaowHierarchyIcon);
		settings.isEnableAssetbundleTest = this.Toggle(nameof(settings.isEnableAssetbundleTest), UnitySymbol.ASSETBUNDLE_TEST, settings.isEnableAssetbundleTest);
		settings.isEnableHotUpdate = this.Toggle(nameof(settings.isEnableHotUpdate), UnitySymbol.HOTUPDATE_TEST, settings.isEnableHotUpdate);
		settings.isEnableFps = this.Toggle(nameof(settings.isEnableFps), UnitySymbol.ENABLE_FPS, settings.isEnableFps);
		settings.isEnableWifiView = this.Toggle(nameof(settings.isEnableWifiView), UnitySymbol.SYSTEM_INFO, settings.isEnableWifiView);
		settings.isEnableSdk = this.Toggle(nameof(settings.isEnableSdk), UnitySymbol.ENABLE_SDK, settings.isEnableSdk);
		settings.isEnableLua = this.Toggle(nameof(settings.isEnableLua), UnitySymbol.ENABLE_LUA, settings.isEnableLua);
		settings.isEnableModel = this.Toggle(nameof(settings.isEnableModel), UnitySymbol.MODEL_LAYER, settings.isEnableModel);
		settings.isEnableSocket = this.Toggle(nameof(settings.isEnableSocket), UnitySymbol.ENABLE_SOCKET, settings.isEnableSocket);
		settings.isUseCCharp = this.Toggle(nameof(settings.isUseCCharp), UnitySymbol.USE_CCHARP, settings.isUseCCharp);
		settings.isRelease = this.Toggle(nameof(settings.isRelease), UnitySymbol.RELEASE, settings.isRelease);
		settings.isStrongSocket = this.Toggle(nameof(settings.isStrongSocket), UnitySymbol.STRONG_SOCKET, settings.isStrongSocket);

		if (GUI.changed) m_isChange = true;
		if (m_isChange)
		{
			if (GUILayout.Button("Apply Symbols", "Button"))
			{
				m_isChange = false;
				settings.SaveAssets();
				
				AssetDatabase.Refresh();
			}
		}
	}

	private string GetTooltip(string fieldKey)
	{
		SymbolSetting settings = SymbolSetting.Instance;
		var field = settings.GetType().GetField(fieldKey);
		return field.GetCustomAttribute<TooltipAttribute>()?.tooltip;
	}

	private bool Toggle(string fieldKey, string tooltip, bool isEnable)
	{
		var label = this.GetTooltip(fieldKey);
		GUIContent uIContent = new GUIContent(label, tooltip);
		using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
		{
			bool isOpen =  EditorGUILayout.Toggle(uIContent, isEnable);
			GUILayout.FlexibleSpace();
			if (!string.IsNullOrEmpty(tooltip))
			{
				if (GUILayout.Button("拷贝 " + tooltip))
				{
					
					UnityEditor.EditorGUIUtility.systemCopyBuffer = uIContent.tooltip;
					GUI.changed = false;

				}
			}
			return isOpen;
		}
	}

	#region 所有文件清单

	private class ItemData
	{
		public int fileCount;
		public long fileSize;
		public string ext;
	}
	static Dictionary<string, List<string>> dicPart = new Dictionary<string, List<string>>();
	static Dictionary<string, ItemData> dicItem = new Dictionary<string, ItemData>();
#if !UNITY_2019_1_OR_NEWER
	[PreferenceItem("Resources Information Total")]
#endif
	private static void PreferencesGUI()
	{
		if (dicItem.Count == 0)
		{

			dicPart.Clear();
			dicItem.Clear();

			var array = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
			foreach (var item in array)
			{
				var ext = Path.GetExtension(item);
				if (!dicPart.ContainsKey(ext))
				{
					dicPart[ext] = new List<string>();
				}
				dicPart[ext].Add(item);
			}


			foreach (var item in dicPart)
			{
				long size = 0;
				int length = item.Value.Count;
				for (int i = 0; i < length; i++)
				{
					FileInfo fi = new FileInfo(item.Value[i]);
					size += fi.Length;
				}
				if (!dicItem.ContainsKey(item.Key) && (!string.IsNullOrEmpty(item.Key)))
				{
					dicItem[item.Key] = new ItemData
					{
						fileCount = length,
						fileSize = size,
						ext = item.Key
					};
				}
			}

		}

		if (dicItem.Count > 0)
		{
			//GUI.DrawTexture(new Rect(0, 0, 1024, 960), Hukiry.HukiryUtilEditor.GetTexture2D("item.tga"));
			GUILayout.Label(new GUIContent("开始资源信息统计"));
			ShowResourceInfo();
		}
	}

	private static Color color = new Color(0.5f, 255, 0.5f, 1);
	private static int sortState = 0;
	private static void ShowResourceInfo()
	{
		if (dicItem.Count > 0)
		{
			var list = dicItem.Select(p => p.Value).ToList();
			sortState = EditorGUILayout.Popup(sortState,
				new string[] {
					"按文件后缀名排序",
					"按文件数量排序",
					"按文件文件大小排序"
			});

			if (sortState == 0)
			{
				list.Sort((n, m) => ((int)n.ext[1]) - ((int)m.ext[1]));
			}
			else if (sortState == 1)
			{
				list.Sort((n, m) => n.fileCount - m.fileCount);
			}
			else
			{
				list.Sort((n, m) => (int)(n.fileSize - m.fileSize));
			}


			int len = list.Count;

			long sumSize = 0;
			color = EditorGUILayout.ColorField("Color", color);
			GUI.color = color;
			for (int i = 0; i < len; i++)
			{

				var d = list[i];
				if (!string.IsNullOrEmpty(d.ext))
				{
					GUILayout.BeginHorizontal();
					GUI.skin.label.alignment = TextAnchor.MiddleLeft;
					GUILayout.Label(d.ext, GUILayout.Width(100));

					GUILayout.Label("文件数量：" + d.fileCount, GUILayout.Width(120));
					GUILayout.Box("此后缀名文件总大小：" + GetM(d.fileSize));
					GUI.skin.label.alignment = TextAnchor.MiddleCenter;
					GUILayout.EndHorizontal();
					sumSize += d.fileSize;
				}
				GUILayout.Space(5);
			}

			GUILayout.Label("总大小：" + GetM(sumSize));
			GUI.color = new Color(1, 1, 1, 1);
		}
	}
	private static string GetM(long size)
	{
		float kb = size / 1024f;
		float m = size / 1024f / 1024f;
		float g = size / 1024f / 1024f / 1024f;
		if (g > 1)
		{
			return string.Format("{0:f1}G", g);
		}
		else if (m > 1)
		{
			return string.Format("{0:f1}M", m);
		}
		else if (kb > 1)
		{
			return string.Format("{0:f1}KB", kb);
		}
		else
		{
			return string.Format("{0:f1}B", size);
		}
	} 
	

	//[MenuItem("Hukiry/Memory 图片暂用内存测试")]
	public static void TestMemory()
    {
        Texture target = Selection.activeObject as Texture;
        var type = Type.GetType("UnityEditor.TextureUtil,UnityEditor");
        MethodInfo methodInfo = type.GetMethod("GetStorageMemorySize", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

        if (target)
        {
            Debug.Log("内存占用：" + EditorUtility.FormatBytes(Profiler.GetRuntimeMemorySizeLong(Selection.activeObject)));
            Debug.Log("硬盘占用：" + EditorUtility.FormatBytes((int)methodInfo.Invoke(null, new object[] { target })));
        }
	}
	#endregion

}
