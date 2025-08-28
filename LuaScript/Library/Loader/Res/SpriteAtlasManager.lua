---
--- 图集管理类
--- Created by hukiry.
--- DateTime: 2020/11/19 16:49
---

---@class SpriteAtlasManager
local SpriteAtlasManager = Class()

function SpriteAtlasManager:ctor()
	self:InitData()
end

function SpriteAtlasManager:InitData()
	---@type table<number, UnityEngine.U2D.SpriteAtlas>
	self.atlasList = {}
end

---同步加载
---@param path string 图集路径
---@param spriteName string 文件名路径
---@return UnityEngine.Sprite
function SpriteAtlasManager:LoadSprite(path, spriteName)
	if not self.atlasList[path] then
		---@type UnityEngine.U2D.SpriteAtlas
		local spriteAtlas = ResManager:Load(path)
		self.atlasList[path] = spriteAtlas
	end
	return self.atlasList[path]:GetSprite(spriteName)
end

---加载图集
---@param path string 图集路径
---@return UnityEngine.U2D.SpriteAtlas
function SpriteAtlasManager:LoadAtlas(path)
	if not self.atlasList[path] then
		---@type UnityEngine.U2D.SpriteAtlas
		local spriteAtlas = ResManager:Load(path)
		self.atlasList[path] = spriteAtlas
	end
	return self.atlasList[path]
end

---异步加载
---@param path string 图集路径
---@param spriteName string 文件名路径
---@param func function<UnityEngine.Sprite> 函数回调
function SpriteAtlasManager:LoadAnsycAtlas(path, spriteName, func)
	if self.atlasList[path] then
		if func then func(self.atlasList[path]:GetSprite(spriteName)) end
	else
		ResManager:LoadAsync(path, function(name, spriteAtlas)
			self.atlasList[name] = spriteAtlas
			if func then func(spriteAtlas:GetSprite(spriteName)) end
		end)
	end
end

---同步加载
---@param path string 图集路径
---@param spriteName string 精灵名
---@param img Hukiry.UI.AtlasImage
function SpriteAtlasManager:LoadAtlasImageSprite(img, path, spriteName)
	if not self.atlasList[path] then
		---@type UnityEngine.U2D.SpriteAtlas
		local spriteAtlas = ResManager:Load(path)
		self.atlasList[path] = spriteAtlas
	end
	img.spriteAtlas = self.atlasList[path]
	img.spriteName = spriteName
end

---卸载图集
---@param path string
function SpriteAtlasManager:UnloadAtlas(path)
	if self.atlasList[path] then
		ResManager:Unload(path)
		self.atlasList[path] = nil
	end
end

---卸载所有图集
function SpriteAtlasManager:UnloadAllAtlas()
	for k, v in pairs(self.atlasList) do
		self:UnloadAtlas(k)
	end
end

return SpriteAtlasManager