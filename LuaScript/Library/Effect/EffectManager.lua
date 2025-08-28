---
--- 特效管理
--- Created by hukiry.
--- DateTime: 2023/3/6 14:57
---



---@class EffectManager
local EffectManager = Class()

function EffectManager:ctor()
    self.duration = 2
end

---场景播放特效
---@param parentGo UnityEngine.GameObject 父对象
---@param info HealGridInfo
---@param isMergeHeal boolean 精灵球治愈的格子
function EffectManager:PlayGrid(parentGo, info, isMergeHeal)
    local position = Util.Map().IndexCoordToWorld(info.x, info.y)
    local effectName =  (isMergeHeal == true and "effect_scene_healGrid" or "effect_scene_healing")
    ---@type EffectItem3D
    local item = EffectItem3D.New(parentGo, effectName)
    item:SetLocalPosition(position)
    item:WaitPlay(self.duration)
end

---场景播放特效
---@param parentGo UnityEngine.GameObject 父对象
---@param info HealGridInfo
---@param effectName string
function EffectManager:PlayEffect(parentGo, info, effectName)
    local position = Util.Map().IndexCoordToWorld(info.x, info.y)
    ---@type EffectItem3D
    local item = EffectItem3D.New(parentGo, effectName)
    item:SetLocalPosition(position)
    item:WaitPlay(self.duration)
end

---@param position UnityEngine.Vector3
---@param effectName string
function EffectManager:PlayPosition(position, effectName, finishCall)
    ---@type EffectItem3D
    local item = EffectItem3D.New(UIManager:GetRootGameObject(), effectName)
    item:SetLocalPosition(position)
    item:WaitPlay(self.duration, finishCall)
end

---场景播放特效
---@param parentGo UnityEngine.GameObject 父对象
---@param effectName string
function EffectManager:PlayGlassEfeect(parentGo, effectName)
    ---@type EffectItem3D
    local item = EffectItem3D.New(parentGo, effectName)
    item:SetLocalPosition(Vector3.New(0,0.3,0))
    item:WaitPlay(10)
end


return EffectManager