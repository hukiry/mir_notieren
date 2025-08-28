---
--- 从本地加载精灵 (图集或图片中)
--- Created by hukiry.
--- DateTime: 2021\4\19 19:50

Type_SpriteRenderer = "UnityEngine.SpriteRenderer"
Type_Image = "UnityEngine.UI.Image"
Type_RawImage = "UnityEngine.UI.RawImage"
Type_AtlasImage = "Hukiry.UI.AtlasImage"

---@class LocalSprite
LocalSprite = Class()

---@param container UnityEngine.SpriteRenderer | UnityEngine.UI.Image | UnityEngine.UI.RawImage|Hukiry.UI.AtlasImage
function LocalSprite:ctor(container)
    self.container = container
    ---类型名称
    self.containerType = self.container:GetType().FullName
    ---@type UnityEngine.Transform
    self.transform = container.transform
    ---@type UnityEngine.GameObject
    self.gameObject = container.gameObject
    ---@type boolean
    self.isloadingFinish = false    --只有加载完成的，才可以释放
    ---@type boolean
    self.isEnableCollider = true    --默认启动:建筑升级中，状态精灵不启动碰撞器
    ---@type boolean
    self.isAddCollider = true       --默认添加：荒地部分不需要碰撞器
end

---使用异步加载
---@param path string 资源路径
---@param spriteName string 精灵名:图集才传
---@param loadFinish function 回调函数
function LocalSprite:LoadAsync(path, spriteName, loadFinish)
    if not self:CheckAndUnload(path, spriteName) then return end
    self.path = path
    self.spriteName = spriteName
    self.spriteNameCopy = spriteName
    self.loadFinish = loadFinish
    if IsEmptyString(self.path) then
        self:LoadFinish(nil)
        return
    end

    self.isloadingFinish = false;
    if not IsEmptyString(spriteName) then --如果是图集
        Single.SpriteAtlas():LoadAnsycAtlas(path, spriteName, function(pObj)
            if self.path == nil or IsNil(self.container) then
                return
            end
            self:LoadFinish(pObj)
        end)
    else     --非图集
        if self.containerType == Type_RawImage then --是texture
            ResManager:LoadAsync(path, function(name, pObj)
                if self.path == nil or IsNil(self.container) then
                    ResManager:Unload(path)
                    return
                end
                if pObj then
                    self:LoadFinish(pObj)
                end
            end)
        else    --精灵
            ResManager:LoadAllAsync(path, function(name, pObj)
                if self.path == nil or IsNil(self.container) then
                    ResManager:Unload(path)
                    return
                end

                if pObj and tonumber(pObj.Length) > 0 then
                    self:LoadFinish(pObj[1])
                else
                    log(string.format("非图集加载图片失败；path:%s, spriteName:%s", path, spriteName))
                    self:LoadFinish(nil)
                end
            end)
        end
    end
    return self
end

---加载完成
function LocalSprite:LoadFinish(pSprite)
    self.isloadingFinish = true
    if self.containerType == Type_RawImage then
        self.container.texture = pSprite
    else
        self.container.sprite = pSprite
    end

    self:SetNativeSize(self.isNativeSize)
    if self.isAddCollider then
        self:ResetPolygonCollider2D(self.resetPolygonCollider2D)
        self:ResetBoxCollider2D(self.resetBoxCollider2D)
        self:EnabledBoxCollider2D(self.enabledBoxCollider2D)
    end
    self:LoadFinishExpand(pSprite)

    if self.loadFinish then self.loadFinish(self, pSprite ~= nil) end
end

function LocalSprite:LoadFinishExpand(pSprite)
end

---重置不规则碰撞盒
---@param reset boolean 重置
function LocalSprite:ResetPolygonCollider2D(reset)
    self.resetPolygonCollider2D = reset
    if reset and self.isloadingFinish then
        ---@type UnityEngine.PolygonCollider2D
        local collider = self.container.gameObject:GetComponent("PolygonCollider2D")
        if collider then
            GameObject.Destroy(collider)
        end
        local Collider = self.container.gameObject:AddComponent(typeof(UnityEngine.PolygonCollider2D))
        Collider.enabled = self.isEnableCollider
    end
end

---重置规则碰撞盒
---@param reset boolean 重置
function LocalSprite:ResetBoxCollider2D(reset)
    self.resetBoxCollider2D = reset
    if reset and self.isloadingFinish then
        ---@type UnityEngine.BoxCollider2D
        self.boxCollider = self.container.gameObject:GetComponent("BoxCollider2D")
        if self.boxCollider then
            GameObject.Destroy(self.boxCollider)
        end
        self.boxCollider = self.container.gameObject:AddComponent(typeof(UnityEngine.BoxCollider2D))
        self.boxCollider.enabled = self.isEnableCollider
    end
end

---移除规则碰撞盒
function LocalSprite:RemoveBoxCollider2D()
    if self.boxCollider then
        GameObject.Destroy(self.boxCollider)
        self.boxCollider = nil
    end
end

---移除不规则碰撞盒
function LocalSprite:RemovePolygonCollider2D()
    local collider = self.container.gameObject:GetComponent("PolygonCollider2D")
    if collider then
        GameObject.Destroy(collider)
    end
end

---启用或禁用规则碰撞盒
---@param enabled boolean 启用或禁用
function LocalSprite:EnabledBoxCollider2D(enabled)
    self.enabledBoxCollider2D = enabled
    if self.boxCollider and enabled ~= nil then
        self.boxCollider.enabled = enabled
    end
end

---设置自适应大小
---@param value boolean
function LocalSprite:SetNativeSize(value)
    self.isNativeSize = value
    if self.isNativeSize and self.containerType == Type_RawImage or self.containerType == Type_Image then
        self.container:SetNativeSize()
    end
end

---校验并释放资源
---@private
---@param path string 全路径
function LocalSprite:CheckAndUnload(path, spriteName)
    if IsNil(self.container) then
        return false
    end
    if self.path == path and self.spriteName == spriteName then
        if self.loadFinish then self.loadFinish(self, true) end
        return false
    else
        if self.path and IsEmptyString(self.spriteName) then
            ResManager:Unload(self.path, 1)
        end
    end
    return true
end

function LocalSprite:SetToEmpty()
    self:Unload()
    if self.containerType == Type_RawImage then
        self.container.mainTexture = nil
    else
        self.container.sprite = nil
    end
end

---卸载贴图
function LocalSprite:Unload()
    if self.path and IsEmptyString(self.spriteName) and self.isloadingFinish then
        ResManager:Unload(self.path)
    end
    self.isloadingFinish = false
    self.spriteName = nil
    self.path = nil
end

function LocalSprite:IsDestroy()
    return self.path == nil
end

---销毁
function LocalSprite:OnDestroy()
    self:Unload()
end