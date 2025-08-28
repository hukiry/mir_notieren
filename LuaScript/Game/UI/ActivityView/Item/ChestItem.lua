---
---
--- Create Time:2023-08-02
--- Author: Hukiry
---

---@class ChestItem:IUIItem
local ChestItem = Class(IUIItem)

function ChestItem:ctor()
	---@type ChestItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function ChestItem:Start(index)
	self.numberId = index
	self.itemList = {}
	self:AddClick(self.itemCtrl.getBtnGo, Handle(self, self.PayOrFree))
	local childCount = self.itemCtrl.horizontalTF.childCount
	for i = 1, childCount do
		local tf =self.itemCtrl.horizontalTF:GetChild(i-1)
		self.itemList[i] = {}
		self.itemList[i].gameObject = tf.gameObject
		self.itemList[i].image = tf.gameObject:GetComponent("AtlasImage")
		self.itemList[i].numTx = tf:GetChild(0).gameObject:GetComponent("HukirySupperText")
	end
end

function ChestItem:PayOrFree()
	if self.numberId == 1 then
		if SingleData.Activity():GetChest():IsBuy() then
			return
		end

		local info = SingleData.Activity():GetChest():GetRechargeInfo()
		Single.SdkPlatform():GetPayInfo():FetchSDK(info.googlePayId,function()
			Single.Request().SendShop(EHttpShopState.Other, info.payId, info.price, Handle(self, self.OnFinish))---后面接入sdk
		end)

	elseif SingleData.Activity():GetChest():GetState(self.numberId) == 1 then
		self:OnFinish()
	end
end

function ChestItem:OnFinish()
	for _, v in ipairs(self.array) do
		SingleData.Activity():PlayReward(EActivityType.chest, v.moneyType, v.number, HandleParams(function(ty, number)
			Single.Player():SetMoneyNum(ty, number)
		end, v.moneyType))
	end

	SingleData.Activity():GetChest():RequestProgressbar(self.numberId)
	UIManager:CloseWindow(ViewID.Chest)
end

---@param array table<number, ChestInfo> 集合
function ChestItem:OnEnable(array)
	self.array = array
	for i, v in pairs(self.itemList) do
		v.gameObject:SetActive(array[i]~=nil)
		if array[i] then
			v.numTx.text = array[i]:GetNumberText()
			SetUIIcon(v.image, array[i]:GetItemIcon(), Vector2.New(130,130))
		end
	end

	self.itemCtrl.coinNum.text  = self.numberId == 1 and SingleData.Activity():GetChest():GetShowPrice() or GetLanguageText(15103)
	if self.numberId == 1 and SingleData.Activity():GetChest():IsBuy() then
		self.itemCtrl.coinNum.text = GetLanguageText(15302)
	elseif SingleData.Activity():GetChest():GetState(self.numberId) == 2 then
		self.itemCtrl.coinNum.text = GetLanguageText(15302)
	end
	local state = SingleData.Activity():GetChest():GetState(self.numberId)
	self.itemCtrl.lockGo:SetActive(state == 0 and self.numberId ~= 1)

end

---隐藏窗口
function ChestItem:OnDisable()
	
end

---消耗释放资源
function ChestItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return ChestItem