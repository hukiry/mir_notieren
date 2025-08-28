---
---
--- Create Time:2023-10-10
--- Author: Hukiry
---

---@class MetaHomeView:UIWindowBase
local MetaHomeView = Class(UIWindowBase)

function MetaHomeView:ctor()
	---@type MetaHomeControl
	self.ViewCtrl = nil

	---@type table<number, EPageSelectType>
	self.pageTab = {
		{ pageNameId = 16101, selectIcon = "y_role2", unSelectIcon = "y_role1"},
		{ pageNameId = 16103, selectIcon = "y_fight2", unSelectIcon = "y_fight1"},
		{ pageNameId = 16102, selectIcon = "y_ditu2", unSelectIcon = "y_ditu1"},
		{ pageNameId = 13007, selectIcon = "y_set2", unSelectIcon = "y_set1"},
		{ pageNameId = 16104, selectIcon = "y_back2", unSelectIcon = "y_back1"}
	}

	for _, v in ipairs(self.pageTab) do
		v.selectBack = "fangkuai9"
		v.unSelectBack = "fangkuai8"
		v.selectColor = "#8D5810"
		v.unSelectColor = "#B6C0C9"
	end

	self.panelNames = {
		[1] = UIPanelType.PlayerMapPanel, [2] = UIPanelType.FightMapPanel,
		[3] = UIPanelType.MyMapPanel, [4] =UIPanelType.SettingMapPanel,
		[5] = UIPanelType.QuitMapPanel
	}
end

---初始属性字段
function MetaHomeView:Awake()
	self.prefabName = "MetaHome"
	self.prefabDirName = "Metauniverse"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	self.lastIndex = 3
	---@type SelectPageView
	self.pageView =  require("Library.UIWidget.Page.ButtonListView").New()
end

---初始界面:注册按钮事件等
function MetaHomeView:Start()
	self.pageView:OnEnable(self.ViewCtrl.pageListTF, self.pageTab, Handle(self, self.SelectPanel))
	---@type table<number, MyMapPanel>
	self.itemList = {}
end

---事件派发
---@param state EFriendHandleType
function MetaHomeView:OnDispatch(state)
	for i, v in pairs(self.itemList) do
		if v.OnDispatch then
			v:OnDispatch(state)
		end
	end
end

---@param index number 选择的索引
function MetaHomeView:SelectPanel(index)
	if self.indexPanel == index then
		return
	end

	self.indexPanel = index
	for i, v in pairs(self.itemList) do
		v:OnDisable()
		v.gameObject:SetActive(i==index)
	end

	if self.itemList[index] == nil then
		self.itemList[index] = UIPanelPool.Get(self.panelNames[index], self.ViewCtrl.panelGo)
		self.itemList[index]:SetAnchorFull()
	end
	self.itemList[index]:OnEnable()

	if index == 2 then
		self:SendMeta()
	end
end

---显示窗口:初次打开
function MetaHomeView:OnEnable(...)
	self.pageView:OnSelect( self.lastIndex)
	self:SendMeta()
end

function MetaHomeView:SendMeta()
	if Single.Http():IsHaveNetwork() and not SingleData.Metauniverse():IsHave() then
		Single.Request().SendMeta(EFriendHandleType.battle)
	end
end

---隐藏窗口
function MetaHomeView:OnDisable()
	self.pageView:OnDisable()
	for index, v in pairs(self.itemList) do
		UIPanelPool.Put(self.panelNames[index], v)
	end
	self.lastIndex = self.indexPanel==5 and 3 or self.indexPanel
	self.indexPanel = 0
	self.itemList = {}
end

---消耗释放资源
function MetaHomeView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaHomeView