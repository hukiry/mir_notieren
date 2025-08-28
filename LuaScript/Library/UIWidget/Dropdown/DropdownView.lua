---
--- DropdownView       
--- Author : hukiry     
--- DateTime 2023/10/12 17:39   
---

---@class EOptionData
EOptionData = {
    ---文本信息
    ---@type string
    text = 1,
    ---精灵对象
    ---@type UnityEngine.Sprite
    image = 2
}

---Unity下拉框视图
---@class DropdownView
local DropdownView = Class()
function DropdownView:ctor()
    self.OptionData = UnityEngine.UI.Dropdown.OptionData
end

---显示窗口:初次打开
---@param dropdown Hukiry.UI.UIDropdown 组件
---@param optionTab table<number, EOptionData> 集合数据
---@param callback function<number> 选择回调
---@param defaultVaule number 设置选定的项
function DropdownView:OnEnable(dropdown, optionTab, callback, defaultVaule)
    self.gameObject = dropdown.gameObject
    ---@type UnityEngine.CanvasGroup
    self.canvasGroup = self.gameObject:GetComponent("CanvasGroup")
    self.valueChangedback = callback
    ---@type Hukiry.UI.UIDropdown
    self.dropdown = dropdown
    self.dropdown.options:Clear()
    for i, v in ipairs(optionTab) do
        local itemOption = self.OptionData.New(v.text, v.image)
        self.dropdown.options:Add(itemOption)
    end

    ---如果 self.dropdown.value 大于0，则会自动调用回调函数，否则不处理. 这里不会回调
    self.dropdown.value = defaultVaule or 0
    self.listenerFunction = Handle(self, self._OnValueChanged)
    self.dropdown.onValueChanged:AddListener(self.listenerFunction)


    if dropdown:GetType().FullName == "Hukiry.UI.UIDropdown" then
        self.dropdown:AddListener(Handle(self, self._ClickDrop))
    end
end

---@private
function DropdownView:_ClickDrop()
    if self.dropdownOthers then
        for i, v in ipairs(self.dropdownOthers) do
            v:Hide()
        end
    end
end

---设置值
---@param valueIndex number 索引值，0开始
function DropdownView:SetDropdownValue(valueIndex)
    self.dropdown.value = valueIndex
    self.dropdown:RefreshShownValue()
end

---隐藏自己
---@param isVisible boolean
function DropdownView:SetActive(isVisible)
    if self.canvasGroup and isVisible~=nil then
        self.canvasGroup:DOKill()
        self.gameObject:SetActive(true)
        local endAlpha = isVisible and 1 or 0
        self.canvasGroup:DOFade(endAlpha, 1)
    else
        self.gameObject:SetActive(isVisible)
    end
end

---@private
---@param index number
function DropdownView:_OnValueChanged(index)
    self.valueChangedback(index)
end

function DropdownView:OnDisable()
    self.dropdown.onValueChanged:RemoveListener(self.listenerFunction)
end

---设置同一界面重叠时，隐藏其他对象
---@param dropdownOthers table<number, Hukiry.UI.UIDropdown>
function DropdownView:SetHideDropDown(dropdownOthers)
    self.dropdownOthers = dropdownOthers
end

return DropdownView