---
--- SelectPageItem       
--- Author : hukiry     
--- DateTime 2023/10/10 17:07   
---

---@class ButtonItem:DisplayObjectBase
local ButtonItem = Class(DisplayObjectBase)
function ButtonItem:ctor(gameObject)
    ---页签名
    self.name = nil
    ---页签图标
    self.icon = nil
end

---初始化
---@param data EPageSelectType
---@param _self ButtonItem
---@param index number
function ButtonItem:Awake(index, _self, data)
    self.data = data
    self.index = index
    ---背景
    self.background = self.transform:GetComponent("AtlasImage")
    ---文本
    self.name = self:FindHukirySupperText("name")
    ---图标
    self.icon = self:FindAtlasImage("icon")

    if self.background then
        self:AddClick(self.background, Handle(_self, _self.OnSelect, self.index))
    else
        self:AddClick(self.gameObject, Handle(_self, _self.OnSelect, self.index))
    end
    self:_ChangeName()
end

---选择后的改变
---@param isSelect boolean 由父对象调用
function ButtonItem:OnEnable(isSelect)
    self:_ChangeBackground(isSelect)
    self:_ChangeSpriteName(isSelect)
    self:_ChangeColor(isSelect)
end

---@private
function ButtonItem:_ChangeName()
    if self.name then
        self.name.text = GetLanguageText(self.data.pageNameId)
    end
end

---@private
function ButtonItem:_ChangeBackground(isSelect)
    if self.background then
        self.background.spriteName = isSelect and  self.data.selectBack or self.data.unSelectBack
    end
end

---@private
function ButtonItem:_ChangeSpriteName(isSelect)
    if self.icon then
        self.icon.spriteName = isSelect and  self.data.selectIcon or  self.data.unSelectIcon
        self.icon:SetNativeSize()
    end
end

---@private
function ButtonItem:_ChangeColor(isSelect)
    if self.name then
        self.name.color = Hukiry.HukiryUtil.StringToColor(isSelect and self.data.selectColor or self.data.unSelectColor)
    end
end

return ButtonItem