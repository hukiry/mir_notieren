---
--- 战斗特效
--- Created by hukiry.
--- DateTime: 2020/2/17 17:12
---

local __effectPath = "Effect/%s"

---@class EffectBase
EffectBase = Class()

---@param parentGo UnityEngine.GameObject
---@param effectName string
function EffectBase:ctor(parentGo, effectName, isRaycastTarget, finishDestroy)
    self.parentGo = parentGo
    self.effectName = effectName
    self.isRaycastTarget = isRaycastTarget or false
    self.effectPath = string.format(__effectPath, self.effectName)
    self.finishDestroy = finishDestroy or false

    ---@type boolean
    self.isPlaying = false;
end

---同步激活/播放
---@param playerFinishFunc function 播放完成回调
function EffectBase:Play(playerFinishFunc)
    self.playerFinishFunc = playerFinishFunc

    self.isPlaying = true;
    if IsNil(self.gameObject) then
        self.gameObject = GameObjectPool.Get(self.effectPath, self.parentGo)
        self.transform = self.gameObject.transform
        self:LoadFinish()
    else
        self:Replay()
    end
end

---异步激活/播放
---@param playerFinishFunc function 播放完成回调
---@param loadFinishFunc function 加载完成回调
function EffectBase:PlayAsync(playerFinishFunc, loadFinishFunc)
    self.playerFinishFunc = playerFinishFunc
    self.isPlaying = true;
    if IsNil(self.gameObject) then
        GameObjectPool.GetAsync(self.effectPath, self.parentGo, function(pGo, pName)
            if not IsNil(self.gameObject) then
                GameObjectPool.Put(self.effectPath, self.gameObject)
            end
            self.gameObject = pGo
            self.transform = pGo.transform
            self:LoadFinish()
            if loadFinishFunc then
                loadFinishFunc()
            end
        end, function(pPrefabName) return self:IsAsyncExecute(pPrefabName) end)

    else
        self:Replay()
    end
end

function EffectBase:IsAsyncExecute(pName)
    return not IsNil(self.parentGo) and self.isPlaying and pName == self.effectPath
end

---加载完成
---@private
function EffectBase:LoadFinish()
    self:LoadFinishInitStart()
    self:GetParticleSystemMount()
    --self:SetSoundVolume()

    self:SetPosition(self.position)
    self:SetLocalPosition(self.localPosition)
    self:SetRotation(self.rotation)
    self:SetLocalScale(self.localScale or 1)

    self:Replay()
    self:LoadFinishInitEnd()
end

---加载完成后初始化开始（用于子类复写）
function EffectBase:LoadFinishInitStart()
end

---加载完成后初始化结束（用于子类复写）
function EffectBase:LoadFinishInitEnd()
end

---准备完成，开始播放
---@protected
function EffectBase:Replay()
    self:AddDestroyTimer()
    if not IsNil(self.gameObject) then
        self.gameObject:SetActive(false)
        self.gameObject:SetActive(true)
    end
end

---停止
function EffectBase:StopPlay()
    self:PlayFinish()
end

---播放完成
---@private
function EffectBase:PlayFinish()
    self.isPlaying = false;
    if not IsNil(self.gameObject) then
        if self.playerFinishFunc ~= nil then self.playerFinishFunc() end

        self:OnDisable()
    end
end

---获取特效总时长
function EffectBase:GetParticleSystemMount()
    if not IsNil(self.gameObject) then
        ---@type ParticleSystemMount
        self.psMount = self.gameObject:GetComponent("ParticleSystemMount")
        if self.psMount and self.psMount.totalTime > 0 then
            self.totalTime = self.psMount.totalTime
        end
    end
end

---播放持续时间
function EffectBase:SetPlayTime(totalTime)
    self.totalTime = totalTime or 2
end

---添加销毁计时器
---@private
function EffectBase:AddDestroyTimer()
    self:RemoveDestroyTimer()
    if self.totalTime ~= nil then
        self.destroyTimer = Single.TimerManger():DoTime(self, function()
            self:PlayFinish()
        end, self.totalTime, 1)
    end
end

---移除销毁计时器
---@private
function EffectBase:RemoveDestroyTimer()
    if self.destroyTimer ~= nil then
        Single.TimerManger():RemoveHandler(self, self.destroyTimer)
        self.destroyTimer = nil
    end
end

---设置特效的世界坐标
---@param position UnityEngine.Vector3
function EffectBase:SetPosition(position)
    self.position = position
    if not IsNil(self.gameObject) and position ~= nil then
        self.gameObject.transform:SetPosition(position, false)
    end
end

---设置特效的相对位置
---@param localPosition UnityEngine.Vector3
function EffectBase:SetLocalPosition(localPosition)
    self.localPosition = localPosition
    if not IsNil(self.gameObject) and localPosition ~= nil then
        self.gameObject.transform:SetPosition(localPosition)
    end
end

---获取特效的相对位置(必须play后再设置这里)
---@return UnityEngine.Vector3
function EffectBase:GetLocalPosition()
    if not IsNil(self.gameObject) then
        return self.gameObject.transform.localPosition
    end
    return Vector3.zero
end

---设置旋转
---@param rotation UnityEngine.Vector3
function EffectBase:SetRotation(rotation)
    self.rotation = rotation
    if not IsNil(self.gameObject) and rotation ~= nil then
        self.gameObject.transform:SetEulerAngles(rotation)
    end
end

---设置特效缩放大小
---@param localScale number
function EffectBase:SetLocalScale(localScale)
    self.localScale = localScale
    if not IsNil(self.gameObject) and localScale ~= nil then
        self.gameObject.transform.localScale = Vector3.New(localScale, localScale, localScale)
        ---@type UIParticleScale
        local com = self.gameObject:GetComponent(typeof(UIParticleScale))
        if com == nil then
            com = self.gameObject:AddComponent(typeof(UIParticleScale))
        end
        com:SetScaleWith(Vector3.one*localScale)
    end
end

function EffectBase:SetLoopParticle(isloop)
    if  self.gameObject then
        ---@type UIParticleScale
        local com = self.gameObject:GetComponent(typeof(UIParticleScale))
        com:SetLoopParticle(isloop)
    end
end

function EffectBase:IsEqual(pEffName)
    return pEffName == self.effName
end

---清理掉当前播放对象与数据，不影响初始化数据
function EffectBase:OnDisable()
    if not IsNil(self.gameObject) then
        GameObjectPool.Put(self.effectPath, self.gameObject)
        if self.finishDestroy then
            GameObjectPool.Clear(self.effectPath)
        end
        self.gameObject = nil
    end
    self:RemoveDestroyTimer()
end

function EffectBase:OnDestroy()
    self:OnDisable()
end