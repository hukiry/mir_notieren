---
--- SortMapPrefabData       
--- Author : hukiry     
--- DateTime 2024/7/8 15:15   
---

---预先排序数据
---@class SortMapPrefabData
local SortMapPrefabData = Class()
function SortMapPrefabData:ctor()
    self.map1 = {
        {1,2,1,3,1,2,3,1,2},
        {1,1,3,1,2,1,1,2,1},
        {2,1,1,2,1,1,2,1,1},
        {1,2,1,3,1,2,3,1,2},
        {3,1,3,1,2,3,1,2,3},
        {1,3,1,1,3,1,1,3,1},
        {1,2,1,3,1,2,3,1,1},
        {3,1,3,1,2,1,1,2,3},
        {2,3,1,2,3,1,2,1,1}
    }

    self.map2 = {
        {1,2,3,2,1,3,2,4,2},
        {3,1,2,1,2,3,1,2,3},
        {4,3,1,2,3,4,2,3,1},
        {1,1,3,4,1,3,2,4,2},
        {3,1,2,3,4,3,1,2,3},
        {4,3,1,2,3,4,2,3,1},
        {1,1,3,4,1,3,2,2,1},
        {3,1,2,3,4,3,1,1,2},
        {1,3,1,3,3,4,2,2,1}
    }

    self.map3 = {
        {1,2,1,4,3,2,3,4,1},
        {2,3,4,1,2,3,4,1,2},
        {3,2,3,4,3,2,3,2,1},
        {1,2,3,4,1,2,3,4,1},
        {2,3,4,1,3,3,4,1,2},
        {3,2,1,2,3,4,2,2,1},
        {1,2,3,4,1,2,3,4,1},
        {2,3,4,1,2,3,4,1,2},
        {3,4,1,2,3,4,1,2,3}
    }
end

function SortMapPrefabData:LoadData()
    if Single.Player():GetMoneyNum(EMoneyType.level) <50 then
        ---@type table<number,table<number,number>>
        self.mapGrid = self.map1
    else
        ---@type table<number,table<number,number>>
        self.mapGrid = self:_GetPrefabMap()
    end
end

---@param x number x>0 索引1开始
---@param y number y>0 索引1开始
---@param minCol number 颜色标签
---@return number
function SortMapPrefabData:GetColorID(x, y, minCol)
    local col  = self.mapGrid[x][y]
    if minCol<=1 then
        return col
    else
        local offset = minCol-1
        return col + offset
    end
end

---@private
---@return table<number,table<number,number>>
function SortMapPrefabData:_GetPrefabMap()
    local flag = math.random(1,2)
    return self["map"..flag]
end

return SortMapPrefabData