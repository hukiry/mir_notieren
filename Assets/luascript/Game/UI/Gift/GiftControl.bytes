---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class GiftControl
local GiftControl = Class()

function GiftControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.verticalGo = self.transform:Find("bg/vertical").gameObject

end

---释放
function GiftControl:OnDestroy()
	for i, v in pairs(GiftControl) do
		if type(v) ~= "function" then
			GiftControl[i] = nil
		end
	end
end

return GiftControl