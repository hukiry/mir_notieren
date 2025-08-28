---
--- 建筑精灵图片
--- Created by qjh.
--- DateTime: 2021\4\17 19:50
---
---@class CommonSprite:LocalSprite
CommonSprite = Class(LocalSprite)

---@param container UnityEngine.SpriteRenderer | UnityEngine.UI.Image
---@param showSize UnityEngine.Vector2 显示的适配区域
---@param isNativeSize boolean 是否自适应大小,只有UI.Image有效
function CommonSprite:ctor(container, showSize, isNativeSize)
    self.container = container
    self.showSize = showSize
    self.isNativeSize = isNativeSize

    self.container.color = Color.white
end

---异步加载精灵
---@param spriteName string 精灵名字:场景图集需要导出
---@param atlasPath EAtlasResPath 如果不是图集，需要填写完整的图片路径
function CommonSprite:LoadSprite(spriteName, atlasPath, callback)
    self.callback = callback
    atlasPath = ESpriteAtlasResource[spriteName] or atlasPath
    self:LoadAsync(atlasPath, spriteName, Handle(self, self._LoadFinish, atlasPath, spriteName))
end

function CommonSprite:_LoadFinish(atlasPath, spriteName, isSuccess)
    if not isSuccess then
        logError("LocalSprite资源加载失败, atlasPath:", atlasPath,"spriteName:", spriteName)
    else
        if self.callback then
            self.callback()
        end
    end
end

function CommonSprite:UpdateMaterial(mat)
    self.container.material = mat
end

---加载完成扩展方法
function CommonSprite:LoadFinishExpand(pSprite)
    if self.showSize then
        if self.containerType ~= Type_SpriteRenderer then
            UtilFunction.SetUIAdaptionSize(self.container, self.showSize)
        elseif self.containerType == Type_SpriteRenderer then
            UtilFunction.SetSceneAdaption(self.container, self.showSize)
        end
    end
end

---播放透明动画
---@param duration number 播放时间
---@param finishCall function
function CommonSprite:PlayColor(duration, finishCall)
    self.container:DOKill()
    self.container:DOColor(Color.New(0,0,0,0),duration):OnComplete(finishCall)
end
