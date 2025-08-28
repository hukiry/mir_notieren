---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-09-04
--- Author: Hukiry
---

---@class ServerTipControl
local ServerTipControl = Class()

function ServerTipControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.UI.RawImage
	self.circle = self.transform:Find("circle"):GetComponent("RawImage")
	---@type UnityEngine.GameObject
	self.tipGo = self.transform:Find("tip").gameObject
	---@type Hukiry.HukirySupperText
	self.tipTxt = self.transform:Find("tip/tipTxt"):GetComponent("HukirySupperText")

end

---释放
function ServerTipControl:OnDestroy()
	for i, v in pairs(ServerTipControl) do
		if type(v) ~= "function" then
			ServerTipControl[i] = nil
		end
	end
end

return ServerTipControl