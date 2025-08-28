---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-12
--- Author: Hukiry
---

---@class SelectCardItemControl
local SelectCardItemControl = Class()

function SelectCardItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.name = self.transform:Find("name"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.tagGo = self.transform:Find("tag").gameObject
	---@type Hukiry.HukirySupperText
	self.tagTxt = self.transform:Find("tag/tagTxt"):GetComponent("HukirySupperText")

end

---释放
function SelectCardItemControl:OnDestroy()
	for i, v in pairs(SelectCardItemControl) do
		if type(v) ~= "function" then
			SelectCardItemControl[i] = nil
		end
	end
end

return SelectCardItemControl