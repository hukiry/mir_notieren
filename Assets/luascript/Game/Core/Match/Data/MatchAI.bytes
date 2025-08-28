---
--- MatchAI       
--- Author : hukiry     
--- DateTime 2023/6/23 10:37   
---

---@class MatchAI:DisplayObjectBase
local MatchAI = Class(DisplayObjectBase)
local LEVEL_MAP_KEY = "LEVEL_MAP_KEY"

function MatchAI:ctor(_self)
    ---@type MatchManager
    self.matchMgr = _self
    self.tipCoord = {}
    self.tipCoord[EPropsType.None] = {{Vector2.New(0,1), Vector2.New(0,-1)}, {Vector2.New(1,0), Vector2.New(-1,0)}}
    self.tipCoord[EPropsType.RainbowBall] = {{Vector2.New(0,1), Vector2.New(0,-1)}, {Vector2.New(1,0), Vector2.New(-1,0)}}
    self.tipCoord[EPropsType.Rocket] = {{Vector2.New(0,1), Vector2.New(0,-1)}, {Vector2.New(1,0), Vector2.New(-1,0)}}
    
    self.tipCoord[EPropsType.Bomb] = {Vector2.New(0,1), Vector2.New(0,-1), Vector2.New(1,0), Vector2.New(-1,0)}
    self.tipCoord[EPropsType.Dragonfly] = {Vector2.New(0,1), Vector2.New(0,-1), Vector2.New(1,0), Vector2.New(-1,0)}
end

function MatchAI:InitData()
    self.totalWipe = 0
    self.downCoord = {}
    self.downCoordLen = 0
end

---创建Ai地图数据
---@param lv number 关卡等级
function MatchAI:AI_CreateMap(lv)
    self.matchMgr.targetIds = {}
    local localTab =  self:ReadBinaryTable(LEVEL_MAP_KEY ..lv, self.matchMgr.matchMap:ToMessageBody())
    if localTab and #localTab.grids>0 then
        self.matchMgr:CreateMap(localTab, true)
        self:_ConfigDownCoord(localTab.jsonItems, self.matchMgr.targetIds)
    else
        --物品列表，目标id，颜色范围，目标集合，移除次数，（颜色id，对应任务颜色id）
        local tab, targetIds, colors, jsonItems, moveNumber, taskIDs = self.matchMgr.matchMap:CreateMap(lv)
        local msgTab = protobuf.ConvertMessage(self.matchMgr.matchMap:ToMessageBody())
        msgTab.colorMin, msgTab.colorMax = colors[1], colors[2]
        msgTab.normalId,  msgTab.taskItemId = taskIDs[1], taskIDs[2]
        for _, v in ipairs(tab) do
            if v.itemId_bottom > 0 then
                self.matchMgr:CreateInfo(v.x, v.y, v.itemId_bottom, EMapLayerView.Bottom)
            end

            if v.itemId_float > 0 then
                self.matchMgr:CreateInfo(v.x, v.y, v.itemId_float, EMapLayerView.Float)
            end

            self.matchMgr:CreateInfo(v.x, v.y, v.itemId)
            table.insert(msgTab.grids, v)
        end

        local nmove = 0
        for id, number in pairs(targetIds) do
            local item = {}
            item.itemId = id
            item.itemNum = number
            table.insert(msgTab.targetIds, item)
            self.matchMgr.targetIds[id] = number
            nmove = nmove + number
        end


        local rm, half = math.random(2,5), math.floor(nmove/2)
        msgTab.totalMove = moveNumber >nmove and half + rm or moveNumber -rm

        for i, v in ipairs(jsonItems) do
            table.insert(msgTab.jsonItems, v)
        end

        self:_ConfigDownCoord(msgTab.jsonItems, self.matchMgr.targetIds)
        self.matchMgr.totalMove = msgTab.totalMove

        local col = msgTab.normalId%100
        local colT = msgTab.taskItemId>106 and (msgTab.taskItemId%100-6) or (msgTab.taskItemId%10)
        for i = msgTab.colorMin, msgTab.colorMax do
            local id = 100+i
            if colT>=msgTab.colorMin and colT<=msgTab.colorMax then
                if colT == i and col > 0 then
                    table.insert(self.matchMgr.colors, msgTab.taskItemId)
                else
                    table.insert(self.matchMgr.colors, id)
                end
            else
                if i == col then
                    table.insert(self.matchMgr.colors, msgTab.taskItemId)
                else
                    table.insert(self.matchMgr.colors, id)
                end
            end
        end
        self.matchMgr:SaveBinaryTable(LEVEL_MAP_KEY ..lv, msgTab)
    end
end

---@param jsonItems table<number,EMapObstacleData>
function MatchAI:_ConfigDownCoord(jsonItems, tagertTab)
    ---记录可移动的物品
    for _, v in ipairs(jsonItems) do
        local id, num, xTab = v.itemId, v.itemNum, v.xTab
        local cfg = self.matchMgr.matchConfig:GetItemInfo(id)
        if cfg then
            local bType = cfg.barrierType
            if bType == EObstacleType.Move or bType == EObstacleType.Animal or bType == EObstacleType.Born then
                self.matchMgr.obstacleIds[id] = {remainNum = num-tagertTab[id], xTab = xTab }
                tagertTab[id] = num
            end
        end
    end
end

---@param view MatchView
---@return table<UnityEngine.Vector2>
function MatchAI:AI_TipPlayer(view)
    ---@type MatchUtil
    local matchUtil = view.matchOperate.matchUtil
    for y = -MatchRule.Size, MatchRule.Size do
        for x = -MatchRule.Size, MatchRule.Size-1 do
            local item1, item2 = view:GetMatchItem(x, y), view:GetMatchItem(x+1, y)
            if item1 and item2 then
                if not item1.info:GetCfgInfo():IsBarrier() and  item2.info:IsNormal()  then
                    local color = item2.info:GetCfgInfo().color
                    local props, coordTab = matchUtil:SearchSameColor(x, y, color)
                    if #coordTab >= 2 then
                        local tab = self:_CheckResult(x, y, props, coordTab , view, color)
                        if #tab > 0 then
                            return tab
                        end
                    end
                end
            end
        end
    end
    return {}
end

function MatchAI:_CheckResult(x, y, props, coordTab, view, color)
    local recordTab, tipCoord = {},{}
    local xnum, ynum = 0, 0
    for _, v in ipairs(coordTab) do
        if v.x~=x or v.y~=y then
            if v.x == x then xnum = xnum + 1 end
            if v.y == y then ynum = ynum + 1 end
            table.insert(tipCoord, Vector2.New(v.x, v.y))
            if recordTab[v.x]==nil then
                recordTab[v.x] = {}
            end
            recordTab[v.x][v.y] = 1
        end
    end

    if props == EPropsType.Rocket_Horizontal then
        props = EPropsType.Rocket
    end

    if props == EPropsType.Bomb or props == EPropsType.Dragonfly then
        local tab = self.tipCoord[props]
        for _, v in ipairs(tab) do
            local nx, ny = v.x + x, v.y + y
            local itemV = view:GetMatchItem(nx, ny)
            if itemV and itemV.info:IsNormal() and itemV.info:GetCfgInfo().color == color and (recordTab[nx]==nil or recordTab[nx][ny]==nil) then
                table.insert(tipCoord, Vector2.New(nx, ny))
                return tipCoord
            end
        end

        if xnum>1 or ynum>1 then
            for _, v in ipairs(tab) do
                local nx, ny = v.x + x, v.y + y
                local vx = xnum>1 and math.abs(v.y) or math.abs(v.x)
                if recordTab[nx] and recordTab[nx][ny] and vx>0 then
                    table.insert(tipCoord, Vector2.New(nx, ny))
                    return tipCoord
                end
            end
        end
    else
        local index = xnum>ynum and 2 or 1
        local tab = self.tipCoord[props][index]
        for _, v in ipairs(tab) do
            local nx, ny = v.x + x, v.y + y
            local itemV = view:GetMatchItem(nx, ny)
            if itemV and itemV.info:IsNormal() and itemV.info:GetCfgInfo().color == color then
                if (recordTab[nx]==nil or recordTab[nx][ny]==nil) then
                    table.insert(tipCoord, Vector2.New(itemV.info.x, itemV.info.y))
                    return tipCoord
                end
            end
        end

        if props ~= EPropsType.None then
            tab = self.tipCoord[props][index==1 and 2 or 1]
            for _, v in ipairs(tab) do
                local nx, ny = v.x + x, v.y + y
                if recordTab[nx] and recordTab[nx][ny] then
                    table.insert(tipCoord, Vector2.New(nx, ny))
                    return tipCoord
                end
            end
        end
    end
    return {}
end

---获取限制范围
---@param v number
function MatchAI:_IsInRange(v)
    return v <= MatchRule.Size and v>=-MatchRule.Size
end

return MatchAI