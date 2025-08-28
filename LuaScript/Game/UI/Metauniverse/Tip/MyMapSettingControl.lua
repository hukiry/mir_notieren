---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-16
--- Author: Hukiry
---

---@class MyMapSettingControl
local MyMapSettingControl = Class()

function MyMapSettingControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.descTxt = self.transform:Find("bg/descTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.Transform
	self.musicBackTF = self.transform:Find("bg/musicBack")
	---@type UnityEngine.Transform
	self.musicEffectTF = self.transform:Find("bg/musicEffect")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.saveBtnGo = self.transform:Find("bg/saveBtn").gameObject
	---@type UnityEngine.Transform
	self.avaibleGridTF = self.transform:Find("bg/avaibleGrid")
	---@type UnityEngine.Transform
	self.targetGridTF = self.transform:Find("bg/targetGrid")

end

---释放
function MyMapSettingControl:OnDestroy()
	for i, v in pairs(MyMapSettingControl) do
		if type(v) ~= "function" then
			MyMapSettingControl[i] = nil
		end
	end
end

return MyMapSettingControl