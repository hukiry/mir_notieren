---
--- FriendDataMgr
--- Author : hukiry     
--- DateTime 2023/7/14 10:38   
---

---@class FriendDataMgr:DisplayObjectBase
local FriendDataMgr = Class(DisplayObjectBase)
function FriendDataMgr:ctor()

end

--清理数据
function FriendDataMgr:InitData()
    ---好友列表
    ---@type table<EFriendState, table<number, FriendInfo>>
    self.infoList = {}
    ---挑战状态列表
    ---@type table<number, number>
    self.challengeList = {}
    ---请求标志符
    self.reqTag = {}
    ---聊天信息：房间，聊天内容
    ---@type table<number,  table<number, ChatInfo>>
    self.chatInfoList = {}
end

---同步消息列表
---@param messages table<number, GFRIENDINFO>
---@param state EFriendState
---@param isSearch number 记录视图的显示
function FriendDataMgr:SyncMessage(messages, state, isSearch)
    self.isSearch = isSearch or false
    self.infoList[state] = {}
    for i, v in ipairs(messages) do
        self.infoList[state][v.roleId] = require("Game.UI.Friend.Data.FriendInfo").New(v)
    end

    self.reqTag[state] = 1
end

---同步聊天
---@param chatInfos table<number,GCHATINFO>
function FriendDataMgr:SyncChat(friendId, chatInfos)
    self.chatInfoList[friendId] = {}
    for _, v in ipairs(chatInfos) do
        local info = require("Game.UI.Friend.Data.ChatInfo").New(v)
        table.insert(self.chatInfoList[friendId], info)
    end
end

---@param state EFriendState
---@return boolean
function FriendDataMgr:IsRequested(state)
    return self.reqTag[state] ~= nil
end

function FriendDataMgr:IsRequestedFriendState(pageIndex)
    local state = EFriendState.Stranger
    if pageIndex == 1 then
        state = EFriendState.Message
    elseif pageIndex == 2 then
        state = EFriendState.Friend
    end
    return self:IsRequested(state)
end

---好友已上线，不可再邀请
function FriendDataMgr:IsLimitFriend()
    if self.infoList[EFriendState.Friend] then
        return false
    end
    return table.length(self.infoList[EFriendState.Friend]) >= 30
end

---获取好友信息
---@return FriendInfo
function FriendDataMgr:GetFriendInfo(id)
    if self.infoList[EFriendState.Friend] then
        return self.infoList[EFriendState.Friend][id]
    end
    return nil
end

---获取商品信息
---@param pageIndex number
---@return table<number, FriendInfo>
function FriendDataMgr:GetFriendArray(pageIndex, isSearch)
    local state = EFriendState.Stranger
    if pageIndex == 1 then
        state = EFriendState.Message
    elseif pageIndex == 2 then
        state = EFriendState.Friend
    elseif isSearch then
        state = EFriendState.Search
    end

    if self.infoList[state] then
        local array = table.toArray(self.infoList[state])
        if #array>1 then
            table.sort(array, function(a, b)
                if a==nil or b==nil then
                    return false
                end
                return a.level > b.level
            end)
        end
        return array
    end
    return {}
end

---获取聊天记录
---@param friendId number 房间id
---@return table<number, ChatInfo>
function FriendDataMgr:GetChatArray(friendId)
    local array = self.chatInfoList[friendId]
    if #array>1 then
        table.sort(array, function(a, b)
            return a.time < b.time
        end)
    end
    return array
end

---移除消息
---@param msg MSG_1502
function FriendDataMgr:RemoveMessage(msg)
    local state = msg.state
    if msg.type == EFriendHandleType.Friend then
        if state == EFriendHandleState.Accept then
            local info = self.infoList[EFriendState.Message][msg.friendId]
            if self.infoList[EFriendState.Friend] ==nil then
                self.infoList[EFriendState.Friend] = {}
            end
            info.state = EFriendHandleState.Accept
            self.infoList[EFriendState.Friend][msg.friendId] = info
            self.infoList[EFriendState.Message][msg.friendId] = nil
        elseif state == EFriendHandleState.RefuseAccept then
            self.infoList[EFriendState.Message][msg.friendId] = nil
        elseif state == EFriendHandleState.SendAccept then
            local info = self.infoList[EFriendState.Stranger][msg.friendId]
            if self.infoList[EFriendState.Message] ==nil then
                self.infoList[EFriendState.Message] = {}
            end
            if info then
                info.state = EFriendHandleState.SendAccept
                self.infoList[EFriendState.Message][msg.friendId] = info
            end
            self.infoList[EFriendState.Stranger][msg.friendId] = nil
        elseif state == EFriendHandleState.CancelAccept then
            self.infoList[EFriendState.Message][msg.friendId] = nil
        elseif state == EFriendHandleState.Delete then
            if self.challengeList[msg.friendId] == EFriendHandleState.Accept then
                --todo 正在挑战中，不可以删除好友
                TipMessageBox.ShowUI(GetLanguageText(14016))
                return
            end
            self.infoList[EFriendState.Friend][msg.friendId] = nil
            self.challengeList[msg.friendId] = nil
            ---移除聊天记录
            self.chatInfoList[msg.friendId] = nil
        end
    else
        if msg.type == EFriendHandleType.Challenge then
            self.challengeList[msg.friendId] = state
        end
    end
end

---缓存状态
---@param stateType number 发送life请求=1, 发送聊天=2
---@param isSave boolean 默认为 false
---@param hour number 超过的时间
function FriendDataMgr:CaculateState(stateType, isSave, hour)
    local KeyName = "FRIENDData"
    local tempTab = {}
    isSave = isSave or false
    local messageBody = {
        stateTypes_IsArray = true,
        stateTypes = {
            curTime = protobuf_type.uint32,
            stateType = protobuf_type.byte,
        }
    }
    local data = self:ReadBinaryTable(KeyName, messageBody)
    local isCreate = false
    if data and data.stateTypes and #data.stateTypes>0 then
        for _, v in ipairs(data.stateTypes) do
            tempTab[v.stateType] = v
        end

        if tempTab[stateType] and tempTab[stateType].curTime then
            if hour then
                isCreate = Util.Time().IsOverHour(tempTab[stateType].curTime, os.time(), hour)
            else
                isCreate = not Util.Time().IsSameDay(tempTab[stateType].curTime, os.time())
            end
        else
            isCreate = true
        end
    else
        isCreate = true
    end

    if isCreate and isSave then
        tempTab[stateType] = {
            curTime = os.time(),
            stateType = stateType
        }
        local msgData = protobuf.ConvertMessage(messageBody)
        for _, v in pairs(tempTab) do
            local msg = {}
            msg.curTime = v.curTime
            msg.stateType = v.stateType
            table.insert(msgData.stateTypes, msg)
        end
        self:SaveBinaryTable(KeyName, msgData)
    end
    return isCreate, tempTab[stateType]~=nil and tempTab[stateType].curTime or 0
end

return FriendDataMgr