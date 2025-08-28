---@class GameObjectPool
GameObjectPool = {}

---@type UnityEngine.GameObject
local _poolGo = nil
---@type table<string, table<number, UnityEngine.GameObject>>
local _goPool = {}

local _timer = nil

--预处理  先加载并创建出来，放到池中，等用时，再通过Get方法来拿
---@param pPrefabName string 资源名字
function GameObjectPool.Pretreatment(pPrefabName)
	if IsEmptyString(pPrefabName) then
		print("资源名为空","red")
	end

	if _goPool[pPrefabName] == nil then
		_goPool[pPrefabName] = {}
	end

	local _go = table.remove(_goPool[pPrefabName],1) -- table.dequeue(_itemPool[pPrefabName])
	if IsNil(_go) then
		local prefab = ResManager:Load(pPrefabName)
		if prefab == nil then _go = nil end
		_go = GameObject.Instantiate(prefab)
	end
	if not IsNil(_go) then
		GameObjectPool.Put(pPrefabName, _go)
	end
end

--获取一个物体  同步加载
---@param pPrefabPath string 资源名路径
---@param pParentGo UnityEngine.GameObject 父对象
---@return UnityEngine.GameObject
function GameObjectPool.Get(pPrefabPath, pParentGo)
	if IsEmptyString(pPrefabPath) then
		print("资源名为空","red")
		return nil
	end

	if _goPool[pPrefabPath] == nil then
		_goPool[pPrefabPath] = {}
	end

	local _go = table.remove(_goPool[pPrefabPath],1) -- table.dequeue(_itemPool[pPrefabName])
	if IsNil(_go) then
		local prefab = ResManager:Load(pPrefabPath)
		if prefab == nil or IsNil(prefab) then
			ResManager:Unload(pPrefabPath)
			return nil
		end
		if not IsNil(pParentGo) then
			_go = GameObject.Instantiate(prefab)
			_go.transform:SetParent(pParentGo.transform, false)
			_go:ChangeLayer(pParentGo.layer)
		else
			_go = GameObject.Instantiate(prefab)
		end
		_go:SetActive(true)
		return _go
	else
		if not IsNil(pParentGo) then
			_go.transform:SetParent(pParentGo.transform, false)
			_go:ChangeLayer(pParentGo.layer)
		end
		_go:SetActive(true)
		return _go
	end
end

--异步获取
---@param pResName string 资源名路径
---@param pParentGo UnityEngine.GameObject 父对象
---@param pFunc function<UnityEngine.GameObject, string> 回调方法<gameObject,prefabName>
---@param pJudgeFun function 加载完毕判断是否继续返回,true的话返回Go,false return掉
function GameObjectPool.GetAsync(pResName, pParentGo, pFunc, pJudgeFun)
	if pFunc == nil then
		print("异步获取没传回调方法","red")
		return
	end
	if IsEmptyString(pResName) then
		print("资源名为空","red")
		return
	end

	if _goPool[pResName] == nil then
		_goPool[pResName] = {}
	end

	if GameObjectPool.Contains(pResName) then
		pFunc(GameObjectPool.Get(pResName, pParentGo),pResName)
		return
	end
	---@param prefab UnityEngine.GameObject
	ResManager:LoadAsync(pResName, function(name, prefab)
		if pJudgeFun ~= nil and not pJudgeFun(pResName) then
			ResManager:Unload(pResName, 0)
			return
		end
		if IsNil(prefab) then
			print(string.format("加载%s失败",pResName),"red")
			pFunc(nil, pResName)
			return
		end
		---@type UnityEngine.GameObject
		local _go = nil
		if not IsNil(pParentGo) then
			_go = GameObject.Instantiate(prefab)
			_go.transform:SetParent(pParentGo.transform,false)
			_go:ChangeLayer(pParentGo.layer)
		else
			_go = GameObject.Instantiate(prefab)
		end
		pFunc(_go,pResName)
	end)
end

--装入一个数据
---@param pPrefabName string 资源名字
---@param pGameObject UnityEngine.GameObject
---@param destroyImmediate boolean 立即销毁
function GameObjectPool.Put(pPrefabName, pGameObject, destroyImmediate)
	if IsEmptyString(pPrefabName) or IsNil(pGameObject) then
		return
	end
	if destroyImmediate then	--立即销毁
		ResManager:Unload(pPrefabName, 0)
		GameObject.Destroy(pGameObject)
		return;
	end

	if _goPool[pPrefabName] == nil then
		print(string.format("GameObjectPool.Put 非法装入: 没有获取，怎有装入? path:%s", pPrefabName),"pink")
		return
	end
	if IsNil(_poolGo) then
		_poolGo = GameObject.New("GameObjectPool")
		GameObject.DontDestroyOnLoad(_poolGo);
	end
	pGameObject.transform.parent = _poolGo.transform
	pGameObject:SetActive(false)

	table.insert(_goPool[pPrefabName], pGameObject)
end

--装入一个table 支持数组、字典
---@param pPrefabName string 资源名字
---@param pTable table
---@param destroyImmediate boolean 立即销毁
function GameObjectPool.PutTable(pPrefabName, pTable, destroyImmediate)
	if IsEmptyString(pPrefabName) or pTable == nil then
		return
	end

	for k, v in pairs(pTable) do
		GameObjectPool.Put(pPrefabName, v, destroyImmediate)
	end
	table.clear(pTable)
end

--清理掉某种对象池
function GameObjectPool.Clear(pPrefabName)
	if _goPool[pPrefabName] == nil then return end
	for k, v in pairs(_goPool[pPrefabName]) do
		ResManager:Unload(pPrefabName, 0)
		GameObject.Destroy(v)
	end
	_goPool[pPrefabName] = nil
end

--清除所有
function GameObjectPool.ClearAll(pRemoveTimer)
	for k, v in pairs(_goPool) do
		GameObjectPool.Clear(k)
	end
	if pRemoveTimer then
		if _timer ~= nil then Single.TimerManager():RemoveHandler(_timer) end
	end
end

---是否有此对象
---@return boolean
function GameObjectPool.Contains(pPrefabName)
	return GameObjectPool.GetLength(pPrefabName) > 0
end

--获取长度
---@return number length
function GameObjectPool.GetLength(pPrefabName)
	if IsEmptyString(pPrefabName) then return end
	if _goPool[pPrefabName] == nil then
		return 0
	end
	return #_goPool[pPrefabName] -- table.length(_gameObjectPool[pPrefabName])
end
