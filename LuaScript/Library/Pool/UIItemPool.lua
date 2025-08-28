
--- UI对象池 含GameObject对象管理
--- Created Hukiry.
--- DateTime: 2017\11\18 0018 17:25

---@class UIItemPool
UIItemPool = {}
local _itemPool = {}

---获取一个对象：需要对象回收
---@param pItemType UIItemType|SceneItemType 对象类型 必须传
---@param pParentGo UnityEngine.GameObject 父对象 必须传
---@param ... table 初始化参数,由Start()方法接收，可传可不传
---@return DisplayObjectBase
function UIItemPool.Get(pItemType, pParentGo, ...)
	if ItemPoolRule[pItemType or 0] == nil then
		logError("获取对象池错误  pItemType:", pItemType)
		return nil
	end
	if _itemPool[pItemType] == nil then
		_itemPool[pItemType] = {}
	end
	if #_itemPool[pItemType] == 0 then
		local _go
		if not IsNil(pParentGo) then
			_go = GameObject.Instantiate(ResManager:Load(ItemPoolRule[pItemType].resPath))
			_go.transform:SetParent(pParentGo.transform, false)
			_go:ChangeLayer(pParentGo.layer)
		else
			_go = GameObject.Instantiate(ResManager:Load(ItemPoolRule[pItemType].resPath))
		end
		---@type IUIItem		
		local _return = require(ItemPoolRule[pItemType].itemClass).New(_go)
		_return.itemCtrl = require(ItemPoolRule[pItemType].itemClass.."Control").New(_go)
		_return:Start(...)
		return _return
	else
		local _return = table.remove(_itemPool[pItemType], 1)
		if _return == nil or IsNil(_return.gameObject) then
			return UIItemPool.Get(pItemType, pParentGo, ...)
		else
			if not IsNil(pParentGo) then
				_return.transform:SetParent(pParentGo.transform, false)
				_return.gameObject:ChangeLayer(pParentGo.layer)
			end
			_return.gameObject:SetActive(true)
			_return:Start(...)
			return _return
		end
	end
end

---加载一个对象:不需要对象回收
---@param pItemType UIItemType 对象类型 必须传
---@param pItemGo UnityEngine.GameObject 自己必须传
---@param ... table 初始化参数,由Start()方法接收，可传可不传
---@return DisplayObjectBase
function UIItemPool.GetObject(pItemType, pItemGo, ...)
	---@type IUIItem
	local _return = require(ItemPoolRule[pItemType].itemClass).New(pItemGo)
	_return.itemCtrl = require(ItemPoolRule[pItemType].itemClass.."Control").New(pItemGo)
	_return:Start(...)
	return _return
end

--装入一个数据
---@param pItemType UIItemType
---@param pItem IUIItem
---@param resetGray boolean 如果需要重置灰色
function UIItemPool.Put(pItemType, pItem, resetGray)
	if pItemType == nil or pItem == nil or IsNil(pItem.gameObject) then --损坏的，不要
		return
	end

	if pItem.OnDisable ~= nil then
		pItem:OnDisable()
	end

	pItem.gameObject.transform.parent = UIManager:GetPool()
	pItem.gameObject:SetActive(false)

	if resetGray ~= nil then
		--pItem.gameObject:SetIsGray(resetGray)
	end

	if _itemPool[pItemType] == nil then --对象是在获取处初始化的
		logError("装入对象池数据错误 pItemType:", pItemType, " ，没有获取怎么会有装入？")
		return
	end
	table.insert(_itemPool[pItemType], pItem)
end

--装入一个table 支持数组、字典
---@param pItemType UIItemType
---@param pTable table
---@param resetGray boolean 如果需要重置灰色，就传true
function UIItemPool.PutTable(pItemType, pTable, resetGray)
	if pItemType == nil then
		logError("装入对象池数据错误  pItemType:", pItemType)
		return nil
	end
	if pTable == nil then
		return nil
	end

	if _itemPool[pItemType] == nil then
		return
	end

	for k, v in pairs(pTable) do
		UIItemPool.Put(pItemType, v, resetGray)
	end
	table.clear(pTable)
end

--清理掉某种对象池中的对象
function UIItemPool.Destory(pItemType)
	if _itemPool[pItemType]== nil then return end
	local tab = _itemPool[pItemType]
	for k, v in pairs(tab) do
		if v ~= nil and not IsNil(v.gameObject) then
			if v.OnDestroy ~= nil then
				v:OnDestroy()
			end
			ResManager:Unload(ItemPoolRule[pItemType].resPath, 0)
			GameObject.Destroy(v.gameObject)
		end
	end
	table.clear(tab)
end

--切换到登录场景时用到
function UIItemPool.DestroyAll()
	for k, v in pairs(_itemPool) do
		UIItemPool.Destory(k)
	end
end

