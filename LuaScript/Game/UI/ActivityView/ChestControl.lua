---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class ChestControl
local ChestControl = Class()

function ChestControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.remainTime = self.transform:Find("bg1/timeBg/remainTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.verticalLayGo = self.transform:Find("bg1/verticalLay").gameObject

end

---释放
function ChestControl:OnDestroy()
	for i, v in pairs(ChestControl) do
		if type(v) ~= "function" then
			ChestControl[i] = nil
		end
	end
end

return ChestControl