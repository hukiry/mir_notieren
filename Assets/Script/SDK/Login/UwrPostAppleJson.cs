using System;
using System.Collections;
using UnityEngine.Networking;

namespace Hukiry.SDK
{

    /// <summary>
    /// 用于上传凭证：json格式拼接发送
    /// </summary>
    public class UwrPostAppleJson
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
		private string postData;

		public UwrPostAppleJson(string address, string postData = null, Action<bool, string> callback = null)
		{
            retryTime = 3;

			this.callback = callback;
			this.url = address;
			this.postData = postData;

			SdkManager.ins.StartCoroutine(StartSendPost());
		}

		IEnumerator StartSendPost()
		{
            UnityEngine.LogManager.Log($"AppleJson request：url:{url+this.postData}");
			UnityWebRequest uwr = new UnityWebRequest(this.url, UnityWebRequest.kHttpVerbPOST);
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            uwr.uploadHandler = new UploadHandlerRaw(postBytes);
            uwr.SetRequestHeader("Content-Type", "application/json");

			uwr.timeout =3;
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
                UnityEngine.LogManager.Log($"AppleJson request secessful：url:{url + this.postData}, result:{content}");
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
