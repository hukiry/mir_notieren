---
---
--- Create Time:2023-07-04
--- Author: Hukiry
---

---@class MailItem:IUIItem
local MailItem = Class(IUIItem)

function MailItem:ctor()
	---@type MailItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function MailItem:Start(isLife, ClickCall,_self)
	self.isLife = isLife
	self.ClickCall = ClickCall
	---@type MailView
	self.mailView = _self

	self.itemCtrl.getBtnGo:SetActive(self.isLife)
	self.itemCtrl.readBtnGo:SetActive(not self.isLife)

	self:AddClick(self.itemCtrl.getBtnGo, Handle(self, self.PlayLife), true)
	self:AddClick(self.itemCtrl.readBtnGo, Handle(self, self.PlaceRewrad), true)
end

---更新数据
---@param info MailInfo|number
function MailItem:OnEnable(info,index)
	self.info = info
	self.numberId = index
	self:ChangeLanguage()
end

function MailItem:ChangeLanguage()
	self.itemCtrl.desc.text = GetLanguageText(self.isLife and 10023 or 10028)
	self.itemCtrl.title.text = type(self.info) == "number" and GetLanguageText(10022) or  self.info:GetTitleText()
end

---邮件奖励
function MailItem:PlaceRewrad()
	Single.Request().SendMailBoard(true, EHttpMailState.Reded, self.info.mailId)
	UIManager:OpenWindow(ViewID.MailTip, self.info, function()
		self.ClickCall(self.isLife, self.numberId, self.info.mailId)
		UIItemPool.Put(UIItemType.MailItem, self)
	end)

	if self.info:IsHasReward() then
		local array = self.info:GetRewardArray()
		--moneyType, num
		for i, vTab in ipairs(array) do
			local moneyType, num = vTab[1], vTab[2]
			Single.Player():SetMoneyNum(moneyType, num)
		end
		EventDispatch:Broadcast(ViewID.Game, 4)
	end
end

---生命值
function MailItem:PlayLife()
	local lifehour, max = Single.Player():GetMoneyNum(EMoneyType.lifehour), Single.Player():GetMoneyNum(EMoneyType.lifeMax)
	local h = math.floor(Single.Player().curLifeTime/(lifehour*3600))
	if h < max then
		---@type Eitem_fly_data
		local fly = {}
		fly.count = 1
		fly.iconName = SingleConfig.Currency():GetKey(EMoneyType.life).icon
		fly.size = Vector2.New(60,60)
		Single.Player():SetMoneyNum(EMoneyType.life, 1)
		EventDispatch:Broadcast(ViewID.Game, 4)

		SingleData.Mail():RemoveData(self.isLife, self.info)
		local pos = self.itemCtrl.icon.transform.position
		Single.Animation():PlayMultipleItem(pos, self.mailView.titleGo.transform, fly, EAnimationFly.ViewToView, nil,nil,
		self.gameObject)

		self.ClickCall(self.isLife, self.numberId)
		StartCoroutine(function()
			WaitForFixedUpdate()
			UIItemPool.Put(UIItemType.MailItem, self)
		end)
	else
		TipMessageBox.ShowUI(GetLanguageText(10026))
	end
end

---隐藏窗口
function MailItem:OnDisable()
	
end

---消耗释放资源
function MailItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return MailItem