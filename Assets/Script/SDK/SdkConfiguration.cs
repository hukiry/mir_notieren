namespace Hukiry.SDK
{
    public class SdkConfiguration
    {
        /// <summary>
        /// 游戏包名
        /// </summary>
        public static string AndroidPackageName = "com.yiliu.gamelibrary";

        /*=========================Android部分---------------------------------*/
        /// <summary>
        /// 资源库库目录
        /// </summary>
        public const string AndroidLibraryDir = "../LibraryAndroid";
        /// <summary>
        /// 接口类，固定永不变
        /// </summary>
        public const string AndroidInterfaceClass = "com.yiliu.util.UnityInterfaceProxy";
        /// <summary>
        /// 结构体数据类，固定永不变
        /// </summary>
        public const string AndroidUnityParamClass = "com.yiliu.util.UnityParam";
        /// <summary>
        /// 主活动入口类接入，固定永不变
        /// </summary>
        public const string AndroidUnityPlayerClass = "com.unity3d.player.UnityPlayer";
        /// <summary>
        /// 当前活动类，固定永不变
        /// </summary>
        public const string AndroidCurrentActivity = "currentActivity";



        /*=========================IOS部分---------------------------------*/
        /// <summary>
        /// 内购公私
        /// </summary>
        public static string AppPublicKey = string.Empty;
        /// <summary>
        /// 内购名
        /// </summary>
        public static string iOSPurchasePayName = "com.yiliu.gamelibrary.pay";
    }
}
