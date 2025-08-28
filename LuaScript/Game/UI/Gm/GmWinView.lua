---
---
--- Create Time:2022-5-5 16:00
--- Author: Hukiry
---

---@class GmWinView:UIWindowBase
local GmWinView = Class(UIWindowBase)

function GmWinView:ctor()
	---@type GmWinControl
	self.ViewCtrl = nil
	self.btnList = {}
end

---初始属性字段
function GmWinView:Awake()
	self.prefabName = "GmWin"
	self.prefabDirName = "Gm"
	self.panelLayer = ViewLayer.Animation
	---配置信息
	self.configTab = {
		{
			btnName = "加10个帽子", callFunc = function()
				Single.Player():SetMoneyNum(EMoneyType.cap, 10)
				EventDispatch:Broadcast(ViewID.LevelMain, 4, EPropsView.DiceRandom)
			end
		},
		{
			btnName = "加10个垂子", callFunc = function()
				Single.Player():SetMoneyNum(EMoneyType.hammer, 10)
				EventDispatch:Broadcast(ViewID.LevelMain, 4, EPropsView.Wipe)
			end
		},
		{
			btnName = "加10个大炮", callFunc = function()
				Single.Player():SetMoneyNum(EMoneyType.gun, 10)
				EventDispatch:Broadcast(ViewID.LevelMain, 4, EPropsView.Cell)
			end
		},
		{
			btnName = "加10个弓箭", callFunc = function()
				Single.Player():SetMoneyNum(EMoneyType.bows, 10)
				EventDispatch:Broadcast(ViewID.LevelMain, 4, EPropsView.Row)
			end
		},
		{
			btnName = "加10个通用", callFunc = function()
				for i = 5, 8 do
					Single.Player():SetMoneyNum(i, 10)
				end
			end
		},

		{
			btnName = "清空货币", callFunc = function()

			Single.Player():SetMoneyNum(EMoneyType.gold, 100000, true)

		end
		},
		{
			btnName = "加10关", callFunc = function()
			Single.Player():SetMoneyNum(EMoneyType.level, 10)

		end
		},
	}
end

---初始界面:注册按钮事件等
function GmWinView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, function()
		self:Close()
	end)
end

---显示窗口:初次打开
function GmWinView:OnEnable(...)
	for i, v in ipairs(self.configTab) do
		if self.btnList[i] == nil then
			local go = self:CreateItem(v.btnName)
			self:AddButtonClick(go, Handle(self, self.CallGm, i))
			self.btnList[i] = go
		end
	end
end

function GmWinView:CallGm(index)
	if self.configTab[index] then
		if self.configTab[index].callFunc then
			self.configTab[index].callFunc()
		end
	end
end

---@return UnityEngine.GameObject
function GmWinView:CreateItem(btnName)
	self.ViewCtrl.templateBtnGo:SetActive(false)
	---@type UnityEngine.GameObject
	local  go = GameObject.Instantiate(self.ViewCtrl.templateBtnGo)
	go.transform:SetParent(self.ViewCtrl.contentTF, false)
	---@type UnityEngine.UI.Text
	local  uitext = go:GetComponentInChildren(typeof(UnityEngine.UI.Text))
	uitext.text = btnName
	go:SetActive(true)
	return go
end

---隐藏窗口
function GmWinView:OnDisable()
	
end

---消耗释放资源
function GmWinView:OnDestroy()
	
end

return GmWinView