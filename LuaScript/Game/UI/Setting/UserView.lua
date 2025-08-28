---
---
--- Create Time:2023-04-10
--- Author: Hukiry
---

---@class UserView:UIWindowBase
local UserView = Class(UIWindowBase)

function UserView:ctor()
	---@type UserControl
	self.ViewCtrl = nil
end

---初始属性字段
function UserView:Awake()
	self.prefabName = "User"
	self.prefabDirName = "Setting"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	self.panelLayer = ViewLayer.Tips
end

---初始界面:注册按钮事件等
function UserView:Start()

	self:AddClick(self.ViewCtrl.buttonGo, function()
		if self.callBack then
			self.callBack()
		end
		self:Close()
	end)

	self:AddClick(self.ViewCtrl.content1Go, function()
		---用户条款
		Application.OpenURL("https://hukiry.github.io/calf/terms.html")
	end)

	self:AddClick(self.ViewCtrl.content2Go, function()
		---隐私协议
		Application.OpenURL("https://hukiry.github.io/calf/index.html")
	end)
end

---显示窗口:初次打开
---@param callBack function
function UserView:OnEnable(callBack)
	self.callBack = callBack
end

---隐藏窗口
function UserView:OnDisable()
	
end

---消耗释放资源
function UserView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return UserView