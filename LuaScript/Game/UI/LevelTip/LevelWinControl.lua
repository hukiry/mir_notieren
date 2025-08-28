---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class LevelWinControl
local LevelWinControl = Class()

function LevelWinControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.Transform
	self.startLeftTF = self.transform:Find("startLeft")
	---@type UnityEngine.Transform
	self.leftMTF = self.transform:Find("leftM")
	---@type UnityEngine.Transform
	self.startrightTF = self.transform:Find("startright")
	---@type UnityEngine.Transform
	self.rightMTF = self.transform:Find("rightM")
	---@type Hukiry.UI.UIProgressbarMask
	self.slider = self.transform:Find("slider"):GetComponent("UIProgressbarMask")
	---@type UnityEngine.Transform
	self.ttttTF = self.transform:Find("slider/icon/tttt")
	---@type UnityEngine.Transform
	self.startTTTTF = self.transform:Find("slider/icon/startTTT")
	---@type UnityEngine.Transform
	self.imgSTF = self.transform:Find("slider/imgS")
	---@type UnityEngine.GameObject
	self.contineGo = self.transform:Find("contine").gameObject

end

---释放
function LevelWinControl:OnDestroy()
	for i, v in pairs(LevelWinControl) do
		if type(v) ~= "function" then
			LevelWinControl[i] = nil
		end
	end
end

return LevelWinControl