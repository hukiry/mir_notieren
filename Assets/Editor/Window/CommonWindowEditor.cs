using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class BaseData
{
	internal bool isOpen = true;
	internal string OpenFilePath;
}
public class EditorBaseData
{
	public string filePath;
	public bool isAdd;
} 
#region 资源处理窗口
/// <summary>
/// 资源数据窗口
/// </summary>
/// <typeparam name="T"></typeparam>
[ExecuteInEditMode]
public abstract class ResourcesWindowEditor<T> : CustomMenuWindow<T> where T : ResourcesWindowEditor<T>
{

    public const string ATLAS_ROOT = "Assets/Resources/Extend/UI/Atlas";
    public virtual bool IsCloseWindow { get { return true; } }
    public Action<bool> ReInitFileFilterCall = null;
    private long CurrentAllFileSize;
    private static T _instance;
    public static T _Instance
    {
        get
        {
            
            if (_instance == null) _instance = ScriptableObject.CreateInstance<T>();
            return _instance;
        }
    }

    private T _Win;

    [SerializeField]
    private List<EditorBaseData> listPath = new List<EditorBaseData>();
    private BaseData baseData;
    private static Rect rectSize = new Rect(20, 20, 700, 300);
    private Vector2 offset;
    private Vector2 offsetDraw;
    public void OpenWindowEditor<K>(string[] ids,K k) where K : BaseData, new()
    {
        if (_Win == null)
        {
            _Win = GetWindowWithRect<T>(rectSize, true, typeof(T).Name);
        }

        _Win.Show();

        listPath.Clear();
        baseData = k;

        if (ids != null && ids.Length > 0)
        {
            foreach (var id in ids)
            {
                string dir = AssetDatabase.GUIDToAssetPath(id);
                _Win.CollectFileCount(dir);
            }
        }
    }

    public EditorBaseData GetEditorBaseData()
    {
        return listPath.Count > 0 ? listPath[0] : null;
    }

    public void SetBaseData(BaseData baseData)
    {
        this.baseData = baseData;
    }
    private void OnGUI()
    {
		if (EditorApplication.isCompiling) return;

        try
        {
            GUILayout.BeginVertical();
            GUI.skin.label.fontSize = 16;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.color = Color.yellow;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;
            GUI.DrawTexture(new Rect(2, 2, this.maxSize.x - 2, 22), this.GetTexture2D(new Color32(0, 120, 255, 255), 50, 22, 1, 1));
            GUILayout.Label(WindowTitileName() + "文件优化");
            GUILayout.Space(10);



            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.fontSize = 14;
            if (listPath != null && listPath.Count > 0)
            {
                GUI.color = Color.yellow;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("选择的文件个数：" + listPath.Count);
                GUI.color = Color.white;
                if (GUILayout.Button("重新选择文件目录"))
                {
                    listPath.Clear();
                    baseData.OpenFilePath = string.Empty;
                }
                EditorGUILayout.EndHorizontal();

                //----------------------------------------------内存计算
                if (CurrentAllFileSize / 1024 / 1024 >= 120)
                {
                    GUI.color = Color.red;

                }
                else if (CurrentAllFileSize / 1024 / 1024 >= 60)
                {
                    GUI.color = Color.yellow;
                }
                else
                {
                    GUI.color = Color.green;
                }
                GUILayout.Label(string.Format("实际文件占用内存：{0:f2}KB\t\t{1:f2}MB", CurrentAllFileSize / 1024f, CurrentAllFileSize / 1024f / 1024f));

                GUI.color = Color.white;

                if (GUILayout.Button(DrawGUI(baseData)))
				{
					if (UnityEditor.EditorUtility.DisplayDialog(Application.version, "一旦修改，则无法撤回！", "确认"))
					{
						this.RefreshData(baseData);
					}
                }


                GUILayout.Space(50);
                GUI.color = Color.yellow;
                GUILayout.Label("优化文件如下：");
                GUI.color = Color.white;
                offset = EditorGUILayout.BeginScrollView(offset, "文件路径");
                for (int i = 0; i < listPath.Count; i++)
                {
                    var data = listPath[i];
                    EditorGUILayout.BeginHorizontal();
#if UNITY_2018_4_OR_NEWER
                    GUI.backgroundColor = data.isAdd ? Color.HSVToRGB(0.4f, 0.3f, 0.8f) : Color.red;
                    GUI.contentColor = data.isAdd ? Color.green : Color.red;
#endif
                    GUILayout.Button(data.filePath);
                    if (data.filePath.EndsWith(".wav") || data.filePath.EndsWith(".mp3"))
                    {
                        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(data.filePath);
                        EditorGUILayout.TextField(string.Format("{0:f1}s",clip.length),GUILayout.Width(50));
                    }
                    data.isAdd = EditorGUILayout.ToggleLeft(listPath[i].isAdd ? "取消" : "添加", listPath[i].isAdd, GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUI.color = Color.green;
                baseData.isOpen = EditorGUILayout.Toggle("Is Use Other Way", baseData.isOpen);
                if (!baseData.isOpen)
                {
                    GUI.color = Color.yellow;
                    GUILayout.Label("请选择要修改的" + WindowTitileName() + "文件！");

                    GUI.color = Color.white;
                    if (GUILayout.Button("打开" + WindowTitileName() + "文件目录"))
                    {

                        baseData.OpenFilePath = EditorUtility.OpenFolderPanel("Hukiry_Unity " + typeof(AudioWindowEditor).Name, baseData.OpenFilePath
                            , "Assets");

                        if (baseData.OpenFilePath != null && baseData.OpenFilePath.Length > 0)
                        {
                            this.CurrentAllFileSize = 0;
                            if (ReInitFileFilterCall != null) ReInitFileFilterCall(true);
                            this.CollectFileCount(baseData.OpenFilePath);
                            if (listPath.Count > 0)
                            {
                                this.ShowNotification(new GUIContent("加载完成！"));
                            }
                            else
                            {
                                this.ShowNotification(new GUIContent("此目录下没有任何" + WindowTitileName() + "文件！"));
                            }
                        }
                    }
                }
                else
                {
                    GUI.color = Color.white;
                    if (Directory.Exists(baseData.OpenFilePath))
                    {
                        EditorGUILayout.BeginHorizontal();
                        baseData.OpenFilePath = EditorGUILayout.TextField("Input Directory Path", baseData.OpenFilePath);
                        GUI.color = Color.white;
                        if (GUILayout.Button("重新选择文件目录"))
                        {
                            listPath.Clear();
                            baseData.OpenFilePath = string.Empty;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (GUILayout.Button("加载目录"))
                        {
                            this.CurrentAllFileSize = 0;
                            if (ReInitFileFilterCall != null) ReInitFileFilterCall(true);
                            baseData.isOpen = true;
                            this.CollectFileCount(baseData.OpenFilePath);
                            if (listPath.Count > 0)
                            {
                                this.ShowNotification(new GUIContent("加载完成！"));
                            }
                            else
                            {
                                this.ShowNotification(new GUIContent("此目录下没有任何" + WindowTitileName() + "文件！"));
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Object obj = null;
                        GUILayout.Label("拖入文件到 None (Object) 区域");
                        GUILayout.BeginHorizontal();
                        GUILayout.Box(this.GetTexture2D(new Color32(100, 160, 20, 160), 110), GUILayout.Height(110));
                        obj = EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), true, GUILayout.Height(110));
                        GUILayout.EndHorizontal();
                        if (obj != null)
                        {
                            if (Directory.Exists(AssetDatabase.GetAssetPath(obj.GetInstanceID())))
                            {
                                baseData.OpenFilePath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
                            }
                            else
                            {
                                baseData.OpenFilePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj.GetInstanceID()));
                            }
                        }
                    }
                }

            }
            if (this)
            {
                GUILayout.EndVertical();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
        Repaint();
    }

    private Texture2D GetTexture2D(Color col,int Width =110,int Hight=110,int WArea=10, int HArea = 10)
    {
        Texture2D tex = new Texture2D(Width, Hight, TextureFormat.ARGB32, false);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Hight; j++)
            {
                tex.SetPixel(i, j, Color.clear);
                if ((j/ WArea + 1) % 2 == 0&& (i/ HArea + 1) % 2 != 0)
                {
                    tex.SetPixel(i, j, col);
                }

                if ((j / WArea + 1) % 2 != 0 && (i / HArea + 1) % 2 == 0)
                {
                    tex.SetPixel(i, j, col);
                }
            }
        }
        tex.Apply();
        return tex;
    }

    private void CollectFileCount(string dir)
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
    private void FileFilter(string path)
    {
        string ext = Path.GetExtension(path).ToLower();
        if (this.FileFilterExtension(ext))
        {
            string filePath = path.Substring(path.IndexOf("Assets"));
            if (listPath.Find(p=>p.filePath==filePath)==null)
            {
                FileInfo fi = new FileInfo(filePath);
                CurrentAllFileSize += fi.Length;
                listPath.Add(new EditorBaseData { filePath = filePath, isAdd = true });
            }
        }
    }

    private void RefreshData(BaseData data)
    {
        this.ShowNotification(new GUIContent("数据转换中.."));
        for (int i = 0; i < listPath.Count; i++)
        {
            if (listPath[i].isAdd)
            {
				
				string assetPath = this.ExcuteData(data, listPath[i]);

				if (string.IsNullOrEmpty(assetPath)) continue;
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.Default);
#if UNITY_2018_4_OR_NEWER
				this.titleContent = new GUIContent(listPath[i] + "        " + i);
#endif
				bool isCancel = EditorUtility.DisplayCancelableProgressBar("Hukiry_Unity " + WindowTitileName() + "文件修改中...", listPath[i].filePath, (i * 1.0f + 1f) / (listPath.Count * 1.0f));
				if (isCancel)
				{

					break;
				}
			}
        }
        this.EndSave();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
        this.ShowNotification(new GUIContent("Finished！"));

        if (IsCloseWindow)
        {
            this.Close();
        }
    }

    public void ReCollectFile()
    {
        if (!string.IsNullOrEmpty(baseData.OpenFilePath))
        {
            this.CurrentAllFileSize = 0;
            listPath.Clear();
            this.CollectFileCount(baseData.OpenFilePath);
        }
    }
    public void Log(string str)
    {
		LogManager.Log(str);
    }
    public void LogError(string str)
    {
        LogManager.LogColor("red", str);
    }

    /// <summary>
    /// 设置GUI
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public abstract string DrawGUI(BaseData data);
    /// <summary>
    /// 窗口名称
    /// </summary>
    public abstract string WindowTitileName();
    /// <summary>
    /// 文件格式过滤
    /// </summary>
    /// <param name="path">遍历的文件路径</param>
    /// <param name="listPathCollect">文件路径集合收集</param>
    public abstract bool FileFilterExtension(string extName);
	/// <summary>
	/// 返回assetPath
	/// </summary>
	/// <param name="data"></param>
	/// <param name="baseData"></param>
	/// <returns></returns>
	public abstract string ExcuteData(BaseData data, EditorBaseData baseData);
    public virtual void EndSave() { }
}
#endregion

#region 创建窗口
/// <summary>
/// 创建窗口
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CreateWindowEditor<T> : BaseWindow<T> where T : CreateWindowEditor<T>
{
    public override void OnGUI()
	{
		if (EditorApplication.isCompiling) return;
		//文本样式描述
		GUI.skin.label.fontSize = 16;
		GUI.skin.label.fontStyle = FontStyle.Normal;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
        GUI.DrawTexture(new Rect(2, 2, this.maxSize.x - 2, 22), Hukiry.HukiryUtilEditor.DrawTexture2D(this.TitleColor(), 50, 22, 1, 1));

        GUILayout.Label(this.titleContent.text + "窗口");
		DrawGUI();
		GUI.skin.label.fontSize = 12;
		this.Repaint();
	}
	public virtual Color TitleColor()
	{
		return new Color32(0, 120, 255, 255);
	}
}
#endregion


#region 基类窗口
public abstract class BaseWindow<T> : CustomMenuWindow<T> where T : BaseWindow<T>
{
	private static T _instance;
	public static T ins
	{
		get
		{
			if (_instance == null) _instance = ScriptableObject.CreateInstance<T>();
			return _instance;
		}
	}
	private static Rect rectSize = new Rect(20, 20, 700, 300);
	public virtual void OpenWindowEditor(string titleName = "窗口")
	{
		_instance = GetWindowWithRect<T>(rectSize, true, typeof(T).Name);

		_instance.titleContent = new GUIContent(titleName);

		_instance.Show(true);//true 只显示一个单例，false 显示多个
	}

	public virtual void OpenWindowEditor(string titleName, string icon, string tooltip = "")
	{
		_instance = GetWindowWithRect<T>(rectSize, true, typeof(T).Name);

		if (!string.IsNullOrEmpty(icon))
		{
			_instance.titleContent = new GUIContent(titleName, Hukiry.HukiryUtilEditor.GetTexture2D(icon), tooltip);
		}
		else
		{
			_instance.titleContent = new GUIContent(titleName);
		}

		_instance.Show(true);//true 只显示一个单例，false 显示多个
	}

    public abstract void OnGUI();
	public abstract void DrawGUI();
}
#endregion

public abstract class CustomMenuWindow<T> : EditorWindow, IHasCustomMenu where T : CustomMenuWindow<T>
{
    void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("定位脚本"), false, () => {
            Hukiry.HukiryUtilEditor.LocationObject<MonoScript>(typeof(T).Name);
        });
    }
}
