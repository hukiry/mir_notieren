---
---
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class TaskView:UIWindowBase
local TaskView = Class(UIWindowBase)

function TaskView:ctor()
	---@type TaskControl
	self.ViewCtrl = nil
end

---初始属性字段
function TaskView:Awake()
	self.prefabName = "Task"
	self.prefabDirName = "Task"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1
end

---初始界面:注册按钮事件等
function TaskView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	---@type TaskBarItem
	self.taskBarItem = require("Game.UI.Main.Item.TaskBarItem").New(self.ViewCtrl.taskBarGo)
	self.taskBarItem:Start(true)
end

---启动计时器后，调用此方法
function TaskView:OnTimer()
	self.taskBarItem:OnUpdate()
end

---显示窗口:初次打开
---@param info ETaskData
---@param taskBarGo UnityEngine.GameObject
function TaskView:OnEnable(info, taskBarGo)
	self.taskBarGo = taskBarGo
	self.info = info
	self.taskBarItem:OnEnable(info)
	local itemInfo = SingleConfig.Item():GetKey(self.info.itemId)
	self.ViewCtrl.iconH.spriteName = string.len(itemInfo.targetIcon)>0 and itemInfo.targetIcon or itemInfo.icon

	local tab, finishs = Single.AutoTask():GetArray()
	for i, v in ipairs(tab) do
		v.state = 0
	end
	tab = table.addArryTable(tab, finishs)
	table.sort(tab, function(a, b) return a.timeId > b.timeId end)
	if self.loopView == nil then
		---@type UILoopItemView
		self.loopView= UILoopItemView.New(self.ViewCtrl.contentGo, UIItemType.TaskItem)
	end
	self.loopView:UpdateList(tab, true, false, #tab)
end

---隐藏窗口
function TaskView:OnDisable()
	self.taskBarGo:SetActive(true)
	self.loopView:OnDisable()
end

---消耗释放资源
function TaskView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return TaskView