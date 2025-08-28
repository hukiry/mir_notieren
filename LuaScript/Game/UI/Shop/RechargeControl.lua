---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-26
--- Author: Hukiry
---

---@class RechargeControl
local RechargeControl = Class()

function RechargeControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("ScrollView/Viewport/Content").gameObject
	---@type UnityEngine.Transform
	self.topTF = self.transform:Find("top")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("closeBtn").gameObject

end

---释放
function RechargeControl:OnDestroy()
	for i, v in pairs(RechargeControl) do
		if type(v) ~= "function" then
			RechargeControl[i] = nil
		end
	end
end

return RechargeControl