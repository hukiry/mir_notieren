---
---
--- Create Time:2023-07-18
--- Author: Hukiry
---

---@class FriendItem:IUIItem
local FriendItem = Class(IUIItem)

function FriendItem:ctor()
	---@type FriendItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function FriendItem:Start()
	self:AddClick(self.itemCtrl.clickBtnGo, Handle(self, self.OnClick))
	self:AddClick(self.itemCtrl.cancelBtnGo, Handle(self, self.OnCancel))
	self:AddClick(self.itemCtrl.chatBtnGo, Handle(self, self.OnChat))

	---@type UnityEngine.UI.LayoutElement
	self.layoutElement = self.gameObject:GetComponent("LayoutElement")
	---@type UnityEngine.CanvasGroup
	self.canvasGroup = self.gameObject:GetComponent("CanvasGroup")
end

---删除，发邀请，接受
function FriendItem:OnClick()
	--我的好友时
	local state = EFriendHandleState.Delete
	if self.info.state == EFriendHandleState.None then
		--添加陌生人
		state = EFriendHandleState.SendAccept
	elseif self.info.state == EFriendHandleState.ReceiveAccepted  then
		--被邀请时
		state = EFriendHandleState.Accept
	end

	Single.Request().SendFriendHandle(EFriendState.Friend, state, self.info.roleId, 0,function(succ)
		if succ then
			self.itemCtrl.clickBtnGo:SetActive(false)
			self.canvasGroup:DOKill()
			self.canvasGroup:DOFade(0,0.5):OnComplete(function()
				self.layoutElement.ignoreLayout = true
			end)
		end
	end)
end

---取消发送+拒绝
function FriendItem:OnCancel()
	--被邀请时
	local state = EFriendHandleState.RefuseAccept
	if self.info.state == EFriendHandleState.SendAccept then
		--添加陌生人时
		state = EFriendHandleState.CancelAccept
	end
	Single.Request().SendFriendHandle(EFriendState.Friend, state, self.info.roleId)
end

---聊天
function FriendItem:OnChat()
	self.chatBackCall(self.info)
end

---更新数据
---@param info FriendInfo
---@param pageIndex number
function FriendItem:OnEnable(info, pageIndex, chatBackCall)
	self.canvasGroup.alpha = 1
	self.layoutElement.ignoreLayout = false

	self.chatBackCall = chatBackCall
	self.info = info
	self.pageIndex = pageIndex
	self.itemCtrl.nick.text = info.nick
	self.itemCtrl.lvTxt.text = info.level
	self.itemCtrl.iD.text  = "ID:"..info.roleId
	self.itemCtrl.icon.spriteName = info:GetHeadIcon()
	self.itemCtrl.cancelTxt.text = info:GetCancelTxt()
	self.itemCtrl.clickTxt.text = info:GetClickTxt()
	self.itemCtrl.chatTxt.text = GetLanguageText(10030)

	self.itemCtrl.clickBtnGo:SetActive(info:IsClick())
	self.itemCtrl.cancelBtnGo:SetActive(info:IsCancel())
	self.itemCtrl.chatBtnGo:SetActive(info:IsFriend())
end

---隐藏窗口
function FriendItem:OnDisable()
	self.layoutElement.ignoreLayout = false
	self.canvasGroup.alpha = 1
end

---消耗释放资源
function FriendItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return FriendItem