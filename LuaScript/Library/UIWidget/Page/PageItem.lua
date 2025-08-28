---
--- PageItem
--- Author : hukiry     
--- DateTime 2023/7/25 14:49   
---

---@class PageItem
local PageItem = Class()
function PageItem:ctor()
    self.scrollIndex = 3
    self.isHasScroll = false
end

---@param downTF UnityEngine.Transform
function PageItem:Start(_self, downTF)
    ---选择页签
    ---@type table<number, {...}>
    self.pageList = {}
    self.downTF = downTF
    ---@type GameControl
    self.ViewCtrl = _self.ViewCtrl

    self.minSize, self.maxSize = 0, 0
    self.selectIndex = 3

    local childCount = downTF.childCount
    for i = 1, childCount-1 do
        local tf = downTF:GetChild(i)
        self.pageList[i] = {transform = tf, iconTf = tf:Find("frameIcon"), nameGo = tf:Find("name").gameObject}
        self.pageList[i].nameGo:SetActive(false)
        _self:AddClick(tf.gameObject, Handle(_self, _self.SelectPage, i, true), false, false, true)
    end

    UIEventListener.onScrollEndUp = Handle(self, self.WhenScrollClickUp)
end

---滚动点击抬起时
function PageItem:WhenScrollClickUp()
    if self.isHasScroll  then--滚动下一页，需要点击+滑动
        self.isHasScroll = false
        self:SelectPage(self.scrollIndex)
    else
        --未滚动下一页，仅滑动
        self:GotoScrollView(self.scrollIndex)
    end
end

---滚动改变时
function PageItem:WhenScrollChange(index)
    self.scrollIndex = Mathf.Clamp(math.abs(index), 1, 5)
    ---是否滑动到下一页
    self.isHasScroll = self.scrollIndex ~= self.selectIndex
end

---页签选择
---@param index number
function PageItem:SelectPage(index, isClick)
    if isClick then
        if self.selectIndex == index then
            return
        end
    end
    self.pageList[self.selectIndex].nameGo:SetActive(false)
    self.pageList[self.selectIndex].iconTf.anchoredPosition = Vector2.New(0,-23)
    self.pageList[self.selectIndex].iconTf.localScale = Vector3.one

    local duration = 0.2
    local y = self.pageList[index].transform.sizeDelta.y
    local tf1, tf2 = self.pageList[self.selectIndex].transform, self.pageList[index].transform

    self.selectIndex = index
    tf1:DOSizeDelta(Vector2.New(self.minSize, y), duration)
    tf2:DOSizeDelta(Vector2.New(self.maxSize, y), duration):OnComplete(function()
        self.pageList[self.selectIndex].nameGo:SetActive(true)
        self.pageList[self.selectIndex].iconTf.anchoredPosition = Vector2.New(0,65)
        self.pageList[self.selectIndex].iconTf:DOScale(Vector3.one*1.1, 0.2)
        self.ViewCtrl.backTF:DOMove(self.pageList[self.selectIndex].transform.position, duration)
    end)

    self:GotoScrollView(index)
end

---定位滚动视图
---@param index number
function PageItem:GotoScrollView(index)
    GotoScrollViewIndex(index,self.ViewCtrl.scrollView,self.ViewCtrl.centerTF, true)
end

function PageItem:OnEnable()
    self.minSize, self.maxSize = self:_CucalateSize()
    for i = 1, 5 do
        local x = self.selectIndex ==i and self.maxSize or self.minSize
        self.pageList[i].transform:SetSizeDelta(x)
    end

    StartCoroutine(function()
        WaitForFixedUpdate()
        WaitForFixedUpdate()
        self.ViewCtrl.backTF:SetSizeDelta(self.maxSize)
    end)

    self:SelectPage(self.selectIndex)
end

---分成5等份
---@return number,number 原来尺寸，选择后的尺寸
function PageItem:_CucalateSize()
    local width = self.ViewCtrl.downTF.rect.width
    local cell = width/5
    local min = cell*0.93
    local max = width-min*4
    return min, max
end

---动态切换语言
function PageItem:OnChangeLanguage()
    for i, v in pairs(self.pageList) do
        ---@type Hukiry.HukirySupperText
        local txtName = v.nameGo:GetComponent("HukirySupperText")
        txtName.text = GetLanguageText(10010 + i)
    end
end

function PageItem:OnDispatch(index)
    if self.selectIndex~=index then
        self.scrollIndex = index
        self:SelectPage(self.scrollIndex)
    end
end

---隐藏窗口
function PageItem:OnDisable()
    
end

return PageItem