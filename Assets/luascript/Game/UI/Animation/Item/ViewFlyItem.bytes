---
--- 金币飞
--- Create Time:2022-12-15
--- Author: Hukiry
---

---@class ViewFlyItem:FlyItemBase
local ViewFlyItem = Class(FlyItemBase)

function ViewFlyItem:ctor()
	---@type ViewFlyItemControl
	self.itemCtrl = nil
end

function ViewFlyItem:Start(iconName, size)
	SetUIIcon(self.itemCtrl.icon, iconName, size)
	if size == nil then
		self.itemCtrl.icon:SetNativeSize()
	end
	self.finishCall = nil
end

function ViewFlyItem:PlayFinish()
	UIItemPool.Put(UIItemType.ViewFlyItem, self)
	---完成回调
	if self.finishCall then
		self.finishCall()
	end
end

function ViewFlyItem:OnDisable()
	self.transform:DOKill()
end

return ViewFlyItem