---
---
--- Create Time:2022-7-3 12:00
--- Author: Hukiry
---

---@class RoleView:UIWindowBase
local RoleView = Class(UIWindowBase)

function RoleView:ctor()
	---@type RoleControl
	self.ViewCtrl = nil
end

---初始属性字段
function RoleView:Awake()
	self.prefabName = "Role"
	self.prefabDirName = "Setting"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function RoleView:Start(isSettingPanel)
	self.isSettingPanel = isSettingPanel or false
	if not self.isSettingPanel then
		self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	end

	self.ViewCtrl.inputField.onValueChanged:AddListener(Handle(self, self._OnInput))
	self:AddClick(self.ViewCtrl.saveBtnGo,Handle(self, self._CheckSave))
end

function RoleView:_CheckSave()
	if string.len(self.selectNick) >=3 then
		if Single.Player():IsEnough(EMoneyType.gold, self.costNum) then
			if self.isSettingPanel then
				UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10004), Handle(self, self._SaveNick))
			else
				self:_SaveNick()
			end
		else
			UIManager:OpenWindow(ViewID.Recharge)
		end
	else
		TipMessageBox.ShowUI( GetLanguageText(string.len(self.selectNick) <= 0 and 13010 or 13011))
	end
end

function RoleView:_SaveNick()
	Single.Player():SetMoneyNum(EMoneyType.nickCount, 1)
	Single.Player():SetMoneyNum(EMoneyType.gold, self.costNum, true)
	EventDispatch:Broadcast(ViewID.Game, 4)

	if self.selectNick ~= Single.Player().roleNick  then
		Single.Player().roleNick = self.selectNick
		Single.Request().SendBindLogout(EHttpLoginState.FixNick, nil, self.selectNick)
	end
	self.ViewCtrl.inputField.text = ""
	TipMessageBox.ShowUI( GetLanguageText(16016),true)
	self:Close()
end

function RoleView:_OnInput(value)
	self.selectNick = string.Trim(value)
end

---显示窗口:初次打开
function RoleView:OnEnable(...)
	self.selectNick = ''
	local buyNickCount = Single.Player():GetMoneyNum(EMoneyType.nickCount)
	self.costNum = buyNickCount < 1 and 0 or (math.floor(buyNickCount/5)*20 + 20)
	self.ViewCtrl.tip.text = (buyNickCount < 1 and GetLanguageText(13012) or GetLanguageText(13017))..self.costNum
	self.ViewCtrl.nickName.text = Single.Player().roleNick

	self.ViewCtrl.goldGo:SetActive(buyNickCount > 0)
end

---隐藏窗口
function RoleView:OnDisable()
	
end

---消耗释放资源
function RoleView:OnDestroy()
	
end

return RoleView