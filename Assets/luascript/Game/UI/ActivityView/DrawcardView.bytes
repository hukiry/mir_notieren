---
---
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class DrawcardView:UIWindowBase
local DrawcardView = Class(UIWindowBase)

function DrawcardView:ctor()
	---@type DrawcardControl
	self.ViewCtrl = nil
end

---初始属性字段
function DrawcardView:Awake()
	self.prefabName = "Drawcard"
	self.prefabDirName = "ActivityView"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function DrawcardView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.getBtnGo, Handle(self, self.BuyCost))
	self:AddClick(self.ViewCtrl.claimBtnGo, Handle(self, self.OnClaim))
	---@type table<number, DrawcardItem>
	self.itemList = {}

	---@type UnityEngine.CanvasGroup
	self.canvasGroup = self.ViewCtrl.iconCenterGo:GetComponent("CanvasGroup")
end

---花费
function DrawcardView:BuyCost()
	if Single.Player():IsEnough(EMoneyType.gold, 100, true, true) then
		self.ViewCtrl.startPageGo:SetActive(false)
		self.ViewCtrl.drawPageGo:SetActive(true)
		Single.Player():SetMoneyNum(EMoneyType.gold, 100, true)
		self:StartPlay()
	end
end

---领取
function DrawcardView:OnClaim()
	self:Close()
	SingleData.Activity():PlayReward(EActivityType.rechargeFirst, self.moneyType, self.number, function(number)
		Single.Player():SetMoneyNum(self.moneyType, number)
	end)
	self.info:RequestFinishProgressbar()
end

---显示窗口:初次打开
---@param info DrawcardActivity
function DrawcardView:OnEnable(info)
	self.info = info
	self.ViewCtrl.startPageGo:SetActive(true)
	self.ViewCtrl.drawPageGo:SetActive(false)
	self.ViewCtrl.closeBtnGo:SetActive(true)

	self.ViewCtrl.itemDesc.text = GetLanguageText(15402)
end

function DrawcardView:StartPlay()
	self.cardIndex = 0
	self.ViewCtrl.closeBtnGo:SetActive(false)
	local array = self.info:GetArrayInfo()

	for _, v in ipairs(array) do
		if self.itemList[v.itemType]==nil then
			self.itemList[v.itemType] = UIItemPool.Get(UIItemType.DrawcardItem, self.ViewCtrl.gridCardGo, self)
		end
		self.itemList[v.itemType]:OnEnable(v.itemType, v.itemNum)
	end

	StartCoroutine(function()
		WaitForFixedUpdate()
		for i, v in pairs(self.itemList) do
			v:SetCardPosition(self.ViewCtrl.iconCenterGo.transform)
		end

		WaitForFixedUpdate()
		for i, v in pairs(self.itemList) do
			v:PlayCardMove()
		end
	end)
end

---播放卡片移动完成
function DrawcardView:FinishCardAnimation()
	self.cardIndex = self.cardIndex + 1
	self.canvasGroup.alpha = 1 - self.cardIndex/9
	self.canvasGroup.transform.localScale = Vector3.one*self.canvasGroup.alpha
	if self.cardIndex >= 9 then
		self.canvasGroup.alpha = 0
		for i, v in pairs(self.itemList) do
			v.isCanPlay = true
		end
	end
end

---翻开卡片
function DrawcardView:PlayFlipCard(moneyType, number)
	self.moneyType, self.number = moneyType, number
	StartCoroutine(function()
		for _, v in pairs(self.itemList) do
			v:PlayFlipCard()
		end

		WaitForSeconds(0.5)
		self.ViewCtrl.claimBtnGo:SetActive(true)
		self.ViewCtrl.itemDesc.text = GetLanguageText(15404)
	end)
end

---隐藏窗口
function DrawcardView:OnDisable()
	self.canvasGroup.alpha = 1
	self.ViewCtrl.claimBtnGo:SetActive(false)
	UIItemPool.PutTable(UIItemType.DrawcardItem, self.itemList)
end

---消耗释放资源
function DrawcardView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return DrawcardView