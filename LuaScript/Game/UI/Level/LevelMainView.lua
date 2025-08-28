---
---
--- Create Time:2023-06-23
--- Author: Hukiry
---

---@class LevelMainView:UIWindowBase
local LevelMainView = Class(UIWindowBase)

function LevelMainView:ctor()
	---@type LevelMainControl
	self.ViewCtrl = nil
end

---初始属性字段
function LevelMainView:Awake()
	self.prefabName = "LevelMain"
	self.prefabDirName = "Level"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1


	MatchRule.isUseProp = false
	---@type table<number, TargetItem>
	self.itemList = {}
	---@type LevelPropsAnimation
	self.propsAni = require("Game.UI.Level.LevelPropsAnimation").New()
end

---初始界面:注册按钮事件等
function LevelMainView:Start()
	---@type PropPartView
	self.propPartView = require("Game.UI.Level.Part.PropPartView").New(self.ViewCtrl.downGridGo)
	self.propPartView:Start(self)
	self.ProgresssMax = 1
	self.propsAni:Start(self.ViewCtrl.firstPropsGo, self.ViewCtrl.slider, self.ViewCtrl.targetPosTF, self.ViewCtrl.mfTF.position)
	self.xiaoTime = 0

	self.mataTime = 0
	self:AddClick(self.ViewCtrl.quitBtnGo, Handle(self, self.QuitFight))
end

---元宇宙测试和战斗
function LevelMainView:QuitFight()
	local text = GetLanguageText(SingleData.Metauniverse().fightState == EMetaFightState.Test and 16214 or 16215)
	UIManager:OpenWindow(ViewID.CommonTip, text, function()
		local _isFlight = SingleData.Metauniverse():StartQuitFight()
		SceneApplication.ChangeState(_isFlight and HomeScene or MetaScene)
	end)
end

---计时器
function LevelMainView:OnTimer()
	self.mataTime = self.mataTime + 1
	if not self.isFlight then
		self.xiaoTime = self.xiaoTime+1
		if self.xiaoTime>=10 then
			self.xiaoTime = 0
			self.ViewCtrl.iconAn.spriteName = "xl_1"
			self.ViewCtrl.iconAn.transform:DOScale(Vector3.one, 0.3):OnComplete(function()
				self.ViewCtrl.iconAn.spriteName = "xl_0"
			end)
		end
	else
		self.ViewCtrl.mapTime.text = Util.Time().GetTimeStringBySecond(self.mataTime)
		SingleData.Metauniverse().playTime = self.mataTime
	end
end

---事件派发
---@param state number 1=移动次数，2= 目标数量显示, 3=使用， 4=更新道具数量
function LevelMainView:OnDispatch(state, itemId, x, y)
	if state == 1 then
		self.ViewCtrl.moveNum.text = Single.Match().totalMove
	elseif state == 2 then
		--更新目标物品
		local id = Single.Match():ConvetID(itemId)
		if self.itemList[id] then
			self.itemList[id]:Update()
		end
	elseif state == 5 then
		self.ViewCtrl.sliderTree.fillAmount = Single.Match():GetCurrentTargetNumber(self.ProgresssMax)/self.ProgresssMax
	end
	self.propPartView:OnDispatch(state, itemId, x, y)
end

---显示窗口:初次打开
function LevelMainView:OnEnable(isFlight)
	self.isFlight = isFlight or false
	self.ViewCtrl.bgHeadGo:SetActive(not self.isFlight)
	self.ViewCtrl.metaBoxGo:SetActive(self.isFlight)
	self.propPartView.gameObject:SetActive((not self.isFlight))
	self.ViewCtrl.quitBtnGo:SetActive(self.isFlight)
	---初始化目标和移动
	self:OnRefresh()
	if self.isFlight then
		self.mataTime = 0
		self.ViewCtrl.nickName.text = Single.Player().roleNick
		self.ViewCtrl.mapName.text = Single.Meta():GetMapInfo().mapName
		self.ViewCtrl.mapTag.text = GetLanguageText(SingleData.Metauniverse():IsFight() and 16126 or 16114)
		self.ViewCtrl.firstPropsGo:SetActive(false)

	else
		self.propPartView:OnEnable()
		self.propsAni:OnEnable()
	end
end

---获取目标视图对象
---@param itemId number 实际物品id
---@return TargetItem
function LevelMainView:GetTargetItem(itemId)
	local id = Single.Match():ConvetID(itemId)
	return self.itemList[id]
end

---显示中重复打开
function LevelMainView:OnRefresh()
	self.ProgresssMax = 0
	UIItemPool.PutTable(UIItemType.TargetItem, self.itemList)
	local targetIds = Single.Match():GetTargetItem()
	for i, v in ipairs(targetIds) do
		if self.itemList[v.itemId] ==  nil then
			self.itemList[v.itemId] = UIItemPool.Get(UIItemType.TargetItem, self.ViewCtrl.targetHorizontalGo)
		end
		self.itemList[v.itemId]:OnEnable(v.itemId, v.itemNum)
		self.ProgresssMax = self.ProgresssMax + v.itemNum
	end
	self.ViewCtrl.moveNum.text = Single.Match().totalMove
	self.ViewCtrl.sliderTree.ProgresssMax = self.ProgresssMax
	self.ViewCtrl.sliderTree.fillAmount = 0
end

---隐藏窗口
function LevelMainView:OnDisable()
	MatchRule.isUseProp = false
	UIItemPool.PutTable(UIItemType.TargetItem, self.itemList)
end

---消耗释放资源
function LevelMainView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return LevelMainView