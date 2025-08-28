---
--- 货币栏Icon
--- Create Time:2021-8-2 23:01
--- Author: Hukiry
---

---@class MoneyItem:DisplayObjectBase
local MoneyItem = Class(DisplayObjectBase)

function MoneyItem:ctor(gameObject, moneyType)
    ---@type EMoneyType
    self.moneyType = moneyType
    self.lastValue = 0
    self.clickCount = 0
    self:Start()
end

---初始:注册按钮事件等
function MoneyItem:Start()
    self.numTx = self:FindHukirySupperText("num")
    self.addButton = self:FindGameObject("AddButton")
    self.icon = self:FindTransform("icon")

    self.numlifeTx = self:FindHukirySupperText("icon/num")

    self:AddClick(self.addButton, function()
        if self.moneyType == EMoneyType.life then
            local lifehour, max = Single.Player():GetMoneyNum(EMoneyType.lifehour), Single.Player():GetMoneyNum(EMoneyType.lifeMax)
            local h = math.floor(Single.Player().curLifeTime/(lifehour*3600))
            if h < max then
                UIManager:OpenWindow(ViewID.Life)
            end
        elseif self.moneyType == EMoneyType.gold  and not UIManager:IsShowing(ViewID.Recharge) then
            EventDispatch:Broadcast(ViewID.Game, 1, 1)
            self.clickCount = self.clickCount + 1
            if DEVELOP and self.clickCount > 3 then
                self.clickCount = 0
                UIManager:OpenWindow(ViewID.Gm)--编辑模式
            end
        end
    end)

    self.numTx.text = self.lastValue
    if self.moneyType == EMoneyType.life then
        self.numTx.text = self:GetTimeString(Single.Player().curLifeTime)
        self.numlifeTx.text = Single.Player():GetMoneyNum(EMoneyType.life)
    end
end

function MoneyItem:GetTimeString(t)
    local summ = math.floor(t/60)
    local h = math.floor(summ/60)
    local m = summ%60
    if h==0 and m==0 then
        return  string.format("%02d:%02d", m, t%60)
    else
        return string.format("%02d:%02d:%02d", h, m, t%60)
    end
end

---更新数据HomeMeshView
function MoneyItem:OnEnable(value)
    if self.moneyType ~= EMoneyType.life then
        if value > self.lastValue then
            self.numTx:DOCounter(self.lastValue, value,1.5)
        else
            self.numTx.text = value
        end
        self.lastValue = value
    else
        self:Update()
    end
end

function MoneyItem:Update()
    if self.moneyType == EMoneyType.life then
        local curLifeTime =  Single.Player().curLifeTime
        curLifeTime = curLifeTime - 1
        if curLifeTime < 0 then
            curLifeTime = 0
        end

        local lifehour = Single.Player():GetMoneyNum(EMoneyType.lifehour)
        local h = math.floor(curLifeTime/(3600*lifehour))
        if h > Single.Player():GetMoneyNum(EMoneyType.lifeMax) then
            self.numTx.text = GetLanguageText(10007)
            --self.icon.spriteName = "life_tag"
            self.numlifeTx.text = ''
        else
            --self.icon.spriteName = "life_fill"
            self.numTx.text = self:GetTimeString(curLifeTime)
            self.numlifeTx.text = Single.Player():GetMoneyNum(EMoneyType.life)
        end

        --if h < 1 then
        --    self.icon.spriteName = "life_heart"
        --end

        Single.Player():SetItem(EMoneyType.life, h)
        Single.Player().curLifeTime = curLifeTime
    end
end

---隐藏窗口
function MoneyItem:OnDisable()
    self.lastValue = 0
end


return MoneyItem