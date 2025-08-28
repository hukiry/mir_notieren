---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-26
--- Author: Hukiry
---

---@class RewardItemControl
local RewardItemControl = Class()

function RewardItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.num = self.transform:Find("num"):GetComponent("HukirySupperText")

end

---释放
function RewardItemControl:OnDestroy()
	for i, v in pairs(RewardItemControl) do
		if type(v) ~= "function" then
			RewardItemControl[i] = nil
		end
	end
end

return RewardItemControl