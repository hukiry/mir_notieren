---
---
--- Create Time:2023-07-16
--- Author: Hukiry
---

---@class LanButtonItem:IUIItem
local LanButtonItem = Class(IUIItem)

function LanButtonItem:ctor()
	---@type LanButtonItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
---@param backCall function<TableMultiLanguageItem>
function LanButtonItem:Start(backCall)
	self.backCall = backCall
	self.img = self.gameObject:GetComponent("AtlasImage")
	self:AddClick(self.gameObject, function()
		self.backCall(self.info)
	end)
end

---更新数据
function LanButtonItem:OnEnable(info, index)
	self.numberId = index
	self.info = info
	self.itemCtrl.txt.text = info.name
end

---切换语言
function LanButtonItem:Select(spriteName, col)
	self.img.spriteName = spriteName
	self.itemCtrl.txt.color = col
end

---隐藏窗口
function LanButtonItem:OnDisable()
	
end

---消耗释放资源
function LanButtonItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return LanButtonItem