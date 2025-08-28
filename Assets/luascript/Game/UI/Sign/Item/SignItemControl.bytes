---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class SignItemControl
local SignItemControl = Class()

function SignItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.numtx = self.transform:Find("icon/numtx"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.bgnameGo = self.transform:Find("bgname").gameObject
	---@type Hukiry.HukirySupperText
	self.dayTxt = self.transform:Find("bgname/dayTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.effect_ui_rewardGo = self.transform:Find("effect_ui_reward").gameObject
	---@type UnityEngine.GameObject
	self.gouGo = self.transform:Find("gou").gameObject

end

---释放
function SignItemControl:OnDestroy()
	for i, v in pairs(SignItemControl) do
		if type(v) ~= "function" then
			SignItemControl[i] = nil
		end
	end
end

return SignItemControl