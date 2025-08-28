---
---
--- Create Time:2023-10-10
--- Author: Hukiry
---

---@class GraphicCardItem:IUIItem
local GraphicCardItem = Class(IUIItem)

function GraphicCardItem:ctor()
	---@type GraphicCardItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function GraphicCardItem:Start()
	self:AddClick(self.itemCtrl.editorBtnGo, Handle(self, self._OnEditor))

	self:AddClick(self.itemCtrl.lookBtnGo, function()
		UIManager:OpenWindow(ViewID.MyMapCreate, false, self.numberId)
	end)

	self:AddClick(self.itemCtrl.iconBack, function()
		UIManager:OpenWindow(ViewID.MyMapInfo, self.numberId)
	end)
end

function GraphicCardItem:_OnEditor()
	local strName = string.format("<b><color=#6718C3>~%s~</color></b>\n", self.info.mapName)
	UIManager:OpenWindow(ViewID.MetaTip, strName..GetLanguageText(16017), function()
		Single.Meta().numberId = self.numberId
		SingleData.Metauniverse().numberId = self.numberId
		SceneApplication.ChangeState(MetaScene)
	end)
end

---更新数据
---@param indexInfo
function GraphicCardItem:OnEnable(indexInfo)
	self.numberId = indexInfo.numberId
	---@type number state=0 设计中，1=更新中，2= 已上传
	self.state = indexInfo.state
	local dataArray = Single.Meta():GetCacheData():GetImageData(self.numberId)
	local atlas = Single.SpriteAtlas():LoadAtlas(EAtlasResPath.Item)
	self.itemCtrl.iconBack:LoadMesh(atlas, dataArray)

	self.info = Single.Meta():GetCacheData():GetMapInfo(self.numberId)
	local len = Util.String().Length(self.info.mapDesc)
	if len >= 20 then
		self.itemCtrl.desc.text = Util.String().Sub(self.info.mapDesc, 0, 20) .. "..."
	else
		self.itemCtrl.desc.text = self.info.mapDesc
	end

	self.itemCtrl.stateBg.spriteName = "hengtiao"..(self.state == 2 and "4" or "3")
	self.itemCtrl.name.text = self.info.mapName
	self.itemCtrl.stateTxt.text  = GetLanguageText(16110 + self.state)
	self.itemCtrl.stateTxt.color = Hukiry.HukiryUtil.StringToColor(self.state == 2 and "#8D5810" or "#DBE3E9")
end

---隐藏窗口
function GraphicCardItem:OnDisable()
	
end

---消耗释放资源
function GraphicCardItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return GraphicCardItem