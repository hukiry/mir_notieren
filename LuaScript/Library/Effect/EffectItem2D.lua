
---@class EffectItem2D:EffectBase
EffectItem2D = Class(EffectBase)

---@param parentGo UnityEngine.GameObject 父对象
---@param pEffName string 特效名字
function EffectItem2D:ctor(parentGo, pEffName)

end

---同步激活/播放
---@param playerFinishFunc function 播放完成回调
function EffectItem2D:Play(playerFinishFunc)
	if IsNil(self.parentGo) then
		logError("播放特效时，父对象已经为空，请检查")
		self:OnDestroy()
		return
	end
	EffectBase.Play(self, playerFinishFunc)
end

---异步激活/播放
---@param playerFinishFunc function 播放完成回调
---@param loadFinishFunc function 加载完成回调
function EffectItem2D:PlayAsync(playerFinishFunc, loadFinishFunc)
	if IsNil(self.parentGo) then
		logError("播放特效时，父对象已经为空，请检查")
		self:OnDestroy()
		return
	end
	EffectBase.PlayAsync(self, playerFinishFunc, loadFinishFunc)
end

---加载完成后初始化结束
function EffectItem2D:LoadFinishInitEnd()
end