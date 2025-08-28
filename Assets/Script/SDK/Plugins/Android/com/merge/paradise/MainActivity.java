package com.merge.paradise;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.tasks.Task;
import com.merge.pay.GoogleBilling;
import com.merge.util.UnityUtil;
import com.unity3d.player.UnityPlayerActivity;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends UnityPlayerActivity {
    public Activity _mActivity;
    private static final int RC_SIGN_IN = 9001;
    private  SignInActivity mSignInActivity;
    private GoogleBilling mGoogleBilling;

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
        Log.d("-----InitSDK",jsonParam.funType+":   "+jsonParam.jsonParams);
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
                Log.d("SDK",e.toString());
            }
        }
        else if (jsonParam.funType == ESdkFunctionType.logoutFetch) {//拉起登出部分
            this.LogOut();
        }
        else if (jsonParam.funType == ESdkFunctionType.productList)
        {
            try{
                Log.d("SDK productList",jsonParam.jsonParams);
                JSONArray productIds = new JSONArray(jsonParam.jsonParams);
                List<String> arrayLst =new ArrayList<>();
                for (int i = 0; i < productIds.length(); i++) {
                    arrayLst.add(productIds.getString(i));
                }
                Log.d("SDK productList",String.valueOf(arrayLst.size()) );
                this.mGoogleBilling.searchProductDetails(arrayLst);
            }
            catch (JSONException e) {
                Log.d("SDK productList",e.toString());
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
    }
//----------------------------------------------------------------------------------------------------------------
}
