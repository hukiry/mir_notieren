---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class FeedfaceControl
local FeedfaceControl = Class()

function FeedfaceControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.UI.InputField
	self.inputFieldDesc = self.transform:Find("bg/contentBg/InputFieldDesc"):GetComponent("InputField")
	---@type UnityEngine.UI.InputField
	self.inputFieldEmail = self.transform:Find("bg/nameBg/InputFieldEmail"):GetComponent("InputField")
	---@type UnityEngine.UI.Dropdown
	self.dropdown = self.transform:Find("bg/contentBg1/Dropdown"):GetComponent("Dropdown")
	---@type UnityEngine.GameObject
	self.sendBtnGo = self.transform:Find("bg/sendBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject

end

---释放
function FeedfaceControl:OnDestroy()
	for i, v in pairs(FeedfaceControl) do
		if type(v) ~= "function" then
			FeedfaceControl[i] = nil
		end
	end
end

return FeedfaceControl