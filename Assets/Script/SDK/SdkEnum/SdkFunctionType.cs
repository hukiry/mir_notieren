namespace Hukiry.SDK
{
    /// <summary>
    /// 与Lua，OC, JAVA一致
    /// </summary>
    public class SdkFunctionType
    {
        /// <summary>
        /// 网络不可见
        /// </summary>
        public const int NetworkUnavailable = 100;
        /// <summary>
        /// 初始化 SDK
        /// </summary>
        public const int Init = 101;
        public const int InitSucces = 102;
        public const int InitFail = 103;

        /// <summary>
        /// 登录拉起
        /// </summary>
        public const int LoginFetch = 201;
        public const int LoginSucces = 202;
        public const int LoginFail = 203;
        public const int LoginCancel = 204;
        /// <summary>
        /// 苹果登出令牌撤销
        /// </summary>
        public const int LoginToken = 205;
        public const int LoginRevoke = 206;
        /// <summary>
        /// 登出
        /// </summary>
        public const int LogoutFetch = 301;
        public const int LogoutSucces = 302;
        public const int LogoutFail = 303;

        /// <summary>
        /// 支付拉起
        /// </summary>
        public const int PayFetch = 401;
        /// <summary>
        /// 支付成功
        /// </summary>
        public const int PaySucces = 402;
        /// <summary>
        /// 支付失败
        /// </summary>
        public const int PayFail = 403;
        /// <summary>
        /// 支付取消
        /// </summary>
        public const int PayCancel = 404;
        /// <summary>
        /// 支付凭证验证成功：可以发货
        /// </summary>
        public const int PayVerifyRecipeSucces = 405;
        /// <summary>
        /// 支付凭证验证失败
        /// </summary>
        public const int PayVerifyRecipeFail = 406;
        /// <summary>
        /// 初始化商品列表拉起
        /// </summary>
        public const int InitProductFetch = 501;
        /// <summary>
        /// 初始化商品列表成功
        /// </summary>
        public const int InitProductSucces = 502;
        /// <summary>
        /// 初始化商品列表失败
        /// </summary>
        public const int InitProductFail = 503;

        //广告初始化
        public const int AdInit = 601;
        public const int AdInitSucces = 602;
        public const int AdInitFail = 603;

        //广告拉起
        public const int AdRewardFetch = 611;
        public const int AdRewardSucces = 612;
        public const int AdRewardFail = 613;

        //仅android 检查google服务
        public const int CheckGooglePlay = 701;
        //检查google服务成功
        public const int CheckGooglePlaySuccessful = 702;
        //检查google服务失败
        public const int CheckGooglePlayFail = 703;

        //离线通知
        public const int LocalNotification = 801;

        //--------------------------其他部分
        public const int GetSystemLanguage = 1;//获取系统语言代码
        public const int GetAppVersionName = 2; //获取App版本号
        //更新SDK 语言
        public const int UpdateSDKLanguage = 3;
        //用户协议
        public const int UserAgreement = 4;
        //数据统计
        public const int CustomsEventUp = 5;
        
    }
}