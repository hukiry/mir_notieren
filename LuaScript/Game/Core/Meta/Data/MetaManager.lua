--- 编辑数据管理
--- MetaManager       
--- Author : hukiry     
--- DateTime 2023/9/13 20:21   
---

---@class MetaManager
local MetaManager = Class()
function MetaManager:ctor()
    ---配置数据
    ---@type MetaConfig
    self.metaConfig = require("Game.Core.Meta.Data.MetaConfig").New()
    ---@type MetaCacheData
    self.cacheData = require("Game.Core.Meta.Data.MetaCacheData").New()
    self.cacheData:InitData(self)
end

---登出游戏清除数据
function MetaManager:InitData()
    self.metaConfig:InitData()
    ---格子字典信息： x, y,info
    ---@type table<number, table<number, TileInfo>>
    self.infoList = {}
    ---底部物品
    ---@type table<number, table<number, TileInfo>>
    self.infoBottomList = {}
    ---漂浮物品
    ---@type table<number, table<number, TileInfo>>
    self.infoFloatList = {}

    ---地图编辑当前编号
    ---@type number
    self.numberId = 1

    ---仅火箭使用
    self.isVertical = false
    ---选择的物品id
    self.selectItemId = 0
    ---@type ESelectState
    self.selectMode = ESelectState.AddItem
    ---由物品选择改变
    ---@type EMapLayerView
    self.selectLayer = EMapLayerView.None
    ---是否可以撤回
    self.isResetDo = false
    ---撤回数据
    self.resetDoInfo = {}
end

---加载地图
function MetaManager:LoadEditorMap()
    ---物品id数据，用于物品选择过滤
    ---@type table<number, number>
    self.idList = {}

    --1，根据地图编号，无此编号就是新地图，否则为旧地图数据
    self.selectItemId = self.cacheData:LoadEditorData(self.numberId)
end

---创建信息
---@param x number 新x坐标
---@param y number 新y坐标
---@param itemId number 物品id
function MetaManager:CreateInfo(itemId, x, y)
    ---@type TileInfo
    local info = require("Game.Core.Meta.Data.Info.TileInfo").New()
    local cfg = self.metaConfig:GetMatchConfig():GetItemInfo(itemId)
    local layerView = EMapLayerView.None
    if cfg.barrierType == EObstacleType.Glass then
        layerView = EMapLayerView.Bottom
    elseif cfg.barrierType == EObstacleType.Bubble then
        layerView = EMapLayerView.Float
    end

    info:SetInfo(itemId, x, y, layerView)
    self:_UpdateInfo(info)

    if self.idList[itemId] == nil then
        self.idList[itemId] =  0
    end
    self.idList[itemId] = self.idList[itemId] + 1
    return info
end

---移除数据
---@param x number 新x坐标
---@param y number 新y坐标
---@return TileInfo
function MetaManager:RemoveInfo(x, y, selectLayer)
    local infoTable = self:_GetTableInfo(selectLayer)
    local result = nil
    if infoTable[x] then
        result = infoTable[x][y]
        if result then
            self.idList[result.itemId] = self.idList[result.itemId] - 1
            if self.idList[result.itemId] <= 0 then
                self.idList[result.itemId] = nil
            end
        end

        infoTable[x][y] = nil
    end
    return result
end

---获取数据
---@param x number 新x坐标
---@param y number 新y坐标
---@return TileInfo
function MetaManager:GetInfo(x, y)
    local infoTable = self:_GetTableInfo(self.selectLayer)
    if infoTable[x] then
        return infoTable[x][y]
    end
    return nil
end

---@private
---@param info TileInfo
function MetaManager:_UpdateInfo(info)
    local infoTable = self:_GetTableInfo(info.sortLayer)
    if infoTable[info.x] == nil then
        infoTable[info.x] = {}
    end
    infoTable[info.x][info.y] = info
end

---@private
function MetaManager:_GetTableInfo(state)
    if state == EMapLayerView.Bottom then
        return self.infoBottomList
    elseif state == EMapLayerView.Float then
        return self.infoFloatList
    else
        return self.infoList
    end
end

---计算障碍物是否已满，不可超过4个
---@return number
function MetaManager:CaculateObstacleCount()
    local obsTab = {}
    local obsFunc = function(tab, _obsTab)
        for _, vTab in pairs(tab) do
            for _, v in pairs(vTab) do
                if _obsTab[v.itemId] == nil and v.itemType == EItemType.Obstacle then
                    if v.indexId == 331 or v.indexId == 334 then
                        _obsTab[3310] = 1
                    else
                        _obsTab[v.itemId] = 1
                    end
                end
            end
        end
    end

    obsFunc(self.infoList, obsTab)
    obsFunc(self.infoBottomList, obsTab)
    obsFunc(self.infoFloatList, obsTab)
    local results = table.toArrayKey(obsTab)
    return #results
end

---当前编辑的地图信息
---@return MapInfo
function MetaManager:GetMapInfo() return self.cacheData:GetMapInfo(self.numberId) end
---获取3个物品坐标集合
function MetaManager:GetMataInfoList() return self.infoList, self.infoBottomList, self.infoFloatList end
---@return MetaConfig
function MetaManager:GetMataConfig() return self.metaConfig end
---获取缓存类，保存数据
---@return MetaCacheData
function MetaManager:GetCacheData() return self.cacheData end

function MetaManager:GetMapLength() return #Single.Player().metaMapIds end

---@return ItemCfgInfo
function MetaManager:GetSelectInfo()
    if self.selectItemId>0 then
        return self.metaConfig:GetMatchConfig():GetItemInfo(self.selectItemId)
    end
    return nil
end

---获取分类物品
---@return table<number, table<number, number>> 字段：物品类型，id， 1
function MetaManager:GetClassificationItem()
    local tempList,colorTab = {},{}
    for id, v in pairs(self.idList) do
        if v>0 then
            local info = self.metaConfig:GetMatchConfig():GetItemInfo(id)
            if tempList[info.itemType] == nil then
                tempList[info.itemType] = {}
            end
            tempList[info.itemType][id] = info

            if info.color > 0 then
                if colorTab[info.color] == nil then
                    colorTab[info.color] = {}
                end
                colorTab[info.color][id] = 1
            end
        end
    end
    return tempList, colorTab
end

---获取目标障碍物数量
---@return table<number, number> id, num
function MetaManager:GetObstacleNumbers(isCanMove)
    isCanMove  = isCanMove or false
    local tempList = {}
    for id, v in pairs(self.idList) do
        if v>0 then
            local info = self.metaConfig:GetMatchConfig():GetItemInfo(id)
            if info.itemType == EItemType.Obstacle then
                local idbox,  itemId = math.floor(id/10), info.itemId
                if idbox==331 or idbox == 334 then  itemId = 3310 end

                if isCanMove and info.barrierType ~= EObstacleType.Move and info.barrierType ~= EObstacleType.Born then
                    itemId = 0
                end

                if itemId > 0 then
                    if tempList[itemId] == nil then tempList[itemId] = 0 end
                    tempList[itemId] = tempList[itemId] + v
                end
            end
        end
    end

    self:GetMapInfo():CheckTarget(tempList)
    return tempList
end

---设置上次操作的数据
function MetaManager:SetResetdoInfo(itemId, x, y, isHorizontal, isHave)
    self.resetDoInfo.itemId = itemId
    self.resetDoInfo.x = x
    self.resetDoInfo.y = y
    self.resetDoInfo.isHorizontal = isHorizontal
    self.resetDoInfo.isHave = isHave
    self.resetDoInfo.selectLayer = self.selectLayer
end

---获取需要撤回的数据
function MetaManager:GetResetdoInfo()
    return self.resetDoInfo
end

return MetaManager