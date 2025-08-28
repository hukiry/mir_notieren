---
--- ToggleView       
--- Author : hukiry     
--- DateTime 2023/10/16 19:37   
---

---Unity下拉框视图
---@class ToggleView:DisplayObjectBase
local ToggleView = Class(DisplayObjectBase)
function ToggleView:ctor()
end

---@param defaultVaule
---@param callback function<boolean>
---@param transform UnityEngine.Transform
function ToggleView:OnEnable(transform, callback, defaultVaule)
    self.isToggle = defaultVaule or false
    self.transform = transform
    self.callback = callback

    self.checkMask = transform:FindGameObject("Background/Checkmark")
    self.background = transform:FindGameObject("Background")
    self.label = transform:FindHukirySupperText("Label")

    self.checkMask:SetActive(self.isToggle)
    self:AddClick(self.background, Handle(self, self._OnClick))
end

---@private
function ToggleView:_OnClick()
    self.isToggle = not self.isToggle
    self.checkMask:SetActive(self.isToggle)
    self.callback(self.isToggle)
end

---设置toggle
---@param isSelected boolean
function ToggleView:SetToggle(isSelected)
    self.isToggle = isSelected
    self.checkMask:SetActive(self.isToggle)
    self.callback(self.isToggle)
end

function ToggleView:OnDisable()

end

return ToggleView