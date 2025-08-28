package com.yiliu.gamelibrary;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.FragmentActivity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.api.ApiException;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;

import java.util.Timer;
import java.util.TimerTask;

///https://blog.csdn.net/houn27/article/details/106260322/
public class SignInActivity extends FragmentActivity {

    private static final String TAG = "SignInActivity";
    private static final int RC_SIGN_IN = 9001;
    public GoogleSignInClient mGoogleSignInClient;
    static SignInActivity mActivity;
    
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        mActivity = this;
        InitActivity();
    }

      @Override
    public void onStart() {
        super.onStart();
        GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(this);
        this.updateUI(account, true);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        //登录回调
        if (requestCode == RC_SIGN_IN) {
            Task<GoogleSignInAccount> task = GoogleSignIn.getSignedInAccountFromIntent(data);
            this.handleSignInResult(task);
        }
        else
        {
            Toast.makeText(this, "登录失败", Toast.LENGTH_SHORT).show();
        }

        Timer timer = new Timer();
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                // 需要延时执行的操作
                SignInActivity.this.finish();
            }
        };
        timer.schedule(task, 2000);
//        this.finish();
    }

    //初始化
    private void InitActivity() {
        GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(mActivity);
        mActivity.updateUI(account, true);
        if (mGoogleSignInClient == null) {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                    .requestEmail().requestIdToken("145268610351-lqd554104jc4p9i9lm1bvrp1vm3m7n8q.apps.googleusercontent.com")
                    .build();

            mGoogleSignInClient = GoogleSignIn.getClient(mActivity, gso);
        }

        this.finish();

    }

    //google登录
    public static void signInGoogle() {
        mActivity.startActivityForResult(mActivity.mGoogleSignInClient.getSignInIntent(), RC_SIGN_IN);
    }
    //登出-撤回数据1
    public static void signOut() {

        mActivity.mGoogleSignInClient.signOut().addOnCompleteListener(mActivity, new OnCompleteListener<Void>() {
            @Override
            public void onComplete(@NonNull Task<Void> task) {
                mActivity.updateUI(null,false);
                mActivity.finish();
            }
        });
    }

    private void handleSignInResult(Task<GoogleSignInAccount> completedTask) {
        try {
            GoogleSignInAccount account = completedTask.getResult(ApiException.class);
            // Signed in successfully, show authenticated UI.
            updateUI(account, false);
        } catch (ApiException e) {
            // The ApiException status code indicates the detailed failure reason.
            // Please refer to the GoogleSignInStatusCodes class reference for more information.
            Log.w(TAG, "signInResult:failed code=" + e.getStatusCode());
//            UnityParam param = new UnityParam();
//            param.funType = ESdkFunctionType.loginFail;
//            param.errorCode = e.getStatusCode();
//            param.errorMsg = e.toString();
//            UnityUtil.JavaToUnity(param);
            Toast.makeText(this, e.getMessage(), Toast.LENGTH_SHORT).show();
        }
    }



    public void revokeAccess() {
        //登出-撤回数据2
        mGoogleSignInClient.revokeAccess().addOnCompleteListener(this, new OnCompleteListener<Void>() {
            @Override
            public void onComplete(@NonNull Task<Void> task) {
                // [START_EXCLUDE]
                updateUI(null,false);
                // [END_EXCLUDE]
            }
        });
    }
    // [END revokeAccess]

    private void updateUI(@Nullable GoogleSignInAccount account, boolean lastRecorde) {
        if (account != null) {
            String id = account.getId();
            String idToken = account.getIdToken();
            UnityParam param = new UnityParam();
            Toast.makeText(this, "登录成功", Toast.LENGTH_SHORT).show();
//            param.funType = ESdkFunctionType.loginSuccessful;
            Log.i(TAG,"idToken:" + idToken+"  ,  id:" + id);
//            try {
//                JSONObject jsonObject = new JSONObject();
//                jsonObject.put("user_id", id);
//                jsonObject.put("user_token", idToken);
//                jsonObject.put("isRecord", lastRecorde);
//                param.jsonParams = jsonObject.toString();
//            } catch (JSONException e) {
//                Log.w(TAG, "signInResult:failed code=" + e.toString());
//            }
//            UnityUtil.JavaToUnity(param);
        } else {
//            UnityParam param = new UnityParam();
//            param.funType = ESdkFunctionType.logoutSuccessful;
//            UnityUtil.JavaToUnity(param);
            Toast.makeText(this, "登录失败1", Toast.LENGTH_SHORT).show();
        }
    }

}
