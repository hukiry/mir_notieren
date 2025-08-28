---
--- MassDataMgr       
--- Author : hukiry     
--- DateTime 2024/7/21 17:37   
---

---@class MassDataMgr:DisplayObjectBase
local MassDataMgr = Class(DisplayObjectBase)
function MassDataMgr:ctor()

end

--清理数据
function MassDataMgr:InitData()
    ---生命状态列表
    ---@type table<number, LifeInfo>
    self.lifeList = {}
    ---@type table<number, MemberInfo>
    self.massList = {}
end

---同步生命
---@param chatInfos table<number, GLIFEINFO>
function MassDataMgr:SyncLife(chatInfos)
    self.lifeList = {}
    for _, v in ipairs(chatInfos) do
        ---@type LifeInfo
        local info = require("Game.UI.Mass.Data.LifeInfo").New(v)
        self.lifeList[info.Id] = info
    end
end

---同步生命
---@param memeberInfos table<number, GMEMEBERINFO>
function MassDataMgr:SyncGass(memeberInfos)
    self.massList = {}
    for _, v in ipairs(memeberInfos) do
        ---@type MemberInfo
        local info = require("Game.UI.Mass.Data.MemberInfo").New(v)
        self.massList[info.roleId] = info
    end
end

---@return boolean
function MassDataMgr:IsRequestLife()
    local info = self.lifeList[Single.Player().roleId]
    if info then
        return true
    end
    return false
end


---获取生命帮助列表
---@return table<number, LifeInfo>
function MassDataMgr:GetLifeArray()
    local array = table.toArray(self.lifeList)
    if #array>1 then
        table.sort(array, function(a, b)
            return a.time > b.time
        end)
    end
    return array
end

---获取社团列表
---@return table<number, MemberInfo>
function MassDataMgr:GetMassArray()
    local array = table.toArray(self.massList)
    if #array>1 then
        table.sort(array, function(a, b)
            return a.level > b.level
        end)
    end
    return array
end

return MassDataMgr