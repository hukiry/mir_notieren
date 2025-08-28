
---@class UIWindowUtil
local UIWindowUtil = {}

UIWindowUtil.blackBgPrefab = nil
--UI窗口背景处理
---@return UnityEngine.GameObject
function UIWindowUtil.CreateWindowBg(panelLayer, panelBgMode, bgClickCallback)
	if panelBgMode == ViewBackgoundMode.None then
		return nil
	end

	--纯黑底背景
	local viewBg = UIWindowUtil.CreateBlackBg()
	viewBg:SetActive(true)
	---@type UnityEngine.CanvasGroup
	local bgSp = viewBg:GetComponent("CanvasGroup")
	if panelBgMode == ViewBackgoundMode.Collider or panelBgMode == ViewBackgoundMode.Collider_Event then
		bgSp.alpha = 0.7
	else
		bgSp.alpha = 0.01
	end

	--if panelBgMode == ViewBackgoundMode.Blur_Collider_Event or panelBgMode == ViewBackgoundMode.Collider_Event then
	UIWindowBase:AddClick(viewBg, bgClickCallback)
	--end
	return viewBg
end

--创建一个半透明背景
function UIWindowUtil.CreateBlackBg()
	if UIWindowUtil.blackBgPrefab == nil then
		UIWindowUtil.blackBgPrefab = ResManager:Load("ui/prefab/common/blackbgpanel")
	end
	return GameObject.Instantiate(UIWindowUtil.blackBgPrefab)
end

return UIWindowUtil