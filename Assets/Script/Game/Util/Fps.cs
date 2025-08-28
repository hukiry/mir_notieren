using Hukiry;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class Fps : MonoBehaviour
{

	private string result = "";

	private string androidMemory = "0";
	private GUIStyle labelStyle;

	public bool monitorGC = false;
	public bool monitorMemory = true;
	public bool monitorBattery = true;

	private Texture2D aTexture;

	void Awake()
	{
#if !UNITY_EDITOR && !ENABLE_FPS
		this.enabled = false;
#endif
		Color color = new Color(0, 0, 0, 0.6f);
		aTexture = new Texture2D(10, 10, TextureFormat.RGBA32, false);
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				aTexture.SetPixel(i, j, color);
			}
		}
		aTexture.Apply();

		labelStyle = new GUIStyle();
		labelStyle.fontSize = 30;
		labelStyle.normal.textColor = Color.green;
        labelStyle.normal.background = aTexture;
    }

	private void callAndroid()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		//androidMemory = GetUseMemory();
#endif
	}

	//android获取手机内存
	public string GetUseMemory()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		string memory = "";
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				memory = obj_Activity.Call<string>("getMemoryInfo");
			}
		}
		try {
			string[] datas = memory.Split('-');
			string me = datas[3];//124054KB
			me = me.Substring(0, me.Length - 2);
			LogManager.Log("Android获取手机内存：" + memory);
			return (int.Parse(me) / 1024).ToString();
		} catch {
			LogManager.Log("Android获取手机内存错误：" + memory);
			return memory;
		}
#else
		return "1";
#endif
	}

	void Update()
	{
		UpdateFPS();
		result = "";
		result += "Fps:" + fps.ToString("f0") + " \n";
		if (monitorMemory)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
		//result += "内存:" + androidMemory + "\n";
#else
		result += "Mono:" + (int)(Profiler.GetMonoUsedSizeLong() / 1024 / 1024) + "\n";
#endif
		}
	}

	void OnGUI()
	{
		if (this.monitorMemory)
		{
			GUI.Label(new Rect(0, 0, 135, 70), result, labelStyle);
		}
		else
		{
			GUI.Label(new Rect(0, 0, 135, 36), result, labelStyle);
		}

#if UNITY_EDITOR
		if (this.monitorGC)
		{
			int size = GUI.skin.button.fontSize;
			GUI.skin.button.fontSize = 20;
			if (GUI.Button(new Rect(140, 0, 45, 50), "GC"))
			{
				UnityEngine.Resources.UnloadUnusedAssets();
				System.GC.Collect();
			}

			if (GUI.Button(new Rect(140, 55, 120, 50), "未释放资源"))
			{
				AssetsLoaderMgr.Instance.UnLoadAllAssets();
			}

			if (GUI.Button(new Rect(140, 110, 120, 50), "释放缓存AB"))
			{
				AssetsLoaderMgr.Instance.UnloadUnusedAssets(true);
			}
			GUI.skin.button.fontSize = size;
		}
#endif
	}

	//FPS
	private float updateInterval = 0.25f;
	private int frames = 0;
	private float fps = 0;
	private bool firstTime = true;
	private float lastUpdate = 0;
	private int requiredFrames = 10;

	void UpdateFPS()
	{

		if (firstTime)
		{
			firstTime = false;
			lastUpdate = Time.realtimeSinceStartup;
			frames = 0;
			return;
		}
		frames++;
		float dt = Time.realtimeSinceStartup - lastUpdate;
		if (dt > updateInterval && frames > requiredFrames)
		{
			fps = (float)frames / dt;
			lastUpdate = Time.realtimeSinceStartup;
			frames = 0;
		}
	}
}