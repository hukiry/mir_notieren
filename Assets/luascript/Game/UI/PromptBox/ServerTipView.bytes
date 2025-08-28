---
---
--- Create Time:2022-11-17 16:00
--- Author: Hukiry
---

---@class ServerTipView:UIWindowBase
local ServerTipView = Class(UIWindowBase)

function ServerTipView:ctor()
	---@type ServerTipControl
	self.ViewCtrl = nil
end

---初始属性字段
function ServerTipView:Awake()
	self.prefabName = "ServerTip"
	self.prefabDirName = "PromptBox"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1

	self.panelLayer = ViewLayer.Tips
end

---初始界面:注册按钮事件等
function ServerTipView:Start()
	self.ViewCtrl.circle.material = Material.New(Shader.Find("Custom/UIWaitLoading"))
end

---启动计时器后，调用此方法
function ServerTipView:OnTimer()
	if self.isEnableTimer then
		self.SumTime = self.SumTime - 1
		if self.SumTime <= 0 then
			self:Close()
		end

		if self.waitTime then
			self.waitTime = self.waitTime - 1
			if self.waitTime <= 0 then
				self.ViewCtrl.circle.gameObject:SetActive(true)
			end
		end
	end
end

---显示窗口:初次打开
---@param circle ECircleNetSocket
function ServerTipView:OnEnable(circle, delaySecond, delayTime)
	self.waitTime = delayTime
	self.ViewCtrl.circle.gameObject:SetActive(delayTime == nil)
	self.circle = circle or ECircleType.HandleClose
	self.ViewCtrl.tipGo:SetActive(circle== ECircleType.HandleTipClose)
	self.ViewCtrl.tipTxt.text = GetLanguageText(11503)
	if self.circle == ECircleType.AutoClose then
		self.isEnableTimer = true
		self.SumTime = 4
		self:OnEnableTimer()
	elseif self.circle == ECircleType.DelayClose then
		self.isEnableTimer = true
		self.SumTime = delaySecond or 4
		self:OnEnableTimer()
	elseif self.circle == ECircleType.HandleClose then
		self.isEnableTimer = false
		EventDispatch:UnRegister(self)
	else
		self.isEnableTimer = true
		self.SumTime = delaySecond or 4
		self:OnEnableTimer()
	end
end

---隐藏窗口
function ServerTipView:OnDisable()
	
end

---消耗释放资源
function ServerTipView:OnDestroy()
	
end

return ServerTipView