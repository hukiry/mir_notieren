---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-04-02
--- Author: Hukiry
---

---@class PromptBoxControl
local PromptBoxControl = Class()

function PromptBoxControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.UI.Text
	self.content = self.transform:Find("back/content"):GetComponent("Text")
	---@type UnityEngine.GameObject
	self.okGo = self.transform:Find("horizontal/ok").gameObject
	---@type UnityEngine.UI.Text
	self.txtOk = self.transform:Find("horizontal/ok/txtOk"):GetComponent("Text")
	---@type UnityEngine.GameObject
	self.cancelGo = self.transform:Find("horizontal/cancel").gameObject
	---@type UnityEngine.UI.Text
	self.txtCancel = self.transform:Find("horizontal/cancel/txtCancel"):GetComponent("Text")

end

---释放
function PromptBoxControl:OnDestroy()
	for i, v in pairs(PromptBoxControl) do
		if type(v) ~= "function" then
			PromptBoxControl[i] = nil
		end
	end
end

return PromptBoxControl