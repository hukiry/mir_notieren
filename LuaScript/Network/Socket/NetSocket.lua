
---socket协议配置
---@class NetSocketProto
NetSocketProto = {
	[10] = "Network.Proto.login_10_pb",
	[11] = "Network.Proto.mail_11_pb",
	[12] = "Network.Proto.activity_12_pb",
	[13] = "Network.Proto.common_13_pb",
	[14] = "Network.Proto.shop_14_pb",
	[15] = "Network.Proto.friend_15_pb",
	[16] = "Network.Proto.meta_16_pb",
	[19] = "Network.Proto.error_19_pb",
}

---菊花类型
---@class ECircleNetSocket
ECircleNetSocket = {
	---4秒后自动关闭
	AutoClose = 1,
	---指定时间关闭
	DelayClose = 2,
	---必须手动关闭
	HandleClose = 3,
}

---@class NetSocketCmd
NetSocketCmd = {
	-------------common-------------
	[1502] = {isWait = 0, isTips = 1},
	--[1101] = {isWait = 0, isTips = 1},
}

---@class NetSocket
NetSocket = {}
function NetSocket:Init()
	self.isOnceError = false
	--用于存储客户端发送给服务端的数据
	self:ResetSerialNumber()
	NetManager.ins:SetOnSocketConnectedFunction(Handle(self, self.OnConnected))
	NetManager.ins:SetOnSocketErrorFunction(Handle(self, self.OnSocketError))
	NetManager.ins:SetOnRecvPacketFunction(Handle(self, self.OnRecvPacketCallback))
end

---连接网络NetError
function NetSocket:Connect(host, port)
	self.addressHost = host
	self.addressPort = port
	self.isOnceError = false
	NetManager.ins:OpenConnection(host, port);--打开链接
end

---网络连接成功
function NetSocket:OnConnected()
	Single.Reconnection():OnConnected() ---链接成功：停止重连
	Single.Request().SendLogin(SceneRule:IsInLoginScene())
	Single.HeartBeat():StartHeartBeat()---心跳
	self:SocketRepack()
end

---网络发生错误
---SocketError.ConnectionReset | SocketError.ConnectionAborted属于服务端主动断开
function NetSocket:OnSocketError(err, code, msg)
	logError("Error:", err, code, msg)
	if self.isOnceError then
		return
	end
	self.isOnceError = true

	Single.HeartBeat():StopHeartBeat()
	Single.Reconnection():TryAutoReconnect()	--开始重连
end

---接收到消息包
---@param buffer System.Byte[]
function NetSocket:OnRecvPacketCallback(buffer, cmd)
	if self:IsNeedWait(cmd) then
		UIManager:CloseWindow(ViewID.ServerTip)
	end

	self.cachePacketList[cmd] = nil--移除缓存包
	---@type MessageResponse
	local responseClass = require("Network.Message.MessageResponse")
	---返回消息体
	if responseClass then
		local OnPacketRecvCallback = responseClass.OnReceive
		---@type BaseProtobuf
		local msg = self:pb(cmd, false);
		---@type Hukiry.Socket.ByteBlock
		local byteBlock = Hukiry.Socket.ByteBlock.New()
		byteBlock:Write(buffer)
		byteBlock.Pos = 0
		msg:ParseFromString(byteBlock)

		---转发错误码
		if cmd == protocal.MSG_1901 then
			if responseClass.OnErrorCode then
				---发送缓存数据
				local data = self.reqMsgTable[msg.cmd]
				self.reqMsgTable[msg.cmd] = nil
				responseClass.OnErrorCode(msg.cmd, data, self:IsNeedErrorTips(cmd))
			end
		else
			---发送缓存数据
			local data = self.reqMsgTable[cmd]
			self.reqMsgTable[cmd] = nil
			if cmd == protocal.MSG_1002 then
				data = self.reqMsgTable[protocal.MSG_1001]
				self.reqMsgTable[protocal.MSG_1001]=nil
			end
			OnPacketRecvCallback(cmd, msg, data)
		end
	else
		logError("接收包 cmd", cmd, "Len:", buffer.Length);
	end
end

---断开网络
---@param exception boolean 是否抛出异常
function NetSocket:Disconnect(code, exception)
	logError(code, exception);
	NetManager.ins:CloseConnection(code, exception);
end

------------------------------------------------------------------------
---获取定义的消息体
---@param cmd number
function NetSocket:pb(cmd, isReq)
	local intCmd = math.floor(cmd/100)
	if NetSocketProto[intCmd] == nil then
		logError("尝试获取一个未监听的模块 module =", intCmd);
		return;
	end

	if NetSocketProto[intCmd] then
		require(NetSocketProto[intCmd])
	end

	local cmdMessage = _G["msg_"..cmd]()
	return cmdMessage
end

---发送网络包
---@param cmd number
---@param isTipNet boolean 是否提示网络状态
function NetSocket:Send(cmd, packet, pData, isTipNet)
	if not GameSymbols:IsEnableNetwork() then
		return
	end

	if packet == nil then
		logError(string.format("尝试发送一个空包!!! cmd:%s  packet:%s", cmd, tostring(packet)))
		return
	end

	if isTipNet and not Single.Http():IsHaveNetwork() then
		UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10002), function()  end)
		return
	end

	if self:IsNeedWait(cmd) then
		UIManager:OpenWindow(ViewID.ServerTip, ECircleNetSocket.DelayClose, 5, 1)
	end
	if pData ~= nil then self.reqMsgTable[cmd] = pData end
	self:SendPacketSerialNumber(cmd, packet)
end

---发送空包，可直接调用这个
---@param cmd number
function NetSocket:SendEmptyPacket(cmd)
	if not GameSymbols:IsEnableNetwork() then
		return
	end

	local msg = self:pb(cmd)
	self:Send(cmd, msg)
end

---是否需要等待服务端返回
---@return boolean
function NetSocket:IsNeedWait(cmd)
	local cfg = NetSocketCmd[cmd]
	return cfg and cfg.isWait == 1
end

---网络连接错误，是否需要提示语
function NetSocket:IsNeedErrorTips(cmd)
	local cfg = NetSocketCmd[cmd]
	return cfg and cfg.isTips == 1
end

--------------------------------补包相关-------------------------------

function NetSocket:SocketRepack()
	---补包，重新发送
	for i, v in pairs(self.cachePacketList) do
		if SceneRule.CurSceneType ~= SceneType.Game then
			self:Send(v.cmd, v.packet)
		end
	end
	self.cachePacketList = {}
end

---发送协议，并自增流水号
---@private
---@param packet BaseProtobuf
function NetSocket:SendPacketSerialNumber(cmd, packet)
	if self.cachePacketList[cmd] == nil and cmd ~= protocal.MSG_1004 then
		--如果需要缓存
		self.cachePacketList[cmd] = {cmd = cmd, packet = packet }
	end

	---@type Hukiry.Socket.ByteBlock
	local byteBlock = Hukiry.Socket.ByteBlock.New()
	local data = packet:SerializeToString(byteBlock);
	local buffer = data:ToArray()

	if cmd ~= protocal.MSG_1004 then
		log("发送包 cmd", cmd, "Len", buffer.Length, "pink");
	end
	NetManager.ins:SendPacket(cmd,  buffer)
end

---重置流水号
function NetSocket:ResetSerialNumber()
	---缓存包列表:发送缓存包
	self.cachePacketList = {}
	self.reqMsgTable = {}
end