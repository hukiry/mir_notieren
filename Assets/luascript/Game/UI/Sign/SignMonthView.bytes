---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

---@class SignMonthView:UIWindowBase
local SignMonthView = Class(UIWindowBase)

function SignMonthView:ctor()
	---@type SignMonthControl
	self.ViewCtrl = nil
end

---初始属性字段
function SignMonthView:Awake()
	self.prefabName = "SignMonth"
	self.prefabDirName = "Sign"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	---签到列表
	---@type table<number, SignItem>
	self.itemList = {}

	---@type SignItem
	self.sevenItem = nil
end

---初始界面:注册按钮事件等
function SignMonthView:Start()

	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.getBtnGo, Handle(self, self.OnSignBtn))

	---还有一天在外面
	if self.sevenItem == nil then
		self.sevenItem = UIItemPool.GetObject(UIItemType.SignItem, self.ViewCtrl.signItemSevenGo)
		self.sevenItem:Start(7)
	end
end

function SignMonthView:OnSignBtn()
	if self.info:IsCanSign() then
		local pross = self.info.dayId .. "|" ..os.time()
		Single.Request().SendActivity(EActivityType.sign, EHttpActivityState.FinishProgressbar, pross)
	end
end

---事件派发
function SignMonthView:OnDispatch()
	self:OnEnable()
end

---显示窗口:初次打开
---@param info SignActivity
function SignMonthView:OnEnable(info)
	self.info = info
	local id = self.info:IsCanSign() == true and 16006 or 16005
	self.ViewCtrl.txt = GetLanguageText(id)

	local array = SingleData.Activity():GetSign():GetSignArray()
	for i = 1, 6 do
		if self.itemList[i] == nil then
			self.itemList[i] = UIItemPool.Get(UIItemType.SignItem, self.ViewCtrl.dayListGo, i)
		end
		self.itemList[i]:OnEnable(array[i])
	end

	if array[7] then
		self.sevenItem:OnEnable(array[7])
	end

	if self.info:IsCanSign() then
		self.ViewCtrl.signTxt.text = GetLanguageText(15002)
	else
		self.ViewCtrl.signTxt.text = GetLanguageText(15003)
	end
end

---隐藏窗口
function SignMonthView:OnDisable()
	UIItemPool.PutTable(UIItemType.SignItem, self.itemList)
end

---消耗释放资源
function SignMonthView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return SignMonthView