---
--- 一个火箭
--- PropsRocket
--- DateTime: 2023/6/19 17:29
---

---@class PropsRocket
local PropsRocket = Class()
function PropsRocket:ctor(_self)
    ---@type MatchPropsControl
    self.propsCtrl = _self
    ---@type MatchOperate
    self.matchOperate = _self.matchOperate
end

---火箭：一个和2个的
---@param isHorizontal boolean 道具为火箭时生效
---@return number, table<number> 时间，下落坐标集合
function PropsRocket:ClickProps(x, y, isHorizontal, isTrigger)
    if isTrigger == nil then isTrigger = true end
    self.isTrigger = isTrigger
    self.isHorizontal = isHorizontal
    --正集合，负集合
    local tab1, tab2 = self.matchOperate.matchUtil:Collect_Rocket(x, y, isHorizontal)

    self:NextStepCheck(x, y, true)

    local t1, t2= #tab1, #tab2
    if t1>0 then--正方向
        local z = isHorizontal == true and 270 or 0
        self:_PlayItemFly(tab1, z)
    end

    if t2>0 then--负方向
        local z = isHorizontal == true and 90 or 180
        self:_PlayItemFly(tab2, z)
    end

    local t = t1>t2 and t1*0.1 or t2*0.1
    local downTab, isALLx,count = {}, false,0
    ---获取时间t
    if  self.isTrigger then
        local tabT = table.addArryTable(tab1, tab2)
        for i, v in ipairs(tabT) do
            local item = self.matchOperate.matchView:GetMatchItem(v.x, v.y)
            if item and item.info:IsProps() then
                local tt, pp = self.propsCtrl:GetTriggerTime(item.info)
                if pp ~= EPropsType.Rocket or count == 0 then
                    t = t + tt
                    if pp == EPropsType.Rocket then
                        count = count + 1
                    end
                end

                if item.info:GetCfgInfo().itemProps == EPropsType.Rocket then
                    isALLx = true
                end
            end
        end
    end

    if isHorizontal or isALLx then
        for i = -MatchRule.Size, MatchRule.Size do
            downTab[i] = 1
        end
    else
        downTab[x] = 1
        --边缘消除下落
        if x+1<=MatchRule.Size then downTab[x+1] = 1 end
        if x-1>=-MatchRule.Size then downTab[x-1] = 1 end
    end
    return t+0.1, downTab
end

---2个火箭交换
function PropsRocket:DoubleProps(p, p2)
    local t1, _ = self:ClickProps(p.x, p.y, false)
    local t2, downTab = self:ClickProps(p2.x, p2.y, true)
    local t = (t1+t2)/2
    return t, downTab
end

---火箭与炸弹
function PropsRocket:DiffProps(p, isHorizontal)
    local y1, y2, x1, x2 = p.y,p.y,p.x,p.x
    if isHorizontal then
        if p.y+1 <= MatchRule.Size then y1 = p.y+1 end
        if p.y-1>=-MatchRule.Size then y2 = p.y-1 end
    else
        if p.x+1 <= MatchRule.Size then x1 = p.x+1 end
        if p.x-1>=-MatchRule.Size then x2 = p.x-1 end
    end

    local t, downTab = self:ClickProps(p.x, p.y, isHorizontal)
    local t1,t2,count = 0 , 0 , 1
    if y1 ~= p.y then
        t1 ,_=self:ClickProps(p.x, y1, isHorizontal)
        count =  count+1
    end

    if y2 ~= p.y then
        t2 ,_=self:ClickProps(p.x, y2, isHorizontal)
        count =  count+1
    end

    if x1 ~= p.x then
        t1 ,_=self:ClickProps(x1, p.y, isHorizontal)
        count =  count+1
    end

    if x2 ~= p.x then t2 ,_=self:ClickProps(x2, p.y, isHorizontal)
        count =  count+1
    end
    return (t+t1+t2)/count, downTab
end

---播放物品:边移动边记录数据
---@param coordTab table<number, UnityEngine.Vector2Int> 坐标集合
function PropsRocket:_PlayItemFly(coordTab, z, finishCall)
    local resPath = "prefab/Scene/FlyItem"
    local go = GameObjectPool.Get(resPath, self.matchOperate.gameObject)
    local pos = Util.Map().Vector2IntCoordToWorld(coordTab[1])
    go.transform:SetPosition(pos)
    go.transform:SetEulerAngles(0,0, z)

    Single.Sound():PlaySound(ESoundResType.item_rocket)--火箭声音
    ---@type MatchEffectItem
    local effectItem = require('Game.Core.Match.View.Effect.MatchEffectItem').New(go, resPath, self)
    effectItem.nextStepCallBack = Handle(self, self.NextStepCheck)
    effectItem:PlayRocketItem(coordTab,  1)--已赋值回调函数
end

function PropsRocket:NextStepCheck(x, y, isSelf)
    self.matchOperate.matchView:RefreshNear(x, y, EColorItem.all, true, true)
    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    if item and item.info:IsNormal() then
        self.matchOperate.matchView:RemoveMatchItem(x, y)
    elseif item and item.info:IsProps() then
        if isSelf then
            self.matchOperate.matchView:RemoveProps(x, y)
            item:OnDisable()
        elseif self.isTrigger then
            --todo 触发其他道具
            self.propsCtrl:GetTrigger(item.info, self.isHorizontal)
        end
    end
end

---@return number
function PropsRocket:GetTriggerTime()
    return 1.2
end

return PropsRocket
