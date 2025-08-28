using System;
using UnityEngine;

namespace Hukiry.SDK
{
    public class iPhoneSDK : IGameSDK
    {
        IInternalUtility internalUtility { get; } =
#if UNITY_IOS || UNITY_IPHONE
            new iOSInternalUtility();
#else
            new iOSDevelopUtility();
#endif

        void IGameSDK.Awake(GameObject gameObject, Func<CallBackJsonParamHandler> _this)
        {

            this.internalUtility.Init( _this);
        }
        void IGameSDK.CallSDKFunction(int funType, string jsonParam)=> this.internalUtility.CallSDKFunction(funType, jsonParam);

        void IGameSDK.StartSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam)=>
            this.internalUtility.InitSDK(gameObjectName, UnityFunctionName, CallFunctionName, jsonParam);

        string IGameSDK.GetCallSDKFunction(int funType)=> this.internalUtility.GetSDK(funType);

        void IGameSDK.CallSDKFunction(UnityParam jsonParam)=> this.internalUtility.CallSDKParam(jsonParam);
    }
}
