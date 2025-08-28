---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MyMapPanelControl
local MyMapPanelControl = Class()

function MyMapPanelControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.menuListTF = self.transform:Find("menuList")
	---@type UnityEngine.GameObject
	self.uploadBtnGo = self.transform:Find("uploadBtn").gameObject
	---@type UnityEngine.GameObject
	self.expanBtnGo = self.transform:Find("expanBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.remain = self.transform:Find("remain"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.releaseBtnGo = self.transform:Find("downExpand/releaseBtn").gameObject
	---@type UnityEngine.GameObject
	self.createmapBtnGo = self.transform:Find("downExpand/createmapBtn").gameObject
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("ScrollView/Viewport/Content").gameObject

end

---释放
function MyMapPanelControl:OnDestroy()
	for i, v in pairs(MyMapPanelControl) do
		if type(v) ~= "function" then
			MyMapPanelControl[i] = nil
		end
	end
end

return MyMapPanelControl