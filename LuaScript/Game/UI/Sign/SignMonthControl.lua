---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class SignMonthControl
local SignMonthControl = Class()

function SignMonthControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.dayListGo = self.transform:Find("bg/contentbg/dayList").gameObject
	---@type UnityEngine.GameObject
	self.signItemSevenGo = self.transform:Find("bg/contentbg/dayList/SignItemSeven").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.iconGo = self.transform:Find("bg/closeBtn/icon").gameObject
	---@type UnityEngine.GameObject
	self.getBtnGo = self.transform:Find("bg/getBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.signTxt = self.transform:Find("bg/getBtn/signTxt"):GetComponent("HukirySupperText")

end

---释放
function SignMonthControl:OnDestroy()
	for i, v in pairs(SignMonthControl) do
		if type(v) ~= "function" then
			SignMonthControl[i] = nil
		end
	end
end

return SignMonthControl