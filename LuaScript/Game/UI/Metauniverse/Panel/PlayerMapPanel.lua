---
---
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class PlayerMapPanel:DisplayObjectBase
local PlayerMapPanel = Class(DisplayObjectBase)

function PlayerMapPanel:ctor()
	---@type PlayerMapPanelControl
	self.panelCtrl = nil
end

---初始界面:注册按钮事件等
function PlayerMapPanel:Start()

end

---显示窗口:初次打开
function PlayerMapPanel:OnEnable()
	local roleNick = Single.Player().roleNick == "" and GetLanguageText(16127) or Single.Player().roleNick
	local tempTab = {
		roleNick
		, Single.Player().roleId
		, Single.Player():GetMoneyNum(EMoneyType.gold)
		, Single.Player():GetMoneyNum(EMoneyType.level)
		, Single.Player():GetMoneyNum(EMoneyType.metaExpendNum)
		, GetLanguageText(16128)
		, '0'
		, '0'
		, Util.Time().GetDateFormat(Single.Player().loginTime)
	}

	local childCount = self.panelCtrl.propertyListTF.childCount
	for i = 1, childCount do
		local tf = self.panelCtrl.propertyListTF:GetChild(i-1)
		tf:GetComponent("HukirySupperText").text = GetLanguageText(16400+i)
		local super = tf:FindHukirySupperText("txt")
		super.text = ": " .. tempTab[i]
	end

end

---隐藏窗口
function PlayerMapPanel:OnDisable()
	
end

---消耗释放资源
function PlayerMapPanel:OnDestroy()
	self.panelCtrl:OnDestroy()
end

return PlayerMapPanel