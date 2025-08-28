---
--- SelectItem       
--- Author : hukiry     
--- DateTime 2023/7/17 20:43   
---

---@class SelectItem:UIWindowBase
local SelectItem = Class(UIWindowBase)

function SelectItem:ctor(gameObject, callback)
    self.callback = callback
    self:Start()
end

---初始界面:注册按钮事件等
function SelectItem:Start()
    self.titleTxt = self:FindHukirySupperText("title")
    ---@type Hukiry.UI.AtlasImage
    self.img = self.gameObject:GetComponent("AtlasImage")

    self:AddClick(self.gameObject, self.callback)
end

function SelectItem:OnEnable(index, langId)
    self.index, self.langId = index, langId
    self:ChangeLanguage()
end

function SelectItem:ChangeLanguage()
    self.titleTxt.text = GetLanguageText(self.langId)
end

---选择
---@param isSelect boolean
function SelectItem:SelectPage(isSelect)
    self.img.spriteName = isSelect and "btl9" or "btl8"
end

---隐藏窗口
function SelectItem:OnDisable()

end

return SelectItem