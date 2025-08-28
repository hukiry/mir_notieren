---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class LevelChallengeControl
local LevelChallengeControl = Class()

function LevelChallengeControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.Transform
	self.contentTF = self.transform:Find("bg1/bg/content")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg1/titleBg/title"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.startBtnGo = self.transform:Find("bg1/startBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.playTxt = self.transform:Find("bg1/startBtn/playTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.failAgainGo = self.transform:Find("bg1/failAgain").gameObject
	---@type UnityEngine.GameObject
	self.selectHorizontalGo = self.transform:Find("bg1/selectHorizontal").gameObject

end

---释放
function LevelChallengeControl:OnDestroy()
	for i, v in pairs(LevelChallengeControl) do
		if type(v) ~= "function" then
			LevelChallengeControl[i] = nil
		end
	end
end

return LevelChallengeControl