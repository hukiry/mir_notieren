---
---
--- Create Time:2024-07-19
--- Author: Hukiry
---

---@class LifeHelpItem:IUIItem
local LifeHelpItem = Class(IUIItem)

function LifeHelpItem:ctor()
	---@type LifeHelpItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function LifeHelpItem:Start()
	self:AddClick(self.itemCtrl.okBtnGo, Handle(self, self.OnOKButton))
end

function LifeHelpItem:OnOKButton()
	if not self.info:IsMe() then
		--帮助后移除
		Single.Request().SendLife(self.info.Id,  ELifeState.Help)
	elseif self.info:IsFinish() then
		--获得后移除
		Single.Request().SendLife(self.info.Id, ELifeState.GetHelp)
	end
end

---更新数据
---@param info LifeInfo
function LifeHelpItem:OnEnable(info, backCall)
	self.info = info
	self.backCall = backCall
	if self.info:IsMe() and not self.info:IsFinish() then
		self.itemCtrl.okBtnGo:SetActive(false)
	else
		self.itemCtrl.okBtnGo:SetActive(true)
	end
	self.itemCtrl.okTxt = info:GetHelpText()
	self.itemCtrl.nick = info.nick
	self.itemCtrl.silder.fillAmount = info.count/5
	self.itemCtrl.silderText.text =  string.format("%s/%s", info.count, 5)
end

---隐藏窗口
function LifeHelpItem:OnDisable()

end

---消耗释放资源
function LifeHelpItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return LifeHelpItem