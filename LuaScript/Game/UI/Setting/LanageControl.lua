---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class LanageControl
local LanageControl = Class()

function LanageControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.UI.ScrollRect
	self.scrollView = self.transform:Find("bg/ScrollView"):GetComponent("ScrollRect")
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("bg/ScrollView/Viewport/Content").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.okBtnGo = self.transform:Find("bg/OkBtn").gameObject

end

---释放
function LanageControl:OnDestroy()
	for i, v in pairs(LanageControl) do
		if type(v) ~= "function" then
			LanageControl[i] = nil
		end
	end
end

return LanageControl