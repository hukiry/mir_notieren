---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class GiftItemControl
local GiftItemControl = Class()

function GiftItemControl:ctor(gameObject)
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
	---@type Hukiry.UI.AtlasImage
	self.bgUp = self.transform:Find("bgUp"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.bgDown = self.transform:Find("bgDown"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.left = self.transform:Find("left"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.right = self.transform:Find("right"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.remianTime = self.transform:Find("timeBg/remianTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.payBtnGo = self.transform:Find("PayBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.payTxt = self.transform:Find("PayBtn/payTxt"):GetComponent("HukirySupperText")

end

---释放
function GiftItemControl:OnDestroy()
	for i, v in pairs(GiftItemControl) do
		if type(v) ~= "function" then
			GiftItemControl[i] = nil
		end
	end
end

return GiftItemControl