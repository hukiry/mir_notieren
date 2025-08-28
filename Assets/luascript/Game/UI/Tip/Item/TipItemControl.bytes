---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-08-05
--- Author: Hukiry
---

---@class TipItemControl
local TipItemControl = Class()

function TipItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.backMoveGo = self.transform:Find("backMove").gameObject
	---@type UnityEngine.GameObject
	self.backGo = self.transform:Find("back").gameObject
	---@type Hukiry.HukirySupperText
	self.tip = self.transform:Find("tip"):GetComponent("HukirySupperText")

end

---释放
function TipItemControl:OnDestroy()
	for i, v in pairs(TipItemControl) do
		if type(v) ~= "function" then
			TipItemControl[i] = nil
		end
	end
end

return TipItemControl