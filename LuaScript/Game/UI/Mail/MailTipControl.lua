---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class MailTipControl
local MailTipControl = Class()

function MailTipControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type Hukiry.HukirySupperText
	self.content = self.transform:Find("bg1/content"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg1/titleBg/title"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.okBtnGo = self.transform:Find("bg1/list/okBtn").gameObject

end

---释放
function MailTipControl:OnDestroy()
	for i, v in pairs(MailTipControl) do
		if type(v) ~= "function" then
			MailTipControl[i] = nil
		end
	end
end

return MailTipControl