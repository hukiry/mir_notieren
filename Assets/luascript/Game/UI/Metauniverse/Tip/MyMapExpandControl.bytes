---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MyMapExpandControl
local MyMapExpandControl = Class()

function MyMapExpandControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.buyBtnGo = self.transform:Find("bg/BuyBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.payTxt = self.transform:Find("bg/BuyBtn/payTxt"):GetComponent("HukirySupperText")

end

---释放
function MyMapExpandControl:OnDestroy()
	for i, v in pairs(MyMapExpandControl) do
		if type(v) ~= "function" then
			MyMapExpandControl[i] = nil
		end
	end
end

return MyMapExpandControl