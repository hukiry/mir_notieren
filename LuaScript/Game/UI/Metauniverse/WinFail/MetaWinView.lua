---
---
--- Create Time:2023-10-18
--- Author: Hukiry
---

---@class MetaWinView:UIWindowBase
local MetaWinView = Class(UIWindowBase)

function MetaWinView:ctor()
	---@type MetaWinControl
	self.ViewCtrl = nil
end

---初始属性字段
function MetaWinView:Awake()
	self.prefabName = "MetaWin"
	self.prefabDirName = "Metauniverse/WinFail"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MetaWinView:Start()
	self:AddClick(self.ViewCtrl.contineGo, Handle(self, self.OnContine))
end

function MetaWinView:OnContine()
	if self.isFight then
		SceneApplication.ChangeState(HomeScene)
	else
		SceneApplication.ChangeState(MetaScene)
	end
end

---显示窗口:初次打开
function MetaWinView:OnEnable(isFight)
	self.isFight = isFight
	self.ViewCtrl.desc.text = GetLanguageText( isFight and 16217 or 16216)
	local pass = GetLanguageText(16218)
	local test = GetLanguageText(16219) .. "\n" ..pass
	local ftext = GetLanguageText(16126).. "\n" ..pass
	self.ViewCtrl.title.text = isFight and ftext or test
end

---隐藏窗口
function MetaWinView:OnDisable()
	
end

---消耗释放资源
function MetaWinView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaWinView