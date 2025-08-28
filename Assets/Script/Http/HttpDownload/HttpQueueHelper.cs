using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hukiry.Http
{
    public class HttpQueueInfo {
		public string filePath;//文件相对路径
		public long size;//文件大小 B
	}

	/// <summary>
	/// HTTP队列下载管理
	/// </summary>
	public class HttpQueueHelper {

		/// <summary>
		/// 下载相对路径
		/// </summary>
		private string urlPath;

		/// <summary>
		/// 本地保存路径
		/// </summary>
		private string localSavePath;

		/// <summary>
		/// 下载进度回调 <当前进度、下载速度(kb)>
		/// </summary>
		public Action<float, int> progressCallback;

		/// <summary>
		/// 下载过程中，在某些时机设置一部分下载完成的数据
		/// </summary>
		public Action<Queue<string>> finishProgressCallback;

		/// <summary>
		/// 全部下载成功完成回调
		/// </summary>
		public Action finishCallback;

		/// <summary>
		///  失败回调
		/// </summary>
		public Action failureCallback;

		/// <summary>
		/// 要下载的文件路径
		/// </summary>
		private Queue<HttpQueueInfo> paths;

		/// <summary>
		/// 下载成功的路径
		/// </summary>
		private Queue<string> successPaths = new Queue<string>();

		/// <summary>
		/// 当前已下载大小 (B)
		/// </summary>
		private long mDownloadSize = 0;

		/// <summary>
		/// 总大小 (KB)
		/// </summary>
		public float totalSize { get; private set; }

		/// <summary>
		/// 下载完成
		/// </summary>
		private bool finish = false;

		/// <summary>
		/// 上次记录的时间
		/// </summary>
		private float lastDownloadTime;

		/// <summary>
		/// 这一秒一共下载了多少资源
		/// </summary>
		private int lastDownloadSize;

		/// <summary>
		/// 正在下载中的
		/// </summary>
		private Dictionary<string, HttpDownload> _loadingDic = new Dictionary<string, HttpDownload>();
		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="mUrlPath">暂时未使用</param>
		/// <param name="mLocalSavePath">保存到本地的路径</param>
		public HttpQueueHelper(string mUrlPath, string mLocalSavePath) {
			this.urlPath = mUrlPath;
			this.localSavePath = mLocalSavePath;
		}

		public void SetDownLoadQueue(Queue<HttpQueueInfo> mPaths) {
			this.paths = mPaths;

			mPaths.ToList().ForEach(info => {
				totalSize += info.size;
			});
			totalSize = totalSize / 1024;
		}

		/// <summary>
		/// 开始下载
		/// </summary>
		/// <param name="mPaths"></param>
		public void StartDownload() {
			if (HttpDownload.DownloadLog) Debug.Log(string.Format("开始下载，总文件数:{0}， 总大小:{1}", paths.Count, totalSize));

			//开始下载时，直接启动两条线
			NextDownload();
			NextDownload();

			lastDownloadTime = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// 继续尝试下载
		/// </summary>
		public void RetryDownload() {
			if (HttpDownload.DownloadLog) Debug.Log(string.Format("继续尝试下载，剩余文件数:{0}", paths.Count));
			//开始下载时，直接启动两条线
			NextDownload();
			NextDownload();
		}

		private void NextDownload() {
			if (this.paths.Count > 0) {
				NextDownload(this.paths.Dequeue());
			}
		}

		private void NextDownload(HttpQueueInfo info) {
			if (!_loadingDic.ContainsKey(info.filePath)) 
			{
				HttpDownload hd = new HttpDownload(this.localSavePath, DownloadProgress, DownloadFinish);
				hd.StartDownload(this.urlPath, info.filePath);

				_loadingDic[info.filePath] = hd;
			}
		}

		/// <summary>
		/// 单个文件下载进度
		/// </summary>
		/// <param name="hd"></param>
		/// <param name="pro"></param>
		private void DownloadProgress(HttpDownload hd, float pro) {
			if (Time.realtimeSinceStartup - lastDownloadTime > 1f) {
				lastDownloadSize = 0;
				_loadingDic.Values.ToList().ForEach((_hd => 
				{
					lastDownloadSize += (int)(_hd.downLength - _hd.lastDownLength);
					_hd.lastDownLength = _hd.downLength;
				}));
				lastDownloadTime = Time.realtimeSinceStartup;
			}

			if (progressCallback != null) progressCallback(downloadSize * 1f / totalSize, lastDownloadSize / 1024);
		}

		/// <summary>
		/// 单个文件下载结束回调
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="isSuccess"></param>
		private void DownloadFinish(string filePath, string relativePath, string msg, bool isSuccess) {
			if (isSuccess) {
				if(HttpDownload.DownloadLog) UnityEngine.Debug.Log(string.Format("文件下载完成:{0}", filePath));
				if (_loadingDic.ContainsKey(relativePath)) {
					HttpDownload hd = _loadingDic[relativePath];
					mDownloadSize += hd.totalLength;

					successPaths.Enqueue(relativePath);
					_loadingDic.Remove(relativePath);
				}
				if (this.paths.Count == 0 && _loadingDic.Count == 0) {
					AllDownloadFinish();
				}else {
					NextDownload();
				}
			}else {
				Debug.LogError(string.Format("下载文件失败:{0}", filePath));
				//只要失败，就把当前正在下载的都停掉，重新放到待下载文件结尾
				_loadingDic.Values.ToList().ForEach(hd => {
					this.paths.Enqueue(new HttpQueueInfo { filePath = hd.relativePath, size = hd.totalLength });
					hd.Dispose();
				});
				_loadingDic.Clear();

				//先把成功的记录保存一下
				if (finishProgressCallback != null) {
					finishProgressCallback(successPaths);
					successPaths = new Queue<string>();
				}

				//下载失败
				FailureCallback();
			}
		}

		/// <summary>
		/// 所有文件下载完成
		/// </summary>
		private void AllDownloadFinish() {
			finish = true;
			if (finishCallback != null) finishCallback();
		}

		/// <summary>
		/// 下载失败回调
		/// </summary>
		private void FailureCallback() {
			if (failureCallback != null) failureCallback();
		}

		/// <summary>
		/// 是否下载完成
		/// </summary>
		/// <returns></returns>
		public bool IsDone() {
			return finish;
		}

		/// <summary>
		/// 当前已下载大小，KB
		/// </summary>
		public float downloadSize {
			get {
				long tempSize = mDownloadSize;
				_loadingDic.Values.ToList().ForEach((_hd => {
					tempSize += _hd.downLength;
				}));
				return tempSize / 1024;
			}
		}
	}
}


///下载例子
///
//private IEnumerator DownloadVersionRecord()
//{
//	HttpQueueHelper helper = new HttpQueueHelper(PathConifg.AssetsUrlPath, PathConifg.AppTempCachePath);
//	helper.progressCallback = (_pro, _speed) => { DownloadProgress(helper, _pro, _speed); };//进度回调
//	helper.finishProgressCallback = FinishProgressCallback;//下载完成部分时回调
//	helper.finishCallback = AllDownloadFinish;//全部下载完成回调
//	//中途下载错误回调
//	helper.failureCallback = () =>
//	{
//		DownloadFilureExecute(() =>
//		{
//			helper.RetryDownload();//继续下载
//		});
//	};
//	helper.SetDownLoadQueue(GIController.updateVersionRecord);//设置下载队列
//	helper.StartDownload();
//	while (!helper.IsDone())
//	{
//		yield return new WaitForEndOfFrame();//等待下载
//	}
//}