---心跳
---@class HeartBeatMgr
local HeartBeatMgr = Class()

function HeartBeatMgr:ctor()
end

--登陆游戏时清理数据
function HeartBeatMgr:InitData()
	self:StopHeartBeat()
	---玩家无操作时间
	self.noActionTime = 0
end

---开始准备心跳
function HeartBeatMgr:StartHeartBeat()
	self.lastHeartBeatTime = 0--上一次请求心跳时间
	self.canReqHeartBeat = true;

	if GameSymbols:IsStrongNetwork() then
		self.limTime = 10
	else
		self.limTime = 60
	end

	Single.TimerManger():DoFrame(self, self.OnFrame, 1, -1)
	Single.TimerManger():DoTime(self, self.OnFrameRate, 1, -1)

	--self:PerformAHeartBeat()
end

function HeartBeatMgr:OnFrame()
	local deltaTime = UnityEngine.Time.deltaTime
	self.lastHeartBeatTime = self.lastHeartBeatTime + deltaTime

	Util.Time().serverTime = Util.Time().serverTime + deltaTime
	Util.Time().meseTime =  Util.Time().serverTime * 1000

	if self.lastHeartBeatTime >= self.limTime then	--心跳每60秒
		self.lastHeartBeatTime = 0
		self:PerformAHeartBeat()
	end

	if Input.GetMouseButtonDown(0) then
		self.noActionTime = 0
		Application.targetFrameRate = 60
	end
end

---接收心跳返回
function HeartBeatMgr:ReceiveHeartBeat(msg)
	Util.Time().serverTime = msg.timeStamp
end

---执行一次心跳
function HeartBeatMgr:PerformAHeartBeat()
	if self.canReqHeartBeat then
		Single.Request().SendHeart()
	end
end

function HeartBeatMgr:OnFrameRate()
	---帧率设置
	self.noActionTime = self.noActionTime + 1
	if self.noActionTime == 60 then --1分钟无操作
		Application.targetFrameRate = 45
	elseif self.noActionTime == 300 then --5分钟无操作
		Application.targetFrameRate = 40
	elseif self.noActionTime == 600 then --10分钟无操作
		Application.targetFrameRate = 35
	end
end

---停止心跳
function HeartBeatMgr:StopHeartBeat()
	Single.TimerManger():RemoveHandler(self)
	self.canReqHeartBeat = false;
	self.lastHeartBeatTime = 0--上一次请求心跳时间
end

return HeartBeatMgr