using Hukiry;
using Hukiry.Http;
using HukiryInitialize;
using System;
using System.IO;
using UnityEngine;


/// <summary>
/// 程序主入口
/// </summary>

public class MainGame :MonoBehaviour
{
	public static MainGame Instance;

	public static GameVersion gameVersion;

	public string JsonGameVersion => JsonUtil.ToJson(gameVersion);

	private long lastDeactiveTime = 0;//上次反激活时间


	public EWorkMode WorkMode;

	public string WorkModeString => WorkMode.ToString();

	void Awake() {

        DontDestroyOnLoad(gameObject);
        Instance = this;
        //设置游戏信息
        Hukiry.HukiryUtil.InitGameInfo();
		//初始化log缓存路径
		LogManager.Init(Application.persistentDataPath + "/Cache/");
		//初始化lua
#if ENABLE_LUA
		gameObject.AddComponent<LuaManager>();
#endif
		//注册时间
		LooperManager.Instance.AddLoopAble(TimerUnit.Looper);
	}

	void Start () {
		LogManager.Log("启动游戏");

		//延时帧执行方法
		Loom.Initialize();

		//初始SDK信息
#if ENABLE_SDK
		Hukiry.SDK.SdkManager.ins.InitSDKInformation();
#endif

#if ENABLE_FPS
		GameObject fpsGo = new GameObject("Fps");
		fpsGo.AddComponent<Fps>();
		GameObject.DontDestroyOnLoad(fpsGo);
#endif

#if SYSTEM_INFO
		GameObject SystemBatteryGo = new GameObject("SystemBattery");
		SystemBatteryGo.AddComponent<SystemBattery>();
		GameObject.DontDestroyOnLoad(SystemBatteryGo);
#endif

#if RELEASE && !UNITY_EDITOR
		GameObject.Destroy(GameObject.Find("Reporter"));
#endif


		Loading.Instance.ShowProgress(1);

        this.RegeditFunction();
        //加载包版本
        this.LoadPacketVersion(ps=> {
			gameVersion = ps;
			//检查更新，并下载资源
			GIController.StartLoad(this.CheckUpdateFinish);
		});
    }

	private void LoadPacketVersion(Action<GameVersion> action)
	{
		string filepath = AssetBundleConifg.WWWPrefix + Path.Combine(AssetBundleConifg.AppPacketPath, AssetBundleConifg.VersionName);
		HttpLocalRequest uwrGet = new HttpLocalRequest(filepath, (isSuccess, msg, _) => {
			if (isSuccess)
			{
				GameVersion ps = JsonUtil.ToObject<GameVersion>(msg);
	
#if !UNITY_EDITOR
				this.WorkMode = (EWorkMode)ps.workMode;
#endif
				action?.Invoke(ps);
			}
			else
				action?.Invoke(new GameVersion());
		});
	}

	/// <summary>
	/// 注册加载的函数
	/// </summary>
	private void RegeditFunction()
	{
        GIController.ShowPanelProgress = () => Loading.Instance.ShowProgress(5);
        GIController.UpdateProgress = (progress, title, downTip) => Loading.Instance.ShowProgress(progress, title, downTip);
        GIController.ShowPanelMsgBox = (s, ok, cancel) => PromptBox.Instance.Show(s, ok, cancel);
    }

	void CheckUpdateFinish() {
		FileHelper.CreateDirectory(AssetBundleConifg.CacheAbPath);

		//资源加载初始化成功才可以初始化脚本引擎
		if (!AssetsLoaderMgr.Initialize(transform))
		{
			Debug.LogError("初始化资源失败!"); return;
		}

#if UNITY_EDITOR
		//初始化一些临时按钮
		InputTest.Initialize();
#endif
//启动lua代码
#if ENABLE_LUA
        LuaManager.instance.EnableGame();
#endif
        //隐藏进度条
        Loading.Instance.HideView();
    }



    private void OnApplicationPause(bool pause) {
		bool isActive = !pause;
		int seconds = 0;
		if (isActive) {
			seconds = Mathf.FloorToInt((this.GetTotalMilliseconds() - lastDeactiveTime) / 1000);
			LogManager.Log("重新激活游戏，置于后台时间：" , seconds);
		} else {
			//保存缓存数据
			lastDeactiveTime = this.GetTotalMilliseconds();
		}
#if ENABLE_LUA
		LuaManager.instance?.ApplicationPause(pause, seconds);
#endif
	}

	void OnApplicationQuit() {
		//保存缓存数据
		LogManager.OnApplicationQuit();
#if ENABLE_LUA
		LuaManager.instance?.ApplicationQuit();
#endif
		StopAllCoroutines();
	}

	private long GetTotalMilliseconds()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return (long)ts.TotalMilliseconds;
	}

}