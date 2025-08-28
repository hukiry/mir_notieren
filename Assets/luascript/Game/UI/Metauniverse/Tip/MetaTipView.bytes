---
---
--- Create Time:2023-11-29
--- Author: Hukiry
---

---@class MetaTipView:UIWindowBase
local MetaTipView = Class(UIWindowBase)

function MetaTipView:ctor()
	---@type MetaTipControl
	self.ViewCtrl = nil
end

---初始属性字段
function MetaTipView:Awake()
	self.prefabName = "MetaTip"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MetaTipView:Start()
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
function MetaTipView:OnEnable(content, okActionCall, cancelAactionCall, ishandle, title)

	if ishandle then
		cancelAactionCall = nil
		self.ViewCtrl.closeBtnGo:SetActive(false)
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
function MetaTipView:OnDisable()
	
end

---消耗释放资源
function MetaTipView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MetaTipView