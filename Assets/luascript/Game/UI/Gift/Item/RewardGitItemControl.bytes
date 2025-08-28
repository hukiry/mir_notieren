---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class RewardGitItemControl
local RewardGitItemControl = Class()

function RewardGitItemControl:ctor(gameObject)
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
function RewardGitItemControl:OnDestroy()
	for i, v in pairs(RewardGitItemControl) do
		if type(v) ~= "function" then
			RewardGitItemControl[i] = nil
		end
	end
end

return RewardGitItemControl