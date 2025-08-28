---
---
--- Create Time:2023-08-03
--- Author: Hukiry
---

---@class RewardGitItem:IUIItem
local RewardGitItem = Class(IUIItem)

function RewardGitItem:ctor()
	---@type RewardGitItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function RewardGitItem:Start(info)
	
end

---更新数据
---@param itemId number|EMoneyType
function RewardGitItem:OnEnable(itemId, itemNum)
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
function RewardGitItem:PlayEffect(targetTf, lifeTf, _selfTf)
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

	Single.Animation():PlayMultipleItem(_selfTf.position, targetTf, fly, EAnimationFly.ViewToView,nil,nil,
			ViewID.Game)
end

---隐藏窗口
function RewardGitItem:OnDisable()
	
end

---消耗释放资源
function RewardGitItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return RewardGitItem