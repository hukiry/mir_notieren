using Hukiry.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 标签层
/// </summary>
public class TagLayer
{
	public static string Untagged = "Untagged";
	public static string UI_CSharp = "UI_CSharp";

	//系统 和子模板
	public static string UI_Lua = "UI_LuaFull";

	public static string UI_LuaTemplate = "UI_LuaItem";
	public static string UI_LuaDataTemplate = "UI_LuaItemAndData";

	/// <summary>
	/// 对话框和提示框
	/// </summary>
	public static string UI_LuaView = "UI_LuaView";
	public static string UI_LuaViewData = "UI_LuaViewAndData";

	public static string UI_LuaPanel = "UI_LuaPanel";

	public static string UIScene_LuaView = "UI_LuaSceneView";

	public static List<string> UI_Control = new List<string>()
	{
		typeof(Image).Name,
		typeof(Text).Name,
		typeof(AtlasImage).Name,
		typeof(Hukiry.HukirySupperText).Name,
		typeof(UIProgressbarMask).Name,
		typeof(CanvasGroup).Name,
		typeof(MeshGraphic).Name,
		typeof(GameObject).Name,
		typeof(Transform).Name,
		typeof(RawImage).Name,
		typeof(InputField).Name,
		typeof(Dropdown).Name,
		typeof(Toggle).Name,
		typeof(Slider).Name,
		typeof(ScrollRect).Name,
		typeof(SpriteRenderer).Name,
	};
}

/// <summary>
/// 相机渲染剔除层
/// </summary>
public class CameraRenderLayers
{
	public static string[] RenderLayers = {
		"Item",
		"Npc",
		"Model"
    };
}


/// <summary>
/// 对象层级排序
/// </summary>
public class ObjectSortingLayers
{
	public static List<string> SortingLayers = new List<string>()
	{
		"Default",
		"Background",
		"Item",
		"Cloud",
		"Effect",

		//画布UI层级部分
		"UIScene",//UI 场景层
		"UIFixed",//主界面固定UI层
		"UIWindow", //全屏窗口层
		"UIGuide", //新手引导层
		"UIAnimation", //动画层：货币飞起动画
		"UISceneLoad", //加载场景进度条层
		"UITips", //Tips层，网络弹框
		"UILoading", //网络菊花层
	};
}



