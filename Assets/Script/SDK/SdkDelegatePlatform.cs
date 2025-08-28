
namespace Hukiry.SDK
{
    /// <summary>
    /// 委托声明 Json解析传参
    /// </summary>
    /// <param name="functionKey">函数<see cref="SdkFunctionType"/></param>
    /// <param name="json">必须json格式</param>
    public delegate void CallBackJsonParamHandler(string json);

    /// <summary>
    /// 委托声明:iOS必须是静态注册的方法,  Android 参数<see cref="UnityParam"/>传入必须使用 java类对象
    /// </summary>
    /// <param name="param">结构体参数</param>
    //此特性表示：函数参数为指针
    //[System.Runtime.InteropServices.UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
    public delegate void CallBackUnityParamHandler(UnityParam param);

    /// <summary>
    /// 支付回调
    /// </summary>
    /// <param name="purchaingState"></param>
    /// <param name="jsonParam"></param>
    public delegate void CallBackPurchaingHandler(EPurchaingState purchaingState, string jsonParam);
}
