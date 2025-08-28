---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

---@class SignItem:DisplayObjectBase
local SignItem = Class(DisplayObjectBase)

function SignItem:ctor()
	---@type SignItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function SignItem:Start(index)
	self.index = index
end

---更新数据
---@param info SignInfo
function SignItem:OnEnable(info)
	self.info = info
	local iconName = SingleConfig.Currency():GetKey(info:GetRewardType()).icon
	SetUIIcon(self.itemCtrl.icon, iconName)
	self.itemCtrl.dayTxt.text = info:GetName()
	self.itemCtrl.bgnameGo:SetActive(SingleData.Activity():GetSign().curDay <= info.day)
	self.itemCtrl.gouGo:SetActive(info.state == 2)
	self.itemCtrl.effect_ui_rewardGo:SetActive(info.state == 1)
	self.itemCtrl.numtx.text = 'x'..info:GetCfg().rewardNum
end

---隐藏窗口
function SignItem:OnDisable()
	
end

---消耗释放资源
function SignItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return SignItem