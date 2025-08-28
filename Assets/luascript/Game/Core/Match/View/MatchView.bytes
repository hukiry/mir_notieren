---
--- Created by Administrator.
--- DateTime: 2023/5/9 21:35
---

require("Game.Core.Match.View.Item.MatchBase")
require("Game.Core.Match.View.Item.MatchNormalBase")
require("Game.Core.Match.View.Item.MatchObstacleBase")
---@class MatchView:DisplayObjectBase
local MatchView = Class(DisplayObjectBase)

function MatchView:ctor(gameObject)
    ---物品：x,y,信息
    ---@type table<number, table<number, MatchBase>>
    self.itemList = {}
    ---@type table<number, table<number, MatchBase>>
    self.itemBottomList = {}
    ---@type table<number, table<number, MatchBase>>
    self.itemFloatList = {}
    ---@type MatchOperate
    self.matchOperate = require('Game.Core.Match.View.MatchOperate').New(gameObject, self)
    ---@type MatchControl
    self.matchCtrl =  require('Game.Core.Match.View.MatchControl').New(self,  self.matchOperate)
    self.itemResPath = "prefab/Scene/GridItem"
    self.fourPoint  = { Vector2Int.New(-1,0), Vector2Int.New(-1,1), Vector2Int.New(0,1) }
    self.scriptTab = {
        [1]=  "MatchColorItem",       --颜色类型
        [2]=  "MatchBombItem",        --炸弹
        [3]=  "MatchDragonflyItem",   --竹蜻蜓
        [4]=  "MatchRocketItem",     --火箭
        [5]=  "MatchRainbowItem",     --彩虹球
        [6]=  "MatchMoveObstacle",   --移动障碍物
        [7]=  "MatchFixedObstacle",  --固定障碍物
        [8]=  "MatchFloorObstacle",  --草地障碍物
        [9]=  "MatchBornObstacle",   --固定生产障碍物
        [10]= "MatchAnimalObstacle",--动物
        [11]= "MatchCloudObstacle", --漂浮物障碍物
        [12]= "MatchBoxObstacle",   --颜色盒子障碍物
        [13]= "MatchBowlObstacle",  --碗柜障碍物
        [14]= "MatchJarObstacle",   --瓶柜障碍物
    }
end

---初始化子视图， 注册事件，计时器
function MatchView:OnStart()
    EventDispatch:Register(self, UIEvent.Match_AITip_Shake_View, self._TipShake)--AI提示
    EventDispatch:Register(self, UIEvent.Match_Shake_View, self.OnShake)--点击物品振动

    --EventDispatchLua:Register(self, UIEvent.Match_Remove_View, self.RemoveMatchItem)--移除物品
    EventDispatch:Register(self, UIEvent.Match_RemoveFloat_View, self._RemoveFloatItem)--移除漂浮物品
    EventDispatch:Register(self, UIEvent.Match_RemoveBottom_View, self._RemoveBottomItem)--移除底层物品
    self.matchOperate:OnStart()
    self.totalTime = 0
    Single.TimerManger():RemoveHandler(self)
    Single.TimerManger():DoTime(self, self.Timer, 1, -1)

end

---每次显示视图：初始化对象，
function MatchView:OnEnable()
    self.isLoadAnimation = true

    local mapInfo, bottomInfo, floatInfo = Single.Match():GetMapInfo()
    for _, tab in pairs(mapInfo) do
        for _, v in pairs(tab) do
            self:CreateMergeItem(v)
        end
    end

    for _, tab in pairs(bottomInfo) do
        for _, v in pairs(tab) do
            self:CreateMergeItem(v,nil, EMapLayerView.Bottom)
        end
    end

    for _, tab in pairs(floatInfo) do
        for _, v in pairs(tab) do
            self:CreateMergeItem(v, nil, EMapLayerView.Float)
        end
    end

end

function MatchView:Timer()
    if not self.isLoadAnimation then
        self.totalTime = self.totalTime + 1
        if self.totalTime >= 5 then
            self.totalTime = 0
            local tipCoord = Single.Match():GetMapAI():AI_TipPlayer(self)
            EventDispatch:Broadcast(UIEvent.Match_AI_Tip_View, tipCoord)
        end
    end
end

---动画完成
function MatchView:FinishLoadAnimation()
    self.isLoadAnimation = false
end

---物品提示振动
function MatchView:_TipShake(x, y, isInitTimer)
    if isInitTimer then self.totalTime = 0 end

    if x and y then
        ---@type MatchBase
        local itemV = self.itemList[x] and self.itemList[x][y]
        if itemV then
            itemV.transform:DOKill(true)
            itemV.transform:DOScale(Vector3.one*1.1,0.5):OnComplete(HandleParams(function(item)
                item.transform:SetScale(Vector3.one)
            end, itemV))
        end
    end
end

---创建物品：由视图操作创建
---@param info MatchInfo 新的数据
---@param oldPos UnityEngine.Vector3
function MatchView:CreateMergeItem(info, oldPos, state)
    state = state or EMapLayerView.None
    if (info:IsJar() or info:IsBowl()) and (self.itemList[info.x] and self.itemList[info.x][info.y]) then
        return self.itemList[info.x][info.y]
    end
    ---@type MatchBase
    local item = self:CreateGo(info, oldPos)
    self:AddMergeItem(item, info.x, info.y, state)
    return item
end

---创建视图
---@param info MatchInfo 新的数据
---@param oldPos UnityEngine.Vector3
---@return MatchBase
function MatchView:CreateGo(info, oldPos)
    local go = GameObjectPool.Get(self.itemResPath, self.gameObject)
    go.name = tostring(info.x).."_"..tostring(info.y).."_"..tostring(info.itemId)
    local scriptName = self.scriptTab[info:GetCfgInfo().scriptType]
    ---@type MatchBase
    local item = require('Game.Core.Match.View.Item.'..scriptName).New(go)
    item:OnEnable(self.itemResPath)
    item:UpdateInfo(info, oldPos)
    return item
end

---添加视图
---@param item MatchBase
---@param state EMapLayerView
function MatchView:AddMergeItem(item, x, y, state)
    if state == EMapLayerView.Bottom then
        if self.itemBottomList[x] == nil then
            self.itemBottomList[x] = {}
        end
        self.itemBottomList[x][y] = item
    elseif state == EMapLayerView.Float then
        if self.itemFloatList[x] == nil then
            self.itemFloatList[x] = {}
        end
        self.itemFloatList[x][y] = item
    else
        if self.itemList[x] == nil then
            self.itemList[x] = {}
        end
        self.itemList[x][y] = item
    end
end

---将物品添加到场景
function MatchView:ChangeMapItem(ox, oy, nx, ny, item)
    if ox and oy then--是否有旧视图
        if self.itemList[ox] then
            self.itemList[ox][oy] = nil
        end
    end

    if self.itemList[nx] == nil then
        self.itemList[nx] = {}
    end
    self.itemList[nx][ny] = item
end

---直接移除物品
function MatchView:RemoveMatchItem(x, y)
    local item = self.itemList[x] and self.itemList[x][y]
    if item then
        self.itemList[x][y] = nil
        if item.info:IsNormal() then--道具或颜色
            Single.Match():GetMapTarget():UpdateTask(ETaskType.Normal, item.info:GetCfgInfo().color)
        end
        item:OnRemoveView()
    end
end

---移除道具，不播放特效
function MatchView:RemoveProps(x, y)
    if self.itemList[x] then
        self.itemList[x][y] = nil
    end
end

---移除漂浮物品
function MatchView:_RemoveFloatItem(x, y)
    local item = self.itemFloatList[x] and self.itemFloatList[x][y]
    self.itemFloatList[x][y] = nil
    if item then item:OnRemoveView() end
end

---移除草地物品
function MatchView:_RemoveBottomItem(x, y)
    local item = self.itemBottomList[x] and self.itemBottomList[x][y]
    self.itemBottomList[x][y] = nil
    if item then item:OnRemoveView() end
end

---消除边界障碍物，道具可以消除泡泡和草地
---@param isWipeCoord boolean 如果false 不可以消除泡泡和草地
function MatchView:RefreshNear(x, y, col, isWipeCoord, isDragonfly)
    ---障碍物品
    local item = self.itemList[x] and self.itemList[x][y]
    if isDragonfly and (item and item.info:IsNormal()) then
        item = nil
    elseif item then
        local info = item.info
        if info and info:GetCfgInfo():IsBarrier() then--障碍物
            info:WipeOnce(col, x, y)
            local barrierType = item.info:GetCfgInfo().barrierType
            if barrierType == EObstacleType.Animal then--动物(下落后，已经处理)
                return
            elseif barrierType == EObstacleType.Born then--产出信封
                item:FinishObstacle(info:GetFlyIcon(), true, true)
            else
                ---消除障碍物完成
                if info:IsFinish() then
                    item:FinishObstacle()
                    self:RemoveMatchItem(x, y)
                else
                    ---没有完成的，播放叶子特效，更新叶子数据
                    item:PlayEffect(x, y, col)
                    item:UpdateInfo(info)
                end
            end
        end
    end

    ---草地上无其他障碍物
    if item == nil and isWipeCoord then
        ---漂浮物品
        item = self.itemFloatList[x] and self.itemFloatList[x][y]
        if item == nil then--草地物品
            item = self.itemBottomList[x] and self.itemBottomList[x][y]
        end

        if item then
            item.info:WipeOnce(col)
            item:PlayEffect(x, y, col)
        end
    end
    --检查碗和瓶子消除完成
    if item == nil then
        local itemFunction = function(_x, _y)
            local itemR = self:GetMatchItem(_x, _y)
            if itemR and (itemR.info:IsBowl() or itemR.info:IsJar()) then
                return itemR
            else
                return nil
            end
        end
        local itemV = itemFunction(x-1, y)
        if itemV == nil then itemV = itemFunction(x, y+1) end
        if itemV == nil then itemV = itemFunction(x-1, y+1) end

        if itemV then
            local nx, ny = itemV.info.x, itemV.info.y
            itemV.info:WipeOnce(col, x, y)
            if itemV.info:IsFinish() then
                itemV:FinishObstacle()
                self:RemoveMatchItem(nx, ny)
            else
                --没有完成的，播放叶子特效，更新叶子数据
                itemV:PlayEffect(x, y, col)
                itemV:UpdateInfo(itemV.info)
            end
        end
    end
end

---点击振动物品
function MatchView:OnShake(x, y)
     local checkFourGrad = function(_x, _y)
        local itemv = self:GetMatchItem(_x, _y)
        if itemv and not (itemv.info:IsJar() or itemv.info:IsBowl()) then
            return nil
        end
        return itemv
    end

    local item = self:GetMatchItem(x, y)
    if item == nil then item = self.itemBottomList[x] and self.itemBottomList[x][y] end
    if item == nil then item = self.itemFloatList[x] and self.itemFloatList[x][y] end
    if item == nil then--4个格子的检查
        for i, v in ipairs(self.fourPoint) do
            item = checkFourGrad(x+v.x,y+v.y)
            if item then break end
        end
    end
    if item then item:PlayShake(x, y) end
end

---恢复视图位置
function MatchView:OnRestorePosition(bx, by, mx, my)
    local itemBefore = self:GetMatchItem(bx, by)
    local item = self:GetMatchItem(mx, my)
    local infoBefore, info = itemBefore.info, item.info
    if itemBefore and item then
        info:SetCoord(bx, by)
        infoBefore:SetCoord(mx, my)
        itemBefore:UpdateInfo(info, itemBefore.transform.localPosition)
        item:UpdateInfo(infoBefore, item.transform.localPosition)
    end
end

---更新视图
---@param props EPropsType
---@param coordTab table<number, UnityEngine.Vector2Int> 集合 nil=邻边视图刷新
function MatchView:UpdateView(x, y, coordTab, props, col)
    if props~= EPropsType.None then
        self:BornProp(x, y, coordTab, props)
    end
    ---消除边界障碍物
    local temp = self.matchOperate.matchUtil:ConvertEdge(coordTab)
    for _, v in ipairs(temp) do
        self:RefreshNear(v.x, v.y, col, v.z == 1)
    end
end

---消除并生成道具
function MatchView:BornProp(x, y, coordTab, props)
    local info =  Single.Match():CreateProps(props, x, y)
    --生成物品并消除
    local itemTarget = self:GetMatchItem(x, y)
    local len = #coordTab
    local pos = Util.Map().IndexCoordToWorld(x, y)
    itemTarget.index = 0
    for i = 1, len do
        local v =coordTab[i]
        if v.x~=x or v.y~=y then
            local itemMerge = self.itemList[v.x] and self.itemList[v.x][v.y]
            if itemMerge then
                Single.Sound():PlaySound(ESoundResType.MergeNewItem)
                self:RemoveMatchItem(v.x, v.y)
                itemMerge:MoveAnimaton(pos, len, info, itemTarget)
            end
        end
    end
end

---下落、补充视图：全局管理
---@param xtab table<number,number> x坐标集合
function MatchView:DownRepairView(xtab)
    self.fallXTab = {}
    local topTabx = {}
    ---检查动物
    for x = -MatchRule.Size, MatchRule.Size do
        local item = self:GetMatchItem(x, -MatchRule.Size)
        local bornItem = self:GetMatchItem(x, -MatchRule.Size)
        if bornItem and bornItem.info:GetCfgInfo().barrierType == EObstacleType.Born then
            item = self:GetMatchItem(x, 1-MatchRule.Size)
        else
            item = self:GetMatchItem(x, -MatchRule.Size)
        end

        if item then
            if item.info:GetCfgInfo().barrierType == EObstacleType.Animal then
                self.itemList[item.info.x][item.info.y] = nil
                item:FinishAnimal()
            end
        end
    end

    ---下落一格，触发上面的一格
    for _, x in ipairs(xtab) do
        local isHasDown = false
        for y = -3, 4 do
            local itemBefore = self:GetMatchItem(x, y-1)
            local item = self:GetMatchItem(x, y)
            if itemBefore == nil and not self.matchCtrl:IsFourItem(x, y-1) then
                if item and item.info:IsCanMove() then
                    isHasDown=true
                    self.fallXTab[x] = 1
                    self.matchCtrl:MoveDown(item, x, y-1)
                    break
                end
            end
        end

        ---无下落物品时，检查补充
        if not isHasDown then
            table.insert(topTabx, x)
            self.fallXTab[x] = 1
        end
    end

    for _, x in ipairs(topTabx) do
        self.matchCtrl:StartRepair(x, true)
    end
end

---完成下落后，检查合成
function MatchView:RemoveDownX(x)
    self.fallXTab[x] = nil
    if table.length(self.fallXTab) == 0 then
        ----避免坐标重复消除
        log("下落完成", "pink")
        StartCoroutine(function()
            WaitForSeconds(0.1)
            self.matchOperate:CheckLoop()
        end)
    end
end

---@return MatchBase
function MatchView:GetMatchItem(x, y)
    if self.itemList[x] then
        return self.itemList[x][y]
    end
    return nil
end

---@return MatchOperate
function MatchView:GetMatchOperate()
    return self.matchOperate
end

---每次隐藏视图：撤销事件，回收对象池，撤销计时器
function MatchView:OnDisable()
    self.totalTime = 0
    Single.TimerManger():RemoveHandler(self)

    self.matchOperate:OnDisable()
    EventDispatch:UnRegister(self)

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
function MatchView:OnDestroy()
    self:OnDisable()
end

return MatchView