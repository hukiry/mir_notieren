---
--- 元世界进入弹框
--- Create Time:2023-09-13
--- Author: Hukiry
---

---@class MetaEnterView:UIWindowBase
local MetaEnterView = Class(UIWindowBase)

function MetaEnterView:ctor()
	---@type MetaEnterControl
	self.ViewCtrl = nil
end

---初始属性字段
function MetaEnterView:Awake()
	self.prefabName = "MetaEnter"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MetaEnterView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.startBtn, Handle(self, self.OnPlay))
end

function MetaEnterView:OnPlay()
	SceneApplication.ChangeState(HomeScene)
end

---启动计时器后，调用此方法
function MetaEnterView:OnTimer()
	local info = SingleData.Activity():GetMeta()
	if info then
		if info:IsInActivity() or info:IsInPreview() then
			self.ViewCtrl.actTime.text = info:GetActRemainTime()
		end
	end
end

---显示窗口:初次打开
function MetaEnterView:OnEnable(...)
	local isFull = Single.Player():GetMoneyNum(EMoneyType.level) > 100
	self.ViewCtrl.startBtn.IsGray = not isFull
	self.ViewCtrl.startBtn.raycastTarget = isFull
	local info = SingleData.Activity():GetMeta()
	if info then
		if info:IsInPreview() then
			self.ViewCtrl.descSelect.text = GetLanguageText(16004)
		elseif info:IsInActivity() then
			self.ViewCtrl.descSelect.text = GetLanguageText(16002)
		else
			self.ViewCtrl.descSelect.text = GetLanguageText(isFull and 16003 or 10006)
		end
	else
		self.ViewCtrl.descSelect.text = GetLanguageText(isFull and 16003 or 10006)
	end

	self.ViewCtrl.timeBgGo:SetActive(info~=nil)
end

---隐藏窗口
function MetaEnterView:OnDisable()
	
end

---消耗释放资源
function MetaEnterView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaEnterView