---
--- ChatContentItem       
--- Author : hukiry     
--- DateTime 2024/7/18 17:07   
---

---@class ChatContentItem:IUIItem
local ChatContentItem = Class(IUIItem)
function ChatContentItem:ctor(gameObject)
end

---初始:注册按钮事件等
function ChatContentItem:Start()
    self.nickTxt = self:FindHukirySupperText("nick")
    self.icon = self:FindAtlasImage("icon")
    self.contentTxt = self:FindHukirySupperText("content")
    self.ganGo = self:FindGameObject("gan")

    self:AddClick(self.gameObject, Handle(self, self.Resend))
end

function ChatContentItem:Resend()
    if not self.isSendFinish then
        Single.Request().SendChat(self.info.Id, self.info.content, EChatState.SendChat)
    end
end

---更新数据
---@param info ChatInfo
function ChatContentItem:OnEnable(info)
    self.info = info
    self.contentTxt.text = info:GetContent()
    --self.nickTxt.text = info:GetNick()--群聊才显示
    self.icon.spriteName = info:GetIconName()
    self.ganGo:SetActive(false)
    self.isSendFinish = true
end

---发送失败时的显示
function ChatContentItem:ShowGan(isSendFinish)
    self.isSendFinish = isSendFinish
    self.ganGo:SetActive(isSendFinish)
end

---隐藏窗口
function ChatContentItem:OnDisable()
end
return ChatContentItem