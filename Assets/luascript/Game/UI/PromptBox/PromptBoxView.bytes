---
---
--- Create Time:2021-8-8 15:26
--- Author: Hukiry
---

---@class PromptBoxView:UIWindowBase
local PromptBoxView = Class(UIWindowBase)

function PromptBoxView:ctor()
	---@type PromptBoxControl
	self.ViewCtrl = nil
	self.okAactionCall = nil
	self.cancelAactionCall = nil
end

---初始属性字段
function PromptBoxView:Awake()
	self.prefabName = "PromptBox"
	self.panelLayer = ViewLayer.Tips
end

---初始界面:注册按钮事件等
function PromptBoxView:Start()
	self:AddClick(self.ViewCtrl.okGo,function()
		self:Close()
		if self.okAactionCall~=nil then
			self.okAactionCall()
		end
	end, true)

	self:AddClick(self.ViewCtrl.cancelGo,function()
		self:Close()
		if self.cancelAactionCall~=nil then
			self.cancelAactionCall()
		end
	end, true)
end

---显示窗口:初次打开
function PromptBoxView:OnEnable(content, okAactionCall, cancelAactionCall)
	UIManager:CloseWindow(ViewID.ServerTip)

	self.ViewCtrl.okGo:SetActive(okAactionCall~=nil)
	self.ViewCtrl.cancelGo:SetActive(cancelAactionCall~=nil)
	self.ViewCtrl.content.text = content
	self.okAactionCall = okAactionCall
	self.cancelAactionCall = cancelAactionCall

	self.ViewCtrl.txtOk.text = GetLanguageText(10010)
	self.ViewCtrl.txtCancel.text = GetLanguageText(10020)
end

---隐藏窗口
function PromptBoxView:OnDisable()
	
end

---消耗释放资源
function PromptBoxView:OnDestroy()
	
end

return PromptBoxView