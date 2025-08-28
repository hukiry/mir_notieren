---
---
--- Create Time:2023-07-29
--- Author: Hukiry
---

---@class LevelChallengeView:UIWindowBase
local LevelChallengeView = Class(UIWindowBase)

function LevelChallengeView:ctor()
	---@type LevelChallengeControl
	self.ViewCtrl = nil
end

---初始属性字段
function LevelChallengeView:Awake()
	self.prefabName = "LevelChallenge"
	self.prefabDirName = "LevelTip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0

	self.propsTab = {
		EMoneyType.rocket,
		EMoneyType.bomb,
		EMoneyType.rainbowBall
	}
end

---初始界面:注册按钮事件等
function LevelChallengeView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self._Close))
	self:AddClick(self.ViewCtrl.startBtnGo, Handle(self, self.PlayLevel))
	---@type table<number, PropsItem>
	self.itemList = {}
	---@type EPropsType
	self.propsSelect = {}

	---@type table<number, UnityEngine.GameObject>
	self.selectItems = {}
	local childCount = self.ViewCtrl.contentTF.childCount
	for i = 1, childCount do
		local  tf = self.ViewCtrl.contentTF:GetChild(i-1)
		self.selectItems[i] = tf:GetChild(0).gameObject
		self.selectItems[i]:SetActive(false)
	end

end

function LevelChallengeView:_Close()
	self:Close()
	if self.isTryPlay then
		SceneApplication.ChangeState(ViewScene)
	end
end

---开始游戏
function LevelChallengeView:PlayLevel()
	local array = table.toArrayKey(self.propsSelect)
	Single.Match():AddSelectProp(array)
	self.propsSelect = {}

	---扣除道具的使用
	for i, v in ipairs(array) do
		local ty = v + 4
		Single.Player():SetMoneyNum(ty, 1, true)
	end
	Single.Player():SaveRoleData()

	SceneApplication.ChangeState(LevelScene)
end

---显示窗口:初次打开
function LevelChallengeView:OnEnable(isTryPlay)
	self.isTryPlay = isTryPlay or false
	local rocket = Single.Player():GetMoneyNum(EMoneyType.rocket)
	if rocket <= 0 then
		table.remove(self.propsTab, 1)
		table.insert(self.propsTab, 1, EMoneyType.dragonfly)
	end

	self.ViewCtrl.playTxt.text = self.isTryPlay and GetLanguageText(12004) or GetLanguageText(12001)
	self.ViewCtrl.failAgainGo:SetActive(self.isTryPlay)

	self.ViewCtrl.title.text = GetLanguageText(12002) .. " "..Single.Player():GetMoneyNum(EMoneyType.level)
	for i, v in ipairs(self.propsTab) do
		self.itemList[i] = UIItemPool.Get(UIItemType.PropsItem, self.ViewCtrl.selectHorizontalGo, Handle(self, self.OnSelect, i))
		self.itemList[i]:OnEnable(v)
	end
end

function LevelChallengeView:OnSelect(index)
	if self.itemList[index] then
		local ty, isSelect = self.itemList[index]:OnSelect()
		if ty then
			self.propsSelect[ty] = isSelect and 1 or nil
		end
	end

	local array = table.toArrayKey(self.propsSelect)
	for i = 1, 3 do
		self.selectItems[i]:SetActive(#array>=i)
	end
end

---隐藏窗口
function LevelChallengeView:OnDisable()
	UIItemPool.PutTable(UIItemType.PropsItem, self.itemList)
end

---消耗释放资源
function LevelChallengeView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return LevelChallengeView