---
--- 从网络上下载图片流直接使用，不存储
--- Created by hukiry.
--- DateTime: 2018\7\2 19:50
---
---@class HttpTextureDownload
HttpTextureDownload = Class()

---@param pTexture UnityEngine.UI.RawImage 显示的texture
function HttpTextureDownload:ctor(pTexture)
    self.uiTexture = pTexture

    ---@type boolean 是否自适应图片大小
    self.isMakePixel = false
end

---开始加载
---@param path string 全路径
function HttpTextureDownload:Load(path)
    self.isHttpSuccess = nil
    if IsNil(self.uiTexture) then
        return
    end
    if self.path == path then
        return
    end
    self.path = path
    ---@type HttpTexture2D
    self.httpDownloadTexture =require("Network.Http.HttpTexture2D").New()
    self.httpDownloadTexture:Get(path, function(pPath, isSuccess, pTexture2d)
        self.isHttpSuccess = isSuccess
        if isSuccess then
            self:LoadSuccess(pPath, pTexture2d)
        end
    end)
end

---加载成功
---@private
function HttpTextureDownload:LoadSuccess(pPath, pTexture2d)
    if self.path == nil or IsNil(self.uiTexture) then
        return
    end
    self.uiTexture.mainTexture = pTexture2d
    if self.isMakePixel == true then
        self.uiTexture:MakePixelPerfect()
    end
end

---销毁
function HttpTextureDownload:OnDestroy()
    if self.httpDownloadTexture then
        if self.isHttpSuccess == nil then    ---表示还没有加载回来，则停止下载
            self.httpDownloadTexture:StopDownload()
        end
    end
    self.httpDownloadTexture = nil
    self.isHttpSuccess = nil
    self.path = nil
end