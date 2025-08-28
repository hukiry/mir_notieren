---
---
--- Create Time:2023-07-31
--- Author: Hukiry
---

---@class TaskItem:IUIItem
local TaskItem = Class(IUIItem)

function TaskItem:ctor()
	---@type TaskItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function TaskItem:Start(len)
	self.len = len
end

---更新数据
---@param info ETaskData
function TaskItem:OnEnable(info)
	self.info = info
	self.itemCtrl.icon.gameObject:SetActive(self.info.state == 0)
	self.itemCtrl.itemNumTx.gameObject:SetActive(self.info.state == 0)
	self.itemCtrl.lockGo:SetActive(self.info.timeId ~= Single.AutoTask().taskId and self.info.state == 0)
	self.itemCtrl.gouGo:SetActive(self.info.state == 1)
	self.itemCtrl.numberTx.text = self.numberId

	if self.info.state == 0 then
		self.itemCtrl.itemNumTx.text = "x"..self.info.rewardNum
		SetUIIcon(self.itemCtrl.icon, SingleConfig.Currency():GetKey(self.info.rewardType).icon, Vector2.New(120,120))
	end

	self.itemCtrl.finishIcon.spriteName = self.info.state == 1 and "yuandi6" or "yuandi5"
end

---隐藏窗口
function TaskItem:OnDisable()
	
end

---消耗释放资源
function TaskItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return TaskItem