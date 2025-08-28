---
--- 账号窗口
--- Created by Administrator.
--- DateTime: 2024/6/24 13:12
---

---@class AccountPanel:DisplayObjectBase
local AccountPanel = Class()--DisplayObjectBase

function AccountPanel:ctor()--gameObject
    --self:Awake()
end

---初始化窗口属性
--function AccountPanel:Awake()
    --self.appleBtn = self:FindGameObject("maskFrame/layout/appleBtn")
    --self.googleBtn = self:FindGameObject("maskFrame/layout/googleBtn")
    --self.loginClose = self:FindGameObject("maskFrame/loginClose")
    --
    --self:AddClick(self.loginClose, Handle(self, self.OnDisable))
    --self:AddClick(self.appleBtn, Handle(self, self.OnLogin, 1))
    --self:AddClick(self.googleBtn, Handle(self, self.OnLogin, 2))
--end

---@param state number apple = 1, google = 2
function AccountPanel:OnLogin(state)
    if Single.Player().isBindLogin then
        return
    end

    Single.SdkPlatform():GetLoginInfo():FetchSDK(state, Handle(self, self.LoginAccount, state))
end

---@private
---@param jsonTab table 登录信息
function AccountPanel:LoginAccount(bindAccount, jsonTab)
    self:OnDisable()

    Single.Player().bindAccount = bindAccount
    Single.Player().isBindLogin = true
    if self.callback then
        self.callback()
    end

    Single.PlayerPrefs():SetInt(EGameSetting.BindFacebook, bindAccount)
    Single.Player():SaveRoleData()
    Single.Request().SendBindLogout(EHttpLoginState.Bind, jsonTab.userID)
    EventDispatch:Broadcast(ViewID.Setting)
end

---显示窗口
---@param callback function
function AccountPanel:OnEnable(callback)
    self.callback = callback
    --self.gameObject:SetActive(true)
    --self.appleBtn:SetActive(Single.SdkPlatform():IsShowAppleLogin())
end

--隐藏窗口时
function AccountPanel:OnDisable()
    --self.gameObject:SetActive(false)
end

return AccountPanel
