---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class TaskItemControl
local TaskItemControl = Class()

function TaskItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("bg/icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.itemNumTx = self.transform:Find("bg/itemNumTx"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.lockGo = self.transform:Find("lock").gameObject
	---@type Hukiry.UI.AtlasImage
	self.finishIcon = self.transform:Find("finishIcon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.numberTx = self.transform:Find("finishIcon/numberTx"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.gouGo = self.transform:Find("gou").gameObject

end

---释放
function TaskItemControl:OnDestroy()
	for i, v in pairs(TaskItemControl) do
		if type(v) ~= "function" then
			TaskItemControl[i] = nil
		end
	end
end

return TaskItemControl