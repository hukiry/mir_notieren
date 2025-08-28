---
--- FriendMassPanel       
--- Author : hukiry     
--- DateTime 2024/7/22 13:29   
---

---@class FriendMassPanel:DisplayObjectBase
local FriendMassPanel = Class(DisplayObjectBase)

function FriendMassPanel:ctor(gameObject)

end

function FriendMassPanel:Start()
    self.ContentGo = self:FindGameObject("ScrollView/Viewport/Content")
    ---@type UnityEngine.UI.ContentSizeFitter
    self.contentFitter = self.ContentGo:GetComponent("ContentSizeFitter")
    self.deleteBtn = self:FindGameObject("ScrollView/bg/deleteBtn")
    self.quiteBtn = self:FindGameObject("ScrollView/bg/quiteBtn")
    self.CreateBtn = self:FindGameObject("ScrollView/bg/CreateBtn")
    self.askBtn = self:FindGameObject("ScrollView/bg/askBtn")
    self.noNetGo = self:FindGameObject("ScrollView/noNet")

    self:AddClick(self.askBtn, Handle(self, self.AskHelp))
    self:AddClick(self.deleteBtn, function()
        Single.Request().SendLife(0, ELifeState.DeleteGass)
    end)

    self:AddClick(self.quiteBtn, function()
        Single.Request().SendLife(0, ELifeState.QuitGass)
    end)

    self:AddClick(self.CreateBtn, function()
        Single.Request().SendLife(0, ELifeState.Gass)
    end)

    self:AddClick(self.CreateBtn, function()
        Single.Request().SendLife(0, ELifeState.Gass)
    end)

    ---@type table<number, LifeHelpItem>
    self.lifeItemList = {}

    self.massItemList = {}
end

function FriendMassPanel:AskHelp()
    Single.Request().SendLife(0,  ELifeState.AskHelp)
end

function FriendMassPanel:OnEnable()
    SetGrayChildren(self.itemCtrl.okBtnGo, SingleData.Mass():IsRequestLife())
    self.itemCtrl.okTxt = GetLanguageText(14023)
    Single.Request().SendLife(0, ELifeState.Life, Handle(self, self.LoadLife))
end


function FriendMassPanel:AddLife()
    local index = table.length(self.lifeItemList)+1
    ---@type LifeInfo
    local info = require("Game.UI.Friend.Data.LifeInfo").New()
    info.Id = Single.Player().roleId
    info.count = 0
    info.state = ELifeState.AskHelp
    info.nick = Single.Player().roleNick
    self.lifeItemList[index] = UIItemPool.Get(UIItemType.LifeHelpItem, self.contentGo)
    self.lifeItemList[index]:OnEnable(info)
end

---加载生命列表
function FriendMassPanel:LoadLife(isSucc)
    if isSucc then
        self.contentFitter.enabled = false
        local tab = SingleData.Mass():GetLifeArray()
        for i, v in ipairs(tab) do
            if self.lifeItemList[i] == nil then
                self.lifeItemList[i] = UIItemPool.Get(UIItemType.LifeHelpItem, self.contentGo)
            end
            self.lifeItemList[i]:OnEnable(v)
        end

        StartCoroutine(function()
            WaitForFixedUpdate()
            WaitForFixedUpdate()
            self.contentFitter.enabled = true
        end)
    end
end

---@param state EChatState
function FriendMassPanel:OnDispatch(state)

end

function FriendMassPanel:OnDisable()
    UIItemPool.PutTable(UIItemType.LifeHelpItem, self.lifeItemList)

end

return FriendMassPanel
