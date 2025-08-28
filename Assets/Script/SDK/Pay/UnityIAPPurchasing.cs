using LitJson;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_SDK
using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;
#endif

namespace Hukiry.SDK
{

    class UnityIAPPurchasing
#if ENABLE_SDK
    //IDetailedStoreListener/*UNITY_2021*/
     :IStoreListener
    {
       
        private IStoreController storeController;
        private IExtensionProvider extensionProvider;
        private IAppleExtensions appleExtensions;
        private string publicKey { get; } = Resources.Load<TextAsset>(nameof(SdkConfiguration.AppPublicKey))?.text;

        private string[] products;
        private bool isInitSuccessful;

        private CallBackPurchaingHandler purchaingHandler;
        private bool isVerifyReceipt;
        /// <summary>
        /// 支付时，发现没有初始化成功，则缓存数据。然后进行初始化商品，在支付。
        /// </summary>
        private PayResult payResult = null;

        private const string RECEIPT_KEY = "RECEIPT";

        private bool IsNotNetwork() => Application.internetReachability == NetworkReachability.NotReachable;

        /// <summary>
        /// 初始化监听函数
        /// </summary>
        /// <param name="purchaingHandler">回调函数</param>
        /// <param name="isVerifyReceipt">默认不自动验证</param>
        public void InitPurchasingListener(CallBackPurchaingHandler purchaingHandler, bool isVerifyReceipt = false)
        {
            this.payResult = new PayResult();
            this.purchaingHandler = purchaingHandler;
            this.isVerifyReceipt = isVerifyReceipt;

            this.CheckReceipt();
        }

        private void CheckReceipt()
        {
            if (this.isVerifyReceipt)
            {
                string receiptData = PlayerPrefs.GetString(RECEIPT_KEY, string.Empty);
                if (!string.IsNullOrEmpty(receiptData))
                {
                    this.payResult = JsonConvert.DeserializeObject<PayResult>(receiptData);
                    this.UpPurchaseReceipt(this.payResult.receipt);
                }
            }
        }

        //获取产品列表
        public void SearchProductList(string[] productList)
        {
            if (isInitSuccessful)
            {
                this.InitProductSuccessful();
            }
            else
            {
                this.products = productList;
                this.StartInitializeProducts();
            }
        }

        //支付
        public void Pay(string productID)
        {
            if (IsNotNetwork())
            {
                this.purchaingHandler(EPurchaingState.NetworkUnavailable, new JsonParams("Please connect to network").ToJson()) ;
                return;
            }

            this.payResult.productId = productID;
            if (!this.isInitSuccessful)
            {
                this.StartInitializeProducts();
            }
            else
            {
                Product product = this.storeController.products.WithID(productID);
                if (product != null)
                {
                    this.CheckReceipt();
                    this.storeController.InitiatePurchase(product);
                }
                else
                {
                    LogManager.LogError("Shop id error：", productID);
                }
            }
        }

        private void StartInitializeProducts()
        {
            var modul = StandardPurchasingModule.Instance();
            modul.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            var builder = ConfigurationBuilder.Instance(modul);
            for (int i = 0; i < this.products.Length; i++)
            {
                builder.AddProduct(this.products[i], ProductType.Consumable);
            }
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// 初始化产品成功
        /// </summary>
        private void InitProductSuccessful()
        {
            List<ProductData> productList = new List<ProductData>();
            foreach (var product in this.storeController.products.all)
            {
                productList.Add(new ProductData
                {
                    id = product.definition.id,
                    Title = product.metadata.localizedTitle,
                    Price = (float)product.metadata.localizedPrice,
                    PriceString = product.metadata.localizedPriceString
                });
            }
            string priceJson = JsonConvert.SerializeObject(productList);
            this.purchaingHandler(EPurchaingState.InitProductSuccessful, priceJson);
        }

#region 商品初始化

        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.storeController = controller;
            this.extensionProvider = extensions;
            this.appleExtensions = this.extensionProvider.GetExtension<IAppleExtensions>();
            this.appleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
            this.isInitSuccessful = true;
            this.InitProductSuccessful();
        }

        private void OnDeferred(Product obj)
        {
            LogManager.Log("Pay network Deferred");
        }

        //[System.Obsolete]
        //void IStoreListener.OnInitializeFailed(InitializationFailureReason error){ }
        //void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        //{
        //    this.isInitSuccessful = false;
        //    this.purchaingHandler(EPurchaingState.InitProductFail, $"{{\"errorMsg\":\"{error},{message}\"}}");
        //    LogManager.Log("init:", message);
        //}

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            this.isInitSuccessful = false;
            string errorMsg = "无法识别APP商品";
            if (error == InitializationFailureReason.PurchasingUnavailable)
            {
                errorMsg = "禁止付费, 可能关闭app内购功能";
            }
            else if (error == InitializationFailureReason.NoProductsAvailable)
            {
                errorMsg = "无可用商品";
            }
            this.purchaingHandler(EPurchaingState.InitProductFail, new JsonParams($"{error},{errorMsg}").ToJson());
            LogManager.Log("InitializeFailed:", errorMsg);
        }

#endregion

#region 支付部分
        //[System.Obsolete]
        //void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) { }
        //void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        //{
        //    var resultReason = failureDescription.reason == PurchaseFailureReason.UserCancelled ? EPurchaingState.PayCancel : EPurchaingState.PayFail;
        //    this.purchaingHandler(resultReason, $"{{\"errorMsg\":\"{failureDescription.reason},{failureDescription.message}\"}}");
        //}

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            LogManager.Log("pay:", failureReason);
            var metadata = JsonUtility.ToJson(product.metadata);
            var resultReason = failureReason == PurchaseFailureReason.UserCancelled ? EPurchaingState.PayCancel : EPurchaingState.PayFail;
            this.purchaingHandler(resultReason, new JsonParams($"{failureReason},{metadata}").ToJson());
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var metadata = JsonUtility.ToJson(purchaseEvent.purchasedProduct.metadata);
            if (purchaseEvent.purchasedProduct.hasReceipt)
            {
                //UnifiedReceipt unifiedReceipt = JsonUtility.FromJson<UnifiedReceipt>(purchaseEvent.purchasedProduct.receipt);
                string receipt = this.appleExtensions.GetTransactionReceiptForProduct(purchaseEvent.purchasedProduct);
                this.UpPurchaseReceipt(receipt);
            }

            this.purchaingHandler(EPurchaingState.PaySuccessful, new JsonParams($"receipt={purchaseEvent.purchasedProduct.receipt},metadata={metadata}", true).ToJson());
            return PurchaseProcessingResult.Complete;
        }

#endregion

#region 上传凭证
        //21000 App Store无法读取你提供的JSON数据
        //21002 收据数据不符合格式
        //21003 收据无法被验证
        //21004 你提供的共享密钥和账户的共享密钥不一致
        //21005 收据服务器当前不可用
        //21006 收据是有效的，但订阅服务已经过期。当收到这个信息时，解码后的收据信息也包含在返回内容中
        //21007 收据信息是测试用（sandbox），但却被发送到产品环境中验证
        //21008 收据信息是产品环境中使用，但却被发送到测试环境中验证
        //21003解决方案：将共享密钥也传给苹果去验证
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receipt"></param>
        /// <param name="isSandbox">参数二，不需要传入，默认为正式</param>
        private void UpPurchaseReceipt(string receipt, bool isSandbox = false)
        {
#if RELEASE && !UNITY_EDITOR
            string url = "https://buy.itunes.apple.com/verifyReceipt";
            if(isSandbox)
            {
                url = "https://sandbox.itunes.apple.com/verifyReceipt";
            }
#else
            string url = "https://sandbox.itunes.apple.com/verifyReceipt";
#endif
            string formJson =  $"{{\"receipt-data\":\"{receipt}\",\"password\":\"{publicKey}\"}}";
            new UwrPostAppleJson(url, formJson, (isSuccess, text) =>
            {
                if (isSuccess)
                {
                    JsonData resoultJson = JsonMapper.ToObject(text);
                    if (resoultJson["status"].ToString() == "0")
                    {
                        LogManager.Log("up receipt sucessful", text);
                        this.purchaingHandler(EPurchaingState.VerifySuccessful, new JsonParams(this.payResult.productId,"").ToJson());
                        //恢复购买
                        this.appleExtensions.RestoreTransactions((ok) => LogManager.Log("Restore buy ", ok));
                        //this.appleExtensions.RestoreTransactions((ok, str) => LogManager.Log("Restore buy ", str));
                        if (PlayerPrefs.HasKey(RECEIPT_KEY))
                        {
                            PlayerPrefs.DeleteKey(RECEIPT_KEY);
                        }
                    }
                    else
                    {
                        if (resoultJson["status"].ToString() == "21007")
                        {
                            this.UpPurchaseReceipt(receipt, true);
                        }
                        else
                        {
                            this.purchaingHandler(EPurchaingState.VerifyFail, new JsonParams(text).ToJson());
                            LogManager.LogError("up receipt fail", text);
                        }

                    }
                }
                else
                {
                    this.payResult.receipt = receipt;
                    PlayerPrefs.SetString(RECEIPT_KEY, JsonConvert.SerializeObject(this.payResult));
                    this.purchaingHandler(EPurchaingState.VerifyFail, new JsonParams(text).ToJson());
                }
            });
        }

#endregion
#else
    {
        public void InitPurchasingListener(CallBackPurchaingHandler purchaingHandler, bool isVerifyReceipt = false) { }
        public void Pay(string productID) { }
        public void SearchProductList(string[] productList) { }
#endif
    }
}
