---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class DrawcardItemControl
local DrawcardItemControl = Class()

function DrawcardItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.cardTF = self.transform:Find("card")
	---@type UnityEngine.Transform
	self.backTF = self.transform:Find("card/back")
	---@type UnityEngine.Transform
	self.frontTF = self.transform:Find("card/front")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("card/front/Icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.num = self.transform:Find("card/front/num"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.ui_book_openGo = self.transform:Find("ui_book_open").gameObject
	---@type UnityEngine.GameObject
	self.ui_RewardItemGo = self.transform:Find("ui_RewardItem").gameObject

end

---释放
function DrawcardItemControl:OnDestroy()
	for i, v in pairs(DrawcardItemControl) do
		if type(v) ~= "function" then
			DrawcardItemControl[i] = nil
		end
	end
end

return DrawcardItemControl