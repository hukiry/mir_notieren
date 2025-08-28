---
--- PropPartView       
--- Author : hukiry     
--- DateTime 2023/10/20 11:01   
---

---@private
---@class EPropPartItem
EPropPartItem = {
    ---@type UnityEngine.GameObject
    effect = 1,
    ---@type UnityEngine.CanvasGroup
    group = 2,
    ---@type Hukiry.HukirySupperText
    txt = 3,
    ---@type UnityEngine.GameObject
    gameObject = 4
}
---@class PropPartView:DisplayObjectBase
local PropPartView = Class(DisplayObjectBase)

function PropPartView:ctor(gameObject)
    ---@type table<number, EPropPartItem>
    self.itemList = {}

    self.propsTab = {
        { icon = "item_hammer", type = EPropsView.Wipe },
        { icon = "item_bows", type = EPropsView.Row },
        { icon = "item_gun", type = EPropsView.Cell },
        { icon = "item_cap", type = EPropsView.DiceRandom },
        { icon = "shezhi", type = EPropsView.None },
    }
end

---每次从对象池中拿出来时调用
function PropPartView:Start(_self)
    ---@type EPropsView
    self.selectProp = EPropsView.None
    ---@type LevelMainView
    self.lvMainView = _self

    local childCount = self.transform.childCount
    for i = 1, childCount do
        local  tf = self.transform:GetChild(i-1)
        local data = self.propsTab[i]
        ---@type EPropPartItem
        local itemInfo = {}
        local img = tf:FindAtlasImage("icon")
        img.spriteName = data.icon

        itemInfo.txt = tf:FindHukirySupperText("num")
        local number = self:GetItemNum(data.type)

        itemInfo.txt.text = number>0 and number or '+'
        itemInfo.effect = tf:FindGameObject("ui_Icon")
        itemInfo.group = tf.gameObject:GetComponent("CanvasGroup")
        itemInfo.gameObject = tf.gameObject
        self.itemList[data.type] = itemInfo
        self:AddClick(tf, Handle(self, self.ClickProps, data.type), true)
    end

    self.isOpenSetting = true
    ---动画播放
    ---@type DG.Tweening.DOTweenAnimation
    self.animationMusicSound = _self.ViewCtrl.musicSoundBtnGo:GetComponent("DOTweenAnimation")
    self.animationMusicSound:DORestart()
    ---@type DG.Tweening.DOTweenAnimation
    self.animationMusicEffect = _self.ViewCtrl.musicEffectBtnGo:GetComponent("DOTweenAnimation")
    self.animationMusicEffect:DORestart()
    ---@type DG.Tweening.DOTweenAnimation
    self.animationExit = _self.ViewCtrl.exitBtnGo:GetComponent("DOTweenAnimation")
    self.animationExit:DORestart()

    self:AddClick(_self.ViewCtrl.aniSetBtnGo,function()
        UIEventListener.ExecuteEvent(self:GetItem(EPropsView.None), EventTriggerType.PointerClick)
    end)

    self:AddClick(_self.ViewCtrl.exitBtnGo, function()
        UIManager:OpenWindow(ViewID.LevelQuit, self:GetItem(EPropsView.None))
    end)


    self.musicOffGo = _self.ViewCtrl.musicSoundBtnGo.transform:FindGameObject("x")
    self.effectOffGo = _self.ViewCtrl.musicEffectBtnGo.transform:FindGameObject("x")
    self.isEffectOff,  self.isMusicOff = false, false
    self:AddClick(_self.ViewCtrl.musicSoundBtnGo, function()
        self.isMusicOff = not self.isMusicOff
        self.musicOffGo:SetActive(not self.isMusicOff)
        Single.Sound():SetMusicMute(self.isMusicOff)
    end)

    self:AddClick(_self.ViewCtrl.musicEffectBtnGo, function()
        self.isEffectOff = not self.isEffectOff
        self.effectOffGo:SetActive(not self.isEffectOff)
        Single.Sound():SetSoundMute(self.isEffectOff)
    end)


end

function PropPartView:OnEnable()
    self.lvMainView.ViewCtrl.propHelpGo:SetActive(false)
    self.lvMainView.ViewCtrl.settingPanelGo:SetActive(false)
    self.lvMainView.ViewCtrl.aniSetBtnGo:SetActive(false)

    self.isMusicOff = Single.PlayerPrefs():GetBool(EGameSetting.MusicMute, false)
    self.isEffectOff = Single.PlayerPrefs():GetBool(EGameSetting.SoundMute, false)
    self.musicOffGo:SetActive(not self.isMusicOff)
    self.effectOffGo:SetActive(not self.isEffectOff)
end

---@param props EPropsView
function PropPartView:ClickProps(props)
    if props ~= EPropsView.None then
        EventDispatch:Broadcast(UIEvent.Match_AITip_Shake_View, nil, nil, true) --设置提示时间为0
        if self:GetItemNum(props) > 0 then
            MatchRule.isUseProp = not MatchRule.isUseProp
            self.selectProp = props
            self:ShowPropsButton(self.selectProp)

            self.lvMainView.ViewCtrl.propHelpGo:SetActive(MatchRule.isUseProp)
            local info = Single.Match():GetMapConifg():GetPropsInfo(props):GetCurrency()
            self.lvMainView.ViewCtrl.titleTx.text = GetLanguageText(info.languageName)
            self.lvMainView.ViewCtrl.descTx.text = GetLanguageText(info.languageDesc)
        else
            UIManager:OpenWindow(ViewID.PropsTip, props)
            self.lvMainView.ViewCtrl.propHelpGo:SetActive(false)
        end
    else
        self.isOpenSetting = not self.isOpenSetting
        if self.isOpenSetting then
            self.animationMusicSound:DOPlayBackwards()
            self.animationMusicEffect:DOPlayBackwards()
            self.animationExit:DOPlayBackwards()
            StartCoroutine(function()
                WaitForSeconds(0.5)
                self.lvMainView.ViewCtrl.aniSetBtnGo:SetActive(false)
                self.lvMainView.ViewCtrl.settingPanelGo:SetActive(false)
            end)
        else
            self.lvMainView.ViewCtrl.propHelpGo:SetActive(false)
            self.lvMainView.ViewCtrl.aniSetBtnGo:SetActive(true)
            self.lvMainView.ViewCtrl.settingPanelGo:SetActive(true)
            self.animationMusicSound:DORestart()
            self.animationMusicEffect:DORestart()
            self.animationExit:DORestart()
        end
    end
end

-----@param props EPropsView
function PropPartView:ShowPropsButton(props)
    EventDispatch:Broadcast(UIEvent.Match_BackgoundMask_View, MatchRule.isUseProp)
    for i, v in pairs(self.itemList) do
        if MatchRule.isUseProp then
            v.group.alpha = i==props and 1 or 0.3
            v.group.blocksRaycasts = i==props
            if v.effect then
                v.effect:SetActive(i==props)
            end
        else
            v.group.alpha = 1
            v.group.blocksRaycasts = true
            if v.effect then
                v.effect:SetActive(false)
            end
        end
    end
end

---@param state number 3=使用数据
function PropPartView:OnDispatch(state, itemId, x, y)
    if state == 3 then
        self:ShowPropsButton(self.selectProp)
        EventDispatch:Broadcast(UIEvent.Match_Operate_Props, self.selectProp, x, y)
        Single.Player():UpdatePropsNum(self.selectProp)
        if self.itemList[self.selectProp] then
            local number = self:GetItemNum(self.selectProp)
            self.itemList[self.selectProp].txt.text =  number>0 and number or '+'
        end
        self.lvMainView.ViewCtrl.propHelpGo:SetActive(false)
    elseif state == 4 then
        if self.itemList[itemId] then
            local number = self:GetItemNum(itemId)
            self.itemList[itemId].txt.text =  number>0 and number or '+'
        end
    end
end

---@private
---@param props EPropsView
function PropPartView:GetItemNum(props)
    local moneyType = EPropsToMoneyType[props]
    return Single.Player():GetMoneyNum(moneyType)
end

---@private
---@param props EPropsView
function PropPartView:GetItem(props)
    return self.itemList[props].gameObject
end

return PropPartView