--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
require "misc.functions"
Mathf		= require "UnityEngine.Mathf"
Vector3 	= require "UnityEngine.Vector3"
Quaternion	= require "UnityEngine.Quaternion"
Vector2		= require "UnityEngine.Vector2"
Vector4		= require "UnityEngine.Vector4"
Color		= require "UnityEngine.Color"
Ray			= require "UnityEngine.Ray"
Bounds		= require "UnityEngine.Bounds"
RaycastHit	= require "UnityEngine.RaycastHit"
Touch		= require "UnityEngine.Touch"
LayerMask	= require "UnityEngine.LayerMask"
Plane		= require "UnityEngine.Plane"
Time		= reimport "UnityEngine.Time"

list		= require "list"
utf8		= require "misc.utf8"


require "event"
require "typeof"
require "slot"
require "System.Timer"
require "System.coroutine"
require "System.ValueType"
require "System.Reflection.BindingFlags"

---@type Ejson
json = require "cjson"
require "class"


---------------------net 取消buf协议---------------------
-- require "misc.strict"

---@class Ejson
Ejson ={
    ---转换成字符串
    ---@type function<table>
    encode = 1,
    ---将字符解析成表
    ---@type function<string>
    decode = 2
}

object			= System.Object
Type			= System.Type
Object          = UnityEngine.Object


---@type UnityEngine.RenderTexture
RenderTexture	= UnityEngine.RenderTexture
---@type UnityEngine.Texture2D
Texture2D	= UnityEngine.Texture2D
---@type UnityEngine.TextureFormat
TextureFormat	= UnityEngine.TextureFormat
---@type UnityEngine.Animation
Animation		= UnityEngine.Animation
---@type UnityEngine.AnimationClip
AnimationClip	= UnityEngine.AnimationClip
---@type UnityEngine.AnimationCurve
AnimationCurve  = UnityEngine.AnimationCurve
---@type UnityEngine.Sprite
Sprite			= UnityEngine.Sprite
---@type UnityEngine.SpriteRenderer
SpriteRenderer  = UnityEngine.SpriteRenderer
---@type UnityEngine.GameObject
GameObject 		= UnityEngine.GameObject
---@type UnityEngine.Transform
Transform 		= UnityEngine.Transform
---@type UnityEngine.Application
Application		= UnityEngine.Application
---@class UnityEngine.NetworkReachability
NetworkReachability		= UnityEngine.NetworkReachability
---@type UnityEngine.SystemInfo
SystemInfo		= UnityEngine.SystemInfo
---@type UnityEngine.Screen
Screen			= UnityEngine.Screen
---@type UnityEngine.Camera
Camera			= UnityEngine.Camera
---@type UnityEngine.Material
Material 		= UnityEngine.Material
---@type UnityEngine.Shader
Shader 		= UnityEngine.Shader
---@type UnityEngine.Color32
Color32			= UnityEngine.Color32
---@type UnityEngine.Renderer
Renderer 		= UnityEngine.Renderer
---@type UnityEngine.GL
GL				= UnityEngine.GL
--Mesh  			= UnityEngine.Mesh
---@type UnityEngine.Rect
Rect			= UnityEngine.Rect
---@type UnityEngine.BoxCollider
BoxCollider		= UnityEngine.BoxCollider
---@type UnityEngine.Keyframe
Keyframe		= UnityEngine.Keyframe

---@type UnityEngine.Input
Input			= UnityEngine.Input

---@type UnityEngine.KeyCode
KeyCode			= UnityEngine.KeyCode
---@type UnityEngine.Physics
Physics			= UnityEngine.Physics
---@type UnityEngine.Physics2D
Physics2D       = UnityEngine.Physics2D
---@type UnityEngine.MeshRenderer
MeshRenderer	= UnityEngine.MeshRenderer
---@type UnityEngine.Random
Random			= UnityEngine.Random
---@type UnityEngine.Texture2D
Texture2D = UnityEngine.Texture2D

---@type UnityEngine.EventSystems.PointerEventData
PointerEventData = UnityEngine.EventSystems.PointerEventData

---@type UnityEngine.EventSystems.EventSystem
EventSystem = UnityEngine.EventSystems.EventSystem
---@type UnityEngine.Networking.UnityWebRequest
UnityWebRequest = UnityEngine.Networking.UnityWebRequest
---@type UnityEngine.WWWForm
WWWForm = UnityEngine.WWWForm
---@type UnityEngine.Networking.UnityWebRequest
DownloadHandlerBuffer = UnityEngine.Networking.DownloadHandlerBuffer
---@type UnityEngine.Networking.UnityWebRequest
UploadHandlerRaw = UnityEngine.Networking.UploadHandlerRaw
---@type UnityEngine.UI.CanvasScaler
CanvasScaler =  UnityEngine.UI.CanvasScaler

---@type DG.Tweening.Ease
Ease = DG.Tweening.Ease
---@type DG.Tweening.LoopType
LoopType = DG.Tweening.LoopType
---@type DG.Tweening.PathType
PathType = DG.Tweening.PathType
---@type DG.Tweening.PathMode
PathMode = DG.Tweening.PathMode
---@type DG.Tweening.AxisConstraint
AxisConstraint = DG.Tweening.AxisConstraint
---@type DG.Tweening.LoopType
LoopType = DG.Tweening.LoopType

---@type UnityEngine.PlayerPrefs
PlayerPrefs = UnityEngine.PlayerPrefs

---@type Hukiry.Socket.NetManager
NetManager = Hukiry.Socket.NetManager

---开始协程
---@type function<function>
StartCoroutine = StartCoroutine
---等待返回
---@type function<UnityEngine.Coroutine>
Yield = Yield
---等待秒
---@type function<number>
WaitForSeconds = WaitForSeconds
---等待一帧
---@type function()
WaitForFixedUpdate = WaitForFixedUpdate
---等待渲染最后一帧结束
---@type function()
WaitForEndOfFrame = WaitForEndOfFrame
---停止协程
---@type function<function>
StopCoroutine = StopCoroutine

---@type UnityEngine.Vector2Int
Vector2Int = UnityEngine.Vector2Int


--有命名空间，需要添加【命名空间名.类名】 或者 类名
function GetClass(className)
    local ty = System.Type.GetType(className,false);
    if ty==nil then
        ty = System.Type.GetType(className..", UnityEngine.dll",false);
    end
    if ty==nil then
        ty = System.Type.GetType(className..", Assembly-CSharp.dll",false);
    end
    return ty;
end

