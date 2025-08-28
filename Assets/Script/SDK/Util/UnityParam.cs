using System.Runtime.InteropServices;
using UnityEngine;

namespace Hukiry.SDK
{
    [StructLayout(LayoutKind.Sequential)]
	public struct UnityParam
	{
		/// <summary>
		/// 函数类型
		/// </summary>
		public int funType;
		/// <summary>
		/// 列表=json格式，否则直接传参
		/// </summary>
		public string jsonParams;
		public int errorCode;
		public string errorMsg;

		//#if UNITY_ANDROID
		internal static UnityParam GetUnityParam(AndroidJavaObject javaObject)
		{
			UnityParam param = new UnityParam();
			param.funType = javaObject.Get<int>(nameof(param.funType));
			param.jsonParams = javaObject.Get<string>(nameof(param.jsonParams));
			param.errorCode = javaObject.Get<int>(nameof(param.errorCode));
			param.errorMsg = javaObject.Get<string>(nameof(param.errorMsg));
			return param;
		}

		internal AndroidJavaObject GetAndroidJavaObject(string className)
		{
			var obj = new AndroidJavaObject(className);
			obj.Set(nameof(this.funType), this.funType);
			obj.Set(nameof(this.jsonParams), this.jsonParams);
			obj.Set(nameof(this.errorCode), this.errorCode);
			obj.Set(nameof(this.errorMsg), this.errorMsg);
			return obj;
		}
		//#endif
	}
}
