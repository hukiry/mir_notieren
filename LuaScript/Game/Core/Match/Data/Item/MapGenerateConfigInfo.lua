---
--- MapGenerateConfigInfo       
--- Author : hukiry     
--- DateTime 2023/8/11 22:43   
---


---地图配置形状
---@class EMapConfigShape
EMapConfigShape = {
    ---标记障碍物的数量，集合1-4
    flagNumbers = 1,
    ---地图数据，集合1-81
    map = 2
}

---地图配置信息
---@class EMapConfigInfo
EMapConfigInfo = {
    ---@type number
    ---地图等级限制
    Lv = 1,
    ---@type table<number>
    ---生成普通物品颜色范围 1-6
    Colors = 2,
    ---@type table<number>
    ---生成的障碍物的集合id范围
    Obstacles =3,
    ---@type function
    ---设定限制移动数量：如果障碍物总数超过可移动的数量，根据障碍物总数/2
    GetMoveNumber = 4,
    ---@type table<number,number,{}>
    ---指定障碍物的数量：随机中生成包含，则计算在内 ；嵌套集合
    CustomItems = 5,
    ---@type EMapConfigShape
    ---生成障碍物的形状限制范围，决定了障碍物的数量, 地图配置范围
    MapShapes = 6,
}


---@class MapGenerateConfigInfo
local MapGenerateConfigInfo = Class()

function MapGenerateConfigInfo:ctor()
    ---关卡地图形状
    self.mapShape = require('LuaConfig.Map.MapShape')

    ---@type table<number, EMapConfigInfo>
    self.mapInfo = {}
    table.insert(self.mapInfo, {
        Lv = 5,
        Colors = {1, 3},
        Obstacles =  {3201, 3202, 3203, 3204, 3602, 3317, 3501, 3341, 3342, 3343,3101},
        GetMoveNumber =  function() return math.random(35, 40) end,
        CustomItems = {
            ---罐子, 蛋, 西瓜, 石雕
            [3201] = 45, [3202] = 35, [3203] = 55, [3204] = 20, [3101] = 37,
        },
        MapShapes = { 1, 2}
    })

    table.insert(self.mapInfo, {
        Lv = 50,
        Colors = {2, 5},
        Obstacles =  {3201, 3202, 3203, 3204, 3601, 3602, 3317, 3501, 3101, 3401, 3342, 3343, 3344},
        GetMoveNumber =  function() return math.random(30, 35) end,
        CustomItems = {
            ---罐子, 蛋, 西瓜, 石雕，动物，信封
            [3201] = 31, [3202] = 33, [3203] = 37, [3204] = 20,
            ---born
            [3101] = 37, [3102] = 23, [3103] = 29,[3104] = 31,
            ---动物
            [3401] = 11, [3402] = 13, [3403] = 17,[3404] = 19
        },
        MapShapes = { 3, 7}
    })

    table.insert(self.mapInfo, {
        Lv = 100,
        Colors = {3, 6},
        Obstacles =  {3316,3310, 3317, 3201, 3202, 3204,3601, 3602, 3101, 3501, 3601, 3102, 3402, 3342, 3343, 3344, 3345, 3312, 3313},
        GetMoveNumber =  function() return math.random(32, 45) end,
        CustomItems = {
            ---罐子, 西瓜
            [3201] = 45, [3203] = 70,
            ---born
            [3101] = 47, [3102] = 33, [3103] = 39,[3104] = 41,
            ---动物
            [3401] = 21, [3402] = 23, [3403] = 27,[3404] = 29
        },
        MapShapes = { 8, 14}
    })

    table.insert(self.mapInfo, {
        Lv = 200,
        Colors = {1, 4},
        Obstacles =  {3316, 3317, 3310, 3201, 3202, 3203, 3204, 3502, 3101, 3601, 3602, 3103, 3403,3313,3314,3315},
        GetMoveNumber =  function() return math.random(43, 55) end,
        CustomItems = {
            ---罐子, 蛋, 西瓜, 石雕
            [3201] = 55, [3202] = 45, [3203] = 65, [3204] = 30,
            ---born
            [3101] = 57, [3102] = 43, [3103] = 49,[3104] = 51,
            ---动物
            [3401] = 31, [3402] = 33, [3403] = 37,[3404] = 39
        },
        MapShapes = { 15, 21}
    })

    table.insert(self.mapInfo, {
        Lv = 350,
        Colors = {2, 5},
        Obstacles =  {3310, 3311, 3312, 3313, 3314, 3202, 3204, 3317, 3101, 3501, 3601,3104, 3404,3313,3314,3311,3312},
        GetMoveNumber =  function() return math.random(54, 65) end,
        CustomItems = {
            ---蛋, 石雕
            [3202] = 60, [3204] = 85,
            ---born
            [3101] = 67, [3102] = 53, [3103] = 59,[3103] = 61,
            ---动物
            [3401] = 41, [3402] = 43, [3403] = 47,[3404] = 49
        },
        MapShapes = { 22, 30}
    })

    table.insert(self.mapInfo, {
        Lv = 700,
        Colors = {3, 6},
        Obstacles =  {3310, 3311, 3312, 3313, 3314, 3202, 3204, 3317, 3101, 3501, 3601, 3104, 3405,3312,3313,3314,3315},
        GetMoveNumber =  function() return math.random(42, 60) end,
        CustomItems = {
            ---罐子, 蛋, 西瓜, 石雕
            [3201] = 65, [3202] = 55, [3203] = 65, [3204] = 50,
            ---born
            [3101] = 60, [3102] = 63, [3103] = 69,[3103] = 71,
            ---动物
            [3401] = 51, [3402] = 53, [3403] = 57,[3404] = 59,[3405] = 51
        },
        MapShapes = { 1, 14}
    })

    table.insert(self.mapInfo, {
        Lv = 1500,
        Colors = {1, 4},
        Obstacles =  {3310, 3311, 3312, 3313, 3314, 3202, 3204, 3317, 3101, 3501, 3601,3104, 3405, 3314, 3315},
        GetMoveNumber =  function() return math.random(75, 85) end,
        CustomItems = {
            ---罐子, 蛋, 西瓜, 石雕
            [3201] = 71, [3202] = 73, [3203] = 75, [3204] = 78,
            ---born
            [3101] = 75, [3102] = 73, [3103] = 79,[3104] = 81,
            ---动物
            [3401] = 61, [3402] = 63, [3403] = 67,[3404] = 69,[3405] = 61
        },
        MapShapes = { 15, 30}
    })

    table.sort(self.mapInfo, function(a, b) return a.Lv<b.Lv end)
end

---获取地图配置
---@param lv number
---@return EMapConfigInfo
function MapGenerateConfigInfo:GetMapConfigInfo(lv)
    local info = nil
    for i, v in ipairs(self.mapInfo) do
        if lv < v.Lv then
            info = v
            break
        end
    end
    return info
end

---获取地图标记数据
---@param mapShapes table<number> 地图所以范围
---@return EMapConfigShape
function MapGenerateConfigInfo:GetMapConfigShape(mapShapes)
    local index = math.random(mapShapes[1], mapShapes[2])
    local flagClassName = "flag"..index
    local result = self.mapShape[flagClassName]
    if result then
        return require(string.format("LuaConfig.Map.%s", flagClassName))
    end
    return nil
end

return MapGenerateConfigInfo