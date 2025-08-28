---
---
--- Create Time:2023-07-17
--- Author: Hukiry
---

---@class PropsTipView:UIWindowBase
local PropsTipView = Class(UIWindowBase)

function PropsTipView:ctor()
	---@type PropsTipControl
	self.ViewCtrl = nil
end

---初始属性字段
function PropsTipView:Awake()
	self.prefabName = "PropsTip"
	self.prefabDirName = "PromptBox"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function PropsTipView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.buyBtnGo, Handle(self, self.OnBuy))
end

function PropsTipView:OnBuy()
	if Single.Player():IsEnough(EMoneyType.gold, self.costGold, true) then
		Single.Player():SetMoneyNum(EMoneyType.gold, self.costGold, true)
		Single.Player():SetMoneyNum(self.propType, self.sellNum)--购买的道具数量
		Single.Player():SetMoneyNum(self.buyCountType, 1)--记录购买次数
		EventDispatch:Broadcast(ViewID.LevelMain, 4, self.props)
		EventDispatch:Broadcast(ViewID.Game, 4)
		self:Close()
	end
end

---显示窗口:初次打开
---@param props EPropsView
function PropsTipView:OnEnable(props)
	self.props = props
	self.buyCountType, self.propType = EPropsBuyCount[props], EPropsToMoneyType[props]
	local info = Single.Match():GetMapConifg():GetPropsInfo(props):GetCurrency()
	self.ViewCtrl.title.text = GetLanguageText(info.languageName)
	self.ViewCtrl.desc.text = GetLanguageText(info.languageDesc)
	SetUIIcon(self.ViewCtrl.icon, info.icon, Vector2.New(280,280))

	self.sellNum = info.sellNum
	self.costGold = info.sellPrice + self:CalculateValue()
	self.ViewCtrl.price.text = self.costGold
end

function PropsTipView:CalculateValue()
	local count = Single.Player():GetMoneyNum(self.buyCountType)
	return math.floor(count/5)*50 + math.floor(count/10)*150 + 50
end

---隐藏窗口
function PropsTipView:OnDisable()
	
end

---消耗释放资源
function PropsTipView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return PropsTipView