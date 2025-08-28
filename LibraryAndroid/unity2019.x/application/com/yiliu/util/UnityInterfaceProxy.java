package com.yiliu.util;
/**
 代理接口，来自于Unity的传入
 */
public interface UnityInterfaceProxy {
     void SDKToUnityParam(UnityParam javaObject);
     void SDKToUnity(int funType, String jsonParam);
}
