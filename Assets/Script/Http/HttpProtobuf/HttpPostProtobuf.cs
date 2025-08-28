using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Hukiry.Http
{

    /// <summary>
    /// http 二进制请求
    /// </summary>
    public class HttpPostProtobuf
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        private string url;

        /// <summary>
        /// 协议号
        /// </summary>
        private int cmd;

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

        /// <summary>
        /// 发送成功，返回的数据
        /// </summary>
        private static Action<int, byte[]> _onRecvPacketCallback;

        /// <summary>
        /// 连接错误
        /// </summary>
        private static Action<int, string> _onConnectionErrorCallback;

        public static void SetOnRecvPacketCallback(Action<int, byte[]> value)
        {
            _onRecvPacketCallback = value;
        }

        public static void SetOnConnectionErrorCallback(Action<int, string> value)
        {
            _onConnectionErrorCallback = value;
        }

        public static HttpPostProtobuf SendPacket(int cmd, byte[] buffer)
        {
            return new HttpPostProtobuf(cmd, buffer);
        }

        public HttpPostProtobuf(int cmd, byte[] buffer)
        {
            retryTime = 3;//WorldRuleC.Protobuf_Timeout_Retry;

            //连接地址
            this.url = "http://www.com.api";//AppConfig.Instance.GetServerUrl()
            this.cmd = cmd;

            UnityEngine.Debug.LogError("URL:" + url);

            byte[] packetBytes = HttpPacket.Encode(cmd, buffer).ToBytes();
            string base64 = Convert.ToBase64String(packetBytes);

            StringBuilder _args = new StringBuilder();
            _args.AppendFormat("&{0}={1}", "time", DateTime.Now.Ticks.ToString());
            _args.AppendFormat("&{0}={1}", "body", base64);
            this.argsBuffer = Encoding.GetEncoding("utf-8").GetBytes(_args.ToString());
            TryRequest();
        }

        private void TryRequest()
        {
            ClearUp();

            thread = new Thread(ProcessRequest);
            thread.Start();
        }

        private void ProcessRequest()
        {
            try
            {
                request = (HttpWebRequest)WebRequest.Create(this.url);
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 3 * 1000;
                request.ServicePoint.Expect100Continue = false;// 默认为true

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(this.argsBuffer, 0, this.argsBuffer.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string retString = myreader.ReadToEnd();

                ConnectionFinsh(Convert.FromBase64String(retString));
            }
            catch (WebException ex)
            {
                ConnectionFail(ex.Message);
            }
        }

        private void ConnectionFinsh(byte[] buffer)
        {
            try
            {
                int bufferLen = buffer.Length;
                int pos = 0;
                while (pos < bufferLen)
                {
                    //拿出cmd
                    int cmd = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(buffer, pos));
                    pos += 4;

                    //拿出body长度
                    int bodyLen = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(buffer, pos));
                    pos += 4;

                    //拿出消息体
                    MemoryStream body = new MemoryStream(buffer, pos, bodyLen);
                    Loom.QueueOnMainThread(() =>
                    {
                        _onRecvPacketCallback?.Invoke(cmd, body.ToArray());
                    });
                    pos += bodyLen;
                }
            }
            catch (SystemException e)
            {
                Loom.QueueOnMainThread(() =>
                {
                    _onConnectionErrorCallback?.Invoke(this.cmd, "协议解析错误：" + e.Message);
                });
                Dispose();
            }
        }

        private void ConnectionFail(string error)
        {
            if (retryTime > 0)
            {
                UnityEngine.Debug.LogError(string.Format("HttpProtobuf发送错误，Url:{0}, Cmd:{1}, error:{2}, 开始第 {3} 次尝试", this.url, cmd, error, (3 - this.retryTime) + 2));
                retryTime--;

                TryRequest();
            }
            else
            {
                Loom.QueueOnMainThread(() =>
                {
                    _onConnectionErrorCallback?.Invoke(this.cmd, error);
                });
                Dispose();
            }
        }

        private bool IsRetryable(WebException ex)
        {
            return
                ex.Status == WebExceptionStatus.ReceiveFailure ||
                ex.Status == WebExceptionStatus.ConnectFailure ||
                ex.Status == WebExceptionStatus.KeepAliveFailure;
        }

        private void ClearUp()
        {
            if (request != null) request.Abort();
            if (response != null) response.Close();
            if (responseStream != null) responseStream.Close();
            if (myreader != null) myreader.Close();
        }

        public void Dispose()
        {
            if (thread != null) thread.Abort();
        }
    }
}
