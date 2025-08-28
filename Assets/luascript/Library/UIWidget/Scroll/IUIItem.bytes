---
--- IUIItem 单项 基类
--- Created by hukiry.
--- DateTime: 2020/7/29 20:02

---@class IUIItem:DisplayObjectBase
IUIItem = Class(DisplayObjectBase)

function IUIItem:ctor(gameObject)
    ---@type number 该item所在列表的索引
    self.numberId = -1
end

---每次从对象池中拿出来时调用
function IUIItem:Start(...)

end

--更新数据
function IUIItem:OnEnable(info)

end

function IUIItem:OnRefresh(...)

end

function IUIItem:OnDisable()
    self.numberId = -1
end

function IUIItem:OnDestroy()

end