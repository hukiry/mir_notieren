---
---
--- Create Time:2023-03-03
--- Author: Hukiry
---

---@class ShopInfo
local ShopInfo = Class()

function ShopInfo:ctor(shopID)
    ---商品id
    self.shopId = shopID
    ---美分
    self.payPrice = self:GetItemCfg().price

    ---充值app商品id
    ---@type string
    self.rechargePayId = self:GetItemCfg().googlePayId

    self.indexType = Mathf.Floor(shopID/1000)

    self.sort = shopID%100
end

---购买获得的奖励
---@return number
function ShopInfo:GetRewardId()
    return self:GetCfg().getId
end

---支付的价格
---@return string
function ShopInfo:GetPayPriceTxt()
    local PriceString = Single.SdkPlatform():GetPayInfo():GetPriceString(self.rechargePayId)
    if PriceString then
        return PriceString
    end
    return self:GetItemCfg().priceShow
end

---获取道具奖励
---@return table<number, ...>
function ShopInfo:GetItemReward()
    local rewardIds, rewardNums=self:GetCfg().rewardIds, self:GetCfg().rewardNums
    local len = #rewardIds < #rewardNums and #rewardIds or #rewardNums
    local temp = {}
    for i = 1, len do
        table.insert(temp, {itemId = rewardIds[i], itemNum = rewardNums[i]})
    end
    return temp
end

---购买获得的奖励
---@return number
function ShopInfo:GetCoinNum()
    return self:GetCfg().coin
end

function ShopInfo:GetCoinShowText()
    local  coin = self:GetCfg().coin
    if coin<1000 then
        return coin
    end
    local pre = math.floor(coin/1000)

    if coin%1000 >0 then
        return pre..' '..(coin%1000)
    end
    return pre..' 000'
end

---@return boolean
function ShopInfo:IsCoin()
    return self.indexType == 2
end

---@return boolean
function ShopInfo:IsGift()
    return self.indexType == 3
end

---获取标题名
function ShopInfo:GetTitle()
    return GetLanguageText(self:GetCfg().name)
end

---获取标签
function ShopInfo:GetTag()
    local tag = self:GetCfg().tag
    if tag > 0 then
        return GetLanguageText(tag)
    end
    return nil
end

---@return TableRechargeItem
function ShopInfo:GetItemCfg()
    return SingleConfig.Recharge():GetKey(self:GetCfg().payId)
end

---@return TableShopItem
function ShopInfo:GetCfg()
   return SingleConfig.Shop():GetKey(self.shopId)
end

return ShopInfo