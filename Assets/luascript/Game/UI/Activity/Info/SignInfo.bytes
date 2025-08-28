---
--- 签到
--- Created by Administrator.
--- DateTime: 2023/3/13 20:26
---

---@class SignInfo
local SignInfo = Class()

function SignInfo:ctor(dayId)
    ---id
    ---@type number
    self.dayId = dayId
    ---签到类型
    self.type = Mathf.Floor(dayId/100)
    ---第几天
    self.day = dayId%100
    ---0=未签到，1=可签到， 2=已签到
    self.state = 0
end

function SignInfo:GetName()
    if self.state >=1 and self.day == SingleData.Activity():GetSign().curDay then
        return GetLanguageText(15004)
    end
    return tostring(self.day) .. " " ..GetLanguageText(15005)
end

function SignInfo:GetRewardType()
    local ty = self:GetCfg().rewardType
    if ty == 1 then
        return EMoneyType.gold
    elseif ty == 3 then
        return EMoneyType.life
    else
        return self:GetCfg().propType
    end
end


function SignInfo:GetCfg()
    return SingleConfig.Sign():GetKey(self.dayId)
end

return SignInfo
