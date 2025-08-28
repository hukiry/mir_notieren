---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class TaskControl
local TaskControl = Class()

function TaskControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.taskBarGo = self.transform:Find("bg1/bg/taskBar").gameObject
	---@type UnityEngine.GameObject
	self.contentGo = self.transform:Find("bg1/bg/ScrollView/Viewport/Content").gameObject
	---@type Hukiry.HukirySupperText
	self.title = self.transform:Find("bg1/titleBg/title"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg1/closeBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.iconH = self.transform:Find("bg1/help/iconH"):GetComponent("AtlasImage")

end

---释放
function TaskControl:OnDestroy()
	for i, v in pairs(TaskControl) do
		if type(v) ~= "function" then
			TaskControl[i] = nil
		end
	end
end

return TaskControl