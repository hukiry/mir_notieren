---
---
--- Create Time:2023-06-23
--- Author: Hukiry
---

---@class TargetItem:IUIItem
local TargetItem = Class(IUIItem)

function TargetItem:ctor()
	---@type TargetItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function TargetItem:Start()
	---@type Hukiry.UI.AtlasImage
	self.icon = self.itemCtrl.gameObject:GetComponent("AtlasImage")
end

---更新数据
function TargetItem:OnEnable(itemId, itemNum)
	self.itemId = itemId
	self.itemNum = itemNum
	self.isBowl = math.floor(self.itemId/10) == 332

	local item = SingleConfig.Item():GetKey(itemId)
	SetUIIcon(self.icon, item.targetIcon, Vector2.New(100,100))
	self.itemCtrl.num.text = self.itemNum
	self.itemCtrl.gouGo:SetActive(self.itemNum<=0)

	self.itemCtrl.num.gameObject:SetActive(true)
end

function TargetItem:Update()
	self.itemNum = self.itemNum - 1
	self.itemCtrl.num.text = self.itemNum

	self.itemCtrl.gouGo.transform:DOKill()
	self.itemCtrl.gouGo:SetActive(self.itemNum<=0)
	self.itemCtrl.gouGo.transform:DOScale(Vector3.one*1.2,0.3):OnComplete(function()
		self.itemCtrl.gouGo.transform:SetScale(1,1,1)
	end)

	self.itemCtrl.num.gameObject:SetActive(self.itemNum>0)
	self.itemCtrl.num.transform:DOKill()
	self.itemCtrl.num.transform:DOScale(Vector3.one*1.2, 0.3):OnComplete(function()
		self.itemCtrl.num.transform:SetScale(1,1,1)
	end)

	Single.Match():UpdateTarget(self.itemId)
end

---@return boolean
function TargetItem:IsFinish()
	return self.itemNum<=0
end

---隐藏窗口
function TargetItem:OnDisable()
	
end

---消耗释放资源
function TargetItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return TargetItem