---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class MailItemControl
local MailItemControl = Class()

function MailItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.getBtnGo = self.transform:Find("GetBtn").gameObject
	---@type UnityEngine.GameObject
	self.readBtnGo = self.transform:Find("readBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("title"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.desc = self.transform:Find("desc"):GetComponent("HukirySupperText")

end

---释放
function MailItemControl:OnDestroy()
	for i, v in pairs(MailItemControl) do
		if type(v) ~= "function" then
			MailItemControl[i] = nil
		end
	end
end

return MailItemControl