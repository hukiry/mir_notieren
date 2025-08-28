---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-17
--- Author: Hukiry
---

---@class RoleControl
local RoleControl = Class()

function RoleControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.nickName = self.transform:Find("bg/nameBg/nickName"):GetComponent("HukirySupperText")
	---@type UnityEngine.UI.InputField
	self.inputField = self.transform:Find("bg/nameBg/InputField"):GetComponent("InputField")
	---@type Hukiry.HukirySupperText
	self.tip = self.transform:Find("bg/nameBg/tip"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.goldGo = self.transform:Find("bg/nameBg/tip/gold").gameObject
	---@type UnityEngine.GameObject
	self.saveBtnGo = self.transform:Find("bg/saveBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject

end

---释放
function RoleControl:OnDestroy()
	for i, v in pairs(RoleControl) do
		if type(v) ~= "function" then
			RoleControl[i] = nil
		end
	end
end

return RoleControl