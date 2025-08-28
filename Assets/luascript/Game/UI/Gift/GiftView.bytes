---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

---@class GiftView:UIWindowBase
local GiftView = Class(UIWindowBase)

function GiftView:ctor()
	---@type GiftControl
	self.ViewCtrl = nil
end

---初始属性字段
function GiftView:Awake()
	self.prefabName = "Gift"
	self.prefabDirName = "Gift"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1
end

---初始界面:注册按钮事件等
function GiftView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	---@type table<number, GiftItem>
	self.itemList = {}
end

---启动计时器后，调用此方法
function GiftView:OnTimer()
	for i, v in pairs(self.itemList) do
		if not self.info:IsEndGift(i) then
			v:OnUpdate()
		end
	end
end

---显示窗口:初次打开
---@param info GiftActivity
function GiftView:OnEnable(info)
	self.info = info
	local arryInfo = self.info:GetGiftArray()
	for i, v in ipairs(arryInfo) do
		if self.itemList[v.sort] == nil then
			self.itemList[v.sort] = UIItemPool.Get(UIItemType.GiftItem, self.ViewCtrl.verticalGo, self, v.sort)
		end
		self.itemList[v.sort]:OnEnable(v)
	end
end

---@param info ShopInfo
function GiftView:OnFinish(info)
	if self.itemList[info.sort] then
		---@type GameView
		local win = UIManager:GetActiveWindow(ViewID.Game)
		local GoldTrans, lifeTrans = nil, nil
		if self.moneyView then
			GoldTrans, lifeTrans = self.moneyView:GetGoldTrans(EMoneyType.gold), self.moneyView:GetGoldTrans(EMoneyType.life)
		else
			GoldTrans, lifeTrans = win:GetGoldTrans(EMoneyType.gold), win:GetGoldTrans(EMoneyType.life)
		end

		self.itemList[info.sort]:PlayRewardEffect(win:GetGoldTrans(0), lifeTrans, win:GetIconHome(EActivityType.gift))
		self.info:RequestFinishProgressbar(info.sort)
		self:Close()
	end
end

---隐藏窗口
function GiftView:OnDisable()
	UIItemPool.PutTable(UIItemType.GiftItem, self.itemList)
end

---消耗释放资源
function GiftView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return GiftView