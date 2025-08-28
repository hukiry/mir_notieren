---
--- UI View Gen From GenUITools，Please Don't Modify!
--- Create Time:2024-07-22
--- Author: Hukiry
---

---@class FriendItemControl
local FriendItemControl = Class()

function FriendItemControl:ctor(gameObject)
	---@type UnityEngine.GameObject 
	self.gameObject = gameObject
	---@type UnityEngine.Transform
	self.transform = gameObject.transform
	---@type UnityEngine.RectTransform
	self.rectTransform = gameObject:GetComponent("RectTransform")
	---@type Hukiry.HukirySupperText
	self.lvTxt = self.transform:Find("bg/lvLabel/lvTxt"):GetComponent("HukirySupperText")
	---@type Hukiry.UI.AtlasImage
	self.icon = self.transform:Find("bg/bgFrame/icon"):GetComponent("AtlasImage")
	---@type Hukiry.HukirySupperText
	self.nick = self.transform:Find("bg/nick"):GetComponent("HukirySupperText")
	---@type Hukiry.HukirySupperText
	self.iD = self.transform:Find("bg/ID"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.clickBtnGo = self.transform:Find("bg/vlayout/clickBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.clickTxt = self.transform:Find("bg/vlayout/clickBtn/clickTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.cancelBtnGo = self.transform:Find("bg/vlayout/cancelBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.cancelTxt = self.transform:Find("bg/vlayout/cancelBtn/cancelTxt"):GetComponent("HukirySupperText")
	---@type UnityEngine.GameObject
	self.chatBtnGo = self.transform:Find("bg/vlayout/chatBtn").gameObject
	---@type Hukiry.HukirySupperText
	self.chatTxt = self.transform:Find("bg/vlayout/chatBtn/chatTxt"):GetComponent("HukirySupperText")

end

---释放
function FriendItemControl:OnDestroy()
	for i, v in pairs(FriendItemControl) do
		if type(v) ~= "function" then
			FriendItemControl[i] = nil
		end
	end
end

return FriendItemControl