---
--- HomeView
--- Created by Administrator.
--- DateTime: 2023/6/29 13:55
---

---@class HomeView:IPageView
local HomeView = Class(UIWindowBase)

---@class EHome_Page_Data
EHome_Page_Data = {
    ---@type UnityEngine.GameObject
    gameObject = 1,
    ---@type Hukiry.UI.AtlasImage
    icon = 2,
    ---@type Hukiry.HukirySupperText
    timeTxt = 3,
}

function HomeView:ctor(gameObject)
    self.index = 3

    ---数据类型，视图界面
    ---@type table<EActivityType, ViewID>
    self.buttonViewTab = {
        [EActivityType.chest] = ViewID.Chest,
        [EActivityType.drawcard] = ViewID.Drawcard,
        [EActivityType.integral] = 0,
        [EActivityType.item] = ViewID.Props,
        [EActivityType.sign] = ViewID.SignMonth,
        [EActivityType.festival] = 0,
        [EActivityType.gift] = ViewID.Gift,
        [EActivityType.pass] = ViewID.Pass,
        [EActivityType.rechargeFirst] = ViewID.RechargeFirst,
    }

    self.buttonIconTab = {
        [EActivityType.chest] = 'baoxiang5',
        [EActivityType.drawcard] = 'chouka',
        [EActivityType.integral] = 'shalou',
        [EActivityType.item] = 'hecheng',
        [EActivityType.sign] = 'rili',
        [EActivityType.festival] = 'xmf',
        [EActivityType.gift] = 'libao',
        [EActivityType.pass] = 'txz',
        [EActivityType.rechargeFirst] = 'shouchong',
    }

    self.buttonLeftData = {
        EActivityType.chest, EActivityType.drawcard, EActivityType.integral,
        EActivityType.item, EActivityType.sign,
    }

    self.buttonRightData = {
        EActivityType.festival, EActivityType.gift,
        EActivityType.pass, EActivityType.rechargeFirst
    }
end

---初始界面:注册按钮事件等
function HomeView:Start()
    self:AddClick(self:FindGameObject("list/lvBtn"), Handle(self, self._OnLevel))
    self.metaBtnGo = self:FindGameObject("list/metaBtn")
    self:AddClick(self.metaBtnGo, Handle(self, self._OnMeta))
    self.levelTx =  self:FindHukirySupperText("list/lvBtn/txt")

    self.taskBarGO = self:FindGameObject("taskBar")

    ---按钮视图：活动类型，视图数据
    ---@type table<EActivityType, EHome_Page_Data>
    self.iconList = {}
    self:InitButtonList(self:FindTransform("leftButton"), true)
    self:InitButtonList(self:FindTransform("rightButton"), false)
end

function HomeView:InitButtonList(transformTf, isLeft)
    local childCount = transformTf.childCount
    for i = 1, childCount do
        local tf = transformTf:GetChild(i-1)
        local actType = isLeft and  self.buttonLeftData[i] or self.buttonRightData[i]
        ---@type Hukiry.UI.AtlasImage
        local img =  tf:Find("Icon"):GetComponent("AtlasImage")
        self.iconList[actType] = {
            gameObject = tf.gameObject,
            icon = img,
            timeTxt = tf:Find("timeBg/time"):GetComponent("HukirySupperText")
        }

        local  spriteName = self.buttonIconTab[actType]
        img.spriteName = spriteName
        UtilFunction.SetUIAdaptionSize(img, Vector2.New(140,140))
        self:AddClick(tf.gameObject, Handle(self, self._OnActivity, actType))
    end
end

---进入关卡
function HomeView:_OnLevel()
    if Single.Player().curLifeTime <= 0 and Single.Player():GetMoneyNum(EMoneyType.life) <= 0 then
        UIManager:OpenWindow(ViewID.Life)
        return
    end
    UIManager:OpenWindow(ViewID.LevelChallenge)
end

---进入元宇宙
function HomeView:_OnMeta()
    UIManager:OpenWindow(ViewID.MetaEnter)
end

---点击活动图标打开
function HomeView:_OnActivity(actType)
    local info = SingleData.Activity():GetActivityInfo(actType)
    local viewId =  self.buttonViewTab[actType]
    if viewId>0 and not info:IsFinish() then
        UIManager:OpenWindow(viewId, info)
    end
end

function HomeView:OnEnable()
    self:ChangeLanguage()
    local data = Single.AutoTask():GetTaskData()
    if data.itemId == 0 then
        return
    end

    self.info = data
    if self.taskBarItem == nil then
        ---@type TaskBarItem
        self.taskBarItem = require("Game.UI.Main.Item.TaskBarItem").New(self.taskBarGO)
        self.taskBarItem:Start()
        self.taskBarItem:OnEnable(self.info)
    end
    self:OnUpdate()
end

function HomeView:OnUpdate()
    if self.taskBarItem then
        self.taskBarItem:OnUpdate()
    end

    for actType, v in pairs(self.iconList) do
        local info = SingleData.Activity():GetActivityInfo(actType)
        if info then
            v.gameObject:SetActive(not info:IsEndActivity())
            v.timeTxt.text = info:GetActRemainTime()
        else
            v.gameObject:SetActive(false)
        end
    end
end

function HomeView:ChangeLanguage()
    self.levelTx.text = GetLanguageText(12002) .." ".. Single.Player():GetMoneyNum(EMoneyType.level)
end

---活动派发
---@param actType EActivityType 控制按钮
function HomeView:OnDispatch(actType)
    if self.iconList[actType] then
        local info = SingleData.Activity():GetActivityInfo(actType)
        if info then
            self.iconList[actType].gameObject:SetActive(not info:IsEndActivity())
            self.iconList[actType].timeTxt.text = info:GetActRemainTime()
        end
    end
end

---获取控件
---@return UnityEngine.Transform
function HomeView:GetControl(actType)
    if actType > 0 then
        return self.iconList[actType].gameObject.transform
    end
    return self.taskBarGO.transform
end

---隐藏窗口
function HomeView:OnDisable()
    if self.taskBarItem then
        self.taskBarItem:OnDisable()
        self.taskBarItem = nil
    end
end

return HomeView