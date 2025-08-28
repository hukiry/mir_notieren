package com.yiliu.gamelibrary;

import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

import android.app.Activity;
import android.content.Context;

public class MainActivity {

    UnityInterfaceProxy mUnityInterfaceProxy;
    public Context mContext = null;
    public Activity mActivity = null;
    protected static final String TAG_UNITY = "SDK";
    //-----------------------------------Public Method begin ， be called of Unity----------------------------------------
    // 初始化SDK
    public void InitSDK(String UnityGameObject, String UnityMethodName, String CallSdkFunction, String JsonFunctionParamKey) {

    }

    //初始化Unity 上下文档
    public void InitActivity(Context context, Activity activity)
    {
        this.mContext = context;
        this.mActivity = activity;
        this.mActivity.startActivity(new Intent(this.mActivity,SignInActivity.class));
        Log.d(TAG_UNITY, this.mContext.toString());
        Log.d(TAG_UNITY, this.mActivity.toString());
    }

    // unity to sdk
    public void CallSDKFunction(int funType, String jsonParam) {
        Log.d(TAG_UNITY,"CallSDKFunction:"+jsonParam);
        mUnityInterfaceProxy.SDKToUnity(funType, jsonParam);
        Toast.makeText(this.mActivity, "开始登录", Toast.LENGTH_SHORT).show();
        SignInActivity.signInGoogle();
    }

    public void CallSDKFunction(UnityParam param) {
        Log.d(TAG_UNITY,"funType:"+param.funType+",UnityParam:"+param.jsonParams);
        mUnityInterfaceProxy.SDKToUnityParam(param);
        SignInActivity.signOut();
    }

    // unity to sdk
    public String GetCallSDKFunction(int funType) {
        return "";
    }

    public void SetUnityCallbackProxy(UnityInterfaceProxy _unityInterfaceProxy) {
        mUnityInterfaceProxy = _unityInterfaceProxy;
    }
}
