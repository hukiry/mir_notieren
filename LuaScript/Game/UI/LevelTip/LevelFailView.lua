---
---
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class LevelFailView:UIWindowBase
local LevelFailView = Class(UIWindowBase)

function LevelFailView:ctor()
	---@type LevelFailControl
	self.ViewCtrl = nil
end

---初始属性字段
function LevelFailView:Awake()
	self.prefabName = "LevelFail"
	self.prefabDirName = "LevelTip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function LevelFailView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self._Close))
	self:AddClick(self.ViewCtrl.buyBtnGo, Handle(self, self._onBuy))

	---@type MoneyView
	self.moneyView = require('Game.UI.Main.MoneyView').New()
	self.moneyView:Start(self.ViewCtrl.topTF)
end

function LevelFailView:_Close()
	UIManager:OpenWindow(ViewID.LevelChallenge, true)
	self:Close()
end

function LevelFailView:_onBuy()
	if Single.Player():IsEnough(EMoneyType.gold, self.costNum, true) then
		Single.Player():SetMoneyNum(EMoneyType.gold, self.costNum, true)
		Single.Match():AddMoveCount(5)
		EventDispatch:Broadcast(ViewID.LevelMain, 1)--更新移动次数
		self.callback()
		self:Close()
	end
end

---显示窗口:初次打开
function LevelFailView:OnEnable(callback)
	self.callback = callback
	local moveCount = Single.Player():GetMoneyNum(EMoneyType.moveCount)
	self.costNum = math.floor(moveCount/5)*50 + 100
	self.costNum = Mathf.Clamp(self.costNum, 100, 4750)
	self.ViewCtrl.buyNum.text = self.costNum

	self.moneyView:OnEnable()
end

---隐藏窗口
function LevelFailView:OnDisable()
	self.moneyView:OnDisable()
end

---消耗释放资源
function LevelFailView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return LevelFailView