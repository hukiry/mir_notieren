---
---
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class PropsView:UIWindowBase
local PropsView = Class(UIWindowBase)

function PropsView:ctor()
	---@type PropsControl
	self.ViewCtrl = nil
end

---初始属性字段
function PropsView:Awake()
	self.prefabName = "Props"
	self.prefabDirName = "ActivityView"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1
end

---初始界面:注册按钮事件等
function PropsView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.getBtnGo, Handle(self, self.OnGet))
end

---领取
function PropsView:OnGet()
	if self.info:IsFinish() then
		return
	end

	SingleData.Activity():PlayReward(EActivityType.item, EMoneyType.gold, self.info.coinNum, function(number)
		Single.Player():SetMoneyNum(EMoneyType.gold, number)
	end)
	self.info:RequestFinished()
	self:Close()
end

---启动计时器后，调用此方法
function PropsView:OnTimer()
	self.ViewCtrl.remainTime.text = self.info:GetActRemainTime()
end

---显示窗口:初次打开
---@param info PropsActivity
function PropsView:OnEnable(info)
	self.info = info
	self.ViewCtrl.remainTime.text = self.info:GetActRemainTime()
	self.ViewCtrl.slider.ProgresssMax = self.info.itemNum
	self.ViewCtrl.slider.fillAmount = self.info.curNum/self.info.itemNum
	self.ViewCtrl.sliderBgGo:SetActive(not (self.info:IsFinish() or self.info:IsCanGet()))
	self.ViewCtrl.timeBgGo:SetActive(not (self.info:IsFinish() or self.info:IsCanGet()))
	self.ViewCtrl.getBtnGo:SetActive(self.info:IsFinish() or self.info:IsCanGet())
	self.ViewCtrl.playTxt.text = self.info:IsFinish() and GetLanguageText(15302) or GetLanguageText(15301)
	self.ViewCtrl.itemDesc.text = GetReplaceFormat(GetLanguageText(15304), self.info.itemNum)
end

---隐藏窗口
function PropsView:OnDisable()
	
end

---消耗释放资源
function PropsView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return PropsView