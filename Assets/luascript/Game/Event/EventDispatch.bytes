--- 事件派发
--- UIEvent
--- Author : hukiry
---
require ("Game.Event.UIEvent")

---@class EventDispatch
EventDispatch = {}

---添加观察者
---@param listenner self
---@param listenername number
---@param functionname function
function EventDispatch:Register(listenner, listenername, functionname)
	--观察者事件列表
	---@type table<number, table<self, function>>
	if self.LuaEventList == nil then
		self.LuaEventList = {};
	end

	if listenner and listenername and functionname then
		if self.LuaEventList[listenername] == nil then
			self.LuaEventList[listenername] = {}
		end
		if self.LuaEventList[listenername][listenner] then
			log("重复添加观察者，请检查！listenername:", listenername, " -listenner:", listenner, " -functionname:", functionname)
		end
		self.LuaEventList[listenername][listenner] = functionname
	end
end

---取消观察者
---@param listenner self
---@param listenername number
function EventDispatch:UnRegister(listenner, listenername)
	if listenner and listenername then
		if self.LuaEventList[listenername] then
			self.LuaEventList[listenername][listenner] = nil
		end
	else
		for _, v in pairs(self.LuaEventList) do
			for _listenner, _ in pairs(v) do
				if _listenner == listenner then
					v[_listenner] = nil
				end
			end
		end
	end
end

function EventDispatch:ClearAll()
	self.LuaEventList = nil; 
	self.LuaEventList = {};
end

---推送消息
---@param listenername UIEvent
---@param ... {...}
function EventDispatch:Broadcast(listenername, ...)
	if self.LuaEventList == nil then
		self.LuaEventList = {};
	end

	local eventList = self.LuaEventList[listenername]
	if eventList == nil then return end;

	for listenner, _function in pairs(eventList) do
		if _function then
			_function(listenner, ...)
		end
	end
end

---获取监听的实例
---@return table<self, function>
function EventDispatch:Find(listenername)
	return self.LuaEventList[listenername];
end

---登出游戏时
function EventDispatch:RemoveAllEvent()
	if self.LuaEventList == nil then
		self.LuaEventList = {};
	end

	for listenername, v in pairs(self.LuaEventList) do
		for _, functionname in pairs(v) do
			if listenername ~= UIEvent.Scene_Load_Finish  and listenername ~= ViewID.Loading then
				log("------请检查，登出游戏时，发现未注销的事件，listenername:", listenername, " -functionname:", functionname, "red")
			end
		end
	end
end

