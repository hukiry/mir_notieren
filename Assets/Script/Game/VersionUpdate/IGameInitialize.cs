using System.Collections;
using UnityEngine;

namespace HukiryInitialize {
	/// <summary>
	/// Coroutines() 子类实现，FinishTask，StopTask 子类调用
	/// </summary>
	public abstract class IGameInitialize : MonoBehaviour{
		/// <summary>
		/// 重试次数，如果失败，直接以无网络状态进入游戏
		/// </summary>
		private const int RETRY_COUNT = 2;

		protected int retryCount = 0;
		protected bool isFinish;
        private static string _assetsUrlPath;

        public static string AssetsUrlPath
		{
			get
			{
				return $"{AssetsUrlPathVersion}{MainGame.gameVersion.GetUpdatePackageVersionName()}/";
			}
		}

		public static string AssetsUrlPathVersion
		{
			get
			{
				if (_assetsUrlPath == null)
				{
					EWorkMode workMode = (EWorkMode)MainGame.gameVersion.workMode;
					//url目录+平台目录+版本目录
					_assetsUrlPath = $"{MainGame.gameVersion.webUrl}/{Hukiry.AssetBundleConifg.buildTarget}/{workMode}/";
				}
				return _assetsUrlPath;
			}
		}
		private void Start() {
			retryCount = RETRY_COUNT;
		}

		/// <summary>
		/// 开始任务：全局
		/// </summary>
		public virtual void StartTask() {
			StartCoroutine(Coroutines());
		}

		/// <summary>
		/// 停止任务
		/// </summary>
		public virtual void StopTask() {
			StopCoroutine(Coroutines());
		}

		//子类实现
		protected abstract IEnumerator Coroutines();

		/// <summary>
		/// 任务完成：子类调用
		/// </summary>
		public virtual void FinishTask() {
			StopTask();
			isFinish = true;
		}

		/// <summary>
		/// 1，任务失败：下载失败回调，停止任务
		/// 2，尝试重新下载，超过尝试次数，弹框网络失败
		/// </summary>
		public virtual void FailureTask(string title, string error, string args) {
			LogManager.LogError($"任务失败:{title}, 参数:{args}, 错误消息:{error},retryCount:{retryCount}");
			StopTask();
			//确认重新下载，回调开始任务
			GIController.ShowPanelMsgBox?.Invoke(2, StartTask, ()=> { Application.Quit(); });

		}

		/// <summary>
		/// 任务是否完成:全局
		/// </summary>
		public virtual bool IsFinish() {
			return isFinish;
		}

		public static void UpdateProgress(float progress, string tip) {
			GIController.UpdateProgress?.Invoke(progress, tip,string.Empty);
		}
	}
}