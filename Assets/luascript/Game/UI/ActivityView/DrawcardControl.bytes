---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-04
--- Author: Hukiry
---

---@class DrawcardControl
local DrawcardControl = Class()

function DrawcardControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.startPageGo = self.transform:Find("bg1/startPage").gameObject
	---@type UnityEngine.GameObject
	self.getBtnGo = self.transform:Find("bg1/startPage/getBtn").gameObject
	---@type UnityEngine.GameObject
	self.drawPageGo = self.transform:Find("bg1/drawPage").gameObject
	---@type UnityEngine.GameObject
	self.downbgGo = self.transform:Find("bg1/drawPage/downbg").gameObject
	---@type Hukiry.HukirySupperText
	self.itemDesc = self.transform:Find("bg1/drawPage/downbg/itemDesc"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.claimBtnGo = self.transform:Find("bg1/drawPage/claimBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.playTxt = self.transform:Find("bg1/drawPage/claimBtn/playTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.gridCardGo = self.transform:Find("bg1/drawPage/GridCard").gameObject
	---@type UnityEngine.GameObject
	self.iconCenterGo = self.transform:Find("bg1/drawPage/iconCenter").gameObject

end

---释放
function DrawcardControl:OnDestroy()
	for i, v in pairs(DrawcardControl) do
		if type(v) ~= "function" then
			DrawcardControl[i] = nil
		end
	end
end

return DrawcardControl