---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-12
--- Author: Hukiry
---

---@class MetaSelectControl
local MetaSelectControl = Class()

function MetaSelectControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.pageListTF = self.transform:Find("bg/pageList")
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("bg/ScrollView/Viewport/Content").gameObject
	---@type Hukiry.UI.AtlasImage
	self.okBtn = self.transform:Find("bg/okBtn"):GetComponent("AtlasImage")
	---@type UnityEngine.Transform
	self.tipBGTF = self.transform:Find("bg/tipBG")
	---@type Hukiry.HukirySupperText
	self.itemName = self.transform:Find("bg/tipBG/itemName"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("closeBtn").gameObject

end

---释放
function MetaSelectControl:OnDestroy()
	for i, v in pairs(MetaSelectControl) do
		if type(v) ~= "function" then
			MetaSelectControl[i] = nil
		end
	end
end

return MetaSelectControl