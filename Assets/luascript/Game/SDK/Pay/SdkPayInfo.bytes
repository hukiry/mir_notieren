---
--- 支付
--- Created by Administrator.
--- DateTime: 2022/10/23 14:04
---

---@class SdkPayInfo
local SdkPayInfo = Class()

function SdkPayInfo:ctor(platformMgr)
    ---@type SdkPlatformManager
    self.platformMgr = platformMgr
    self.hasProduct = false
    ---@type table<string, Hukiry.SDK.ProductData>
    self.infoList = {}
end

---开始支付; 先检查网络，后检查是否启动SDK
---@param callFunc function 支付成功回调
---@param produceId string 商品id
function SdkPayInfo:FetchSDK(produceId, callFunc)
    if not Single.Http():IsHaveNetwork() then
        UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10002), function()  end)
        return
    end

    if GameSymbols:IsEnableSDK() then
        UIManager:OpenWindow(ViewID.BuyTip, 12)
        local jsonTab =  {}
        jsonTab.stringKey1 = produceId
        jsonTab.stringKey2 = SingleConfig.Recharge():GetShopName(produceId)
        self.callFunc = callFunc
        self.platformMgr:CallSDKFunction(EGameSdkType.PayFetch, jsonTab)
    else
        callFunc()
    end
end

---初始化商品
function SdkPayInfo:InitProduct()
    local jsonTab =  {}
    jsonTab.stringKey1 = ""
    local tab = SingleConfig.Recharge():GetTable()
    local len = table.length(tab)
    local index = 0
    for i, v in pairs(tab) do
        index = index+1
        if index >=len  then
            jsonTab.stringKey1 =  jsonTab.stringKey1  .. v.googlePayId
        else
            jsonTab.stringKey1 =  jsonTab.stringKey1  .. v.googlePayId .. ','
        end

    end
    self.jsonTab = jsonTab
    self.platformMgr:CallSDKFunction(EGameSdkType.InitProductFetch, jsonTab)
end

---请求SDK的商品清单：在商店视图打开时
function SdkPayInfo:ReqSDKProductList()
    if not self.hasProduct then
        self.platformMgr:CallSDKFunction(EGameSdkType.InitProductFetch,  self.jsonTab)
    end
end

---获取价格描述
---@param payId string 苹果后台的充值ID
---@return string
function SdkPayInfo:GetPriceString(payId)
    if  self.hasProduct and self.infoList[payId] then
        return self.infoList[payId].PriceString
    end
    return nil
end

---支付回调
---@param sdkType EGameSdkType
---@param jsonTab ESdkMessage
function SdkPayInfo:OnCallBack(sdkType, jsonTab)
    if sdkType == EGameSdkType.PaySucces then
        UIManager:OpenWindow(ViewID.BuyTip, 6)
    elseif sdkType == EGameSdkType.PayVerifyRecipeSucces then
        UIManager:CloseWindow(ViewID.BuyTip)
        if self.callFunc then
            self.callFunc()
        end
    elseif sdkType == EGameSdkType.InitProductSucces then
        self.hasProduct = true
        for _, v in ipairs(jsonTab) do
            self.infoList[v.id] = v
        end
    elseif sdkType == EGameSdkType.PayCancel then
        UIManager:CloseWindow(ViewID.BuyTip)
        UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10031), function() end)
    elseif sdkType == EGameSdkType.NetworkUnavailable then
        UIManager:CloseWindow(ViewID.BuyTip)
        UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10001))
    else
        logError(jsonTab.errorMsg)
        UIManager:CloseWindow(ViewID.BuyTip)
        UIManager:OpenWindow(ViewID.CommonTip, jsonTab.errorMsg, function() end)
    end
end

return SdkPayInfo