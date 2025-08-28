---
---
--- Create Time:2022-7-3 12:00
--- Author: Hukiry
---

---@class SettingView:UIWindowBase
local SettingView = Class(UIWindowBase)

function SettingView:ctor()
	---@type SettingControl
	self.ViewCtrl = nil
end

---初始属性字段
function SettingView:Awake()
	self.prefabName = "Setting"
	self.prefabDirName = "Setting"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function SettingView:Start()
	---登录窗口
	---@type AccountPanel
	self.accountPanel = require("Game.UI.Setting.AccountPanel").New()--self.ViewCtrl.loginWinGo
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))

	self:AddClick(self.ViewCtrl.appleBtnGo, Handle(self.accountPanel, self.accountPanel.OnLogin, 1),true)

	self:AddClick(self.ViewCtrl.languageBtnGo, function()
		local code = Single.SdkPlatform():GetLanguageCode()
		local info = SingleConfig.MultiLanguage():GetInfoByCode(code)
		UIManager:OpenWindow(ViewID.Lanage, info.lanId)
	end)

	self:AddClick(self.ViewCtrl.editorBtnGo, function()
		UIManager:OpenWindow(ViewID.Role)
	end)

	self:AddClick(self.ViewCtrl.privateBtnGo, function()
		UIManager:OpenWindow(ViewID.User)
	end)

	self:AddClick(self.ViewCtrl.askBtnGo, function()
		UIManager:OpenWindow(ViewID.Feedface)
	end)

	self:AddClick(self.ViewCtrl.rateStarGo, function()
		Application.OpenURL("itms-apps://itunes.apple.com/app/id6449088011?action=write-review")
	end)

	self:AddClick(self.ViewCtrl.headBtnGo, Handle(self,self.SeletHead))

	self:AddClick(self.ViewCtrl.logoutBtnGo, Handle(self,self.Logout))

	local tabIndex = {EGameSetting.MusicMute, EGameSetting.SoundMute}
	---@type table<number, UnityEngine.UI.Toggle>
	self.toggleList = {}
	local childCount = self.ViewCtrl.toggleListTF.childCount
	for i = 1, childCount do
		local go = self.ViewCtrl.toggleListTF:GetChild(i-1).gameObject
		---@type UnityEngine.UI.Toggle
		local togBtn = go:GetComponent("Toggle")
		local index = tabIndex[i]
		self.toggleList[index] = togBtn

		local isOn = Single.PlayerPrefs():GetBool(index, false)
		self.toggleList[index].isOn = isOn
		togBtn.onValueChanged:AddListener(Handle(self, self.OnToggle, i))
	end

	SetUIIcon(self.ViewCtrl.headIcon, "role_"..Single.Player().headId, Vector2.New(120,120))

end

function SettingView:SeletHead()
	---选择头像
	UIManager:OpenWindow(ViewID.Browse, function(id)
		Single.Player().headId = id
		Single.Request().SendHeadId(id)
		SetUIIcon(self.ViewCtrl.headIcon, "role_"..id, Vector2.New(120,120))
	end)
end

function SettingView:Logout()
	UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10008), function()
		if not Single.Http():IsHaveNetwork() then
			TipMessageBox.ShowUI( GetLanguageText(10002))
			return
		end

		UIManager:OpenWindow(ViewID.ServerTip)
		Single.Request().SendBindLogout(EHttpLoginState.Logout, nil,nil,function(isSucc)
			UIManager:CloseWindow(ViewID.ServerTip)
			if not isSucc then
				TipMessageBox.ShowUI( GetLanguageText(10001))
			end
		end)
	end)
end

---@param isOn boolean
function SettingView:OnToggle(index, isOn)
	if index == 1 then
		Single.Sound():SetMusicMute(isOn)
	elseif index == 2 then
		Single.Sound():SetSoundMute(isOn)
	end
end

---事件派发
function SettingView:OnDispatch()
	self:OnEnable()
end

---显示窗口:初次打开
function SettingView:OnEnable(...)
	local code = Single.SdkPlatform():GetLanguageCode()
	local info = SingleConfig.MultiLanguage():GetInfoByCode(code)
	self.ViewCtrl.lanTxt.text = info.name

	self.ViewCtrl.roleTxt.text = "ID:"..Single.Player().roleId
	self.ViewCtrl.version.text = "v"..SingleData.Login():GetGameVersion().version

	local isOn = Single.PlayerPrefs():GetBool(EGameSetting.MusicMute, false)
	self.toggleList[EGameSetting.MusicMute].isOn = isOn
	isOn = Single.PlayerPrefs():GetBool(EGameSetting.SoundMute, false)
	self.toggleList[EGameSetting.SoundMute].isOn = isOn

	if Single.SdkPlatform():IsShowAppleLogin() then
		self.ViewCtrl.appleBtnGo:SetActive(not Single.Player().isBindLogin)
		self.ViewCtrl.logoutBtnGo:SetActive(Single.Player().isBindLogin )
	else
		self.ViewCtrl.appleBtnGo:SetActive(false)
	end
end

---改变语言
function SettingView:OnChangeLanguage()
	self:OnEnable()
end

---隐藏窗口
function SettingView:OnDisable()
	
end

---消耗释放资源
function SettingView:OnDestroy()
	
end

return SettingView