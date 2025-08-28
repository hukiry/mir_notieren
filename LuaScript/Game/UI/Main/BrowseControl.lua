---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-12-04
--- Author: Hukiry
---

---@class BrowseControl
local BrowseControl = Class()

function BrowseControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("bg1/ScrollView/Viewport/Content").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function BrowseControl:OnDestroy()
	for i, v in pairs(BrowseControl) do
		if type(v) ~= "function" then
			BrowseControl[i] = nil
		end
	end
end

return BrowseControl