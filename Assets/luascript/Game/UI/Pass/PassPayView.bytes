---
---
--- Create Time:2023-08-01
--- Author: Hukiry
---

---@class PassPayView:UIWindowBase
local PassPayView = Class(UIWindowBase)

function PassPayView:ctor()
	---@type PassPayControl
	self.ViewCtrl = nil
end

---初始属性字段
function PassPayView:Awake()
	self.prefabName = "PassPay"
	self.prefabDirName = "Pass"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function PassPayView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo ,Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.payBtnGo ,Handle(self, self._OnPay))

	self.itemList = {}
	local childCount = self.ViewCtrl.rewardVerticleTF.childCount
	for i = 1, childCount do
		local tf = self.ViewCtrl.rewardVerticleTF:GetChild(i-1)
		self.itemList[i] = {}
		self.itemList[i].gameObject = tf.gameObject
		self.itemList[i].icon = tf:Find("icon"):GetComponent("AtlasImage")
		self.itemList[i].numTx = tf:Find("num"):GetComponent("HukirySupperText")
	end
end

---显示中重复打开
function PassPayView:_OnPay()
	if self.info:GetPayInfo():IsPayed() then
		return
	end

	Single.SdkPlatform():GetPayInfo():FetchSDK(self.info:GetPayInfo().rechargePayId, Handle(self, self.PaySucc))
end

---事件派发
function PassPayView:PaySucc()
	Single.Request().SendShop(EHttpShopState.Other, self.info:GetPayInfo().shopId, self.info:GetPayInfo().payPrice)
	self.info:PayFinish()
	local ids, _ =  self.info:GetPayInfo():GetPayReward()
	for _, vTab in ipairs(ids) do
		local ty, num = vTab[1], vTab[2]
		SingleData.Activity():PlayReward(EActivityType.pass, ty, num, HandleParams(function(ty1, number)
			Single.Player():SetMoneyNum(ty1, number)
		end, ty))
	end

	if SceneRule.CurSceneType == SceneType.LevelCity then
		Single.Match():AddMoveCount(5)
	end
	UIManager:CloseWindow(ViewID.Pass)
	self:Close()
end

---显示窗口:初次打开
---@param info PassActivity
function PassPayView:OnEnable(info)
	self.info = info
	self.ViewCtrl.remainTime.text = self.info:GetActRemainTime()
	local payRewards = self.info:GetPayInfo():GetPayReward()
	for i, v in pairs(self.itemList) do
		local tab = payRewards[i]
		v.gameObject:SetActive(tab~=nil)
		if tab then
			local spriteName, number = SingleConfig.Currency():GetKey(tab[1]).icon, tab[2]
			v.numTx.text = 'x'..number
			SetUIIcon(v.icon, spriteName, Vector2.New(120,120))
		end
	end

	self.ViewCtrl.move5Go:SetActive(SceneRule.CurSceneType == SceneType.LevelCity)
	self.ViewCtrl.playTxt.text = self.info:GetPayInfo():GetPayShowtext()
end

---隐藏窗口
function PassPayView:OnDisable()

end

---消耗释放资源
function PassPayView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return PassPayView