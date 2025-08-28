---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-21
--- Author: Hukiry
---

---@class RankItemControl
local RankItemControl = Class()

function RankItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.sortTxt = self.transform:Find("sortTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.lvTxt = self.transform:Find("lvTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.bgFrame = self.transform:Find("bgFrame"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.massIcon = self.transform:Find("bgFrame/massIcon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.massName = self.transform:Find("massName"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.roleName = self.transform:Find("roleName"):GetComponent("HukirySupperText")

end

---释放
function RankItemControl:OnDestroy()
	for i, v in pairs(RankItemControl) do
		if type(v) ~= "function" then
			RankItemControl[i] = nil
		end
	end
end

return RankItemControl