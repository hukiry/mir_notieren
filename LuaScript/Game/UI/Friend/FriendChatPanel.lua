---
--- 好友聊天面板
--- Created by Administrator.
--- DateTime: 2024/7/17 16:02
---

---@class FriendChatPanel:DisplayObjectBase
local FriendChatPanel = Class(DisplayObjectBase)

function FriendChatPanel:ctor(gameObject)

end

function FriendChatPanel:Start()
    self.contentGo = self:FindGameObject("ScrollChat/Viewport/Content")
    self.pageBackGo = self:FindGameObject("pageBack")
    self.inputChat = self:FindInputField("InputChat/InputField")
    self.sendBtn = self:FindGameObject("InputChat/sendBtn")

    self.sc_ScrollView = self:FindScrollRect("ScrollChat")
    ---@type UnityEngine.UI.ContentSizeFitter
    self.contentFitter = self.contentGo:GetComponent("ContentSizeFitter")

    self.fnickTxt =self:FindHukirySupperText("pageBack/fnick")

    self:AddClick(self.sendBtn, Handle(self, self.OnSend))
    self:AddClick(self.pageBackGo, Handle(self, self.OnDisable))
    self.inputChat.onValueChanged:AddListener(Handle(self, self.onValueChanged))
    ---@type table<number, ChatItem>
    self.itemList = {}
end

---@param friendGo UnityEngine.GameObject 好友列表
---@param info FriendInfo
function FriendChatPanel:OnEnable(friendGo, info)
    EventDispatch:Register(self, UIEvent.UI_ChatView, self.OnDispatch)
    self.friendGo = friendGo
    self.info = info
    self.fnickTxt.text = info.nick
    self.friendGo:SetActive(false)
    self.gameObject:SetActive(true)
    Single.Request().SendChat(info.roleId,nil, EChatState.Chat)
end

---@param state EChatState
function FriendChatPanel:OnDispatch(state)
    if state == EChatState.Chat then
        self.contentFitter.enabled = false
        local array = SingleData.Friend():GetChatArray(self.info.roleId)
        for i, v in ipairs(array) do
            if self.itemList[i] == nil then
                self.itemList[i] = UIItemPool.Get(UIItemType.ChatItem, self.contentGo)
            end
            self.itemList[i]:OnEnable(v)
        end

        StartCoroutine(function()
            WaitForSeconds(0.1)
            self.contentFitter.enabled = true
            WaitForSeconds(0.1)
            GotoScrollViewIndex(table.length(array), self.sc_ScrollView,self.contentGo.transform,false)
        end)
    elseif state == EChatState.SendChat then
        self.inputChat.text = ''
        self.itemList[self.index]:Show(false)
    end
end

function FriendChatPanel:OnSend()
    if string.len(self.content)>0 then
        self:UpdateMessage()
        Single.Request().SendChat(self.info.roleId, self.content, EChatState.SendChat)
    else
        TipMessageBox.ShowUI(GetLanguageText(13106), true)
    end
end

function FriendChatPanel:onValueChanged(value)
    local content = string.Trim(value)
    if Util.String().Length(content) > 50 then
        return
    end
    self.content = content
end

function FriendChatPanel:UpdateMessage()
    self.contentFitter.enabled = false

    ---@type ChatInfo
    local info = require("Game.UI.Friend.Data.ChatInfo").New({
        Id = Single.Player().roleId,
        time = os.time(),
        content = self.content
    })

    self.index  = table.length(self.itemList)+1
    self.itemList[self.index] = UIItemPool.Get(UIItemType.ChatItem, self.contentGo)
    self.itemList[self.index]:OnEnable(info)
    self.itemList[self.index]:Show(true)

    StartCoroutine(function()
        WaitForFixedUpdate()
        WaitForFixedUpdate()
        self.contentFitter.enabled = true

        GotoScrollViewIndex(self.index, self.sc_ScrollView,self.contentGo.transform,false)
    end)
end

function FriendChatPanel:OnDisable()
    if self.friendGo then
        self.friendGo:SetActive(true)
    end
    self.gameObject:SetActive(false)
    EventDispatch:UnRegister(self)
    UIItemPool.PutTable(UIItemType.ChatItem, self.itemList)
end

return FriendChatPanel