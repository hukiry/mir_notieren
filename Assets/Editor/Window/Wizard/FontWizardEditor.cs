using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.U2D;
using Hukiry.UI;
using UnityEditorInternal;

public class FontWizardEditor : ScriptableWizard
{
	[SerializeField] private Font m_font;
	[SerializeField] private SpriteAtlas m_SpriteJsonData;
	[SerializeField] private GameObject m_monoData;
	[SerializeField] private MonoScript m_monoTargetData;
	[SerializeField] private GameObject m_GameObject;

	private List<SelectPrefab> dicDir =new List<SelectPrefab>();
	private List<PrefabInfo> m_prefabs = new List<PrefabInfo>();
	[SerializeField]
	private List<CommponentInfo> m_coms = new List<CommponentInfo>();

	private bool m_isSelected = false;
	private const string PrefabDirPath = "Assets/ResourcesEx/prefab/UI/";
	private Vector2 scrollPosition;
	private ReplaceType m_replaceType = ReplaceType.FontConvert;
	public static bool IsSelectAll;

    private void OnDisable()
    {
		m_coms.Clear();

	}

    private void OnEnable()
    {
		this.position = new Rect(this.position.x, this.position.y, 550, 360);
	}

    private void OnGUI()
	{
	
		string titleName = GetStringByEnum(m_replaceType);
		this.titleContent = new GUIContent($"预制件{titleName}");
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
		EditorGUILayout.HelpBox($"界面预制件{titleName}修改", MessageType.Info);
		LayoutEnumGui();
		int Count = dicDir.Count;
		const int CELL_NUM = 5;
		if (Count > 0)
		{
			for (int i = 0; i < Count; i++)
			{
				if (i % CELL_NUM == 0) //0,3
				{
					EditorGUILayout.BeginHorizontal();
				}

				var selectItem = dicDir[i];
				selectItem.isSelected = EditorGUILayout.ToggleLeft(selectItem.dirName, selectItem.isSelected, GUILayout.Width(100));

				if ((i + 1) % CELL_NUM == 0 || i == Count - 1)
				{
					EditorGUILayout.EndHorizontal();
				}

				if (selectItem.isSelected)
				{
				    m_isSelected = true;
					if (GUI.changed)
					{
						m_prefabs = selectItem.prefabInfos;
						for (int j = 0; j < dicDir.Count ; j++)
						{
							dicDir[j].isSelected = i==j;
						}

						m_prefabs.ForEach(p => p.isReplace = false);
					}
				}
			}
		}

		LayoutOk(titleName);
		
		int length = m_prefabs.Count;
		if (length > 0)
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			for (int i = 0; i < length; i++)
			{
				var pre = m_prefabs[i];
				EditorGUILayout.BeginHorizontal();
				GUI.color = pre.isReplace ? Color.green : Color.red;
				EditorGUILayout.LabelField(pre.prefabName);
				if (GUILayout.Button(pre.isReplace ? "×" : "√"))
				{
					pre.isReplace = !pre.isReplace;
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
		}

		this.Repaint();
	}

	private void LayoutOk(string titleName)
	{
		const string parentName_ = "prefabParent";
		if (m_replaceType != ReplaceType.ComponentConvert)
		{
			EditorGUILayout.Space();
			IsSelectAll = EditorGUILayout.ToggleLeft("是全选", IsSelectAll);
			EditorGUILayout.Space();
			if (IsSelectAll)
			{
				if (m_prefabs.Find(p => !p.isReplace) != null)
				{
					m_prefabs.ForEach(p => p.isReplace = true);
				}
			}
		}

		if (GUILayout.Button("确定替换"))
		{
			bool isOk=EditorUtility.DisplayDialog(titleName, $"您是否立马{titleName}!", "确定");
			if (!isOk)
			{
				return;
			}

			int len = m_prefabs.Count;
			if ((m_font != null && m_replaceType == ReplaceType.FontConvert) ||
				(len > 0 && m_replaceType == ReplaceType.AtlasConvert))
			{
				var parent = new GameObject(parentName_).transform;
				for (int i = 0; i < len; i++)
				{
					var pre = m_prefabs[i];
					if (pre.isReplace)
					{
#if !UNITY_2018_4_5 || UNITY_2019_4
						var intanceprefab = PrefabUtility.InstantiatePrefab(pre.gameObject, parent) as GameObject;

						if (m_replaceType == ReplaceType.FontConvert)
						{
							this.LoopReplaceFont(intanceprefab.transform, m_font);
						}
						else if (m_replaceType == ReplaceType.AtlasConvert)
						{
							var texArrray = intanceprefab.GetComponentsInChildren<AtlasImage>();
							if (texArrray != null && texArrray.Length > 0)
							{
								for (int j = 0; j < texArrray.Length; j++)
								{
									texArrray[j].spriteAtlas = m_SpriteJsonData;
								}
							}
						}

						pre.isReplace = false;
						EditorUtility.DisplayProgressBar(titleName, pre.assetPath, (float)i / (float)len);
						PrefabUtility.SaveAsPrefabAsset(intanceprefab, pre.assetPath, out bool success);
						if (!success)
						{
							LogManager.LogColor("red", "修改失败", intanceprefab.name, pre.assetPath);
						}
						else
						{
							LogManager.LogObject("green", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pre.assetPath), pre.assetPath);
						}
#endif
					}
				}

				AssetDatabase.SaveAssets();

				int childCount = parent.childCount;
				for (int i = childCount - 1; i >= 0; i--)
				{
					GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
					EditorUtility.DisplayProgressBar("保存中", "..." + i, (float)i / (float)childCount);

					if (i == 0)
					{
						GameObject.DestroyImmediate(parent.gameObject);
					}
				}
				this.ShowNotification(new GUIContent("Finished"));
			}
			else if (m_replaceType == ReplaceType.ComponentConvert)
			{
				if (m_monoData != null && m_GameObject != null&& m_monoTargetData!=null)
				{
					this.StartReplaceComponent();
				}
				else {
					LogManager.LogColor("red", m_GameObject == null ?  "未选中替换的对象": "未选择指定的组件" );
				}
			}
			else
			{
				LogManager.LogColor("red", m_font == null ? "未选择指定的字体" : "未选中替换的项");
			}
			EditorUtility.ClearProgressBar();
		}
	}

	private void StartReplaceComponent()
	{
		var monoBehaviour = m_coms.Single(p => p.isReplace);
		if (PrefabUtility.IsPartOfPrefabAsset(m_GameObject))
		{
			var comArrray = m_GameObject.GetComponentsInChildren(monoBehaviour.monoBehaviour.GetType());
			foreach (var item in comArrray)
			{
				ComponentUtility.CopyComponent(item);
				GameObject.DestroyImmediate(item);
				var newCom = item.gameObject.AddComponent(m_monoTargetData.GetClass());
				ComponentUtility.PasteComponentValues(newCom);
			}
			PrefabUtility.SaveAsPrefabAsset(m_GameObject, AssetDatabase.GetAssetPath(m_GameObject));
		}
		else
		{
			var comArrray = m_GameObject.GetComponents(monoBehaviour.monoBehaviour.GetType());
			foreach (var item in comArrray)
			{
				ComponentUtility.CopyComponent(item);
				GameObject.DestroyImmediate(item);
				var newCom = m_GameObject.AddComponent(m_monoTargetData.GetClass());
				ComponentUtility.PasteComponentValues(newCom);
			}
		}
		this.ShowNotification(new GUIContent("Finished"));
	}

	private void LayoutEnumGui()
	{
		m_replaceType = (ReplaceType)EditorGUILayout.EnumPopup("ReplaceType", m_replaceType);

		if (m_replaceType == ReplaceType.FontConvert)
		{
			m_font = EditorGUILayout.ObjectField("替换当前字体文件", m_font, typeof(Font), true) as Font;
		}
		else if (m_replaceType == ReplaceType.AtlasConvert)
		{
			m_SpriteJsonData = EditorGUILayout.ObjectField("替换当前图集文件", m_SpriteJsonData, typeof(SpriteAtlas), true) as SpriteAtlas;
		}
		else if (m_replaceType == ReplaceType.ComponentConvert)
		{
			dicDir.Clear(); m_prefabs.Clear();
			EditorGUILayout.Space();
			m_GameObject = EditorGUILayout.ObjectField("当前对象文件", m_GameObject, typeof(GameObject), true) as GameObject;
			EditorGUILayout.Space();
			m_monoData = EditorGUILayout.ObjectField("当前对象组件", m_monoData, typeof(GameObject), true) as GameObject;
			if (m_monoData != CommponentInfo.lastObj)
			{
				CommponentInfo.lastObj = m_monoData;
				m_coms.Clear();
				if (m_monoData)
				{
					var MonoBehaviours = m_monoData.GetComponents<Behaviour>();
					foreach (var item in MonoBehaviours)
					{
						m_coms.Add(new CommponentInfo() { comName = item.GetType().Name, monoBehaviour = item });
					}
				}
			}

			if (CommponentInfo.lastObj && m_coms.Count>0)
			{
                foreach (var item in m_coms)
                {
					EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
					item.isReplace = EditorGUILayout.ToggleLeft(item.comName, item.isReplace, GUILayout.Width(100));
					EditorGUILayout.ObjectField(item.monoBehaviour, typeof(MonoBehaviour), true);
					if (item.isReplace)
					{
						m_coms.ForEach(p => p.isReplace = p== item);
					}
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.Space();
			m_monoTargetData = EditorGUILayout.ObjectField("替换目标组件", m_monoTargetData, typeof(MonoScript), true) as MonoScript;
			EditorGUILayout.Space();
		}

		if (dicDir.Count == 0&& m_replaceType != ReplaceType.ComponentConvert)
		{
			LoadPrefab();
		}

	}

	private string GetStringByEnum(ReplaceType m_replaceType)
	{
		switch (m_replaceType)
		{
			case ReplaceType.FontConvert:
				return "字体替换";
			case ReplaceType.AtlasConvert:
				return "图集替换";
			case ReplaceType.ComponentConvert:
				return "组件替换";
		}
		return "文件替换";
	}

	private void LoadPrefab()
	{
		if (Directory.Exists(PrefabDirPath))
		{
			string[] arrayDir = Directory.GetDirectories(PrefabDirPath);
			foreach (var item in arrayDir)
			{
				var dirName = item.Substring(item.LastIndexOf('/') + 1);
				SelectPrefab selectPrefab = new SelectPrefab();
				selectPrefab.dirName = dirName;
				selectPrefab.isSelected = false;
				string[] files=Directory.GetFiles(item, "*.prefab", SearchOption.AllDirectories);

				for (int i = 0; i < files.Length; i++)
				{
					string assetPath=files[i].Replace(Application.dataPath, "").Replace('\\','/');
					var go= AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
						
					selectPrefab.prefabInfos.Add(new PrefabInfo {
						gameObject = go,
						prefabName = go.name,
						isReplace=true 	 ,
						assetPath=assetPath
					});
				}

				dicDir.Add(selectPrefab);
			}
		}
	}

	private void LoopReplaceFont(Transform tf, Font font)
	{
		int childCount = tf.childCount;
		if (childCount > 0)
		{
			for (int i = 0; i < childCount; i++)
			{
				var tx = tf.GetChild(i).GetComponent<Text>();
				if (tx)
				{
					tx.font = font;
				}
				LoopReplaceFont(tf.GetChild(i), font);
			}
		}
	}

	class SelectPrefab
	{
		public List<PrefabInfo> prefabInfos = new List<PrefabInfo>();
		public string dirName;
		public bool isSelected;
	}

	class PrefabInfo
	{
		public GameObject gameObject;
		public string prefabName;
		public bool isReplace;
		public string assetPath;
	}

	[Serializable]
	class CommponentInfo
	{
		public string comName;
		public Component monoBehaviour;
		public bool isReplace;

		public static GameObject lastObj;
	}

	[FieldName("类型选择")]
	enum ReplaceType
	{
		[FieldName("预制件字体更换")]
		FontConvert,
		[FieldName("预制件图集更换")]
		AtlasConvert, 
		[FieldName("预制件组件更换")]
		ComponentConvert
	}
}

