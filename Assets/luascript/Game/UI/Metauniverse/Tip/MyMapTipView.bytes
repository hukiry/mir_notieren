---
--- 我的作品
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MyMapTipView:UIWindowBase
local MyMapTipView = Class(UIWindowBase)

function MyMapTipView:ctor()
	---@type MyMapTipControl
	self.ViewCtrl = nil
end

---初始属性字段
function MyMapTipView:Awake()
	self.prefabName = "MyMapTip"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MyMapTipView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))

	self:AddClick(self.ViewCtrl.sendBtnGo, function()
		---todo 发送挑战
		TipMessageBox.ShowUI("正在开发中。。。")
	end)

	self:AddClick(self.ViewCtrl.playBtnGo, function()
		SingleData.Metauniverse().numberId = self.numberId
		SingleData.Metauniverse():StartTest(EMetaFightState.Fight)
		SceneApplication.ChangeState(FightScene)
	end)
end

---显示窗口:初次打开
function MyMapTipView:OnEnable(indexInfo)
	self.numberId = indexInfo.numberId
	local dataArray = Single.Meta():GetCacheData():GetImageData(self.numberId)
	local atlas = Single.SpriteAtlas():LoadAtlas(EAtlasResPath.Item)
	self.ViewCtrl.iconBack:LoadMesh(atlas, dataArray)

	self.info = Single.Meta():GetCacheData():GetMapInfo(self.numberId)
	self.ViewCtrl.idTxt.text = self.info:GetMapIdText(self.numberId)
	self.ViewCtrl.timeTxt.text = self.info:GetTimeText()
	self.ViewCtrl.authorTxt.text = GetLanguageText(16018) .. self.info.author

	self.ViewCtrl.headIcon.spriteName = "role_" .. Single.Player().headId
end

---隐藏窗口
function MyMapTipView:OnDisable()
	
end

---消耗释放资源
function MyMapTipView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MyMapTipView