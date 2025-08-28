---
---
--- Create Time:2023-07-03
--- Author: Hukiry
---

---@class RewardItem:IUIItem
local RewardItem = Class(IUIItem)

function RewardItem:ctor()
	---@type RewardItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function RewardItem:Start(i)
	
end

---更新数据
---@param itemId number|EMoneyType
function RewardItem:OnEnable(itemId, itemNum)
	self.itemId = itemId
	self.itemNum = itemNum
	local  item = SingleConfig.Currency():GetKey(itemId)
	self.iconName = item.icon
	SetUIIcon(self.itemCtrl.icon, item.icon)
	if self.itemId == EMoneyType.life then
		self.itemCtrl.num.text = itemNum..'h'
	else
		self.itemCtrl.num.text = 'x' .. itemNum
	end
end

---@param targetTf UnityEngine.Transform
function RewardItem:PlayEffect(targetTf, lifeTf, parentGo)
	---@type Eitem_fly_data
	local fly = {}
	fly.count = self.itemNum>25 and 25 or self.itemNum
	fly.iconName = self.iconName
	fly.size = Vector2.New(60,60)

	if self.itemId == EMoneyType.life then
		targetTf = lifeTf
		Single.Player():SetMoneyNum(EMoneyType.lifehour, self.itemNum)
		EventDispatch:Broadcast(ViewID.Game, 4)
		EventDispatch:Broadcast(ViewID.Recharge, 0)
	else
		Single.Player():SetMoneyNum(self.itemId, self.itemNum)
	end

	Single.Animation():PlayMultipleItem(self.itemCtrl.icon.transform.position, targetTf, fly, EAnimationFly.ViewToView,
			nil,nil, parentGo)
end


---隐藏窗口
function RewardItem:OnDisable()
	
end

---消耗释放资源
function RewardItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return RewardItem