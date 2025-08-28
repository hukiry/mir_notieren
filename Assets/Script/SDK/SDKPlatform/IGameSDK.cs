using System;
using UnityEngine;

namespace Hukiry.SDK
{
    /// <summary>
    /// 登录，支付，广告，通知
    /// </summary>
    public interface IGameSDK
    {
        /// <summary>
        /// 初始化委托方法
        /// </summary>
        /// <param name="_this">函数支付验证回调</param>
        /// <param name="coroutineMonoBehaviour">函数回调登出</param>
        void Awake(GameObject gameObject, Func<CallBackJsonParamHandler> _this);
        /// <summary>
        /// 必须先调用<see cref="IGameSDK.Awake(Func{CallBackJsonParamHandler})">IGameSDK.Awake</see>，
        /// 后调用<see cref="StartSDK(string, string, string, string)">IGameSDK.Awake</see>启动Sdk，设置 Unity 对象名称
        /// </summary>
        /// <param name="gameObjectName">游戏对象名</param>
        /// <param name="UnityFunctionName">对象里附加的函数名</param>
        /// <param name="CallFunctionName">sdk注册的函数key</param>
        /// <param name="jsonParam">函数参数key</param>
        void StartSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam);

        /// <summary>
        /// 调用SDK
        /// </summary>
        /// <param name="jsonParam">结构体参数</param>
        void CallSDKFunction(UnityParam jsonParam);
        /// <summary>
        /// 调用SDK
        /// </summary>
        /// <param name="funType">方法类型</param>
        /// <param name="jsonParam">json参数</param>
        void CallSDKFunction(int funType, string jsonParam);
        /// <summary>
        /// 获取SDK的字符串类型
        /// </summary>
        /// <typeparam name="T"><see cref="AndroidSDK"/></typeparam>
        /// <param name="funType"></param>
        /// <returns>返回Json字符串</returns>
        string GetCallSDKFunction(int funType);

    }
}

