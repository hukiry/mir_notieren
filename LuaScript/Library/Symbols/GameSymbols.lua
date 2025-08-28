---
--- 游戏宏定义符号
--- Created by hukiry.
--- DateTime: 2022/5/8 19:32

UNITY_EDITOR = false
UNITY_ANDROID = false
UNITY_IOS = false
UNITY_STANDALONE =  false

DEBUG = false
DEVELOP = false
RELEASE = false

ENABLE_SDK = false
---是否使用LUA
ENABLE_LUA = false

SYSTEM_INFO = false
ENABLE_FPS = false

ASSETBUNDLE_TEST = false
WEAK_NETWORK = false
---是否使用C#
USE_CCHARP = false

---@class GameSymbols
GameSymbols = {}

---初始化游戏宏
function GameSymbols:Init()
    UNITY_EDITOR = RootCanvas.Instance:GetSymbols("UNITY_EDITOR")
    UNITY_ANDROID = RootCanvas.Instance:GetSymbols("UNITY_ANDROID")
    UNITY_IOS = RootCanvas.Instance:GetSymbols("UNITY_IOS")
    UNITY_STANDALONE = RootCanvas.Instance:GetSymbols("UNITY_STANDALONE")

    DEBUG = RootCanvas.Instance:GetSymbols("DEBUG")
    DEVELOP = RootCanvas.Instance:GetSymbols("DEVELOP")
    RELEASE = RootCanvas.Instance:GetSymbols("RELEASE")

    ENABLE_SDK = RootCanvas.Instance:GetSymbols("ENABLE_SDK")
    ENABLE_LUA = RootCanvas.Instance:GetSymbols("ENABLE_LUA")
    SYSTEM_INFO = RootCanvas.Instance:GetSymbols("SYSTEM_INFO")
    ENABLE_FPS = RootCanvas.Instance:GetSymbols("ENABLE_FPS")

    ASSETBUNDLE_TEST = RootCanvas.Instance:GetSymbols("ASSETBUNDLE_TEST")
    ENABLE_SOCKET = RootCanvas.Instance:GetSymbols("ENABLE_SOCKET")
    USE_CCHARP = RootCanvas.Instance:GetSymbols("USE_CCHARP")
    STRONG_SOCKET = RootCanvas.Instance:GetSymbols("STRONG_SOCKET")
end

---开通网络
---@return boolean
function GameSymbols:IsEnableNetwork()
    return ENABLE_SOCKET
end

---强联网
function GameSymbols:IsStrongNetwork()
    return STRONG_SOCKET
end

---手机平台
---@return boolean
function GameSymbols:IsMobile()
    return (UNITY_IOS or UNITY_ANDROID) and not UNITY_EDITOR
end

---使用了SDK
---@return boolean
function GameSymbols:IsEnableSDK()
    return ENABLE_SDK and not UNITY_EDITOR
end

---编辑模式
function GameSymbols:IsWindowsEditor()
    return UNITY_EDITOR
end

---运行模式
---@class EWorkModeApp
EWorkModeApp = {
    ---无热更:本地白包
    Debug = 0,
    ---开发版 热更
    Develop = 1,
    ---正式版 热更
    Release = 2,
}