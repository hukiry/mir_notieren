---
--- 飞行特效部分
--- Created by hukiry.
--- DateTime: 2020/2/17 17:43
---

---@class EffectItem3DFly:EffectBase
EffectItem3DFly = Class(EffectBase)

---@param startPos UnityEngine.Vector3 起始点
---@param targetPos UnityEngine.Vector3 目标点
---@param flySpeed number 飞行速度
---@param finishCallback function 回调函数
function EffectItem3DFly:ctor(parentGo, effectName, startPos, targetPos, flySpeed, finishCallback)
    self.startPos = startPos
    self.targetPos = targetPos
    self.finishCallback = finishCallback
    self.flySpeed = flySpeed

    self:Play()
end

function EffectItem3DFly:Replay()
end

function EffectItem3DFly:LoadFinishInitEnd()
    self.gameObject.transform.position = self.startPos
    self.gameObject.transform.localEulerAngles = Vector3.zero

    ---@type UnityEngine.Transform
    self.tf = self.gameObject.transform
    self.tf:LookAt(self.targetPos);
    self.timer = Single.TimerManger():DoFrame(self, function () self:OnFrame() end, -1)

    ---@type number 剩余距离
    self.surplusDis = Vector3.Distance(self.targetPos, self.tf.position)
    self.lastDis = self.surplusDis - 0.5    --上一次的距离
end

function EffectItem3DFly:OnFrame()
    --这种计算到达目标点的方法会正加准确，不会因为卡帧一下子冲过目标点
    local tempDis = Vector3.Distance(self.targetPos, self.tf.position)
    local frameDis = math.abs(self.lastDis - tempDis)
    self.surplusDis = self.surplusDis - frameDis

    if self.surplusDis < 0 then
        self:FlyFinish()
    else
        self.tf:Translate(Vector3.forward * Time.deltaTime * self.cfgEffect.flySpeed)
    end
    self.lastDis = tempDis
end

function EffectItem3DFly:FlyFinish()
    self.isPlaying = false
    if self.finishCallback then
        self.finishCallback()
    end
    self:OnDestroy()--飞行特效有些特殊，直接销毁
end

function EffectItem3DFly:OnDestroy()
    if self.isDestroy == true then
        return
    end
    self.isDestroy = true
    Single.TimerManger():RemoveHandler(self, self.timer)
    self.finishCallback = nil

    EffectBase.OnDestroy(self)
end