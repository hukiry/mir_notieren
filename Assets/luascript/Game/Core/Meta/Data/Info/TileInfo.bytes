--- 编辑格子信息
--- TileInfo       
--- Author : hukiry     
--- DateTime 2023/9/13 20:37   
---

---@class TileInfo
local TileInfo = Class()

function TileInfo:ctor()
    ---地图上的id
    self.itemId = 0

    ---地图x坐标
    ---@type number
    self.x = 0

    ---地图y坐标
    ---@type number
    self.y = 0
end

---设置格子的信息
---@param sortLayer EMapLayerView
function TileInfo:SetInfo(itemId, x, y, sortLayer)
    self.itemId = itemId
    self.x = x
    self.y = y

    ---排序层级
    ---@type EMapLayerView
    self.sortLayer = sortLayer

    ---用于id区分
    self.indexId = math.floor(itemId/10)


    local info = self:GetCfgInfo()
    ---物品类型
    ---@type EItemType
    self.itemType = info.itemType

    ---当前消除次数
    self.count = info.count

    self.isHorizontal = false
end

---@return string
function TileInfo:GetIcon()
    local info = self:GetCfgInfo()
    if info.barrierType == EObstacleType.Fixed then
        return info.icon
    end

    if info:IsWipeBarrier() and info.count>1  then
        return info.icon .. "_"..(self.count-1)
    end

    local spriteName = self.isHorizontal and "horizontal_rocket" or "vertical_rocket"
    if info.itemProps == EPropsType.Rocket then
        return spriteName
    end
    return info.icon
end

---@return ItemCfgInfo
function TileInfo:GetCfgInfo()
    return Single.Meta():GetMataConfig():GetMatchConfig():GetItemInfo(self.itemId)
end

---获取物品名称
function TileInfo:GetName()
    return self:GetCfgInfo():GetName()
end

function TileInfo:GetWorldPos()
    return Util.Map().IndexCoordToWorld(self.x, self.y)
end

return TileInfo

