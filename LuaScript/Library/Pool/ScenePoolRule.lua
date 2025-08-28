---
---Update Time:2024-8-5
---Author:Hukiry
---

---@class SceneItemType
SceneItemType = {
	---@type SceneFlyItem
	SceneFlyItem = 1,
}

---@class ScenePoolRule
ScenePoolRule = {
	[SceneItemType.SceneFlyItem] = { luaClass = "Game/Scene/View/SceneFlyItem", resPath = "Prefab/Scene/SceneFlyItem"},
}

