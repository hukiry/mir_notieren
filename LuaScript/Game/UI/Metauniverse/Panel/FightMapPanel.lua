---
---
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class FightMapPanel:DisplayObjectBase
local FightMapPanel = Class(DisplayObjectBase)

function FightMapPanel:ctor()
	---@type FightMapPanelControl
	self.panelCtrl = nil

	self.pageTab = {
		{ pageNameId = 16125 },
		{ pageNameId = 16126 }
	}

	for _, v in ipairs(self.pageTab) do
		v.selectBack = "fangkuai9"
		v.unSelectBack = "fangkuai8"
		v.selectColor = "#8D5810"
		v.unSelectColor = "#B6C0C9"
	end
end

---初始界面:注册按钮事件等
function FightMapPanel:Start()
	---@type SelectPageView
	self.pageView =  require("Library.UIWidget.Page.ButtonListView").New()
end

---事件派发
---@param state EFriendHandleType
function FightMapPanel:OnDispatch(state)
	log("FightMapPanel 刷新我的地图UI",state,"red")
end

---显示窗口:初次打开
function FightMapPanel:OnEnable(...)
	self.pageView:OnEnable(self.panelCtrl.pageListTF, self.pageTab, Handle(self, self.OnChangePage))
	self.pageView:OnSelect(self.indexPage or 1)
end

---@param index number 1=我的作品，2=挑战
function FightMapPanel:OnChangePage(index)
	self.indexPage = index
	if self.loopView == nil then
		---@type UILoopItemView
		self.loopView = UIItemView.New(self.panelCtrl.contentGo, UIItemType.GraphicFightItem)
	end
	self.loopView:OnDisable()
	local metaMapIds = self.indexPage == 1 and Single.Player().metaMapIds or SingleData.Metauniverse():GetMetaHomeArray()
	self.loopView:UpdateList(metaMapIds, false, false,index == 2)
end

---隐藏窗口
function FightMapPanel:OnDisable()
	self.loopView:OnDisable()
end

---消耗释放资源
function FightMapPanel:OnDestroy()
	self.panelCtrl:OnDestroy()
end

return FightMapPanel