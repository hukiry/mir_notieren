---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-16
--- Author: Hukiry
---

---@class MyMapInfoControl
local MyMapInfoControl = Class()

function MyMapInfoControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.MeshGraphic
	self.backGraphic = self.transform:Find("bg/backGraphic"):GetComponent("MeshGraphic")
	---@type UnityEngine.GameObject
	self.messgeBtnGo = self.transform:Find("bg/messgeBtn").gameObject
	---@type UnityEngine.GameObject
	self.updateBtnGo = self.transform:Find("bg/updateBtn").gameObject
	---@type UnityEngine.GameObject
	self.dataBtnGo = self.transform:Find("bg/dataBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.headIcon = self.transform:Find("bg/headIcon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.nameTxt = self.transform:Find("bg/nameTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.descTxt = self.transform:Find("bg/descTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject

end

---释放
function MyMapInfoControl:OnDestroy()
	for i, v in pairs(MyMapInfoControl) do
		if type(v) ~= "function" then
			MyMapInfoControl[i] = nil
		end
	end
end

return MyMapInfoControl