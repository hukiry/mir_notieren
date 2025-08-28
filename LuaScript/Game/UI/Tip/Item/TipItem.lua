---
---
--- Create Time:2023-10-16
--- Author: Hukiry
---

---@class TipItem:IUIItem
local TipItem = Class(IUIItem)

function TipItem:ctor()
	---@type TipItemControl
	self.itemCtrl = nil

	self.waitDuration = 0.9
	self.showFade = 0.2
	self.hiddenFade = 0.6

	self.moveDuration = self.waitDuration + self.hiddenFade
end

---初始:注册按钮事件等
function TipItem:Start(tip)
	self.itemCtrl.tip.text = tip
	---@type UnityEngine.CanvasGroup
	self.canvsGroup = self.gameObject:GetComponent("CanvasGroup")
end

---更新数据
function TipItem:OnEnable(isCancelAnimaton)
	self.transform:DOKill()
	self.canvsGroup:DOFade(1, self.showFade)

	if not isCancelAnimaton then
		self.transform.localPosition = Vector3.New(0,-200,0)
		self.transform:DOLocalMoveY(200, self.moveDuration)
	end

	self.itemCtrl.backMoveGo:SetActive(not isCancelAnimaton)
	self.itemCtrl.backGo:SetActive(isCancelAnimaton)
	self.transform:DOScale(1, self.waitDuration):OnComplete(Handle(self, self.PlayFade))
end

function TipItem:PlayFade()
	self.canvsGroup:DOFade(0, self.hiddenFade):OnComplete(function()
		UIItemPool.Put(UIItemType.TipItem, self)
	end)
end

---隐藏窗口
function TipItem:OnDisable()
	self.transform:DOKill()
	self.canvsGroup.alpha = 0
end

---消耗释放资源
function TipItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return TipItem