
namespace Hukiry.SDK
{
    /// <summary>
    /// json参数传递 ,ios
    /// </summary>
    [System.Serializable]
    public class JsonParams
    {
        //字符串参数传递
        public string stringKey1 = string.Empty;
        public string stringKey2 = string.Empty;
        public string stringKey3 = string.Empty;
        //整数参数传递
        public int intKey1;
        public int intKey2;
        public int intKey3;
        //布尔参数传递
        public bool boolKey1;
        public bool boolKey2;
        public bool boolKey3;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errorMsg = string.Empty;
        public string message = string.Empty;

        public JsonParams() { }

        public JsonParams(string msg, bool isMessge = false)
        {
            if (isMessge)
                this.message = msg;
            else
                this.errorMsg = msg;
        }

        public JsonParams(int intKey1) { 
            this.intKey1 = intKey1;
        }
        public JsonParams(int intKey1, int intKey2) {
            this.intKey1 = intKey1;
            this.intKey2 = intKey2;
        }
        public JsonParams(int intKey1, int intKey2, int intKey3) {
            this.intKey1 = intKey1;
            this.intKey2 = intKey2;
            this.intKey3 = intKey3;
        }

        public JsonParams(bool boolKey1)
        {
            this.boolKey1 = boolKey1;
        }
        public JsonParams(bool boolKey1, bool boolKey2)
        {
            this.boolKey1 = boolKey1;
            this.boolKey2 = boolKey2;
        }
        public JsonParams(bool boolKey1, bool boolKey2, bool boolKey3)
        {
            this.boolKey1 = boolKey1;
            this.boolKey2 = boolKey2;
            this.boolKey3 = boolKey3;
        }

        public JsonParams(string stringKey1, string stringKey2) { 
            this.stringKey1 = stringKey1; this.stringKey2 = stringKey2; 
        }

        public JsonParams(string stringKey1, string stringKey2, string stringKey3) { 
            this.stringKey1 = stringKey1; 
            this.stringKey2 = stringKey2;
            this.stringKey3 = stringKey3;
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}