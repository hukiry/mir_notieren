---
--- AssetBundleLoader       
--- Author : hukiry     
--- DateTime 2024/8/8 14:25   
---

---@class AssetBundleLoader
local AssetBundleLoader = Class()
---初始化字段
function AssetBundleLoader:ctor()

end

---加载资源
function AssetBundleLoader:LoadAsset(assetName,abName)
    return SingleAssist.AssetBlock():LoadAssets(assetName, abName)
end

---同步加载列表资源
---@return UnityEngine.Object[]
function AssetBundleLoader:LoadAllAsset(assetName,abName)
    return SingleAssist.AssetBlock():LoadAssetsList(assetName, abName)
end

---异步加载资源
function AssetBundleLoader:LoadAsync(assetName,abName,OnLoadedCallback,loadObject)
    SingleAssist.AssetAsync():LoadAssets(assetName, abName, OnLoadedCallback, loadObject)
end

---异步加载资源列表
function AssetBundleLoader:LoadAllAsync(assetName,abName,OnLoadedCallback)
    SingleAssist.AssetAsync():LoadAllAsync(assetName, abName, OnLoadedCallback)
end

---异步加载场景
---@param onProgress function<number>
---@param onFinish function
function AssetBundleLoader:LoadSceneAsync( levelName,  abName,  onProgress,  onFinish)
    StartCoroutine(Handle(self, self._LoadSceneAsync, levelName, abName, onProgress, onFinish))
end

---@private
function AssetBundleLoader:_LoadSceneAsync(levelName, abName, onProgress, onFinish)
    local proportion, displayProgress = 0.5, 0
    if SingleAssist.AssetBundle():GetAssetBundleDataAtPath(abName) then
        local abLoadFinish = false
        self:LoadAsync(levelName, abName, function(name, obj)
            abLoadFinish = true
        end, true)

        while(not abLoadFinish) do
            displayProgress = math.min(displayProgress + 0.01, proportion)
            onProgress(displayProgress)
            WaitForEndOfFrame()
        end
    end

    local op  = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName)
    while(not op.isDone) do
        onProgress(op.progress * proportion + proportion)
        WaitForEndOfFrame()
    end

    SingleAssist.LoaderMgr():Unload(abName, 0, true)
    if onFinish then
        onFinish()
    end
end

function AssetBundleLoader:Unload(assetName, delay) end
---立即卸载资源
function AssetBundleLoader:UnloadImmediate(assetName, force)
    AssetBundleCache:UnloadImmediate(assetName .. ".ab")
end
---卸载未使用的资源
function AssetBundleLoader:UnloadUnusedAssets(gc)
    AssetBundleCache:UnloadUnuseAsset(gc)
end
---清理所有资源
function AssetBundleLoader:UnLoadAllAssets()
    AssetBundleCache:UnloadAllAsset()
end

function AssetBundleLoader:Destroy() end

return AssetBundleLoader