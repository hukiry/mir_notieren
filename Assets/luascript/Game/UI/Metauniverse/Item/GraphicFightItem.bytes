---
---
--- Create Time:2023-10-10
--- Author: Hukiry
---

---@class GraphicFightItem:IUIItem
local GraphicFightItem = Class(IUIItem)

function GraphicFightItem:ctor()
	---@type GraphicFightItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function GraphicFightItem:Start(isFight)
	self.isFight = isFight
	self:AddClick(self.itemCtrl.iconBack, function()
		UIManager:OpenWindow(self.isFight and ViewID.MyMapFight or ViewID.MyMapTip, self.info)
	end)

	self:AddClick(self.itemCtrl.fightBtn, function()
		UIManager:OpenWindow(ViewID.MyMapFight , self.info)
	end)

	self.itemCtrl.authordesc.gameObject:SetActive(not isFight)
	self.itemCtrl.fightBtn.gameObject:SetActive(isFight)
	self.itemCtrl.authorLabelGo:SetActive(not isFight)
end

---更新数据
---@param info MetaFightInfo
function GraphicFightItem:OnEnable(info)
	self.info = info
	local dataArray = Single.Meta():GetCacheData():GetImageData(self.info.numberId)
	local atlas = Single.SpriteAtlas():LoadAtlas(EAtlasResPath.Item)
	self.itemCtrl.iconBack:LoadMesh(atlas, dataArray)

	local infoMap = Single.Meta():GetCacheData():GetMapInfo(self.info.numberId)
	self.itemCtrl.name.text = infoMap.mapName
	self.itemCtrl.authordesc.text = infoMap.mapDesc
end

---隐藏窗口
function GraphicFightItem:OnDisable()
	
end

---消耗释放资源
function GraphicFightItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return GraphicFightItem