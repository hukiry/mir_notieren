---
--- 计时器   正常时间行走  受Timer.TimeScale影响
--- Created by hukiry.
--- DateTime: 2020/7/14 14:31
---


---@class TimerManger
local TimerManger = Class()

function TimerManger:ctor()
	---@type table<number, TimerHandler>
	self._handlers = {}
	self._currFrame = 0
	self.UpdateBeat = UpdateBeat
	self.UpdateBeat:Add(self.Update, self)
end

---@private
function TimerManger:Update()
	self._currFrame = self._currFrame + 1;
	local index = 1
	local currTime = self:GetCurrentTime()
	local len = #self._handlers	--循环开始时的长度
	while (index <= len) do
		local tempLen = #self._handlers
		if len ~= tempLen then	--如果循环长度与实际长度不符，重新执行
			len = tempLen
			index = 1
		end
		local handler = self._handlers[index];
		if handler then
			local t = handler.useFrame and self._currFrame or currTime
			while (t >= handler.exeTime) do
				handler.exeTime = handler.exeTime + handler.delay;
				handler.loopCount = handler.loopCount == -1 and -1 or handler.loopCount - 1

				local ok, errors = xpcall(handler.callbackFunc, debug.traceback)
				if not ok then
					error(errors, 2)
				end

				if not ok or handler.loopCount == 0 then
					local result = self:RemoveHandler(handler.listener, handler.method);
					if result then
						len = len - 1
						index = index - 1
					end
					break
				end
			end
		end
		index = index + 1
	end
end

---@private
function TimerManger:Create(listener, method, useFrame, delay, loopCount)
	if loopCount == nil then loopCount = 1 end
	if delay == 0 or loopCount == 0 then
		method(listener, loopCount)
		return
	end

	local createFlag = false
	---@type TimerHandler
	local handler = self:FindHandlerByMethod(listener, method)
	if handler == nil then
		handler = require("Library.Timer.TimerHandler").New()
		createFlag = true
	end

	handler.listener = listener
	handler.useFrame = useFrame
	handler.delay = delay
	handler.method = method
	handler.loopCount = loopCount
	handler.exeTime = delay + (useFrame and self._currFrame or self:GetCurrentTime())
	handler.callbackFunc = function()
		handler.method(handler.listener, handler.loopCount)
	end
	if createFlag then
		table.insert(self._handlers, handler)
	end
	return method
end

---定时执行(基于秒)
---@param listener self 监听者
---@param method function 回调函数的方法
---@param second number 间隔执行时间(秒)
---@param loopCount number 循环次数，默认只执行一次(-1是无限循环)
function TimerManger:DoTime(listener, method, second, loopCount)
	return self:Create(listener, method, false, second, loopCount)
end

---定时执行(基于帧率)
---@param listener self 监听者
---@param method function 回调函数的方法
---@param frame number 间隔多少帧执行
---@param loopCount number 循环次数，默认只执行一次(-1是无限循环)
function TimerManger:DoFrame(listener, method, frame, loopCount)
	return self:Create(listener, method, true, frame, loopCount)
end

---清理定时器
---@param listener TimerHandler 监听者
---@param method function 要清理的具体函数(如果不传，清理listener下的所有监听)
function TimerManger:RemoveHandler(listener, method)
	if listener == nil then
		return
	end

	if method == nil then
		local i = 1
		while(i <= #self._handlers) do
			local handler = self._handlers[i]
			if handler.listener == listener then
				local handler = table.remove(self._handlers, i)
				handler:Clear()
				i = i - 1
			end
			i = i + 1
		end
	else
		local index = self:FindIndexByMethod(listener, method)
		if index > 0 then
			local handler = table.remove(self._handlers, index)
			handler:Clear()
		end
	end
end

---清理所有定时器
function TimerManger:RemoveAllHandler()
	for k, v in ipairs(self._handlers) do
		v:Clear()
	end
	table.clear(self._handlers)
end

---游戏自启动运行时间(真实时间，不受加速限制)，秒
function TimerManger:GetCurrentTime()
	return Time.time
end

---查找一个回调
---@param method function
---@return number 返回所有索引
function TimerManger:FindIndexByMethod(listener, method)
	for i = 1, #self._handlers do
		local handler = self._handlers[i]
		if handler.listener == listener and handler.method == method then
			return i
		end
	end
	return 0
end

---查找一个回调
---@param method function
---@return number 返回所有索引
function TimerManger:FindHandlerByMethod(listener, method)
	for i = 1, #self._handlers do
		local handler = self._handlers[i]
		if handler.listener == listener and handler.method == method then
			return handler
		end
	end
	return nil
end

--移除update帧听
function TimerManger:Dispose()
	self.UpdateBeat:Remove(self.Update, self)
end

return TimerManger