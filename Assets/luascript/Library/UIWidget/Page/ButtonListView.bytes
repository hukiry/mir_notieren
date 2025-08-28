--- 控制页签的选择:静态与动态加载
--- SelectPageView       
--- Author : hukiry     
--- DateTime 2023/10/10 17:07   
---

---页签选择数据
---@class EPageSelectType
EPageSelectType = {
    ---选中的背景
    ---@type string
    selectBack=1,
    ---默认背景
    ---@type string
    unSelectBack=2,
    ---选中的图标
    ---@type string
    selectIcon=3,
    ---默认图标
    ---@type string
    unSelectIcon=4,
    ---选中的文本颜色，例如：#DF33FF
    ---@type string
    selectColor=5,
    ---默认颜色
    ---@type string
    unSelectColor=6,

    ---页签名的语言id
    ---@type number
    pageNameId=7
}

---选择静态页签管理
---页签下的 name, icon
---@class ButtonListView
local ButtonListView = Class()
function ButtonListView:ctor()
    ---@type table<number, ButtonItem>
    self.itemList = {}
end

---加载数据
---@param callback function<number> 选择页签回调
---@param pageTab table<number, EPageSelectType> 页签数据
---@param parent UnityEngine.Transform 父对象
---@param isDynamic boolean 是动态资源加载
---@param dynamicPrePath string 预制件路径
function ButtonListView:OnEnable(parent, pageTab, callback, isDynamic, dynamicPrePath)
    self.callback = callback
    local childCount = parent.childCount
    if #pageTab < childCount then
        logError("数据长度不能超过初始化项的长度！")
        return
    end

    self.isDynamic = isDynamic

    if self.isDynamic then
        for i, v in ipairs(pageTab) do
            local go = GameObjectPool.Get(dynamicPrePath, parent.gameObject)
            self:_UpdatePageItem(i, v ,go)
        end
    else
        for i = 1, childCount do
            local tf = parent:GetChild(i-1)
            self:_UpdatePageItem(i, pageTab[i] ,tf.gameObject)
        end
    end
end

---@private
---@param data EPageSelectType
---@param gameObject UnityEngine.GameObject
---@param i number
function ButtonListView:_UpdatePageItem(i, data, gameObject)
    self.itemList[i] = require("Library.UIWidget.Page.ButtonItem").New(gameObject)
    self.itemList[i]:Awake(i, self, data)
end

---选中页签
---@param index number 选中的索引
function ButtonListView:OnSelect(index)
    for i, v in pairs(self.itemList) do
        v:OnEnable(i==index)
    end

    if self.callback then
        self.callback(index)
    end
end

function ButtonListView:OnDisable()
    --动态加载需要销毁
    if self.isDynamic then
        GameObjectPool.PutTable(self.itemList)
    end
end

return ButtonListView