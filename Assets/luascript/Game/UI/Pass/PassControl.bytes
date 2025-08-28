---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-01
--- Author: Hukiry
---

---@class PassControl
local PassControl = Class()

function PassControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("GameObject/ScrollView/Viewport/Content").gameObject
	---@type Hukiry.HukirySupperText
	self.passNum = self.transform:Find("bg/passBG/passNum"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.maxTxt = self.transform:Find("bg/passBG/maxBg/maxTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.activateBtnGo = self.transform:Find("bg/activateBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.passTime = self.transform:Find("bg/passTimebg/passTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.iconGo = self.transform:Find("closeBtn/icon").gameObject

end

---释放
function PassControl:OnDestroy()
	for i, v in pairs(PassControl) do
		if type(v) ~= "function" then
			PassControl[i] = nil
		end
	end
end

return PassControl