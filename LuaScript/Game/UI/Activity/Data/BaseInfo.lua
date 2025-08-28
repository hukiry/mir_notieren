---
--- BaseInfo
--- Created by Administrator.
--- DateTime: 2023/7/21 21:32
---

---@class BaseInfo
BaseInfo = Class()
function BaseInfo:ctor(activityType)
    ---@type EActivityType
    self.actType = activityType
end

---@param msg MSG_1201
function BaseInfo:SyncHttpData(msg)
    ---角色id
    self.roleId = msg.roleId
    ---活动ID
    self.configId = msg.configId
    ---活动开始时间
    self.creatTime = msg.creatTime
    ---活动结束时间
    self.expirateTime = msg.expirateTime
    ---2=活动完成，3=活动进度值
    self.state =  msg.state

    ---活动数据，需要解析, targetId,Num|rewardId, num
    self.paramsValue = msg.paramsValue
    ---活动进度字符串
    self.strValue = msg.strValue

    if msg.isFinshed then
        self.state = 2
    end

    self:SyncRemoteData()
end

---子类重写
---@private
function BaseInfo:SyncRemoteData()

end

---子类重写
---@private
function BaseInfo:SyncProgress()

end

---是在活动时间内
---@return boolean
function BaseInfo:IsInActivity()
    local st = Util.Time().GetServerTime()
    return st>self.creatTime and st<self.expirateTime
end

---活动剩余时间差
---@return number
function BaseInfo:_RemainTime()
    return self.expirateTime - Util.Time().GetServerTime()
end

---是活动结束
---@return boolean
function BaseInfo:IsEndActivity()
    local t = self:_RemainTime()
    return t <= 0
end

---获取活动剩余时间-文本
---@return string
function BaseInfo:GetActRemainTime()
    if self:IsFinish() then
        return GetLanguageText(15302)
    end
    local t = self:_RemainTime()
    return Util.Time().GetTimeStringBySecond(t)
end


---预告时间差
---@return number
function BaseInfo:_RemainPreviewTime()
    return self.creatTime - Util.Time().GetServerTime()
end

---是在活动预告时间内
---@return boolean
function BaseInfo:IsInPreview()
    local t = self:_RemainPreviewTime()
    return t > 0
end

---获取活动预告时间-文本
---@return string
function BaseInfo:GetActPreviewTime()
    local t = self:_RemainPreviewTime()
    return Util.Time().GetTimeStringBySecond(t)
end

---是在活动完成
---@return boolean
function BaseInfo:IsFinish()
    return self.state == 2
end
