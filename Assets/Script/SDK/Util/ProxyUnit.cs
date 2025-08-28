using Newtonsoft.Json;
using System;
namespace Hukiry.SDK
{
    public class ProxyUnit
    {
        private Func<CallBackJsonParamHandler> m_SdkFuncLua;
        private UnityParam param;
        public int frameCount;

        public ProxyUnit(Func<CallBackJsonParamHandler> sdkFuncLua, UnityParam param, int frameCount = 3)
        {
            m_SdkFuncLua = sdkFuncLua;
            this.param = param;
            this.frameCount = frameCount;
        }

        public void Runable()
        {
            m_SdkFuncLua()?.Invoke(JsonConvert.SerializeObject(param));
        }
    }
}