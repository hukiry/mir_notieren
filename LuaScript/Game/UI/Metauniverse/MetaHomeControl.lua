---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-17
--- Author: Hukiry
---

---@class MetaHomeControl
local MetaHomeControl = Class()

function MetaHomeControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.pageListTF = self.transform:Find("pageList")
	---@type UnityEngine.GameObject
	self.panelGo = self.transform:Find("panel").gameObject

end

---释放
function MetaHomeControl:OnDestroy()
	for i, v in pairs(MetaHomeControl) do
		if type(v) ~= "function" then
			MetaHomeControl[i] = nil
		end
	end
end

return MetaHomeControl