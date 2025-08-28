---
---
--- Create Time:2023-10-16
--- Author: Hukiry
---

---@class MyMapSettingView:UIWindowBase
local MyMapSettingView = Class(UIWindowBase)

function MyMapSettingView:ctor()
	---@type MyMapSettingControl
	self.ViewCtrl = nil
end

---初始属性字段
function MyMapSettingView:Awake()
	self.prefabName = "MyMapSetting"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MyMapSettingView:Start()
	---@type ToggleView
	self.toggleMusic =  require("Library.UIWidget.Toggle.ToggleView").New()
	---@type ToggleView
	self.toggleMusicEffect =  require("Library.UIWidget.Toggle.ToggleView").New()

	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.saveBtnGo, Handle(self, self._OnSave))
	---@type table<number, TargetEditorItem>
	self.targetList = {}
	---@type table<number, UnityEngine.UI.InputField>
	self.inputList = {}

	self.inputIds = {}

	local childCount = self.ViewCtrl.targetGridTF.childCount
	for i = 1, childCount do
		---@type UnityEngine.UI.InputField
		self.inputList[i] = self.ViewCtrl.targetGridTF:GetChild(i-1):GetComponent("InputField")
		self.inputList[i].onEndEdit:AddListener(Handle(self, self._OnInputEnd, i))
	end
end

function MyMapSettingView:_OnSave()
	local info = Single.Meta():GetMapInfo()

	for id, number in pairs(self.inputIds) do
		local value = nil
		for i, v in ipairs(info.jsonItems) do
			if v.itemId == id then
				value = v
				break
			end
		end

		if value then
			value.number = number
		else
			table.insert(info.jsonItems,{
				itemId = id,
				---目标数量
				itemNum = number,
				---x下落坐标集合
				xTab = { math.random(-4,4) }
			})
		end
	end

	TipMessageBox.ShowUI(GetLanguageText(16016))
	self:Close()
end

function MyMapSettingView:_OnInputEnd(index, value)
	local itemId = self.targetList[index].itemId
	local number = self.targetIds[itemId]
	local inputNumber = tonumber(value)
	if inputNumber >= number then
		self.inputIds[itemId] = inputNumber
	else
		self.inputList[index].text = number
		TipMessageBox.ShowUI(GetLanguageText(16022), true)
	end
end

---显示窗口:初次打开
function MyMapSettingView:OnEnable(...)
	local MusicMute = Single.PlayerPrefs():GetBool(EGameSetting.MusicMute, false)
	local SoundMute = Single.PlayerPrefs():GetBool(EGameSetting.SoundMute, false)
	self.toggleMusic:OnEnable(self.ViewCtrl.musicBackTF, Handle(self, self.ChangeValue, true), MusicMute)
	self.toggleMusicEffect:OnEnable(self.ViewCtrl.musicEffectTF, Handle(self, self.ChangeValue, false),SoundMute)

	self.targetIds = Single.Meta():GetObstacleNumbers(true)
	local info = Single.Meta():GetMapInfo()


	local childCount = self.ViewCtrl.avaibleGridTF.childCount
	for i = 1, childCount do
		local tf = self.ViewCtrl.avaibleGridTF:GetChild(i-1)
		tf.gameObject:SetActive(false)
	end

	local index = 0
	for id, number in pairs(self.targetIds) do
		local tf = self.ViewCtrl.avaibleGridTF:GetChild(index)
		tf.gameObject:SetActive(true)
		index = index + 1
		if self.targetList[index] == nil then
			self.targetList[index] = require(ItemPoolRule[UIItemType.TargetEditorItem].itemClass).New(tf.gameObject)
			self.targetList[index].itemCtrl = require(ItemPoolRule[UIItemType.TargetEditorItem].itemClass.."Control").New(tf.gameObject)
			self.targetList[index]:Start(...)
		end
		self.targetList[index]:OnEnable(id, number)
		self.inputList[index].text = number
		for i, v in ipairs(info.jsonItems) do
			if id == v.itemId then
				self.inputList[index].text = v.itemNum
			end
		end
	end

	childCount = self.ViewCtrl.targetGridTF.childCount
	for i = 1, childCount do
		local tf = self.ViewCtrl.targetGridTF:GetChild(i-1)
		tf.gameObject:SetActive(index >= i)
	end

end

function MyMapSettingView:ChangeValue(isMusic, value)
	if isMusic then
		Single.Sound():SetMusicMute(value)
	else
		Single.Sound():SetSoundMute(value)
	end
end

---隐藏窗口
function MyMapSettingView:OnDisable()
	
end

---消耗释放资源
function MyMapSettingView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MyMapSettingView