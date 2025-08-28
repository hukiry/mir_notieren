---
--- UI模型控制器
--- Created by hukiry.
--- DateTime: 2020-08-25 16:57
---

---@class UIModelController:ModelController
UIModelController = Class(ModelController)

--模型索引,用来计算偏移
local ModelIndex = 0

---@param uiTexture UITexture 要接收的texture
---@param size number 画布大小（必须是2的幂，256,512,1024）
---@param templatePath string 模版资源路径
function UIModelController:ctor(uiTexture, size, templatePath)
    self.texture = uiTexture
    self.size = size

    ModelIndex = ModelIndex + 1

    --这个实例化可以考虑用对象池
    ---@type UnityEngine.GameObject
    self.gameObject = GameObject.Instantiate(ResManager:Load(templatePath))
    self.transform = self.gameObject.transform

    GameObject.DontDestroyOnLoad(self.gameObject)
    self.camera = self.transform:Find("Camera"):GetComponent("Camera")
    self.renderTexture = UnityEngine.RenderTexture.New(self.size, self.size, 16)
    self.renderTexture.name = "ModelTexture"
    self.camera.targetTexture = self.renderTexture
    self.texture.mainTexture = self.camera.targetTexture
    self.transform.localPosition = Vector3.New(ModelIndex * 10, 2000, 0)

    self.parentGo = self.transform:Find("ModelPos").gameObject
end