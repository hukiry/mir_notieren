---
---
--- Create Time:2023-03-17
--- Author: Hukiry
---

---@class PassInfo
local PassInfo = Class()

function PassInfo:ctor(id)
    ---表id
    ---@type number
    self.id = id
    ---一个月中的第几天
    ---@type number
    self.day = id%100
    ---免费奖励状态：1=可以领取，2=已经领取，0=未领取
    ---@type number
    self.freeState = 0
    ---支付状态：1=已经支付，0=未支付
    ---@type number
    self.payState = 0

    self.rechargePayId = self:GetRechargeCfg().googlePayId
    self.payPrice = self:GetRechargeCfg().price
    self.shopId = self:GetRechargeCfg().payId
end

---锁住状态，不可领取
function PassInfo:IsLock()
    return self.freeState == 0
end

---可以领取
function PassInfo:IsCanGetFree()
    return self.freeState == 1
end

---已经领取
function PassInfo:IsGot()
    return self.freeState == 2
end

---是已经支付
function PassInfo:IsPayed()
    return self.payState == 1
end

---@return string
function PassInfo:GetFreeNumTxt()
    local cfg = self:GetCfg()
    if cfg.type==4 then
        return "x1"
    end
    return  "x"..cfg.freeRewards[1][2]
end

---获取免费图标
---@return string
function PassInfo:GetFreeIcon()
    local cfg = self:GetCfg()
    if cfg.type==1 then
        return "baoxiang"..(self.id%3+1), true
    end
    local iconTy = cfg.freeRewards[1][1]
    return SingleConfig.Currency():GetKey(iconTy).icon, false
end

---获取免费奖励
---@return table<number, table<number, number>>, number
function PassInfo:GetFreeReward()
    local cfg = self:GetCfg()
    return cfg.freeRewards, cfg.type
end

function PassInfo:GetPayReward()
    return self:GetCfg().payRewards
end

function PassInfo:GetPayShowtext()
    return SingleConfig.Recharge():GetShowPrice(self:GetCfg().shopId)
end

---@return TableRechargeItem
function PassInfo:GetRechargeCfg()
    return SingleConfig.Recharge():GetKey(self:GetCfg().shopId)
end

---@return TablePassItem
function PassInfo:GetCfg()
   return SingleConfig.Pass():GetKey(self.id)
end

return PassInfo