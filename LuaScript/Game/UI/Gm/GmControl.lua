---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-06-24
--- Author: Hukiry
---

---@class GmControl
local GmControl = Class()

function GmControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.UI.InputField
	self.inputFieldX = self.transform:Find("InputFieldX"):GetComponent("InputField")
	---@type UnityEngine.UI.InputField
	self.inputFieldY = self.transform:Find("InputFieldY"):GetComponent("InputField")
	---@type UnityEngine.UI.InputField
	self.inputField = self.transform:Find("InputField"):GetComponent("InputField")
	---@type UnityEngine.GameObject
	self.startBtnGo = self.transform:Find("StartBtn").gameObject
	---@type UnityEngine.GameObject
	self.winBtnGo = self.transform:Find("winBtn").gameObject

end

---释放
function GmControl:OnDestroy()
	for i, v in pairs(GmControl) do
		if type(v) ~= "function" then
			GmControl[i] = nil
		end
	end
end

return GmControl