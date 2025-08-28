---
--- ChatInfo       
--- Author : hukiry     
--- DateTime 2024/7/17 16:13   
---

---@class ChatInfo
local ChatInfo = Class()
---@param msg GCHATINFO
function ChatInfo:ctor(msg)
    ---聊天角色id
    self.Id = msg.Id
    ---聊天时间
    self.time = msg.time
    ---聊天内容
    self.content = msg.content
end

---@return boolean
function ChatInfo:IsMe()
    return self.Id == Single.Player().roleId
end

---聊天日期
---@return string
function ChatInfo:GetTimeString()
    if Util.Time().IsSameDay(self.time, os.time()) then
        return Util.Time().GetStringFormatDate(self.time, ETimeFormat.ShortTime)
    else
        return Util.Time().GetStringFormatDate(self.time, ETimeFormat.FullTime)
    end
end

---聊天内容
---@return string
function ChatInfo:GetContent()
    return self.content
end

---@return string
function ChatInfo:GetIconName()
    if self.Id == Single.Player().roleId then
        return "role_" .. Single.Player().headId
    end
    local info = SingleData.Friend():GetFriendInfo(self.Id)
    if info then
        return "role_" .. info.headId
    end
    return "role_1"
end

---@return string
function ChatInfo:GetNick()
    local info = SingleData.Friend():GetFriendInfo(self.Id)
    if info then
        return info.nick
    end
    return ""
end

return ChatInfo