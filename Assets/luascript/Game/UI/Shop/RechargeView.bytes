---
---
--- Create Time:2023-07-04
--- Author: Hukiry
---

---@class RechargeView:UIWindowBase
local RechargeView = Class(UIWindowBase)

function RechargeView:ctor(gameObject)
	---@type RechargeControl
	self.ViewCtrl = nil
end

---初始属性字段
function RechargeView:Awake()
	self.prefabName = "Recharge"
	self.prefabDirName = "Shop"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	self.panelLayer = ViewLayer.Window

end

---初始界面:注册按钮事件等
function RechargeView:Start()
	self.contentGo = self:FindGameObject("ScrollView/Viewport/Content")
	---@type UnityEngine.UI.ContentSizeFitter
	self.contentSize = self.contentGo:GetComponent("ContentSizeFitter")

	self.moreBtnGo = self:FindGameObject("ScrollView/Viewport/Content/moreBtn")
	---@type table<number, ShopItem>
	self.itemShopList = {}
	---@type table<number, ShopCoinItem>
	self.itemCoinList = {}

	self.parentGo = self:FindGameObject("top")


	---货币栏
	if self.ViewCtrl  then
		self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
		---@type MoneyView
		self.moneyView = require('Game.UI.Main.MoneyView').New()
		self.moneyView:Start(self.ViewCtrl.topTF)
	end

	if self.moreBtnGo then
		self:AddClick(self.moreBtnGo, Handle(self, self.LoadCoin))
	end
end

---事件派发
function RechargeView:OnDispatch(shopId)
	if shopId == 0  then
		if self.moneyView then
			self.moneyView:OnDispatch(EMoneyType.gold)
		end
		return
	end

	Single.Sound():PlaySound(ESoundResType.PayReward)

	local item = self.itemShopList[shopId] or self.itemCoinList[shopId]
	local info = SingleData.Shop():GetShopInfo(shopId)
	---@type Eitem_fly_data
	local fly = {}

	fly.iconName = SingleConfig.Currency():GetKey(EMoneyType.gold).icon

	---@type GameView
	local win = UIManager:GetActiveWindow(ViewID.Game)
	local GoldTrans, lifeTrans = nil, nil
	if self.moneyView then
		GoldTrans, lifeTrans = self.moneyView:GetGoldTrans(EMoneyType.gold), self.moneyView:GetGoldTrans(EMoneyType.life)
	else
		GoldTrans, lifeTrans = win:GetGoldTrans(EMoneyType.gold), win:GetGoldTrans(EMoneyType.life)
	end

	local coinNum = info:GetCoinNum()
	local data = Single.Animation():CaculateCount(coinNum)
	fly.count = data.len
	Single.Animation():PlayMultipleItem(item.transform.position, GoldTrans, fly, EAnimationFly.ViewToView, Handle(self, self.FinishAnimation, data),
	nil, self.gameObject)

	if item.PlayRewardEffect then
		if self.targetGo == nil then
			self.targetGo = self.gameObject
		end
		item:PlayRewardEffect(self.parentGo.transform, lifeTrans, self.targetGo)
	end

	self:PlayEffect()
end

function RechargeView:LoadCoin()
	if self.moreBtnGo then
		self.moreBtnGo:SetActive(false)
	end

	for _, v in ipairs(self.coinTab) do
		if self.itemCoinList[v.shopId] == nil then
			self.itemCoinList[v.shopId] = UIItemPool.Get(UIItemType.ShopCoinItem, self.contentGo)
		end
		self.itemCoinList[v.shopId]:OnEnable(v)
	end
end

---显示窗口:初次打开
function RechargeView:OnEnable()
	Single.SdkPlatform():GetPayInfo():ReqSDKProductList()

	self.contentSize.enabled = false
	local shopTab, coinTab = SingleData.Shop():GetShopArray()
	for i, v in ipairs(shopTab) do
		if self.itemShopList[v.shopId] == nil then
			self.itemShopList[v.shopId] = UIItemPool.Get(UIItemType.ShopItem, self.contentGo)
		end
		self.itemShopList[v.shopId]:OnEnable(v)
	end

	self.coinTab = coinTab
	if self.moreBtnGo then
		self.moreBtnGo.transform:SetAsLastSibling()
	else
		self:LoadCoin()
	end

	StartCoroutine(function()
		WaitForFixedUpdate()
		WaitForFixedUpdate()
		self.contentSize.enabled = true
	end)

	if self.moneyView then
		self.moneyView:OnEnable()
	end
end

---充值返回的特效
function RechargeView:PlayEffect()
	if self.effect == nil then
		---@type EffectItem2D
		self.effect = EffectItem2D.New(self.parentGo, "ui_rechargeReward")
		self.effect:SetLocalPosition(Vector3.zero)
	end

	self.effect:SetLoopParticle(true)
	self.effect:PlayAsync(nil,function()
		self.effect:SetLoopParticle(true)
	end)

	if self.corotine then
		StopCoroutine(self.corotine)
	end

	self.corotine = StartCoroutine(function()
		WaitForSeconds(2)
		if self.effect then
			self.effect:SetLoopParticle(false)
		end
	end)
end


function RechargeView:FinishAnimation(data)
	--{len, addNum, modNum index}
	data.index = data.index + 1
	Single.Player():SetMoneyNum(EMoneyType.gold, data.addNum, false, false)
	if  data.index == data.len and data.modNum > 0 then
		Single.Player():SetMoneyNum(EMoneyType.gold, data.modNum)
	end

	Single.Sound():PlaySound(ESoundResType.AddGold)

	if self.moneyView then
		self.moneyView:OnDispatch(EMoneyType.gold)
	else
		EventDispatch:Broadcast(ViewID.Game, 4)
	end

end

---隐藏窗口
function RechargeView:OnDisable()
	UIItemPool.PutTable(UIItemType.ShopItem , self.itemShopList)
	UIItemPool.PutTable(UIItemType.ShopCoinItem , self.itemCoinList)

	if self.effect then
		self.effect:OnDisable()
		self.effect = nil
	end

	if self.moneyView then
		self.moneyView:OnDisable()
	end
end

---消耗释放资源
function RechargeView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return RechargeView