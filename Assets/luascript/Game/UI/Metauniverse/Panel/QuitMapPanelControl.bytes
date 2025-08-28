---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-06-24
--- Author: Hukiry
---

---@class QuitMapPanelControl
local QuitMapPanelControl = Class()

function QuitMapPanelControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.quitBtnGo = self.transform:Find("quitBtn").gameObject
	---@type UnityEngine.GameObject
	self.appleBtnGo = self.transform:Find("bg/appleBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.appTxt = self.transform:Find("bg/appleBtn/appTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.loginWinGo = self.transform:Find("bg/loginWin").gameObject

end

---释放
function QuitMapPanelControl:OnDestroy()
	for i, v in pairs(QuitMapPanelControl) do
		if type(v) ~= "function" then
			QuitMapPanelControl[i] = nil
		end
	end
end

return QuitMapPanelControl