---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2023-11-29
--- Author: Hukiry
---

---@class LevelMainControl
local LevelMainControl = Class()

function LevelMainControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.GameObject
	self.backgoudGo = self.transform:Find("backgoud").gameObject
	---@type UnityEngine.GameObject
	self.bgHeadGo = self.transform:Find("backgoud/bgHead").gameObject
	---@type Hukiry.UI.AtlasImage
	self.iconAn = self.transform:Find("backgoud/bgHead/iconAn"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.metaBoxGo = self.transform:Find("backgoud/metaBox").gameObject
	---@type Hukiry.HukirySupperText
	self.nickName = self.transform:Find("backgoud/metaBox/nickName"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.mapTag = self.transform:Find("backgoud/metaBox/mapTag"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.mapTime = self.transform:Find("backgoud/metaBox/mapTime"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.targetBack = self.transform:Find("backgoud/targetBack"):GetComponent("AtlasImage")
	---@type UnityEngine.GameObject
	self.targetHorizontalGo = self.transform:Find("backgoud/targetBack/targetHorizontal").gameObject
	---@type Hukiry.UI.AtlasImage
	self.moveBack = self.transform:Find("backgoud/moveBack"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.moveNum = self.transform:Find("backgoud/moveBack/moveNum"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.UIProgressbarMask
	self.sliderTree = self.transform:Find("backgoud/sliderTree"):GetComponent("UIProgressbarMask")
	---@type UnityEngine.GameObject
	self.downGridGo = self.transform:Find("downGrid").gameObject
	---@type UnityEngine.GameObject
	self.aniSetBtnGo = self.transform:Find("downGrid/iconBtn5/aniSetBtn").gameObject
	---@type UnityEngine.GameObject
	self.propHelpGo = self.transform:Find("PropHelp").gameObject
	---@type Hukiry.HukirySupperText
	self.titleTx = self.transform:Find("PropHelp/titleTx"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.descTx = self.transform:Find("PropHelp/descTx"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.settingPanelGo = self.transform:Find("SettingPanel").gameObject
	---@type UnityEngine.GameObject
	self.musicEffectBtnGo = self.transform:Find("SettingPanel/frameSetting/MusicEffectBtn").gameObject
	---@type UnityEngine.GameObject
	self.musicSoundBtnGo = self.transform:Find("SettingPanel/frameSetting/MusicSoundBtn").gameObject
	---@type UnityEngine.GameObject
	self.exitBtnGo = self.transform:Find("SettingPanel/frameSetting/ExitBtn").gameObject
	---@type UnityEngine.GameObject
	self.firstPropsGo = self.transform:Find("FirstProps").gameObject
	---@type Hukiry.UI.UIProgressbarMask
	self.slider = self.transform:Find("FirstProps/slider"):GetComponent("UIProgressbarMask")
	---@type UnityEngine.Transform
	self.mfTF = self.transform:Find("FirstProps/slider/mf")
	---@type UnityEngine.Transform
	self.targetPosTF = self.transform:Find("targetPos")
	---@type UnityEngine.GameObject
	self.quitBtnGo = self.transform:Find("quitBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.mapName = self.transform:Find("quitBtn/boxFrame/mapName"):GetComponent("HukirySupperText")

end

---释放
function LevelMainControl:OnDestroy()
	for i, v in pairs(LevelMainControl) do
		if type(v) ~= "function" then
			LevelMainControl[i] = nil
		end
	end
end

return LevelMainControl