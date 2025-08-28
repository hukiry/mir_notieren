---
--- MatchTask       
--- Author : hukiry     
--- DateTime 2023/6/26 20:00   
---

---@class MatchTarget
local MatchTarget = Class()
function MatchTarget:ctor()
    self.isStart = false
    self.metaState = 0
end

---加载地图开始
---@param state EMetaFightState
function MatchTarget:StartGame(state)
    self.metaState = state or EMetaFightState.Level
    if self.metaState == EMetaFightState.Level then
        self.isStart = true
    end
end

---@param taskType ETaskType
function MatchTarget:UpdateTask(taskType, value, totalMove)
    if taskType == ETaskType.Normal then
        self:RecordNormal(value)
    elseif taskType == ETaskType.Props then
        self:RecordProps(value)
    elseif taskType == ETaskType.Level then
        self:RecordLevel(value, totalMove)
    elseif taskType == ETaskType.Obstacle then
        self:RecordObstacle(value)
    end
end

---记录障碍物活动进度
---@private
---@param itemId number 生成的道具属性
function MatchTarget:RecordObstacle(itemId)
    if not self.isStart  then return end

    Single.AutoTask():UpdateTaskTarget(itemId)
end

---记录道具活动进度
---@private
---@param props EPropsType 生成的道具属性
function MatchTarget:RecordProps(props)
    if not self.isStart  then return end

    --todo 可获得生命+积分
    local itemId = Single.Match():GetMapConifg():GetPropsInfo(props).itemId
    Single.AutoTask():UpdateTaskTarget(itemId)
    if SingleData.Activity():IsHasType(EActivityType.item) then
        SingleData.Activity():GetProps():AddBornItem(itemId)
    end
end

---记录普通数据记录
---@private
---@param color EColorItem
function MatchTarget:RecordNormal(color)
    if not self.isStart  then return end

    if color ~= EColorItem.none then
        local itemId = 100 + color
        Single.AutoTask():UpdateTaskTarget(itemId)
    end
end

---胜利关卡时：连续通关，通行证
---@private
---@param totalMove number 剩余移动次数：兑换积分
function MatchTarget:RecordLevel(isWin, totalMove)
    if self.isStart then
        self.isStart = false
        if isWin then
            Single.Sound():PlaySound(ESoundResType.LevelWin)--播放音乐
            Single.Player():SetMoneyNum(EMoneyType.level, 1)--等级升级
            Single.AutoTask():SaveData()--保存数据
            UIManager:OpenWindow(ViewID.LevelWin)--播放视图
            if  SingleData.Activity():IsHasType(EActivityType.pass) then
                SingleData.Activity():GetPass():FinishLevelStar()--完成关卡
            end

            if totalMove > 0 then
                Single.Player():SetMoneyNum(EMoneyType.integral, totalMove)
            end
        else
            Single.Sound():PlaySound(ESoundResType.LevelFail)
            UIManager:OpenWindow(ViewID.LevelFail, Handle(self, self.StartGame))
        end
    else
        if self.metaState == EMetaFightState.Test then
            self.metaState = EMetaFightState.TestFinish
            Single.Sound():PlaySound(isWin and ESoundResType.LevelWin or ESoundResType.LevelFail)
            SingleData.Metauniverse():UpdateResult(self.metaState, isWin)
        elseif self.metaState == EMetaFightState.Fight then
            self.metaState = EMetaFightState.FightFinish
            Single.Sound():PlaySound(isWin and ESoundResType.LevelWin or ESoundResType.LevelFail)
            SingleData.Metauniverse():UpdateResult(self.metaState, isWin)
        end
    end
end

return MatchTarget