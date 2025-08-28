---
---
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MyMapInfoView:UIWindowBase
local MyMapInfoView = Class(UIWindowBase)

function MyMapInfoView:ctor()
	---@type MyMapInfoControl
	self.ViewCtrl = nil
end

---初始属性字段
function MyMapInfoView:Awake()
	self.prefabName = "MyMapInfo"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MyMapInfoView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
end

---显示窗口:初次打开
function MyMapInfoView:OnEnable(index)
	self.index = index
	local dataArray = Single.Meta():GetCacheData():GetImageData(index)
	local atlas = Single.SpriteAtlas():LoadAtlas(EAtlasResPath.Item)
	self.ViewCtrl.backGraphic:LoadMesh(atlas, dataArray)

	local info = Single.Meta():GetCacheData():GetMapInfo(index)
	self.ViewCtrl.descTxt.text = GetLanguageText(16018) .. info.author
	self.ViewCtrl.nameTxt.text = info.mapName
	self.ViewCtrl.headIcon.spriteName = "role_" .. Single.Player().headId
end

---隐藏窗口
function MyMapInfoView:OnDisable()
	
end

---消耗释放资源
function MyMapInfoView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MyMapInfoView