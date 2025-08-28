---
--- MatchAnimationItem       
--- Author : hukiry     
--- DateTime 2023/5/21 11:55   
---

---管理：item 特效，头顶UI，物品掉落
---@class MatchAnimation:DisplayObjectBase
local MatchAnimation = Class(DisplayObjectBase)

function MatchAnimation:ctor(gameObject, matchItem)
    ---@type function
    self.finishCall = nil
    ---@type MatchBase
    self.matchItem = matchItem

    self.resEffectPath = ""
    self.resCurPath = ""
end

---播放缩放动画
---@param scale number 放大倍数
---@param finishCall function 播放完成函数回调
---@param isRotate boolean 默认旋转，带起其他道具飞起时不旋转
function MatchAnimation:PlayScale(scale, finishCall, isRotate)
    if isRotate == nil then isRotate = true end
    self.finishScaleCall = finishCall
    if isRotate then
        self.transform:DORotateAround(Vector3.forward, 20,1)
    end
    self.transform:DOScale(Vector3.one * scale,0.3):OnComplete(function()
        self.finishScaleCall()
    end)
end

---播放移动动画：火箭，彩虹球
---@param targetPos UnityEngine.Vector3 移动动画目标位置
---@param finishCall function 播放完成函数回调
function MatchAnimation:PlayMove(targetPos, finishCall)
    self.finishMoveCall = finishCall
    self.transform:DOMove(targetPos,0.4):OnComplete(function()
        self.matchItem:OnDisable()
        self.finishMoveCall()
    end)
end

---播放物品透明渐变：使用道具后的消除动画
---@param endAlpha UnityEngine.Vector3 移动动画目标位置
---@param finishCall function 播放完成函数回调
function MatchAnimation:PlayAlpha(endAlpha,  finishCall)
    self.finishAlphaCall = finishCall
    self.matchItem.canvasGroup:DOFade(endAlpha,0.3):OnComplete(function()
        self.finishAlphaCall()
    end)
end

---播放旋转:消除动画
---@param finishCall function 播放完成函数回调
function MatchAnimation:PlayRotation(finishCall)
    self.finishRotationCall = finishCall

    self.transform:DOLocalRotate(Vector3.New(0,0,180),0.3):OnComplete(function()
        self.finishRotationCall()
    end)
end

---播放旋转+移动动画：竹蜻蜓
---@param targetPos UnityEngine.Vector3 移动动画目标位置
---@param followPos UnityEngine.Vector3 跟随旋转的位置2
---@param finishCall function 播放完成函数回调
function MatchAnimation:PlayRotationAndMove(followPos, targetPos, finishCall)
    self.finishCall = finishCall
    self.matchItem:SetSortingOrder()
    local cenPos = Vector3.Lerp(self.transform.position, followPos,0.5)
    self.transform:DORotateAround(cenPos, Vector3.forward, 20,0.6):OnComplete(function()
        self:PlayMove(targetPos, self.finishCall)
    end)
end

---播放飞起特效:彩虹球
---@param startPos UnityEngine.Vector3 开始位置
---@param endPos UnityEngine.Vector3 结束位置
---@param finishCall function 播放完成函数回调
function MatchAnimation:PlayEffectFly(startPos, endPos, finishCall)
    self.resEffectPath = "Prefab/Effect/"..ESceneEffectRes.Rainbowball_fly
    local go = GameObjectPool.Get(self.resEffectPath, self.transform.parent.gameObject)
    go.transform:SetPosition(startPos)
    ---@type MatchEffectItem
    local effectItem = require('Game.Core.Match.View.Effect.MatchEffectItem').New(go)
    effectItem:PlayEffectFly(finishCall, endPos, self.resEffectPath)
end

---播放当前特效
---@param startPos UnityEngine.Vector3 开始位置
---@param resCurPath string 特效路径
---@param duration number 播放时长 默认为1
function MatchAnimation:PlayEffect(startPos, resCurPath, duration)
    duration = duration or 1
    self.resCurPath = "Prefab/Effect/"..resCurPath
    local go = GameObjectPool.Get(self.resCurPath, self.transform.parent.gameObject)
    go.transform:SetPosition(startPos)
    ---@type MatchEffectItem
    local effectItem = require('Game.Core.Match.View.Effect.MatchEffectItem').New(go)
    effectItem:PlayEffect(self.resCurPath, duration)
end

return MatchAnimation