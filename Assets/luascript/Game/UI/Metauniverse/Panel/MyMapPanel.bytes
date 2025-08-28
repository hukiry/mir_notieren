---
---
--- Create Time:2023-10-10
--- Author: Hukiry
---

---@class MyMapPanel:IUIItem
local MyMapPanel = Class(IUIItem)

function MyMapPanel:ctor()
	---@type MyMapPanelControl
	self.panelCtrl = nil

	---@type table<number, EPageSelectType>
	self.pageTab = {
		{ pageNameId = 16109 },
		{ pageNameId = 16110 },
		{ pageNameId = 16111 },
	}

	for _, v in ipairs(self.pageTab) do
		v.selectBack = "fangkuai9"
		v.unSelectBack = "fangkuai8"
		v.selectColor = "#6C7779"
		v.unSelectColor = "#6C7779"
	end
end

---初始:注册按钮事件等
function MyMapPanel:Start()
	self:AddClick(self.panelCtrl.releaseBtnGo, Handle(self, self._ReleaseMsg))
	self:AddClick(self.panelCtrl.expanBtnGo, Handle(self, self._ExpandMap))
	self:AddClick(self.panelCtrl.uploadBtnGo, Handle(self, self._UploadMap))
	self:AddClick(self.panelCtrl.createmapBtnGo, Handle(self, self._CreateMap))

	---@type SelectPageView
	self.pageView =  require("Library.UIWidget.Page.ButtonListView").New()
end

function MyMapPanel:_CreateMap()
	local curNum = Single.Meta():GetMapLength()
	if not Single.Player():IsEnough(EMoneyType.metaExpendNum, curNum+1) then
		UIManager:OpenWindow(ViewID.MyMapExpand, Handle(self, self.UpdateExpand))
	else
		UIManager:OpenWindow(ViewID.MyMapCreate, true, curNum+1)
	end
end

---地图发布
function MyMapPanel:_ReleaseMsg()
	TipMessageBox.ShowUI( "地图发布")
end

---扩容地图
function MyMapPanel:_ExpandMap()
	UIManager:OpenWindow(ViewID.MyMapExpand, Handle(self, self.UpdateExpand))
end

---上传地图
function MyMapPanel:_UploadMap()
	TipMessageBox.ShowUI("上传地图")
end

---更新数据
function MyMapPanel:OnEnable()
	self.pageView:OnEnable(self.panelCtrl.menuListTF, self.pageTab, Handle(self, self.OnChangePage))
	self.pageView:OnSelect(self.indexPage or 1)

	self:UpdateExpand()
end

function MyMapPanel:UpdateExpand()
	local len = #Single.Player().metaMapIds
	self.panelCtrl.remain.text = len .. '/' ..Single.Player():GetMoneyNum(EMoneyType.metaExpendNum)
end

function MyMapPanel:OnChangePage(index)
	self.indexPage = index
	if self.loopView == nil then
		---@type UILoopItemView
		self.loopView = UIItemView.New(self.panelCtrl.contentGo, UIItemType.GraphicCardItem)
	end
	self.loopView:OnDisable()
	self.loopView:UpdateList(Single.Player().metaMapIds)
end

---隐藏窗口
function MyMapPanel:OnDisable()
	self.loopView:OnDisable()
end

---消耗释放资源
function MyMapPanel:OnDestroy()
	self.panelCtrl:OnDestroy()
end

return MyMapPanel