#if USE_CCHARP
using Hukiry.SDK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSDK : MonoBehaviour
{
    //支付
    public class SendBillingData
    {
        /// <summary>
        /// 货品id
        /// </summary>
        public int id;
        /// <summary>
        /// 商品id
        /// </summary>
        public string productId;
    }

    public class ProductDetails
    {
        public string product_id;
        public string name;
        public string type;
        public string price;
        public string currencyCode;
        public string amount;
    }

    public class ProductGoogle
    {
        public static Dictionary<int, ProductDetails> dic = new Dictionary<int, ProductDetails>()
        {
            { 1,new ProductDetails(){ product_id="com.hooeygamez.arrangement290",   amount="29",  price="$2.90"}},
            { 2,new ProductDetails(){ product_id="com.hooeygamez.arrangement2990",  amount="299", price="$29.90"}},
            { 3,new ProductDetails(){ product_id="com.hooeygamez.arrangement4990",  amount="499", price="$49.90"}},
            { 4,new ProductDetails(){ product_id="com.hooeygamez.arrangement9990",  amount="999", price="$99.90"}},
	    };

        public static List<string> ProductList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var item in dic.Values)
                {
                    list.Add(item.product_id);

                }
                return list;
            }
        }
    }

    private Dictionary<string, ProductDetails> productList = new Dictionary<string, ProductDetails>();
    private SendBillingData sendBillingData;
    private Action<SendBillingData> backShopView;
    private bool IsInitSDK = false;
    private void Start()
    {
        this.InitSDK();
    }

    public void InitSDK()
    {
        IsInitSDK = true;
        SdkManager.ins.InitSDKInformation();
        SdkManager.ins.RegeditFunction(OnBackSDK);
    }

    private void OnBackSDK(string param)
    {
        UnityParam json = JsonConvert.DeserializeObject<UnityParam>(param);
        if (json.funType == SdkFunctionType.InitProductSucces)
        {
            Debug.Log("jsonParams:" + json.jsonParams);
            var data = LitJson.JsonMapper.ToObject<List<ProductDetails>>(json.jsonParams);
            if (data != null)
            {
                foreach (var item in data)
                {
                    productList[item.product_id] = item;
                }
            }
        }
        else if (json.funType >= SdkFunctionType.PaySucces && json.funType <= SdkFunctionType.PayVerifyRecipeFail)
        {
            if (json.funType == SdkFunctionType.PaySucces)
            {
                UnityEngine.Debug.Log("IAP >> OnPurchaseSuccess" + sendBillingData.productId);
                backShopView?.Invoke(sendBillingData);
            }
            else
            {
                backShopView?.Invoke(null);
                Debug.LogError($"funType:{json.funType},errorCode:{json.errorCode},errorMsg:{json.errorMsg}");
            }
        }
    }

    //拉起商品列表
    public void FetchProductList()
    {
        if (productList.Count > 0) return;
        SdkManager.ins.CallSDKFunction(new UnityParam()
        {
            funType = SdkFunctionType.InitProductFetch,
            jsonParams = LitJson.JsonMapper.ToJson(ProductGoogle.ProductList)
        });
    }

    //拉起支付SDK
    public bool FetchPay(int Id)
    {
        if (IsInitSDK)
        {
            if (ProductGoogle.dic.ContainsKey(Id))
            {
                sendBillingData = new SendBillingData();
                sendBillingData.id = Id;
                sendBillingData.productId = ProductGoogle.dic[Id].product_id;
                SdkManager.ins.CallSDKFunction(new UnityParam()
                {
                    funType = SdkFunctionType.PayFetch,
                    jsonParams = LitJson.JsonMapper.ToJson(sendBillingData)
                });
            }
            else
            {
                Debug.LogError("no config that ID:" + Id);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 注册视图，购买成功返回
    /// </summary>
    /// <param name="backShopView"></param>
    public void AddShopBack(Action<SendBillingData> backShopView)
    {
        this.backShopView = backShopView;
    }

    //获取商品id
    public ProductDetails GetProductDetails(int id)
    {
        string product = string.Empty;
        foreach (var item in ProductGoogle.dic)
        {
            if (item.Key == id)
            {
                product = item.Value.product_id;
            }
        }

        if (productList.Count > 0)
        {
            if (productList.ContainsKey(product))
            {
                return productList[product];
            }
        }
        else
        {
            if (ProductGoogle.dic.ContainsKey(id))
            {
                return ProductGoogle.dic[id];
            }
        }
        return null;
    }
}
#endif