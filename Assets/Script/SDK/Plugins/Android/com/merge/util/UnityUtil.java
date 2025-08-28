package com.merge.util;

import android.util.Log;

import com.merge.paradise.UnityInterfaceProxy;
import com.merge.paradise.UnityParam;

public class UnityUtil {
    public static UnityInterfaceProxy mUnityInterfaceProxy;

    // Jave to sdk
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
