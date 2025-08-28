using UnityEngine;

public abstract class CommonAssets<T>: ScriptableObject where T: ScriptableObject
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
#if UNITY_EDITOR
				string filePath = GetFilePath(typeof(T).Name, typeof(T).Name).Replace(".cs", ".asset");
				instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(filePath);
				if (instance == null)
				{
					instance = CreateInstance<T>();
					instance.name = typeof(T).Name;
					UnityEditor.AssetDatabase.CreateAsset(instance, filePath);
				}
#endif
			}
			return instance;
		}
	}

	public virtual void SaveAssetsBefore()
	{
	
	}

	//提供给编辑模式
	public void SaveAssets()
	{
		this.SaveAssetsBefore();
#if UNITY_EDITOR

		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}

#if UNITY_EDITOR
	private static string GetFilePath(string assetName, string scriptName = null)
	{
		var assets = UnityEditor.AssetDatabase.FindAssets(assetName);
		string assetPath = null;
		if (assets.Length == 1)
		{
			assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
		}
		else
		{
			for (int i = 0; i < assets.Length; i++)
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[i]);
				if (System.IO.Path.GetFileNameWithoutExtension(path) == scriptName)
				{
					assetPath = path;
					break;
				}
			}
		}
		return assetPath;
	}

    private void OnValidate()
    {
		
	}
#endif
}
