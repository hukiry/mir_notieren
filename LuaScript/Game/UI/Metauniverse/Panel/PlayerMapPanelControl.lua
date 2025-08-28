---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-11-29
--- Author: Hukiry
---

---@class PlayerMapPanelControl
local PlayerMapPanelControl = Class()

function PlayerMapPanelControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.headIcon = self.transform:Find("headBG/headIcon"):GetComponent("AtlasImage")
	---@type UnityEngine.Transform
	self.propertyListTF = self.transform:Find("propertyList")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/nickLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/roleIDLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/goldLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/lvLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/mapLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/likeLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/rankLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/metaChallengeLabel/txt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.txt = self.transform:Find("propertyList/timeLabel/txt"):GetComponent("HukirySupperText")

end

---释放
function PlayerMapPanelControl:OnDestroy()
	for i, v in pairs(PlayerMapPanelControl) do
		if type(v) ~= "function" then
			PlayerMapPanelControl[i] = nil
		end
	end
end

return PlayerMapPanelControl