---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-16
--- Author: Hukiry
---

---@class LanButtonItemControl
local LanButtonItemControl = Class()

function LanButtonItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("txt"):GetComponent("HukirySupperText")

end

---释放
function LanButtonItemControl:OnDestroy()
	for i, v in pairs(LanButtonItemControl) do
		if type(v) ~= "function" then
			LanButtonItemControl[i] = nil
		end
	end
end

return LanButtonItemControl