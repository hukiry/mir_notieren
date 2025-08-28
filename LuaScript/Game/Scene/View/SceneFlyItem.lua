---
---
--- Create Time:2023-05-09
--- Author: Hukiry
---

---@class SceneFlyItem:FlyItemBase
local SceneFlyItem = Class(FlyItemBase)

function SceneFlyItem:ctor()
	---@type SceneFlyItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function SceneFlyItem:Start(iconName, size)
	self.iconSprite = SetSceneIcon(self.itemCtrl.icon, iconName, size)
	self.finishCall = nil
end

function SceneFlyItem:PlayFinish()
	SceneItemPool.Put(SceneItemType.SceneFlyItem, self)
	if self.finishCall then
		self.finishCall()
	end
end

---隐藏窗口
function SceneFlyItem:OnDisable()
	self.transform:DOKill()
	self.iconSprite:OnDestroy()
end

---消耗释放资源
function SceneFlyItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return SceneFlyItem