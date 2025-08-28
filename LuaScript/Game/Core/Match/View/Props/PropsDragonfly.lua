--- -个蜻蜓
--- PropsDragonfly       
--- Author : hukiry     
--- DateTime 2023/6/19 17:34   
---

---@class PropsDragonfly
local PropsDragonfly = Class()
function PropsDragonfly:ctor(_self)
    ---@type MatchPropsControl
    self.propsCtrl = _self
    ---@type MatchOperate
    self.matchOperate = _self.matchOperate
end

---竹蜻蜓：一个和2个的
---@param isTrigger boolean
function PropsDragonfly:ClickProps(x, y, isTrigger, followPos, isRotate)
    Single.Sound():PlaySound(ESoundResType.item_zhuqingting)
    local tab, targetCoord = self.matchOperate.matchUtil:Props_Dragonfly(x, y, self.matchOperate.matchView)
    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    item:SetSortingOrder()
    local pos = Util.Map().Vector2IntCoordToWorld(targetCoord)
    if isRotate then
        item:GetAnimation():PlayRotationAndMove(followPos, pos, Handle(self, self.WipeItem, targetCoord.x, targetCoord.y, item))
    else
        item:GetAnimation():PlayMove(pos, Handle(self, self.WipeItem, targetCoord.x, targetCoord.y, item))
    end
    self.matchOperate.matchView:RemoveProps(x, y)

    local downTab,t = {}, 0.7
    downTab[x] = 1
    downTab[targetCoord.x] = 1
    if targetCoord.x+1<=MatchRule.Size then downTab[targetCoord.x+1] = 1 end
    if targetCoord.x-1>=-MatchRule.Size then downTab[targetCoord.x-1] = 1 end
    if not isTrigger then
        for i, v in ipairs(tab) do
            local pt, pdowntab, props = self:WipeItem(v.x, v.y, nil, isRotate)
            if props ~=  EPropsType.Dragonfly then
                t = t + pt
            end

            for key, _ in pairs(pdowntab) do
                downTab[key] = 1
            end

            if v.x+1<=MatchRule.Size then downTab[v.x+1] = 1 end
            if v.x-1>=-MatchRule.Size then downTab[v.x-1] = 1 end
        end
    end

    if isRotate then
        return t, downTab, targetCoord
    end
    return t, downTab, {}
end

---2个竹蜻蜓交换
function PropsDragonfly:DoubleProps(p1, p2)
    local targetPos1 = Util.Map().Vector2IntCoordToWorld(p1)
    local targetPos2 = Util.Map().Vector2IntCoordToWorld(p2)
    local t, downTab,_ = self:ClickProps(p1.x, p1.y,false, targetPos2, true)
    self:ClickProps(p2.x, p2.y,false, targetPos1, true)
    return t, downTab
end

---竹蜻蜓与炸弹，火箭
function PropsDragonfly:DiffProps(p1, p2, prop, isHorizontal)
    local targetPos1 = Util.Map().Vector2IntCoordToWorld(p1)
    local targetPos2 = Util.Map().Vector2IntCoordToWorld(p2)

    local item = self.matchOperate.matchView:GetMatchItem(p2.x, p2.y)
    local t2, _ = self.propsCtrl:GetTriggerTime(item.info)

    local t1, _, targetCoord = self:ClickProps(p1.x, p1.y,false, targetPos2, true)
    local pos = Util.Map().Vector2IntCoordToWorld(targetCoord)
    item:GetAnimation():PlayRotationAndMove(targetPos1, pos, Handle(self, self.TriggerProps, targetCoord, item, prop, isHorizontal))

    local t, downTab = t1+t2,{}
    for i = -MatchRule.Size, MatchRule.Size do
        downTab[i] = 1
    end
    return t, downTab
end

---@param item MatchBase
function PropsDragonfly:TriggerProps(p, item, prop, isHorizontal)
    local x, y = item.info.x, item.info.y
    self.matchOperate.matchView:RemoveMatchItem(x, y)
    if prop == EPropsType.Rocket then
        self.propsCtrl.propsRocket:ClickProps(p.x, p.y, isHorizontal)
    else
        self.propsCtrl.propsBomb:ClickProps(p.x, p.y, nil, true)
    end
end

---@param itemSelf MatchBase
function PropsDragonfly:WipeItem(x, y,itemSelf, isRotate)
    if itemSelf then
        itemSelf:OnDisable()
        self.matchOperate.matchView:RefreshNear(x, y, EColorItem.all, true, true)
    else
        self.matchOperate.matchView:RefreshNear(x, y, EColorItem.all, true)
    end

    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    if item and item.info:IsNormal() then
        self.matchOperate.matchView:RemoveMatchItem(x, y)
    elseif (item and item.info:IsProps()) and not isRotate then
        --todo 触发其他道具
        return self.propsCtrl:GetTrigger(item.info)
    end
    return 0, {}, EPropsType.None
end
---@return number
function PropsDragonfly:GetTriggerTime()
    return 0.5
end

return PropsDragonfly