---
--- RankingView       
--- Author : hukiry     
--- DateTime 2023/6/29 13:58   
---

---@class RankingView:IPageView
local RankingView = Class(UIWindowBase)

function RankingView:ctor(gameObject)
    self.index = 5
    self.lanNameTab = {
        14001,
        14002,
        14003
    }
end

---初始界面:注册按钮事件等
function RankingView:Start()
    self.contentGo = self:FindGameObject("ScrollView/Viewport/Content")
    self.pageTF = self:FindTransform("pageList")
    self.noNetGo = self:FindGameObject("ScrollView/noNet")
    self.noNeworkGo = self:FindGameObject("ScrollView/noNetwork")
    ---@type table<number, SelectItem>
    self.pageList = {}
    for i, v in ipairs(self.lanNameTab) do
        local go = self.pageTF:GetChild(i-1).gameObject
        self.pageList[i] = require("Game.UI.Main.Item.SelectItem").New(go, Handle(self, self.OnSelectPage, i))
        self.pageList[i]:OnEnable(i, v)
    end

    self.page = 1
end

---选择页签
---@param index number
function RankingView:OnSelectPage(index, isReqHand)
    if self.page~=index or isReqHand then
        self:OnDisable()
        for i, v in pairs(self.pageList) do
            v:SelectPage(i==index)
        end

        self.page = index
        if Single.Http():IsHaveNetwork() and not SingleData.Rank():IsHasType(self.page) then
            self.noNeworkGo.gameObject:SetActive(true)
            Single.Request().SendCommonRank(index, Handle(self, self.LoadFinish))
        else
            self:LoadFinish()
        end
    end
end

function RankingView:OnEnable()
    self.noNetGo:SetActive(not Single.Http():IsHaveNetwork())
    self:OnSelectPage(EHttpRankType.Month, Single.Http():IsHaveNetwork())
end

---加载排序的视图
function RankingView:LoadFinish()
    self.noNetGo:SetActive(not Single.Http():IsHaveNetwork())
    self.noNeworkGo.gameObject:SetActive(Single.Http():IsHaveNetwork() and not SingleData.Rank():IsHasType(self.page))
    local tab = SingleData.Rank():GetMassArray(self.page)
    if tab then
        if self.loopView == nil then
            ---@type UILoopItemView
            self.loopView = UILoopItemView.New(self.contentGo, UIItemType.RankItem)
        end
        self.loopView:UpdateList(tab, true, false)
    end

    if Single.Http():IsHaveNetwork() and not SingleData.Rank():IsHasType(self.page) then
       self.coroutineReq =  StartCoroutine(function()
           WaitForSeconds(20)
           self:OnSelectPage(self.page, Single.Http():IsHaveNetwork())
       end)
    end
end

function RankingView:ChangeLanguage()
    for i, v in pairs(self.pageList) do
        v:ChangeLanguage()
    end
end

---隐藏窗口
function RankingView:OnDisable()
    if self.loopView then
        self.loopView:OnDisable()
    end

    if self.coroutineReq then
        StopCoroutine(self.coroutineReq)
        self.coroutineReq = nil
    end
end

return RankingView