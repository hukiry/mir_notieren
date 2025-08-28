--- 资源加载  管理LoadRequest，异步加载资源
--- AssetAsyncLoader       
--- Author : hukiry     
--- DateTime 2024/8/8 14:31   
---

---@class LoadWaitingInfo
local LoadWaitingInfo = Class()

function LoadWaitingInfo:ctor()
    self.assetName = ''
    self.abName = ''
    ---集合
    ---@type table<number, function<UnityEngine.Object>>
    self.onLoadedCallback = {}
    self.loadObject = false
end

---@class AssetAsyncLoader
local AssetAsyncLoader = Class()
---最大同时处理个数
local MAX_PROGRESS = 5
---初始化字段
function AssetAsyncLoader:ctor()
    ---队列 集合
    ---@type table<number, LoadWaitingInfo>
    self.waitingQueue = {}
    ---正在加载的请求 字典
    ---@type table<string, LoadRequest>
    self.m_WaitingRequest = {}

    ---当前等待处理列表 集合
    ---@type table<number, LoadRequest>
    self.mWaitingRequest = {}
    ---当前处理列表 集合
    ---@type table<number, LoadRequest>
    self.mProgressRequest = {}
    ---是否运行更新
    self.KeepRunning = false
end

function AssetAsyncLoader:Awake()
    Single.TimerManger():RemoveHandler(self)
    Single.TimerManger():DoFrame(self, self.Update,1,-1)
end

---异步加载资源
---@param onLoadedCallback function<UnityEngine.Object>
function AssetAsyncLoader:LoadAssets(assetName, abName, onLoadedCallback, loadObject)
    --log("异步加载资源：当前等待数：" , #self.waitingQueue, "正在加载数：" , table.length(self.m_WaitingRequest))
    self:AddOneLoadAsync(assetName, abName, onLoadedCallback, loadObject)
end

---@private
function AssetAsyncLoader:AddOneLoadAsync(assetName, abName, onLoadedCallback, loadObject)
    local has = false
    for i, v in ipairs(self.waitingQueue) do
        if v.assetName == assetName then
            table.insert(v.onLoadedCallback, onLoadedCallback)
            has = true
        end
    end

    if not has then
        local info = LoadWaitingInfo.New()
        info.assetName = assetName;
        info.abName = abName;
        info.onLoadedCallback = {}
        table.insert(info.onLoadedCallback, onLoadedCallback)
        info.loadObject = loadObject;
        table.insert(self.waitingQueue, info)
    end
end

---每帧更新
---@private
function AssetAsyncLoader:Update()
    if #self.m_WaitingRequest == 0 and #self.waitingQueue>0 then
        local info = table.remove(self.waitingQueue)
        StartCoroutine(Handle(self, self.LoadAssetAsync,info.assetName, info.abName, info.onLoadedCallback, info.loadObject))
    end
end

---异步加载资源
---@private
function AssetAsyncLoader:LoadAssetAsync(assetName, abName, onLoadedCallback, loadObject)
    local holder = AssetBundleCache:FindAssetBundle(abName)
    if holder == nil then
        local request = self:CreateRequest(abName, true)
        self:StartLoopUpdate()
        while (self.KeepRunning) do
            WaitForFixedUpdate()
        end

        holder = AssetBundleCache:FindAssetBundle(abName)
        request:DelReference(abName)
    end

    if holder then
        if onLoadedCallback then
            if holder.IsStreamed then
                for i, fun in ipairs(onLoadedCallback) do
                    fun(assetName, nil)
                end
                return
            end

            if not loadObject then
                for i, fun in ipairs(onLoadedCallback) do
                    fun(assetName, nil)
                end
                return
            end

            if not holder:HasLoaded(assetName) then
                local abReq = holder:LoadAsync(assetName)
                while(not abReq.isDone) do
                    Yield(abReq)
                end
                holder:SetAsyncLoad(assetName, abReq.asset)
            end

            for i, fun in ipairs(onLoadedCallback) do
                fun(assetName, holder:LoadAsset(assetName))
            end
        end
        WaitForFixedUpdate()
        return
    end

    logError("AssetAsyncLoader manifestmap文件清单中找不到， 异步资源加载失败!", assetName)
    if onLoadedCallback then
        for i, fun in ipairs(onLoadedCallback) do
            fun(assetName, nil)
        end
    end

    SingleAssist.AssetBlock():FailReleaseLoaded(assetName, abName)
end


---异步加载资源列表
---@private
---@param onLoadedCallback function<UnityEngine.Object>
function AssetAsyncLoader:LoadAllAsync(assetName, abName, onLoadedCallback)
    StartCoroutine(Handle(self, self.LoadAssetListAsync, assetName, abName, onLoadedCallback))
end
---@private
function AssetAsyncLoader:LoadAssetListAsync(assetName, abName, onLoadedCallback)
    local holder = AssetBundleCache:FindAssetBundle(abName)
    if holder == nil then
        local request = self:CreateRequest(abName, true)
        self:StartLoopUpdate()
        while (self.KeepRunning) do
            WaitForFixedUpdate()
        end
        request:DelReference(abName)
        holder = AssetBundleCache:FindAssetBundle(abName)
    end

    if holder then
        if holder.IsStreamed then
            onLoadedCallback(assetName, nil)
            return
        end

        if not holder:HasLoaded() then
            local abReq = holder:LoadAllAsync()
            while(not abReq.isDone) do
                Yield(abReq)
            end
            holder:SetAllAsyncLoad(assetName, abReq.allAssets)
        end

        if onLoadedCallback then
            onLoadedCallback(assetName, holder:LoadAllAssets())
        end
        WaitForFixedUpdate()
        return
    end

    logError("manifestmap文件清单中找不到， 异步资源加载失败!", assetName)
    if onLoadedCallback then
        onLoadedCallback(assetName, nil)
    end

    SingleAssist.AssetBlock():FailReleaseLoaded(assetName, abName)
end

---查找加载中的
function AssetAsyncLoader:FindLoading(abName)
    return self.m_WaitingRequest[abName]
end

---移除加载中的
function AssetAsyncLoader:RemoveLoading(abName)
     self.m_WaitingRequest[abName] = nil
end
---@private
---是否有子节点加载失败
function AssetAsyncLoader:HasChildErrorOrCancel(abName)
    local dependencies = SingleAssist.AssetBlock():GetDependencies(abName)
    for i, childname in ipairs(dependencies) do
        if self:HasChildErrorOrCancel(childname) then
            return true
        end

        local request = self.m_WaitingRequest[childname]
        if request then
            if not IsEmptyString(request:Error()) then
                return true
            end
        end
    end
    return false
end

---创建请求, 不是携程返回
---@private
---@return LoadRequest
function AssetAsyncLoader:CreateRequest(abName, root)
    local md = SingleAssist.AssetBlock():GetManifestData(abName)
    local request = self.m_WaitingRequest[abName]
    if request then
        if root then
            request:AddRefParent(abName)
        end
        return request
    end

    ---@type LoadRequest
    local loading = require("Library.Loader.Res.ABLoader.LoadRequest.ABRequest").New(abName)
    if md:IsRemote() then
        loading = require("Library.Loader.Res.ABLoader.LoadRequest.ABMemoryRequest").New(md)
    end

    if root then
        loading:AddRefParent(abName)
    end
    self.m_WaitingRequest[abName] = loading
    table.insert(self.mWaitingRequest, loading)

    local dependencies = SingleAssist.AssetBlock():GetDependencies(abName)
    for i, abname in ipairs(dependencies) do
        local holder = AssetBundleCache:FindAssetBundle(abname)
        if holder then
            holder:AddRefParent(abName)
        else
            local newReq = self:CreateRequest(abname, false)
            newReq:AddRefParent(abName)
        end
    end

    return loading
end

---@private
---启动更新
function AssetAsyncLoader:StartLoopUpdate()
    if self.KeepRunning then
        return
    end
    self.KeepRunning = true
    StartCoroutine(Handle(self, self.OnLoopUpdate))
end
---@private
---启动更新
function AssetAsyncLoader:OnLoopUpdate()
    while (self.KeepRunning) do
        local execTaskIndex = #self.mProgressRequest
        while(execTaskIndex>0) do
            local curRequest = self.mProgressRequest[execTaskIndex]
            if not curRequest:IsLoading() then
                table.remove(self.mProgressRequest, execTaskIndex)
                curRequest:OnAssetsLoaded()
            end
            execTaskIndex = execTaskIndex-1
        end

        local waitingIndex = #self.mWaitingRequest
        while(#self.mProgressRequest < MAX_PROGRESS and waitingIndex>0) do
            local curRequest = self.mWaitingRequest[waitingIndex]
            if AssetBundleCache:IsAllDependicesReady(curRequest.abName) then
                if AssetBundleCache:IsExist(curRequest.abName) then
                    curRequest:OnAssetsLoaded()
                    table.remove(self.mWaitingRequest, waitingIndex)
                else
                    curRequest:CreateRequest()
                    table.insert(self.mProgressRequest, curRequest)
                    table.remove(self.mWaitingRequest, waitingIndex)
                end
            elseif self:HasChildErrorOrCancel(curRequest.abName) then
                curRequest:OnAssetsLoaded()
                table.remove(self.mWaitingRequest, waitingIndex)
            end
            waitingIndex = waitingIndex-1
        end

        self.KeepRunning = #self.mWaitingRequest>0 or #self.mProgressRequest>0
        if self.KeepRunning then
            WaitForEndOfFrame()
        else
            break
        end
    end
end

function AssetAsyncLoader:OnDestroy()
    Single.TimerManger():RemoveHandler(self)
end

return AssetAsyncLoader