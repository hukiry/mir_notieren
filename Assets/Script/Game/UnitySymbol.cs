
namespace Hukiry
{
	public static class UnitySymbol
	{
		/// <summary>
		/// UNITY_EDITOR 平台
		/// </summary>
		public const string UNITY_EDITOR = nameof(UNITY_EDITOR);
		/// <summary>
		/// UNITY_ANDROID 平台
		/// </summary>
		public const string UNITY_ANDROID = nameof(UNITY_ANDROID);
		/// <summary>
		/// UNITY_IOS 平台
		/// </summary>
		public const string UNITY_IOS = nameof(UNITY_IOS);
		/// <summary>
		/// UNITY_STANDALONE_OSX 专门为macos（包括Universal, PPC，Intel architectures）平台的定义
		/// </summary>
		public const string UNITY_STANDALONE_OSX = nameof(UNITY_STANDALONE_OSX);
		/// <summary>
		/// UNITY_STANDALONE_WIN 专门为windows平台的定义
		/// </summary>
		public const string UNITY_STANDALONE_WIN = nameof(UNITY_STANDALONE_WIN);
		/// <summary>
		/// UNITY_STANDALONE_LINUX 专门为Linux平台的定义
		/// </summary>
		public const string UNITY_STANDALONE_LINUX = nameof(UNITY_STANDALONE_LINUX);
		/// <summary>
		/// UNITY_STANDALONE 独立平台(Mac OS X, Windows or Linux).
		/// </summary>
		public const string UNITY_STANDALONE = nameof(UNITY_STANDALONE);
		/// <summary>
		/// DEBUG 测试
		/// </summary>
		public const string DEBUG = nameof(DEBUG);
		/// <summary>
		/// DEVELOP 开发
		/// </summary>
		public const string DEVELOP = nameof(DEVELOP);
		/// <summary>
		/// RELEASE 发布
		/// </summary>
		public const string RELEASE = nameof(RELEASE);
		/// <summary>
		/// 启动SDK模式
		/// </summary>
		public const string ENABLE_SDK = nameof(ENABLE_SDK);
		/// <summary>
		/// 电池信息
		/// </summary>
		public const string SYSTEM_INFO = nameof(SYSTEM_INFO);
		/// <summary>
		/// fps刷帧
		/// </summary>
		public const string ENABLE_FPS = nameof(ENABLE_FPS);
		/// <summary>
		/// ab文件编辑模式下测试
		/// </summary>
		public const string ASSETBUNDLE_TEST = nameof(ASSETBUNDLE_TEST);
		/// <summary>
		/// 启动联网
		/// </summary>
		public const string ENABLE_SOCKET = nameof(ENABLE_SOCKET);
		/// <summary>
		/// 强联网
		/// </summary>
		public const string STRONG_SOCKET = nameof(STRONG_SOCKET);
		/// <summary>
		/// 打包时是否使用C#代码
		/// </summary>
		public const string USE_CCHARP = nameof(USE_CCHARP);


		/// <summary>
		/// ab文件热更新下测试
		/// </summary>
		public const string HOTUPDATE_TEST = nameof(HOTUPDATE_TEST);
		/// <summary>
		/// 启动热更新：单击和联网版
		/// </summary>
		public const string ENABLE_HOTUPDATE = nameof(ENABLE_HOTUPDATE);
		/// <summary>
		/// 打包android模块层
		/// </summary>
		public const string MODEL_LAYER = nameof(MODEL_LAYER);


		/// <summary>
		/// 启动Lua模块
		/// </summary>
		public const string ENABLE_LUA = nameof(ENABLE_LUA);
	}
}