using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Hukiry.SDK
{

	/// <summary>
	/// 用于苹果登录撤销：WWWForm拼接字符串发送
	/// </summary>
	public class UwrPostAppleWWW
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

		/// <summary>
		/// POST参数
		/// </summary>
		private WWWForm formData;
		private const string headKey = "Content-Type";
		private const string headValue = "application/x-www-form-urlencoded";

		public UwrPostAppleWWW(string address, WWWForm formData = null, Action<bool, string> callback = null)
		{
			retryTime = 2;

			this.callback = callback;
			this.url = address;
			this.formData = formData;
			SdkManager.ins.StartCoroutine(StartSendPost());
		}

		IEnumerator StartSendPost()
		{
            UnityEngine.LogManager.Log($"UwrPostWww请求：url:{url}");
			UnityWebRequest uwr =UnityWebRequest.Post(this.url, this.formData);
            uwr.SetRequestHeader(headKey, headValue);
			uwr.timeout = 5;
			uwr.downloadHandler = new DownloadHandlerBuffer();

			yield return uwr.SendWebRequest();
			bool code = false;
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
                UnityEngine.LogManager.Log($"Apple WWW request succesful：url:{url}, result:{content}");
				callback?.Invoke(code, content);
			}
			else
			{
				if (retryTime > 0)
				{
                    UnityEngine.LogManager.LogError ($"AppleWWW request error，Url:{url }, begin {(3 - this.retryTime) + 2}  times to try");

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
