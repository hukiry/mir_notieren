---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class LifeControl
local LifeControl = Class()

function LifeControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type Hukiry.HukirySupperText
	self.remainTime = self.transform:Find("bg1/bg/remainTime"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.lifeNum = self.transform:Find("bg1/bg/AtlasImage/lifeNum"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg1/title"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.freeBtnGo = self.transform:Find("bg1/freeBtn").gameObject
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.buyBtnGo = self.transform:Find("bg1/buyBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.buyNum = self.transform:Find("bg1/buyBtn/buyNum"):GetComponent("HukirySupperText")

end

---释放
function LifeControl:OnDestroy()
	for i, v in pairs(LifeControl) do
		if type(v) ~= "function" then
			LifeControl[i] = nil
		end
	end
end

return LifeControl