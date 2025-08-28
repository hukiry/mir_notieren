---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-29
--- Author: Hukiry
---

---@class LoadingControl
local LoadingControl = Class()

function LoadingControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.Transform
	self.backTF = self.transform:Find("back")
	---@type Hukiry.UI.UIProgressbarMask
	self.sliderFore = self.transform:Find("backSlider/sliderFore"):GetComponent("UIProgressbarMask")
	---@type UnityEngine.UI.Text
	self.percentTxt = self.transform:Find("backSlider/percentTxt"):GetComponent("Text")

end

---释放
function LoadingControl:OnDestroy()
	for i, v in pairs(LoadingControl) do
		if type(v) ~= "function" then
			LoadingControl[i] = nil
		end
	end
end

return LoadingControl