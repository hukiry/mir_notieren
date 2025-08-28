---
--- SceneItemPool       
--- Author : hukiry     
--- DateTime 2022/12/19 19:10   
---

require "Library.Pool.ScenePoolRule"

---场景对象池管理
---@class SceneItemPool
SceneItemPool = {}
--获取一个对象
---@param pItemType SceneItemType 对象类型 必须传
---@param pParentGo UnityEngine.GameObject 父对象 必须传
---@param ... table 初始化参数,由Start()方法接收，可传可不传
---@return DisplayObjectBase
function SceneItemPool.Get(pItemType, pParentGo, ...)
    if ScenePoolRule[pItemType or 0] == nil or pParentGo == nil then
        logError("SceneItemPool 获取对象池错误  pItemType:", pItemType)
        return nil
    end
    local _go = GameObjectPool.Get(ScenePoolRule[pItemType].resPath, pParentGo)
    ---@type IUIItem
    local _return = require(ScenePoolRule[pItemType].luaClass).New(_go)
    _return.itemCtrl = require(ScenePoolRule[pItemType].luaClass.."Control").New(_go)
    _return:Start(...)
    return _return
end

---加载一个对象:不需要对象回收
---@param pItemType SceneItemType 对象类型 必须传
---@param _selfGo UnityEngine.GameObject 自己必须传
---@param ... table 初始化参数,由Start()方法接收，可传可不传
---@return DisplayObjectBase
function SceneItemPool.GetObject(pItemType, _selfGo, ...)
    ---@type IUIItem
    local _return = require(ScenePoolRule[pItemType].luaClass).New(_selfGo)
    _return.itemCtrl = require(ScenePoolRule[pItemType].luaClass.."Control").New(_selfGo)
    _return:Start(...)
    return _return
end

--装入一个数据
---@param pItemType SceneItemType
---@param pItem IUIItem
function SceneItemPool.Put(pItemType, pItem, destroyImmediate)
    if pItemType == nil or pItem == nil or IsNil(pItem.gameObject) then --损坏的，不要
        return
    end
    if pItem.OnDisable ~= nil then
        pItem:OnDisable()
    end
    GameObjectPool.Put(ScenePoolRule[pItemType].resPath, pItem.gameObject, destroyImmediate)
end

--装入一个table 支持数组、字典
---@param pItemType SceneItemType
---@param pTable table<number, IUIItem>
---@param destroyImmediate boolean 是否立即消耗
function SceneItemPool.PutTable(pItemType, pTable, destroyImmediate)
    for k, v in pairs(pTable) do
        SceneItemPool.Put(pItemType, v, destroyImmediate)
    end
    table.clear(pTable)
end

--清理掉某种对象池中的对象
---@param pItemType SceneItemType
function SceneItemPool.Destory(pItemType)
    GameObjectPool.Clear(ScenePoolRule[pItemType].resPath)
end

