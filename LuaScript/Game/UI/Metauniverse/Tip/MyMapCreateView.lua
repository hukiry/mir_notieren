---
---
--- Create Time:2023-10-11
--- Author: Hukiry
---

---@class MyMapCreateView:UIWindowBase
local MyMapCreateView = Class(UIWindowBase)

function MyMapCreateView:ctor()
	---@type MyMapCreateControl
	self.ViewCtrl = nil
end

---初始属性字段
function MyMapCreateView:Awake()
	self.prefabName = "MyMapCreate"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	self.normalTab = {
		{text = '2'},{text = '3'},{text = '4'}
	}

	self.mapName, self.mapDesc, self.totalMove, self.colorNum, self.obstacleNum  = '', '', 0, 0, 0
end

---初始界面:注册按钮事件等
function MyMapCreateView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.createBtnGo , Handle(self, self._Save))

	self.ViewCtrl.inputFieldName.onEndEdit:AddListener(Handle(self, self._OnInputEnd, 1))
	self.ViewCtrl.inputFieldDesc.onEndEdit:AddListener(Handle(self, self._OnInputEnd, 2))
	self.ViewCtrl.inputFieldMove.onEndEdit:AddListener(Handle(self, self._OnInputEnd, 3))

	---@type DropdownView
	self.dropdownTrue = require("Library.UIWidget.Dropdown.DropdownView").New()
	---@type DropdownView
	self.dropdownFalse = require("Library.UIWidget.Dropdown.DropdownView").New()
end

function MyMapCreateView:_Save()
	if string.len(self.mapName) <= 0 then
		TipMessageBox.ShowUI(GetLanguageText(16212))
		return
	elseif string.len(self.mapDesc) <= 0 then
		TipMessageBox.ShowUI(GetLanguageText(16213))
		return
	end

	local info = Single.Meta():GetCacheData():GetMapInfo(self.mapNumber)
	info.mapDesc = self.mapDesc
	info.mapName = self.mapName
	info.totalMove = self.totalMove
	info.colorNum = self.colorNum
	info.obstacleNum = self.obstacleNum
	info.updateTime = os.time()
	info.author = Single.Player().roleNick
	if self.isCreate then
		Single.Meta().numberId = self.mapNumber
		Single.Player():AddMetaMap(self.mapNumber)
		SceneApplication.ChangeState(MetaScene)
	else
		Single.Meta():GetCacheData():SaveEditorInfo(info, self.mapNumber)
	end
	self:Close()

	TipMessageBox.ShowUI(GetLanguageText(16016), true)
end

function MyMapCreateView:_OnInputEnd(index, v)
	local txt = string.Trim(v)
	if index ==1 then
		self.mapName = txt
	elseif index ==2 then
		self.mapDesc = txt
	else
		self.totalMove = tonumber(txt)
	end
end

---显示窗口:初次打开
function MyMapCreateView:OnEnable(isCreate, mapNumber)
	self.mapNumber = mapNumber
	self.isCreate = isCreate
	self.ViewCtrl.createTxt.text = GetLanguageText(isCreate and 10017 or 13002)
	self.ViewCtrl.nameTxt.text = GetLanguageText(isCreate and 16203 or 16202)
	self.dropdownTrue:OnEnable(self.ViewCtrl.dropdownNormal, self.normalTab, Handle(self, self._SelectNormal, true),2)
	self.dropdownFalse:OnEnable(self.ViewCtrl.dropdownObs, self.normalTab, Handle(self, self._SelectNormal, false),2)
	local info = Single.Meta():GetCacheData():GetMapInfo(self.mapNumber)

	self.dropdownTrue:SetDropdownValue(info.colorNum<2 and 0 or info.colorNum-2)
	self.dropdownFalse:SetDropdownValue(info.obstacleNum<2 and 0 or info.obstacleNum-2)

	self.dropdownFalse:SetHideDropDown({ self.ViewCtrl.dropdownNormal })
	self.dropdownTrue:SetHideDropDown({self.ViewCtrl.dropdownObs})

	self.totalMove, self.mapName, self.mapDesc = info.totalMove,info.mapName,info.mapDesc
	self.colorNum, self.obstacleNum = info.colorNum, info.obstacleNum
	self.ViewCtrl.inputFieldMove.text = info.totalMove
	self.ViewCtrl.inputFieldName.text = info.mapName
	self.ViewCtrl.inputFieldDesc.text = info.mapDesc

	self.ViewCtrl.mapIdTxt.text = "ID:"..mapNumber
end

function MyMapCreateView:_SelectNormal(isNormal, index)
	if isNormal then
		self.colorNum = index + 2
	else
		self.obstacleNum = index + 2
	end
end

---隐藏窗口
function MyMapCreateView:OnDisable()
	self.dropdownTrue:OnDisable()
	self.dropdownFalse:OnDisable()
end

---消耗释放资源
function MyMapCreateView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MyMapCreateView