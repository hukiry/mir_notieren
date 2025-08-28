---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-06-24
--- Author: Hukiry
---

---@class GameControl
local GameControl = Class()

function GameControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type UnityEngine.UI.ScrollRect
	self.scrollView = self.transform:Find("ScrollView"):GetComponent("ScrollRect")
	---@type UnityEngine.Transform
	self.centerTF = self.transform:Find("ScrollView/Viewport/Center")
	---@type UnityEngine.Transform
	self.homeRawTF = self.transform:Find("ScrollView/Viewport/Center/Home/homeRaw")
	---@type UnityEngine.Transform
	self.topTF = self.transform:Find("ScrollView/Viewport/Center/Home/top")
	---@type UnityEngine.GameObject
	self.settingBtnGo = self.transform:Find("ScrollView/Viewport/Center/Home/top/settingBtn").gameObject
	---@type UnityEngine.GameObject
	self.redpointGo = self.transform:Find("ScrollView/Viewport/Center/Home/top/settingBtn/redpoint").gameObject
	---@type UnityEngine.Transform
	self.downTF = self.transform:Find("down")
	---@type UnityEngine.Transform
	self.backTF = self.transform:Find("down/back")

end

---释放
function GameControl:OnDestroy()
	for i, v in pairs(GameControl) do
		if type(v) ~= "function" then
			GameControl[i] = nil
		end
	end
end

return GameControl