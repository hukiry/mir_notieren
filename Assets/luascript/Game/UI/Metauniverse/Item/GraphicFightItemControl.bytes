---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-17
--- Author: Hukiry
---

---@class GraphicFightItemControl
local GraphicFightItemControl = Class()

function GraphicFightItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.titleBg = self.transform:Find("titleBg"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.name = self.transform:Find("titleBg/name"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.MeshGraphic
	self.iconBack = self.transform:Find("iconBack"):GetComponent("MeshGraphic")
	---@type UnityEngine.GameObject
	self.authorLabelGo = self.transform:Find("authorLabel").gameObject
	---@type Hukiry.HukirySupperText
	self.authordesc = self.transform:Find("authordesc"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.fightBtn = self.transform:Find("fightBtn"):GetComponent("AtlasImage")

end

---释放
function GraphicFightItemControl:OnDestroy()
	for i, v in pairs(GraphicFightItemControl) do
		if type(v) ~= "function" then
			GraphicFightItemControl[i] = nil
		end
	end
end

return GraphicFightItemControl