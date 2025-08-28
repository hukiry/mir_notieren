using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Hukiry.SDK
{
    //D:\Program Files\Unity\Editor\Data\PlaybackEngines\AndroidPlayer\Variations\mono\Release\Classes
    public class AndroidProxyUtility: AndroidJavaProxy
    {
        private  Func<CallBackJsonParamHandler> m_SdkFuncLua;
        /// <summary>
        /// 创建代理:UnityCallBackProxy是java接口类名
        /// </summary>
        /// <param name="interfaceName">此接口名，必须是java接口类名路径，例如com.example.library.UnityCallbackProxy</param>
        public AndroidProxyUtility(Func<CallBackJsonParamHandler> _this, string interfaceName) :base(interfaceName)
        {
            this.m_SdkFuncLua = _this;
        }

        private void SDKToUnityParam(AndroidJavaObject javaObject)
        {
            UnityParam param = UnityParam.GetUnityParam(javaObject);
            UnityEngine.Debug.Log($"UnityParam ==unity sdk back call：msgJson = {JsonConvert.SerializeObject(param)}");
            SdkManager.ins.AddProxyUnit(new ProxyUnit(this.m_SdkFuncLua, param)); 
        }

        private void SDKToUnity(int funType, string jsonParam)
        {
            UnityEngine.Debug.Log($"SDKToUnity ==unity sdk back call：functionKey = {funType}, jsonParam = {jsonParam}");
        }

        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            if (methodName == nameof(SDKToUnityParam))
            {
                this.SDKToUnityParam(javaArgs[0]);
            }
            return null;
        }

        public override AndroidJavaObject Invoke(string methodName, object[] args)
        {
            if (methodName == nameof(SDKToUnity))
            {
                int funType = args.Length >= 1 ? (int)args[0] : 0;
                string jsonParam = args.Length >= 2 ? (string)args[1] : null;
                this.SDKToUnity(funType, jsonParam);
            }
            return null;
        }
    }
}
