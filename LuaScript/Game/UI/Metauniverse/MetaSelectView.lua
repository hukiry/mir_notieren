---
---
--- Create Time:2023-10-11
--- Author: Hukiry
---

---@class MetaSelectView:UIWindowBase
local MetaSelectView = Class(UIWindowBase)

function MetaSelectView:ctor()
	---@type MetaSelectControl
	self.ViewCtrl = nil

	---@type table<number, EPageSelectType>
	self.pageTab = {
		{ pageNameId = 16105}, { pageNameId = 16106},
		{ pageNameId = 16107}, { pageNameId = 16108}
	}

	for _, v in ipairs(self.pageTab) do
		v.selectBack = "fangkuai4"
		v.unSelectBack = "fangkuai4"
		v.selectColor = "#FFFFFF"
		v.unSelectColor = "#05778D"
	end
end

---初始属性字段
function MetaSelectView:Awake()
	self.prefabName = "MetaSelect"
	self.prefabDirName = "Metauniverse"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	---@type SelectPageView
	self.pageView =  require("Library.UIWidget.Page.ButtonListView").New()
end

---初始界面:注册按钮事件等
function MetaSelectView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.okBtn, function()
		self.callback(self.selectInfo)
		self:Close()
	end)
	self.pageView:OnEnable(self.ViewCtrl.pageListTF, self.pageTab, Handle(self, self.SelectPanel))

	---@type UnityEngine.CanvasGroup
	self.tipCanvas = self.ViewCtrl.tipBGTF:GetComponent("CanvasGroup")
	self.tipCanvas.alpha = 0
	self.restTop = {}
end

---点击页签-切换面板
function MetaSelectView:SelectPanel(index)
	local tab = Single.Meta():GetMataConfig():GetPageArray(index)
	if self.loopView == nil then
		---@type UILoopItemView
		self.loopView =  UILoopItemView.New(self.ViewCtrl.contentGo, UIItemType.SelectCardItem)
	end

	local array = self.itemMaps[index-1]
	if index>=3 then
		array = self.itemMaps[2]
	end

	self.loopView:UpdateList(tab, self.restTop[index]==nil, self.restTop[index]==nil)
	self.loopView:RefreshRender(Handle(self, self.SelectItem), array, Handle(self, self.FliterColor))
	self.restTop[index] = 1
	self.indexPage = index
end

---子卡片选择回调
---@param info ItemCfgInfo
function MetaSelectView:SelectItem(info)
	self.selectInfo = info
	local str = ''
	if info.barrierType>0 then
		str = "-<b><color=#FF6300>".. GetLanguageText(16300 + info.barrierType).."</color></b>"
	end
	local nameStr = string.format(":<b><color=#14FF5F>%s</color></b>", info:GetName())
	self.ViewCtrl.itemName.text = GetLanguageText(16005) .. nameStr..str
	self.tipCanvas.alpha = 1
	self.tipCanvas:DOKill()
	self.tipCanvas:DOFade(0,3)

	self.ViewCtrl.okBtn.raycastTarget = self.selectInfo ~= nil
	self.ViewCtrl.okBtn.IsGray = self.selectInfo == nil
end

---@param
---@return boolean true 隐藏，false 不隐藏
function MetaSelectView:FliterColor(itemId, color, itemType)
	local colArray = table.toArrayKey(self.colorTab)
	if itemType == EItemType.Obstacle then
		if #colArray >= 4 and  color ~= EColorItem.none then
			return self.colorTab[color] == nil, self.colorTab[color]
		else
			return false, false
		end
	elseif itemType == EItemType.Normal then
		local colors = self.colorTab[color]
		if #colArray >= 4 then
			---大于等于4个颜色的处理
			if colors then
				local id = (itemId%100)>6 and itemId-6 or itemId+6
				---存在其中一个普通的颜色
				if colors[itemId] or colors[id] then
					return colors[itemId] == nil, false
				end
			end
			---只存在障碍物颜色
			return colors == nil, false
		elseif colors then
			--不足4个时
			return colors[itemId] == nil, false
		end
	end
	return false, false
end

---显示窗口:初次打开
---@param callback function<number>
function MetaSelectView:OnEnable(callback)
	self.callback = callback
	self.itemMaps, self.colorTab = Single.Meta():GetClassificationItem()
	self.pageView:OnSelect(self.indexPage or 1)
	self.ViewCtrl.okBtn.raycastTarget = self.selectInfo ~= nil
	self.ViewCtrl.okBtn.IsGray = self.selectInfo == nil
end

---隐藏窗口
function MetaSelectView:OnDisable()
	self.pageView:OnDisable()
	self.loopView:OnDisable()
end

---消耗释放资源
function MetaSelectView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaSelectView