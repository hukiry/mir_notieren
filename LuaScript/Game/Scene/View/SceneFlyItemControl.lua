---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-08-05
--- Author: Hukiry
---

---@class SceneFlyItemControl
local SceneFlyItemControl = Class()

function SceneFlyItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.SpriteRenderer
	self.icon = self.transform:Find("icon"):GetComponent("SpriteRenderer")

end

---释放
function SceneFlyItemControl:OnDestroy()
	for i, v in pairs(SceneFlyItemControl) do
		if type(v) ~= "function" then
			SceneFlyItemControl[i] = nil
		end
	end
end

return SceneFlyItemControl