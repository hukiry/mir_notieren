---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-26
--- Author: Hukiry
---

---@class ShopCoinItemControl
local ShopCoinItemControl = Class()

function ShopCoinItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.payBtnGo = self.transform:Find("bg/PayBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.payTxt = self.transform:Find("bg/PayBtn/payTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.num = self.transform:Find("bg/num"):GetComponent("HukirySupperText")

end

---释放
function ShopCoinItemControl:OnDestroy()
	for i, v in pairs(ShopCoinItemControl) do
		if type(v) ~= "function" then
			ShopCoinItemControl[i] = nil
		end
	end
end

return ShopCoinItemControl