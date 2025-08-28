--- 好友
--- FriendView       
--- Author : hukiry     
--- DateTime 2023/6/29 13:57   
---

---@class FriendView:IPageView
local FriendView = Class(UIWindowBase)

function FriendView:ctor(gameObject)
    ---10019=队
    self.lanNameTab = {
        14005,
        14004,
        14006
        --10014
    }

    self.reqestlifeTime = UnityEngine.Time.realtimeSinceStartup
end

---初始界面:注册按钮事件等
function FriendView:Start()

    self.scrollView = self:FindScrollRect("ScrollView")
    self.contentGo = self:FindGameObject("ScrollView/Viewport/Content")
    self.pageTF = self:FindTransform("pageList")
    ---网络提示
    self.noNetGo = self:FindGameObject("ScrollView/noNet")
    ---网络请求中
    self.noNeworkGo = self:FindGameObject("ScrollView/noNetwork")

    ---@type UnityEngine.UI.ContentSizeFitter
    self.contentFitter = self.contentGo:GetComponent("ContentSizeFitter")
    ---查找
    self.searchGo = self:FindGameObject("ScrollView/Viewport/Content/search")
    self:AddClick(self.searchGo.transform:FindGameObject("searchBtn"), Handle(self, self.OnSearch))
    self.searchInput = self.searchGo.transform:FindInputField("InputField")
    self.searchInput.onValueChanged:AddListener(Handle(self, self._SearchInput))
    self.searchId = 0

    ---@type table<number, SelectItem>
    self.pageList = {}
    for i, v in ipairs(self.lanNameTab) do
        local go = self.pageTF:GetChild(i-1).gameObject
        self.pageList[i] = require("Game.UI.Main.Item.SelectItem").New(go, Handle(self, self.OnSelectPage, i))
        self.pageList[i]:OnEnable(i, v)
    end

    self.pageIndex = 0
    ---视图列表
    ---@type table<number, FriendItem>
    self.itemList = {}

    ---聊天部分
    self.goChat = self:FindGameObject("ChatPanel")
    ---@type FriendChatPanel
    self.friendChatPanel = require("Game.UI.Friend.FriendChatPanel").New(self.goChat)
    self.friendChatPanel:Start()
    self.friendChatPanel:OnDisable()
    ---社团部分
    --self.goMass = self:FindGameObject("MassPanel")
    -----@type FriendMassPanel
    --self.massPanel = require("Game.UI.Friend.FriendMassPanel").New(self.goMass)
    --self.massPanel:Start()
end

function FriendView:ChangeChatPanel(info)
    self.friendChatPanel:OnEnable(self.scrollView.gameObject, info)
end

function FriendView:OnSearch()
    if string.Trim(self.searchInput.text) == '' then
        --输入不可为空
        TipMessageBox.ShowUI(GetLanguageText(13106), true)
        return
    end

    if self.searchId == Single.Player().roleId then
        --不能查询自己
        TipMessageBox.ShowUI(GetLanguageText(14021), true)
        return
    end

    if SingleData.Friend():IsLimitFriend() then
        TipMessageBox.ShowUI(GetLanguageText(14020), true)
        return
    end

    Single.Request().SendFriend(EFriendState.Search, self.searchId, function(succ)
        if not succ then
            TipMessageBox.ShowUI(GetLanguageText(14019), true)
        end
    end)
end

---检查输入
function FriendView:_SearchInput(v)
    if self.searchId ~= tonumber(v) then
        self.searchId = tonumber(v)
        if string.Trim(v) == '' then
            local isOK = SingleData.Friend():GetFriendArray(self.pageIndex)
            if isOK then
                self:LoadFinish()
            end
        else
            UIItemPool.PutTable(UIItemType.FriendItem, self.itemList)
        end
    end
end

---选择页签
---@param index number
function FriendView:OnSelectPage(index)
    if self.pageIndex == index then
        return
    end
    ---切换时清空
    self.searchInput.text = ''
    self.searchId = 0

    self:OnDisable()
    self.pageIndex = index
    self.searchGo:SetActive(self.pageIndex == 3)

    for i, v in pairs(self.pageList) do
        v:SelectPage(i==index)
    end

    self.noNetGo:SetActive(not Single.Http():IsHaveNetwork())
    if Single.Http():IsHaveNetwork() then
        self.noNeworkGo.gameObject:SetActive(true)
        Single.Request().SendFriend(self:GetFriendState(), 0, Handle(self, self.LoadFinish))
    else
        self:LoadFinish()
    end

    --self.goMass:SetActive(self.pageIndex == 4)
    --if self.pageIndex == 4 then
    --    self.scrollView.gameObject:SetActive(false)
    --    self.goChat:SetActive(false)
    --    self.massPanel:OnEnable( self.scrollView)
    --end
end

function FriendView:GetFriendState()
    if self.pageIndex == 1 then
        return EFriendState.Message
    elseif self.pageIndex == 2 then
        return EFriendState.Friend
    else
        return EFriendState.Stranger
    end
end

----value 1=退出刷新
function FriendView:OnDispatch(isSearch)
    self:LoadFinish(isSearch)
end

function FriendView:OnEnable()
    self:OnSelectPage(2)
end

---加载视图
function FriendView:LoadFinish(isSearch)
    UIItemPool.PutTable(UIItemType.FriendItem, self.itemList)
    self.contentFitter.enabled = false
    local tab = SingleData.Friend():GetFriendArray(self.pageIndex, isSearch or false)
    for i, v in ipairs(tab) do
        if self.itemList[i] == nil then
            self.itemList[i] = UIItemPool.Get(UIItemType.FriendItem, self.contentGo)
        end
        self.itemList[i]:OnEnable(v, self.pageIndex, Handle(self, self.ChangeChatPanel))
    end

    StartCoroutine(function()
        WaitForFixedUpdate()
        WaitForFixedUpdate()
        self.contentFitter.enabled = true
        WaitForSeconds(0.1)
        if self.pageIndex == 1 then
            self.scrollView:DOVerticalNormalizedPos(0, 0)
        end
    end)

    local isHasNet = Single.Http():IsHaveNetwork()
    self.noNetGo:SetActive(not isHasNet and #tab == 0)
    self.noNeworkGo:SetActive(isHasNet and not SingleData.Friend():IsRequestedFriendState(self.pageIndex))
end

function FriendView:ChangeLanguage()
    for _, v in pairs(self.pageList) do
        v:ChangeLanguage()
    end
end

---隐藏窗口
function FriendView:OnDisable()
    UIItemPool.PutTable(UIItemType.FriendItem, self.itemList)
    self.pageIndex = -1
    --self.massPanel:OnDisable()
end

return FriendView