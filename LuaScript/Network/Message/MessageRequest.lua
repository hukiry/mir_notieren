---
--- 消息请求
--- Created by hukiry.
--- DateTime: 2023/5/23 20:05
---

require("Network.Message.MessageRule")

---@class MessageRequest
local MessageRequest = {}

local SIGN_KEY = "hctam5emoc4yekngis7"
---请求数据
function MessageRequest.SendLogin(isLoginView, callback)
    ---@type MSG_1001
    local msg = NetSocket:pb(protocal.MSG_1001)
    msg.deviceId = UnityEngine.SystemInfo.deviceUniqueIdentifier
    msg.lang = Single.SdkPlatform():GetLanguageCode()
    msg.login_type = Single.SdkPlatform().loginType
    if UNITY_EDITOR then
        msg.login_type = 0
        msg.platform = 0
    elseif UNITY_ANDROID then
        msg.platform = 1
    elseif UNITY_IOS then
        msg.platform = 2
    end
    Single.Player().loginTime = os.time()
    NetSocket:Send(protocal.MSG_1001, msg, {callback = callback,isLoginView=isLoginView})
end

---上传游戏数据
function MessageRequest.SendRoleData()
    ---@type MSG_1002
    local msg = NetSocket:pb(protocal.MSG_1002)
    msg.deviceId = UnityEngine.SystemInfo.deviceUniqueIdentifier
    Single.Player():SetPackValue(ELogin.HeadID,  Single.Player().headId)
    Single.Player():SetPackValue(ELogin.Nick,  Single.Player().roleNick)
    Single.Player():SetPackValue(ELogin.LangCode, Single.SdkPlatform():GetLanguageCode())
    local items = Single.Player().items
    for key, v in pairs(items) do
        ---@type ITEMSRESOURCE
        local message ={}
        message.type = key
        message.number = v
        table.insert(msg.items, message)
    end

    local itemsPack = Single.Player().itemsPack
    for key, v in pairs(itemsPack) do
        ---@type ITEMSPACK
        local message ={}
        message.type = key
        message.value = v
        table.insert(msg.itemsPacks, message)
    end
    NetSocket:Send(protocal.MSG_1002, msg)
end

---绑定或登出数据
---@param state EHttpLoginState
function MessageRequest.SendBindLogout(state, token, roleNick, callback)
    token = token or ""
    ---@type MSG_1003
    local msg = NetSocket:pb(protocal.MSG_1003)
    msg.state = state
    msg.token = token or ''
    msg.roleNick = roleNick or ''
    msg.deviceId = UnityEngine.SystemInfo.deviceUniqueIdentifier
    ---仅绑定时用
    msg.bindAccount = Single.Player().bindAccount
    NetSocket:Send(protocal.MSG_1003, msg, {callback = callback})
end

---心跳时间戳：用于所有活动在线
function MessageRequest.SendHeart()
    ---@type MSG_1004
    local msg = NetSocket:pb(protocal.MSG_1004)
    NetSocket:Send(protocal.MSG_1004, msg)
end

---上传货币类型
---@param type EMoneyType
---@param number number
function MessageRequest.SendMoney(type,number)
    ---@type MSG_1005
    local msg = NetSocket:pb(protocal.MSG_1005)
    msg.type = type
    msg.number = number
    NetSocket:Send(protocal.MSG_1005, msg)
end

---上传头像
---@param headId number
function MessageRequest.SendHeadId(headId)
    ---@type MSG_1006
    local msg = NetSocket:pb(protocal.MSG_1006)
    msg.headID = headId
    NetSocket:Send(protocal.MSG_1006, msg)
end

---邮件和公告数据
---@param isMail boolean 1=邮件 0=公告
---@param state EMailState
---@param callback function 刷新视图函数
function MessageRequest.SendMailBoard(isMail, state, mailId, callback)
    ---@type MSG_1101
    local msg = NetSocket:pb(protocal.MSG_1101)
    msg.roleId = Single.Player().roleId
    msg.state = state
    msg.type = isMail and 1 or 2
    msg.id = mailId or 0
    msg.lanCode = Single.SdkPlatform():GetLanguageCode()
    NetSocket:Send(protocal.MSG_1101, msg, {callback = callback})
end


---所有活动：定时请求，领取操作
---@param type EActivityType
---@param state EHttpActivityState
---@param callback function 刷新视图函数
---@param value string 进度字符串值
function MessageRequest.SendActivity(type, state, value, callback)
    ---@type MSG_1201
    local msg = NetSocket:pb(protocal.MSG_1201)
    msg.roleId = Single.Player().roleId
    msg.state = state
    msg.type = type
    msg.strValue = value or ''
    NetSocket:Send(protocal.MSG_1201, msg, {callback = callback})
end

---排行榜数据
---@param type EHttpRankType
---@param callback function 刷新视图函数
function MessageRequest.SendCommonRank(type, callback)
    ---@type MSG_1301
    local msg = NetSocket:pb(protocal.MSG_1301)
    msg.type = type
    NetSocket:Send(protocal.MSG_1301, msg, {callback = callback})
end

---上传反馈数据
---@param type EHttpFeedfaceType
---@param content string 限制30个字符
function MessageRequest.SendCommonFeedface(type, content, e_mail, callback)
    ---@type MSG_1302
    local msg = NetSocket:pb(protocal.MSG_1302)
    msg.roleId = Single.Player().roleId
    msg.type = type
    msg.content = content
    msg.e_mail = e_mail
    NetSocket:Send(protocal.MSG_1302, msg, {callback = callback})
end

---商店数据
---@param state EHttpShopState
---@param shopId number 商品id
---@param price number 充值价格
---@param callback function 刷新视图函数
function MessageRequest.SendShop(state, shopId, price, callback)
    ---@type MSG_1401
    local msg = NetSocket:pb(protocal.MSG_1401)
    msg.roleId = Single.Player().roleId
    msg.state = state
    msg.shopId = shopId
    msg.price = price
    NetSocket:Send(protocal.MSG_1401, msg,{callback = callback})
end

---好友，陌生人，消息，生命列表
---@param state EFriendState
function MessageRequest.SendFriend(state, friendId, callback)
    ---@type MSG_1501
    local msg = NetSocket:pb(protocal.MSG_1501)
    msg.roleId = Single.Player().roleId
    msg.state = state
    msg.friendId = friendId or 0
    NetSocket:Send(protocal.MSG_1501, msg,{callback = callback})
end

---元宇宙，好友操作，生命操作
---@param type EFriendHandleType
---@param state EFriendHandleState
---@param friendId number 好友id
---@param mapId number 发起挑战的地图id
function MessageRequest.SendFriendHandle(type, state, friendId, mapId, callback)
    ---@type MSG_1502
    local msg = NetSocket:pb(protocal.MSG_1502)
    msg.roleId = Single.Player().roleId
    msg.type = type
    msg.state = state
    msg.friendId = friendId
    msg.mapId = mapId or 0
    NetSocket:Send(protocal.MSG_1502, msg,{callback = callback, state = state})
end

---好友聊天：发送消息，获取聊天记录，开房间
---@param state EChatState
function MessageRequest.SendChat(friendId, content, state, callback)
    ---@type MSG_1503
    local msg = NetSocket:pb(protocal.MSG_1503)
    msg.roleId = Single.Player().roleId
    msg.friendId = friendId
    msg.state = state
    msg.message = content or ''
    NetSocket:Send(protocal.MSG_1503, msg,{callback = callback})
end

---社团信息
---@param state ELifeState
--function MessageRequest.SendLife(friendId, state, callback)
--    ---@type MSG_1504
--    local msg = NetSocket:pb(protocal.MSG_1504)
--    msg.roleId = Single.Role().roleId
--    msg.gassId = Single.Role().gassId
--    msg.friendId = friendId
--    msg.state = state
--    NetSocket:Send(protocal.MSG_1504, msg,{callback = callback})
--end

---元宇宙操作
---@param state EFriendHandleType
---@param friendId number 好友角色id
---@param numberId number 好友地图id点赞
function MessageRequest.SendMeta(state, friendId, numberId, comment, callback)
    ---@type MSG_1601
    local msg = NetSocket:pb(protocal.MSG_1601)
    msg.state = state
    msg.roleId = Single.Player().roleId
    msg.friendId = friendId or 0
    msg.numberId = numberId or 0
    msg.comment = comment or 0
    msg.metaInfos = {}
    NetSocket:Send(protocal.MSG_1601, msg,{callback = callback})
end

return MessageRequest