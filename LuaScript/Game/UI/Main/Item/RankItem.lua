---
---
--- Create Time:2023-07-17
--- Author: Hukiry
---

---@class RankItem:IUIItem
local RankItem = Class(IUIItem)

function RankItem:ctor()
	---@type RankItemControl
	self.itemCtrl = nil
end

---初始:注册按钮事件等
function RankItem:Start()
	self:AddClick(self.gameObject, function()

	end)
end

---更新数据
---@param info RankInfo
function RankItem:OnEnable(info)
	self.info = info
	self.itemCtrl.sortTxt.text = info.number
	self.itemCtrl.lvTxt.text = info.level
	self.itemCtrl.massIcon.spriteName = info:GetIcon()
	StartCoroutine(function()
		WaitForFixedUpdate()
		UtilFunction.SetUIAdaptionSize(self.itemCtrl.massIcon, Vector2.New(125, 125))
	end)

	self.itemCtrl.massName.text = info.massName
	self.itemCtrl.roleName.text = info.roleNick
end

---隐藏窗口
function RankItem:OnDisable()
	
end

---消耗释放资源
function RankItem:OnDestroy()
	self.itemCtrl:OnDestroy()
end

return RankItem