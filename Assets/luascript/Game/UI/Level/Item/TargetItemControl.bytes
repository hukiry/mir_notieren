---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-06-24
--- Author: Hukiry
---

---@class TargetItemControl
local TargetItemControl = Class()

function TargetItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.gouGo = self.transform:Find("gou").gameObject
	---@type Hukiry.HukirySupperText
	self.num = self.transform:Find("num"):GetComponent("HukirySupperText")

end

---释放
function TargetItemControl:OnDestroy()
	for i, v in pairs(TargetItemControl) do
		if type(v) ~= "function" then
			TargetItemControl[i] = nil
		end
	end
end

return TargetItemControl