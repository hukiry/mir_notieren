ResManager = {}

---@type Hukiry.AssetsLoaderMgr|AssetsLoaderMgr
local loaderMgr  = Hukiry.AssetsLoaderMgr
local loaderMgrLua = SingleAssist.LoaderMgr()


--同步加载资源
function ResManager:Load(assetName)
	if USE_CCHARP then
		return loaderMgr.LoadAsset(assetName);
	else
		return loaderMgrLua:LoadAsset(assetName);
	end
end

--同步加载资源
function ResManager:LoadAll(assetName)
	if USE_CCHARP then
		return loaderMgr.LoadAllAsset(assetName);
	else
		return loaderMgrLua:LoadAllAsset(assetName);
	end

end

--异步加载资源
--@param OnLoadedCallback(assetName, object)
function ResManager:LoadAsync(assetName, onLoadedCallback)
	if USE_CCHARP then
		loaderMgr.LoadAsync(assetName, onLoadedCallback, true);
	else
		loaderMgrLua:LoadAsync(assetName, onLoadedCallback, true);
	end

end

--异步加载资源
--@param OnLoadedCallback(assetName, object[])
function ResManager:LoadAllAsync(assetName, onLoadedCallback)
	if USE_CCHARP then
		loaderMgr.LoadAllAsync(assetName, onLoadedCallback);
	else
		loaderMgrLua:LoadAllAsync(assetName, onLoadedCallback);
	end

end

--异步加载场景
--@param onProgress(float pencent)
function ResManager:LoadSceneAsync(levelName, onProgress, preloaded)
	if USE_CCHARP then
		loaderMgr.LoadSceneAsync(levelName, onProgress, preloaded or nil);
	else
		loaderMgrLua:LoadSceneAsync(levelName, onProgress, preloaded or nil);
	end

end

--通过Resources.LoadAsync异步加载MP4
--@param OnLoadedCallback(assetName, object)
function ResManager:LoadAsyncMp4(assetName, onLoadedCallback)
	SingleAssist.Resources():LoadAsyncMp4(assetName, onLoadedCallback)
end

--释放Resources资源加载的对象
--@param UnityEngine.Object
function ResManager:ResourcesUnload(object)
	SingleAssist.Resources():Unload(object)
end

---释放资源
---@param delay number 默认为0
function ResManager:Unload(assetName, delay)
	if USE_CCHARP then
		loaderMgr.Unload(assetName, delay or 0, false);
	else
		loaderMgrLua:Unload(assetName, delay or 0, false);
	end

end

--立即释放资源
function ResManager:UnloadImmediate(assetName, force)
	if USE_CCHARP then
		loaderMgr.UnloadImmediate(assetName, force);
	else
		loaderMgrLua:UnloadImmediate(assetName, force);
	end

end

--释放未使用的资源
function ResManager:UnloadUnusedAssets(gc)
	if USE_CCHARP then
		loaderMgr.UnloadUnusedAssets(gc);
	else
		loaderMgrLua:UnloadUnusedAssets(gc);
	end

end

--资源所有资源
function ResManager:UnLoadAllAssets()
	if USE_CCHARP then
		loaderMgr.UnLoadAllAssets()
	else
		loaderMgrLua:UnLoadAllAssets();
	end

end