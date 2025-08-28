using System;
using System.Runtime.InteropServices;

namespace Hukiry.SDK
{
    public abstract class iOSInternalBase : IInternalUtility
    {
        /// <summary>
        /// 通用方法，需静态
        /// </summary>
        protected static Func<CallBackJsonParamHandler> m_SdkFuncLua;
        /// <summary>
        /// 结构指针
        /// </summary>
        protected T IntPtrToStructure<T>(IntPtr intPtr) => (T)Marshal.PtrToStructure(intPtr, typeof(T));
        /// <summary>
        /// 字符串指针
        /// </summary>
        protected string IntPtrToString(IntPtr intPtr) => Marshal.PtrToStringAnsi(intPtr);

        public static AppleLoginVerify AppleLoginVerify { get; } = new AppleLoginVerify();

        public abstract void CallSDKFunction(int funType, string jsonParam);
        public abstract void CallSDKParam(UnityParam jsonParam);
        public abstract string GetSDK(int funType);
        public virtual void Init(Func<CallBackJsonParamHandler> _this) {
            m_SdkFuncLua = _this;
        }

        public abstract void InitSDK(string gameObjectName, string UnityFunctionName, string CallFunctionName, string jsonParam);
    }
}
