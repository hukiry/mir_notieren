---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class PropsTipControl
local PropsTipControl = Class()

function PropsTipControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("bg1/Icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.buyNum = self.transform:Find("bg1/Icon/buyNum"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.desc = self.transform:Find("bg1/desc"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg1/titleBg/title"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.buyBtnGo = self.transform:Find("bg1/buyBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.price = self.transform:Find("bg1/buyBtn/price"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function PropsTipControl:OnDestroy()
	for i, v in pairs(PropsTipControl) do
		if type(v) ~= "function" then
			PropsTipControl[i] = nil
		end
	end
end

return PropsTipControl