---
--- 全局实例类
--- Created by hukiry.
--- DateTime: 2021/7/13 23:05
---
require('Library.Animation.AnimationRule')

---@class Single
Single = {}
---@type table<string,table>
local m_list = {}
---导入lua
---@private
---@param className string
---@param classPath string
function Single.ImportClass(className, classPath)
    if m_list[className] == nil then
        local classObj = require(classPath)
        if classObj.New then
            m_list[className] = classObj.New()
        else
            m_list[className] = classObj
        end

        if m_list[className].InitData then
            m_list[className]:InitData()
        end
    end
    return m_list[className]
end

---切换账号时登录
function Single.InitData()
    for _, v in pairs(m_list) do
        if v.InitData then
            v:InitData()
        end
    end
end

---切换账号时登录
function Single.SaveData()
    for _, v in pairs(m_list) do
        if v.SaveData then
            v:SaveData()
        end
    end
end

---退出游戏时清空
function Single.ClearData()
    for _, v in pairs(m_list) do
        if v.ClearData then
            v:ClearData()
        end
    end
    m_list = {};
end

---计时器
---@return TimerManger
function Single.TimerManger()
    return Single.ImportClass('TimerManger','Library.Timer.TimerManger')
end

---适配器
---@return ScreenIPhoneSize
function Single.Screen()
    return Single.ImportClass('ScreenIPhoneSize','Library.Adaption.ScreenIPhoneSize')
end

---图集
---@return SpriteAtlasManager
function Single.SpriteAtlas()
    return Single.ImportClass('SpriteAtlasManager','Library.Loader.Res.SpriteAtlasManager')
end

---声音
---@return SoundManager
function Single.Sound()
    return Single.ImportClass('SoundManager','Library.Audio.SoundManager')
end

---Binary二级制本地数据缓存
---@return BinaryDataManager
function Single.Binary()
    return Single.ImportClass('BinaryDataManager','Library.DataCache.BinaryDataManager')
end

---持久化数据
---@return PlayerPrefsManager
function Single.PlayerPrefs()
    return Single.ImportClass('PlayerPrefsManager','Library.DataCache.PlayerPrefsManager')
end

---动画（UI和场景）
---@return AnimationManager
function Single.Animation()
    return Single.ImportClass('AnimationManager','Library.Animation.AnimationManager')
end

---场景特效播放
---@return EffectManager
function Single.Effect()
    return Single.ImportClass('EffectManager','Library.Effect.EffectManager')
end

---重连
---@return ReconnectionMgr
function Single.Reconnection()
    return Single.ImportClass('ReconnectionMgr','Network.Socket.ReconnectionMgr')
end

---心跳
---@return HeartBeatMgr
function Single.HeartBeat()
    return Single.ImportClass('HeartBeatMgr','Network.Socket.HeartBeatMgr')
end

---http数据加载
---@return HttpManager
function Single.Http()
    return Single.ImportClass('HttpManager','Network.Http.HttpManager')
end

---请求
---@return MessageRequest
function Single.Request()
    return Single.ImportClass('MessageRequest','Network.Message.MessageRequest')
end

---sdk平台
---@return SdkPlatformManager
function Single.SdkPlatform()
    return Single.ImportClass('SdkPlatformManager','Game.SDK.SdkPlatformManager')
end

---游戏中心-通用模块
---@return GameCenterMgr
function Single.GameCenter()
    return Single.ImportClass('GameCenterMgr','Game.Manager.GameCenterMgr')
end

---角色属性-通用模块
---@return PlayerDataManager
function Single.Player()
    return Single.ImportClass('PlayerDataManager','Game.Player.PlayerDataManager')
end

-------------------------------------自定义部分---------------------------------------------------

---匹配
---@return MatchManager
function Single.Match()
    return Single.ImportClass('MatchManager','Game.Core.Match.Data.MatchManager')
end

---自动生成任务
---@return AutoTaskManager
function Single.AutoTask()
    return Single.ImportClass('AutoTaskManager','Game.Core.Auto.AutoTaskManager')
end

---元宇宙
---@return MetaManager
function Single.Meta()
    return Single.ImportClass('MetaManager','Game.Core.Meta.Data.MetaManager')
end