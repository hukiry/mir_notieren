package com.yiliu.util;

import android.util.Log;

/**
 全局工具
*/
public class UnityUtil {
    public static UnityInterfaceProxy mUnityInterfaceProxy;

    public static String UnityGameObject;
    public static String UnityMethodName;
    public static String CallSdkFunction;
    public static String JsonFunctionParamKey;
    /**
     Java to Unity Client
     @param jsonParam Unity参数结构
     */
    public static void JavaToUnity(UnityParam jsonParam) {
        if (mUnityInterfaceProxy != null) {
            mUnityInterfaceProxy.SDKToUnityParam(jsonParam);
        } else {
            Log.d("SDK", "error:UnityInterfaceProxy");
        }
    }

    public  static  String  ToString()
    {
        return "UnityGameObject:"+UnityGameObject+
                ",UnityMethodName:"+UnityMethodName+
                ",CallSdkFunction:"+CallSdkFunction+
                ",JsonFunctionParamKey:"+JsonFunctionParamKey;
    }
}
