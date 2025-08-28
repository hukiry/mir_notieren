---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-04
--- Author: Hukiry
---

---@class LevelQuitControl
local LevelQuitControl = Class()

function LevelQuitControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type Hukiry.HukirySupperText
	self.desc = self.transform:Find("bg1/Contentbg/desc"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.quitBtnGo = self.transform:Find("bg1/quitBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function LevelQuitControl:OnDestroy()
	for i, v in pairs(LevelQuitControl) do
		if type(v) ~= "function" then
			LevelQuitControl[i] = nil
		end
	end
end

return LevelQuitControl