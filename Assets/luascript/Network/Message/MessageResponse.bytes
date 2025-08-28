---
--- 消息响应
--- Created by hukiry.
--- DateTime: 2023/5/23 20:04
---


---@class MessageResponse
local MessageResponse = {}
function MessageResponse.OnReceive(cmd, msg, data)
    local funMothed =  MessageResponse["_"..cmd]
    if funMothed then
        funMothed(msg, data)
    end

    if data and data.callback then
        data.callback(true)
        data.callback = nil
    end
end

---请求失败，无数据
---@param cmd protocal 发送的消息
function MessageResponse.OnErrorCode(cmd, data, isTip)
    if data and data.callback then
        data.callback(false)
        data.callback = nil
    end

    if Single.Http():IsHaveNetwork() and isTip then
        TipMessageBox.ShowUI( GetLanguageText(10001))
    end
end

-------------------------------------所有消息体的方法----------------------------------------
---无返回
---@param msg MSG_1001
function MessageResponse._1001(msg, data)

end

---登录成功
---@param msg MSG_1002
function MessageResponse._1002(msg, data)
    if msg.state == 0 then
        return
    end

    Single.Player():InitMsg(msg)
    Util.Time().serverTime = msg.timeStamp

    if data and data.isLoginView then
        SingleData.Login():EnterGame()
    end

    SingleData.Activity():SendALLHttp()
end

---绑定或注销成功
---@param msg MSG_1003
function MessageResponse._1003(msg, data)
    if msg.state == EHttpLoginState.Logout then
        Single.SdkPlatform():GetLoginInfo():Logout()
        Single.PlayerPrefs():SetInt(EGameSetting.BindFacebook, 0)
        Single.GameCenter():LoginOut()
        Single.Binary():DeleteDirectory()
    elseif msg.state == EHttpLoginState.Bind  then
        --绑定成功
        TipMessageBox.ShowUI(GetLanguageText(10005))
    elseif msg.state == EHttpLoginState.ChangeDevice  then
        UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10003), function()
            Single.GameCenter():LoginOut()
        end, function()  end, true)
    elseif msg.state == EHttpLoginState.FixNick  then
        --修改昵称成功
        TipMessageBox.ShowUI(GetLanguageText(10004))
    end
end

---心跳
---@param msg MSG_1004
function MessageResponse._1004(msg, data)
    Single.HeartBeat():ReceiveHeartBeat(msg)
end

---请求邮件 和 操作邮件
---@param msg MSG_1101
function MessageResponse._1101(msg, data)
    if msg.state == EHttpMailState.Request then
        if msg.type == 1 then
            SingleData.Mail():SyncMail(msg.mails)
        elseif msg.type == 2 then--公告
            if #msg.mails>0 then
                SingleData.Mail():SyncBillBoard(msg.mails[1])
            end
        end
    elseif msg.state == EHttpMailState.Reded then
        if msg.type == 1 then--邮件
            SingleData.Mail():RemoveData(false, msg.id)
        end
    end
end

---所有活动：定时请求，领取操作
---@param msg MSG_1201
function MessageResponse._1201(msg, data)
    if msg.state == EHttpActivityState.Request then
        SingleData.Activity():SyncHttpData(msg)
    elseif msg.state == EHttpActivityState.Finished then
        SingleData.Activity():SyncHttpFinish(msg)
    elseif msg.state == EHttpActivityState.FinishProgressbar then
        SingleData.Activity():SyncProgress(msg)
    end
end

---排行榜
---@param msg MSG_1301
function MessageResponse._1301(msg, data)
    SingleData.Rank():SyncMessage(msg)
end

---请求商店数据，购买商品
---@param msg MSG_1401
function MessageResponse._1401(msg, data)
    if msg.state == EHttpShopState.Pay then
        SingleData.Shop():PaySuccessful(msg)
    end
end

---好友
---@param msg MSG_1501
function MessageResponse._1501(msg, data)
    if msg.state == EFriendState.Friend then
        SingleData.Friend():SyncMessage(msg.friendInfos, EFriendState.Friend)
    elseif msg.state == EFriendState.Message then
        SingleData.Friend():SyncMessage(msg.friendInfos, EFriendState.Message)
    elseif msg.state == EFriendState.Life then
        SingleData.Friend():SyncMessage(msg.friendInfos, EFriendState.Life)
    else
        if msg.state == EFriendState.Stranger then
            SingleData.Friend():SyncMessage(msg.friendInfos, EFriendState.Stranger)
        elseif msg.state == EFriendState.Search then
            SingleData.Friend():SyncMessage(msg.friendInfos, EFriendState.Search, true)
        else
            SingleData.Friend():SyncMessage(msg.friendInfos, EFriendState.Stranger)
        end
    end
    EventDispatch:Broadcast(ViewID.Game, 3, EGamePage.FriendView, msg.state == EFriendState.Search)
end

---@param msg MSG_1502
function MessageResponse._1502(msg, data)
    if msg.type == EFriendHandleType.Friend then
        if msg.state == EFriendHandleState.OverLimit then
            if data.state == EFriendHandleState.SendAccept then
                --todo 消息列表超出上线，请取消后再邀请
                TipMessageBox.ShowUI(GetLanguageText(14022), true)
            elseif data.state == EFriendHandleState.Accept then
                TipMessageBox.ShowUI(GetLanguageText(14020), true)
            end
        else
            SingleData.Friend():RemoveMessage(msg)
            EventDispatch:Broadcast(ViewID.Game, 3, EGamePage.FriendView)
        end
    end
end


---@param msg MSG_1503
function MessageResponse._1503(msg, data)
    if msg.state == EChatState.Chat then
        SingleData.Friend():SyncChat(msg.friendId, msg.chatInfos)
    end
    EventDispatch:Broadcast(UIEvent.UI_ChatView, msg.state)
end

---@param msg MSG_1504
function MessageResponse._1504(msg, data)
    if msg.state == EFriendHandleState.OverLimit then
        --todo 当前社团超出上线，请加入其他
        TipMessageBox.ShowUI(GetLanguageText(14027), true)
    elseif msg.state == ELifeState.Life then
        SingleData.Mass():SyncLife(msg.lifeInfos)
    elseif msg.state == ELifeState.Gass then
        SingleData.Mass():SyncGass(msg.memeberInfos)
    end
end

---元宇宙操作
---@param msg MSG_1601
function MessageResponse._1601(msg, data)
    if msg.state == EFriendHandleType.battle then
        SingleData.Metauniverse():SyncServerData(msg.metaInfos)
    end
    EventDispatch:Broadcast(ViewID.MetaHome, msg.state)
end

return MessageResponse