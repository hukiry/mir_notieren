using Hukiry;
using Hukiry.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PathConifg = Hukiry.AssetBundleConifg;

namespace HukiryInitialize
{
    /// <summary>
    /// 游戏初始化控制器
    /// </summary>
    public class GIController : MonoBehaviour {
		/// <summary>
		/// 显示进度条面板
		/// </summary>
		public static Action ShowPanelProgress;
		/// <summary>
		/// 下载进度条
		/// </summary>
		public static Action<float,string,string> UpdateProgress;
		/// <summary>
		/// 更新资源，包，或下载失败的提示框
		/// </summary>
		public static Action<int,Action, Action> ShowPanelMsgBox; 


		public delegate void CheckFileFinish();
		CheckFileFinish OnCheckFileFinish;

		public static GIController instance;
		//游戏开始
		public static void StartLoad(CheckFileFinish onCheckFileFinish) {
			ShowPanelProgress?.Invoke();
			GameObject go = new GameObject("GameInitialize");
			instance = go.AddComponent<GIController>();

			instance.OnCheckFileFinish = onCheckFileFinish;
		}

		private List<IGameInitialize> loadQueue = new List<IGameInitialize>();

		/// <summary>
		/// 是否是新包
		/// </summary>
		public static bool newPack = false;

		/// <summary>
		/// 服务器版本号
		/// </summary>
		public static GameVersion serverVersions;

		/// <summary>
		/// 是否需要更新：版本号对比
		/// </summary>
		public static bool isNeedUpdate;
		/// <summary>
		/// 是否更新App
		/// </summary>

		public static bool isAppUpdate;

		/// <summary>
		/// 需要更新的记录文件清单
		/// </summary>
		public static Queue<HttpQueueInfo> updateVersionRecord = new Queue<HttpQueueInfo>();


		void Start() {
			DontDestroyOnLoad(gameObject);

			//创建资源文件夹
			if (!Directory.Exists(PathConifg.AppCachePath)) {
				Directory.CreateDirectory(PathConifg.AppCachePath);
			}

			loadQueue.Add(gameObject.AddComponent<VersionComparison>());//版本号下载与比较
			loadQueue.Add(gameObject.AddComponent<VersionRecordComparison>());//如果更新，下载清单文件版本对比，如果更新则添加记录
			loadQueue.Add(gameObject.AddComponent<DownloadVersionUpdate>());//下载要更新的清单文件，然后初始化Lua

			MainGame.Instance.StartCoroutine(CoroutineUpdateRes());
		}

		IEnumerator CoroutineUpdateRes()
		{
#if ENABLE_SOCKET
#if !STRONG_SOCKET
			if (HukiryUtil.GetNetworkState() > 0)//无网络下，不更新
#endif
			{ 
				for (int i = 0; i < loadQueue.Count; i++)
				{
					IGameInitialize gi = loadQueue[i];
					gi.StartTask();
					while (true)
					{
						yield return null;
						if (gi.IsFinish())
						{
							break;
						}
					}
				}
			}
#endif
			//清单文件下载完成
			yield return UpdateFinish();
		}

		/// <summary>
		/// 初始化检查更新完成(失败或成功)
		/// </summary>
		public static IEnumerator UpdateFinish() {
			IGameInitialize.UpdateProgress(0.9f, "load_finish");
			yield return null;
#if !UNITY_EDITOR || ASSETBUNDLE_TEST
			IGameInitialize gi = instance.gameObject.AddComponent<LoadLocalManifestMap>();
			gi.StartTask();
			while (true) {
				yield return null;
				if (gi.IsFinish()) {
					break;
				}
			}
#endif
			yield return ToLuaInitialize();

			if (instance.OnCheckFileFinish != null) {
				instance.OnCheckFileFinish();
				instance.OnCheckFileFinish = null;
			}
			yield return null;
		}

		//下载lua完成,初始化lua
		private static IEnumerator ToLuaInitialize() {
			float lastTime = Time.realtimeSinceStartup;
			LogManager.Log("初始化脚本引擎");
			IGameInitialize.UpdateProgress(1f, "load_finish");
			yield return null;

			//lua文件加载
#if ENABLE_LUA
			yield return LuaManager.instance.InitLuaFile();
#endif
			yield return null;

			LogManager.Log("脚本引擎初始化完成，耗时：" + (Time.realtimeSinceStartup - lastTime));
		}
	}
}
