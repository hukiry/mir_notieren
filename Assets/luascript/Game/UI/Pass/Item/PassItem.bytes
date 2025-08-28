---
---
--- Create Time:2023-03-17
--- Author: Hukiry
---

---@class PassItem:IUIItem
local PassItem = Class(IUIItem)

function PassItem:ctor()
	---@type PassItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function PassItem:Start()
	--self:AddClick(self.itemCtrl.passGo, Handle(self, self.ShowTip, false))
	self:AddClick(self.itemCtrl.freeGo, function()
		if self.info:IsCanGetFree() then
			SingleData.Activity():GetPass():SendState(2, nil, Handle(self, self.OnFreeReward), self.info.id)
		else
			self:ShowTip(true)
		end
	end)

	---@type UnityEngine.CanvasGroup
	self.tipCanvas = self.itemCtrl.tipsGo:GetComponent("CanvasGroup")
end

---显示物品提示
function PassItem:ShowTip(isFree)
	---@type UnityEngine.UI.ContentSizeFitter
	local sizeFitter =  self.itemCtrl.tipsGo:GetComponent("ContentSizeFitter")
	sizeFitter.enabled = false
	self.itemCtrl.tipsGo:SetActive(true)
	local ids, ty = self.info:GetPayReward(), 5
	if isFree then
		self.itemCtrl.tipsGo.transform.position = self.itemCtrl.iconFree.transform.position
		ids, ty = self.info:GetFreeReward()
	else
		self.itemCtrl.tipsGo.transform.position = self.itemCtrl.iconPass.transform.position
	end

	local childCount = self.itemCtrl.tipsGo.transform.childCount
	for i = 1, childCount do
		local tf = self.itemCtrl.tipsGo.transform:GetChild(i-1)
		local img = tf:GetComponent("AtlasImage")
		tf.gameObject:SetActive(ids[i]~=nil)
		if ids[i] then
			---@type Hukiry.HukirySupperText
			local txt = tf:Find("num"):GetComponent("HukirySupperText")
			txt.text = 'x'..ids[i][2]
			local spriteName = SingleConfig.Currency():GetKey(ids[i][1]).icon
			SetUIIcon(img, spriteName, Vector2.New(100,100))
		end
	end

	self.tipCanvas:DOKill()
	self.tipCanvas.alpha = 1
	self.itemCtrl.tipsGo.transform:DOKill()
	self.itemCtrl.tipsGo.transform:DOScale(Vector3.one,2):OnComplete(function()
		self.tipCanvas:DOFade(0,1)
	end)

	StartCoroutine(function()
		WaitForFixedUpdate()
		sizeFitter.enabled = true
	end)
end
---免费奖励领取
function PassItem:OnFreeReward()
	self.itemCtrl.get_effectGo:SetActive(false)
	local ids, _ =  self.info:GetFreeReward()

	for i, vTab in ipairs(ids) do
		local ty, num = vTab[1], vTab[2]
		SingleData.Activity():PlayReward(EActivityType.pass, ty, num, HandleParams(function(ty1, number)
			Single.Player():SetMoneyNum(ty1, number)
		end, ty))
	end

	UIManager:CloseWindow(ViewID.Pass)
end

---更新数据
---@param info PassInfo
function PassItem:OnEnable(info)
	self.info = info
	local day = SingleData.Activity():GetPass():GetPassIndex()
	self.itemCtrl.maskDownGo:SetActive(self.info.day==day)
	self.itemCtrl.maskUpGo:SetActive(self.info.day==day)
	self.itemCtrl.gouFreeGo:SetActive(self.info:IsGot())
	self.itemCtrl.gouPassGo:SetActive(self.info:IsPayed())
	self.itemCtrl.lockGo:SetActive(self.info.day==day)
	self.itemCtrl.starIconGo:SetActive(self.info.day<=day)
	self.itemCtrl.day.gameObject:SetActive(self.info.day>day)
	self.itemCtrl.day.text = self.info.day
	self.itemCtrl.lockPassGo:SetActive(not self.info:IsPayed())

	local isNext = self.info.day-1 == day
	self.itemCtrl.linehorizontal1Go:SetActive(not isNext or self.info.day==1)
	self.itemCtrl.linehorizontal2Go:SetActive(not isNext or self.info.day==1)
	self.itemCtrl.lineDownGo:SetActive(self.info.day==day)
	self.itemCtrl.numFree.text = info:GetFreeNumTxt()
	local icon, isok = info:GetFreeIcon()
	local spritePath = isok == true and "Atlas/UI/UIIcon" or ESpriteAtlasResource[icon]
	LoadAtlasImageSpriteAtlas(self.itemCtrl.iconFree, spritePath, icon, function()
		UtilFunction.SetUIAdaptionSize(self.itemCtrl.iconFree, Vector2.New(180,180))
	end, false)
	self.itemCtrl.iconPass.spriteName = "baoxiang"..(self.info.day%3+1)
	self.itemCtrl.get_effectGo:SetActive(self.info:IsCanGetFree())
end

---隐藏窗口
function PassItem:OnDisable()
	
end

---消耗释放资源
function PassItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return PassItem