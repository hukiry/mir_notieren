package com.yiliu.gamelibrary;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.tasks.Task;
import com.unity3d.player.UnityPlayerActivity;
import com.yiliu.util.UnityInterfaceProxy;
import com.yiliu.util.UnityParam;
import com.yiliu.util.UnityUtil;

public class MainActivity extends UnityPlayerActivity {
    private static final int RC_SIGN_IN = 9001;
    private  SignInActivity mSignInActivity;
    protected static final String TAG_UNITY = "SDK";
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if(this.mSignInActivity!=null) {
            GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(this);
            this.mSignInActivity.updateUI(account, true);
        }
        else
        {
            this.mSignInActivity = new SignInActivity(this);
        }
    }

    @Override
    public void onStart() {
        super.onStart();
        if(this.mSignInActivity!=null) {
            GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(this);
            this.mSignInActivity.updateUI(account, true);
        }
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        //登录回调
        if (requestCode == RC_SIGN_IN) {
            Task<GoogleSignInAccount> task = GoogleSignIn.getSignedInAccountFromIntent(data);
            this.mSignInActivity.handleSignInResult(task);
        }
    }

    //google登录
    private void signInGoogle() {
        Intent signInIntent = this.mSignInActivity.mGoogleSignInClient.getSignInIntent();
        startActivityForResult(signInIntent, RC_SIGN_IN);
    }

    //-----------------------------------Public Method begin ， be called of Unity----------------------------------------
    // 初始化SDK
    public void InitSDK(String UnityGameObject, String UnityMethodName, String CallSdkFunction, String JsonFunctionParamKey) {
        UnityUtil.UnityGameObject=UnityGameObject;
        UnityUtil.UnityMethodName=UnityMethodName;
        UnityUtil.CallSdkFunction=CallSdkFunction;
        UnityUtil.JsonFunctionParamKey=JsonFunctionParamKey;
    }

    // unity to sdk
    public void CallSDKFunction(int funType, String jsonParam) {
        Log.d(TAG_UNITY,"CallSDKFunction:"+jsonParam);
        Toast.makeText(this, "开始登录", Toast.LENGTH_SHORT).show();
        signInGoogle();
    }

    public void CallSDKFunction(UnityParam param) {
        Log.d(TAG_UNITY,"funType:"+param.funType+",UnityParam:"+param.jsonParams);
        Toast.makeText(this, "signOut", Toast.LENGTH_SHORT).show();
        this.mSignInActivity.signOut();
    }

    // unity to sdk
    public String GetCallSDKFunction(int funType) {
        return "";
    }

    public void SetUnityCallbackProxy(UnityInterfaceProxy _unityInterfaceProxy) {
        UnityUtil.mUnityInterfaceProxy = _unityInterfaceProxy;
    }
}
