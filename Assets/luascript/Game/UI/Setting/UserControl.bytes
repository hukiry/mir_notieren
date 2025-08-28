---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-16
--- Author: Hukiry
---

---@class UserControl
local UserControl = Class()

function UserControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.UI.Text
	self.title = self.transform:Find("contentBg/title"):GetComponent("Text")
	---@type UnityEngine.GameObject
	self.buttonGo = self.transform:Find("contentBg/button").gameObject
	---@type UnityEngine.GameObject
	self.contentDescGo = self.transform:Find("contentBg/list/contentDesc").gameObject
	---@type UnityEngine.GameObject
	self.content1Go = self.transform:Find("contentBg/list/content1").gameObject
	---@type UnityEngine.GameObject
	self.content2Go = self.transform:Find("contentBg/list/content2").gameObject

end

---释放
function UserControl:OnDestroy()
	for i, v in pairs(UserControl) do
		if type(v) ~= "function" then
			UserControl[i] = nil
		end
	end
end

return UserControl