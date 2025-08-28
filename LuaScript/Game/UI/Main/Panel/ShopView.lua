---
--- ShopView       
--- Author : hukiry     
--- DateTime 2023/6/29 13:51   
---


---@class ShopView:IPageView
local ShopView = Class(UIWindowBase)

function ShopView:ctor(gameObject)
    self.index = 1
end

---初始界面:注册按钮事件等
function ShopView:Start()
    ---@type RechargeView
    self.rechargeView = require('Game.UI.Shop.RechargeView').New(self.gameObject)
    self.rechargeView:Start()
    self.rechargeView.targetGo = UIManager:GetActiveWindow(ViewID.Game).gameObject
end

function ShopView:OnEnable()
    self.rechargeView:OnEnable()
end

---充值成功后派发
---@param shopId number
function ShopView:OnDispatch(shopId)
    self.rechargeView:OnDispatch(shopId)
end

---隐藏窗口
function ShopView:OnDisable()
    self.rechargeView:OnDisable()
end

return ShopView