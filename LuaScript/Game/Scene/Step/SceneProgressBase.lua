---
--- 场景加载进度基类
--- Created huiry
--- DateTime: 2021/5/27 15:39
---

---@class SceneProgressBase
SceneProgressBase = Class()

function SceneProgressBase:ctor(proportion)
    self.proportion = proportion    --这个阶段对进度条所占比例
    self.progress = 0   --0-1
    self.isDone = false
end

function SceneProgressBase:Start()
    --子类复写
end

---获取当前阶段进度
function SceneProgressBase:GetProgress()
    return self.progress
end

---是否完成
function SceneProgressBase:IsDone()
    return self.isDone
end

