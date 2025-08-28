---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-05-06
--- Author: Hukiry
---

---@class BuyTipControl
local BuyTipControl = Class()

function BuyTipControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.UI.RawImage
	self.circle = self.transform:Find("circle"):GetComponent("RawImage")
	---@type Hukiry.HukirySupperText
	self.waitTxt = self.transform:Find("waitTxt"):GetComponent("HukirySupperText")

end

---释放
function BuyTipControl:OnDestroy()
	for i, v in pairs(BuyTipControl) do
		if type(v) ~= "function" then
			BuyTipControl[i] = nil
		end
	end
end

return BuyTipControl