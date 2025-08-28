---
---
--- Create Time:2022-7-2 17:00
--- Author: Hukiry
---

---@class ShopItem:DisplayObjectBase
local ShopItem = Class(DisplayObjectBase)

function ShopItem:ctor()
	---@type ShopItemControl
	self.itemCtrl = nil
	self.index = -1

	self.tabBg = {
		[1] = { bg = "mianban7", bgtag = "btl4", frame = "fangkuai4"},
		[2] = { bg = "mianban8", bgtag = "btl3", frame = "fangkuai6"},
		[3] = { bg = "mianban10", bgtag = "btl6", frame = "fangkuai8"},
		[4] = { bg = "mianban9", bgtag = "btl2", frame = "fangkuai5"},

		[5] = { bg = "mianban8", bgtag = "btl3", frame = "fangkuai6"},
		[6] = { bg = "mianban10", bgtag = "btl6", frame = "fangkuai8"}
	}
end

---初始:注册按钮事件等
function ShopItem:Start()
	self:AddClick(self.itemCtrl.payBtnGo, Handle(self, self.StartPay), true)
	---@type table<number, RewardItem>
	self.rewardList = {}

	---@type Hukiry.UI.AtlasImage
	self.backgroud = self.gameObject:GetComponent("AtlasImage")
end

function ShopItem:StartPay()
	Single.SdkPlatform():GetPayInfo():FetchSDK(self.info.rechargePayId, function()
		Single.Request().SendShop(EHttpShopState.Pay, self.info.shopId, self.info.payPrice)---后面接入sdk
	end)
end

---@return UnityEngine.Vector3
function ShopItem:GetCenterPos()
	return self.itemCtrl.icon.transform.position
end

---更新数据
---@param info ShopInfo
function ShopItem:OnEnable(info)
	self.info = info
	self.itemCtrl.title.text = info:GetTitle()
	self.itemCtrl.payTxt.text = info:GetPayPriceTxt() ---暂时先用美元
	self.itemCtrl.coinNum.text = info:GetCoinShowText()
	local tagStr =  info:GetTag()
	self.itemCtrl.tagbgGo:SetActive(tagStr~=nil)
	if tagStr then
		self.itemCtrl.tagTxt.text = tagStr
	end

	if string.len(info:GetCfg().icon)>0 then
		SetUIIcon(self.itemCtrl.icon, info:GetCfg().icon, Vector2.New(260,260))
	end

	local tab = info:GetItemReward()
	for i, v in ipairs(tab) do
		local parentGO = i>=5 and self.itemCtrl.bgDown.gameObject or self.itemCtrl.bgUp.gameObject
		if self.rewardList[i]  == nil then
			self.rewardList[i] = UIItemPool.Get(UIItemType.RewardItem, parentGO, i)
		end
		self.rewardList[i]:OnEnable(v.itemId, v.itemNum)
	end

	---改变样式
	local data = self.tabBg[info.sort]
	self.backgroud.spriteName = data.bg
	self.itemCtrl.bg.spriteName = data.bgtag
	self.itemCtrl.bgUp.spriteName = data.frame
	self.itemCtrl.bgDown.spriteName = data.frame
end

function ShopItem:PlayRewardEffect(targetTf, lifeTf, parentGo)
	for i, v in pairs(self.rewardList) do
		v:PlayEffect(targetTf, lifeTf, parentGo)
	end
end

---隐藏窗口
function ShopItem:OnDisable()
	UIItemPool.PutTable(UIItemType.RewardItem, self.rewardList)
	self.rewardList = {}

	if self.effect then
		self.effect:OnDisable()
		self.effect = nil
	end
end

return ShopItem