---
---
--- Create Time:2023-10-12
--- Author: Hukiry
---

---@class TargetEditorItem:IUIItem
local TargetEditorItem = Class(IUIItem)

function TargetEditorItem:ctor()
	---@type TargetEditorItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function TargetEditorItem:Start()
	
end

---更新数据
function TargetEditorItem:OnEnable(id, number)
	self.itemId = id
	local info = Single.Meta():GetMataConfig():GetMatchConfig():GetItemInfo(id)
	self.itemCtrl.num.text = number
	SetUIIcon(self.itemCtrl.icon, info.targetIcon)
end

---隐藏窗口
function TargetEditorItem:OnDisable()
	
end

---消耗释放资源
function TargetEditorItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return TargetEditorItem