using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Hukiry.Http
{
    /// <summary>
    /// UnityWebRequest 本地get
    /// </summary>
    public class HttpLocalRequest {
		/// <summary>
		/// 回调函数
		/// </summary>
		private Action<bool, string, byte[]> callback;

		/// <summary>
		/// 请求路径
		/// </summary>
		private string url;

		/// <summary>
		/// 是否已经完成
		/// </summary>
		public bool isDone;

		//版本加载，lua加载
		public HttpLocalRequest(string address, Action<bool, string, byte[]> callback)
		{
			this.callback = callback;
			this.url = address;
			MainGame.Instance.StartCoroutine(StartSendGet());
		}


		IEnumerator StartSendGet()
		{
			UnityWebRequest uwr = UnityWebRequest.Get(this.url);
			uwr.timeout = 3;
			uwr.downloadHandler = new DownloadHandlerBuffer();
			yield return uwr.SendWebRequest();
			bool isSuccess = false;
			string content;
			byte[] buffer = null;
			if (uwr.isNetworkError || uwr.isHttpError)
			{
				content = uwr.error;
			}
			else
			{
				isSuccess = true;
				content = uwr.downloadHandler.text;
				buffer = uwr.downloadHandler.data;
			}
			uwr.Dispose();
			this.ActionResult(isSuccess, content, buffer);
		}

		private void ActionResult(bool isSuccess, string content,byte[] buffer)
		{
			if (isSuccess)
			{
                //UnityEngine.LogManager.Log($"UwrGet请求成功：url:{this.url}, 结果:{content}");
				callback?.Invoke(isSuccess, content, buffer);
				isDone = true;
			}
			else
			{
                UnityEngine.Debug.Log($"UwrGet请求失败：url:{this.url}, 结果:{content}");
				callback?.Invoke(isSuccess, content, buffer);
			}
		}
	}
}
