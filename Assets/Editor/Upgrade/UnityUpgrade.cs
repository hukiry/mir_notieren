using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_2019 || UNITY_2018 || UNITY_2017 || UNITY_5
public class UnityUpgrade : EditorWindow
{
	class PrefabDataClass
	{
		private const string audio_dir = "Resources/Extend/UI/Prefab";//自定义声音路径
		private const string prefab_dir = "Resources/Extend/UI";//自定义预制件路径
		public string componetName;
		public object componet;
		public PrefabDataClass(string componetName, Component componet)
		{
			this.componetName = componetName;
			this.componet = componet;
		}
		public string GetClassData()
		{
			if (componet != null)
			{
				Type ty = componet.GetType();
				string str = "";
				FieldInfo[] fis = ty.GetFields();
				for (int i = 0; i < fis.Length; i++)
				{
					if (fis[i].FieldType == typeof(string[]))
					{
						string[] st = (string[])fis[i].GetValue(componet);
						if (st != null && st.Length > 0)
						{
							string st1 = "";
							for (int n = 0; n < st.Length; n++)
							{
								st1 += (st[n] + ",");
							}
							st1 = st1.Replace(",", "-");
							str += (fis[i].Name + "," + st1 + fis[i].GetValue(componet) + ">");
						}
						else
						{
							str += (fis[i].Name + "," + fis[i].GetValue(componet) + ">");
						}
					}
					else if (fis[i].FieldType == typeof(List<string>))
					{
						List<string> st = (List<string>)fis[i].GetValue(componet);
						if (st != null && st.Count > 0)
						{
							string st1 = "";
							for (int n = 0; n < st.Count; n++)
							{
								st1 += (st[n] + ",");
							}
							st1 = st1.Replace(",", "-");
							str += (fis[i].Name + "," + st1 + fis[i].GetValue(componet) + ">");
						}
						else
						{
							str += (fis[i].Name + "," + fis[i].GetValue(componet) + ">");
						}
					}
					else
					{
						str += (fis[i].Name + "," + fis[i].GetValue(componet) + ">");
					}
				}
				return componetName + ">" + str.TrimEnd('>');
			}
			else
			{
				return componetName;
			}
		}
		public static Type ReadClassData(string json, string dllName = "Assembly-CSharp")
		{
			string[] jsonArray = json.Split('>');
			if (jsonArray == null || jsonArray.Length == 0) return null;
			Type ty = Type.GetType(string.Format("{0}, {1}", jsonArray[0], dllName), false);
			if (ty == null)
			{
				ty = Type.GetType(string.Format("{0}, Assembly-CSharp", jsonArray[0]), false);
			}
			return ty;
		}
		public static void SetClassData(string json, Component obj)
		{
			string[] jsonArray = json.Split('>');
			if (jsonArray == null || jsonArray.Length == 0) return;

			Type ty = obj.GetType();

			if (ty != null)
			{
				FieldInfo[] fis = ty.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);
				for (int i = 0; i < fis.Length; i++)
				{
					for (int j = 1; j < jsonArray.Length; j++)
					{
						string[] twoSplit = jsonArray[j].Split(',');
						if (twoSplit[0] == fis[i].Name)
						{
							fis[i].SetValue(obj, GetValue(fis[i].FieldType, jsonArray[j].Substring(jsonArray[j].IndexOf(',') + 1), obj, CustomFunc));
							break;
						}
					}

				}
			}
		}
		private static object CustomFunc(Type ty, string json, Component obj)
		{
			if (ty == typeof(GameObject))
			{
				string[] a = json.Split(' ');
				if (a.Length >= 2)
				{
					var template = obj.transform.Find(a[0].Trim());
					if (template != null)
					{
						return template.gameObject;
					}
					else
					{
						return AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine(Application.dataPath, prefab_dir) + "/" + a[0].Trim() + ".prefab");
					}
				}
				return null;
			}
			//else if (ty == typeof(UITexture))
			//{
			//    string[] a = json.Split(' ');
			//    if (a.Length >= 2)
			//    {
			//        return obj.transform.GetComponent<UITexture>();
			//    }
			//    return null;
			//}
			else
			{ return null; }
		}
		private static object GetValue(Type ty, string json, Component obj, Func<Type, string, Component, object> isContainFunc = null)
		{
			if (ty.IsEnum)
			{
				return Enum.Parse(ty, json);
			}
			else if (ty == typeof(int) || ty == typeof(long) || ty == typeof(short))
			{
				return int.Parse(json);
			}
			else if (ty == typeof(float) || ty == typeof(double))
			{
				return float.Parse(json);
			}
			else if (ty == typeof(bool))
			{
				return bool.Parse(json);
			}
			else if (ty == typeof(string))
			{
				return json;
			}
			else if (ty == typeof(Vector2) || ty == typeof(Vector3) || ty == typeof(Vector4))
			{
				string[] arry = json.Trim('(', ')').Split(',');
				if (arry.Length == 2)
				{
					return new Vector2(float.Parse(arry[0]), float.Parse(arry[1]));
				}
				else if (arry.Length == 3)
				{
					return new Vector3(float.Parse(arry[0]), float.Parse(arry[1]), float.Parse(arry[2]));
				}
				else if (arry.Length == 4)
				{
					return new Vector4(float.Parse(arry[0]), float.Parse(arry[1]), float.Parse(arry[2]), float.Parse(arry[3]));
				}
				else
				{
					return Vector3.zero;
				}
			}
			else if (ty == typeof(AudioClip))
			{
				string[] a = json.Split(' ');
				if (a.Length >= 2)
				{
					foreach (var item in clipDic.Keys)
					{
						if (item == a[0].Trim())
						{
							return AssetDatabase.LoadAssetAtPath<AudioClip>(clipDic[item]);
						}
					}
				}
				return null;
			}
			else if (ty == typeof(string[]) || ty == typeof(List<string>))
			{
				List<string> alist = new List<string>();
				alist.AddRange(json.Split('-'));
				if (alist.Count >= 2)
				{
					alist.RemoveAt(alist.Count - 1);
					if (ty == typeof(string[]))
						return alist.ToArray();
					else
						return alist;
				}
				return null;
			}
			else
			{
				if (isContainFunc != null)
				{
					return isContainFunc(ty, json, obj);
				}
				else
				{
					Log(json, obj, "red");
					return null;
				}
			}
		}
		private static Dictionary<string, string> clipDic = new Dictionary<string, string>();
		public static void InitAudioClip()
		{
			var dirs = Directory.GetFiles(Path.Combine(Application.dataPath, audio_dir), ".mp3");
			foreach (var item in dirs)
			{
				var fileName = Path.GetFileNameWithoutExtension(item);
				clipDic[fileName] = item;
			}
		}
	}
	static List<string> AllFileNames = new List<string>();
	private static List<string> listPath = new List<string>();
	static UnityUpgrade prefab;

	private static string OutPath
	{
		get
		{
			return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Prefab");
		}
	}


	//[MenuItem("Assets/Hukiry_Unity Upgrade/Open Prefab Editor")]
	//[MenuItem("GameObject/Hukiry_Unity Upgrade/Open Prefab Editor", false, -12)]
	static void OpenPrefabWin()
	{

		AllFileNames.Clear();
		string[] s = Directory.GetFiles(OutPath);
		foreach (var t in s)
		{
			AllFileNames.Add(Path.GetFileNameWithoutExtension(t));
		}
		AllFileNames.Sort();
		if (prefab == null)
		{
			prefab = GetWindow<UnityUpgrade>();
		}

		prefab.Show(true);
	}
	void OnGUI()
	{
		if (AllFileNames.Count > 0)
		{
			for (int i = 0; i < AllFileNames.Count; i++)
			{
				if (GUI.Button(new Rect(i % 10 * 155 + 5, i / 10 * 35 + 5, 150, 30), AllFileNames[i]))
				{
					AllFileNames.RemoveAt(i);
				}
			}
		}
	}


	//[MenuItem("Assets/Hukiry_Unity Upgrade/Check Prefab Miss")]
	static void CheckPrefabMiss()
	{
		ClearUnityConsole();
		listPath.Clear();

		var ids = Selection.assetGUIDs;
		if (ids != null && ids.Length > 0)
		{
			foreach (var id in ids)
			{
				string dir = AssetDatabase.GUIDToAssetPath(id);
				CollectFileCount(dir);
			}
		}

		if (listPath.Count == 0)
		{
			return;
		}
		int i = 0;
		foreach (var item in listPath)
		{
			var go = AssetDatabase.LoadAssetAtPath<GameObject>(item);
			PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(go);
			switch (prefabType)
			{
				case PrefabAssetType.Model:
					Log("The object is an imported 3D model asset:" + item, go);
					break;
				case PrefabAssetType.MissingAsset:
					Log("The object was an instance of a prefab, but the original prefab could not be found:" + item, go, "blue");
					break;
				case PrefabAssetType.NotAPrefab:
					Log("The object is an instance of a user created prefab, but the connection is broken:" + item, go, "yellow");
					break;
				case PrefabAssetType.Variant:
					Log("The object is an instance of a user created prefab, but the connection is broken:" + item, go, "pink");
					break;
			}
			i++;
			UnityEditor.EditorUtility.DisplayProgressBar("hukiry 检查预制件异常问题", Path.GetFileNameWithoutExtension(item), i * 1.0f / (listPath.Count * 1.0f));
		}
		UnityEditor.EditorUtility.ClearProgressBar();
	}
	#region 所有预制体修改
	//var ids = Selection.assetGUIDs;
	//if (ids != null && ids.Length > 0)
	//{
	//    foreach (var id in ids)
	//    {
	//        string dir = AssetDatabase.GUIDToAssetPath(id);
	//        CollectFileCount(dir);
	//    }
	//}

	//if (listPath.Count == 0)
	//{
	//    return;
	//}

	//for (int i = 0; i < listPath.Count; i++)
	//{
	//    string filePath = Path.Combine(outPath, Path.GetFileNameWithoutExtension(listPath[i]) + ".txt");
	//    var target = AssetDatabase.LoadAssetAtPath(listPath[i], typeof(GameObject)) as GameObject;
	//    if (target == null) continue;
	//    var Modifications = PrefabUtility.GetPropertyModifications(target);
	//    foreach (var item in Modifications)
	//    {
	//        Debug.Log(item.propertyPath+"   "+ item.target +"  "+ item.value+"   "+item.objectReference);

	//    }
	//    PrefabUtility.SetPropertyModifications(target, Modifications);
	//}

	#endregion
	//[MenuItem("Assets/Hukiry_Unity Upgrade/修复预制件中所有组件")]
	static void FixAllMissScript()
	{
		listPath.Clear();
		var ids = Selection.assetGUIDs;
		if (ids != null && ids.Length > 0)
		{
			foreach (var id in ids)
			{
				string dir = AssetDatabase.GUIDToAssetPath(id);
				CollectFileCount(dir);
			}
		}

		if (listPath.Count == 0)
		{
			return;
		}

		PrefabDataClass.InitAudioClip();

		for (int i = 0; i < listPath.Count; i++)
		{
			AssetDatabase.ImportAsset(listPath[i], ImportAssetOptions.ForceSynchronousImport);
			var go = AssetDatabase.LoadAssetAtPath<GameObject>(listPath[i]);
			if (go)
			{
				DeleteMissComponent(go);
				StartFixMissScript(go.transform);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			UnityEditor.EditorUtility.DisplayCancelableProgressBar("hukiry 获取文件", go.name, i * 1.0f / (listPath.Count * 1.0f));
		}
		UnityEditor.EditorUtility.ClearProgressBar();
	}

	//[MenuItem("GameObject/Hukiry_Unity Upgrade/Fix Miss Script", false, -10)]
	static void FixMissScript()
	{
		ClearUnityConsole();
		//Log("---------------------begin");
		Transform[] target = Selection.GetTransforms(SelectionMode.Assets);

		if (target == null)
		{
			return;
		}

		PrefabDataClass.InitAudioClip();
		StartFixMissScript(target);
		GetDllScript();
	}

	private static void StartFixMissScript(params Transform[] target)
	{
		Dictionary<string, List<string>> NewComponent = new Dictionary<string, List<string>>();
		for (int i = 0; i < target.Length; i++)
		{
			string filePath = Path.Combine(OutPath, target[i].root.name + ".txt");
			if (!File.Exists(filePath))
			{
				Debug.LogError("文件不存在：" + filePath);
				return;
			}

			//Log(target[i].name  + "   " + filePath);

			string[] array = File.ReadAllLines(filePath);
			foreach (var item in array)
			{
				if (!string.IsNullOrEmpty(item))
				{
					string[] str = item.Split('=');
					string key = str[0]/*.Substring(str[0].IndexOf('/') + 1)*/;
					if (!NewComponent.ContainsKey(key))
					{
						NewComponent.Add(key, new List<string>());
					}

					if (str[i].Length >= 2)
					{
						string[] three = str[1].Split('|');
						foreach (var t in three)
						{
							if (!string.IsNullOrEmpty(t))
							{
								NewComponent[key].Add(t);
							}
						}
					}
				}
			}

			var tarGet = target[i];
			foreach (var item in NewComponent)
			{
				Transform tf = tarGet.Find(item.Key);
				if (item.Key == tarGet.name)
				{
					tf = tarGet;
				}

				if (tf != null)
				{

					foreach (var v in item.Value)
					{
						Type ty = PrefabDataClass.ReadClassData(v, "LibGameClient");
						if (ty != null)
						{
							Component cp = tf.gameObject.GetComponent(ty) ?? tf.gameObject.AddComponent(ty);
							PrefabDataClass.SetClassData(v, cp);
						}
					}

				}
			}
			NewComponent.Clear();
		}

	}

	//[MenuItem("Assets/Hukiry_Unity Upgrade/获取预制件中所有dll组件数据")]
	static void GetMissScript()
	{
		listPath.Clear();
		var ids = Selection.assetGUIDs;
		if (ids != null && ids.Length > 0)
		{
			foreach (var id in ids)
			{
				string dir = AssetDatabase.GUIDToAssetPath(id);
				CollectFileCount(dir);
			}
		}

		if (listPath.Count == 0)
		{
			return;
		}

		Dictionary<string, List<PrefabDataClass>> dicComponent = new Dictionary<string, List<PrefabDataClass>>();

		if (!Directory.Exists(OutPath)) Directory.CreateDirectory(OutPath);

		for (int i = 0; i < listPath.Count; i++)
		{
			string filePath = Path.Combine(OutPath, Path.GetFileNameWithoutExtension(listPath[i]) + ".txt");
			var target = AssetDatabase.LoadAssetAtPath(listPath[i], typeof(GameObject)) as GameObject;
			if (target == null) continue;

			for (int n = 0; n < target.transform.childCount; ++n)
			{
				GetScriptNode(target.transform.GetChild(n), null, dicComponent, target.transform);
			}
			UnityEditor.EditorUtility.DisplayProgressBar("hukiry 获取文件", target.name, i * 1.0f / (listPath.Count * 1.0f));
			List<string> line = new List<string>();
			foreach (var item in dicComponent)
			{
				string str = item.Key + "=";
				if (item.Value.Count > 0)
				{
					foreach (var comp in item.Value)
					{
						str += (comp.GetClassData() + "|");
					}
					line.Add(str.TrimEnd('|'));
				}
			}

			if (line.Count > 0)
			{
				File.WriteAllLines(filePath, line.ToArray());

				dicComponent.Clear();
				line.Clear();

			}
		}

		UnityEditor.EditorUtility.ClearProgressBar();
		Application.OpenURL(OutPath);
	}

	/// <summary>
	/// 收集丢失的组件
	/// </summary>
	/// <param name="trans">选择的Transform</param>
	/// <param name="parent">路径</param>
	/// <param name="nodeDic">组件数据集合</param>
	static void GetMissScripNode(Transform trans, string parent, Dictionary<string, Transform> nodeDic, bool isDll = false)
	{
		string nodeName = string.IsNullOrEmpty(parent) ? trans.name : (parent + "/" + trans.name);
		// 先检查自己
		Component[] allComponent = trans.GetComponents<Component>();
		foreach (Component cp in allComponent)
		{
			//如果为空，表示丢失了组件
			if (cp == null)
			{
				nodeDic[nodeName] = trans;
				break;
			}
			else
			{
				if (isDll)
				{
					string st = cp.ToString();
					int startIndex = st.IndexOf("(");
					int endIndex = st.LastIndexOf(")");
					string findName = st.Substring(startIndex + 1, endIndex - startIndex - 1);
					if (IsFind(findName.Trim()))
					{
						continue;
					}
					nodeDic[nodeName] = trans;
				}
			}
		}

		for (int i = 0; i < trans.childCount; ++i)
		{
			GetMissScripNode(trans.GetChild(i), nodeName, nodeDic, isDll);
		}

	}
	//[MenuItem("GameObject/Hukiry/删除miss的脚本", false, -10)]
	static void CleanupMissingScript()
	{
		GameObject[] objects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));

		DeleteMissComponent(objects);
	}

	private static void DeleteMissComponent(params GameObject[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			if (objects[i].hideFlags == HideFlags.None)//HideFlags.None 获取Hierarchy面板所有Object
			{
				var components = objects[i].GetComponents<Component>();
				var serializedObject = new SerializedObject(objects[i]);
				var prop = serializedObject.FindProperty("m_Component");
				int index = 0;

				for (int j = 0; j < components.Length; j++)
				{
					if (components[j] == null)
					{
						prop.DeleteArrayElementAtIndex(j - index);
						index++;
					}
				}

				serializedObject.ApplyModifiedProperties();//保存修改后的对象
			}
		}
	}

	//[MenuItem("GameObject/Hukiry/Miss Script", false, -10)]
	static void OutPutCurrentSelectMissScript()
	{
		DebugPrefabDllScript(false);
	}

	//[MenuItem("GameObject/Hukiry/Get DLL Script", false, -10)]
	static void GetDllScript()
	{
		DebugPrefabDllScript(true);
	}


	private static void DebugPrefabDllScript(bool isGetDllScript)
	{
		ClearUnityConsole();

		Transform[] target = Selection.GetTransforms(SelectionMode.Assets);

		if (target == null)
		{
			return;
		}
		Dictionary<string, Transform> nodeDic = new Dictionary<string, Transform>();
		for (int i = 0; i < target.Length; ++i)
		{
			GetMissScripNode(target[i], "", nodeDic, isGetDllScript);
		}

		//foreach (var item in nodeDic)
		//{
		//    if (item.Value != null)
		//        Debug.Log(EditorHelper.GetColorString(item.Key, EditorHelper.StringColor.SC_Green), item.Value);
		//}

		if (nodeDic.Count == 0)
		{
			Log(isGetDllScript ? "not miss script" : "not dll script", target[0], "green");
		}
	}

	static void Log(string text, UnityEngine.Object obj, string color = "orange")
	{
		Debug.Log("<color=" + color.ToLower() + ">" + text + "</color>", obj);
	}
	private static void CollectFileCount(string dir)
	{
		if (File.Exists(dir))
		{
			FileFilter(dir);
		}
		else
		{
			DirectoryInfo di = new DirectoryInfo(dir);
			FileSystemInfo[] fsinfo = di.GetFileSystemInfos();
			foreach (var item in fsinfo)
			{
				if (item is FileInfo)
				{
					FileFilter(item.FullName);
				}
				else
				{
					CollectFileCount(item.FullName);
				}
			}
		}
	}
	private static void FileFilter(string path)
	{
		string ext = Path.GetExtension(path).ToLower();
		if (ext == ".prefab")
		{
			string filePath = path.Substring(path.IndexOf("Assets"));
			if (!listPath.Contains(filePath))
			{
				FileInfo fi = new FileInfo(filePath);
				listPath.Add(filePath);
			}
		}
	}

	static void GetScriptNode(Transform trans, string parent, Dictionary<string, List<PrefabDataClass>> dicComponent, Transform selfTransform = null)
	{

		string nodeName = string.IsNullOrEmpty(parent) ? trans.name : (parent + "/" + trans.name);
		if (selfTransform != null)
		{
			nodeName = selfTransform.name;
			trans = selfTransform;
		}
		// 先检查自己
		Component[] allComponent = trans.GetComponents<Component>();
		foreach (Component cp in allComponent)
		{
			if (!dicComponent.ContainsKey(nodeName))
			{
				dicComponent.Add(nodeName, new List<PrefabDataClass>());
			}
			if (cp == null) continue;

			string st = cp.ToString();
			int startIndex = st.IndexOf("(");
			int endIndex = st.LastIndexOf(")");
			string findName = st.Substring(startIndex + 1, endIndex - startIndex - 1);//包含了命名空间的
			if (IsFind(findName.Trim()))
			{
				continue;
			}
			PrefabDataClass d = new PrefabDataClass(findName, cp);

			if (!dicComponent[nodeName].Contains(d))
				dicComponent[nodeName].Add(d);
		}

		// 递归检查孩子

		for (int i = 0; i < trans.childCount; ++i)
		{
			GetScriptNode(trans.GetChild(i), selfTransform != null ? null : nodeName, dicComponent);
		}
	}
	static bool IsFind(string typeName)
	{

		try
		{
			if (typeName == "UIRoot" || typeName == "UIPanel") return true;

			Type ty = Type.GetType(string.Format("{0}, Assembly-CSharp", typeName), false);// 类名，Assembly-CSharp.dll库名
			if (ty != null) return true;


			ty = Type.GetType(string.Format("{0}, UnityEngine", typeName), false);// 类名，UnityEngine.dll库名
			if (ty != null) return true;

			return false;
		}
		catch
		{
			return false;
		}
	}

	private static void ClearUnityConsole()
	{
		//Clear控制台
		var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
		var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
		clearMethod.Invoke(null, null);
	}
}
#endif