---
---
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class ChestView:UIWindowBase
local ChestView = Class(UIWindowBase)

function ChestView:ctor()
	---@type ChestControl
	self.ViewCtrl = nil
end

---初始属性字段
function ChestView:Awake()
	self.prefabName = "Chest"
	self.prefabDirName = "ActivityView"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function ChestView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	---@type table<number, ChestItem>
	self.itemList = {}
end

---启动计时器后，调用此方法
function ChestView:OnTimer()
	self.ViewCtrl.remainTime.text = self.info:GetActRemainTime()
end

---显示窗口:初次打开
---@param info ChestActivity
function ChestView:OnEnable(info)
	self.info = info
	self.ViewCtrl.remainTime.text = self.info:GetActRemainTime()
	for i = 1, 3 do
		local array = self.info:GetArrayInfo(i)
		if self.itemList[i] == nil then
			self.itemList[i] = UIItemPool.Get(UIItemType.ChestItem, self.ViewCtrl.verticalLayGo, i)
		end
		self.itemList[i]:OnEnable(array)
	end
end

---隐藏窗口
function ChestView:OnDisable()
	UIItemPool.PutTable(UIItemType.ChestItem, self.itemList)
end

---消耗释放资源
function ChestView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return ChestView