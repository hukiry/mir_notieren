---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-10-20
--- Author: Hukiry
---

---@class MetaFailControl
local MetaFailControl = Class()

function MetaFailControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.mapName = self.transform:Find("bg/bg/mapName"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.mapTime = self.transform:Find("bg/bg/mapTime"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.startBtnGo = self.transform:Find("bg/startBtn").gameObject
	---@type UnityEngine.GameObject
	self.backBtnGo = self.transform:Find("bg/backBtn").gameObject

end

---释放
function MetaFailControl:OnDestroy()
	for i, v in pairs(MetaFailControl) do
		if type(v) ~= "function" then
			MetaFailControl[i] = nil
		end
	end
end

return MetaFailControl