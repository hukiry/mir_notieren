---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class LevelFailControl
local LevelFailControl = Class()

function LevelFailControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.buyBtnGo = self.transform:Find("bg1/buyBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.buyNum = self.transform:Find("bg1/buyBtn/buyNum"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type UnityEngine.Transform
	self.topTF = self.transform:Find("top")

end

---释放
function LevelFailControl:OnDestroy()
	for i, v in pairs(LevelFailControl) do
		if type(v) ~= "function" then
			LevelFailControl[i] = nil
		end
	end
end

return LevelFailControl