---
--- 模型控制器
--- Created by hukiry.
--- DateTime: 2020-02-07 16:57
---

---@class ModelController:AsynModelController
ModelController = Class(AsynModelController)

---@param parentGo UnityEngine.GameObject
---@param immediatelyDestroy boolean 使用完后是否立即销毁
function ModelController:ctor(parentGo, immediatelyDestroy)
end

---加载完成初始化方法
function ModelController:OnLoadCompleteInitialization()
    ---@type UnityEngine.Animator
    self.animator = self.modelGo:GetComponent("Animator")
    self:PlayAnimation(self.aniName)
    self:SetColor(self.color)
end

--播放动作
---@param aniName EAnimation 动作名
function ModelController:PlayAnimation(aniName)
    self.aniName = aniName
    if not IsNil(self.modelGo) and self.animator then
        self.animator:Play(aniName, 0, 0)
    end
end

---设置颜色
---@param color UnityEngine.Color
function ModelController:SetColor(color)
    if color == nil then return end
    self.color = color
    if not IsNil(self.modelGo) then
        if self.meshRendererList == nil then
            ---@type table<number, UnityEngine.Renderer>
            self.meshRendererList = self.modelGo:GetComponentsInChildren(typeof(UnityEngine.Renderer), true)
        end

        for i = 0, self.meshRendererList.Length - 1  do
            local materials = self.meshRendererList[i].materials
            for j = 0, materials.Length - 1 do
                materials[j].color = color
            end
        end
    end
end

function ModelController:Clear()
    AsynModelController.Clear(self)
    self.aniName = EAnimation.Idle --动画初始状态
    self.meshRendererList = nil
    self.animator = nil
end