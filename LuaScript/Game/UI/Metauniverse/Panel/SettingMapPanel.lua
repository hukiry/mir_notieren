---
---
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class SettingMapPanel:DisplayObjectBase
local SettingMapPanel = Class(DisplayObjectBase)

function SettingMapPanel:ctor()
	---@type SettingMapPanelControl
	self.panelCtrl = nil
end

---初始界面:注册按钮事件等
function SettingMapPanel:Start()
	---@type ToggleView
	self.toggleMusic =  require("Library.UIWidget.Toggle.ToggleView").New()
	---@type ToggleView
	self.toggleMusicEffect =  require("Library.UIWidget.Toggle.ToggleView").New()

	---@type RoleView
	self.roleView = require("Game.UI.Setting.RoleView").New(self.gameObject)
	self.roleView.ViewCtrl = require("Game.UI.Setting.RoleControl").New(self.gameObject)
	self.roleView:Start(true)
end

---显示窗口:初次打开
function SettingMapPanel:OnEnable(...)
	local MusicMute = Single.PlayerPrefs():GetBool(EGameSetting.MusicMute, false)
	local SoundMute = Single.PlayerPrefs():GetBool(EGameSetting.SoundMute, false)
	self.toggleMusic:OnEnable(self.panelCtrl.musicBackTF, Handle(self, self.ChangeValue, true), MusicMute)
	self.toggleMusicEffect:OnEnable(self.panelCtrl.musicEffectTF, Handle(self, self.ChangeValue, false),SoundMute)
	self.roleView:OnEnable()
end

function SettingMapPanel:ChangeValue(isMusic, value)
	if isMusic then
		Single.Sound():SetMusicMute(value)
	else
		Single.Sound():SetSoundMute(value)
	end
end

---隐藏窗口
function SettingMapPanel:OnDisable()
	
end

---消耗释放资源
function SettingMapPanel:OnDestroy()
	self.panelCtrl:OnDestroy()
end

return SettingMapPanel