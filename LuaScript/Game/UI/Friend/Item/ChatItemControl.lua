---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-07-18
--- Author: Hukiry
---

---@class ChatItemControl
local ChatItemControl = Class()

function ChatItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.time = self.transform:Find("time"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.leftGo = self.transform:Find("left").gameObject
	---@type UnityEngine.GameObject
	self.rightGo = self.transform:Find("right").gameObject

end

---释放
function ChatItemControl:OnDestroy()
	for i, v in pairs(ChatItemControl) do
		if type(v) ~= "function" then
			ChatItemControl[i] = nil
		end
	end
end

return ChatItemControl