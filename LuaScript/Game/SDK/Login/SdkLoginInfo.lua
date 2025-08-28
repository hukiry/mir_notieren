---
--- 登录
--- Created by Administrator.
--- DateTime: 2022/10/23 14:04
---

---@class SdkLoginInfo
local SdkLoginInfo = Class()
local ACCESS_TOKEN = "access_token"

function SdkLoginInfo:ctor(platformMgr)
    ---@type SdkPlatformManager
    self.platformMgr = platformMgr
    ---@type Hukiry.SDK.AppleLoginVerify
    self.AppleLoginVerify = self.platformMgr.SdkManager.ins.AppleLoginVerify
    local client_secret = "eyJraWQiOiI4NTRBVFlSSjJIIiwiYWxnIjoiRVMyNTYifQ.eyJpc3MiOiJKODhLSEZaV1Y2IiwiaWF0IjoxNzE5MTMxMzIzLCJleHAiOjE3MzQ2ODMzMjMsImF1ZCI6Imh0dHBzOi8vYXBwbGVpZC5hcHBsZS5jb20iLCJzdWIiOiJjb20ueWlsaXUuSGFwcHlNYXRjaCJ9.C5LFhqzonxxqQh_pMV4t0i0_6CDGy76YyXTMcXhAKjHYX3XGqNnEZYWIDE4Yozi7gCg86uDYZ364PcH2iErJog"
    local client_id = "com.yiliu.HappyMatch"
    self.AppleLoginVerify:SetClient(client_id, client_secret)
end

---开始登录
---@param state number apple = 1, google = 2
---@param callFunc function
function SdkLoginInfo:FetchSDK(state, callFunc)
    if not Single.Http():IsHaveNetwork() then
        TipMessageBox.ShowUI(GetLanguageText(10002))
        return
    end

    self:AppleRevokeLogin()
    local jsonTab =  {}
    jsonTab.intKey1 = state
    self.callFunc = callFunc
    self.platformMgr:CallSDKFunction(EGameSdkType.Login, jsonTab)
end

---登出facebook
function SdkLoginInfo:Logout()
    self.isStartRevoke = false
    self:AppleRevokeLogin()
    local state =  Single.PlayerPrefs():GetInt(EGameSetting.BindFacebook, 0)
    if state == 2 then
        local jsonTab =  {}
        jsonTab.intKey1 = state
        self.platformMgr:CallSDKFunction(EGameSdkType.Logout, jsonTab)
    end
end

---登录回调
---@private
---@param sdkType EGameSdkType
---@param jsonTable ESdkMessage
function SdkLoginInfo:OnCallBack(sdkType, jsonTable)
    if sdkType == EGameSdkType.LoginSucces then
        local jsonTab = json.decode(jsonTable.jsonParams)
        self.callFunc(jsonTab)
    elseif sdkType == EGameSdkType.LoginFail then--登录失败
        logError("fail login",jsonTable.errorMsg)
    elseif sdkType == EGameSdkType.LoginToken then
        local jsonTab = json.decode(jsonTable.jsonParams)
        self.isStartToken = false
        self:AppleTokenLogin(jsonTab.authorizationCode)
    else --取消登录
        logError("cancel login",jsonTable.errorMsg)
    end
end

--获取苹果登录token
function SdkLoginInfo:AppleTokenLogin(authorizationCode)
    --log("开始请求-authorizationCode", authorizationCode)
    self.AppleLoginVerify:RequstToken(authorizationCode, function(isSuccess, jsonParam)
        if isSuccess then
            log("RequstToken", jsonParam)
            local tab = json.decode(jsonParam)
            PlayerPrefs.SetString(ACCESS_TOKEN, tab.access_token)
        else
            log("RequstFail", jsonParam)
            if self.isStartToken then
                return
            end
            self.isStartToken = true
            self:AppleTokenLogin(authorizationCode)
        end
    end)
end

---撤回授权
function SdkLoginInfo:AppleRevokeLogin()
    if PlayerPrefs.HasKey(ACCESS_TOKEN) then
        local token = PlayerPrefs.GetString(ACCESS_TOKEN, '')
        self.AppleLoginVerify:RequstRevoke(token, function(isSuccess, jsonParam)
            if isSuccess then
                log("LoginRevoke", "successful", jsonParam)
                PlayerPrefs.DeleteKey(ACCESS_TOKEN)
            else
                if self.isStartRevoke then
                    return
                end
                self.isStartRevoke = true
                self:AppleRevokeLogin()
            end
        end)
    end
end

return SdkLoginInfo