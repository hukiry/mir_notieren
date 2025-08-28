---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class PropsControl
local PropsControl = Class()

function PropsControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type Hukiry.HukirySupperText
	self.itemDesc = self.transform:Find("bg1/bg/itemDesc"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.coinNum = self.transform:Find("bg1/bg/coinNum"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.timeBgGo = self.transform:Find("bg1/bg/timeBg").gameObject
	---@type Hukiry.HukirySupperText
	self.remainTime = self.transform:Find("bg1/bg/timeBg/remainTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.getBtnGo = self.transform:Find("bg1/getBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.playTxt = self.transform:Find("bg1/getBtn/playTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.sliderBgGo = self.transform:Find("bg1/sliderBg").gameObject
	---@type Hukiry.UI.UIProgressbarMask
	self.slider = self.transform:Find("bg1/sliderBg/slider"):GetComponent("UIProgressbarMask")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("bg1/sliderBg/Icon"):GetComponent("AtlasImage")

end

---释放
function PropsControl:OnDestroy()
	for i, v in pairs(PropsControl) do
		if type(v) ~= "function" then
			PropsControl[i] = nil
		end
	end
end

return PropsControl