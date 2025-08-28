---
--- MatchDataInfo       
--- Author : hukiry     
--- DateTime 2023/5/16 10:14   
---

---@class ItemCfgInfo
local ItemCfgInfo = Class()

function ItemCfgInfo:ctor(itemId)
    self.itemId = itemId
    local info = self:GetCfg()
    ---道具名称
    ---@type number
    self.nameId = info.name
    ---对应颜色
    ---@type EColorItem
    self.color = info.color
    ---消除总次数
    ---@type number
    self.count = info.count
    ---物品类型
    ---@type EItemType
    self.itemType = info.itemType
    ---道具类型
    ---@type EPropsType
    self.itemProps = info.itemStyle
    ---障碍物类型
    ---@type EObstacleType
    self.barrierType = info.barrierType
    ---图标
    ---@type string
    self.icon = info.icon
    ---罐子颜色集合
    ---@type table<number>
    self.jarColors = info.jarColors
    ---目标显示图标
    ---@type string
    self.targetIcon = info.targetIcon
    ---分类页签类型
    ---@type number
    self.metaPage = info.metaPage
    ---脚本分类
    ---@type number
    self.scriptType = info.scriptType

    ---元图标
    ---@type string
    self.metaIcon = info.metaIcon
    ---是元宇宙数据
    self.isMeta = info.metaPage>0
end

---是障碍物类型
function ItemCfgInfo:IsBarrier()
    return self.itemType == EItemType.Obstacle
end

---是有消除次数：可以被消除的，动物和信封是没有消除物的
function ItemCfgInfo:IsWipeBarrier()
    return self.count>0 and self.itemType == EItemType.Obstacle
end

---在漂浮层
function ItemCfgInfo:IsFloat()
    return self.barrierType == EObstacleType.Bubble
end

---在底部层
function ItemCfgInfo:IsBottom()
    return self.barrierType == EObstacleType.Glass
end

---@private
---@return TableItemItem
function ItemCfgInfo:GetCfg()
    return SingleConfig.Item():GetKey(self.itemId)
end

---@return TableCurrencyItem
function ItemCfgInfo:GetCurrency()
    if self.itemProps == EPropsType.None then
        return nil
    end
    local key = EPropsBuyCount[self.itemProps]%10
    return SingleConfig.Currency():GetKey(key)
end

---获取层视图
---@return EMapLayerView
function ItemCfgInfo:GetLayerView()
    local layerView = EMapLayerView.None
    if self.barrierType == EObstacleType.Glass then
        layerView = EMapLayerView.Bottom
    elseif self.barrierType == EObstacleType.Bubble then
        layerView = EMapLayerView.Float
    end
    return layerView
end

---获取物品名称
function ItemCfgInfo:GetName()
    return GetLanguageText(self.nameId)
end

return ItemCfgInfo
