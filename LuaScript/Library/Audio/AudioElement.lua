---
--- 声音
--- Created by Administrator.
--- DateTime: 2024/5/31 16:05
---

---@class AudioElement:DisplayObjectBase
local AudioElement = Class(DisplayObjectBase)

function AudioElement:ctor(gameObject)
    ---@type UnityEngine.AudioSource
    self.audioSource = nil
    self._isFinish = true;
    self._isPause = false;

    self.soundName = ""
    self.eType = ESoundPlay.Time
    self.interval=0
    self.time=0

    self.isEnableTimer = false
end

function AudioElement:Awake()
    self.audioSource = self.gameObject:AddComponent(typeof(UnityEngine.AudioSource))
    Single.TimerManger():RemoveHandler(self, self.Update)
    self.isEnableTimer = false
end

function AudioElement:EnableTimer()
    if not self.isEnableTimer then
        Single.TimerManger():RemoveHandler(self, self.Update)
        Single.TimerManger():DoFrame(self, self.Update,1, -1)
        self.isEnableTimer=true;
    end
end

function AudioElement:StartPlay( soundName,  eType,  interval,  time)
    self:EnableTimer();
    self.soundName = soundName;
    self.eType = eType;
    self.interval = interval;
    self.time = time;
    self._isFinish = false;
    ResManager:LoadAsync("sound/" .. self.soundName, Handle(self, self.LoadFinish));
end

---@param go UnityEngine.AudioClip
function AudioElement:LoadFinish(path, go)
    if go == nil then
        self:OnFinish();
        return;
    end
    self.audioSource.clip = go;
    self:AgainPlay();
end


---再次播放
function AudioElement:AgainPlay() 
    if not self:IsLoadFinish() then
        return;
    end

    try {
        function()
            if self.audioSource.isPlaying then
                self.audioSource:Stop();
            end
            self.audioSource:Play();
            self.nextPlayTime = self.audioSource.clip.length + self.interval;
        end,
        catch = function(error)
            logError(error)
            self:OnFinish();
        end
    }
end

function AudioElement:Play() 
    try {
        function()
            self.audioSource:Play();
        end,
        catch = function(error)
            logError(error)
            self:OnFinish();
        end
    }
end

--- 暂停播放
function AudioElement:Pause()
    try {
        function()
            self.audioSource:Pause();
            self._isPause = true;
        end,
        catch = function(error)
            logError(error)
            self:OnFinish();
        end
    }
end

--- 取消暂停
function AudioElement:UnPause()
    try {
        function()
            self.audioSource:UnPause();
            self._isPause = false;
        end,
        catch = function(error)
            logError(error)
            self:OnFinish();
        end
    }
end

function AudioElement:Update() 
    if not self:IsLoadFinish() or self._isPause then
        return;
    end

    self.nextPlayTime =  self.nextPlayTime - UnityEngine.Time.deltaTime;
    if self.nextPlayTime <= 0 then
        self:PlayFinish();
        self:AgainPlay();
    end

    if self.eType == ESoundPlay.Time then
        self.time = self.time - UnityEngine.Time.deltaTime;
        if self.time <= 0 then
            self:OnFinish();
        end
    end
end

--- 停止播放
function AudioElement:StopPlay()
    self:OnFinish();
end


--- 声音播放完成，只是单个clip，可能会再循环
function AudioElement:PlayFinish()
    if self.eType == ESoundPlay.Count then
        self.time=self.time-1;
    end

    if self.time == 0 then
        self:OnFinish();
    end
end


--- 表示整个任完完成
function AudioElement:OnFinish()
    self._isFinish = true;
    self._isPause = false;
    self.audioSource.clip = nil;
    self.soundName = "";
    ResManager:Unload(self.soundName,3);
end


--- 是否加载完成
function AudioElement:IsLoadFinish()
    return self.audioSource.clip ~= nil;
end

--- 是否播放完成
function AudioElement:IsFinish()
    return self._isFinish == true and self.audioSource.clip == nil;
end

--- 设置声音大小
function AudioElement:Volume(volume)
    self.audioSource.volume = volume;
end

--- 声音静音
function AudioElement:VoiceMute(isMute)
    self.audioSource.mute = isMute;
end

return AudioElement