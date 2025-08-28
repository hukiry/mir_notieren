---
--- MainFunction       
--- Author : hukiry     
--- DateTime 2023/3/5 11:34   
---

---调试log
---@param{...}
function log(...)
    if not RELEASE then
        print(...)
    end
end

---错误log
function logError(...)
    if not RELEASE then
        printError(...)
    end
end

---不满足条件，则会打印log
---@param condition boolean
function logAssert(condition, message)
    if not RELEASE then
        printAssert(condition, message)
    end
end

---获取语言表id
---@param id number
---@return string
function GetLanguageText(id)
    return SingleConfig.Language():GetLanguage(id)
end

---字符串公式转换为函数
---@param formulaStr string 符串公式
---@return function<number, number, number, ...> 返回函数
function GetFormula(formulaStr)
    local params = "params"
    local expression = formulaStr
    for i = 1, 5 do
        ---替换字符串
        expression = string.gsub(expression,"#"..i, params.."["..i.."]")
    end
    ---将字符串封装成函数
    local strfunc = loadstring("return function(...) local params = { ... } return " .. string.lower(expression) .." end")
    local func = strfunc()
    return func
end

---替换字符串格式
---@param desc string 替换的语言表格式 #
---@return string 返回字符串
function GetReplaceFormat(desc, ...)
    local tab,result = {...}, desc
    local len = #tab
    if len == 0 then return result end
    for i = 1, len do
        result = string.gsub(result,"#"..i, tostring(tab[i]))
    end
    return result
end

---设置场景精灵图标
---@param spriteRenderer UnityEngine.SpriteRenderer 图标组件
---@param spriteName string 精灵名称
---@param size UnityEngine.Vector2
---@return CommonSprite
function SetSceneIcon(spriteRenderer, spriteName, size)
    ---@type CommonSprite
    local iconSprite = CommonSprite.New(spriteRenderer, size)
    iconSprite:LoadSprite(spriteName,nil, function()
        UtilFunction.SetSceneAdaption(spriteRenderer, size)
    end)
    return iconSprite
end

---设置UI图标:固定图集
---@param image Hukiry.UI.AtlasImage 图标组件
---@param nameOrItemId string|number 精灵名称或物品ID
---@param size UnityEngine.Vector2 物品校正尺寸
---@param isNativeSize boolean 图片原始尺寸
function SetUIIcon(image, nameOrItemId, size, isNativeSize)
    if image == nil then logError("image is not empty")  return end
    isNativeSize = isNativeSize or false
    local spriteName = nameOrItemId
    if type(spriteName) == "number" then
        spriteName = "icon_" .. nameOrItemId
    end

    local atlasPath = ESpriteAtlasResource[spriteName]
    if atlasPath == nil then logError("atlasPath is not empty", spriteName)  return end
    if size then
        LoadAtlasImageSpriteAtlas(image, atlasPath, spriteName, isNativeSize,function()
            UtilFunction.SetUIAdaptionSize(image, size)
        end)
    else
        LoadAtlasImageSpriteAtlas(image, atlasPath, spriteName, isNativeSize)
    end
end

---@param image Hukiry.UI.AtlasImage 图标组件
---@param finishCall function
function LoadAtlasImageSpriteAtlas(image, path,  spriteName, isNativeSize, finishCall)
    local atlasName = nil
    if image.spriteAtlas then
        atlasName = image.spriteAtlas.name
    end

    if atlasName and image.spriteAtlas:GetSprite(spriteName) then
        image.spriteName = spriteName
        if isNativeSize then image:SetNativeSize() end
        if finishCall then finishCall() end
    else
         finishCall = finishCall or function()  end
         SingleAssist.LoaderMgr():LoadAsync(path, HandleParams(function(_image, _spriteName, _finishCall,_isNativeSize, _, atlas)
             _image.spriteAtlas = atlas
             _image.spriteName = _spriteName
             if _isNativeSize then _image:SetNativeSize() end
             if _finishCall then _finishCall() end
         end, image, spriteName, finishCall, isNativeSize))
    end
end

---定位垂直和水平滚动视图:需要在 StartCoroutine 里面调用,父预制件和子预制件对齐方向一直
---@param index number 索引的子视图
---@param scrollView UnityEngine.UI.ScrollRect
---@param contentTf UnityEngine.Transform
---@param isHorizontal boolean 是水平滚动
---@param duration number 滚动动画的时间，默认值为0.3s
function GotoScrollViewIndex(index, scrollView, contentTf, isHorizontal, duration)
    ---可见视图值
    local visibleValue = isHorizontal and contentTf.parent.rect.width or contentTf.parent.rect.height
    ---内容总值
    local contentSize = isHorizontal and contentTf.sizeDelta.x or contentTf.sizeDelta.y
    ---累加索引的子元素值
    local childTotal = 0
    for i = 1, contentTf.childCount do
        if i < index then
            local childTf = contentTf:GetChild(i-1)
            childTotal = childTotal + (isHorizontal and childTf.sizeDelta.x or childTf.sizeDelta.y)
        end
    end

    ---滚动的值
    local normalValue = isHorizontal and childTotal /(contentSize -visibleValue) or 1- childTotal /(contentSize -visibleValue)
    if normalValue <= 0 then
        normalValue = 0
    end

    scrollView:DOKill()
    if isHorizontal then
        scrollView:DOHorizontalNormalizedPos(normalValue, duration or 0.3)
    else
        scrollView:DOVerticalNormalizedPos(normalValue, duration or 0.3)
    end
end

---仅引用对象类，并初始化Start，不创建资源
---@param pItemType UIItemType
---@param gameObject UnityEngine.GameObject
---@param ... table 参数列表，Start初始化
function RequireObject(pItemType, gameObject, ...)
    local pValue = ItemPoolRule[pItemType]
    if pValue then
        local _return = require(pValue.itemClass).New(gameObject)
        _return.itemCtrl = require(pValue.itemClass.."Control").New(gameObject)
        _return:Start(...)
        return _return
    end
    logError("参数 pItemType 不能为空")
    return nil
end

---禁用按钮触发
---@param pGo UnityEngine.GameObject
---@param isGray boolean
function SetGrayButton(pGo, isGray)
    ---@type Hukiry.UI.AtlasImage
    local img = pGo:GetComponent("AtlasImage")
    if img then
        img.raycastTarget = not isGray
        img.IsGray = isGray
    end
end

---禁用按钮触发
---@param parentGo UnityEngine.GameObject
---@param isGray boolean
function SetGrayChildren(parentGo, isGray)
    local array = parentGo:GetComponentsInChildren(typeof(UnityEngine.Graphics), true)
    if array then
        for i = 1, array.Length do
           local go =  array[i-1].gameObject
            SetGrayButton(go, isGray)
        end
    end
end
