using System;

namespace Hukiry.SDK
{
    /// <summary>
    /// 商品数据
    /// </summary>
    [Serializable]
    public class ProductData
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public string id;
        /// <summary>
        /// 商品价格
        /// </summary>
        public float Price;
        /// <summary>
        /// 商品标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 价格字符串
        /// </summary>
        public string PriceString;
    }
}
