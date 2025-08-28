---重连管理类
---@class ReconnectionMgr
local ReconnectionMgr = Class()

local RE_COUNT = 3;

--重连管理类
function ReconnectionMgr:ctor()

end

--清理数据
function ReconnectionMgr:InitData()
	---弱联网-弹框3次数
	self.reconnectionCount = RE_COUNT
	---自动重连3次数
	self.autoReconnectionCount = RE_COUNT
	Single.TimerManger():RemoveHandler(self)
end

---自动尝试重连
function ReconnectionMgr:TryAutoReconnect()
	if not GameSymbols:IsStrongNetwork() then
		UIManager:OpenWindow(ViewID.ServerTip, ECircleNetSocket.HandleClose)
	end

	if self.autoReconnectionCount <= 0 then --自动链接三次失败，弹框
		UIManager:CloseWindow(ViewID.ServerTip)
		self:TryReconnectView()
	else
		Single.TimerManger():DoTime(self, self.Reconnection, 2) --只执行一次
	end
end

---重连Socket
function ReconnectionMgr:Reconnection()
	logError("重连次数：", self.autoReconnectionCount)
	self.autoReconnectionCount = self.autoReconnectionCount - 1
	NetSocket:Connect(NetSocket.addressHost, NetSocket.addressPort) --重连
end

---弹框尝试重连
function ReconnectionMgr:TryReconnectView()
	self.autoReconnectionCount = RE_COUNT
	logError("reconnectionCount", self.reconnectionCount)
	---已经弹框3次数 或者 在登录界面
	if self.reconnectionCount <= 0 or SceneRule.IsInLoginScene() then
		local tips = "已经尝试重连了3次"
		UIManager:OpenWindow(ViewID.PromptBox, GetLanguageText(11502), Handle(self, self.Logout, tips))
	else
		---每次弹框扣一次
		self.reconnectionCount = self.reconnectionCount - 1
		UIManager:OpenWindow(ViewID.PromptBox, GetLanguageText(11502), Handle(self, self.TryAutoReconnect))
	end
end

---连接成功：调用
function ReconnectionMgr:OnConnected()
	self.reconnectionCount = RE_COUNT
	self.autoReconnectionCount = RE_COUNT
	log("Connected Successful","pink")
	UIManager:CloseWindow(ViewID.ServerTip)
end

---回到登陆界面:切换到后台，超过5分钟，则登出
function ReconnectionMgr:Logout(tips)
	UIManager:CloseWindow(ViewID.PromptBox);
	NetSocket:Disconnect(tips .. ",回到登陆界面", false)
	Single.GameCenter():LoginOut()
end

return ReconnectionMgr