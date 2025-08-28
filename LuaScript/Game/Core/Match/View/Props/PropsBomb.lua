--- 一个炸弹
--- PropsBomb       
--- Author : hukiry     
--- DateTime 2023/6/19 17:34   
---

---@class PropsBomb
local PropsBomb = Class()
function PropsBomb:ctor(_self)
    ---@type MatchOperate
    self.matchOperate = _self.matchOperate
end

---炸弹：一个和2个的
---@param props EPropsType 道具
function PropsBomb:ClickProps(x, y, props, isExchange)
    Single.Sound():PlaySound(ESoundResType.item_bomb)
    local tab = self.matchOperate.matchUtil:Props_Bomb(x, y, props == EPropsType.Bomb)
    local item = self.matchOperate.matchView:GetMatchItem(x, y)
    if item and not isExchange then
        local pos = item.info:GetWorldPos()
        item:SetSortingOrder()
        item:GetAnimation():PlayEffect(pos, ESceneEffectRes.Match_rotation)
        item:GetAnimation():PlayScale(8, Handle(self, self.FinishWipe, tab, item))
    else
        self:FinishWipe(tab, item)
    end

    local downTab = {}
    for i, v in ipairs(tab) do
        downTab[v.x] = 1
        if v.x+1<=MatchRule.Size then downTab[v.x+1] = 1 end
        if v.x-1>=-MatchRule.Size then downTab[v.x-1] = 1 end
    end
    local len = #tab
    local t = props == EPropsType.Bomb and 2 or 0.4+len*0.02
    return t , downTab
end

function PropsBomb:DoubleProps(p1, p2, props)
    self.matchOperate.matchView:RemoveMatchItem(p2.x, p2.y)
    return self:ClickProps(p1.x, p1.y, props)
end

---@param itemV MatchBase
function PropsBomb:FinishWipe(tab, itemV)
    if itemV then
        local _x, _y = itemV.info.x, itemV.info.y
        if itemV.info:IsProps() then
            self.matchOperate.matchView:RemoveMatchItem(_x, _y )
        end
    end

    StartCoroutine(function()
        for _, v in ipairs(tab) do
            WaitForFixedUpdate()
            local x, y = v.x, v.y
            local item = self.matchOperate.matchView:GetMatchItem(x, y)
            if item then
                if item.info:IsNormal() or item.info:IsProps() then
                    self.matchOperate.matchView:RemoveMatchItem(x, y)
                else
                    self.matchOperate.matchView:RefreshNear(x, y, EColorItem.all, true)
                end
            else
                self.matchOperate.matchView:RefreshNear(x, y, EColorItem.all, true)
            end
        end
    end)
end
---@return number
function PropsBomb:GetTriggerTime()
    return 0.6
end

return PropsBomb