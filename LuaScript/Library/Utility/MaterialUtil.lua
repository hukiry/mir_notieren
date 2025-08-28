---
--- MaterialUtil       
--- Author : hukiry     
--- DateTime 2022/10/16 15:39   
---


---@class MaterialUtil
local MaterialUtil = {}

function MaterialUtil.GetRoleMaterial()
    local material = MaterialUtil.CreateMaterial("Texture/role", "Custom/UI/UISeqAnimate")
    material:SetFloat("_Rows", 2)
    material:SetFloat("_Cols", 4)
    material:SetFloat("_FrameCount", 8)
    material:SetFloat("_Speed", 10)
    material.name = "role"
    return material
end

---@param texName string  主贴图名称 _MainTex
---@param shaderName string shader名
function MaterialUtil.CreateMaterial(texName, shaderName)
    local tex = ResManager:Load(texName)
    local material = Material.New(Shader.Find(shaderName))
    if tex then
        material:SetTexture("_MainTex", tex)
    end
    return material
end

return MaterialUtil


