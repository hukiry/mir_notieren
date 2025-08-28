---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-17
--- Author: Hukiry
---

---@class MyMapCreateControl
local MyMapCreateControl = Class()

function MyMapCreateControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.createBtnGo = self.transform:Find("bg/createBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.createTxt = self.transform:Find("bg/createBtn/createTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.nameTxt = self.transform:Find("bg/nameTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.mapIdTxt = self.transform:Find("bg/mapIdTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.UI.InputField
	self.inputFieldName = self.transform:Find("bg/InputFieldName"):GetComponent("InputField")
	---@type UnityEngine.UI.InputField
	self.inputFieldDesc = self.transform:Find("bg/InputFieldDesc"):GetComponent("InputField")
	---@type Hukiry.UI.UIDropdown
	self.dropdownNormal = self.transform:Find("bg/DropdownNormal"):GetComponent("UIDropdown")
	---@type Hukiry.UI.UIDropdown
	self.dropdownObs = self.transform:Find("bg/DropdownObs"):GetComponent("UIDropdown")
	---@type UnityEngine.UI.InputField
	self.inputFieldMove = self.transform:Find("bg/InputFieldMove"):GetComponent("InputField")

end

---释放
function MyMapCreateControl:OnDestroy()
	for i, v in pairs(MyMapCreateControl) do
		if type(v) ~= "function" then
			MyMapCreateControl[i] = nil
		end
	end
end

return MyMapCreateControl