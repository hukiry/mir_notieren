using System;

namespace Hukiry.SDK
{
    public interface IInternalUtility
	{
		/// <summary>
		/// 初始化函数
		/// </summary>
		/// <param name="_this">支付回调</param>
		/// <param name="coroutine">登出验证</param>
		void Init(Func<CallBackJsonParamHandler> _this);
		/// <summary>
		/// 旧方法
		/// </summary>
		/// <param name="funType">函数类型<see cref="SdkFunctionType"/></param>
		/// <param name="jsonParam">json格式</param>
		void CallSDKFunction(int funType, string jsonParam);
		/// <summary>
		///  旧方法
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="UnityFunctionName"></param>
		/// <param name="CallFunctionName"></param>
		/// <param name="jsonParam"></param>
		void InitSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam);
		/// <summary>
		/// 返回对应参数
		/// </summary>
		/// <param name="funType">查看函数类型<see cref="SdkFunctionType"/></param>
		/// <returns></returns>
		string GetSDK(int funType);

		/// <summary>
		/// 调用sdk
		/// </summary>
		/// <param name="jsonParam">结构体参数</param>
		void CallSDKParam(UnityParam jsonParam);
	}
}
