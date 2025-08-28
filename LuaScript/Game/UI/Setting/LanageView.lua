---
---
--- Create Time:2022-7-3 12:00
--- Author: Hukiry
---

---@class LanageView:UIWindowBase
local LanageView = Class(UIWindowBase)

function LanageView:ctor()
	---@type LanageControl
	self.ViewCtrl = nil
end

---初始属性字段
function LanageView:Awake()
	self.prefabName = "Lanage"
	self.prefabDirName = "Setting"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	---@type table<number, LanButtonItem>
	self.itemList = {}
end

---初始界面:注册按钮事件等
function LanageView:Start()
	--self:AddClick(self.ViewCtrl.maskGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))
	self:AddClick(self.ViewCtrl.okBtnGo, function()
		if self.info and self.info.code ~= Single.SdkPlatform():GetLanguageCode() then
			Single.SdkPlatform():SetLanguage(self.info.code)
			EventDispatch:Broadcast(ViewID.Setting)
		end
		self:Close()
	end)

	---@type UnityEngine.UI.ContentSizeFitter
	self.contentFitter = self.ViewCtrl.contentGo:GetComponent("ContentSizeFitter")
	self.visibleWidth = self.ViewCtrl.contentGo.transform.parent.rect.height
end

---@param info TableMultiLanguageItem
function LanageView:OnClick(info)
	self.info = info
	local index = 1
	for i, v in pairs(self.itemList) do
		local col = i==info.lanId and  Color.New(0.03,0.28,.08) or Color.New(0.5,0.23,0)
		local spriteName = i==info.lanId and "anniu3" or "anniu2"
		v:Select(spriteName, col)
		if i == info.lanId then
			index = v.numberId
		end
	end

	StartCoroutine(function()
		WaitForFixedUpdate()
		WaitForFixedUpdate()
		GotoScrollViewIndex(index, self.ViewCtrl.scrollView, self.ViewCtrl.contentGo.transform, false)
	end)

end

---显示窗口:初次打开
function LanageView:OnEnable(lanId)
	self.contentFitter.enabled = false
	local k = 0
	for i = 1, 20 do
		local info = SingleConfig.MultiLanguage():GetKey(i, false)
		if info then
			k = k + 1
			if self.itemList[i] == nil then
				self.itemList[i] = UIItemPool.Get(UIItemType.LanButtonItem, self.ViewCtrl.contentGo, Handle(self, self.OnClick))
			end
			self.itemList[i]:OnEnable(info, k)
		end
	end

	StartCoroutine(function()
		WaitForFixedUpdate()
		self.contentFitter.enabled = true
	end)

	local info = SingleConfig.MultiLanguage():GetKey(lanId)
	self:OnClick(info)
end

---隐藏窗口
function LanageView:OnDisable()
	UIItemPool.PutTable(UIItemType.LanButtonItem, self.itemList)
end

---消耗释放资源
function LanageView:OnDestroy()
	
end

return LanageView