using UnityEditorInternal;
using UnityEngine;

public abstract class ProjectSettingsScriptable<T>: ScriptableObject where T: ScriptableObject
{
	private static string PATH_SETTINGS => $"ProjectSettings/{typeof(T).Name}.asset";
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				
				Object[] objects = InternalEditorUtility.LoadSerializedFileAndForget(PATH_SETTINGS);
				
				if (objects == null || objects.Length == 0)
				{
					instance = CreateInstance<T>();
					instance.name = typeof(T).Name;
					InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] {
					instance}, PATH_SETTINGS, true);
				}
				else
				{
					instance = objects[0] as T;
				}
			}
			return instance;
		}
	}


	/// <summary>
	/// 保存后刷新
	/// </summary>
	public abstract void Refresh();


	public void SaveAssets()
	{
		InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { instance }, PATH_SETTINGS, true);
		this.Refresh();
	}
}
