---
--- CommonFunction       
--- Author : hukiry     
--- DateTime 2022/10/10 8:31   
---

UtilFunction = {}

---@desc 设置UI精灵显示适配尺寸
---@param _UISprite Hukiry.UI.AtlasImage|UnityEngine.UI.Image
---@param _clampSize UnityEngine.Vector2 适配区域
function UtilFunction.SetUIAdaptionSize(_UISprite, _clampSize)
    if _UISprite == nil or _clampSize == nil or _UISprite.sprite == nil then
        log("参数不能为空",_UISprite, _clampSize,"red")
        return
    end
    local sprite = _UISprite.sprite

    local spriteSize = Vector2.New(sprite.rect.width, sprite.rect.height)
    local newSize = Vector2.zero
    if spriteSize.x > spriteSize.y then
        local scale = spriteSize.y / spriteSize.x
        newSize = Vector2.New(_clampSize.x, _clampSize.x * scale) ---把宽缩放给高
    else
        local scale = spriteSize.x / spriteSize.y
        newSize = Vector2.New(_clampSize.y * scale, _clampSize.y) ---把高缩放给宽
    end

    local rect = _UISprite.gameObject:GetComponent(typeof(UnityEngine.RectTransform))
    rect.sizeDelta = newSize
end

---@desc 设置场景精灵显示适配尺寸
---@param spriteRenderer UnityEngine.SpriteRenderer
---@param _clampSize UnityEngine.Vector2 适配区域
function UtilFunction.SetSceneAdaption(spriteRenderer, _clampSize)
    if spriteRenderer == nil or _clampSize == nil or spriteRenderer.sprite == nil then
        --logError("参数不能为空", spriteRenderer, _clampSize,"red")
        return
    end
    local sprite = spriteRenderer.sprite
    local newSize, spriteSize = Vector2.zero, Vector2.New(sprite.rect.width, sprite.rect.height)
    local x, y = _clampSize.x , _clampSize.y
    if spriteSize.x <_clampSize.x then x = spriteSize.x end
    if spriteSize.y <_clampSize.y then y = spriteSize.y end
    newSize = Vector2.New(x/spriteSize.x, y / spriteSize.y) ---把宽缩放给高
    spriteRenderer.transform.localScale = Vector3.New(newSize.x, newSize.y, 1)
    return newSize
end

---跟随屏幕尺寸设置
---@param spriteRenderer UnityEngine.SpriteRenderer
---@param _clampSize UnityEngine.Vector2 适配区域
function UtilFunction.SetScreenAdaption(spriteRenderer, _clampSize)
    if spriteRenderer == nil or _clampSize == nil or spriteRenderer.sprite == nil then
        logError("参数不能为空", spriteRenderer, _clampSize,"red")
        return
    end
    local sprite = spriteRenderer.sprite
    local newSize, spriteSize = Vector2.zero, Vector2.New(sprite.rect.width, sprite.rect.height)
    local x, y = _clampSize.x , _clampSize.y
    newSize = Vector2.New(x/spriteSize.x, y / spriteSize.y) ---把宽缩放给高
    spriteRenderer.transform.localScale = Vector3.New(newSize.x, newSize.y, 1)
    return newSize
end

---转换到千位字符串
---@param num number 数字
---@param isOne boolean 默认为true
function UtilFunction.NumberToKMB(num, isOne)
    if isOne == nil then
        isOne = true
    end

    if num > 1000 then
        if isOne then
            return string.format("%0.1fK", num/1000)
        else
            return string.format("%0.2fK", num/1000)
        end
    elseif num > 1000000 then
        if isOne then
            return string.format("%0.1fM", num/1000000)
        else
            return string.format("%0.2fM", num/1000000)
        end
    else
        return num
    end
end

---设置IOS刘海屏
function UtilFunction.SetScreenNotch(rectTransform)
    if UNITY_IOS then
        local anchor = rectTransform.anchorMin
        anchor.y = 35 / (1.0 * UnityEngine.Screen.width)
        rectTransform.anchorMin = anchor
    end
end

---给某个物体添加mono组件 已存在就不添加
---@param _ComType UnityEngine.Component 组件类型
---@param pGo UnityEngine.GameObject
---@return UnityEngine.MonoBehaviour
function UtilFunction.AddComponent(_ComType, pGo)
    if _ComType and pGo then
        local _Com = pGo:GetComponent(_ComType)
        if _Com  == nil then
            _Com = pGo:AddComponent(_ComType)
            return _Com
        end
    end
end

---移除组件
function UtilFunction.RemoveComponent(_ComType, pGo)
    if _ComType and pGo then
        local _Com = pGo:GetComponent(_ComType)
        if _Com  ~= nil then
            GameObject.Destroy(_Com)
        end
    end
end

---对相机拍摄区域进行截图
---@return UnityEngine.Texture2D 返回Texture2D对象
function UtilFunction.CameraCapture()
    local width, height = (Screen.width) / 2, (Screen.height) / 2
    local render = RenderTexture.New(width, height, -1);--创建一个RenderTexture对象

    local camera = Camera.main

    if camera == nil then
        camera = UIManager.UICamera
    end
    camera.targetTexture = render;--设置截图相机的targetTexture为render
    camera:Render();--手动开启截图相机的渲染

    RenderTexture.active = render;--激活RenderTexture
    local tex = Texture2D.New(width, height, TextureFormat.RGB24, false);	--新建一个Texture2D对象
    tex:ReadPixels(Rect.New(0, 0, width, height), 0, 0);--读取像素
    tex:Apply();--保存像素信息

    camera.targetTexture = nil;--重置截图相机的targetTexture
    RenderTexture.active = nil;--关闭RenderTexture的激活状态
    GameObject.Destroy(render);--删除RenderTexture对象


    --UI截屏
    local render = RenderTexture.New(width, height, -1);--创建一个RenderTexture对象

    local camera = UIManager.UICamera

    camera.targetTexture = render;--设置截图相机的targetTexture为render
    camera:Render();--手动开启截图相机的渲染

    RenderTexture.active = render;--激活RenderTexture

    local uitex = Texture2D.New(width, height, TextureFormat.RGB24, false);	--新建一个Texture2D对象
    uitex:ReadPixels(Rect.New(0, 0, width, height), 0, 0);--读取像素
    uitex:Apply();--保存像素信息

    for i = 0, width - 1 do
        for j = 0, height - 1 do
            tex:SetPixel(i, j, uitex:GetPixel(i, j));
        end
    end
    tex:Apply()

    camera.targetTexture = nil;--重置截图相机的targetTexture
    RenderTexture.active = nil;--关闭RenderTexture的激活状态
    GameObject.Destroy(render);--删除RenderTexture对象
    return tex
end