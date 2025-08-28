using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Hukiry.SDK
{
    public class SdkManager : MonoBehaviour
	{
		/// <summary>
		/// 回调Unity的函数名
		/// </summary>
		private static readonly string UNITY_FUNCTION_NAME = nameof(OnSDKToUnity);
		/// <summary>
		/// 解析函数Key名
		/// </summary>
		private static readonly string CALL_BACK_FUNCTION_KEY = nameof(CALL_BACK_FUNCTION_KEY);
		/// <summary>
		/// 解析json 数据Key名
		/// </summary>
		private static readonly string JSON_DATA_KEY = nameof(JSON_DATA_KEY);

		private IGameSDK _gameSDK;
		/// <summary>
		/// unity 函数注册:Ios或Android
		/// </summary>
		private CallBackJsonParamHandler m_SdkFuncLua = null;
		private QueueProxyUtility m_queueProxyUtility;

		private static SdkManager _instance;
		public static SdkManager ins
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject().AddComponent<SdkManager>();
					_instance.name = "_GameSDKManager";
					DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}
        public AppleLoginVerify AppleLoginVerify => iOSInternalBase.AppleLoginVerify;
		

		void Awake()
		{
			_instance = this;
		}

        public void InitSDKInformation()
		{
			m_queueProxyUtility = gameObject.GetComponent<QueueProxyUtility>()?? gameObject.AddComponent<QueueProxyUtility>();
			if (_gameSDK == null)
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				_gameSDK = new WindowSDK();
#elif UNITY_ANDROID
				_gameSDK = new AndroidSDK();
#elif UNITY_IOS || UNITY_IPHONE
				_gameSDK = new iPhoneSDK();
#endif
				UnityFireBase.ins.InitFireBase();
				_gameSDK.Awake(gameObject, () => m_SdkFuncLua);
				_gameSDK.StartSDK(_instance.name, UNITY_FUNCTION_NAME, CALL_BACK_FUNCTION_KEY, JSON_DATA_KEY);
			}
		}

		internal void AddProxyUnit(ProxyUnit item)
		{
			m_queueProxyUtility?.Add(item);
		}
		///
		/// <summary>
		/// C#，Lua 注册回调函数
		/// </summary>
		/// <param name="functionkey">方法名</param>
		/// <param name="luaFunction">回调函数</param>
		public void RegeditFunction(CallBackJsonParamHandler luaFunction)
		{
			this.m_SdkFuncLua = luaFunction;
		}

		/// <summary>
		/// C# 调用 SDK
		/// </summary>
		public void CallSDKFunction(UnityParam param)
		{
			_gameSDK.CallSDKFunction(param);
		}

        /// <summary>
        /// Lua或C# 调用 SDK
        /// </summary>
        /// <param name="funType">枚举sdk函数类型</param>
        /// <param name="jsonParam">参数</param>
        public void CallSDKFunction(int funType, string jsonParam)
        {
            if (funType != SdkFunctionType.AdInit)
            {
                Debug.Log($"unity call sdk：funType = {funType}, jsonParam = {jsonParam}");
            }

			if (string.IsNullOrEmpty(jsonParam)) jsonParam = "{}";
			UnityParam param = new UnityParam();
			param.funType = funType;
			param.jsonParams = jsonParam;
			_gameSDK.CallSDKFunction(param);
        }

        public string GetCallSDKFunction(int funType)
        {
            return _gameSDK.GetCallSDKFunction(funType);
        }

		#region Unity2018—before
		/// <summary>
		/// 被SDK调用:发送消息的机制
		/// </summary>
		/// <param name="json">sdk json</param>
		private void OnSDKToUnity(string jsonParam)
        {
            try
            {
                JsonData jsonObj = JsonMapper.ToObject(jsonParam);//解析字符串
                int functionKey = (int)jsonObj[CALL_BACK_FUNCTION_KEY];
                string json = (string)jsonObj[JSON_DATA_KEY];
                Debug.Log($"unity sdk back call：functionKey = {functionKey}, jsonParam = {jsonParam}");
                this.m_SdkFuncLua?.Invoke(jsonParam);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"json解析失败！jsonParam ={jsonParam}，{ex.ToString()}");
            }
        }
        #endregion

        #region 获取语言，app版本，ip
        /// <summary>
        /// 获取平台的ip
        /// </summary>
        internal string GetPlatformIP(string ip, string port)
		{
			var newServerIp = ip;
#if UNITY_EDITOR

#elif UNITY_IPHONE || UNITY_IOS
			try
			{
				string mIPv6 = iOSInternalUtility._GetIPv6(ip, port);
				if (!string.IsNullOrEmpty(mIPv6))
				{
					string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
					if (m_StrTemp != null && m_StrTemp.Length >= 2)
					{
						string IPType = m_StrTemp[1];
						if (IPType == "ipv6")
						{
							newServerIp = m_StrTemp[0];
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogManager.LogError("ip异常：" + ex.ToString());
			}
#endif
			return newServerIp;
		}

		/*	20种语言地区代码********************************************************************************
		*	简体		zh-cn	繁体		zh-hk	英文		en		西班牙语	es		葡萄牙语	pt	
		*	法语		fr		德语		de		俄语		ru		意大利语	it		日语		ja		
		*	韩语		ko		土耳其语 tr		越南语	vi		波兰语   pl		印尼语   id		
		*	泰语		th		马来语	ms		荷兰语   nl		希腊语	el		捷克语	cs
		*/
		private static readonly Dictionary<string, byte> m_languageCode = new Dictionary<string, byte>() {
			{"cn",1 }, {"hk",1 }, {"en",1 }, {"es",1 }, {"pt",1 },
			{"fr",1 }, {"de",1 }, {"ru",1 }, {"it",1 }, {"ja",1 },
			{"ko",1 }, {"tr",1 }, {"vi",1 }, {"pl",1 }, {"id",1 },
			{"th",1 }, {"ms",1 }, {"nl",1 }, {"el",1 }, {"cs",1 }
        };
		private const string DEFAULT_LANGUAGECODE = "en";

		/// <summary>
		/// 获取语言代码
		/// </summary>
		public string getLanguageCode()
		{
			string tag = DEFAULT_LANGUAGECODE;
			//获取上一次保存的语言
			string lanCode = PlayerPrefs.GetString("SYSTEM_LANGUAGE_KEY", "");
			if (!string.IsNullOrEmpty(lanCode))
			{
				return lanCode;
			}
#if UNITY_EDITOR
			tag = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
			if (tag.ToLower().StartsWith("zh"))
			{
				if (tag.ToLower().Contains("cn"))
				{
					tag = "cn";
				}
				else
				{
					tag = "hk";
				}
			}
#elif UNITY_IPHONE || UNITY_IOS
			tag = iOSInternalUtility._getLocationLanguage();
			if (tag.ToLower().StartsWith("hans"))
			{
				tag = "cn";
			}
			else if (tag.ToLower().StartsWith("hant"))
			{
				tag = "hk";
			}
#elif UNITY_ANDROID
			tag = this.GetCallSDKFunction(SdkFunctionType.GetSystemLanguage);
			if (tag.StartsWith("zh"))
			{
				if (tag.ToLower().Contains("cn"))
				{
					tag = "cn";
				}
				else
				{
					tag = "hk";
				}
			}
#endif
			LogManager.Log("系统语言代码：", tag);
			tag = tag.Substring(0, 2)?.ToLower();
			if (!m_languageCode.ContainsKey(tag))//如果没有地区语言，设置为默认语言
			{
				tag = DEFAULT_LANGUAGECODE;
			}
			return tag;
		}

		/// <summary>
		/// 获取app版本
		/// </summary>
		public string getAppVersionName()
		{

#if UNITY_EDITOR
			return Application.version;
#elif UNITY_ANDROID
			return this.GetCallSDKFunction(SdkFunctionType.GetAppVersionName);
#elif UNITY_IOS || UNITY_IPHONE
			return iOSInternalUtility._getAppVersionName();
#endif
		}

		/// <summary>
		/// Ios13以上 显示登录：仅iOS
		/// </summary>
		public bool isShowAppleLogin()
		{
#if UNITY_EDITOR
			return true;
#elif UNITY_IOS || UNITY_IPHONE
			return iOSInternalUtility._isAvailable13() == 1;
#else
			return false;
#endif
		}

		/// <summary>
		/// 获取手机设备型号：仅iOS
		/// </summary>
		public string getDeviceModel()
		{
#if UNITY_EDITOR
			return string.Empty;
#elif UNITY_IOS || UNITY_IPHONE
			return iOSInternalUtility._getDeviceModel();
#else
			return string.Empty;
#endif
		}
		#endregion

		/// <summary>
		/// 动态改变本地化语言
		/// </summary>
		public void ChanageLocalizedText()
		{
			LocalizedLanguage.RefreshChangeLanguage();
		}
	}
}