using System;
using System.IO;
using System.Net;
using System.Text;

namespace Hukiry.Http
{

    /// <summary>
    ///  请求http服务器 数据json
    /// </summary>
    public class HttpJson
	{
		/// <summary>
		/// 请求路径
		/// </summary>
		private string url;
		private byte[] argsBuffer;
		/// <summary>
		/// 失败重试剩余次数
		/// </summary>
		private int retryTime;
		Stream responseStream = null;
		HttpWebResponse response = null;
		HttpWebRequest request = null;
		private static Action<string> _onRecvPacketCallback;
		private static Action<string> _onConnectionErrorCallback;
		private readonly string HttpMothod;
		/// <summary>
		/// 发送成功，返回的数据
		/// </summary>
		public static void RegeditOnRecvPacketCallback(Action<string> value) => _onRecvPacketCallback = value;
		/// <summary>
		/// 连接错误
		/// </summary>
		public static void RegeditOnConnectionErrorCallback(Action<string> value) => _onConnectionErrorCallback = value;
		/// <summary>
		/// 发送的数据
		/// </summary>
		public static HttpJson SendPacket(string url, string bufferJson,string mothod = WebRequestMethods.Http.Post) => new HttpJson(url, bufferJson, mothod);
		private HttpJson(string url,string jsonString, string mothod)
		{
			this.HttpMothod = mothod;
			UnityEngine.Debug.Log("URL:" + url);
			retryTime = 3;
			this.url = url;
			this.argsBuffer = Encoding.GetEncoding("utf-8").GetBytes(jsonString.ToString());
			TryRequest();
		}

		private void TryRequest() {
			if (request != null) request.Abort();
			if (response != null) response.Close();
			if (responseStream != null) responseStream.Close();

			try {
				request = (HttpWebRequest)WebRequest.Create(this.url);
				request.Method = this.HttpMothod;
				request.ContentType = "application/x-www-form-urlencoded";
				request.Timeout = 3 * 1000;
				request.ServicePoint.Expect100Continue = false;// 默认为true

				using (Stream stream = request.GetRequestStream()) {
					stream.Write(this.argsBuffer, 0, this.argsBuffer.Length);
				}

				response = (HttpWebResponse)request.GetResponse();
				responseStream = response.GetResponseStream();
				StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
				string retString = myreader.ReadToEnd();

				ConnectionFinsh(retString);
			} catch (WebException ex) {
				if (retryTime > 0)
				{
					UnityEngine.Debug .LogError(string.Format("HttpProtobuf发送错误，Url:{0},error:{2}, 开始第 {3} 次尝试", this.url, ex.Message, (3 - this.retryTime) + 2));
					retryTime--;
					TryRequest();
				}
				else
				{
					_onConnectionErrorCallback?.Invoke(ex.Message);

				}
			}
		}

		private void ConnectionFinsh(string body)
		{
			try
			{
				_onRecvPacketCallback?.Invoke(body);
			}
			catch (SystemException e)
			{

				_onConnectionErrorCallback?.Invoke("协议解析错误：" + e.Message);

			}
		}
	}
}
