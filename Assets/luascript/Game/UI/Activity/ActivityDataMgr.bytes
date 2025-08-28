---
--- ActivityDataMgr       
--- Author : hukiry     
--- DateTime 2023/7/21 21:20   
---

require("Game.UI.Activity.Data.BaseInfo")

---@class ActivityDataMgr:DisplayObjectBase
local ActivityDataMgr = Class(DisplayObjectBase)
function ActivityDataMgr:ctor()
    ---间隔是 1秒请求一个类型。 每个类型：每60秒请求一次
end

--清理数据
function ActivityDataMgr:InitData()
    ---@type table<EActivityType, ChestActivity|DrawcardActivity|FestivalActivity|GiftActivity|IntegralActivity|PropsActivity|PassActivity|RechargeActivity|SignActivity>
    self.infoList = {}
    ---@type table<EActivityType, number>
    self.httpTypeTab = {}

    self.classTab = {
        [EActivityType.chest] = "Chest",
        [EActivityType.drawcard] = "Drawcard",
        --[EHttpActivityType.festival] = "Festival",
        [EActivityType.gift] = "Gift",
        --[EHttpActivityType.integral] = "Integral",
        [EActivityType.item] = "Props",
        [EActivityType.pass] = "Pass",
        [EActivityType.rechargeFirst] = "Recharge",
        [EActivityType.sign] = "Sign",
        [EActivityType.meta] = "Meta",
    }

    self.sendHttpTab = {}
    for key, v in pairs(self.classTab ) do
        table.insert(self.sendHttpTab, key)
    end

    self.indexHttp = 1
end

---@return EActivityType
function ActivityDataMgr:GetIndexType()
    local actType = self.sendHttpTab[self.indexHttp]
    if self.infoList[actType] then
        local len = #self.sendHttpTab
        for i = self.indexHttp, len do
            if not self.infoList[actType] then
                return actType
            else
                self.indexHttp = self.indexHttp + 1
            end
        end
    end
    return actType
end

---请求数据:成功后，发送下一个
function ActivityDataMgr:SendHttp()
    if self.sendHttpTab[self.indexHttp] == nil then
        self.indexHttp = 1
        return
    end

    local actType = self:GetIndexType()
    if self.infoList[actType] == nil then--未请求过的
        Single.Request().SendActivity(actType, EHttpActivityState.Request, nil, Handle(self, self._HttpFail, actType))
    end
end

function ActivityDataMgr:SendALLHttp()
    for i, v in ipairs(self.sendHttpTab) do
        Single.Request().SendActivity(v, EHttpActivityState.Request, nil, Handle(self, self._HttpFail, v))
    end
end

---请求数据错误返回
function ActivityDataMgr:_HttpFail(actType, succ)
    if succ then
        self.indexHttp = self.indexHttp + 1
        EventDispatch:Broadcast(ViewID.Game, 3, EGamePage.HomeView, actType)
    end
end

---同步http数据
---@param msg MSG_1201
function ActivityDataMgr:SyncHttpData(msg)
    local className =  self.classTab[msg.type]
    if className then
        if msg.configId > 0 then
            self.httpTypeTab[msg.type] = 1
            local classPath = string.format("Game.UI.Activity.Data.%sActivity", className)
            if self.infoList[msg.type] == nil then
                self.infoList[msg.type] = require(classPath).New(msg.type)
                if  self.infoList[msg.type].InitData then
                    self.infoList[msg.type]:InitData()
                end
            end
            self.infoList[msg.type]:SyncHttpData(msg)
        else
            log(msg.type, msg.configId)
        end
    end
end

---同步完成
---@param msg MSG_1201
function ActivityDataMgr:SyncProgress(msg)
    if  self.infoList[msg.type] then
        self.infoList[msg.type].strValue = msg.strValue
        self.infoList[msg.type]:SyncProgress()
    end
end

---同步完成
---@param msg MSG_1201
function ActivityDataMgr:SyncHttpFinish(msg)
    if  self.infoList[msg.type] then
        self.infoList[msg.type].state = EHttpActivityState.Finished
    end
end

---播放奖励
---@param actType EActivityType 控制按钮
---@param finishCall function<number> 播放完成回调
---@param resType EMoneyType 资源类型
---@param rewardNum number 奖励数量
function ActivityDataMgr:PlayReward(actType, resType, rewardNum, finishCall)
    ---@type Eitem_fly_data
    local flyData = {}
    flyData.size = Vector2.New(100,100)
    flyData.iconName = SingleConfig.Currency():GetKey(resType).icon
    local data = Single.Animation():CaculateCount(rewardNum)
    flyData.count = data.len
    ---@type GameView
    local win = UIManager:GetActiveWindow(ViewID.Game)
    if win then
        local startPos = win:GetIconHome(actType).position
        local targetTf = win:GetGoldTrans(resType)
        Single.Animation():PlayMultipleItem(startPos, targetTf, flyData, EAnimationFly.ViewToView, HandleParams(function(finishCall1, data1)
            data1.index = data1.index + 1
            if  data1.index == data1.len and data1.modNum > 0 then
                finishCall1(data1.modNum)
            else
                finishCall1(data1.addNum)
            end
        end, finishCall, data),nil, ViewID.Game)
    else
        logError("找不到窗口", resType, rewardNum)
    end
end


---是否已经请求过
---@param type EActivityType
function ActivityDataMgr:IsHasType(type)
    return self.httpTypeTab[type]~=nil
end

---移除完成的活动信息
---@param type EActivityType
function ActivityDataMgr:RemoveActivityInfo(type)
    self.infoList[type] = nil
end

---获取商品信息
---@param type EActivityType
---@return BaseInfo
function ActivityDataMgr:GetActivityInfo(type)
    return self.infoList[type]
end

---@return ChestActivity
function ActivityDataMgr:GetChest() return self.infoList[EActivityType.chest] end
---@return DrawcardActivity
function ActivityDataMgr:GetDrawcard() return self.infoList[EActivityType.drawcard] end
---@return FestivalActivity
function ActivityDataMgr:GetFestival() return self.infoList[EActivityType.festival] end
---@return GiftActivity
function ActivityDataMgr:GetGift() return self.infoList[EActivityType.gift] end
---@return IntegralActivity
function ActivityDataMgr:GetIntegral() return self.infoList[EActivityType.integral] end
---@return PropsActivity
function ActivityDataMgr:GetProps() return self.infoList[EActivityType.item] end
---@return PassActivity
function ActivityDataMgr:GetPass() return self.infoList[EActivityType.pass] end
---@return RechargeActivity
function ActivityDataMgr:GetRecharge() return self.infoList[EActivityType.rechargeFirst] end
---@return SignActivity
function ActivityDataMgr:GetSign() return self.infoList[EActivityType.sign] end
---@return MetaActivity
function ActivityDataMgr:GetMeta() return self.infoList[EActivityType.meta] end

return ActivityDataMgr