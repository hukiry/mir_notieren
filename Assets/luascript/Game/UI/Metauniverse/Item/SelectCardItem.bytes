---
---
--- Create Time:2023-10-11
--- Author: Hukiry
---

---@class SelectCardItem:IUIItem
local SelectCardItem = Class(IUIItem)

function SelectCardItem:ctor()
	---@type SelectCardItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function SelectCardItem:Start(args)
	if args == nil then return end

	self.callback = args[1]
	---@type table<number, ItemCfgInfo>
	self.idTabs = args[2]
	self.fliterCall = args[3]
	self.idLen = 0
	self.isHaveBox =false
	if self.idTabs then
		for i, v in pairs(self.idTabs) do
			local indexId = math.floor(v.itemId/10)
			if indexId==331 or indexId==334 then
				if not self.isHaveBox then
					self.isHaveBox =true
					self.idLen = self.idLen + 1
				end
			else
				self.idLen = self.idLen + 1
			end
		end
	end

	self:AddClick(self.gameObject, function()
		if self.itemCtrl.icon.IsGray or self.info == nil then
			return
		end
		self.callback(self.info)
	end)
end

---更新数据-插件调用
---@param info ItemCfgInfo
function SelectCardItem:OnEnable(info)
	self.info = info
end

---刷新数据-插件调用
function SelectCardItem:OnRefresh(...)
	self:Start({...})
	self:_SetInfo(self.info)
end

---设置数据
---@param info ItemCfgInfo
function SelectCardItem:_SetInfo(info)
	self.itemCtrl.name.text = self.info:GetName()
	SetUIIcon(self.itemCtrl.icon, info.metaIcon, Vector2.New(140,140))

	if  info.isVertical  then
		self.itemCtrl.icon.transform:SetRotation(0,0,-90)
	else
		self.itemCtrl.icon.transform:SetRotation(0,0,0)
	end

	local infoMap = Single.Meta():GetMapInfo()
	local needLen = infoMap.obstacleNum
	if info.itemType == EItemType.Normal then
		needLen = infoMap.colorNum
	elseif info.itemType == EItemType.Item then
		needLen = 4
	end

	---颜色过滤
	local isGray, isExitCol = self.fliterCall(info.itemId, info.color, info.itemType)
	if self.idLen >= needLen then
		isGray = self.idTabs[info.itemId]==nil
		if self.isHaveBox then
			if info.itemId == 3317 or isExitCol then
				isGray = false
			end
		end
	end
	self.itemCtrl.icon.IsGray = isGray


	self.itemCtrl.tagGo:SetActive(self.info:IsBarrier())
	if self.info:IsBarrier() then
		self.itemCtrl.tagTxt.text = GetLanguageText(16300 + self.info.barrierType)
	end
end

---隐藏窗口
function SelectCardItem:OnDisable()
	
end

---消耗释放资源
function SelectCardItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return SelectCardItem