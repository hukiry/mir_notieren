package com.five.mergegame;

import android.app.Activity;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import android.util.Log;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.api.ApiException;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.five.util.UnityUtil;

import org.json.JSONException;
import org.json.JSONObject;

///https://blog.csdn.net/houn27/article/details/106260322/
public class SignInActivity{

    private static final String TAG = "SignInActivity";

    public GoogleSignInClient mGoogleSignInClient;

    private Activity _Activity;
    public SignInActivity(Activity _Activity)
    {
        this._Activity = _Activity;
        GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail().requestIdToken("139022981594-h2kt36krhpof7pbp3i06hqao2m9276os.apps.googleusercontent.com")
                .build();
        mGoogleSignInClient = GoogleSignIn.getClient(_Activity, gso);
    }

    public void handleSignInResult(Task<GoogleSignInAccount> completedTask) {
        try {
            GoogleSignInAccount account = completedTask.getResult(ApiException.class);
            // Signed in successfully, show authenticated UI.
            updateUI(account, false);
        } catch (ApiException e) {
            // The ApiException status code indicates the detailed failure reason.
            // Please refer to the GoogleSignInStatusCodes class reference for more information.
            Log.w(TAG, "signInResult:failed code=" + e.getStatusCode());
            UnityParam param = new UnityParam();
            param.funType = ESdkFunctionType.loginFail;
            param.errorCode = e.getStatusCode();
            param.errorMsg = e.toString();
            UnityUtil.JavaToUnity(param);
        }
    }

    public void signOut() {
        //登出-撤回数据1
        mGoogleSignInClient.signOut().addOnCompleteListener(this._Activity, new OnCompleteListener<Void>() {
            @Override
            public void onComplete(@NonNull Task<Void> task) {
                updateUI(null,false);
            }
        });
    }

    public void revokeAccess() {
        //登出-撤回数据2
        mGoogleSignInClient.revokeAccess().addOnCompleteListener(this._Activity, new OnCompleteListener<Void>() {
            @Override
            public void onComplete(@NonNull Task<Void> task) {
                // [START_EXCLUDE]
                updateUI(null,false);
                // [END_EXCLUDE]
            }
        });
    }
    // [END revokeAccess]

    public void updateUI(@Nullable GoogleSignInAccount account,boolean lastRecorde) {
        if (account != null) {
            String id = account.getId();
            String idToken = account.getIdToken();
            UnityParam param = new UnityParam();
            param.funType = ESdkFunctionType.loginSuccessful;
            Log.i(TAG,"idToken:" + idToken+"  ,  id:" + id);
            try {
                JSONObject jsonObject = new JSONObject();
                jsonObject.put("user_id", id);
                jsonObject.put("user_token", idToken);
                jsonObject.put("isRecord", lastRecorde);
                param.jsonParams = jsonObject.toString();
            } catch (JSONException e) {
                Log.w(TAG, "signInResult:failed code=" + e.toString());
            }
            UnityUtil.JavaToUnity(param);
        } else {
            UnityParam param = new UnityParam();
            param.funType = ESdkFunctionType.logoutSuccessful;
            UnityUtil.JavaToUnity(param);
        }
    }

}
