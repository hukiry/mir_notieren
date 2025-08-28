using Hukiry;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SymbolSetting : ProjectSettingsScriptable<SymbolSetting>
{
	[Tooltip("是在检测面板显示图标")]//是否显示图标
	public bool isShaowHierarchyIcon = false;
	public Dictionary<int, SymbolInfo> defineSymbols = new Dictionary<int, SymbolInfo> ();

	[Tooltip("启动 Assetbundle 测试")]
	public bool isEnableAssetbundleTest;

	[Tooltip("启动FPS")]
	public bool isEnableFps;

	[Tooltip("启动电池和wifi信号")]
	public bool isEnableWifiView;

	[Tooltip("使用SDK")]
	public bool isEnableSdk;

	[Tooltip("启动Lua插件")]
	public bool isEnableLua;

	[Tooltip("仅编辑模式下，启动 热更新 测试模式")]
	public bool isEnableHotUpdate;

	[Tooltip("启动Android模块层")]
	public bool isEnableModel;

	[Tooltip("启动Socket")]
	public bool isEnableSocket;

	[Tooltip("使用C#部分")]
	public bool isUseCCharp;

	[Tooltip("发布模式")]
	public bool isRelease;

	[Tooltip("强Socket链接")]
	public bool isStrongSocket;

    public override void Refresh()
    {
		defineSymbols[1] = new SymbolInfo() { isEnable = isEnableAssetbundleTest, symbols = UnitySymbol.ASSETBUNDLE_TEST };
		defineSymbols[2] = new SymbolInfo() { isEnable = isEnableFps, symbols = UnitySymbol.ENABLE_FPS };
		defineSymbols[3] = new SymbolInfo() { isEnable = isEnableWifiView, symbols = UnitySymbol.SYSTEM_INFO };
		defineSymbols[4] = new SymbolInfo() { isEnable = isEnableSdk, symbols = UnitySymbol.ENABLE_SDK };
		defineSymbols[5] = new SymbolInfo() { isEnable = isEnableHotUpdate, symbols = UnitySymbol.HOTUPDATE_TEST };
		defineSymbols[6] = new SymbolInfo() { isEnable = isEnableModel, symbols = UnitySymbol.MODEL_LAYER };
		defineSymbols[7] = new SymbolInfo() { isEnable = isEnableSocket, symbols = UnitySymbol.ENABLE_SOCKET };
		defineSymbols[8] = new SymbolInfo() { isEnable = isUseCCharp, symbols = UnitySymbol.USE_CCHARP };
		defineSymbols[9] = new SymbolInfo() { isEnable = isRelease, symbols = UnitySymbol.RELEASE };
		defineSymbols[10] = new SymbolInfo() { isEnable = isStrongSocket, symbols = UnitySymbol.STRONG_SOCKET };
		defineSymbols[11] = new SymbolInfo() { isEnable = isEnableLua, symbols = UnitySymbol.ENABLE_LUA };
		this.SymbolsForGroup();
	}

	private void SymbolsForGroup()
	{
		BuildTargetGroup curBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
		string existSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(curBuildTargetGroup);
		foreach (var item in defineSymbols.Values)
		{
			if (item.isEnable)
			{
				if (!existSymbols.Contains(item.symbols))
				{
					if (string.IsNullOrEmpty(existSymbols))
						existSymbols = item.symbols;
					else
						existSymbols += ";" + item.symbols;
				}
			}
			else
			{
				if (existSymbols.Contains(item.symbols))
				{
					existSymbols = existSymbols.Replace(";" + item.symbols, "");
					if (existSymbols.Contains(item.symbols))
					{
						existSymbols = existSymbols.Replace(item.symbols, "");
					}
				}
			}
		}
		//string.Join(";", defineSymbols.ToArray())
		PlayerSettings.SetScriptingDefineSymbolsForGroup(curBuildTargetGroup, existSymbols);
	}

	[System.Serializable]
	public class SymbolInfo
	{
		public string symbols;
		public bool isEnable;
	}
}
