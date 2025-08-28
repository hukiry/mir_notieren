---
--- 相机工具方法
--- Created huiry
--- DateTime: 2021/4/15 11:45
---

local widthRadio = 1920 / Screen.width
local heightRadio = 1080 / Screen.height

---@class CameraUtil
local CameraUtil = {}

---把屏幕坐标 转 世界坐标
---@param mousePosition UnityEngine.Vector3 屏幕坐标
---@return UnityEngine.Vector3 世界坐标
function CameraUtil.ScreenToWorldPoint(mousePosition)
	return Camera.main:ScreenToWorldPoint(mousePosition);
end

---把世界坐标 转 屏幕坐标
---@param worldPoint UnityEngine.Vector3 世界坐标
---@return UnityEngine.Vector3 屏幕坐标
function CameraUtil.WorldToScreenPoint(worldPoint)
	return Camera.main:WorldToScreenPoint(worldPoint);
end

---把屏幕坐标比例转换
---@param position UnityEngine.Vector3 屏幕坐标
---@return UnityEngine.Vector3
function CameraUtil.ScreenProportionToRadio(position)
	position.x = position.x * widthRadio
	position.y = position.y * heightRadio
	return position
end

---把世界坐标 转 视点坐标
---@param position UnityEngine.Vector3 世界坐标
---@return UnityEngine.Vector3 屏幕坐标
function CameraUtil.WorldToViewportPoint(position)
	return Camera.main:WorldToViewportPoint(position)
end

---把屏幕坐标 转 摄像点
---@param mousePosition UnityEngine.Vector3 屏幕坐标
---@return UnityEngine.Ray
function CameraUtil.ScreenPointToRay(mousePosition)
	return Camera.main:ScreenPointToRay(mousePosition)
end

---UGUI世界坐标 转 屏幕坐标
---@param worldPoint UnityEngine.Vector3
---@return UnityEngine.Vector3
function CameraUtil.WorldUGUIToScreenPoint(worldPoint)
	return UnityEngine.RectTransformUtility.WorldToScreenPoint(UIManager.UICamera, worldPoint);
end

---屏幕坐标 转 UGUI世界坐标点
---@param rectTransform UnityEngine.RectTransform
---@param screenPoint UnityEngine.Vector3
---@return UnityEngine.Vector3
function CameraUtil.ScreenPointToUGUIWorldPoint(rectTransform, screenPoint)
	local _ , worldPos= UnityEngine.RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,
			screenPoint, UIManager.UICamera, Vector3.zero);
	return worldPos
end

---世界坐标 转 UGUI世界坐标点
---@param worldPoint UnityEngine.Vector3 世界坐标
---@param rectTransform UnityEngine.RectTransform
---@return UnityEngine.Vector3
function CameraUtil.WorldToUGUIPoint(rectTransform, worldPoint)
	local screenPoint = CameraUtil.WorldToScreenPoint(worldPoint)
	local _ , worldPos= UnityEngine.RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,
			screenPoint, UIManager.UICamera, Vector3.zero);
	return worldPos
end

---UGUI世界坐标点 转 世界坐标
---@param worldPoint UnityEngine.Vector3 UGUI世界坐标点
---@return UnityEngine.Vector3
function CameraUtil.UGUIPointToWorld(worldPoint)
	local screenPoint = CameraUtil.WorldUGUIToScreenPoint(worldPoint)
	local  worldPos= CameraUtil.ScreenToWorldPoint(screenPoint)
	return worldPos
end

---屏幕坐标 转 某个RectTransform下的localPosition坐标
---@param rectTransform UnityEngine.RectTransform
---@param screenPoint UnityEngine.Vector3
---@return UnityEngine.Vector3
function CameraUtil.ScreenPointToUGUILocalPoint(rectTransform, screenPoint)
	local _ , worldPos= UnityEngine.RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
			screenPoint, UIManager.UICamera, Vector3.zero);
	return worldPos
end

---屏幕坐标是否在UGUI rectTransform 框内
---@param rectTransform UnityEngine.RectTransform
---@param screenPoint UnityEngine.Vector3
---@return boolean
function CameraUtil.RectangleContainsScreenPoint(rectTransform, screenPoint)
	return UnityEngine.RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint);
end

return CameraUtil