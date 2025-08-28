using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Hukiry.Http
{
	/// <summary>
	///  http 热更下载
	/// </summary>
	public class HttpDownload
    {
		/// <summary>
		/// 是否有下载日志
		/// </summary>
		public static bool DownloadLog = true;

		/// <summary>
		/// 一次读取大小
		/// </summary>
		const short BUFFER_SIZE = 1024;

		/// <summary>
		/// URL路径 不含文件路径
		/// </summary>
		private string urlPath;

		/// <summary>
		/// 文件相对路径
		/// </summary>
		public string relativePath { get; private set; }

		/// <summary>
		/// 请求地址
		/// </summary>
		private string httpUrl;

		/// <summary>
		/// 保存路径
		/// </summary>
        private string savePath;

		/// <summary>
		/// 保存路径(含文件名)
		/// </summary>
		private string toSavePath;

		/// <summary>
		/// 下载进度回调
		/// </summary>
		private Action<HttpDownload, float> downloadProgress;

		/// <summary>
		/// 成功或失败回调
		/// </summary>
		private Action<string, string, string, bool> downloadCallback;

		/// <summary>
		/// 当前下载大小
		/// </summary>
		public long downLength { get; private set; }

		/// <summary>
		/// 总文件大小
		/// </summary>
		public long totalLength { get; private set; }

		/// <summary>
		/// 用于记录上一秒下载了多少
		/// </summary>
		public long lastDownLength;

		/// <summary>
		/// 失败尝试次数
		/// </summary>
		private int retryTime = 0;

		FileStream fsStream = null;
		HttpWebResponse response = null;
		HttpWebRequest webRequest = null;
		Thread thread = null;

		public HttpDownload(string savePath, Action<HttpDownload, float> _downloadProgress, Action<string, string, string, bool> _downloadCallback)
        {
            this.savePath = savePath;
			this.downloadProgress = _downloadProgress;
			this.downloadCallback = _downloadCallback;
		}

        /// <summary>
        /// 启动下载
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <param name="savePath"></param>
        public void StartDownload(string mUrlPath, string mFilePath)
        {
			this.urlPath = mUrlPath;
			this.relativePath = mFilePath;
			this.httpUrl  = Path.Combine(mUrlPath, mFilePath);
			this.toSavePath = Path.Combine(savePath, mFilePath);

			TryRequest();
		}

		private void TryRequest() {
			ClearUp();

			thread = new Thread(ProcessDownload);
			thread.Start();
		}

		/// <summary>
		/// 创建一个文件夹
		/// </summary>
		/// <param name="folder"></param>
		 void CreateDirectory(string folder)
		{
			if (folder.LastIndexOf('.') != -1)
			{
				folder = folder.Substring(0, folder.LastIndexOf('/'));
			}
			folder = folder.Replace("\\", "/");
			if (folder[folder.Length - 1] != Path.DirectorySeparatorChar)
				folder += Path.DirectorySeparatorChar;
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);
		}

		private void ProcessDownload()
        {
			if(DownloadLog) UnityEngine.Debug.Log(string.Format("开始下载:{0}", httpUrl));

			this.CreateDirectory(toSavePath);

			fsStream = new FileStream(toSavePath, FileMode.Create);

			try
            {
				//创建链接
				webRequest = WebRequest.CreateHttp(httpUrl);
                webRequest.Timeout = 3 * 1000;
				webRequest.KeepAlive = false;
				webRequest.Proxy = WebRequest.DefaultWebProxy;//不使用代理，减少自动搜索代理的过程，提升性能  
				response = (HttpWebResponse)webRequest.GetResponse();

                totalLength = response.ContentLength;

				retryTime = 3;	//连接成功，重置失败尝试次数

				OnDownloadProgress(downLength * 1.0f / totalLength);
				if (response.StatusCode == HttpStatusCode.OK) {
					fsStream.SetLength(0); downLength = 0;
				}

				int read = 0;
                byte[] buffer = new byte[1 * BUFFER_SIZE];
				//开始读并写入文件
				Stream reader = response.GetResponseStream();
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
					downLength += read;

					fsStream.Write(buffer, 0, read);
					OnDownloadProgress(downLength * 1.0f / totalLength);
                }

				if (downLength != totalLength) {
					throw new WebException("连接失败，文件未下载完成", WebExceptionStatus.ConnectFailure);
				}
				if (fsStream != null) fsStream.Close();
				DownloadSuccess();
			}
            catch (WebException e)
            {
				GC.Collect();

				if (retryTime > 0) {
					Thread.Sleep(2000);//当前下载线程休息2ms
					UnityEngine.Debug.LogError(string.Format("下载错误，Url:{0}, error:{1}, 开始第 {2} 次尝试", this.httpUrl, e, (3 - this.retryTime) + 2));

					retryTime--;

					TryRequest();//重试
				} else {
					DownloadFailure(e.Message);
				}
			}
		}

		/// <summary>
		/// 下载成功
		/// </summary>
		private void DownloadSuccess() {
			Loom.QueueOnMainThread(() => {
				downloadCallback?.Invoke(this.httpUrl, this.relativePath, "", true);
			});

			Dispose();
		}

		/// <summary>
		/// 下载失败
		/// </summary>
		private void DownloadFailure(string msg) {
			Loom.QueueOnMainThread(() => {
				downloadCallback?.Invoke(this.httpUrl, this.relativePath, msg, false);
			});
			
			Dispose();
		}

		/// <summary>
		/// 取消下载
		/// </summary>
		public void StopDownload() {
			Loom.QueueOnMainThread(() => {
				downloadCallback?.Invoke(this.httpUrl, this.relativePath, "", false);
			});
			
			Dispose();
		}

		private bool IsRetryable(WebException ex) {
			return
				ex.Status == WebExceptionStatus.ReceiveFailure ||
				ex.Status == WebExceptionStatus.ConnectFailure ||
				ex.Status == WebExceptionStatus.KeepAliveFailure;
		}

		/// <summary>
		/// 清理
		/// </summary>
		private void ClearUp() {
			if (webRequest != null) webRequest.Abort();
			if (response != null) response.Close();
			if (fsStream != null) fsStream.Close();
		}

		/// <summary>
		/// 销毁
		/// </summary>
		public void Dispose() {
			ClearUp();
			if (thread != null) thread.Abort();
		}

		/// <summary>
		/// 当前进度
		/// </summary>
		/// <param name="progress"></param>
		protected void OnDownloadProgress(float progress)
        {
			Loom.QueueOnMainThread(() =>
			{
				downloadProgress?.Invoke(this, progress);
			});
        }
    }
}
