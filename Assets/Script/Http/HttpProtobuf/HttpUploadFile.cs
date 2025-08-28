using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Hukiry.Http
{
    /// <summary>
    /// http 文件上传
    /// </summary>
    public class HttpUpload
    {
        /// <summary>
        /// 是否有上载日志
        /// </summary>
        public static bool UploadLog = true;

        /// <summary>
        /// 一次上传大小
        /// </summary>
        const short BUFFER_SIZE = 4096;

        /// <summary>
        /// URL路径 不含文件路径
        /// </summary>
        private string urlPath;

        /// <summary>
        /// URL路径 含文件路径
        /// </summary>
        private string urlFilePath;

        /// <summary>
        /// 请求地址
        /// </summary>
        private string filePath;

        /// <summary>
        /// 文件名
        /// </summary>
        private string fileName;

        /// <summary>
        /// 上载进度回调
        /// </summary>
        private Action<HttpUpload, float> uploadProgress;

        /// <summary>
        /// 成功或失败回调
        /// </summary>
        private Action<string, string, bool> uploadCallback;

        /// <summary>
        /// 当前上载大小
        /// </summary>
        public long uploadLength { get; private set; }

        /// <summary>
        /// 总文件大小
        /// </summary>
        public long totalLength { get; private set; }

        /// <summary>
        /// 用于记录上一秒上载了多少
        /// </summary>
        public long lastuploadLength;

        FileStream fsStream = null;
        HttpWebRequest webRequest = null;
        HttpWebResponse response = null;
        Thread thread = null;

        public HttpUpload(Action<HttpUpload, float> _uploadProgress, Action<string, string, bool> _uploadCallback)
        {
            this.uploadProgress = _uploadProgress;
            this.uploadCallback = _uploadCallback;
        }

        /// <summary>
        /// 启动上载
        /// </summary>
        /// <param name="mUrlPath"></param>
        /// <param name="mFilePath"></param>
        public void StartUpload(string mUrlPath, string mFilePath, string fileName)
        {
            this.urlPath = mUrlPath;
            this.urlFilePath = Path.Combine(mUrlPath, fileName);
            //this.filePath = Path.Combine(PathConifg.AppCachePath, mFilePath);
            this.fileName = fileName;

            TryRequest();
        }

        private void TryRequest()
        {
            ClearUp();

            thread = new Thread(UploadRequest);
            thread.Start();
        }

        private void UploadRequest()
        {
            // 时间戳，用做boundary
            string timeStamp = DateTime.Now.Ticks.ToString("x");

            //根据uri创建HttpWebRequest对象
            webRequest = (HttpWebRequest)WebRequest.Create(new Uri(this.urlPath));
            webRequest.Method = "POST";
            webRequest.AllowWriteStreamBuffering = false; //对发送的数据不使用缓存
            webRequest.Timeout = 300000;  //设置获得响应的超时时间（300秒）
            webRequest.ContentType = "multipart/form-data; boundary=" + timeStamp;

            //文件
            fsStream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fsStream);

            //头信息
            string boundary = "--" + timeStamp;
            string dataFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n";
            string header = string.Format(dataFormat, "file", this.fileName);
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(header);

            //结束边界
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + timeStamp + "--\r\n");

            totalLength = fsStream.Length + postHeaderBytes.Length + boundaryBytes.Length;

            webRequest.ContentLength = totalLength;//请求内容长度

            try
            {
                byte[] buffer = new byte[BUFFER_SIZE];

                int size = binaryReader.Read(buffer, 0, buffer.Length);
                Stream postStream = webRequest.GetRequestStream();

                OnUploadProgress(uploadLength * 1.0f / totalLength);
                //发送请求头部消息
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    uploadLength += size;
                    size = binaryReader.Read(buffer, 0, buffer.Length);

                    OnUploadProgress(uploadLength * 1.0f / totalLength);
                }

                //添加尾部边界
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应
                response = (HttpWebResponse)webRequest.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string returnValue = readStream.ReadToEnd();
                response.Close();
                readStream.Close();

                UploadSuccess(returnValue);
            }
            catch (WebException ex)
            {
                UploadFailure(ex.Message);
            }
        }

        /// <summary>
        /// 上载成功
        /// </summary>
        private void UploadSuccess(string result)
        {
            Loom.QueueOnMainThread(() =>
            {
                uploadCallback?.Invoke(this.urlFilePath, result, true);
            });

            Dispose();
        }

        /// <summary>
        /// 上载失败
        /// </summary>
        private void UploadFailure(string msg)
        {
            Loom.QueueOnMainThread(() =>
            {
                uploadCallback?.Invoke(this.urlFilePath, msg, false);
            });

            Dispose();
        }

        /// <summary>
        /// 取消上载
        /// </summary>
        public void StopUpload()
        {
            Loom.QueueOnMainThread(() =>
            {
                uploadCallback?.Invoke(this.filePath, "", false);
            });

            Dispose();
        }

        private bool IsRetryable(WebException ex)
        {
            return
                ex.Status == WebExceptionStatus.ReceiveFailure ||
                ex.Status == WebExceptionStatus.ConnectFailure ||
                ex.Status == WebExceptionStatus.KeepAliveFailure;
        }

        /// <summary>
        /// 清理
        /// </summary>
        private void ClearUp()
        {
            if (webRequest != null) webRequest.Abort();
            if (response != null) response.Close();
            if (fsStream != null) fsStream.Close();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            ClearUp();
            if (thread != null) thread.Abort();
        }

        /// <summary>
        /// 当前进度
        /// </summary>
        /// <param name="progress"></param>
        protected void OnUploadProgress(float progress)
        {
            Loom.QueueOnMainThread(() =>
            {
                uploadProgress?.Invoke(this, progress);
            });
        }
    }
}
