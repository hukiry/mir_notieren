---
---
--- Create Time:2023-07-30
--- Author: Hukiry
---

---@class LifeView:UIWindowBase
local LifeView = Class(UIWindowBase)

function LifeView:ctor()
	---@type LifeControl
	self.ViewCtrl = nil
end

---初始属性字段
function LifeView:Awake()
	self.prefabName = "Life"
	self.prefabDirName = "Main"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1
end

---初始界面:注册按钮事件等
function LifeView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo , Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.freeBtnGo , Handle(self, self._onFree))
	self:AddClick(self.ViewCtrl.buyBtnGo , Handle(self, self._onBuy))
end

function LifeView:_onFree()
	Single.Player():SetMoneyNum(EMoneyType.life, 1)
	Single.Player():SetMoneyNum(EMoneyType.lifeFree, 1)
	EventDispatch:Broadcast(ViewID.Game, 4)
	self:Close()
end

function LifeView:_onBuy()
	local isEnough = Single.Player():IsEnough(EMoneyType.gold, self.costNum)
	if isEnough then
		Single.Player():SetMoneyNum(EMoneyType.life, 1)
		Single.Player():SetMoneyNum(EMoneyType.lifeCount, 1)
		Single.Player():SetMoneyNum(EMoneyType.gold, self.costNum, true)
		EventDispatch:Broadcast(ViewID.Game, 4)
		Single.Sound():PlaySound(ESoundResType.BuyGoldClick)
		self:Close()
	else
		UIManager:OpenWindow(ViewID.Recharge)
	end
end

function LifeView:GetTimeString(t)
	local summ = math.floor(t/60)
	local h = math.floor(summ/60)
	local m = summ%60
	if h==0 and m==0 then
		return  string.format("%02d:%02d", m, t%60)
	else
		return string.format("%02d:%02d:%02d", h, m, t%60)
	end
end

---启动计时器后，调用此方法
function LifeView:OnTimer()
	self.ViewCtrl.remainTime.text = self:GetTimeString(Single.Player().curLifeTime)
end

---显示窗口:初次打开
function LifeView:OnEnable()
	self.ViewCtrl.remainTime.text = self:GetTimeString(Single.Player().curLifeTime)
	self.ViewCtrl.lifeNum.text = Single.Player():GetMoneyNum(EMoneyType.life)+1

	self.costNum = Mathf.Floor(Single.Player():GetMoneyNum(EMoneyType.lifeCount)/10) + 100
	self.ViewCtrl.buyNum.text = self.costNum

	local  lifefree = Single.Player():GetMoneyNum(EMoneyType.lifeFree)
	self.ViewCtrl.freeBtnGo:SetActive(lifefree==0)
end

---隐藏窗口
function LifeView:OnDisable()
	
end

---消耗释放资源
function LifeView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return LifeView