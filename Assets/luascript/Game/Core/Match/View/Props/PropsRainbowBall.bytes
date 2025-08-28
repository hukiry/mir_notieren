--- -一个彩虹球
--- PropsRainbowBall       
--- Author : hukiry     
--- DateTime 2023/6/19 17:33   
---

---@class PropsRainbowBall
local PropsRainbowBall = Class()
function PropsRainbowBall:ctor(_self)
    ---@type MatchPropsControl
    self.propsCtrl = _self
    ---@type MatchOperate
    self.matchOperate = _self.matchOperate
end

---彩虹球：一个和2个的，独立
---@param props EPropsType 道具
function PropsRainbowBall:ClickProps(x, y, props, color)
    color  = color or EColorItem.none
    local tab = self.matchOperate.matchUtil:Props_RainbowBall(color, self.matchOperate.matchView, props)
    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    if item then
        local startPos = Util.Map().IndexCoordToWorld(x, y)
        item:SetSortingOrder()
        item:GetAnimation():PlayScale(1.5, Handle(self, self.FinsihScale, x, y, tab, props))
        item:GetAnimation():PlayEffect(startPos, ESceneEffectRes.Match_rotation)
    end

    local  t = 0.85 + self.propsCtrl:GetTriggerTime(nil, props)
    local downTab = {}
    if props ~= EPropsType.None then
        for i = -MatchRule.Size, MatchRule.Size do
            downTab[i] = 1
        end
    else
        for i, v in ipairs(tab) do
            downTab[v.x] = 1
            if v.x+1<=MatchRule.Size then downTab[v.x+1] = 1 end
            if v.x-1>=-MatchRule.Size then downTab[v.x-1] = 1 end
        end
    end
    return t , downTab
end

---2个彩虹球交换
function PropsRainbowBall:DoubleProps(p1, p2)
    local item1 = self.matchOperate.matchView:GetMatchItem(p1.x, p1.y)
    local item2 = self.matchOperate.matchView:GetMatchItem(p2.x, p2.y)
    item1:SetSortingOrder()
    item1:GetAnimation():PlayScale(8)
    item2:SetSortingOrder()
    item2:GetAnimation():PlayScale(10, Handle(self, self.StartDouble))
    local t ,downTab = 1.3, {}
    for x = -MatchRule.Size, MatchRule.Size do
        downTab[x] = 1
    end
    return t ,downTab
end

---彩虹球与其他
function PropsRainbowBall:DiffProps(p, p2, prop)
    local item2 = self.matchOperate.matchView:GetMatchItem(p2.x, p2.y)
    local targetPos = Util.Map().Vector2IntCoordToWorld(p)
    self.matchOperate.matchView:RemoveProps(p2.x, p2.y)
    item2:GetAnimation():PlayMove(targetPos, HandleParams(function(item)
        item:OnDisable()
    end, item2))
    return self:ClickProps(p.x, p.y, prop)
end

function PropsRainbowBall:StartDouble()
    StartCoroutine(function()
        for x = -MatchRule.Size, MatchRule.Size do
            WaitForSeconds(0.1)
            for y = -MatchRule.Size, MatchRule.Size do
                self:WipeAll(x, y)
            end
        end
    end)
end

function PropsRainbowBall:FinsihScale(x, y, tab, props)
    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    local startPos = Util.Map().IndexCoordToWorld(x, y)
    Single.Sound():PlaySound(ESoundResType.ItemMove)
    item.index = 0
    for _, v in ipairs(tab) do
        local endPos = Util.Map().IndexCoordToWorld(v.x, v.y)
        ---播放飞的动画特效
        if props == EPropsType.None then
            item:GetAnimation():PlayEffectFly(startPos, endPos,
                    Handle(self, self.WipeItem, item, tab, props))
        else
            item:GetAnimation():PlayEffectFly(startPos, endPos,
                    Handle(self, self.SendProps, v, props, item, tab))
        end
    end
end

---消除颜色
---@param item MatchBase
function PropsRainbowBall:WipeItem(item, tab)
    item.index = item.index + 1
    if item.index == #tab then
        self.matchOperate.matchView:RemoveMatchItem(item.info.x, item.info.y)
        for i, v in ipairs(tab) do
            self.matchOperate.matchView:RemoveMatchItem(v.x, v.y)
            self.matchOperate.matchView:RefreshNear(v.x, v.y, EColorItem.all, true, true)
        end
    end
end

---生成道具
---@param props EPropsType
function PropsRainbowBall:SendProps(p, props, item, tab)
    local bornPros = props
    if props == EPropsType.Rocket then
        bornPros = math.random(0,1) == 1 and EPropsType.Rocket_Horizontal or props
    end
    local info = Single.Match():CreateProps(bornPros, p.x, p.y)
    ---@type MatchBase
    local itemV =  self.propsCtrl.matchView:GetMatchItem(p.x, p.y)
    itemV:UpdateInfo(info)
    item.index = item.index + 1

    if item.index == #tab then
        self.propsCtrl.matchView:RemoveMatchItem(item.info.x, item.info.y)--移除自己
        for _, v in ipairs(tab) do
            local x, y = v.x, v.y
            local itemNew =  self.propsCtrl.matchView:GetMatchItem(x, y)
            if props  == EPropsType.Rocket then
                self.propsCtrl.propsRocket:ClickProps(x, y, itemNew.info.isHorizontal, false)
            elseif props  == EPropsType.Dragonfly then
                self.propsCtrl.propsDragonfly:ClickProps(x, y, true)
            elseif props  == EPropsType.Bomb then
                self.propsCtrl.propsBomb:ClickProps(x, y, EPropsType.None, true)
            end
        end
    end
end

---消除所有
function PropsRainbowBall:WipeAll(x, y)
    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    if item then
        if item.info:IsNormal() or item.info:IsProps() then
            self.matchOperate.matchView:RemoveMatchItem(x, y)
        else
            self.matchOperate.matchView:RefreshNear(x, y, EColorItem.all, true)
        end
    end
end

---@return number
function PropsRainbowBall:GetTriggerTime()
    return 0.85
end

return PropsRainbowBall