---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class HeadItemControl
local HeadItemControl = Class()

function HeadItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")

end

---释放
function HeadItemControl:OnDestroy()
	for i, v in pairs(HeadItemControl) do
		if type(v) ~= "function" then
			HeadItemControl[i] = nil
		end
	end
end

return HeadItemControl