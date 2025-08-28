using System;
using UnityEngine;
namespace Hukiry.SDK
{
    public class WindowSDK : IGameSDK
    {
        void IGameSDK.Awake(GameObject gameObject, Func<CallBackJsonParamHandler> _this)
        {
            Debug.Log($"Awake:{gameObject},{_this}");
        }

        void IGameSDK.CallSDKFunction(int funType, string jsonParam)
        {
            Debug.Log($"CallSDKFunction:{funType},{jsonParam}");
        }

        void IGameSDK.StartSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam)
        {
            Debug.Log($"InitSDK:{gameObjectName},{UnityFunctionName}");
        }

        string IGameSDK.GetCallSDKFunction(int funType)
        {
            Debug.Log($"GetCallSDKFunction:{funType}");
            return string.Empty;
        }

        void IGameSDK.CallSDKFunction(UnityParam jsonParam)
        {
            Debug.Log($"{jsonParam.funType},CallSDKFunction:{jsonParam.jsonParams}");
        }
    }
}
