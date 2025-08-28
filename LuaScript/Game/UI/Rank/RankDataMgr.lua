---
--- RankDataMgr       
--- Author : hukiry     
--- DateTime 2023/7/17 17:22   
---

---@class RankDataMgr
local RankDataMgr = Class()
function RankDataMgr:ctor()

end

--清理数据
function RankDataMgr:InitData()
    self.rankType = 1
    ---字典集合
    ---@type table<number, table<number, RankInfo>>
    self.infoList = {}

    ---请求标签
    ---@type table<EHttpRankType, number>
    self.httpTypeTab = {}
end

---同步服务器消息
---@param msg MSG_1301
function RankDataMgr:SyncMessage(msg)
    self.rankType = msg.type
    if self.infoList[msg.type] == nil then
        self.infoList[msg.type] = {}
    end

    self.httpTypeTab[msg.type] = 1

    if msg.jsonDatas == '' then
        return
    end
    local tabList = json.decode(msg.jsonDatas)
    for _, v in ipairs(tabList) do
        ---@type RankInfo
        local info = require("Game.UI.Rank.RankInfo").New(v)
        self.infoList[self.rankType][info.roleId] = info
    end
end

---@param type EHttpRankType
---@return boolean
function RankDataMgr:IsHasType(type)
    return self.httpTypeTab[type] ~= nil
end

---@param id number
---@return RankInfo
function RankDataMgr:GetMassInfo(ty, id)
    if self.infoList[ty] then
        return self.infoList[ty][id]
    end
    return nil
end

---获取排行榜数据集合
---@return table<number, RankInfo>
function RankDataMgr:GetMassArray(rankType)
    if self.infoList[rankType] == nil then  return nil end
    local array = table.toArray(self.infoList[rankType])
    if #array>1 then
        table.sort(array, function(a, b) return a.level > b.level end)
    end

    for i, v in ipairs(array) do
        v.number = i
    end
    return array
end

return RankDataMgr