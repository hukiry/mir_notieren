---
---
--- Create Time:2023-05-06
--- Author: Hukiry
---

---@class BuyTipView:UIWindowBase
local BuyTipView = Class(UIWindowBase)

function BuyTipView:ctor()
	---@type BuyTipControl
	self.ViewCtrl = nil
end

---初始属性字段
function BuyTipView:Awake()
	self.prefabName = "BuyTip"
	self.prefabDirName = "PromptBox"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1

	self.panelLayer = ViewLayer.Tips

	self.SumTime = 8
end

---初始界面:注册按钮事件等
function BuyTipView:Start()
	self.ViewCtrl.circle.material = Material.New(Shader.Find("Custom/UIWaitLoading"))
end

---启动计时器后，调用此方法
function BuyTipView:OnTimer()
	self.SumTime = self.SumTime - 1
	if self.SumTime < 0 then
		self.ViewCtrl.waitTxt.gameObject:SetActive(false)
		self:Close()
	end
	self.ViewCtrl.waitTxt.text = GetLanguageText(11501).. self.SumTime .. 's'
end

---显示窗口:初次打开
function BuyTipView:OnEnable(...)
	self.SumTime = 8
	self.ViewCtrl.waitTxt.text = GetLanguageText(11501).. self.SumTime .. 's'
	self.ViewCtrl.waitTxt.gameObject:SetActive(true)
end

function BuyTipView:OnRefresh()
	self:OnEnable()
end

---隐藏窗口
function BuyTipView:OnDisable()
	
end

---消耗释放资源
function BuyTipView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return BuyTipView