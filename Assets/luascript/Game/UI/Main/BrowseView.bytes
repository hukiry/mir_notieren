---
--- 头像预览视图
--- Create Time:2023-07-19
--- Author: Hukiry
---

---@class BrowseView:UIWindowBase
local BrowseView = Class(UIWindowBase)

function BrowseView:ctor()
	---@type BrowseControl
	self.ViewCtrl = nil
end

---初始属性字段
function BrowseView:Awake()
	self.prefabName = "Browse"
	self.prefabDirName = "Main"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0


	self.headTab = {}
	for i = 1, 30 do
		table.insert(self.headTab, i)
	end
end

---初始界面:注册按钮事件等
function BrowseView:Start()
	self:AddClick(self.ViewCtrl.maskGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
end

---显示窗口:初次打开
function BrowseView:OnEnable(callback)
	if self.loopView == nil then
		---@type UILoopItemView
		self.loopView = UILoopItemView.New(self.ViewCtrl.contentGo, UIItemType.HeadItem)
	end
	self.loopView:UpdateList(self.headTab, true, false, callback)
end

---隐藏窗口
function BrowseView:OnDisable()
	if self.loopView then
		self.loopView:OnDisable()
	end
end

---消耗释放资源
function BrowseView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return BrowseView