using System;
using System.Collections;
using UnityEngine.Networking;

namespace Hukiry.SDK
{

    /// <summary>
    /// 获取keys json格式
    /// </summary>
    public class UwrGetAppleJson
	{
		/// <summary>
		/// 剩余尝试次数
		/// </summary>
		private int retryTime = 0;

		/// <summary>
		/// 回调函数
		/// </summary>
		private Action<bool, string> callback;

		/// <summary>
		/// 请求路径
		/// </summary>
		private string url;

		private const string headKey = "Content-Type";
		private const string headValue = "application/json";

		public UwrGetAppleJson(string address, Action<bool, string> callback = null)
		{
            retryTime = 2;

			this.callback = callback;
			this.url = address;
			SdkManager.ins.StartCoroutine(StartSendPost());
		}

		IEnumerator StartSendPost()
		{
            UnityEngine.LogManager.Log($"AppleJson request：url:{url}");
			UnityWebRequest uwr =UnityWebRequest.Get(this.url);
            uwr.SetRequestHeader(headKey, headValue);
			uwr.timeout = 5;
			uwr.downloadHandler = new DownloadHandlerBuffer();

			yield return uwr.SendWebRequest();
			bool code;
			string content;
			if (uwr.isNetworkError || uwr.isHttpError)
			{
				code = false;
				content = uwr.error;
			}
			else
			{
				code = true;
				content = uwr.downloadHandler.text;
			}
			uwr.Dispose();

			ActionResult(code, content);
		}

		private void ActionResult(bool code, string content)
		{
			if (code)
			{
                UnityEngine.LogManager.Log($"AppleJson request successful：url:{url}, result:{content}");
				callback?.Invoke(code, content);
			}
			else
			{
				if (retryTime > 0)
				{
                    UnityEngine.LogManager.LogError ($"AppleJson request error，Url:{url }, begin {(3 - this.retryTime) + 2} time to try");

					SdkManager.ins.StartCoroutine(StartSendPost());
					retryTime--;
				}
				else
				{
					callback?.Invoke(code, content);
				}
			}
		}
	}
}
