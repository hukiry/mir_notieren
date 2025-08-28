---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-17
--- Author: Hukiry
---

---@class SettingMapPanelControl
local SettingMapPanelControl = Class()

function SettingMapPanelControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.musicEffectTF = self.transform:Find("bgMusic/musicEffect")
	---@type UnityEngine.Transform
	self.musicBackTF = self.transform:Find("bgMusic/musicBack")

end

---释放
function SettingMapPanelControl:OnDestroy()
	for i, v in pairs(SettingMapPanelControl) do
		if type(v) ~= "function" then
			SettingMapPanelControl[i] = nil
		end
	end
end

return SettingMapPanelControl