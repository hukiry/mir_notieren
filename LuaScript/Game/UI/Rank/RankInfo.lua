---
--- RankInfo       
--- Author : hukiry     
--- DateTime 2023/7/17 17:26   
---

---@class RankInfo
local RankInfo = Class()
function RankInfo:ctor(data)
    self.roleId = data.roleId
    ---昵称
    self.roleNick =  data.roleNick
    ---等级
    self.level =  data.level
    ---语言国家
    self.lanCode =  data.lanCode

    ---头像id
    self.headId = data.headId
    ---团队名称
    self.massName = data.massName
    ---排序编号
    self.number = 0
end

function RankInfo:GetIcon()
    local id = self.headId <=0 and 1 or self.headId
    return "role_"..id
end

return RankInfo