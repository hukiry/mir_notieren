--- 数据配置
--- MatchData       
--- Author : hukiry     
--- DateTime 2023/5/16 10:10   
---

---@class MatchConfig
local MatchConfig = Class()
function MatchConfig:ctor()

end

---登出游戏清除数据
function MatchConfig:InitData()
    ---物品信息：id， 数据
    ---@type table<number, ItemCfgInfo>
    self.infoList = {}
    ---物品信息：物品类型， id，数据
    ---@type table<EItemType, table<number,ItemCfgInfo>>
    self.typeList = {}

    ---物品信息：道具类型， id，数据
    ---@type table<EPropsType, ItemCfgInfo>
    self.propsList = {}

    ---障碍物信息：道具类型， id，数据
    ---@type table<EObstacleType, table<number,ItemCfgInfo>>
    self.obstacleList = {}

    local tab = SingleConfig.Item():GetTable()
    for _, v in pairs(tab) do
        ---@type ItemCfgInfo
        local info = require("Game.Core.Match.Data.Item.ItemCfgInfo").New(v.itemId)
        self.infoList[info.itemId]= info
        if self.typeList[info.itemType] == nil then
            self.typeList[info.itemType] =  {}
        end

        self.typeList[info.itemType][info.itemId] = info
        self.propsList[info.itemProps] = info

        if self.obstacleList[info.barrierType]==nil then
            self.obstacleList[info.barrierType] = {}
        end
        self.obstacleList[info.barrierType][info.itemId] = {}
    end
end

---获取物品类型集合
---@param type EItemType
---@return table<number,ItemCfgInfo> 集合
function MatchConfig:GetItemArray(type)
    local t = self.typeList[type]
    return table.toArray(t)
end

---获取道具信息
---@param type EPropsType
---@return ItemCfgInfo 数据信息
function MatchConfig:GetPropsInfo(type)
    return self.propsList[type]
end

---获取物品信息
---@param itemId number
---@return ItemCfgInfo 数据信息
function MatchConfig:GetItemInfo(itemId)
    return self.infoList[itemId]
end

---获取物品信息
---@param type EObstacleType
---@return ItemCfgInfo 数据信息
function MatchConfig:GetObstacleArry(type)
    local t = self.obstacleList[type]
    return table.toArray(t)
end

return MatchConfig

