using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Hukiry.Editor
{
    /// <summary>
    /// 设置编辑图标显示
    /// </summary>
    public partial class ScriptIconEditor
	{
		static Dictionary<System.Type, DrawIconInfo> m_DrawIconInfo = new Dictionary<System.Type, DrawIconInfo>();

		static Dictionary<int, List<System.Type>> m_instanceIdTable = new Dictionary<int, List<System.Type>>();
		internal static void IconsOnPlaymodeStateChanged(PlayModeStateChange stateChange)
		{

			if (Application.isEditor)
			{
				EditorApplication.hierarchyWindowItemOnGUI -= IconsOnGUI;

				EditorApplication.hierarchyChanged -= IconsOnChanged;

				
				EditorApplication.hierarchyWindowItemOnGUI += IconsOnGUI;

				EditorApplication.hierarchyChanged += IconsOnChanged;

				IconsOnChanged();
			}
		}


		internal static void IconsOnChanged()
		{
			m_instanceIdTable.Values.ToList().ForEach(p => p?.Clear());
			m_instanceIdTable.Clear();
			foreach (var keyType in m_DrawIconInfo.Keys)
			{
				Object[] objects = Object.FindObjectsOfType(keyType);
				foreach (Component c in objects)
				{
					int id = c.gameObject.GetInstanceID();
					if (!m_instanceIdTable.ContainsKey(id))
						m_instanceIdTable[id] = new List<System.Type>();
					m_instanceIdTable[id].Add(c.GetType());
				}
			}
		}

		internal static void IconsOnGUI(int instanceId, Rect selectionRect)
		{
			if (SymbolSetting.Instance.isShaowHierarchyIcon && m_instanceIdTable.ContainsKey(instanceId))
			{
				var array = m_instanceIdTable[instanceId];
				int len = Mathf.Clamp(array.Count, 1, 5);
				int afterIndex = 0, beforeIndex = 0;
				for (int i = 0; i < len; i++)
				{
					var type = array[i];
					if (m_DrawIconInfo.ContainsKey(type))
					{
						var drawInfo = m_DrawIconInfo[type];
						if (drawInfo.hierarchyDrawIconLayout == HierarchyIconLayout.After)
						{
							afterIndex++;
							Rect rect = new Rect(selectionRect.x + selectionRect.width - 10f - 16 * (afterIndex - 1), selectionRect.y, 16f, 16f);
							GUI.DrawTexture(rect, drawInfo.IsShowOnHierarchy ? drawInfo.ScriptIcon : drawInfo.HierarchyIcon);
						}
						else if (drawInfo.hierarchyDrawIconLayout == HierarchyIconLayout.Before)
						{
							beforeIndex++;
							Rect rect = new Rect(selectionRect.x - 16 * (afterIndex + 1), selectionRect.y, 16f, 16f);
							GUI.DrawTexture(rect, drawInfo.IsShowOnHierarchy ? drawInfo.ScriptIcon : drawInfo.HierarchyIcon);
						}
					}
				}
			}
		}

		private bool IsContainType(int instanceId, System.Type type)
		{
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (gameObject != null && gameObject.GetComponent(type) != null)
            {
				return true;
            }

			return false;
        }
	}


	[InitializeOnLoad]
	public partial class ScriptIconEditor
	{
		static ScriptIconEditor()
		{
			m_DrawIconInfo.Clear();

			var assemblyTuple = GetAssembly();

			System.Type[] arrayType = assemblyTuple.assetmblys.GetTypes();

			foreach (var ty in arrayType)
			{
				var draw = ty.GetCustomAttribute(assemblyTuple.drawType);

				if (draw != null)
				{
					DrawIconInfo info = new DrawIconInfo() { iconAttribute = draw, ClassName = ty.Name, };

					info.HierarchyIcon = GetTexture(info.IconNameOfHierarchy, info.typeNameOfScript);

					info.ScriptIcon = GetTexture(info.IconNameOfScript, info.typeNameOfScript);

					m_DrawIconInfo[ty] = info;
				}
			}

			foreach (var item in m_DrawIconInfo.Values)
			{
				SetIconForObjectByFindFile(item.IconNameOfScript, item.ClassName, item.typeNameOfScript);
			}

			EditorApplication.playModeStateChanged -= IconsOnPlaymodeStateChanged;

			EditorApplication.playModeStateChanged += IconsOnPlaymodeStateChanged;

			IconsOnPlaymodeStateChanged(PlayModeStateChange.EnteredEditMode);
		}

		private static (Assembly assetmblys, System.Type drawType) GetAssembly(string Assembly_CSharp = "Assembly-CSharp")
		{
			System.Type drawType = System.Type.GetType($"UnityEngine.DrawIconAttribute, {Assembly_CSharp}");

			Assembly assetmblys = drawType.Assembly;

			if (assetmblys == null) assetmblys = Assembly.Load(Assembly_CSharp);

			if (assetmblys == null) System.AppDomain.CurrentDomain.GetAssemblies().First(p => p.GetName().Name.Equals(Assembly_CSharp));

			return (assetmblys, drawType);
		}

		private static Texture2D GetTexture(string iconName, System.Type type)
		{
			if (!string.IsNullOrEmpty(iconName))
			{
				var arrowPath = GetFilePath($"t:texture {iconName}");
				Texture2D tex = null;
				if (arrowPath != null)
				{
					tex = AssetDatabase.LoadAssetAtPath<Texture2D>(arrowPath);
				}

				if (tex == null)
				{
					tex = Hukiry.HukiryUtilEditor.GetTexture2D(iconName);

				}
				return tex;
			}
			else
			{
				return EditorGUIUtility.ObjectContent(null, type).image as Texture2D;
			}
		}

		private static void SetIconForObjectByFindFile(string iconName, string className, System.Type typeIcon)
		{
			var objPath = GetFilePath($"t:script {className}", className);

			var texPath = GetFilePath($"t:texture {iconName}");

			Object obj = AssetDatabase.LoadAssetAtPath<Object>(objPath);

			Texture2D tex = null;

			if (typeIcon != null)
			{
				tex = EditorGUIUtility.ObjectContent(null, typeIcon).image as Texture2D;
			}
			else
			{
				tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
			}

			if (tex == null)
			{
				tex = Hukiry.HukiryUtilEditor.GetTexture2D(iconName);
			}

			SetScriptIcon(obj, tex);
		}

		private static string GetFilePath(string assetName, string scriptName = null)
		{
			var assets = AssetDatabase.FindAssets(assetName);

			string assetPath = null;

			if (assets.Length == 1)
			{
				assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
			}
			else
			{
				for (int i = 0; i < assets.Length; i++)
				{
					var path = AssetDatabase.GUIDToAssetPath(assets[i]);

					if (Path.GetFileNameWithoutExtension(path) == scriptName)
					{
						assetPath = path;

						break;
					}
				}
			}
			return assetPath;
		}

		private static void SetScriptIcon(Object obj, Texture2D tex)
		{
			var ty = typeof(EditorGUIUtility);
			var method = ty.GetMethod("Set" +
				"Icon" +
				"For" +
				"Object",
				BindingFlags.NonPublic |
				BindingFlags.Static);

			method?.Invoke(null, new object[] { obj, tex });
		}

		private static void SetIcon(GameObject gameObject, Texture2D icon)
		{
			typeof(EditorGUIUtility).InvokeMember("Set" +
				"Icon" +
				"For" +
				"Object", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic, null, null, new object[2] {
				gameObject,
				icon
			});
		}

	}


	internal sealed class DrawIconInfo
	{
		private Dictionary<string, object> infoDic = new Dictionary<string, object>();

		public string ClassName;

		public Texture2D HierarchyIcon;

		public Texture2D ScriptIcon;

		public System.Attribute iconAttribute;


		public string IconNameOfScript => GetFieldValue<string>(nameof(IconNameOfScript));

		public string IconNameOfHierarchy => GetFieldValue<string>(nameof(IconNameOfHierarchy));

		public bool IsShowOnHierarchy => GetFieldValue<bool>(nameof(IsShowOnHierarchy));

		public System.Type typeNameOfScript => GetFieldValue<System.Type>(nameof(typeNameOfScript));

		public HierarchyIconLayout hierarchyDrawIconLayout => GetFieldValue<HierarchyIconLayout>(nameof(hierarchyDrawIconLayout));

		
		public bool IsCanDrawOnHierarchy<T>() => ClassName == typeof(T).Name;

		public T GetFieldValue<T>(string fieldName)
		{
			if (infoDic.ContainsKey(fieldName)) return (T)infoDic[fieldName];

			var fieldInfo = iconAttribute.GetType().GetField(fieldName);

			var obj = fieldInfo.GetValue(iconAttribute);

			infoDic[fieldName] = obj;

			return (T)obj;
		}
	}
}