---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2022-12-19
--- Author: Hukiry
---

---@class ViewFlyItemControl
local ViewFlyItemControl = Class()

function ViewFlyItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")

end

return ViewFlyItemControl