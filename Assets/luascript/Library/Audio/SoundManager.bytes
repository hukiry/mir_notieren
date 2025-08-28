---
--- 声音管理类
--- Created by hukiry.
--- DateTime: 2018\5\16 0016 10:33
---

---@class SoundManager
local SoundManager = Class()
function SoundManager:ctor()

end

function SoundManager:InitData()
    local rootGo = UIManager:GetRootGameObject()
    local tf = rootGo.Find("SoundManager")
    if tf == nil then
        ---@type UnityEngine.GameObject
        tf = UnityEngine.GameObject.New("SoundManager").transform
        tf:SetParent(rootGo.transform, false)
        local newGo = UnityEngine.GameObject.New("MusicSource")
        newGo.transform:SetParent(tf, false)
    end

    ---@type AudioManager
    self.musicInfo = require("Library.Audio.AudioManager").New(tf.gameObject)
    self.musicInfo:Awake()
    local isMusicOff = Single.PlayerPrefs():GetBool(EGameSetting.MusicMute, false)
    local isEffectOff = Single.PlayerPrefs():GetBool(EGameSetting.SoundMute, false)
    self.musicInfo:MusicMute(not isMusicOff);
    self.musicInfo:SoundMute(not isEffectOff);
end

---播放背景音乐
---@param musicName ESoundResType
---@param interval number 播放间隔(秒)
function SoundManager:PlayMusic(musicName, interval)
    self.musicInfo:PlayMusic(musicName, interval or 0 );
end

---停止当前正在播放的背景音乐
function SoundManager:StopMusic()
    self.musicInfo:StopMusic();
end

---播放声音
---@param soundName string 声音名字
function SoundManager:PlaySound(soundName)
    self.musicInfo:PlaySound(soundName);
end

---播放声音  计次方式
---@param soundName string 声音名字
---@param interval number 间隔时长（秒）
---@param continueCount number 持续次数 -1：无限循环
function SoundManager:PlaySoundCount(soundName, interval, continueCount)
    self.musicInfo:PlaySoundCount(soundName, interval or 0, continueCount or 1);
end

---播放声音  计时方式
---@param soundName string 声音名字
---@param interval number 间隔时长（秒）
---@param time number 持续时长
function SoundManager:PlaySoundTime(soundName, interval, time)
    self.musicInfo:PlaySoundTime(soundName, interval or 0, time or 1);
end

---停止播放声音
---@param soundName string 声音名字
function SoundManager:StopSound(soundName)
    self.musicInfo:StopSound(soundName);
end

---暂停播放所有声音
function SoundManager:Pause()
    self.musicInfo:Pause()
end

---取消暂停的所有声音
function SoundManager:UnPause()
    self.musicInfo:UnPause()
end

---背景音乐 设置静音
---@param value boolean true=开启声音 false=关闭
function SoundManager:SetMusicMute(value)
    self.musicInfo:MusicMute(not value);
    Single.PlayerPrefs():SetBool(EGameSetting.MusicMute, value)
end

---音效 设置静音
---@param value boolean true=开启声音 false=关闭
function SoundManager:SetSoundMute(value)
    self.musicInfo:SoundMute(not value);
    Single.PlayerPrefs():SetBool(EGameSetting.SoundMute,value)
end

---背景音乐 音量调整
---@param value number 音量大小
function SoundManager:SetBackMusicVolume(value)
    self.musicInfo:MusicVolume(value);
    Single.PlayerPrefs():SetFloat(EGameSetting.MusicVolume,value)
end

---音效  音量调整
---@param value number 音量大小
function SoundManager:SetSoundEffectVolume(value)
    self.musicInfo:SoundVolume(value);
    Single.PlayerPrefs():SetFloat(EGameSetting.SoundVolume,value)
end

return SoundManager