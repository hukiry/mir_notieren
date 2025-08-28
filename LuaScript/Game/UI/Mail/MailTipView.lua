---
---
--- Create Time:2023-07-04
--- Author: Hukiry
---

---@class MailTipView:UIWindowBase
local MailTipView = Class(UIWindowBase)

function MailTipView:ctor()
	---@type MailTipControl
	self.ViewCtrl = nil
end

---初始属性字段
function MailTipView:Awake()
	self.prefabName = "MailTip"
	self.prefabDirName = "Mail"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MailTipView:Start()
	self:AddClick(self.ViewCtrl.maskGo, Handle(self, self.OnOk))
	self:AddClick(self.ViewCtrl.okBtnGo, Handle(self, self.OnOk))
end

---启动计时器后，调用此方法
function MailTipView:OnOk()
	self:Close()
	self.clickCall()
end


---显示窗口:初次打开
---@param info MailInfo
function MailTipView:OnEnable(info, clickCall)
	self.info = info
	self.clickCall = clickCall
	self.ViewCtrl.title.text = self.info.title
	self.ViewCtrl.content.text = self.info.content
end

---隐藏窗口
function MailTipView:OnDisable()
	
end

---消耗释放资源
function MailTipView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MailTipView