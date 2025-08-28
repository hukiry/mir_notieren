---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-12
--- Author: Hukiry
---

---@class MetaEditorControl
local MetaEditorControl = Class()

function MetaEditorControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.headIcon = self.transform:Find("backUp/mapback/headIcon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.mapLabel = self.transform:Find("backUp/mapback/mapLabel"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.mapName = self.transform:Find("backUp/mapback/mapName"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.authorLv = self.transform:Find("backUp/mapback/authorLv"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.mapEditorTime = self.transform:Find("backUp/mapback/mapEditorTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.quitBtnGo = self.transform:Find("backUp/quitBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.itemBtn = self.transform:Find("backUp/Editorback/itemBtn"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.selectIcon = self.transform:Find("backUp/Editorback/itemBtn/selectIcon"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.delBtn = self.transform:Find("backUp/Editorback/delBtn"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.delIcon = self.transform:Find("backUp/Editorback/delBtn/delIcon"):GetComponent("AtlasImage")
	---@type Hukiry.UI.AtlasImage
	self.resetBtn = self.transform:Find("backUp/Editorback/resetBtn"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.mapTip = self.transform:Find("backUp/Editorback/mapTip"):GetComponent("HukirySupperText")
	---@type UnityEngine.UI.Dropdown
	self.dropdownLayer = self.transform:Find("backUp/Editorback/dropdownLayer"):GetComponent("Dropdown")
	---@type UnityEngine.GameObject
	self.targetBackGo = self.transform:Find("backDown/targetBack").gameObject
	---@type UnityEngine.GameObject
	self.editorBtnGo = self.transform:Find("backDown/editorBtn").gameObject
	---@type UnityEngine.GameObject
	self.setBtnGo = self.transform:Find("backDown/setBtn").gameObject

end

---释放
function MetaEditorControl:OnDestroy()
	for i, v in pairs(MetaEditorControl) do
		if type(v) ~= "function" then
			MetaEditorControl[i] = nil
		end
	end
end

return MetaEditorControl