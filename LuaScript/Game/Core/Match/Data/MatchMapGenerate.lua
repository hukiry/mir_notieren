---
--- 地图数据生成
--- MatchMapRandom       
--- Author : hukiry     
--- DateTime 2023/5/16 13:31   
---

---@class EMapData
EMapData = {
    x = 1,
    y = 1,
    itemId=2,
    itemId_float = 3,
    itemId_bottom = 4,
    isHorizontal = 5
}

---可移动障碍物数量
---@class EMapObstacleData
EMapObstacleData = {
    ---障碍物Id
    ---@type number
    itemId = 1,
    ---目标数量
    ---@type number
    itemNum = 2,
    ---x下落坐标集合
    ---@type table<number>
    xTab = 3
}

---@class MatchMapGenerate
local MatchMapGenerate = Class()
function MatchMapGenerate:ctor(matchData)
    ---@type MatchConfig
    self.matchConfig = matchData
end

---登出游戏清除数据
function MatchMapGenerate:InitData()
    ---@type MapGenerateConfigInfo
    self.configInfo = require("Game.Core.Match.Data.Item.MapGenerateConfigInfo").New()
    ---@type SortMapPrefabData
    self.prefabData = require("Game.Core.Match.Data.SortMapPrefabData").New()
    self.MAX = MatchRule.Size
    self.MIX = -MatchRule.Size
end

---返回结构体
---@return table<number>
function MatchMapGenerate:ToMessageBody()
    local temp = {
        grids_IsArray = true,
        ---地图坐标数据
        grids = {
            x = protobuf_type.int16,
            y = protobuf_type.int16,
            itemId = protobuf_type.int16,
            itemId_float = protobuf_type.int16,
            itemId_bottom = protobuf_type.int16,
            isHorizontal = protobuf_type.bool
        },

        targetIds_IsArray = true,
        ---目标障碍物集合
        targetIds = {
            itemId = protobuf_type.int16,
            itemNum = protobuf_type.int16
        },
        ---颜色范围
        colorMax = protobuf_type.byte,
        colorMin = protobuf_type.byte,
        ---总移动次数
        totalMove = protobuf_type.byte,
        jsonItems_IsArray = true,
        jsonItems = {
            ---障碍物Id：可移动
            itemId = protobuf_type.int16,
            ---障碍物数量
            itemNum = protobuf_type.byte,
            xTab_IsArray = true,
            ---下落的列坐标集合
            xTab = protobuf_type.byte
        },
        ---颜色id
        normalId= protobuf_type.byte,
        ---对应任务id
        taskItemId= protobuf_type.byte,
    }
    return temp
end


---地图数据，障碍物id和数量（字典），颜色集合
---@return table<number, EMapData>, table<number, number>, table<EColorItem>, table<number>
function MatchMapGenerate:CreateMap(lv)
    ---地图数据
    ---@type table<number, EMapData>
    local mapTab = {}
    self.prefabData:LoadData()
    ---@type EMapConfigInfo
    local info = self.configInfo:GetMapConfigInfo(lv)
    ---地图标记数据 用于计算
    ---@type EMapConfigShape
    local mapData = self.configInfo:GetMapConfigShape(info.MapShapes)
    ---{障碍物id,数量,下落坐标集合};{目标id和数量 字典},{普通物品}
    local jsonItems, targetIDs, minColor = {}, {}, info.Colors[1]
    local Obstacles = table.toArray(info.Obstacles)

    local taskInfo = Single.AutoTask():GetTaskInfo()
    local taskItemId, normalId = taskInfo.itemId, 0

    local colid = taskItemId>106 and (taskItemId%100-6) or (taskItemId%10)
    local tempBornAnimal = {}
    local x, y, tempObs = 0, 0, {}
    for i = self.MIX, self.MAX do
        x = i+4
        for j = self.MIX, self.MAX do
            y = j + 5
            ---@type EMapData
            local temp = {}
            temp.x = j
            --y轴需要由上而下
            temp.y = -i
            temp.itemId = 100 + self.prefabData:GetColorID(i + 5,j + 5, minColor)--随机物品
            temp.itemId_bottom = 0
            temp.itemId_float = 0
            ---确定任务颜色值
            if normalId == 0 then
                if colid>=minColor and colid<=info.Colors[2] then
                    if temp.itemId%10 == colid then
                        normalId = temp.itemId
                    end
                else
                    normalId = temp.itemId
                end
            end

            if normalId==temp.itemId and taskInfo.itemType == EItemType.Normal then
                temp.itemId = taskItemId
            end

            ---生成障碍物的索引位置
            local index = x*9+y
            local indexFlag = mapData.map[index]
            if indexFlag>0 then
                if tempObs[indexFlag] == nil then
                    local pos = math.random(1, #Obstacles)
                    tempObs[indexFlag] = table.remove(Obstacles, pos)
                    if taskInfo.itemType == EItemType.Obstacle and taskItemId > 0 then
                        tempObs[indexFlag] = taskItemId
                        taskItemId = 0
                    end
                end

                local id = tempObs[indexFlag]
                local cfg = self.matchConfig:GetItemInfo(id)
                if cfg:IsFloat() then
                    temp.itemId_float = id
                elseif cfg:IsBottom() then
                    temp.itemId_bottom = id
                else
                    if cfg.barrierType == EObstacleType.Animal then
                        if tempBornAnimal[id]==nil then tempBornAnimal[id] = 0 end
                        if temp.y == 2 or temp.y == 3 then
                            temp.itemId = id
                            tempBornAnimal[id] = tempBornAnimal[id] + 1
                        end
                    elseif cfg.barrierType == EObstacleType.Born then
                        if tempBornAnimal[id]==nil then tempBornAnimal[id] = 0 end
                        if temp.x == -4 or temp.x == 4 or temp.y == -4 then
                            temp.itemId = id
                            tempBornAnimal[id] = tempBornAnimal[id] + 1
                        end
                    else
                        local idIndex = math.floor(id/10)
                        if (idIndex==331 or idIndex==334) and cfg.color>0 then
                            if tempBornAnimal[id]==nil then tempBornAnimal[id] = 0 end
                            if self:IsColorRange(taskItemId, minColor, info.Colors[2]) then--任务id在颜色范围内
                                if self:IsColorRange(id, minColor, info.Colors[2]) then
                                    temp.itemId = id
                                    tempBornAnimal[id] = tempBornAnimal[id] + 1
                                end
                            elseif taskItemId>200 and self:IsColorRange(id, minColor, info.Colors[2]) then--任务id不在颜色范围内
                                temp.itemId = id
                                tempBornAnimal[id] = tempBornAnimal[id] + 1
                            end
                        else
                            temp.itemId = id
                        end
                    end
                end

                ---目标障碍物数量统计
                if targetIDs[id] == nil then
                    targetIDs[id] = 0
                end

                if tempBornAnimal[id] then
                    ---动物 ，信封
                    targetIDs[id] = tempBornAnimal[id]
                else
                    targetIDs[id] = targetIDs[id] + 1
                end
            end
            table.insert(mapTab, temp)
        end
    end

    self:BornJsonItems(targetIDs, info, jsonItems)

    if taskInfo.itemType ~= EItemType.Normal then
        normalId = 0
    end
    return mapTab, targetIDs, info.Colors, jsonItems, info.GetMoveNumber(), {normalId, taskItemId}
end

---@param targetIDs table<number, number>
---@param jsonItems table<number, EMapObstacleData>
---@param info EMapConfigInfo
function MatchMapGenerate:BornJsonItems(targetIDs, info, jsonItems)
    local lenObs, bornTab = table.length(targetIDs),{}
    for id, number in pairs(targetIDs) do
        local cfg = self.matchConfig:GetItemInfo(id)
        local isContinue = true
        if cfg and (cfg.barrierType == EObstacleType.Born or cfg.color>0) then
            if number == 0 then
                table.insert(bornTab, id)
                isContinue = false
            end
        end

        if info.CustomItems[id] and isContinue then
            local targNum = number
            --自定义数量大于地图的标记数量，那么就给定自定义的数量
            if number<info.CustomItems[id] then
                targNum = info.CustomItems[id]
            end

            ---目标障碍物下落坐标统计
            local xtab, tempX, len = {},{}, math.random(1,3)
            if lenObs == 3 then len = math.random(1,2)
            elseif lenObs >= 4 then len = 1 end

            if len == 1 then
                local indexX = math.random(-MatchRule.Size, MatchRule.Size)
                tempX[indexX] = 0
            else
                for _ = 1, len do
                    local indexX = math.random(-MatchRule.Size, MatchRule.Size)
                    tempX[indexX] = 0
                end
            end


            xtab = table.toArrayKey(tempX)
            ---记录下落坐标
            table.insert(jsonItems, {
                itemId = id,
                itemNum = targNum,
                xTab = xtab
            })
        end
    end

    ---过滤信封站,颜色障碍物
    for i, vId in ipairs(bornTab) do
        targetIDs[vId] = nil
    end
end

function MatchMapGenerate:IsColorRange(itemId, minCol, maxCol)
    local cfg = self.matchConfig:GetItemInfo(itemId)
    if cfg and cfg.color >0 then
        if cfg.color>=minCol and cfg.color<=maxCol then
            return true
        end
    end
    return false
end

return MatchMapGenerate