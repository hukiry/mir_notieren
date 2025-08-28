---
---
--- Create Time:2023-07-16
--- Author: Hukiry
---

---@class FeedfaceView:UIWindowBase
local FeedfaceView = Class(UIWindowBase)

function FeedfaceView:ctor()
	---@type FeedfaceControl
	self.ViewCtrl = nil
end

---初始属性字段
function FeedfaceView:Awake()
	self.prefabName = "Feedface"
	self.prefabDirName = "Setting"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

end

---初始界面:注册按钮事件等
function FeedfaceView:Start()
	self:AddClick(self.ViewCtrl.maskGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))

	self.ViewCtrl.inputFieldDesc.onValueChanged:AddListener(Handle(self, self.OnInputContent))
	self.ViewCtrl.inputFieldEmail.onValueChanged:AddListener(Handle(self, self.OnInputEmail))
	self.content = ''
	self.email = ''

	self:AddClick(self.ViewCtrl.sendBtnGo, Handle(self, self.SendMessage))

	---@type DropdownView
	self.dropdownView = require("Library.UIWidget.Dropdown.DropdownView").New()
end

function FeedfaceView:OnInputEmail(value)
	self.email = string.Trim(value)

end

function FeedfaceView:OnInputContent(value)
	self.content = string.Trim(value)
end

function FeedfaceView:SendMessage()
	if string.len(self.content) == 0 then
		TipMessageBox.ShowUI( GetLanguageText(13106))
		return
	end

	if not self:isRightEmail(self.email) then
		TipMessageBox.ShowUI(GetLanguageText(13105))
		return
	end
	Single.Request().SendCommonFeedface(self.gameType, self.content, self.email, function(isok)
		if isok then
			self:Close()
		end
	end)
end

function FeedfaceView:OnValueChanged(index)
	self.gameType = index + 1
end

---显示窗口:初次打开
function FeedfaceView:OnEnable(...)
	local tab = {}
	for i = 1, 4 do
		table.insert(tab, {text = GetLanguageText(13100+i)})
	end
	self.dropdownView:OnEnable(self.ViewCtrl.dropdown, tab, Handle(self, self.OnValueChanged))
	self.gameType = 1
end

function FeedfaceView:isRightEmail(str)
	if string.len(str or "") < 6 then return false end
	local b,e = string.find(str or "", '@')
	local bstr = ""
	local estr = ""
	if b then
		bstr = string.sub(str, 1, b-1)
		estr = string.sub(str, e+1, -1)
	else
		return false
	end

	-- check the string before '@'
	local p1,p2 = string.find(bstr, "[%w_]+")
	if (p1 ~= 1) or (p2 ~= string.len(bstr)) then return false end

	-- check the string after '@'
	if string.find(estr, "^[%.]+") then return false end
	if string.find(estr, "%.[%.]+") then return false end
	if string.find(estr, "@") then return false end
	if string.find(estr, "[%.]+$") then return false end

	local _,count = string.gsub(estr, "%.", "")
	if (count < 1 ) or (count > 3) then
		return false
	end

	return true
end

---隐藏窗口
function FeedfaceView:OnDisable()
	
end

---消耗释放资源
function FeedfaceView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return FeedfaceView