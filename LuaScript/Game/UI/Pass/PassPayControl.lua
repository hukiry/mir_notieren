---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class PassPayControl
local PassPayControl = Class()

function PassPayControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.remainTime = self.transform:Find("bg1/bgContent/timeBg/remainTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.move5Go = self.transform:Find("bg1/bgContent/move5").gameObject
	---@type UnityEngine.Transform
	self.rewardVerticleTF = self.transform:Find("bg1/bgContent/rewardVerticle")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg1/titleBg/title"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.payBtnGo = self.transform:Find("bg1/payBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.playTxt = self.transform:Find("bg1/payBtn/playTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject

end

---释放
function PassPayControl:OnDestroy()
	for i, v in pairs(PassPayControl) do
		if type(v) ~= "function" then
			PassPayControl[i] = nil
		end
	end
end

return PassPayControl