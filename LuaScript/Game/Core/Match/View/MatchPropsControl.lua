--- 道具视图控制
--- MatchPropsControl       
--- Author : hukiry     
--- DateTime 2023/5/9 21:36   
---

---@class MatchPropsControl
local MatchPropsControl = Class()
---@param matchOperate MatchOperate
function MatchPropsControl:ctor(matchOperate)
    ---@type MatchOperate
    self.matchOperate = matchOperate
    ---@type MatchView
    self.matchView = matchOperate.matchView

    ---@type PropsRocket
    self.propsRocket = require('Game.Core.Match.View.Props.PropsRocket').New(self)
    ---@type PropsRainbowBall
    self.propsRainbowBall = require('Game.Core.Match.View.Props.PropsRainbowBall').New(self)
    ---@type PropsDragonfly
    self.propsDragonfly = require('Game.Core.Match.View.Props.PropsDragonfly').New(self)
    ---@type PropsBomb
    self.propsBomb = require('Game.Core.Match.View.Props.PropsBomb').New(self)
end

function MatchPropsControl:OnStart()
    EventDispatch:Register(self, UIEvent.Match_Operate_Props, self.PlayerUseProps)
end

---道具交换：相同的道具，不同道具，道具和物品
function MatchPropsControl:ChangeProps(pos1, pos2, props1, props2, isHorizontal)
    local t, downTab =0,  {}
    if props1 == props2 then--相同道具
        if props1 == EPropsType.RainbowBall then
            t, downTab = self.propsRainbowBall:DoubleProps(pos1, pos2)
        elseif props1  == EPropsType.Rocket then
            t, downTab = self.propsRocket:DoubleProps(pos1, pos2)
        elseif props1  == EPropsType.Dragonfly then
            t, downTab = self.propsDragonfly:DoubleProps(pos1, pos2)
        elseif props1  == EPropsType.Bomb then
            t, downTab = self.propsBomb:DoubleProps(pos1, pos2, props1)
        end
    else--6种情况
        local p1, p2, prop1, prop2 = self:ComProps(pos1, pos2, props1, props2)
        if prop1 == EPropsType.Rocket then
            t, downTab = self.propsRocket:DiffProps(p1, isHorizontal)
        elseif prop1 == EPropsType.Dragonfly then
            t, downTab = self.propsDragonfly:DiffProps(p1, p2, prop2, isHorizontal)
        elseif prop1 == EPropsType.RainbowBall then
            t, downTab = self.propsRainbowBall:DiffProps(p1, p2, prop2)
        end
    end

    StartCoroutine(function()
        WaitForSeconds(t)
        self.matchView:DownRepairView(table.toArrayKey(downTab))--下落
    end)
end

---调整组合顺序
function MatchPropsControl:ComProps(p1, p2, props1, props2)
    if props1 == EPropsType.RainbowBall or props2 == EPropsType.RainbowBall then--彩球+炸弹，彩球+火箭，彩球+蜻蜓
        if props1 == EPropsType.RainbowBall then
            return p1, p2, props1, props2
        else
            return p2, p1, props2,  props1
        end
    elseif props1 == EPropsType.Dragonfly or props2 == EPropsType.Dragonfly then--蜻蜓+炸弹，蜻蜓+火箭
        if props1 == EPropsType.Dragonfly then
            return p1, p2, props1, props2
        else
            return p2, p1, props2,  props1
        end
    else--火箭+炸弹
        if props1 == EPropsType.Rocket then
            return p1, p2, props1, props2
        else
            return p2, p1, props2,  props1
        end
    end
end

---点击道具：用户不可操作
---@param props EPropsType 道具
---@param isHorizontal boolean 道具为火箭时生效
---@param tab table<UnityEngine.Vector2> 消除坐标
---@param col EColorItem 彩虹球
function MatchPropsControl:ClickProps(x, y, props, isHorizontal, col, tab)
    local t, downTab =0,  {}
    if props == EPropsType.RainbowBall then
        t, downTab = self.propsRainbowBall:ClickProps(x, y, EPropsType.None, col)
    elseif props  == EPropsType.Rocket then
        t, downTab = self.propsRocket:ClickProps(x, y, isHorizontal)
    elseif props  == EPropsType.Dragonfly then
        t, downTab,_ = self.propsDragonfly:ClickProps(x, y)
    elseif props  == EPropsType.Bomb then
        t, downTab = self.propsBomb:ClickProps(x, y, EPropsType.None)
    end

    tab = tab or {}
    if #tab > 0 then
        for _, v in ipairs(tab) do
            if v.x+1<=MatchRule.Size then downTab[v.x+1] = 1 end
            if v.x-1>=-MatchRule.Size then downTab[v.x-1] = 1 end
        end
    end

    StartCoroutine(function()
        WaitForSeconds(t)
        self.matchView:DownRepairView(table.toArrayKey(downTab))--火箭下落
    end)
end

---获取触发的时间，坐标，道具类型
---@param info MatchInfo
---@return number, table<number>, EPropsType
function MatchPropsControl:GetTrigger(info, isHorizontal)
    local x, y, col,props = info.x, info.y, info:GetCfgInfo().color, info:GetCfgInfo().itemProps
    local t, downTab =0,  {}
    if props == EPropsType.RainbowBall then
        t, downTab = self.propsRainbowBall:ClickProps(x, y, EPropsType.None, col)
    elseif props  == EPropsType.Rocket then
        t, downTab = self.propsRocket:ClickProps(x, y, not isHorizontal, true)
    elseif props  == EPropsType.Dragonfly then
        t, downTab, _ = self.propsDragonfly:ClickProps(x, y, true)
    elseif props  == EPropsType.Bomb then
        t, downTab = self.propsBomb:ClickProps(x, y, EPropsType.None)
    end
    return t, downTab, props
end

---@param info MatchInfo
---@return number
function MatchPropsControl:GetTriggerTime(info, itemProps)
    local props = itemProps or info:GetCfgInfo().itemProps
    local t = 0
    if props == EPropsType.RainbowBall then
        t = self.propsRainbowBall:GetTriggerTime()
    elseif props == EPropsType.Rocket then
        t = self.propsRocket:GetTriggerTime()
    elseif props == EPropsType.Dragonfly then
        t = self.propsDragonfly:GetTriggerTime()
    elseif props == EPropsType.Bomb then
        t = self.propsBomb:GetTriggerTime()
    end
    return t, props
end

------------------------------------------------ 使用道具部分 -----------------------------------------------
---使用道具
---@param propsV EPropsView
---@param x number 点击的x 坐标
---@param y number 点击的y 坐标
function MatchPropsControl:PlayerUseProps(propsV, x, y)
    --self.matchOperate.isCanMove = false
    Single.Sound():PlaySound(ESoundResType.UseProps)
    if propsV == EPropsView.Row then
        self:_RowWipe(x, y)
    elseif propsV == EPropsView.Cell then
        self:_CellWipe(x, y)
    elseif propsV == EPropsView.Wipe then
        self:_CoordWipe(x, y)
    elseif propsV == EPropsView.DiceRandom then
        self:_DiceRandom()
    end
end

---消除行
function MatchPropsControl:_RowWipe(x, y)
    local t, downTab = self.propsRocket:ClickProps(x, y, true)
    StartCoroutine(function()
        WaitForSeconds(t)
        self.matchView:DownRepairView(table.toArrayKey(downTab))--火箭下落
    end)
end

---消除列
function MatchPropsControl:_CellWipe(x, y)
    local t, downTab = self.propsRocket:ClickProps(x, y, false)
    StartCoroutine(function()
        WaitForSeconds(t)
        self.matchView:DownRepairView(table.toArrayKey(downTab))--火箭下落
    end)
end

---粉碎任意一个
function MatchPropsControl:_CoordWipe(x, y)
    local item = self.matchView:GetMatchItem(x, y)
    if item then
        EventDispatch:Broadcast(ViewID.LevelMain, 2, item.info.itemId)
        if item.info:GetCfgInfo():IsBarrier() then
            self.matchView:RefreshNear(x, y, EColorItem.all, true)
        else
            self.matchView:RemoveMatchItem(x, y)
        end
    else
        local itemFunction = function(_x, _y)
            local itemR = self.matchView:GetMatchItem(_x, _y)
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
            self.matchView:RefreshNear(x, y, EColorItem.all, true)
        end
    end

    StartCoroutine(function()
        WaitForSeconds(0.25)
        local xtab = {}
        xtab[x] = 1
        --边缘消除下落
        if x+1<=MatchRule.Size then xtab[x+1] = 1 end
        if x-1>=-MatchRule.Size then xtab[x-1] = 1 end
        self.matchView:DownRepairView(table.toArrayKey(xtab))--手动下落
    end)
end

---旋转游戏盘
function MatchPropsControl:_DiceRandom()
    local array, arrayCoord = {}, {}
    --收集颜色物品
    for x, vtab in pairs(self.matchView.itemList) do
        for y, item in pairs(vtab) do
            if item and item.info:IsNormal() then
                if item.info:GetCfgInfo().color >0 then
                    table.insert(array, Vector2Int.New(x, y))
                    table.insert(arrayCoord, Vector2Int.New(x, y))
                end
            end
        end
    end

    --乱序坐标
    local temp = {}
    for _, v in ipairs(arrayCoord) do
        local rIndex = Mathf.Random(1,#array)
        local p = table.remove(array, rIndex)
        local item = self.matchView:GetMatchItem(p.x, p.y)
        --开始坐标，结束坐标
        table.insert(temp, {item = item, pos = v})
        if self.matchView.itemList[p.x] then
            self.matchView.itemList[p.x][p.y] = nil
        end
    end

    ---改变视图坐标，播放动画
    for i, data in ipairs(temp) do
        local x, y = data.pos.x, data.pos.y
        local targetPos = Util.Map().IndexCoordToWorld(x, y)
        if data.item then
            self.matchView:ChangeMapItem(nil, nil, x, y, data.item)
            data.item.info:SetCoord(x, y)
            data.item.transform:DOMove(targetPos,0.4):OnComplete(Handle(self, self._Finish, i, #temp))
        end
    end
end

---移动完成后，检查消除
function MatchPropsControl:_Finish(index, len)
    if index == len then
        self.matchOperate:CheckLoop()
    end
end

---每次隐藏视图：撤销事件，回收对象池，撤销计时器
function MatchPropsControl:OnDisable()
    EventDispatch:UnRegister(self)
end

return MatchPropsControl
