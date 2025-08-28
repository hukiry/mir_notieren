---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-06-24
--- Author: Hukiry
---

---@class GmWinControl
local GmWinControl = Class()

function GmWinControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("Panel/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.templateBtnGo = self.transform:Find("templateBtn").gameObject
	---@type UnityEngine.Transform
	self.contentTF = self.transform:Find("ScrollView/Viewport/Content")

end

---释放
function GmWinControl:OnDestroy()
	for i, v in pairs(GmWinControl) do
		if type(v) ~= "function" then
			GmWinControl[i] = nil
		end
	end
end

return GmWinControl