---
---
--- Create Time:2023-03-22
--- Author: Hukiry
---

---@class CommonTipView:UIWindowBase
local CommonTipView = Class(UIWindowBase)

function CommonTipView:ctor()
	---@type CommonTipControl
	self.ViewCtrl = nil
	self.okActionCall = nil
	self.cancelAactionCall = nil
end

---初始属性字段
function CommonTipView:Awake()
	self.prefabName = "CommonTip"
	self.prefabDirName = "PromptBox"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function CommonTipView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))

	self:AddClick(self.ViewCtrl.okBtnGo,function()
		self:Close()
		if self.okActionCall~=nil then
			self.okActionCall()
			self.okActionCall=nil
		end
	end)

	self:AddClick(self.ViewCtrl.cancelBtnGo,function()
		self:Close()
		if self.cancelAactionCall~=nil then
			self.cancelAactionCall()
			self.cancelAactionCall = nil
		end
	end)
end


---显示窗口:初次打开
---@param content string
---@param okActionCall function
---@param cancelAactionCall function
function CommonTipView:OnEnable(content, okActionCall, cancelAactionCall, ishandle, title)

	if ishandle then
		cancelAactionCall = nil
		self.ViewCtrl.closeBtnGo:SetActive(false)
	else
		self.ViewCtrl.closeBtnGo:SetActive(cancelAactionCall==nil)
	end

	self.ViewCtrl.okBtnGo:SetActive(okActionCall~=nil)
	self.ViewCtrl.cancelBtnGo:SetActive(cancelAactionCall~=nil)
	self.ViewCtrl.content.text = content
	self.okActionCall = okActionCall
	self.cancelAactionCall = cancelAactionCall

	if title then
		self.ViewCtrl.title.text = title
	else
		self.ViewCtrl.title.text = GetLanguageText(10009)
	end

end

---隐藏窗口
function CommonTipView:OnDisable()
end

---消耗释放资源
function CommonTipView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return CommonTipView