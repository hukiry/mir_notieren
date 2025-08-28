---
---
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class QuitMapPanel:DisplayObjectBase
local QuitMapPanel = Class(DisplayObjectBase)

function QuitMapPanel:ctor()
	---@type QuitMapPanelControl
	self.panelCtrl = nil
end

---初始界面:注册按钮事件等
function QuitMapPanel:Start()
	self:AddClick(self.panelCtrl.quitBtnGo ,function()
		SceneApplication.ChangeState(ViewScene)
	end)

	---登录窗口
	---@type AccountPanel
	self.accountPanel = require("Game.UI.Setting.AccountPanel").New()--self.panelCtrl.loginWinGo
	self:AddClick(self.panelCtrl.appleBtnGo, Handle(self, self.OnLogin), true)
end

function QuitMapPanel:OnLogin()
	self.accountPanel:OnEnable(function()
		self.panelCtrl.appTxt.text = GetLanguageText(Single.Player().isBindLogin == true and 13205 or 13204)
	end)

	self.accountPanel:OnLogin(1)
end

---显示窗口:初次打开
function QuitMapPanel:OnEnable(...)
	if Single.SdkPlatform():IsShowAppleLogin() then
		self.panelCtrl.appleBtnGo:SetActive(not Single.Player().isBindLogin )
		self.panelCtrl.appTxt.text = GetLanguageText(Single.Player().isBindLogin == true and 13205 or 13204)
	else
		self.panelCtrl.appleBtnGo:SetActive(false)
	end
end

---隐藏窗口
function QuitMapPanel:OnDisable()
end

---消耗释放资源
function QuitMapPanel:OnDestroy()
	self.panelCtrl:OnDestroy()
end

return QuitMapPanel