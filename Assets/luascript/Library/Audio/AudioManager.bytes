---
--- 声音管理
--- Created by Administrator.
--- DateTime: 2024/5/31 16:04
---

---@class ESoundPlay
ESoundPlay = {
    Loop=1,	--无限循环
    Count=2,	--按次数
    Time=3,	--按总时长
}

---@class AudioManager:DisplayObjectBase
local AudioManager = Class(DisplayObjectBase)

function AudioManager:ctor(gameObject)
    ---背景音乐
    ---@type AudioElement
    self.musicInfo = nil
    self.unloadDelay = 3
    self.isMuteSoundEffect = false;
    self.soundEffectVolume = 1;

    ---音效
    ---@type table<number, AudioElement>
    self.infoList = {}
end

---初始化
function AudioManager:Awake()
    local musicGo = self:FindGameObject("MusicSource")
    self.musicInfo = require("Library.Audio.AudioElement").New(musicGo)
    self.musicInfo:Awake()
end

--- 背景音乐 设置静音
function AudioManager:MusicMute( value)
    self.musicInfo:VoiceMute(value);
end

--- 声音 设置静音
function AudioManager:SoundMute( value)
    self.isMuteSoundEffect = value;
    for i, v in ipairs(self.infoList) do
        v:VoiceMute(value)
    end
end


--- 背景音乐 音量
function AudioManager:MusicVolume( value)
    self.musicInfo:Volume(value);
end

--- 播放背景音乐
function AudioManager:PlayMusic( musicName,  interval)
    if self.musicInfo.soundName == musicName then
        return;
    end
    self.musicInfo:StartPlay(musicName, ESoundPlay.Loop, interval, 0);
end

--- 停止当前正在播放的背景音乐
function AudioManager:StopMusic()
    self.musicInfo:StopPlay();
end

--- 音效 音量
function AudioManager:SoundVolume( value)
    self.soundEffectVolume = value;
    for i, v in ipairs(self.infoList) do
        v:Volume(value)
    end
end

--- 播放声音 一次
function AudioManager:PlaySound(soundName)
    self:PlaySoundCount(soundName, 0, 1);
end

--- 播放声音 计数
---@param soundName string 声音名字
---@param interval number 间隔时长（秒）
---@param continueCount number 持续次数
function AudioManager:PlaySoundCount( soundName,  interval,  continueCount)
    local se = self:GetSoundElement();
    se:StartPlay(soundName, ESoundPlay.Count, interval, continueCount);
end


--- 播放声音 计时
--- <param name="soundName">声音名字</param>
--- <param name="interval">间隔时长（秒）</param>
--- <param name="continueTime">播放时间</param>
function AudioManager:PlaySoundTime( soundName,  interval,  continueTime)
    local se = self:GetSoundElement();
    se:StartPlay(soundName, ESoundPlay.Time, interval, continueTime);
end


--- 停止播放声音
--- <param name="soundName"></param>
function AudioManager:StopSound( soundName)
    for i, v in ipairs(self.infoList) do
        if v.soundName == soundName then
            v:StopPlay();
        end
    end
end

--- 暂停播放
function AudioManager:Pause()
    for i, v in ipairs(self.infoList) do
        v:Pause()
    end
    self.musicInfo:Pause();
end

--- 取消暂停
function AudioManager:UnPause()
    for i, v in ipairs(self.infoList) do
        v:UnPause()
    end
    self.musicInfo:UnPause();
end

---获取一个声音源
---@return AudioElement
function AudioManager:GetSoundElement()
    local se = nil;
    for i, v in ipairs(self.infoList) do
        v:UnPause()
        if v:IsFinish() then
            se = v;
            break;
        end
    end

    if se == nil then
        se= require("Library.Audio.AudioElement").New(self.gameObject)
        se:Awake()
        se:VoiceMute(self.isMuteSoundEffect);
        se:Volume(self.soundEffectVolume);

        table.insert(self.infoList, se);
    end
    return se;
end

return AudioManager