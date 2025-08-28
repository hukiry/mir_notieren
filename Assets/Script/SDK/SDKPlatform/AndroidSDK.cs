using System;
using UnityEngine;
namespace Hukiry.SDK
{
    public class AndroidSDK : IGameSDK
    {
        private AndroidJavaObject _plugin = null;
        private AndroidProxyUtility proxyUtility;
        private string javaInterface => SdkConfiguration.AndroidInterfaceClass;
        private string javaUnityParam => SdkConfiguration.AndroidUnityParamClass;
        private string javaUnityPlayer => SdkConfiguration.AndroidUnityPlayerClass;
        private string javaUnityCurrentActivity => SdkConfiguration.AndroidCurrentActivity;
        void IGameSDK.Awake(GameObject gameObject, Func<CallBackJsonParamHandler> _this)
        {
            this.proxyUtility = new AndroidProxyUtility(_this, javaInterface);
        }

        void IGameSDK.StartSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam)
        {
            if (_plugin == null)
            {
                using (AndroidJavaClass UnityPlayer = new AndroidJavaClass(javaUnityPlayer))
                {
                    _plugin = UnityPlayer.GetStatic<AndroidJavaObject>(javaUnityCurrentActivity);
                }
            }
            _plugin?.Call("InitSDK", gameObjectName, UnityFunctionName, CallFunctionName, jsonParam);
            _plugin?.Call("SetUnityCallbackProxy", this.proxyUtility);
        }

        void IGameSDK.CallSDKFunction(int funType, string jsonParam)
        {
            _plugin.Call("CallSDKFunction", funType, jsonParam);
        }

        string IGameSDK.GetCallSDKFunction(int funType)
        {
            return _plugin.Call<string>("GetCallSDKFunction", funType);
        }

        void IGameSDK.CallSDKFunction(UnityParam jsonParam)
        {
            AndroidJavaObject javaObject =jsonParam.GetAndroidJavaObject(javaUnityParam);
            _plugin.Call("CallSDKFunction", javaObject);
        }
    }
}
