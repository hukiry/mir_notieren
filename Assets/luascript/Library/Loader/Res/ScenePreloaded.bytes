--- 场景资源预加载
--- ScenePreloaded       
--- Author : hukiry     
--- DateTime 2024/8/6 15:03   
---

---@class ScenePreloaded
local ScenePreloaded = Class()

function ScenePreloaded:ctor()
    ---加载进度回调
    ---@type function<number>
    self.progressCallback = nil
    ---加载完成
    ---@type function
    self.finishCallback = nil
    self.paths = {}
    self.cacheAssets = false
    self.doneCount = 0
end

---初始化资源
---@param paths table<string>
---@param cacheAssets boolean
function ScenePreloaded:InitPreloadedQueue(paths, cacheAssets)
    self.cacheAssets = cacheAssets or false
    self.paths = paths
end

---开始加载
function ScenePreloaded:StartPreloaded()
    for _, path in ipairs(self.paths) do
        SingleAssist.LoaderMgr():LoadAsync(path, Handle(self, self.LoadFinish));
    end
end

---@private
function ScenePreloaded:LoadFinish(path, prefab)
    if self.cacheAssets then
        SingleAssist.LoaderMgr():ToCacheAssets(path);
    end
    self.doneCount =  self.doneCount+1
    local length = #self.paths

    if self.progressCallback then
        self.progressCallback(self.doneCount/length)
    end

    if self.doneCount == length then
        if self.finishCallback then
            self.finishCallback()
        end
    end
end

---释放
function ScenePreloaded:UnloadAll()
    for _, path in ipairs(self.paths) do
        SingleAssist.LoaderMgr():Unload(path);
    end
end

return ScenePreloaded