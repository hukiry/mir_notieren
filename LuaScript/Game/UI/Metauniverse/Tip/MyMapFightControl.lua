---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-18
--- Author: Hukiry
---

---@class MyMapFightControl
local MyMapFightControl = Class()

function MyMapFightControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.MeshGraphic
	self.iconBack = self.transform:Find("bg/iconBack"):GetComponent("MeshGraphic")
	---@type Hukiry.HukirySupperText
	self.mapdesc = self.transform:Find("bg/mapdesc"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.writeComment = self.transform:Find("bg/otherbg/label_writeComment/writeComment"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.commentBtnGo = self.transform:Find("bg/otherbg/label_writeComment/commentBtn").gameObject
	---@type UnityEngine.GameObject
	self.commentBoxGo = self.transform:Find("bg/otherbg/Scroll View/Viewport/commentBox").gameObject
	---@type UnityEngine.UI.InputField
	self.inputField = self.transform:Find("bg/otherbg/Scroll View/Viewport/commentBox/InputField"):GetComponent("InputField")
	---@type UnityEngine.GameObject
	self.sendBtnGo = self.transform:Find("bg/otherbg/Scroll View/Viewport/commentBox/sendBtn").gameObject
	---@type UnityEngine.GameObject
	self.playBtnGo = self.transform:Find("bg/playBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.nameTxt = self.transform:Find("bg/nameTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.headIcon = self.transform:Find("bg/headbg/headIcon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.nickTxt = self.transform:Find("bg/headbg/nickTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.downTxt = self.transform:Find("bg/downTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.timeTxt = self.transform:Find("bg/timeTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.mapTxt = self.transform:Find("bg/mapTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.rateBtnGo = self.transform:Find("bg/rateBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject

end

---释放
function MyMapFightControl:OnDestroy()
	for i, v in pairs(MyMapFightControl) do
		if type(v) ~= "function" then
			MyMapFightControl[i] = nil
		end
	end
end

return MyMapFightControl