---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-11-29
--- Author: Hukiry
---

---@class GraphicCardItemControl
local GraphicCardItemControl = Class()

function GraphicCardItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.name = self.transform:Find("name"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.MeshGraphic
	self.iconBack = self.transform:Find("iconBack"):GetComponent("MeshGraphic")
	---@type Hukiry.HukirySupperText
	self.desc = self.transform:Find("iconBack/desc"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.stateBg = self.transform:Find("stateBg"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.stateTxt = self.transform:Find("stateBg/stateTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.editorBtnGo = self.transform:Find("EditorBtn").gameObject
	---@type UnityEngine.GameObject
	self.lookBtnGo = self.transform:Find("lookBtn").gameObject

end

---释放
function GraphicCardItemControl:OnDestroy()
	for i, v in pairs(GraphicCardItemControl) do
		if type(v) ~= "function" then
			GraphicCardItemControl[i] = nil
		end
	end
end

return GraphicCardItemControl