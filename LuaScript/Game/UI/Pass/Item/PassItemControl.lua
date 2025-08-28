---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-05-04
--- Author: Hukiry
---

---@class PassItemControl
local PassItemControl = Class()

function PassItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskUpGo = self.transform:Find("maskUp").gameObject
	---@type UnityEngine.GameObject
	self.maskDownGo = self.transform:Find("maskDown").gameObject
	---@type UnityEngine.GameObject
	self.freeGo = self.transform:Find("free").gameObject
	---@type Hukiry.UI.AtlasImage
	self.iconFree = self.transform:Find("free/iconFree"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.numFree = self.transform:Find("free/numFree"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.get_effectGo = self.transform:Find("free/get_effect").gameObject
	---@type UnityEngine.GameObject
	self.gouFreeGo = self.transform:Find("free/gouFree").gameObject
	---@type UnityEngine.GameObject
	self.linehorizontal1Go = self.transform:Find("linehorizontal1").gameObject
	---@type UnityEngine.GameObject
	self.linehorizontal2Go = self.transform:Find("linehorizontal2").gameObject
	---@type UnityEngine.GameObject
	self.passGo = self.transform:Find("pass").gameObject
	---@type Hukiry.UI.AtlasImage
	self.iconPass = self.transform:Find("pass/iconPass"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.lockPassGo = self.transform:Find("pass/lockPass").gameObject
	---@type UnityEngine.GameObject
	self.gouPassGo = self.transform:Find("pass/gouPass").gameObject
	---@type UnityEngine.GameObject
	self.starIconGo = self.transform:Find("star/starIcon").gameObject
	---@type Hukiry.HukirySupperText
	self.day = self.transform:Find("star/day"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.lineDownGo = self.transform:Find("lineDown").gameObject
	---@type UnityEngine.GameObject
	self.lockGo = self.transform:Find("lock").gameObject
	---@type UnityEngine.GameObject
	self.tipsGo = self.transform:Find("tips").gameObject

end

---释放
function PassItemControl:OnDestroy()
	for i, v in pairs(PassItemControl) do
		if type(v) ~= "function" then
			PassItemControl[i] = nil
		end
	end
end

return PassItemControl