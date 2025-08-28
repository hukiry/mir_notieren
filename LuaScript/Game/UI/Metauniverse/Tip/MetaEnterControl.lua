---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MetaEnterControl
local MetaEnterControl = Class()

function MetaEnterControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.descSelect = self.transform:Find("bg1/bg/descSelect"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.timeBgGo = self.transform:Find("bg1/bg/timeBg").gameObject
	---@type Hukiry.HukirySupperText
	self.actTime = self.transform:Find("bg1/bg/timeBg/actTime"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.startBtn = self.transform:Find("bg1/startBtn"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function MetaEnterControl:OnDestroy()
	for i, v in pairs(MetaEnterControl) do
		if type(v) ~= "function" then
			MetaEnterControl[i] = nil
		end
	end
end

return MetaEnterControl