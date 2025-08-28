---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class RechargeFirstControl
local RechargeFirstControl = Class()

function RechargeFirstControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type Hukiry.HukirySupperText
	self.coinNum = self.transform:Find("bg1/bgContent/coinNum"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.payBtnGo = self.transform:Find("bg1/payBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.payTxt = self.transform:Find("bg1/payBtn/payTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function RechargeFirstControl:OnDestroy()
	for i, v in pairs(RechargeFirstControl) do
		if type(v) ~= "function" then
			RechargeFirstControl[i] = nil
		end
	end
end

return RechargeFirstControl