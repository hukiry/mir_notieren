---
---
--- Create Time:2023-03-17
--- Author: Hukiry
---

---@class PassView:UIWindowBase
local PassView = Class(UIWindowBase)

function PassView:ctor()
	---@type PassControl
	self.ViewCtrl = nil
end

---初始属性字段
function PassView:Awake()
	self.prefabName = "Pass"
	self.prefabDirName = "Pass"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1

	self.isFirst = true
end

---初始界面:注册按钮事件等
function PassView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo ,Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.activateBtnGo ,function()
		UIManager:OpenWindow(ViewID.PassPay, self.info)
	end)
end

---启动计时器后，调用此方法
function PassView:OnTimer()
	self.ViewCtrl.passTime.text = self.info:GetActRemainTime()
end


function PassView:OnRefresh()
	self:OnEnable()
end

---显示窗口:初次打开
---@param info PassActivity
function PassView:OnEnable(info)
	self.info = info
	local array = self.info:GetPassArray()
	if self.loopView == nil then
		---@type UILoopItemView
		self.loopView = UILoopItemView.New(self.ViewCtrl.contentGo, UIItemType.PassItem)
	end
	self.loopView:UpdateList(array,true, self.isFirst)
	self.isFirst = false
	self.loopView:GotoLocationByIndex(self.info:GetPassIndex())

	---@type Hukiry.UI.AtlasImage
	local img = self.ViewCtrl.activateBtnGo:GetComponent("AtlasImage")
	img.IsGray = self.info:GetPayInfo():IsPayed()
end

---隐藏窗口
function PassView:OnDisable()
	self.loopView:OnDisable()
end

---消耗释放资源
function PassView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return PassView