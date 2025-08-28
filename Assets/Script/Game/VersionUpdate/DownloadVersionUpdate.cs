
using Hukiry;
using Hukiry.Http;
using Hukiry.Socket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using PathConifg = Hukiry.AssetBundleConifg;

namespace HukiryInitialize {
	/// <summary>
	/// 下载版本更新
	/// </summary>
	public class DownloadVersionUpdate : IGameInitialize {


		protected override IEnumerator Coroutines() {
			//有更新文件
			if (GIController.updateVersionRecord.Count > 0) {
				UpdateProgress(1f, "assets_update");
				yield return DownloadVersionRecord();
			}
			FinishTask();
		}

		/// <summary>
		/// 第一步 下载需要更新的文件
		/// </summary>
		/// <returns></returns>
		private IEnumerator DownloadVersionRecord()
		{
			HttpQueueHelper helper = new HttpQueueHelper(AssetsUrlPath, PathConifg.AppTempCachePath);
			//进度回调
			helper.progressCallback = (_pro, _speed) => { DownloadProgress(helper, _pro, _speed); };

			//下载完成部分时回调
			helper.finishProgressCallback = FinishProgressCallback;

			//全部下载完成回调
			helper.finishCallback = AllDownloadFinish;

			//中途下载错误回调
			helper.failureCallback = () =>
			{
				DownloadFilureExecute(() =>
				{
					helper.RetryDownload();//继续下载
				});
			};

			//设置下载队列
			helper.SetDownLoadQueue(GIController.updateVersionRecord);

            //开始下载 超出下载的尺寸，提示弹框
            //float tipSize = 6; //wifi下载提示
            //if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            //{
            //    tipSize = 2; //数据下载提示
            //}
            //float totalSize = helper.totalSize / 1024;
            //if (totalSize >= tipSize) { 
            //string tip = "download_size_tips:" + totalSize.ToString("f2");
            //	//弹框，确认下载，或退出游戏
            //	GIController.ShowPanelMsgBox?.Invoke(tip, () =>
            //	{
            //		helper.StartDownload();
            //	}, () =>
            //	{
            //		Application.Quit();
            //	});
            //}
            //else {
            //helper.StartDownload();
            //}

            helper.StartDownload();
			//等待下载
			while (!helper.IsDone())
			{
				yield return new WaitForEndOfFrame();
			}
		}

		/// <summary>
		/// 下载失败处理，弹窗给用户，用户点了后再继续下载
		/// </summary>
		/// <param name="error"></param>
		public void DownloadFilureExecute(Action failBackCall) {
			Debug.LogError($"下载失败处理，弹窗给用户，用户点了后再继续下载");
			GIController.ShowPanelMsgBox?.Invoke(1, failBackCall, null);
		}

		public override void FailureTask(string title, string error, string args) {
			
		}

		/// <summary>
		/// 下载进度展示
		/// </summary>
		private static void DownloadProgress(HttpQueueHelper helper, float pro, int speed) {
			string tip = ("assets_update");
			string downTip = "download_progress"+(helper.totalSize / 1024).ToString("f2")+ speed.ToString("f2");
			GIController.UpdateProgress(pro, tip, downTip);
		}

		/// <summary>
		/// 下载到一半时，网络错误，先保存下载好的记录，如果用户重启应用，可接着下载
		/// </summary>
		/// <param name="finishQueue">ab路径清单 <see cref="HttpQueueInfo.filePath"/></param>
		private static void FinishProgressCallback(Queue<string> finishQueue) {
			//缓存下载成功文件清单
			if (!Directory.Exists(PathConifg.AppCachePath)) Directory.CreateDirectory(PathConifg.AppCachePath);
			List<string> list = new List<string>();
			list.Add(MainGame.gameVersion.version);
			list.AddRange(finishQueue.ToArray());
			File.WriteAllLines(Path.Combine(PathConifg.AppCachePath, PathConifg.TempDownload), finishQueue.ToArray());
		}

		/// <summary>
		/// 所有文件下载完成，在这里处理后续逻辑
		/// </summary>
		private static void AllDownloadFinish() {
			Debug.Log("所有文件全部下载完成");
			//copy下载好的文件到缓存目标，替换旧文件，等于替换了新的文件清单
			FileHelper.CopyDirectoryAndRename(PathConifg.AppTempCachePath, PathConifg.AppCachePath);
			FileHelper.DeleteDirectory(PathConifg.AppTempCachePath, true);

			//下载完成清空
			string tempFile = Path.Combine(PathConifg.AppCachePath, PathConifg.TempDownload);
			if (File.Exists(tempFile))
			{
				File.Delete(tempFile);
			}
		}
	}
}