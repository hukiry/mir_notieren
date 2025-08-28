---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-18
--- Author: Hukiry
---

---@class MyMapTipControl
local MyMapTipControl = Class()

function MyMapTipControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.MeshGraphic
	self.iconBack = self.transform:Find("bg1/iconBack"):GetComponent("MeshGraphic")
	---@type UnityEngine.GameObject
	self.sendBtnGo = self.transform:Find("bg1/sendBtn").gameObject
	---@type UnityEngine.GameObject
	self.playBtnGo = self.transform:Find("bg1/playBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.headIcon = self.transform:Find("bg1/headBG/headIcon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.idTxt = self.transform:Find("bg1/idTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.timeTxt = self.transform:Find("bg1/timeTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.authorTxt = self.transform:Find("bg1/authorTxt"):GetComponent("HukirySupperText")

end

---释放
function MyMapTipControl:OnDestroy()
	for i, v in pairs(MyMapTipControl) do
		if type(v) ~= "function" then
			MyMapTipControl[i] = nil
		end
	end
end

return MyMapTipControl