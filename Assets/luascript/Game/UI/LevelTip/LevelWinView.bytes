---
---
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class LevelWinView:UIWindowBase
local LevelWinView = Class(UIWindowBase)

function LevelWinView:ctor()
	---@type LevelWinControl
	self.ViewCtrl = nil
end

---初始属性字段
function LevelWinView:Awake()
	self.prefabName = "LevelWin"
	self.prefabDirName = "LevelTip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function LevelWinView:Start()
	self:AddClick(self.ViewCtrl.maskGo, Handle(self, self._Close))

	self.RightanchoredPosition = self.ViewCtrl.rightMTF.anchoredPosition
	self.LeftanchoredPosition = self.ViewCtrl.leftMTF.anchoredPosition
end

function LevelWinView:_Close()
	if self.isFinsih then
		self:Close()
		SceneApplication.ChangeState(ViewScene)
	end
end

---显示窗口:初次打开
function LevelWinView:OnEnable(...)
	self.ViewCtrl.slider.fillAmount = 0.45
	self.ViewCtrl.ttttTF.position = self.ViewCtrl.startTTTTF.position

	self.ViewCtrl.slider:DOFillAmount(1, 1.5):OnComplete(function()
		self.ViewCtrl.ttttTF:DOMove(self.ViewCtrl.startTTTTF.position, 1)
	end)

	self.ViewCtrl.rightMTF:DOMove(self.ViewCtrl.startrightTF.position, 2)
	self.ViewCtrl.leftMTF:DOMove(self.ViewCtrl.startLeftTF.position, 2)

	self.ViewCtrl.contineGo:SetActive(false)
	self.isFinsih = false
	StartCoroutine(function()
		WaitForSeconds(2)
		self.isFinsih = true
		self.ViewCtrl.contineGo:SetActive(true)
	end)
end

---隐藏窗口
function LevelWinView:OnDisable()
	self.ViewCtrl.rightMTF.anchoredPosition = self.RightanchoredPosition
	self.ViewCtrl.leftMTF.anchoredPosition = self.LeftanchoredPosition
end

---消耗释放资源
function LevelWinView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return LevelWinView