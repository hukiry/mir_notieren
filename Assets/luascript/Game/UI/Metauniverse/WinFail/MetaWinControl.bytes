---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-20
--- Author: Hukiry
---

---@class MetaWinControl
local MetaWinControl = Class()

function MetaWinControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("slider/icon/title"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.desc = self.transform:Find("desc"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.contineGo = self.transform:Find("contine").gameObject

end

---释放
function MetaWinControl:OnDestroy()
	for i, v in pairs(MetaWinControl) do
		if type(v) ~= "function" then
			MetaWinControl[i] = nil
		end
	end
end

return MetaWinControl