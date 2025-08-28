package com.five.pay;

import android.app.Activity;
import android.util.Log;
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
import com.five.mergegame.ESdkFunctionType;
import com.five.mergegame.UnityParam;
import com.five.util.UnityUtil;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;

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
                        Log.w(TAG, "JSONObject failed code=" + e.toString());
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

    //查询商品清单
    public void searchProductDetails(List<String> array)
    {
        if(!this.isReadyPay) {
            Log.d(TAG, "product_id================================================"+String.valueOf(array.size()));
            this.startConnection();
            return;
        }
        List<QueryProductDetailsParams.Product> products = new ArrayList<>();
        for (String product_id:array)
        {
            Log.d(TAG, "product_id================================================"+product_id);
            products.add(QueryProductDetailsParams.Product.newBuilder()
                    .setProductId(product_id)
                    .setProductType(BillingClient.ProductType.INAPP)
                    .build());
        }

        QueryProductDetailsParams queryProductDetailsParams =QueryProductDetailsParams.newBuilder()
                .setProductList(products).build();

        billingClient.queryProductDetailsAsync( queryProductDetailsParams,new ProductDetailsResponseListener() {
                public void onProductDetailsResponse(BillingResult billingResult,List<ProductDetails> productDetailsList) {
                    // check billingResult
                    // process returned productDetailsList
                    if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                        StringBuilder items = new StringBuilder("[");
                        ProductDetails pResult = null;
                        for (ProductDetails p : productDetailsList) {
                            if (!dicProduct.containsKey(p.getProductId()))
                                dicProduct.put(p.getProductId(), p);

                            items.append("{\"product_id\":").append(p.getProductId()).append(",")
                                    .append("\"name\":").append(p.getName()).append(",")
                                    .append("\"type\":").append(p.getProductType()).append(",")
                                    .append("\"price\":").append(p.getOneTimePurchaseOfferDetails().getFormattedPrice()).append(",")
                                    .append("\"currencyCode\":").append(p.getOneTimePurchaseOfferDetails().getPriceCurrencyCode()).append(",")
                                    .append("\"amount\":").append(p.getOneTimePurchaseOfferDetails().getPriceAmountMicros()).append("},");
                            if (Objects.equals(GoogleBilling.this.curPayProductId, p.getProductId()))
                            {
                                Log.d(TAG, "查询结果================================================"+p.getProductId());
                                pResult = p;
                                GoogleBilling.this.curPayProductId = "";
                            }
                        }
                        items.append("]");

                        Log.d(TAG, items.toString());
                        UnityParam param = new UnityParam();
                        param.funType = ESdkFunctionType.productListSuccessful;
                        try {
                            JSONObject jsonObject = new JSONObject();
                            jsonObject.put("items", items.toString());
                            param.jsonParams = jsonObject.toString();
                        } catch (JSONException e) {
                            Log.w(TAG, "signInResult:failed code=" + e.toString());
                        }

                        Log.d(TAG, "初始化商品列表================================dicProduct="+dicProduct.size());
                        UnityUtil.JavaToUnity(param);

                        if (pResult!=null)
                        {
                            Log.d(TAG, "开始支付=================================");
                            GoogleBilling.this.startPay(pResult);
                        }
                        else
                        {
                            Log.d(TAG, "查询失败=================================");
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
