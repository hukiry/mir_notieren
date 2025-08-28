using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Hukiry.Http
{

    /// <summary>
    /// http请求服务器，数据拼接： k=v&k=v
    /// </summary>
    public class HttpPost
    {
        /// <summary>
        /// 请求超时时间
        /// </summary>
        const short TIME_OUT = 6000;

        /// <summary>
        /// 回调
        /// </summary>
        private Action<bool, string> endCallback;

        /// <summary>
        /// 请求路径
        /// </summary>
        private string url;

        /// <summary>
        /// 发送的数据
        /// </summary>
        private byte[] argsBuffer;

        /// <summary>
        /// 失败重试剩余次数
        /// </summary>
        private int retryTime;

        Stream responseStream = null;
        HttpWebResponse response = null;
        HttpWebRequest request = null;
        StreamReader myreader = null;
        Thread thread = null;

        StringBuilder _args;

        public HttpPost(string url, Dictionary<string, string> args = null, Action<bool, string> callback = null)
        {
            this.url = url;
            this.endCallback = callback;

            if (args != null)
            {
                _args = new StringBuilder();
                foreach (var kv in args)
                {
                    _args.AppendFormat("{0}={1}&", kv.Key, kv.Value);
                }
                int len = _args.Length;
                _args.Remove(len - 1, 1);

                this.argsBuffer = Encoding.GetEncoding("utf-8").GetBytes(_args.ToString());
            }

            TryRequest();
        }

        private void TryRequest()
        {
            ClearUp();

            thread = new Thread(ProcessDownload);
            thread.Start();
        }

        private void ProcessDownload()
        {
            try
            {
                request = (HttpWebRequest)WebRequest.Create(this.url);
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = TIME_OUT;
                request.ServicePoint.Expect100Continue = false;// 默认为true

                if (this.argsBuffer != null)
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(this.argsBuffer, 0, this.argsBuffer.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string retString = myreader.ReadToEnd();

                ActionResult(true, retString);
            }
            catch (Exception ex)
            {
                ActionResult(false, ex.Message);
            }

            ClearUp();
        }

        private void ActionResult(bool code, string content)
        {
            if (code)
            {
                UnityEngine.Debug .Log(string.Format("Http请求成功：url:{0}, 结果:{1}", this.GetUrl(), content));
                this.endCallback?.Invoke(code, content);
            }
            else
            {
                UnityEngine.Debug .LogError(string.Format("Http请求错误，剩余尝试次数：{0}, url:{1}, error:{2}", this.retryTime, this.GetUrl(), content));
                if (retryTime > 0)
                {
                    TryRequest();
                    retryTime--;
                }
                else
                {
                    this.endCallback?.Invoke(code, content);
                }
            }
        }

        private bool IsRetryable(WebException ex)
        {
            return ex.Status == WebExceptionStatus.ReceiveFailure ||  ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.KeepAliveFailure;
        }

        private string GetUrl()
        {
            return this._args == null?this.url: this.url + "?" + this._args.ToString();
        }

        /// <summary>
        /// 清理
        /// </summary>
        private void ClearUp()
        {
            if (request != null) request.Abort();
            if (response != null) response.Close();
            if (responseStream != null) responseStream.Close();
            if (myreader != null) myreader.Close();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()=> this.ClearUp();
    }
}
