---
--- 广告
--- Created by Administrator.
--- DateTime: 2022/10/4 14:24
---

---@class SdkADInfo
local SdkADInfo = Class()

function SdkADInfo:ctor(platformMgr)
    self.indexPos = 1
    ---@type SdkPlatformManager
    self.platformMgr = platformMgr
end

---@return number
function SdkADInfo:GetAdPosition()
    self.indexPos = self.indexPos%3 + 1
    return self.indexPos
end

---初始化广告
function SdkADInfo:InitAd()
    self.platformMgr:CallSDKFunction(EGameSdkType.AdInit)
end

---启动广告
---@param adType EAdVideo
---@param callFunc function
function SdkADInfo:FetchSDK(adType, callFunc)
    local jsonTab =  {}
    jsonTab.intKey1 = self:GetAdPosition()
    self.adType = adType
    self.callFunc = callFunc
    self.platformMgr:CallSDKFunction(EGameSdkType.AdRewardFetch, jsonTab)
end

---广告回调
---@param sdkType EGameSdkType
---@param jsonTab ESdkMessage
function SdkADInfo:OnCallBack(sdkType, jsonTab)
    if sdkType == EGameSdkType.AdRewardSucces then
        self:OnSuccessful()
    elseif sdkType == EGameSdkType.AdRewardFail then
        log(jsonTab.errorMsg,"red")
        self:OnFail()
    end
end

---播放广告成功
---@private
function SdkADInfo:OnSuccessful()
    if  self.callFunc then
        self.callFunc()
    end
end

---播放广告失败
---@private
function SdkADInfo:OnFail()
    local lanCode = 35002
    if self.adType == EAdVideo.SleepHouse then
        lanCode = "正在获取广告，请稍后尝试"
    elseif self.adType == EAdVideo.Power then
        lanCode = 35002
    elseif self.adType == EAdVideo.Float then
        lanCode = 31018
    end
    TipMessageBox.ShowUI( GetLanguageText(lanCode))
end

return SdkADInfo