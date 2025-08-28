---
---
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class RechargeFirstView:UIWindowBase
local RechargeFirstView = Class(UIWindowBase)

function RechargeFirstView:ctor()
	---@type RechargeFirstControl
	self.ViewCtrl = nil
end

---初始属性字段
function RechargeFirstView:Awake()
	self.prefabName = "RechargeFirst"
	self.prefabDirName = "ActivityView"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function RechargeFirstView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.payBtnGo, Handle(self, self.OnPay))
end

---支付
function RechargeFirstView:OnPay()
	Single.SdkPlatform():GetPayInfo():FetchSDK(self.info.googlePayId,function()
		Single.Request().SendShop(EHttpShopState.Other, self.info.id, self.info.price, Handle(self, self.OnFinish))---后面接入sdk
	end)
end

function RechargeFirstView:OnFinish()
	self:Close()
	SingleData.Activity():PlayReward(EActivityType.rechargeFirst, EMoneyType.gold, self.info.coinNum, function(number)
		Single.Player():SetMoneyNum(EMoneyType.gold, number)
	end)

	self.info:RequestFinished()
end

---显示窗口:初次打开
---@param info RechargeActivity
function RechargeFirstView:OnEnable(info)
	self.info = info
	self.ViewCtrl.payTxt.text = self.info:GetShowPrice()
	self.ViewCtrl.coinNum.text = "+"..self.info.coinNum
end

---隐藏窗口
function RechargeFirstView:OnDisable()
	
end

---消耗释放资源
function RechargeFirstView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return RechargeFirstView