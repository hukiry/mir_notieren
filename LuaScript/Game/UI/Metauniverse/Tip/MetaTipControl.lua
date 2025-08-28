---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-11-29
--- Author: Hukiry
---

---@class MetaTipControl
local MetaTipControl = Class()

function MetaTipControl:ctor(gameObject)
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
	---@type UnityEngine.GameObject
	self.cancelBtnGo = self.transform:Find("bg1/list/cancelBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function MetaTipControl:OnDestroy()
	for i, v in pairs(MetaTipControl) do
		if type(v) ~= "function" then
			MetaTipControl[i] = nil
		end
	end
end

return MetaTipControl