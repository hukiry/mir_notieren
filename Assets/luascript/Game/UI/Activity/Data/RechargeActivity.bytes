---
--- RechargeActivity       
--- Author : hukiry     
--- DateTime 2023/7/21 21:25   
---

---@class RechargeActivity:BaseInfo
local RechargeActivity = Class(BaseInfo)
function RechargeActivity:ctor()
    --参数配置：充值id,金币
end

function RechargeActivity:InitData()
    self.id, self.coinNum = 0,0
end

function RechargeActivity:SyncRemoteData()
    if not self:IsFinish() then
        ---充值id,金币
        local array = string.Split(self.paramsValue, ',')
        self.id, self.coinNum = tonumber(array[1]), tonumber(array[2])
        ---@type TableRechargeItem
        self.infoRecharge = SingleConfig.Recharge():GetKey(self.id)
        self.googlePayId = self.infoRecharge.googlePayId
        self.price = self.infoRecharge.price
    end
end

function RechargeActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end

---@return string
function RechargeActivity:GetShowPrice()
    return SingleConfig.Recharge():GetShowPrice(self.id)
end


---完成进度同步
function RechargeActivity:SyncProgress()

end

return RechargeActivity