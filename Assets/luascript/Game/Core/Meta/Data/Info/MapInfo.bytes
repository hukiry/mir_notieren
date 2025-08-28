--- 元宇宙地图信息
--- MapInfo       
--- Author : hukiry     
--- DateTime 2023/10/14 10:06   
---

---@class MapInfo
local MapInfo = Class()

function MapInfo:ctor()
    ---总移动次数
    self.totalMove = 25
    ---仅用于创建和编辑地图
    self.colorNum = 4
    ---可编辑障碍物数量
    self.obstacleNum = 4
    ---更新时间
    self.updateTime = 0
    ---地图名称
    self.mapName = ""
    ---地图描述
    self.mapDesc = ""
    ---地图编辑作者
    self.author = ""
    ---颜色集合
    self.colors = {}
    ---地图坐标数据
    ---@type table<number, EMapData>
    self.grids = {}
    ---可移动的障碍物
    ---@type table<number, EMapObstacleData>
    self.jsonItems = {}
end

function MapInfo:UpdateData(msgTab)
    for key, v in pairs(msgTab) do
        if self[key] then
            self[key] = v
        end
    end
end

---获取目标障碍物：仅游戏对战和测试
---@return table<number, number> 字典：id，num
function MapInfo:GetTargetArray()
    local targetIds, tempColors, obstacleIds = {},{},{}
    local matchConfig = Single.Meta():GetMataConfig():GetMatchConfig()
    local targetFunc = function(itemId)
        if itemId > 0 then
            local info = matchConfig:GetItemInfo(itemId)
            if info.itemType == EItemType.Obstacle then
                local id,  indexId= info.itemId, math.floor(info.itemId/10)
                if indexId == 331 or indexId == 334 then id = 3310 end

                if targetIds[id] == nil then targetIds[id] = 0 end
                targetIds[id] = targetIds[id] + 1
            elseif info.itemType == EItemType.Normal and info.color>0 then
                tempColors[info.color] = info.itemId
            end
        end
    end
    ---地图上的目标数量
    for _, v in ipairs(self.grids) do
        targetFunc(v.itemId)
        targetFunc(v.itemId_bottom)
        targetFunc(v.itemId_float)
    end

    ---目标数量
    for _, v in ipairs(self.jsonItems) do
        local targetNum = targetIds[v.itemId]
        if targetNum and targetNum<v.itemNum then
            targetIds[v.itemId] = v.itemNum
            obstacleIds[v.itemId] = {remainNum = v.itemNum-targetNum, xTab = v.xTab}
        end
    end

    ---匹配颜色
    local array = table.toArrayKey(tempColors)
    local lenCol = self.colorNum-#array
    if lenCol > 0 then
        for i = 1, lenCol do
            for col = 1, 6 do
                if tempColors[col] == nil then
                    tempColors[col] = col+100
                    break
                end
            end
        end
    end

    return targetIds, table.toArray(tempColors), obstacleIds
end

---校正下落目标障碍物
function MapInfo:CheckTarget(tempList)
    local array = {}
    for i, v in ipairs(self.jsonItems) do
        if tempList[v.itemId] then
            table.insert(array, v)
        end
    end
    self.jsonItems = array
end

---获取网格数据图标
---@private
---@param itemId number 物品id
---@param isHorizontal boolean
---@return string
function MapInfo:_GetMetaIcon(itemId, isHorizontal)
    isHorizontal = isHorizontal or false
    local info = Single.Meta():GetMataConfig():GetMatchConfig():GetItemInfo(itemId)
    local spriteName = isHorizontal and "horizontal_rocket" or "vertical_rocket"
    if info.itemProps == EPropsType.Rocket then
        return spriteName
    end
    return info.metaIcon
end

---获取网格数据
---@return table<SpriteMeshInfo>
function MapInfo:GetSpriteInfoArray()
    local spriteInfoArray = {}
    for _, v in ipairs(self.grids) do
        ---网格生成使用
        local id = v.itemId>0 and v.itemId or (v.itemId_float>0 and v.itemId_float or v.itemId_bottom)
        if id>0 then
            local info = SpriteMeshInfo.New()
            info.spriteName = self:_GetMetaIcon(id, v.isHorizontal)
            info.centerPos = Util.Map().IndexCoordToWorld(v.x, v.y)
            info.sort = 8-v.y
            table.insert(spriteInfoArray, info)
        end
    end
    return spriteInfoArray
end

---写入到数据
---@param msgTab
function MapInfo:WriteData(msgTab, tabList)
    ---字段部分
    self.author = Single.Player().roleNick
    self.updateTime = os.time()
    msgTab.totalMove, msgTab.updateTime = self.totalMove,  self.updateTime
    msgTab.colors, msgTab.colorNum  = self.colors,self.colorNum
    msgTab.obstacleNum, msgTab.author = self.obstacleNum, self.author
    msgTab.mapName, msgTab.mapDesc = self.mapName, self.mapDesc

    for _, v in ipairs(self.jsonItems) do
        local jsonMsg = {}
        jsonMsg.itemId = v.itemId
        jsonMsg.itemNum = v.itemNum
        jsonMsg.xTab = v.xTab
        table.insert(msgTab.jsonItems, jsonMsg)
    end

    for _, v in ipairs(tabList) do
        local item = {}
        item.x = v.x
        item.y = v.y
        item.isHorizontal = v.isHorizontal
        item.itemId = v.itemId
        item.itemId_float = v.itemId_float
        item.itemId_bottom = v.itemId_bottom
        table.insert(msgTab.grids, item)
    end
    self.grids = msgTab.grids
end

---获取地图id
---@param numberId number
---@return string
function MapInfo:GetMapIdText(numberId)
    local idstr = string.format("ID:<b>%s</b>",numberId)
    return  GetLanguageText(16102)..idstr
end

---获取时间
---@return string
function MapInfo:GetTimeText()
    local timeStr = Util.Time().GetStringFormatDate(self.updateTime, ETimeFormat.ShortData)
    local str = string.format(":<b>%s</b>",timeStr)
    return GetLanguageText(16115) .. str
end

return MapInfo