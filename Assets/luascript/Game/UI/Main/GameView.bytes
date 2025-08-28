---
---
--- Create Time:2023-06-28
--- Author: Hukiry
---

---@class GameView:UIWindowBase
local GameView = Class(UIWindowBase)

function GameView:ctor()
	---@type GameControl
	self.ViewCtrl = nil
end

---初始属性字段
function GameView:Awake()
	self.prefabName = "Game"
	self.prefabDirName = "Main"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1


	---背景动画
	---@type table<number, UnityEngine.Transform>
	self.backList = {}
	---页签视图
	---@type PageManager
	self.pageView = require("Library.UIWidget.Page.PageManager").New()
	---页签视图
	---@type PageItem
	self.pageButtonSelect = require("Library.UIWidget.Page.PageItem").New()

end

---初始界面:注册按钮事件等
function GameView:Start()
	---设置
	self:AddClick(self.ViewCtrl.settingBtnGo, function()
		UIManager:OpenWindow(ViewID.Setting)
	end)
	---初始化活动按钮


	---初始化滚动视图页面
	local childCount = self.ViewCtrl.centerTF.childCount
	---滚动可见宽
	self.visibleWidth = self.ViewCtrl.centerTF.parent.rect.width
	self.visibleHeight = self.ViewCtrl.centerTF.parent.rect.height
	self.viewList = {
	    [EGamePage.ShopView] = "Shop",
	    [EGamePage.MailView] = "Mail",
	    [EGamePage.HomeView] = "Home",
	    [EGamePage.FriendView] = "Friend",
	    [EGamePage.RankingView] = "Ranking",
	}

	for i = 1, childCount do
		local tf = self.ViewCtrl.centerTF:GetChild(i-1)
		tf.sizeDelta = Vector2.New(self.visibleWidth, tf.sizeDelta.y)
		self.backList[i] = tf
		local classPath = string.format("Game.UI.Main.Panel.%sView", self.viewList[i])
		self.pageView:Start(i, classPath, tf.gameObject)
	end
	local viewY = self.ViewCtrl.centerTF.sizeDelta.y
	self.ViewCtrl.centerTF.sizeDelta = Vector2.New(self.visibleWidth*5, viewY)
	self.ViewCtrl.scrollView.onValueChanged:AddListener( Handle(self, self.WhenScrollChange))

	---初始化选择按钮
	self.pageButtonSelect:Start(self, self.ViewCtrl.downTF)

	---初始化货币
	---@type MoneyView
	self.moneyView = require('Game.UI.Main.MoneyView').New()
	self.moneyView:Start(self.ViewCtrl.topTF)

	StartCoroutine(function()
		WaitForFixedUpdate()
		WaitForFixedUpdate()
		self.ViewCtrl.centerTF:SetSizeDelta(self.ViewCtrl.centerTF.sizeDelta.x,self.visibleHeight)
		self.pageView:ResetSize(self.visibleHeight, self.ViewCtrl.homeRawTF,  EGamePage.HomeView)
	end)
end

---获取货币栏
---@return UnityEngine.Transform
function GameView:GetGoldTrans(moneyType)
	if moneyType == EMoneyType.gold or moneyType == EMoneyType.life then
		return self.moneyView:GetGoldTrans(moneyType)
	end
	return self.pageView:GetControl(EGamePage.HomeView, 0)
end

---获取图标
---@param actType EActivityType 控制按钮
---@return UnityEngine.Transform
function GameView:GetIconHome(actType)
	return self.pageView:GetControl(EGamePage.HomeView, actType)
end

---滚动改变时
function GameView:WhenScrollChange()
	--取偏离中心点的值，设置锚点靠最左边
	local centerX = self.ViewCtrl.centerTF.anchoredPosition.x-(self.visibleWidth/2)
	local index = math.floor(centerX/self.visibleWidth)
	self.pageButtonSelect:WhenScrollChange(index)
end

function GameView:SelectPage(index, isClick)
	self.pageButtonSelect:SelectPage(index, isClick)
	self.pageView:OnEnable(index)
end

---启动计时器后，调用此方法
function GameView:OnTimer()
	self.moneyView:OnTimer()
	self.pageView:OnUpdate()
end

---事件派发
---@param state number 1=切换页签 2=任务视图， 3=页签派发模式， 4，更新货币动画 ，5，同步绑定红点
---@param index EGamePage
function GameView:OnDispatch(state, index, value)
	if state == 1 then
		self.pageButtonSelect:OnDispatch(index)
	elseif state == 2 then
		self.pageView:OnUpdate()
	elseif state == 3 then
		self.pageView:OnDispatch(index, value)
	elseif state == 4 then
		self.moneyView:OnDispatch(EMoneyType.gold)
	elseif state == 5 then
		--self.ViewCtrl.redpointGo:SetActive(not Single.Role().isBindLogin)
	end
end

---显示窗口:初次打开
function GameView:OnEnable(finishCall)
	EventDispatch:Register(self, UIEvent.ChangeView_Language, self.OnChangeLanguage)
	if finishCall then finishCall() end

	self.pageButtonSelect:OnEnable()
	self.moneyView:OnEnable()
	self:OnChangeLanguage()
	---启动所有页签
	for index = EGamePage.ShopView, EGamePage.RankingView do
		self.pageView:OnEnable(index)
	end

	--self.ViewCtrl.redpointGo:SetActive(not Single.Role().isBindLogin)
end

---动态切换语言
function GameView:OnChangeLanguage()
	self.pageButtonSelect:OnChangeLanguage()
	self.pageView:ChangeLanguage()
end

---隐藏窗口
function GameView:OnDisable()
	self.pageView:OnDisable()
	self.moneyView:OnDisable()
	self.pageButtonSelect:OnDisable()
end

---消耗释放资源
function GameView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return GameView