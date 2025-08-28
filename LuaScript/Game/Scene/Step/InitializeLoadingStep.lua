---
--- 进入场景后的初始化阶段
--- Created by Hukiry.
--- DateTime: 2021/5/27 18:04
---

---@class InitializeLoadingStep:SceneProgressBase
InitializeLoadingStep = Class(SceneProgressBase)

function InitializeLoadingStep:ctor(proportion)
end

function InitializeLoadingStep:Start()
end

---自定义进度
function InitializeLoadingStep:SetProgress(progress)
    self.progress = progress
end

---自定义为完成阶段
function InitializeLoadingStep:SetDone()
    self.progress = 1
    self.isDone = true
end