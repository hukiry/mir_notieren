---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-12
--- Author: Hukiry
---

---@class TargetEditorItemControl
local TargetEditorItemControl = Class()

function TargetEditorItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.num = self.transform:Find("num"):GetComponent("HukirySupperText")

end

---释放
function TargetEditorItemControl:OnDestroy()
	for i, v in pairs(TargetEditorItemControl) do
		if type(v) ~= "function" then
			TargetEditorItemControl[i] = nil
		end
	end
end

return TargetEditorItemControl