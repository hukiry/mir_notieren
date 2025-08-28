--- 社团消息
--- MassInfo       
--- Author : hukiry     
--- DateTime 2023/7/17 17:27   
---

---@class FriendInfo
local FriendInfo = Class()
---@param msg GFRIENDINFO
function FriendInfo:ctor(msg)
    ---用户id
    self.roleId = msg.roleId
    ---1=申请好友，2=接受好友，3=邀请好友挑战，4=接受挑战
    ---@type EFriendHandleState
    self.state = msg.state
    ---等级
    self.level = msg.level
    ---昵称
    self.nick = msg.nick
    ---头像
    self.headId = msg.headId
    ---好友邀请地图
    self.mapId = msg.mapId
end

function FriendInfo:GetHeadIcon()
    if self.headId <= 0 then
        self.headId = 1
    end
    return "role_"..self.headId
end

function FriendInfo:IsCancel()
    return self.state == EFriendHandleState.SendAccept or self.state == EFriendHandleState.ReceiveAccepted
end

function FriendInfo:IsClick()
    return self.state == EFriendHandleState.None or self.state == EFriendHandleState.ReceiveAccepted or self.state == EFriendHandleState.Friend
end

function FriendInfo:IsFriend()
    return self.state == EFriendHandleState.Friend
end

function FriendInfo:GetCancelTxt()
    if self.state == EFriendHandleState.SendAccept then
        return GetLanguageText(14008) --发起显示取消
    else
        return GetLanguageText(14010)--受邀显示拒绝
    end
end

function FriendInfo:GetClickTxt()
    if self.state == EFriendHandleState.SendAccept then
        return GetLanguageText(14007) --已邀请
    elseif self.state == EFriendHandleState.ReceiveAccepted then
        return GetLanguageText(14009)--接受
    elseif self.state == EFriendHandleState.Friend then
        return GetLanguageText(14011)--删除
    else
        return GetLanguageText(14014)--邀请
    end
end



return FriendInfo