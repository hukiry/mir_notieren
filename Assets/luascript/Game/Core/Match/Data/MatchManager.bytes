---
--- 匹配管理控制
--- Created by Administrator.
--- DateTime: 2023/5/9 21:37
---

---@class ETargetItem
ETargetItem = {
    ---物品ID
    itemId = 1,
    ---物品数量
    itemNum = 2,
}
---@class ECoordObstacle
ECoordObstacle= {
    ---剩余数量
    remainNum = 1,
    ---下落的x坐标集合
    xTab = 2,
}

---@class MatchManager:DisplayObjectBase
local MatchManager = Class(DisplayObjectBase)
function MatchManager:ctor()
    ---配置数据
    ---@type MatchConfig
    self.matchConfig = require("Game.Core.Match.Data.MatchConfig").New()
    ---地图生成
    ---@type MatchMapGenerate
    self.matchMap = require("Game.Core.Match.Data.MatchMapGenerate").New(self.matchConfig)
    ---AI检查
    ---@type MatchAI
    self.matchAI = require("Game.Core.Match.Data.MatchAI").New(self)

    ---@type MatchTarget
    self.matchTarget = require("Game.Core.Match.Data.MatchTarget").New(self)
    ---关卡地图
    self.mapLevel = require('LuaConfig.Map.MapLevel')

    self.bornPropTab = {}
end

---登出游戏清除数据
function MatchManager:InitData()
    self.matchConfig:InitData()
    self.matchMap:InitData()
    self.matchAI:InitData()
    self:InitProperty()
end

function MatchManager:InitProperty()
    ---格子字典信息： x, y,info
    ---@type table<number, table<number, MatchInfo>>
    self.infoList = {}
    ---底部物品
    ---@type table<number, table<number, MatchInfo>>
    self.infoBottomList = {}
    ---漂浮物品
    ---@type table<number, table<number, MatchInfo>>
    self.infoFloatList = {}

    ---目标视图显示
    self.targetIds = {}
    ---运行累计的目标数量
    self.targetIdRun = {}
    ---物品补充颜色范围
    self.colors = {}
    ---障碍物下落坐标，id , x
    ---@type table<number, ECoordObstacle>
    self.obstacleIds = {}
    ---地图加载等级
    self.lv = 1
    ---移动总次数：高级关卡=40障碍物数 /2-4, 初级关卡=20障碍物数 /2+4
    self.totalMove = 0
    ---@type table<number, UnityEngine.Vector2>
    self.defaultCoord = {Vector2.New(0,0), Vector2.New(1,0), Vector2.New(0,-1), Vector2.New(1,-1)}
end

--region 地图部分
---加载地图数据
function MatchManager:LoadMap()
    self:InitProperty()
    self.lv = Single.Player():GetMoneyNum(EMoneyType.level)
    local dataTab = self:LoadConfig()
    if dataTab then
        self:CreateMap(dataTab)
    else
        self.matchAI:AI_CreateMap(self.lv)
    end

    self.matchTarget:StartGame()
end

---加载元地图
---@param mapInfo MapInfo 地图数据
---@param metaState EMetaFightState
function MatchManager:LoadMetaMap(metaState, mapInfo)
    self:InitProperty()
    self.matchTarget:StartGame(metaState)

    self:CreateGrid(mapInfo.grids)
    local targetIds, tempColors, obstacleIds = mapInfo:GetTargetArray()
    self.targetIds = targetIds
    self.colors = tempColors
    self.totalMove = mapInfo.totalMove

    self.obstacleIds = obstacleIds

end

---创建数据
---@param data table
function MatchManager:CreateMap(data, isAI)
    self:CreateGrid(data.grids)
    --目标数据
    if isAI then
        for _, v in ipairs(data.targetIds) do
            self.targetIds[v.itemId] = v.itemNum
        end
    else
        self.targetIds = data.targetIds
    end
    --移动次数
    self.totalMove = data.totalMove

    local col = data.normalId%100
    local colT = data.taskItemId>106 and (data.taskItemId%100-6) or (data.taskItemId%10)
    for i = data.colorMin, data.colorMax do
        local id = 100+i
        if colT>=data.colorMin and colT<=data.colorMax then
            if colT == i and col > 0 then
                table.insert(self.colors, data.taskItemId)
            else
                table.insert(self.colors, id)
            end
        else
            if i == col then
                table.insert(self.colors, data.taskItemId)
            else
                table.insert(self.colors, id)
            end
        end
    end
end

---创建地图数据
---@param grids table<number, EMapData>
function MatchManager:CreateGrid(grids)
    for i, v in ipairs(grids) do
        if v.itemId_bottom > 0 then
            self:CreateInfo(v.x, v.y, v.itemId_bottom, EMapLayerView.Bottom)
        end

        if v.itemId_float > 0 then
            self:CreateInfo(v.x, v.y, v.itemId_float, EMapLayerView.Float)
        end

        if v.itemId>0 then
            local info = self:CreateInfo(v.x, v.y, v.itemId)
            info.isHorizontal = v.isHorizontal
        end
    end
end

---加载本地数据
function MatchManager:LoadConfig()
    local nameLv = "level_"..self.lv
    if self.mapLevel[nameLv] then
        local jsonStr = require('LuaConfig.Map.'..nameLv)
        local jsonTab = json.decode(jsonStr)
        local tagertTab = {}
        ---任务目标
        local taskInfo = Single.AutoTask():GetTaskInfo()
        local taskItemId, normalId = taskInfo.itemId, 0

        for _, v in ipairs(jsonTab.grids) do
            if v.itemId_float>0 then
                if tagertTab[v.itemId_float] == nil then
                    tagertTab[v.itemId_float] = 0
                end
                tagertTab[v.itemId_float] = tagertTab[v.itemId_float] + 1
            end

            if v.itemId_bottom>0 then
                if tagertTab[v.itemId_bottom] == nil then
                    tagertTab[v.itemId_bottom] = 0
                end
                tagertTab[v.itemId_bottom] = tagertTab[v.itemId_bottom] + 1
            end

            if v.itemId>0 and self.matchConfig:GetItemInfo(v.itemId):IsBarrier() then
                if tagertTab[v.itemId] == nil then
                    tagertTab[v.itemId] = 0
                end
                tagertTab[v.itemId] = tagertTab[v.itemId] + 1
            end

            if taskInfo.itemType == EItemType.Normal and math.floor(v.itemId/100)==1 then
                if normalId == 0 then
                    normalId = v.itemId
                end
            end
        end

        jsonTab.normalId = normalId
        jsonTab.taskItemId = taskItemId

        self.obstacleIds = {}
        if string.len(jsonTab.jsonItem) > 0 then
            ---记录可移动的物品
            ---@type table<...>
            local jsonItems = json.decode(jsonTab.jsonItem)
            for _, vtab in ipairs(jsonItems) do
                local id, num, xTab = vtab[1], vtab[2], vtab[3]
                local cfg = self.matchConfig:GetItemInfo(id)
                if cfg and (cfg.barrierType == EObstacleType.Move or cfg.barrierType == EObstacleType.Animal or cfg.barrierType == EObstacleType.Born) then
                    local xtab = type(xTab) == "number" and {xTab} or xTab
                    self.obstacleIds[id] = {remainNum = num-tagertTab[id], xTab = xtab }
                    tagertTab[id] = num
                end
            end
        end

        jsonTab.targetIds = tagertTab
        return jsonTab
    end
    return nil
end

---@param tab table<number, EPropsType> 集合
function MatchManager:AddSelectProp(tab)
    self.bornPropTab = tab
end

---@param number number
function MatchManager:AddMoveCount(number)
    self.totalMove = self.totalMove + number
end
--endregion

---@param x number 新x坐标
---@param y number 新y坐标
---@param itemId number 物品id
function MatchManager:CreateInfo(x, y, itemId, state)
    state = state or EMapLayerView.None
    ---@type MatchInfo
    local info = require("Game.Core.Match.Data.Item.MatchInfo").New(itemId, self.matchConfig)
    info.sortLayer = state
    if info:IsBowl() or info:IsJar() then
        info:SetCoord(x, y)
        for _, v in ipairs(self.defaultCoord) do
            local nx, ny = x+v.x, y+v.y
            self:SetMatchInfo(nx, ny, info, state, false)
        end
    else
        self:SetMatchInfo(x, y, info, state)
    end
    return info
end

---获取颜色随机
---@return number
function MatchManager:GetColorId() return self.colors[math.random(1, #self.colors)] end

---仅创建颜色物品，或移动障碍物
function MatchManager:CreateOnly(x, y)
    local itemId = self:GetColorId()
    for id, v in pairs(self.obstacleIds) do
        local cfg = self.matchConfig:GetItemInfo(id)
        if cfg and table.contains(v.xTab, x) then
            local bType = cfg.barrierType
            if v.remainNum > 0  and (bType == EObstacleType.Move or bType == EObstacleType.Animal) then
                v.remainNum = v.remainNum - 1
                itemId = id
                break
            end
        end
    end

    ---@type MatchInfo
    local info = require("Game.Core.Match.Data.Item.MatchInfo").New(itemId, self.matchConfig)
    info:SetCoord(x, y)
    info.sortLayer = EMapLayerView.None
    return info
end

---@param nx number 新x坐标
---@param ny number 新x坐标
---@param info MatchInfo 数据
---@param state EMapLayerView 漂浮层状态 1=底部，2=漂浮
function MatchManager:SetMatchInfo(nx, ny, info, state, isSetCoor)
    if isSetCoor == nil  then isSetCoor = true end
    state = state or EMapLayerView.None
    if self.infoList[nx] == nil then self.infoList[nx] = {} end
    if self.infoBottomList[nx] == nil then self.infoBottomList[nx] = {} end
    if self.infoFloatList[nx] == nil then self.infoFloatList[nx] = {} end

    if state == EMapLayerView.Bottom then
        info:SetCoord(nx, ny)
        self.infoBottomList[nx][ny] = info
    elseif state == EMapLayerView.Float then
        info:SetCoord(nx, ny)
        self.infoFloatList[nx][ny] = info
    else
        if isSetCoor then info:SetCoord(nx, ny) end
        self.infoList[nx][ny] = info
    end
end

---获取物品数据
---@return MatchInfo
function MatchManager:GetMatchInfo(x, y)
    if self.infoList[x] then
        return self.infoList[x][y]
    end
    return nil
end


---颜色旋转
---@return table<number, MatchInfo>, table<number, UnityEngine.Vector2Int>
function MatchManager:GetColorList()
    local temp, tempCoord = {}, {}
    for _, vTab in pairs(self.infoColorList) do
        for _, v in pairs(vTab) do
            table.insert(temp, v)
            table.insert(tempCoord, Vector2Int.New(v.x, v.y))
        end
    end
    return temp, tempCoord
end

---@return MatchConfig
function MatchManager:GetMapConifg() return self.matchConfig end
---@return MatchAI
function MatchManager:GetMapAI() return self.matchAI end
---@return MatchTarget
function MatchManager:GetMapTarget() return self.matchTarget end

---@return number
function MatchManager:ConvetID(itemId)
    local indexID = math.floor(itemId/10)
    if indexID == 331 or indexID == 334 then--box
        itemId = 3310
    elseif indexID == 332 then--bowl
        itemId = 3320
    elseif indexID == 333 then--jar
        itemId = 3330
    end
    return itemId
end

---获取任务目标
---@return table<number, ETargetItem> 集合
function MatchManager:GetTargetItem()
    local tempTarget = {}
    self.targetIdRun = {}
    local jarNum, boxNum, bowlNum = 0 , 0, 0
    for id, number in pairs(self.targetIds) do
        local indexID = math.floor(id/10)
        if indexID == 331 or indexID == 334 then--box
            boxNum = boxNum + number
        elseif indexID == 332 then--bowl
            bowlNum = bowlNum + 5*number
        elseif indexID == 333 then--jar
            local len = #self.matchConfig:GetItemInfo(id).jarColors
            jarNum = jarNum + len*number
        else
            table.insert(tempTarget, { itemId = id, itemNum = number})
        end
    end

    if jarNum>0 then
        table.insert(tempTarget, { itemId = 3330, itemNum = jarNum})
    end

    if bowlNum>0 then
        table.insert(tempTarget, { itemId = 3320, itemNum = bowlNum})
    end

    if boxNum>0 then
        table.insert(tempTarget, { itemId = 3310, itemNum = boxNum})
    end

    for i, v in ipairs(tempTarget) do
        self.targetIdRun[v.itemId] = v.itemNum
    end

    return tempTarget
end

---@return number
function MatchManager:IsFinishTarget(itemId)
    return self.targetIds[itemId] <= 0
end

---障碍物动画结束：调用
---@param itemId number
function MatchManager:UpdateTarget(itemId)
    if itemId and self.targetIds[itemId] then
        self.targetIds[itemId] = self.targetIds[itemId] - 1
    end

    local isFinish = true
    if itemId then
        local id = self:ConvetID(itemId)
        if self.targetIdRun[id] then
            self.targetIdRun[id] = self.targetIdRun[id] - 1
        end
    end

    for _, v in pairs(self.targetIdRun) do
        if v > 0 then
            isFinish = false
            break
        end
    end

    EventDispatch:Broadcast(ViewID.LevelMain, 5)
    if isFinish and itemId == nil then
        self.matchTarget:UpdateTask(ETaskType.Level, true, self.totalMove)
    end
    return isFinish
end

---目标进度条
---@return number
function MatchManager:GetCurrentTargetNumber(progresssMax)
    local temp = 0.0
    for _, v in pairs(self.targetIdRun) do
        temp = temp + v
    end
    return progresssMax-temp
end

---更新移动次数：检查输赢
---@param isCheck boolean
function MatchManager:UpdateMoveCount(isCheck)
    EventDispatch:Broadcast(ViewID.LevelMain, 1)--更新移动次数
    EventDispatch:Broadcast(UIEvent.Match_AI_Tip_View)--不提示
    EventDispatch:Broadcast(UIEvent.Match_AITip_Shake_View, nil, nil, true) --设置提示时间为0
    if isCheck then
        local win = self:UpdateTarget()
        if self.totalMove <= 0 and not win then
            self.matchTarget:UpdateTask(ETaskType.Level, false, self.totalMove)
        end
    else
        self.totalMove = self.totalMove - 1
    end
end

---获取地图数据
---@return table<number, table<number, MatchInfo>>,table<number, table<number, MatchInfo>>,table<number, table<number, MatchInfo>>
function MatchManager:GetMapInfo()
    return self.infoList, self.infoBottomList, self.infoFloatList
end

---------------------------------新的开发--------------------------------------------------
---创建道具
---@param props EPropsType 生成的道具
---@return MatchInfo
function MatchManager:CreateProps(props, mx, my)
    local isHorizontal = props == EPropsType.Rocket_Horizontal
    if props == EPropsType.Rocket_Horizontal then
        props = EPropsType.Rocket
    end
    local itemID = self.matchConfig:GetPropsInfo(props).itemId
    local info = self:CreateInfo(mx, my, itemID)
    info.isHorizontal = isHorizontal
    return info
end

return MatchManager