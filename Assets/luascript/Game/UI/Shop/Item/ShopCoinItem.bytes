---
---
--- Create Time:2023-07-03
--- Author: Hukiry
---

---@class ShopCoinItem:IUIItem
local ShopCoinItem = Class(IUIItem)

function ShopCoinItem:ctor()
	---@type ShopCoinItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function ShopCoinItem:Start()
	self:AddClick(self.itemCtrl.payBtnGo, Handle(self, self.StartPay), true)
end

function ShopCoinItem:StartPay()
	if not Single.Http():IsHaveNetwork() then
		UIManager:OpenWindow(ViewID.CommonTip, GetLanguageText(10002), function()  end)
		return
	end

	Single.SdkPlatform():GetPayInfo():FetchSDK(self.info.rechargePayId,function()
		Single.Request().SendShop(EHttpShopState.Pay, self.info.shopId, self.info.payPrice)---后面接入sdk
	end)
end

---更新数据
---@param info ShopInfo
function ShopCoinItem:OnEnable(info)
	self.info = info
	self.itemCtrl.payTxt.text = info:GetPayPriceTxt() ---暂时先用美元
	self.itemCtrl.num.text = info:GetCoinShowText()
end

---隐藏窗口
function ShopCoinItem:OnDisable()
	
end

---消耗释放资源
function ShopCoinItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return ShopCoinItem