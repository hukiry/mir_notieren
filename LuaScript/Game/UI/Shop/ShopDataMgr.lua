---
---
--- Create Time:2023-03-03
--- Author: Hukiry
---

---@class ShopDataMgr
local ShopDataMgr = Class()
function ShopDataMgr:ctor()

end

--清理数据
function ShopDataMgr:InitData()
    ---商店信息：页签，id，信息
	---@type table<number, ShopInfo>
    self.infoList = {}
    local  tab = SingleConfig.Shop():GetTable()
    for _, v in pairs(tab) do
        ---@type ShopInfo
        local info = require("Game.UI.Shop.Data.ShopInfo").New(v.productId)
        self.infoList[info.shopId] = info
    end
end

---@param msg MSG_1401
function ShopDataMgr:PaySuccessful(msg)
    ---@type ShopInfo
    local info = self.infoList[msg.shopId]
    if info.indexType == 1 or info.indexType  == 2  then--充值道具和金币
        EventDispatch:Broadcast(ViewID.Game, 3, EGamePage.ShopView,  msg.shopId)
        EventDispatch:Broadcast(ViewID.Recharge, msg.shopId)
    end
end

---@param shopId number
---@return ShopInfo
function ShopDataMgr:GetShopInfo(shopId)
    return self.infoList[shopId]
end

---获取商品信息
---@return table<number, ShopInfo>, table<number, ShopInfo>
function ShopDataMgr:GetShopArray()
    local shopTab, coinTab = {}, {}
    for i, v in pairs(self.infoList) do
        if v:IsCoin() then
            table.insert(coinTab, v)
        elseif not v:IsGift() then
            table.insert(shopTab, v)
        end
    end

    table.sort(shopTab, function(a, b) return a.sort < b.sort end)
    table.sort(coinTab, function(a, b) return a.sort < b.sort end)
    return shopTab, coinTab
end

---获取礼包信息
---@return table<number, ShopInfo>
function ShopDataMgr:GetGiftArray()
    local shopTab={}
    for i, v in pairs(self.infoList) do
        if v:IsGift() then
            table.insert(shopTab, v)
        end
    end
    table.sort(shopTab, function(a, b) return a.sort < b.sort end)
    return shopTab
end

return ShopDataMgr