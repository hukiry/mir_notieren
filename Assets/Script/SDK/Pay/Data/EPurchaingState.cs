namespace Hukiry.SDK
{
    public enum EPurchaingState
    {
        /// <summary>
        /// 网络不可见
        /// </summary>
        NetworkUnavailable,
        /// <summary>
        /// 支付成功：关闭界面
        /// </summary>
        PaySuccessful,
        /// <summary>
        /// 支付失败
        /// </summary>
        PayFail,
        /// <summary>
        /// 支付取消
        /// </summary>
        PayCancel,
        /// <summary>
        /// 商品初始化成功
        /// </summary>
        InitProductSuccessful,
        /// <summary>
        /// 商品初始化失败
        /// </summary>
        InitProductFail,
        /// <summary>
        /// 验证支付成功，立马发货
        /// </summary>
        VerifySuccessful,
        /// <summary>
        /// 验证上传失败，取消发货
        /// </summary>
        VerifyFail
    }
}
