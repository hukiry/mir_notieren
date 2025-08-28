---
--- Spine控制器
--- Created by hukiry.
--- DateTime: 2021-01-18 16:57
---

---@class SpineController:AsynModelController
SpineController = Class(AsynModelController)

---@param parentGo UnityEngine.GameObject
---@param immediatelyDestroy boolean 使用完后是否立即销毁(不放回对象池)
function SpineController:ctor(parentGo, immediatelyDestroy)
end

---加载完成初始化方法
function SpineController:OnLoadCompleteInitialization()
    ---@type Spine.Unity.SkeletonGraphic
    self.animator = self.modelGo:GetComponent("SkeletonGraphic")
    self:PlayAnimation(self.aniName)
    self:SetColor(self.color)
    self:SetDirection(self.direction)
end

---播放动作
---@param aniName ESpineAnimation 动作名
function SpineController:PlayAnimation(aniName)
    self.aniName = aniName
    if not IsNil(self.modelGo) and self.animator then
        self.animator.AnimationState:SetAnimation(0, "idle", true)
        self.animator.AnimationState:SetAnimation(1, "say", true)
    end
end

---设置朝向
---@param direction ESpineDirection
function SpineController:SetDirection(direction)
    self.direction = direction
    if not IsNil(self.modelGo) then

    end
end

---设置颜色
---@param color UnityEngine.Color
function SpineController:SetColor(color)
    if color == nil then return end
    self.color = color
    if not IsNil(self.modelGo) then

    end
end

function SpineController:Clear()
    AsynModelController.Clear(self)
    self.direction = ESpineDirection.Right
    self.aniName = ESpineAnimation.Idle --动画初始状态
end