using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Hukiry/PackingWinConfigAssets", order = 3)]
public class PackingWinConfigAssets : CommonAssets<PackingWinConfigAssets>
{
	/*打包窗口配置*/
	[Description("清除缓存资源")]
	public  bool ClearCache;
	[Description("清除标记资源")]
	public  bool ClearMarkRes;

	[Description("Lua代码")] public bool Lua;
	[Description("重新生成所有资源")] public  bool Res;
	[Description("打包场景")] public  bool BuildScene;

	[Description("启动分包")] public  bool IsDelivery;
	/// <summary>
	/// 打印日志
	/// </summary>
	[Description("打印日志")] public  bool DebugLog;
	/// <summary>
	/// 显示帧率
	/// </summary>
	[Description("显示帧率")] public  bool Fps;
	/// <summary>
	/// SDK
	/// </summary>
	[Description("Enable SDK")] public  bool Enable_SDK = false;
	/// <summary>
	/// 显示电池
	/// </summary>
	[Description("显示电池")] public bool Battery = false;

	[Description("Armv7")] public  bool Armv7 = false;
	[Description("Arm64")] public  bool Arm64 = false;
	[Description("编译包")] public  bool BuildPlayer;
	/// <summary>
	/// 包名
	/// </summary>
	[Description("包名")] public string identifier;
	[Description("游戏名")] public string productName;
	[Description("App版本号")] public int appVersion=0;

	public string WorkMode = "Debug";
	public PackageType packageType;

	public override void SaveAssetsBefore()
	{
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}


}

/// <summary>
/// 出包类型
/// </summary>
public enum PackageType
{
	/// <summary>
	/// 测试包
	/// </summary>
	apk = 0,
	/// <summary>
	/// Google包 Gradle
	/// </summary>
	aab = 1,
	/// <summary>
	/// 导出android工程
	/// </summary>
	exportProject = 2
}

public enum EBuildSystem
{
	Internal,
	Gradle,
}
