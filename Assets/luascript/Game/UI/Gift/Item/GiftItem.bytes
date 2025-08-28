---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

---@class GiftItem:DisplayObjectBase
local GiftItem = Class(DisplayObjectBase)

function GiftItem:ctor()
	---@type GiftItemControl
	self.itemCtrl = nil

	self.tabBg = {
		[1] = { bg = "mianban9", bgtag = "btl2", frame = "fangkuai5"},
		[2] = { bg = "mianban7", bgtag = "btl4", frame = "fangkuai4"},
		[3] = { bg = "mianban8", bgtag = "btl3", frame = "fangkuai6"},
	}
end

---初始:注册按钮事件等
function GiftItem:Start(_self, index)
	---@type GiftView
	self.giftView = _self
	self.index = index
	self:AddClick(self.itemCtrl.payBtnGo, Handle(self, self.StartPay), true)
	---@type Hukiry.UI.AtlasImage
	self.backgroud = self.gameObject:GetComponent("AtlasImage")

	---@type table<number, RewardGitItem>
	self.rewardList = {}
end

function GiftItem:StartPay()
	if SingleData.Activity():GetGift():IsEndGift(self.index) then
		return
	end

	if not Single.Http():IsHaveNetwork() then
		UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10002), function()  end)
		return
	end

	if SingleData.Activity():GetGift():IsBuy(self.index) then
		return
	end

	Single.SdkPlatform():GetPayInfo():FetchSDK(self.info.rechargePayId,function()
		self.giftView:OnFinish(self.info)
		Single.Request().SendShop(EHttpShopState.Other, self.info.shopId, self.info.payPrice
		, Handle(self.giftView, self.giftView.OnFinish, self.info))---后面接入sdk
	end)

end


function GiftItem:OnUpdate()
	self.itemCtrl.remianTime.text = SingleData.Activity():GetGift():GetGiftRemaintime(self.index)
end

---@return UnityEngine.Vector3
function GiftItem:GetCenterPos(itemId)
	return self.rewardList[itemId].transform.position
end

---更新数据
---@param info ShopInfo
function GiftItem:OnEnable(info)
	self.info = info
	local text = SingleData.Activity():GetGift():IsEndGift(self.index) and GetLanguageText(11403) or info:GetPayPriceTxt()
	self.itemCtrl.payTxt.text = text
	if SingleData.Activity():GetGift():IsBuy(self.index) then
		self.itemCtrl.payTxt.text = GetLanguageText(15302)
	end
	self.itemCtrl.remianTime.text = SingleData.Activity():GetGift():GetGiftRemaintime(self.index)

	self.itemCtrl.title.text = self.info:GetTitle()
	---奖励部分
	local tab = info:GetItemReward()
	table.insert(tab, {itemId = 9, itemNum = info:GetCoinNum()})
	for i, v in ipairs(tab) do
		local parentGO = self.itemCtrl.left.gameObject
		if v.itemId == 9 then
			parentGO = self.itemCtrl.bgUp.gameObject
		elseif v.itemId>8 then
			parentGO = self.itemCtrl.bgDown.gameObject
		elseif v.itemId>4 then
			parentGO = self.itemCtrl.right.gameObject
		end

		if self.rewardList[v.itemId]  == nil then
			self.rewardList[v.itemId] = UIItemPool.Get(UIItemType.RewardGitItem, parentGO, i)
		end
		self.rewardList[v.itemId]:OnEnable(v.itemId, v.itemNum)
	end

	---改变样式
	local data = self.tabBg[info.sort]
	self.backgroud.spriteName = data.bg
	self.itemCtrl.bg.spriteName = data.bgtag
	self.itemCtrl.bgUp.spriteName = data.frame
	self.itemCtrl.bgDown.spriteName = data.frame
	self.itemCtrl.left.spriteName = data.frame
	self.itemCtrl.right.spriteName = data.frame
end

function GiftItem:PlayRewardEffect(targetTf, lifeTf, _selfTf)
	for i, v in pairs(self.rewardList) do
		v:PlayEffect(targetTf, lifeTf, _selfTf)
	end
end

---隐藏窗口
function GiftItem:OnDisable()
	UIItemPool.PutTable(UIItemType.RewardItem, self.rewardList)
end

---消耗释放资源
function GiftItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return GiftItem