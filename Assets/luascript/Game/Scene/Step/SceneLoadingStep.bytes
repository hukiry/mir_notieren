---
--- 场景加载阶段
--- Created huiry
--- DateTime: 2021/5/27 15:48
---

---@class SceneLoadingStep:SceneProgressBase
SceneLoadingStep = Class(SceneProgressBase)

---@param sceneName
---@param finishCallback
function SceneLoadingStep:ctor(proportion, sceneName, startCallback, finishCallback)
    self.sceneName = sceneName
    self.startCallback = startCallback
    self.finishCallback = finishCallback
end

function SceneLoadingStep:Start()
    if self.startCallback then
        self.startCallback()
    end

    ResManager:LoadSceneAsync(SceneRule.CurSceneType, function(progress)
        self.progress = progress
    end, function()
        self.isDone = true
        if self.finishCallback then
            self.finishCallback()
        end
    end)
end