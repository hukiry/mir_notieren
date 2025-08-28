---
---
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MyMapExpandView:UIWindowBase
local MyMapExpandView = Class(UIWindowBase)

function MyMapExpandView:ctor()
	---@type MyMapExpandControl
	self.ViewCtrl = nil
end

---初始属性字段
function MyMapExpandView:Awake()
	self.prefabName = "MyMapExpand"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MyMapExpandView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.buyBtnGo, Handle(self, self._OnBuy))
end

function MyMapExpandView:_OnBuy()
	Single.SdkPlatform():GetPayInfo():FetchSDK(self.info.rechargePayId,function()
		Single.Request().SendShop(EHttpShopState.Other, self.info.shopId, self.info.payPrice, Handle(self, self.OnFinish))---后面接入sdk
	end)
end

---完成扩容
function MyMapExpandView:OnFinish()
	Single.Player():SetMoneyNum(EMoneyType.metaExpendNum, 1)
	self.callBack()
	Single.Player():SaveRoleData()
	self:Close()
end

---显示窗口:初次打开
function MyMapExpandView:OnEnable(callBack)
	self.callBack = callBack
	---@type ShopInfo
	self.info = SingleData.Shop():GetShopInfo(2001)
	self.ViewCtrl.payTxt.text = self.info:GetPayPriceTxt()
end

---隐藏窗口
function MyMapExpandView:OnDisable()
	
end

---消耗释放资源
function MyMapExpandView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MyMapExpandView