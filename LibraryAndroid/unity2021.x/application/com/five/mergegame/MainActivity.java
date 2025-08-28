package com.five.mergegame;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.tasks.Task;
import com.five.pay.GoogleBilling;
import com.five.util.UnityUtil;
import com.unity3d.player.UnityPlayerActivity;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class MainActivity extends UnityPlayerActivity {
    public Activity _mActivity;
    private static final int RC_SIGN_IN = 9001;
    private  SignInActivity mSignInActivity;
    private GoogleBilling mGoogleBilling;
    private static final String TAG = "SDK MainActivity";
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this._mActivity = this;
        this.mSignInActivity = new SignInActivity(this);

        if(this.mSignInActivity!=null) {
            GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(this);
            this.mSignInActivity.updateUI(account, true);
        }

        this.mGoogleBilling = new GoogleBilling(this);
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
    // 初始化SDK unity to sdk
    public void InitSDK(String gameObjectName) {
        Log.d("InitSDK", gameObjectName);
    }

    // unity to sdk
    public void CallSDKFunction(UnityParam jsonParam) {
        Log.d(TAG,jsonParam.funType+":   "+jsonParam.jsonParams);
        //登录部分
        if (jsonParam.funType == ESdkFunctionType.loginFetch) {//拉起登录
            this.signInGoogle();
        }
        else if (jsonParam.funType == ESdkFunctionType.payFetch) {//拉起支付部分
            try{
                JSONObject jsonObject = new JSONObject(jsonParam.jsonParams);
                String productId = jsonObject.getString("productId");//商品id
                payment(productId);
            }
            catch (JSONException e) {
                Log.d(TAG,e.toString());
            }
        }
        else if (jsonParam.funType == ESdkFunctionType.logoutFetch) {//拉起登出部分
            this.LogOut();
        }
        else if (jsonParam.funType == ESdkFunctionType.productList)
        {
            try{
                Log.d(TAG,jsonParam.jsonParams);
                JSONArray productIds = new JSONArray(jsonParam.jsonParams);
                ArrayList<String> arrayLst =new ArrayList<>();
                for (int i = 0; i < productIds.length(); i++) {
                    arrayLst.add(productIds.getString(i));
                }
                Log.d(TAG,String.valueOf(arrayLst.size()) );
                this.mGoogleBilling.searchProductDetails(arrayLst);
            }
            catch (JSONException e) {
                Log.d(TAG,e.toString());
            }
        }
    }
    // unity to sdk
    public String GetCallSDKFunction(int funType) {
        return "";
    }
    // unity to sdk
    public void SetUnityCallbackProxy(UnityInterfaceProxy _unityInterfaceProxy) {
        UnityUtil.mUnityInterfaceProxy = _unityInterfaceProxy;
    }

    //-----------------------------------
    private  void LogOut()
    {
        mSignInActivity.signOut();
    }

    //支付
    private void payment(String productId) {
        mGoogleBilling.pay(productId);
//        Intent intent = new Intent(Settings.ACTION_MANAGE_OVERLAY_PERMISSION);
//        intent.setClass(getApplicationContext(), GoogleBilling.class);
//        intent.putExtra("productId",productId);
//        startActivity(intent);

    }
//----------------------------------------------------------------------------------------------------------------
}
