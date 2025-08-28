--- 滚动视图嵌套
--- UIScrollViewNested       
--- Author : hukiry     
--- DateTime 2023/7/18 13:11   
---

---@class UIScrollViewNested:DisplayObjectBase
UIScrollViewNested = Class(DisplayObjectBase)

---@private
---@class EScrollDir
EScroll_View_Dir = {
    Horizontal=1, Vertical=2
}

function UIScrollViewNested:ctor(gameObject)
    ---默认组件
    ---@type UnityEngine.UI.ScrollRect
    self.scrollRect = self.gameObject:GetComponent("ScrollRect")
    self.scrollRect:Awake()
    ---父对象滚动组件
    ---@type UnityEngine.UI.ScrollRect
    self.scrollRectParent = nil
    if self.transform.parent ~= nil then
        self.scrollRectParent = self.transform.parent:GetComponentInParent(typeof(UnityEngine.UI.ScrollRect))
    end
    ---默认方向
    ---@type EScrollDir
    self.scrollDir = self.scrollRect.horizontal and EScroll_View_Dir.Horizontal or EScroll_View_Dir.Vertical
    ---当前方向
    ---@type EScrollDir
    self.curScrollDir = EScroll_View_Dir.Horizontal

    self.isEnable = true
    UIEventListener.Get(self.gameObject).onBeginDrag = Handle(self, self.OnBeginDrag)
    UIEventListener.Get(self.gameObject).onDrag = Handle(self, self.OnDrag)
    UIEventListener.Get(self.gameObject).onEndDrag = Handle(self, self.OnEndDrag)
    UIEventListener.Get(self.gameObject).onScroll = Handle(self, self.OnScroll)
end

---@param eventData UnityEngine.EventSystems.PointerEventData
function UIScrollViewNested:OnBeginDrag(eventData)
    if self.scrollRectParent then
        self.curScrollDir = Mathf.Abs(eventData.delta.x)>Mathf.Abs(eventData.delta.y) and EScroll_View_Dir.Horizontal or EScroll_View_Dir.Vertical
        if self.curScrollDir~=self.scrollDir then
            UIEventListener.ExecuteEvent(self.scrollRectParent.gameObject, EventTriggerType.BeginDrag, eventData)
            return
        end
    end

    if self.isEnable then
        self.scrollRect:OnBeginDrag(eventData)
    end
end

---@param eventData UnityEngine.EventSystems.PointerEventData
function UIScrollViewNested:OnDrag(eventData)
    if self.scrollRectParent then
       if self.curScrollDir~=self.scrollDir then
            UIEventListener.ExecuteEvent(self.scrollRectParent.gameObject, EventTriggerType.Drag, eventData)
            return
        end
    end

    if self.isEnable then
        self.scrollRect:OnDrag(eventData)
    end
end

---@param eventData UnityEngine.EventSystems.PointerEventData
function UIScrollViewNested:OnEndDrag(eventData)
    if self.scrollRectParent then
        if self.curScrollDir~=self.scrollDir then
            UIEventListener.ExecuteEvent(self.scrollRectParent.gameObject, EventTriggerType.Drag, eventData)
            return
        end
    end

    if self.isEnable then
        self.scrollRect:OnEndDrag(eventData)
    end
end

---@param eventData UnityEngine.EventSystems.PointerEventData
function UIScrollViewNested:OnScroll(eventData)
    if self.scrollRectParent then
        if self.curScrollDir~=self.scrollDir then
            UIEventListener.ExecuteEvent(self.scrollRectParent.gameObject, EventTriggerType.Scroll, eventData)
            return
        end
    end

    if self.isEnable then
        self.scrollRect:OnScroll(eventData)
    end
end

function UIScrollViewNested:OnDisable()
    self.scrollRect = nil
    self.scrollRectParent = nil
    UIEventListener.Get(self.gameObject).onBeginDrag = nil
    UIEventListener.Get(self.gameObject).onDrag = nil
    UIEventListener.Get(self.gameObject).onEndDrag = nil
    UIEventListener.Get(self.gameObject).onScroll = nil
end
