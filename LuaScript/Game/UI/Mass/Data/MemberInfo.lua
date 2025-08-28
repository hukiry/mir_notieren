---
--- MemberInfo       
--- Author : hukiry     
--- DateTime 2024/7/22 14:09   
---

---@class MemberInfo
local MemberInfo = Class()
---@param msg GMEMEBERINFO
function MemberInfo:ctor(msg)
    ---用户id
    self.roleId = msg.roleId
    ---等级
    self.level = msg.level
    ---昵称
    self.nick = msg.nick
    ---头像
    self.headId = msg.headId
    ---是团主
    self.isMasser = msg.isMasser
end


return MemberInfo