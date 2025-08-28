---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-16
--- Author: Hukiry
---

---@class FightMapPanelControl
local FightMapPanelControl = Class()

function FightMapPanelControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.pageListTF = self.transform:Find("pageList")
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("ScrollView/Viewport/Content").gameObject

end

---释放
function FightMapPanelControl:OnDestroy()
	for i, v in pairs(FightMapPanelControl) do
		if type(v) ~= "function" then
			FightMapPanelControl[i] = nil
		end
	end
end

return FightMapPanelControl