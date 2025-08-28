using System;

namespace Hukiry.SDK
{
    public class iOSDevelopUtility : IInternalUtility
	{
		void IInternalUtility.Init(Func<CallBackJsonParamHandler> _this) { }
		void IInternalUtility.CallSDKFunction(int funType, string jsonParam) { }
		void IInternalUtility.InitSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam) { }
		string IInternalUtility.GetSDK(int funType) => string.Empty;
		void IInternalUtility.CallSDKParam(UnityParam jsonParam) { }
	}
}
