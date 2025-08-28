using LitJson;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace Hukiry.SDK
{
#if UNITY_IOS || UNITY_IPHONE
    public class iOSInternalUtility : iOSInternalBase
	{
    #region __Internal
    #region Util
        [DllImport("__Internal")]
        public static extern string _GetIPv6(string mHost, string mPort);//获取ip6地址
        [DllImport("__Internal")]
        public static extern string _getAppVersionName();//获取app版本名
        [DllImport("__Internal")]
        public static extern string _getLocationLanguage();//获取手机系统语言
        [DllImport("__Internal")]
        public static extern string _getDeviceModel();//获取手机型号
        [DllImport("__Internal")]
        public static extern int _isAvailable13();//是否是13.0以上版本 
    #endregion


        [DllImport("__Internal")]
		public static extern void _CallSDKFunction(int funType, string json);//调用OC:把json字符串传给IOS 
		[DllImport("__Internal")]//初始化SDK
		public static extern void _InitSDK(string gameObjectName, string UnityFunctionName, string sdkFunctionName, string sdkJsonParamKey);
		[DllImport("__Internal")]
		public static extern string _GetSDK(int funType);
		[DllImport("__Internal")]
		public static extern void _RegeditCallFunction(CallBackUnityParamHandler iOSCallBack);//把C#中的回调函数传给IOS
		[DllImport("__Internal")]
		public static extern void _CallSDKParam(UnityParam param);//调用OC:把C#中的结构体传给IOS 
    #endregion


		[AOT.MonoPInvokeCallback(typeof(CallBackUnityParamHandler))]//此特性用于 C#签名函数
		private static void SDKToUnityParam(UnityParam param)
		{
			SdkManager.ins.AddProxyUnit(new ProxyUnit(m_SdkFuncLua, param));

            UnityEngine.Debug.Log($"unity sdk back call：functionKey = {param.funType}, jsonParam = {JsonConvert.SerializeObject(param)}");

		}

		private UnityIAPPurchasing m_unityIAPPurchasing;
		
		public override void Init(Func<CallBackJsonParamHandler> _this)
		{
			base.Init(_this);
			m_unityIAPPurchasing = new UnityIAPPurchasing();
			m_unityIAPPurchasing.InitPurchasingListener(this.PurchasingListener, true);
			_RegeditCallFunction(SDKToUnityParam);
		}
		public override void CallSDKFunction(int funType, string jsonParam) => this.StartCallOC(funType, jsonParam);
		public override void InitSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam) =>
			_InitSDK(gameObjectName, UnityFunctionName, CallFunctionName, jsonParam);
		public override string GetSDK(int funType) => _GetSDK(funType);
		public override void CallSDKParam(UnityParam jsonParam) => this.StartCallOC(jsonParam);
		private void PurchasingListener(EPurchaingState purchaingState, string jsonParam)
		{
			UnityParam param = new UnityParam();
			switch (purchaingState)
			{
				case EPurchaingState.NetworkUnavailable:
					param.funType = SdkFunctionType.NetworkUnavailable;
					param.errorMsg = jsonParam;
					break;
				case EPurchaingState.PaySuccessful:
					param.funType = SdkFunctionType.PaySucces;
					param.jsonParams = jsonParam;
					break;
				case EPurchaingState.PayFail:
					param.funType = SdkFunctionType.PayFail;
					param.errorMsg = jsonParam;
					break;
				case EPurchaingState.PayCancel:
					param.funType = SdkFunctionType.PayCancel;
					param.errorMsg = jsonParam;
					break;
				case EPurchaingState.InitProductSuccessful:
					param.funType = SdkFunctionType.InitProductSucces;
					param.jsonParams = jsonParam;
					break;
				case EPurchaingState.InitProductFail:
					param.funType = SdkFunctionType.InitProductFail;
					param.errorMsg = jsonParam;
					break;
				case EPurchaingState.VerifySuccessful:
					param.funType = SdkFunctionType.PayVerifyRecipeSucces;
					param.jsonParams = jsonParam;
					break;
				case EPurchaingState.VerifyFail:
					param.funType = SdkFunctionType.PayVerifyRecipeFail;
					param.errorMsg = jsonParam;
					break;
				default:
					break;
			}
			SdkManager.ins.AddProxyUnit(new ProxyUnit(m_SdkFuncLua, param, 1));
		}

		private void StartCallOC(UnityParam unityParam)
		{
			if (unityParam.funType == SdkFunctionType.PayFetch || unityParam.funType == SdkFunctionType.InitProductFetch)
			{
				this.FetchPayOrProduct(unityParam.funType, unityParam.jsonParams);
			}
			else
			{
				_CallSDKParam(unityParam);
			}
		}

		private void StartCallOC(int funType, string jsonParam)
		{
			if (funType == SdkFunctionType.PayFetch || funType == SdkFunctionType.InitProductFetch)
			{
				this.FetchPayOrProduct(funType, jsonParam);
			}
			else
			{
				_CallSDKFunction(funType,jsonParam);
			}
		}

		//拉起支付相关
		private void FetchPayOrProduct(int funType, string jsonParam)
		{
			JsonData jsonData = JsonMapper.ToObject(jsonParam);
			string stringKey1 = (string)jsonData["stringKey1"];
			if (funType == SdkFunctionType.PayFetch)
			{
				m_unityIAPPurchasing.Pay(stringKey1);
			}
			else if (funType == SdkFunctionType.InitProductFetch)
			{
				m_unityIAPPurchasing.SearchProductList(stringKey1.Split(','));
			}
		}
	}
#endif
}
