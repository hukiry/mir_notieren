package com.merge.pay;

import android.app.Activity;
import android.content.Intent;
import android.icu.text.AlphabeticIndex;
import android.os.Bundle;
import android.util.Log;

import androidx.fragment.app.FragmentActivity;

import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingClientStateListener;
import com.android.billingclient.api.BillingFlowParams;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.ConsumeParams;
import com.android.billingclient.api.ConsumeResponseListener;
import com.android.billingclient.api.ProductDetails;
import com.android.billingclient.api.ProductDetailsResponseListener;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.android.billingclient.api.QueryProductDetailsParams;
import com.android.billingclient.api.SkuDetailsParams;
import com.merge.paradise.ESdkFunctionType;
import com.merge.paradise.UnityParam;
import com.merge.util.UnityUtil;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Optional;

public class GoogleBilling  {
    private BillingClient billingClient;
    private PurchasesUpdatedListener purchasesUpdatedListener;
    private Activity mActivity;
    private  boolean isReadyPay;
    private static final String TAG = "SDK pay";

    private Map<String,ProductDetails> dicProduct = new HashMap<>() ;
    private String curPayProductId;

    //初始化
    public GoogleBilling(Activity _Activity)
    {
        this.mActivity = _Activity;
        this.purchasesUpdatedListener = new PurchasesUpdatedListener() {
            @Override
            public void onPurchasesUpdated(BillingResult billingResult, List<Purchase> purchases) {
                // To be implemented in a later section.
                if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK
                        && purchases != null) {
                    for (Purchase purchase : purchases) {
                        handlePurchase(purchase);
                    }
                } else if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.USER_CANCELED) {
                    // Handle an error caused by a user cancelling the purchase flow.
                    //购买取消
                    UnityParam param = new UnityParam();
                    param.funType = ESdkFunctionType.payCancel;
                    param.errorCode = billingResult.getResponseCode();
                    param.errorMsg =billingResult.getDebugMessage();
                    UnityUtil.JavaToUnity(param);
                } else {
                    // Handle any other error codes.
                    //购买错误
                    UnityParam param = new UnityParam();
                    param.funType = ESdkFunctionType.payFail;
                    param.errorCode = billingResult.getResponseCode();
                    param.errorMsg =billingResult.getDebugMessage();
                    UnityUtil.JavaToUnity(param);
                }

                if(!GoogleBilling.this.isReadyPay ) {
                    GoogleBilling.this.startConnection();
                }
            }
        };

        //开始监听
        this.billingClient = BillingClient.newBuilder(this.mActivity)
                .setListener(purchasesUpdatedListener)
                .enablePendingPurchases()
                .build();

        this.startConnection();
    }

      //查询商品清单
    public void searchProductDetails(List<String> array)
    {
        List<QueryProductDetailsParams.Product> products = new ArrayList<>();
        for (String product_id:array)
        {
            products.add(QueryProductDetailsParams.Product.newBuilder()
                    .setProductId(product_id)
                    .setProductType(BillingClient.ProductType.INAPP)
                    .build());
        }

        QueryProductDetailsParams queryProductDetailsParams =QueryProductDetailsParams.newBuilder()
                .setProductList(products).build();

        billingClient.queryProductDetailsAsync( queryProductDetailsParams,new ProductDetailsResponseListener() {
                public void onProductDetailsResponse(BillingResult billingResult,List<ProductDetails> productDetailsList) {
                    // check billingResult process returned productDetailsList
                    if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                        JSONArray exampleArray = new JSONArray();
                        ProductDetails pResult = null;
                        for (ProductDetails p : productDetailsList) {
                            if (!dicProduct.containsKey(p.getProductId()))
                                dicProduct.put(p.getProductId(), p);

                            if (Objects.equals(GoogleBilling.this.curPayProductId, p.getProductId()))
                            {
                                pResult = p;
                                GoogleBilling.this.curPayProductId = "";
                            }

                             try {
                                JSONObject jsonObject = new JSONObject();
                                jsonObject.put("product_id", p.getProductId());
                                jsonObject.put("name", p.getName());
                                jsonObject.put("type", p.getProductType());
                                jsonObject.put("price", p.getOneTimePurchaseOfferDetails().getFormattedPrice());
                                jsonObject.put("currencyCode", p.getOneTimePurchaseOfferDetails().getPriceCurrencyCode());
                                jsonObject.put("amount", p.getOneTimePurchaseOfferDetails().getPriceAmountMicros() + "");
                                exampleArray.put(jsonObject);
                            } catch (JSONException e) {
                                Log.w(TAG, "JSONObject failed code=" + e.toString());
                            }
                          
                        }

                        Log.d(TAG,  exampleArray.toString());
                        UnityParam param = new UnityParam();
                        param.funType = ESdkFunctionType.productListSuccessful;
                        param.jsonParams = exampleArray.toString();
                        UnityUtil.JavaToUnity(param);

                        Log.d(TAG, "初始化商品列表================================dicProduct="+dicProduct.size());
                        if (pResult!=null)
                        {
                            Log.d(TAG, "开始支付=================================");
                            GoogleBilling.this.startPay(pResult);
                        }
                    }
                    else
                    {
                        Log.d(TAG, "查询google商品失败=================================");
                        //查询google商品失败，请检查后台配置
                        UnityParam param = new UnityParam();
                        param.funType = ESdkFunctionType.productListFail;
                        param.errorMsg = billingResult.getDebugMessage();
                        param.errorCode = billingResult.getResponseCode();
                        UnityUtil.JavaToUnity(param);
                    }
                }
            }
        );
    }
    //开始购买
    public void pay(String productId)
    {
        if(!this.isReadyPay) {
            this.startConnection();
        }
        else {
            ProductDetails product = dicProduct.get(productId);
            if (product != null) {
                this.startPay(product);
            } else {
                Log.d(TAG, "查找商品id================================="+productId);
                this.curPayProductId = productId;
                //if none, then search product "productId"
                ArrayList<String> temp = new ArrayList<>();
                temp.add(productId);
                searchProductDetails(temp);
            }
        }
    }

    void startConnection(){
        this.billingClient.startConnection(new BillingClientStateListener() {
            @Override
            public void onBillingSetupFinished(BillingResult billingResult) {
                if (billingResult.getResponseCode() ==  BillingClient.BillingResponseCode.OK) {
                    GoogleBilling.this.isReadyPay = true;
                    Log.d(TAG, "The BillingClient is ready. You can query purchases here.");
                }
                else
                {
                    GoogleBilling.this.isReadyPay = false;
                    UnityParam param = new UnityParam();
                    param.funType = ESdkFunctionType.payAvailable;
                    param.errorMsg = billingResult.getDebugMessage();
                    param.errorCode = billingResult.getResponseCode();
                    UnityUtil.JavaToUnity(param);
                }
            }
            @Override
            public void onBillingServiceDisconnected() {
                GoogleBilling.this.isReadyPay = false;
                Log.d(TAG, "Try to restart the connection on the next request to Google Play by calling the startConnection() method.");
            }
        });
    }

    //验证订单
    void handlePurchase(Purchase purchase) {
        // Verify the purchase.
        // Ensure entitlement was not already granted for this purchaseToken.
        // Grant entitlement to the user.
        ConsumeParams consumeParams =ConsumeParams.newBuilder().setPurchaseToken(purchase.getPurchaseToken()).build();
        ConsumeResponseListener listener = new ConsumeResponseListener() {
            @Override
            public void onConsumeResponse(BillingResult billingResult, String purchaseToken) {
                UnityParam param = new UnityParam();
                if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                    // Handle the success of the consume operation.
                    //购买成功
                    param.funType = ESdkFunctionType.paySuccessful;
                    try {
                        JSONObject jsonObject = new JSONObject();
                        jsonObject.put("purchaseToken", purchaseToken);
                        param.jsonParams = jsonObject.toString();
                    } catch (JSONException e) {
                        Log.w(TAG, "signInResult:failed code=" + e.toString());
                    }
                }
                else
                {
                    param.funType = ESdkFunctionType.payFail;
                    param.errorCode = billingResult.getResponseCode();
                    param.errorMsg = billingResult.getDebugMessage();
                }
                UnityUtil.JavaToUnity(param);
            }
        };

        billingClient.consumeAsync(consumeParams, listener);
    }

    private void  startPay(ProductDetails product)
    {
        Log.d(TAG, "拉起支付================================="+product.getProductId());
        try {
            BillingFlowParams.ProductDetailsParams params = BillingFlowParams.ProductDetailsParams.newBuilder().setProductDetails(product).build();

            List<BillingFlowParams.ProductDetailsParams> productDetailsParamsList = new ArrayList<>();
            productDetailsParamsList.add(params);

            BillingFlowParams billingFlowParams = BillingFlowParams.newBuilder()
                    .setProductDetailsParamsList(productDetailsParamsList)
                    .build();
            BillingResult billingResult = billingClient.launchBillingFlow(mActivity, billingFlowParams);
            if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                Log.w(TAG, "pay successful");
            } else {
                Log.w(TAG, "pay failed :" + billingResult.getDebugMessage());
            }
        }catch (Exception exp){
            Log.w(TAG, "异常："+exp.toString());
        }
    }

}
