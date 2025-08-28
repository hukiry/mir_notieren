--- 编辑数据缓存
--- MetaCacheData
--- Created by Administrator.
--- DateTime: 2023/9/15 11:12
---


---@class MetaCacheData:DisplayObjectBase
local MetaCacheData = Class(DisplayObjectBase)
local META_MAPEDITOR = "META_MAP"
function MetaCacheData:ctor()
    ---地图生成:这里只用于协议结构
    ---@type MatchMapGenerate
    self.matchMap = require("Game.Core.Match.Data.MatchMapGenerate").New()
end

---登出游戏清除数据
function MetaCacheData:InitData(metaManager)
    ---@type MetaManager
    self.metaMgr = metaManager
    ---图片数据集合：地图编号，集合数组
    ---@type table<number, table<number, SpriteMeshInfo>>
    self.spriteInfoList = {}
    ---地图信息集合
    ---@type table<number, MapInfo>
    self.infoList = {}
end

---返回结构体
---@return table<number>
function MetaCacheData:ToMessageBody()
    local temp = {
        ---总移动次数
        totalMove = protobuf_type.byte,
        ---仅用于创建和编辑地图
        colorNum = protobuf_type.byte,
        ---可编辑障碍物数量
        obstacleNum = protobuf_type.byte,
        ---更新时间
        updateTime = protobuf_type.uint32,
        ---地图名称
        mapName =  protobuf_type.string,
        ---地图描述
        mapDesc = protobuf_type.string,
        ---地图编辑作者
        author = protobuf_type.string,
        ---颜色集合
        colors_IsArray = true,
        colors = protobuf_type.byte,

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

        ---可移动的障碍物
        jsonItems_IsArray = true,
        jsonItems = {
            ---障碍物Id：可移动
            itemId = protobuf_type.int16,
            ---障碍物数量
            itemNum = protobuf_type.byte,
            xTab_IsArray = true,
            ---下落的列坐标集合
            xTab = protobuf_type.byte
        }
    }
    return temp
end

---计算出地图数据
---@private
function MetaCacheData:_CaculateMapGrids()
    local tabList = {}
    local itemFunc = function(x, y, layerView, tab)
        local infoTab = self.metaMgr:_GetTableInfo(layerView)
        local info = infoTab[x] and infoTab[x][y]
        if info then
            if layerView == EMapLayerView.None then tab.isHorizontal = info.isHorizontal end
            return info.itemId
        else
            return 0
        end
    end

    for x = -MatchRule.Size, MatchRule.Size do
        for y = -MatchRule.Size, MatchRule.Size do
            local info = { itemId=0, itemId_float=0, itemId_bottom = 0, isHorizontal = false}
            info.x, info.y = x, y
            info.itemId = itemFunc(x, y, EMapLayerView.None, info)
            info.itemId_bottom = itemFunc(x, y, EMapLayerView.Bottom)
            info.itemId_float = itemFunc(x, y, EMapLayerView.Float)
            table.insert(tabList, info)
        end
    end
    return tabList
end

---加载地图编辑数据
---@param numberId number 读取编号
function MetaCacheData:LoadEditorData(numberId)
    local readItemId = 101
    local infoMap = self.infoList[numberId]

    if infoMap then
        for _, v in ipairs(infoMap.grids) do
            if v.itemId_float>0 then self.metaMgr:CreateInfo(v.itemId_float, v.x, v.y) end
            if v.itemId_bottom>0 then self.metaMgr:CreateInfo(v.itemId_bottom, v.x, v.y) end
            if v.itemId>0 then
                local info = self.metaMgr:CreateInfo(v.itemId, v.x, v.y)
                info.isHorizontal = v.isHorizontal
                readItemId = info.itemId
            end
        end
    elseif self.infoList[numberId] == nil then
        self.infoList[numberId] = require("Game.Core.Meta.Data.Info.MapInfo").New()
    end
    return readItemId
end

---读取编辑数据
---@param numberId number 读取编号
function MetaCacheData:ReadEditorData(numberId)
    ---读取地图数据
    if self.infoList[numberId] == nil then
        self.infoList[numberId] = require("Game.Core.Meta.Data.Info.MapInfo").New()
    end

    local keyName = self:_GetMapKeyName(numberId)
    ---@type MapInfo
    local msgTab = self:ReadBinaryTable(keyName, self:ToMessageBody())
    if msgTab and #msgTab.grids>0 then
        self.infoList[numberId]:UpdateData(msgTab)
        ---获取地图网格数据
        if self.spriteInfoList[numberId] == nil then
            self.spriteInfoList[numberId] = self.infoList[numberId]:GetSpriteInfoArray()
        end
    end
end

---保存编辑地图数据
function MetaCacheData:SaveEditorData()
    local tabList = self:_CaculateMapGrids()
    local msgTab = protobuf.ConvertMessage(self:ToMessageBody())
    local numberId = self.metaMgr.numberId

    self.metaMgr:GetMapInfo():WriteData(msgTab, tabList)
    self.infoList[numberId]:UpdateData(msgTab)
    self.spriteInfoList[numberId] = self.infoList[numberId]:GetSpriteInfoArray()

    local keyName = self:_GetMapKeyName(numberId)
    self:SaveBinaryTable(keyName, msgTab)
end

---@param info MapInfo
function MetaCacheData:SaveEditorInfo(info, numberId)
    local msgTab = protobuf.ConvertMessage(self:ToMessageBody())
    for key, v in pairs(info) do
        if msgTab[key] then
            msgTab[key] = v
        end
    end
    local keyName = self:_GetMapKeyName(numberId)
    self:SaveBinaryTable(keyName, msgTab)
end

---获取地图网格数据：用于绘制图片
---@param mapNumber number 地图编号
---@return table<number, SpriteMeshInfo>
function MetaCacheData:GetImageData(mapNumber)
    local infoTab = self.spriteInfoList[mapNumber]
    if infoTab == nil then
        --读取本地数据
        self:ReadEditorData(mapNumber)
        infoTab = self.spriteInfoList[mapNumber]
        --根据用户元宇宙数据，提前下载好地图
    end
    return infoTab or {}
end

---@private
function MetaCacheData:_GetMapKeyName(numberId)
    return Single.Player().roleId ..META_MAPEDITOR .. numberId
end

---获取地图信息
---@param numberId number
---@return MapInfo
function MetaCacheData:GetMapInfo(numberId)
    if self.infoList[numberId] == nil then
        self:ReadEditorData(numberId)
    end
    return self.infoList[numberId]
end

return MetaCacheData