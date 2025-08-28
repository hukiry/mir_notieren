---
--- 龙骨控制器
--- Created by hukiry.
--- DateTime: 2021-01-18 16:57
---

---@class ArmatureController:AsynModelController
ArmatureController = Class(AsynModelController)

---@param parentGo UnityEngine.GameObject
---@param immediatelyDestroy boolean 使用完后是否立即销毁(不放回对象池)
function ArmatureController:ctor(parentGo, immediatelyDestroy)
end

---加载完成初始化方法
function ArmatureController:OnLoadCompleteInitialization()
    ---@type DragonBones.UnityArmatureComponent
    self.armatureComponent = self.modelGo:GetComponent("UnityArmatureComponent")
    self:PlayAnimation(self.aniName)
    self:SetColor(self.color)
    self:SetFlipXY(self.flipX, self.flipY)
end

---播放动作
---@param aniName ESpineAnimation 动作名
function ArmatureController:PlayAnimation(aniName)
    self.aniName = aniName
    if not IsNil(self.modelGo) and self.armatureComponent then
        self.armatureComponent.animation:Play(self.aniName, 0)
    end
end

---设置朝向
---@param flipX boolean
---@param flipY boolean
function ArmatureController:SetFlipXY(flipX, flipY)
    self.flipX = flipX
    self.flipY = flipY
    if not IsNil(self.modelGo) then
        self.armatureComponent.armature.flipX = flipX
        self.armatureComponent.armature.flipY = flipY
    end
end

---设置颜色
---@param color UnityEngine.Color
function ArmatureController:SetColor(color)
    if color == nil then return end
    self.color = color
    if not IsNil(self.modelGo) then

    end
end

function ArmatureController:Clear()
    AsynModelController.Clear(self)
    self.flipX = false
    self.flipY = false
    self.aniName = ""
end