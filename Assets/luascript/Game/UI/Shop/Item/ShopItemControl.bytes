---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-26
--- Author: Hukiry
---

---@class ShopItemControl
local ShopItemControl = Class()

function ShopItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.bg = self.transform:Find("bg"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg/title"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.coinNum = self.transform:Find("coinNum"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.bgUp = self.transform:Find("bgUp"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.bgDown = self.transform:Find("bgDown"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.tagbgGo = self.transform:Find("tagbg").gameObject
	---@type Hukiry.HukirySupperText
	self.tagTxt = self.transform:Find("tagbg/tagTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.payBtnGo = self.transform:Find("PayBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.payTxt = self.transform:Find("PayBtn/payTxt"):GetComponent("HukirySupperText")

end

---释放
function ShopItemControl:OnDestroy()
	for i, v in pairs(ShopItemControl) do
		if type(v) ~= "function" then
			ShopItemControl[i] = nil
		end
	end
end

return ShopItemControl