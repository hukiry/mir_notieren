---
---
--- Create Time:2024-07-18
--- Author: Hukiry
---

---@class ChatItem:IUIItem
local ChatItem = Class(IUIItem)

function ChatItem:ctor()
	---@type ChatItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function ChatItem:Start()
	---@type ChatContentItem
	self.leftChat = require("Game/UI/Friend/Item/ChatContentItem").New(self.itemCtrl.leftGo)
	self.leftChat:Start()
	---@type ChatContentItem
	self.rightChat = require("Game/UI/Friend/Item/ChatContentItem").New(self.itemCtrl.rightGo)
	self.rightChat:Start()
end

---更新数据
---@param info ChatInfo
function ChatItem:OnEnable(info)
	self.info = info
	self.itemCtrl.time.text = info:GetTimeString()
	self.itemCtrl.leftGo:SetActive(not  info:IsMe())
	self.itemCtrl.rightGo:SetActive(info:IsMe())
	if info:IsMe() then
		self.rightChat:OnEnable(info)
	else
		self.leftChat:OnEnable(info)
	end
end

function ChatItem:Show(isSendFinish)
	self.rightChat:ShowGan(isSendFinish)
end

---隐藏窗口
function ChatItem:OnDisable()
	
end

---消耗释放资源
function ChatItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return ChatItem