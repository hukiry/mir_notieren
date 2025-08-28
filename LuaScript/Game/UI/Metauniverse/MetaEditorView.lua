---
---
--- Create Time:2023-09-15
--- Author: Hukiry
---

---@class MetaEditorView:UIWindowBase
local MetaEditorView = Class(UIWindowBase)

function MetaEditorView:ctor()
	---@type MetaEditorControl
	self.ViewCtrl = nil
end

---初始属性字段
function MetaEditorView:Awake()
	self.prefabName = "MetaEditor"
	self.prefabDirName = "Metauniverse"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1
end

---初始界面:注册按钮事件等
function MetaEditorView:Start()
	self:AddClick(self.ViewCtrl.quitBtnGo, Handle(self, self.Quit))

	---擦除物品
	self:AddClick(self.ViewCtrl.delBtn, Handle(self, self.EditorItem, ESelectState.Delete))
	---选择物品
	self:AddClick(self.ViewCtrl.itemBtn, Handle(self, self.EditorItem, ESelectState.AddItem))
	---撤回
	self:AddClick(self.ViewCtrl.resetBtn, Handle(self, self.EditorItem, ESelectState.ResetDo))

	---设置
	self:AddClick(self.ViewCtrl.setBtnGo, function()
		UIManager:OpenWindow(ViewID.MyMapSetting)
	end)

	self:AddClick(self.ViewCtrl.editorBtnGo, Handle(self, self.TestSelf))

	---@type table<number, TargetEditorItem>
	self.targetList ={}

	---@type DropdownView
	self.dropdownView = require("Library.UIWidget.Dropdown.DropdownView").New()
end

---自己测试
function MetaEditorView:TestSelf()
	local obsIds = Single.Meta():GetObstacleNumbers()
	if table.length(obsIds) == 0 then
		TipMessageBox.ShowUI(GetLanguageText(16023), true)
	else
		--测试弹框，测试成功，弹出发布
		UIManager:OpenWindow(ViewID.MetaTip, GetLanguageText(16210), function()
			Single.Meta():GetCacheData():SaveEditorData()
			--切换到测试场景,
			SingleData.Metauniverse():StartTest()
			SceneApplication.ChangeState(FightScene)
		end)
	end
end

function MetaEditorView:Quit()
	UIManager:OpenWindow(ViewID.MetaTip, GetLanguageText(16204), function()
		Single.Meta():GetCacheData():SaveEditorData()
		self:Close()
		SceneApplication.ChangeState(HomeScene)
	end, function()
		self:Close()
		SceneApplication.ChangeState(HomeScene)
	end)
end

---@param state
function MetaEditorView:EditorItem(state)
	if state == ESelectState.AddItem then
		---@param info ItemCfgInfo
		UIManager:OpenWindow(ViewID.MetaSelect, function(info)
			self.editorInfo = info
			Single.Meta().selectLayer = info:GetLayerView()
			Single.Meta().selectItemId = info.itemId
			Single.Meta().isVertical = info.isVertical == true
			Single.Meta().selectMode = ESelectState.AddItem

			SetUIIcon(self.ViewCtrl.selectIcon, info.metaIcon, Vector2.New(120,120))
			self.ViewCtrl.selectIcon.transform:SetRotation(0,0, info.isVertical == true and -90 or 0)
			self.ViewCtrl.delIcon.spriteName = "y_cuizi2"
			self.ViewCtrl.mapTip.text = GetLanguageText(16009)
			self.dropdownView:SetDropdownValue(info:GetLayerView())
			self.dropdownView:SetActive(Single.Meta().selectMode == ESelectState.Delete)

			EventDispatch:Broadcast(UIEvent.Meta_Operate_LayerView, Single.Meta().selectLayer)
		end)
	elseif state == ESelectState.ResetDo then
		if Single.Meta().isResetDo then
			Single.Meta().isResetDo = false
			self.ViewCtrl.resetBtn.spriteName = "mianban3"
			self.ViewCtrl.mapTip.text = GetLanguageText(16012)
			EventDispatch:Broadcast(UIEvent.Meta_Operate_ResetDo)
		else
			TipMessageBox.ShowUI( GetLanguageText(16013))
		end
	elseif state == ESelectState.Delete then
		Single.Meta().selectMode = Single.Meta().selectMode == state and ESelectState.AddItem or state
		self.ViewCtrl.delIcon.spriteName = Single.Meta().selectMode == state and "y_write1" or "y_cuizi2"

		if self.editorInfo then
			local layerView = self.editorInfo:GetLayerView()
			if Single.Meta().selectMode == ESelectState.AddItem and Single.Meta().selectLayer~= layerView then
				Single.Meta().selectLayer = layerView
				EventDispatch:Broadcast(UIEvent.Meta_Operate_LayerView, layerView)
			else
				self.dropdownView:SetDropdownValue(layerView)
			end
		end
	end
	self.dropdownView:SetActive(Single.Meta().selectMode == ESelectState.Delete)
end

---启动计时器后，调用此方法
function MetaEditorView:OnTimer()
	self.startTime = self.startTime + 1
	local str = string.format(":<b>%s</b>", Util.Time().GetTimeStringBySecond(self.startTime))
	self.ViewCtrl.mapEditorTime.text = GetLanguageText(16201) .. str
end

---事件派发
---@param state number 1=成功创建，2=成功删除 3=替换 4=相同物品警告 5=撤回
function MetaEditorView:OnDispatch(state, info)
	if state <= 3 and info  then
		local colStr = string.format(" <b><color=#14FF5F>%s</color></b>", info:GetName())
		local lanId = 16006+state
		if state == 3 then
			lanId = 16011
		end
		self.ViewCtrl.mapTip.text = GetLanguageText(lanId) .. colStr
		Single.Meta().isResetDo = true
		self.ViewCtrl.resetBtn.spriteName = "fangkuai9"
		self:ChangeTarget()
	elseif state == 4 then
		self.ViewCtrl.mapTip.text = "<color=#FF2626>".. GetLanguageText(16010).."</color>"
	elseif state == 5 then
		self:ChangeTarget()
	end
end

---刷新目标统计视图
function MetaEditorView:ChangeTarget()
	local obsIds = Single.Meta():GetObstacleNumbers()

	UIItemPool.PutTable(UIItemType.TargetEditorItem, self.targetList)
	for id, number in pairs(obsIds) do
		if self.targetList[id] == nil then
			self.targetList[id] = UIItemPool.Get(UIItemType.TargetEditorItem, self.ViewCtrl.targetBackGo)
		end
		self.targetList[id]:OnEnable(id, number)
	end
end

---显示窗口:初次打开
function MetaEditorView:OnEnable(...)
	self.editorInfo = Single.Meta():GetSelectInfo()
	if self.editorInfo then
		SetUIIcon(self.ViewCtrl.selectIcon, self.editorInfo.metaIcon,Vector2.New(120,120))
	else
		self.ViewCtrl.selectIcon.spriteName = "baseboard"
	end
	self.startTime = 0
	self.ViewCtrl.resetBtn.spriteName = "mianban3"
	self.ViewCtrl.mapTip.text = ''
	self.ViewCtrl.authorLv.text = "Lv:<b>"..Single.Player():GetMoneyNum(EMoneyType.level).."</b>"
	self.ViewCtrl.mapName.text = "<b>".. Single.Meta():GetMapInfo().mapName.."</b>"

	self:ChangeTarget()

	local downTab = {}
	table.insert(downTab, {text = GetLanguageText(16307)})
	table.insert(downTab, {text = GetLanguageText(16305)})
	table.insert(downTab, {text = GetLanguageText(16306)})
	self.dropdownView:OnEnable(self.ViewCtrl.dropdownLayer, downTab, Handle(self, self.OnValueChanged))
	self.dropdownView:SetActive()
	EventDispatch:Broadcast(UIEvent.Meta_Operate_LayerView, Single.Meta().selectLayer)

end

function MetaEditorView:OnValueChanged(index)
	Single.Meta().selectLayer = index
	EventDispatch:Broadcast(UIEvent.Meta_Operate_LayerView, Single.Meta().selectLayer)
end

---隐藏窗口
function MetaEditorView:OnDisable()
	UIItemPool.PutTable(UIItemType.TargetEditorItem, self.targetList)
end

---消耗释放资源
function MetaEditorView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaEditorView