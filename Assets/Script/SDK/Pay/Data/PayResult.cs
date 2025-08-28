namespace Hukiry.SDK
{
    /// <summary>
    /// 支付返回结果
    /// </summary>
    [System.Serializable]
    public class PayResult
    {
        /// <summary>
        /// 支付商品
        /// </summary>
        public string productId = string.Empty;
        /// <summary>
        /// 支付凭证
        /// </summary>
        public string receipt = string.Empty;
    }
}