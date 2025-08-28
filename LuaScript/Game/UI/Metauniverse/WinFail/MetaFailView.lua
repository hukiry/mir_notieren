---
---
--- Create Time:2023-10-18
--- Author: Hukiry
---

---@class MetaFailView:UIWindowBase
local MetaFailView = Class(UIWindowBase)

function MetaFailView:ctor()
	---@type MetaFailControl
	self.ViewCtrl = nil
end

---初始属性字段
function MetaFailView:Awake()
	self.prefabName = "MetaFail"
	self.prefabDirName = "Metauniverse/WinFail"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MetaFailView:Start()
	self:AddClick(self.ViewCtrl.backBtnGo, Handle(self, self.OnContine))
	self:AddClick(self.ViewCtrl.startBtnGo, Handle(self, self.OnPaly))
end

function MetaFailView:OnContine()
	--todo 到元宇宙的家园
	if self.isFight then
		SceneApplication.ChangeState(HomeScene)
	else
		SceneApplication.ChangeState(MetaScene)
	end
end

function MetaFailView:OnPaly()
	SingleData.Metauniverse():StartTest(self.isFight and EMetaFightState.Fight or EMetaFightState.Test)
	SceneApplication.ChangeState(FightScene)
end

---显示窗口:初次打开
function MetaFailView:OnEnable(isFight)
	self.isFight = isFight
	local info = Single.Meta():GetMapInfo()
	self.ViewCtrl.mapName.text = info.mapName .."<b>[" ..GetLanguageText(isFight and 16126 or 16219).."]</b>"
	self.ViewCtrl.mapTime.text = GetLanguageText(16221) .."\n".. Util.Time().GetTimeStringBySecond(SingleData.Metauniverse().playTime)
end

---隐藏窗口
function MetaFailView:OnDisable()
	
end

---消耗释放资源
function MetaFailView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaFailView