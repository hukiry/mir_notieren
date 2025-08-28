---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-08-08
--- Author: Hukiry
---

---@class SettingControl
local SettingControl = Class()

function SettingControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.maskGo = self.transform:Find("mask").gameObject
	---@type UnityEngine.GameObject
	self.cloudBtnGo = self.transform:Find("bg/cloudBtn").gameObject
	---@type UnityEngine.GameObject
	self.redpoint1Go = self.transform:Find("bg/cloudBtn/redpoint1").gameObject
	---@type UnityEngine.GameObject
	self.rateStarGo = self.transform:Find("bg/rateStar").gameObject
	---@type UnityEngine.Transform
	self.toggleListTF = self.transform:Find("bg/contentbg/toggleList")
	---@type UnityEngine.GameObject
	self.languageBtnGo = self.transform:Find("bg/languageBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.lanTxt = self.transform:Find("bg/languageBtn/lanTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.headBtnGo = self.transform:Find("bg/headBtn").gameObject
	---@type Hukiry.UI.AtlasImage
	self.headIcon = self.transform:Find("bg/headBtn/Frame/headIcon"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.editorBtnGo = self.transform:Find("bg/editorBtn").gameObject
	---@type UnityEngine.GameObject
	self.privateBtnGo = self.transform:Find("bg/privateBtn").gameObject
	---@type UnityEngine.GameObject
	self.askBtnGo = self.transform:Find("bg/askBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.roleTxt = self.transform:Find("bg/roleTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.version = self.transform:Find("bg/version"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.closeBtnGo = self.transform:Find("bg/closeBtn").gameObject
	---@type UnityEngine.GameObject
	self.appleBtnGo = self.transform:Find("bg/appleBtn").gameObject
	---@type UnityEngine.GameObject
	self.redpointGo = self.transform:Find("bg/appleBtn/redpoint").gameObject
	---@type UnityEngine.GameObject
	self.logoutBtnGo = self.transform:Find("bg/logoutBtn").gameObject
	---@type UnityEngine.GameObject
	self.loginWinGo = self.transform:Find("bg/loginWin").gameObject

end

---释放
function SettingControl:OnDestroy()
	for i, v in pairs(SettingControl) do
		if type(v) ~= "function" then
			SettingControl[i] = nil
		end
	end
end

return SettingControl