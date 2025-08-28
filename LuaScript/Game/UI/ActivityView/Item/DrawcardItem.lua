---
---
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class DrawcardItem:IUIItem
local DrawcardItem = Class(IUIItem)

function DrawcardItem:ctor()
	---@type DrawcardItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
---@param _self DrawcardView
function DrawcardItem:Start(_self)
	self.drawcardView = _self
	self:AddClick(self.itemCtrl.gameObject, Handle(self, self.FlipCard))
end

---翻动卡片
function DrawcardItem:FlipCard()
	if self.isCanPlay then
		self.drawcardView:PlayFlipCard(self.moneyType, self.number)
		StartCoroutine(function()
			WaitForSeconds(0.5)
			self.itemCtrl.ui_RewardItemGo:SetActive(true)
		end)
	end
end

---更新数据
---@param moneyType EMoneyType
---@param number number
function DrawcardItem:OnEnable(moneyType, number)
	---允许点击生效
	self.isCanPlay = false
	self.itemCtrl.num.gameObject:SetActive(number>1)
	self.itemCtrl.num.text = '+'..number
	self.moneyType, self.number = moneyType, number
	local iconName = SingleConfig.Currency():GetKey(moneyType).icon
	SetUIIcon(self.itemCtrl.icon, iconName, Vector2.New(140,140))
	self.itemCtrl.cardTF.gameObject:SetActive(false)
	self.itemCtrl.frontTF:SetRotation(0,90,0)
	self.itemCtrl.backTF:SetRotation(0,0,0)
end

---等一帧后，设置卡片位置
function DrawcardItem:SetCardPosition(iconCenter)
	self.itemCtrl.cardTF:SetPosition(iconCenter.position, false)
	self.itemCtrl.cardTF.gameObject:SetActive(true)
end

---等一帧后，播放卡片移动
function DrawcardItem:PlayCardMove()
	local count = self.moneyType%5+1
	self.itemCtrl.cardTF:DOMove(self.transform.position, count*0.2):OnComplete(function()
		self.drawcardView:FinishCardAnimation()
	end)
end

---播放翻卡片
function DrawcardItem:PlayFlipCard()
	self.isCanPlay = false
	self.itemCtrl.ui_book_openGo:SetActive(true)
	self.itemCtrl.backTF:DORotate(Vector3.New(0,90,0),0.5)
	self.itemCtrl.frontTF:DORotate(Vector3.New(0,0,0),0.5):OnComplete(function()
		self.itemCtrl.ui_book_openGo:SetActive(false)
	end)
end

---隐藏窗口
function DrawcardItem:OnDisable()
	self.itemCtrl.ui_book_openGo:SetActive(false)
	self.itemCtrl.ui_RewardItemGo:SetActive(false)
end

---消耗释放资源
function DrawcardItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return DrawcardItem