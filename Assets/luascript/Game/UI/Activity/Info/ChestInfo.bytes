---
--- ChestInfo       
--- Author : hukiry     
--- DateTime 2023/8/3 13:56   
---

---@class ChestInfo
local ChestInfo = Class()

--3列集合配置：101,数量,充值数量
function ChestInfo:ctor(tab, index)
    ---类型id
    self.id = tonumber(tab[1])
    ---配置顺序
    self.index = index
    ---集合类型
    self.sort = math.floor(self.id/100)
    ---货币类型
    ---@type EMoneyType
    self.moneyType = self.id%100
    ---奖励数量
    self.number = tonumber(tab[2])

    ---0=锁，1=可领取，2=已领取, 3=已支付
    ---@type number
    self.state = 0
end

function ChestInfo:IsPayed()
    return self.state == 3
end

function ChestInfo:IsFree()
    return self.sort > 1
end

function ChestInfo:GetNumberText()
    return 'x'..self.number
end

function ChestInfo:GetItemIcon()
    return SingleConfig.Currency():GetKey(self.moneyType).icon
end

return ChestInfo