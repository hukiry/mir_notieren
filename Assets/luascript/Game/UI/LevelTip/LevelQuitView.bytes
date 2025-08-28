---
---
--- Create Time:2023-08-04
--- Author: Hukiry
---

---@class LevelQuitView:UIWindowBase
local LevelQuitView = Class(UIWindowBase)

function LevelQuitView:ctor()
	---@type LevelQuitControl
	self.ViewCtrl = nil
end

---初始属性字段
function LevelQuitView:Awake()
	self.prefabName = "LevelQuit"
	self.prefabDirName = "LevelTip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function LevelQuitView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.quitBtnGo, Handle(self, self.OnQuit))
end

---启动计时器后，调用此方法
function LevelQuitView:OnQuit()
	Single.Player():SetMoneyNum(EMoneyType.life, 1, true)
	UIEventListener.ExecuteEvent(self.obj, EventTriggerType.PointerClick)
	UIManager:CloseWindow(ViewID.LevelMain)
	SceneApplication.ChangeState(ViewScene)
end

---显示窗口:初次打开
function LevelQuitView:OnEnable(obj)
	self.obj = obj
	local  desc = GetReplaceFormat(GetLanguageText(12102), "<color=red>1</color>")
	self.ViewCtrl.desc.text = string.format("<color=black>%s</color>", desc)
end

---隐藏窗口
function LevelQuitView:OnDisable()
	
end

---消耗释放资源
function LevelQuitView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return LevelQuitView