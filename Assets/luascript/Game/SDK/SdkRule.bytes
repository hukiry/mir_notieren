---
--- SDK枚举
--- Created by Administrator.
--- DateTime: 2022/10/4 14:35
---

---sdk类型
---@class EGameSdkType
EGameSdkType =
{
    ---网络不可见
    NetworkUnavailable = 100,

    ---初始化 SDK
    Init = 101,
    InitSucces = 102,
    InitFail = 103,

    ---登录 SDK
    Login = 201,
    LoginSucces = 202,
    LoginFail = 203,
    LoginCancel = 204,
    LoginToken = 205,
    LoginRevoke = 206,
    ---登出 sdk
    Logout = 301,
    LogoutSucces = 302,
    LogoutFail = 303,

    --- 支付拉起
    PayFetch = 401,
    --- 支付成功
    PaySucces = 402,
    --- 支付失败
    PayFail = 403,
    --- 支付取消
    PayCancel = 404,
    --- 支付凭证验证成功：可以发货
    PayVerifyRecipeSucces = 405,
    --- 支付凭证验证失败
    PayVerifyRecipeFail = 406,
    --- 初始化商品列表拉起
    InitProductFetch = 501,
    --- 初始化商品列表成功
    InitProductSucces = 502,
    --- 初始化商品列表失败
    InitProductFail = 503,

    ---广告初始化
    AdInit = 601,
    AdInitSucces = 602,
    AdInitFail = 603,
    ---广告拉起
    AdRewardFetch = 611,
    AdRewardSucces = 612,
    AdRewardFail = 613,

    ---仅android 检查google服务
    CheckGooglePlay = 701,
    ---检查google服务成功
    CheckGooglePlaySuccessful = 702,
    ---检查google服务失败
    CheckGooglePlayFail =703,

    ---离线通知
    LocalNotification = 801,
    ----------------------------其他部分--------------------------------
    ------获取系统语言代码
    GetSystemLanguage = 1,
    ------获取App版本号
    GetAppVersionName = 2,
    ---更新SDK 语言
    UpdateLanguage = 3,
    ---用户协议
    UserAgreement = 4,
    ---数据事件统计
    CustomsEventUp = 5,
    ---打开URL-app
    OpenUrl = 6,
    ---打开apple评分
    OpenRate = 7,
}


---@class ESdkMessage
ESdkMessage = {
    funType = 0,
    ---@type string
    jsonParams = 1,
    errorCode = 2,
    ---@type string
    errorMsg = 3,
}

---@class EAdVideo
EAdVideo = {
    ---休息屋
    SleepHouse = 1,
    ---体力恢复
    Power = 2,
    ---漂浮物
    Float =3,
}