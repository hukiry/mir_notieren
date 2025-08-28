---
---
--- Create Time:2022-5-3 14:00
--- Author: Hukiry
---

---@class GmView:UIWindowBase
local GmView = Class(UIWindowBase)

function GmView:ctor()
	---@type GmControl
	self.ViewCtrl = nil
end

---初始属性字段
function GmView:Awake()
	self.prefabName = "Gm"
	self.panelLayer = ViewLayer.SceneFixed
end

---初始界面:注册按钮事件等
function GmView:Start()
	self:AddClick(self.ViewCtrl.startBtnGo, Handle(self, self.SendMsg))
	self:AddClick(self.ViewCtrl.winBtnGo, function()
		UIManager:OpenWindow(ViewID.GmWin)
	end)
	self.ViewCtrl.inputField.text = 1

end

function GmView:SendMsg()
	local txt,txtx, txty = self.ViewCtrl.inputField.text, self.ViewCtrl.inputFieldX.text, self.ViewCtrl.inputFieldY.text
	if txt == nil or txt == '' then
		print("您输入有误","red")
		return
	end

	local props, x, y = tonumber(txt),tonumber(txtx),tonumber(txty)
	if props ~= EPropsType.None then
		---创建物品，发送数据
		EventDispatch:Broadcast(UIEvent.Match_Create_GM, props, x, y, false)
	else
		EventDispatch:Broadcast(UIEvent.Match_Create_GM, props, x, y, false)
	end
end

---事件派发
function GmView:OnDispatch(x, y)
	self.ViewCtrl.inputFieldX.text, self.ViewCtrl.inputFieldY.text = x, y
end

---显示窗口:初次打开
function GmView:OnEnable(...)
	
end


---隐藏窗口
function GmView:OnDisable()
	
end

---消耗释放资源
function GmView:OnDestroy()
	
end

return GmView