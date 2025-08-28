---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-07-22
--- Author: Hukiry
---

---@class LifeHelpItemControl
local LifeHelpItemControl = Class()

function LifeHelpItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.nick = self.transform:Find("bg/backN/nick"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.okBtnGo = self.transform:Find("bg/okBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.okTxt = self.transform:Find("bg/okBtn/okTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.Transform
	self.lifeBarTF = self.transform:Find("bg/lifeBar")
	---@type Hukiry.UI.UIProgressbarMask
	self.silder = self.transform:Find("bg/lifeBar/Silder"):GetComponent("UIProgressbarMask")
	---@type Hukiry.HukirySupperText
	self.silderText = self.transform:Find("bg/lifeBar/silderText"):GetComponent("HukirySupperText")

end

---释放
function LifeHelpItemControl:OnDestroy()
	for i, v in pairs(LifeHelpItemControl) do
		if type(v) ~= "function" then
			LifeHelpItemControl[i] = nil
		end
	end
end

return LifeHelpItemControl