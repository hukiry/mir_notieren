---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class ChestItemControl
local ChestItemControl = Class()

function ChestItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.horizontalTF = self.transform:Find("horizontal")
	---@type UnityEngine.GameObject
	self.getBtnGo = self.transform:Find("getBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.coinNum = self.transform:Find("getBtn/coinNum"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.lockGo = self.transform:Find("getBtn/lock").gameObject

end

---释放
function ChestItemControl:OnDestroy()
	for i, v in pairs(ChestItemControl) do
		if type(v) ~= "function" then
			ChestItemControl[i] = nil
		end
	end
end

return ChestItemControl