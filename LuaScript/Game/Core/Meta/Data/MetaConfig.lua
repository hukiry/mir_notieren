--
--- MetaConfig、
--- Author : hukiry     
--- DateTime 2023/10/11 10:51   
---
---@class MetaConfig
local MetaConfig = Class()
function MetaConfig:ctor()
    ---配置数据
    ---@type MatchConfig
    self.matchConfig = require("Game.Core.Match.Data.MatchConfig").New()
end

---登出游戏清除数据
function MetaConfig:InitData()
    self.matchConfig:InitData()
    ---字典数据：页签索引，物品id，物品数据
    ---@type table<number, table<number, ItemCfgInfo>>
    self.itemList = {}
    --初始化配置表数据
    for _, v in pairs(self.matchConfig.infoList) do
        if v.isMeta then
            if self.itemList[v.metaPage] == nil then
                self.itemList[v.metaPage] = {}
            end
            self.itemList[v.metaPage][v.itemId] = v
        end
    end
end

---@return MatchConfig
function MetaConfig:GetMatchConfig() return self.matchConfig end

---@param indexPage number
---@return table<number, ItemCfgInfo> 集合
function MetaConfig:GetPageArray(indexPage)
    local temp = table.toArray(self.itemList[indexPage])
    table.sort(temp, function(a, b) return a.itemId<b.itemId end)
    if indexPage == 2 then
        local info = Clone(self.itemList[indexPage][203])
        info.isVertical = true
        table.insert(temp, info)
    end
    return temp
end

return MetaConfig
