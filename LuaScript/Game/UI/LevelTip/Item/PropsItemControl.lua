---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-29
--- Author: Hukiry
---

---@class PropsItemControl
local PropsItemControl = Class()

function PropsItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("Icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.itemNum = self.transform:Find("itemNum"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.gouGo = self.transform:Find("gou").gameObject

end

---释放
function PropsItemControl:OnDestroy()
	for i, v in pairs(PropsItemControl) do
		if type(v) ~= "function" then
			PropsItemControl[i] = nil
		end
	end
end

return PropsItemControl