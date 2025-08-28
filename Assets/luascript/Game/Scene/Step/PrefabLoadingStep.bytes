---
--- 预加载资源阶段
--- Created huiry
--- DateTime: 2021/5/27 15:48
---

---@class PrefabLoadingStep:SceneProgressBase
PrefabLoadingStep = Class(SceneProgressBase)

---@param paths
---@param finishCallback
function PrefabLoadingStep:ctor(proportion, paths, finishCallback)
    self.paths = paths
    self.finishCallback = finishCallback
end

function PrefabLoadingStep:Start()
    if USE_CCHARP then
        ---@type SceneLoader  cs脚本
        self.preloaded = SceneLoader.New();
    else
        ---@type ScenePreloaded
        self.preloaded =  require("Library.Loader.Res.ScenePreloaded").New();
    end
    self.preloaded = SceneLoader.New();
    self.preloaded:InitPreloadedQueue(self.paths, false);
    self.preloaded.progressCallback = function(progress)
        self.progress = progress
    end
    self.preloaded.finishCallback = function()
        self.isDone = true
        if self.finishCallback then
            self.finishCallback()
        end
    end
    self.preloaded:StartPreloaded()
end

function PrefabLoadingStep:UnloadAll()
    self.preloaded:UnloadAll()
end