---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-03-16
--- Author: Hukiry
---

---@class GuideControl
local GuideControl = Class()

function GuideControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.bgGo = self.transform:Find("bg").gameObject
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg/bgTitle/title"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.content = self.transform:Find("bg/content"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.skipBtnGo = self.transform:Find("bg/skipBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.itemIcon = self.transform:Find("bg/itemIcon"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.shaderMashGo = self.transform:Find("shaderMash").gameObject
	---@type Hukiry.HukirySupperText
	self.contentMask = self.transform:Find("shaderMash/contentMask"):GetComponent("HukirySupperText")
	---@type UnityEngine.Transform
	self.arrowTF = self.transform:Find("arrow")

end

---释放
function GuideControl:OnDestroy()
	for i, v in pairs(GuideControl) do
		if type(v) ~= "function" then
			GuideControl[i] = nil
		end
	end
end

return GuideControl