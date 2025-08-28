---
--- SingleAssist       
--- Author : hukiry     
--- DateTime 2024/8/8 17:53   
--- Library.Loader.Res.ABLoader | Library.Loader.Res.Assist


require("Library.Loader.Res.ABLoader.LoadRequest.LoadRequest")
require("Library.Loader.Res.ABLoader.AssetBundleCache")
require("Library.Loader.Res.ABLoader.RefObject")

---@class SingleAssist
SingleAssist = {}
---@type table<string,table>
local m_list = {}
---导入lua
---@private
---@param className string
---@param classPath string
function SingleAssist.ImportClass(className, classPath)
    if m_list[className] == nil then
        local classObj = require(classPath)
        if classObj.New then
            m_list[className] = classObj.New()
        else
            m_list[className] = classObj
        end

        if m_list[className].Awake then
            m_list[className]:Awake()
        end
    end
    return m_list[className]
end

function SingleAssist.Clear()
    for i, v in pairs(m_list) do
        if v.OnDestroy then
            v:OnDestroy()
        end
    end
    table.clear(m_list)
end

---AssetBundle 管理
---@return AssetBundleManager
function SingleAssist.AssetBundle()
    return SingleAssist.ImportClass("AssetBundleManager", "Library.Loader.Res.Assist.AssetBundleManager")
end

---资源加载助手
---@return AssetsLoaderAssist
function SingleAssist.AssetsLoader()
    return SingleAssist.ImportClass("AssetsLoaderAssist", "Library.Loader.Res.Assist.AssetsLoaderAssist")
end

---资源
---@return ResourcesLoader
function SingleAssist.Resources()
    return SingleAssist.ImportClass("ResourcesLoader", "Library.Loader.Res.Assist.ResourcesLoader")
end

---资源块
---@return AssetBlockAssist
function SingleAssist.AssetBlock()
    return SingleAssist.ImportClass("AssetBlockAssist", "Library.Loader.Res.Assist.AssetBlockAssist")
end

---异步加载资源
---@return AssetAsyncLoader
function SingleAssist.AssetAsync()
    return SingleAssist.ImportClass("AssetAsyncLoader", "Library.Loader.Res.ABLoader.AssetAsyncLoader")
end

---资源加载管理
---@return AssetsLoaderMgr
function SingleAssist.LoaderMgr()
    return SingleAssist.ImportClass("AssetsLoaderMgr", "Library.Loader.Res.Weg.AssetsLoaderMgr")
end