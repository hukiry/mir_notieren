---
--- sdk 事件上报
--- Created by Administrator.
--- DateTime: 2022/11/4 21:35
---

---@class SdkUploadEventInfo
local SdkUploadEventInfo = Class()

function SdkUploadEventInfo:ctor(platformMgr)
    ---@type SdkPlatformManager
    self.platformMgr = platformMgr
end

---上传事件
---@param eventName string 事件名称
function SdkUploadEventInfo:UpEvent(eventName)
    self.platformMgr:CallSDKFunction(EGameSdkType.CustomsEventUp, eventName)
end

return SdkUploadEventInfo
