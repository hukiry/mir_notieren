---
---
--- Create Time:2023-07-29
--- Author: Hukiry
---

---@class PropsItem:IUIItem
local PropsItem = Class(IUIItem)

function PropsItem:ctor()
	---@type PropsItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function PropsItem:Start(callback)
	self.isSelect = false
	self:AddClick(self.gameObject , callback)
	---@type Hukiry.UI.AtlasImage
	self.bg = self.gameObject:GetComponent("AtlasImage")
end

---@return EMoneyType,boolean
function PropsItem:OnSelect()
	if self.num <= 0 then
		UIManager:OpenWindow(ViewID.Recharge)
		return nil, nil
	end

	self.isSelect = not self.isSelect
	self.itemCtrl.gouGo:SetActive(self.isSelect)
	self.itemCtrl.itemNum.gameObject:SetActive(not self.isSelect)
	--self.bg.color = self.isSelect and Color.New(0.5,0,1) or Color.white
	return self.propsType ,self.isSelect
end

---更新数据
---@param moneyType EMoneyType
function PropsItem:OnEnable(moneyType)
	self.propsType = moneyType - 4

	self.num = Single.Player():GetMoneyNum(moneyType)
	self.itemCtrl.itemNum.text = self.num<=0 and '+' or tostring(self.num)
	local  info = SingleConfig.Currency():GetKey(moneyType)
	SetUIIcon(self.itemCtrl.icon, info.icon, Vector2.New(120,120))
end

---隐藏窗口
function PropsItem:OnDisable()
	self.itemCtrl.gouGo:SetActive(false)
	self.itemCtrl.itemNum.gameObject:SetActive(true)
	--self.bg.color =  Color.white
end

---消耗释放资源
function PropsItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return PropsItem