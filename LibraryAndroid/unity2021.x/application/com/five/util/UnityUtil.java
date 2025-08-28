package com.five.util;

import android.util.Log;

import com.five.mergegame.UnityInterfaceProxy;
import com.five.mergegame.UnityParam;

public class UnityUtil {
    public static UnityInterfaceProxy mUnityInterfaceProxy;

    /// Java to sdk
    public static void JavaToUnity(UnityParam jsonParam) {
        if (mUnityInterfaceProxy!=null)
        {
            mUnityInterfaceProxy.SDKToUnityParam(jsonParam);
        }
        else
        {
            Log.d("SDK","error:UnityInterfaceProxy");
        }
    }
}
