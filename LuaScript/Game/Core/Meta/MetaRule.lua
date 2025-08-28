---
--- MetaRule       
--- Author : hukiry     
--- DateTime 2023/9/13 20:14   
---

---选择状态
---@class ESelectState
ESelectState = {
    None = 0,
    ---编辑物品
    AddItem = 1,
    ---擦除物品
    Delete = 2,
    ---撤回
    ResetDo = 3
}