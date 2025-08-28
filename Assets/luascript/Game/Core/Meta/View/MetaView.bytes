--- 编辑视图管理
--- MetaSenceView       
--- Author : hukiry     
--- DateTime 2023/9/13 20:12   
---

---@class MetaView:DisplayObjectBase
local MetaView = Class(DisplayObjectBase)

function MetaView:ctor(gameObject, lineMap)
    ---@type MetaOperate
    self.matchOperate = require('Game.Core.Meta.View.MetaOperate').New(lineMap, self)
end

---初始化子视图， 注册事件，计时器
function MetaView:OnStart()
    self.matchOperate:OnStart()

    self.itemResPath = "prefab/Scene/MetaItem"

    ---物品：x,y,信息
    ---@type table<number, table<number, TileItem>>
    self.itemList = {}

    ---@type table<number, table<number, TileItem>>
    self.itemBottomList = {}

    ---@type table<number, table<number, TileItem>>
    self.itemFloatList = {}
end

---每次显示视图：初始化对象，
function MetaView:OnEnable()
    local mapInfo, bottomInfo, floatInfo = Single.Meta():GetMataInfoList()
    for x, tab in pairs(mapInfo) do
        for y, v in pairs(tab) do
            self:CreateItem(v)
        end
    end

    for x, tab in pairs(bottomInfo) do
        for y, v in pairs(tab) do
            self:CreateItem(v)
        end
    end

    for x, tab in pairs(floatInfo) do
        for y, v in pairs(tab) do
            self:CreateItem(v)
        end
    end
end

---创建视图
---@param info TileInfo 新的数据
---@return TileItem
function MetaView:_CreateGo(info)
    local go = GameObjectPool.Get(self.itemResPath, self.gameObject)
    go.name = tostring(info.x).."_"..tostring(info.y).."_"..tostring(info.itemId)
    ---@type TileItem
    local item = require('Game.Core.Meta.View.TileItem').New(go)
    item:OnEnable(self.itemResPath)
    item:UpdateInfo(info)
    return item
end

---创建物品：由视图操作创建
---@param info TileInfo 新的数据
function MetaView:CreateItem(info, isOperate)
    ---@type MatchBase
    local item = self:_CreateGo(info)
    local x, y, itemTemTab = info.x, info.y, self:GetViewTable(info.sortLayer)
    if itemTemTab[x] == nil then
        itemTemTab[x] = {}
    end
    itemTemTab[x][y] = item

    if isOperate then
        Single.Sound():PlaySound(ESoundResType.MergeNewItem)
        item:GetAnimation():PlayEffect(info:GetWorldPos(), ESceneEffectRes.Born_props)
    end
end

---移除视图：视图操作
function MetaView:RemoveItem(x, y, stateLayer, isOperate)
    local itemTemTab = self:GetViewTable(stateLayer)
    if itemTemTab[x] and itemTemTab[x][y] then
        local itemTem = itemTemTab[x][y]

        if isOperate then
            Single.Sound():PlaySound(ESoundResType.item_poshui)
            ---播放消耗效果
            local info = itemTem.info
            local pos = Util.Map().IndexCoordToWorld(x, y)
            self:PlayExplosion(info:GetIcon(), pos)
        end

        itemTem:OnDisable()
        itemTemTab[x][y] = nil
    end
end

---获取层物品表
---@return table<number, table<number, TileItem>>
function MetaView:GetViewTable(stateLayer)
    if stateLayer == EMapLayerView.Bottom then
        return self.itemBottomList
    elseif stateLayer == EMapLayerView.Float then
        return self.itemFloatList
    else
        return self.itemList
    end
end

---爆炸效果
---@param spriteName string 精灵图标
---@param pos UnityEngine.Vector3 指定当前位置
---@param finishCall function
function MetaView:PlayExplosion(spriteName, pos, finishCall)
    local resPath = "prefab/Scene/ExplosionItem"
    ---@type UnityEngine.GameObject
    local go = GameObjectPool.Get(resPath, self.gameObject)
    go.transform:SetPosition(pos)

    local paramsAction = HandleParams(function(goParams, finishCallParams)
        local forceCall = function() return math.random(20,100)  end
        local radiusCall = function() return math.random(1,5)  end
        GameObject.Destroy(goParams)
        --ExplosionForce.Instance:PlayExplosion(goParams, forceCall, radiusCall, math.random(6,8))
        if finishCallParams then
            finishCallParams()
        end
        StartCoroutine(function()
            WaitForFixedUpdate()
            ResManager:Unload(resPath)
        end)
    end, go, finishCall)

    ---@type CommonSprite
    local common = CommonSprite.New(go:GetComponent("SpriteRenderer"), nil, true)
    common.resetBoxCollider2D = true
    common:LoadSprite(spriteName, nil, paramsAction)
end

---每次隐藏视图：撤销事件，回收对象池，撤销计时器
function MetaView:OnDisable()
    self.matchOperate:OnDisable()

    for _, vTab in pairs(self.itemList) do
        for _, v in pairs(vTab) do
            GameObjectPool.Put(self.itemResPath, v.gameObject)
        end
    end
    self.itemList = {}

    for _, vTab in pairs(self.itemBottomList) do
        for _, v in pairs(vTab) do
            GameObjectPool.Put(self.itemResPath, v.gameObject)
        end
    end
    self.itemBottomList = {}

    for _, vTab in pairs(self.itemFloatList) do
        for _, v in pairs(vTab) do
            GameObjectPool.Put(self.itemResPath, v.gameObject)
        end
    end
    self.itemFloatList = {}
end

---消耗资源
function MetaView:OnDestroy()
    self:OnDisable()

end

return MetaView