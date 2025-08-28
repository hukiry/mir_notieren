---
--- RefObject       
--- Author : hukiry     
--- DateTime 2024/8/8 14:30   
---

---@class RefObject
RefObject = Class()
---初始化字段
function RefObject:ctor()
    self.m_ReferencedCount = 0
end

---@return number
function RefObject:ReferencedCount()
    return self.m_ReferencedCount
end

function RefObject:Retain()
    self.m_ReferencedCount = self.m_ReferencedCount + 1
end

function RefObject:Release()
    self.m_ReferencedCount = self.m_ReferencedCount - 1

    if UNITY_EDITOR then
        if self.m_ReferencedCount<0 then
            logAssert(false, "RefObject over killed")
        end
    end

    if self.m_ReferencedCount == 0 then
        self:DeleteSelf()
    end
end

function RefObject:DeleteSelf()

end
