---
--- 异步加载模型基类
--- Created by hukiry.
--- DateTime: 2020-02-07 16:57
---

---@class AsynModelController
AsynModelController = Class()

---@param parentGo UnityEngine.GameObject
---@param immediatelyDestroy boolean Clear时是否立即销毁
function AsynModelController:ctor(parentGo, immediatelyDestroy)
    self.parentGo = parentGo
    self.immediatelyDestroy = immediatelyDestroy or false;

    self:Clear()
end

---@param modelName string 模型名字
function AsynModelController:LoadModel(modelName)
    if self.modelName == modelName then
        return
    end
    self:Clear()
    self.modelName = modelName

    GameObjectPool.GetAsync(modelName, self.parentGo, function(pGo, name)
        if not IsNil(self.modelGo) then
            GameObjectPool.Put(self.modelPath, self.modelGo)
        end

        if IsNil(pGo) then return end
        ---@type UnityEngine.GameObject
        self.modelGo = pGo
        self.modelPath = name

        UIHelper:SetChildrenToLayer(self.modelGo, self.parentGo.layer)
        self:SetLocalScale(self.scale)
        self:SetLocalEulerAngles(self.eaVector3)
        self:OnLoadCompleteInitialization()
    end, function () return self:IsAsyncExecute() end)
end

---加载完成初始化方法
function AsynModelController:OnLoadCompleteInitialization()
    --子类复写
end

---设置缩放大小
function AsynModelController:SetLocalScale(scale)
    self.scale = scale
    if not IsNil(self.modelGo) then
        self.modelGo.transform.localScale = Vector3.New(scale, scale, scale)
    end
end

---设置旋转
---@param eaVector3 UnityEngine.Vector3
function AsynModelController:SetLocalEulerAngles(eaVector3)
    self.eaVector3 = eaVector3
    if not IsNil(self.modelGo) then
        self.modelGo.transform.localEulerAngles = eaVector3
    end
end

function AsynModelController:IsAsyncExecute()
    return not IsNil(self.parentGo)
end

function AsynModelController:Clear()
    if not IsNil(self.modelGo) and self.modelPath then
        GameObjectPool.Put(self.modelPath, self.modelGo, self.immediatelyDestroy)
        self.modelGo = nil
        self.modelPath = nil
    end

    self.eaVector3 = Vector3.zero
    self.scale = 1
    self.modelName = nil
end

-- 隐藏的时候调用
function AsynModelController:OnHideModel()
    self:Clear()
end

-- 销毁的时候调用
function AsynModelController:OnDestroy()
end