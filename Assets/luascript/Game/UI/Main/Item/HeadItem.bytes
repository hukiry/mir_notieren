---
---
--- Create Time:2023-07-19
--- Author: Hukiry
---

---@class HeadItem:IUIItem
local HeadItem = Class(IUIItem)

function HeadItem:ctor()
	---@type HeadItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function HeadItem:Start(callback)
	self.callback = callback
	self:AddClick(self.itemCtrl.gameObject, function()
		UIManager:CloseWindow(ViewID.Browse)
		self.callback(self.id)
	end)
end

---更新数据
function HeadItem:OnEnable(id)
	self.id = id
	self.itemCtrl.icon.spriteName = "role_"..id
	StartCoroutine(function()
		UtilFunction.SetUIAdaptionSize(self.itemCtrl.icon, Vector2.New(176,176))
	end)
end

---隐藏窗口
function HeadItem:OnDisable()
	
end

---消耗释放资源
function HeadItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return HeadItem