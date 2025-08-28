---
--- InputFieldItem       
--- Author : hukiry     
--- DateTime 2024/8/14 15:06   
---

---@class InputFieldItem:DisplayObjectBase
local InputFieldItem = Class(DisplayObjectBase)
function InputFieldItem:ctor(gameObject)
    self.inputfield = self.transform:FindInputField()
end

---@param changeTextCall function<string>
---@param isEndEdit boolean
function InputFieldItem:Start(changeTextCall, isEndEdit)
    self.changeTextCall = changeTextCall
    if isEndEdit then
        self.inputfield.onEndEdit:AddListener(Handle(self, self._onEndEdit))
    else
        self.inputfield.onValueChanged:AddListener(Handle(self, self._onValueChanged))
    end
end

---@private
function InputFieldItem:_onValueChanged(v)
    self.changeTextCall(v)
end
---@private
function InputFieldItem:_onEndEdit(v)
    self.changeTextCall(v)
end

function InputFieldItem:OnDisable()
end

return InputFieldItem