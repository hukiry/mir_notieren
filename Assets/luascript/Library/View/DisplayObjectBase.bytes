
---@class ETriggerType
ETriggerType = {
	PointerEnter = 0,
	PointerExit = 1,
	PointerDown = 2,
	PointerUp = 3,
	PointerClick = 4,
	Drag = 5,
	Drop = 6,
	Scroll = 7,
	UpdateSelected = 8,
	Select = 9,
	Deselect = 10,
	Move = 11,
	InitializePotentialDrag = 12,
	BeginDrag = 13,
	EndDrag = 14,
	Submit = 15,
	Cancel = 16
}

---@class DisplayObjectBase
DisplayObjectBase = Class()
---@param gameObject UnityEngine.GameObject
function DisplayObjectBase:ctor(gameObject)
	if type(gameObject) == "userdata" and gameObject ~= nil then
		self:SetGameObject(gameObject)
	end
end

function DisplayObjectBase:SetGameObject(gameObject)
	---@type UnityEngine.GameObject
	self.gameObject = gameObject;
	if gameObject ~= nil  then
		---@type UnityEngine.Transform
		self.transform = gameObject.transform
		---@type UnityEngine.RectTransform
		self.rectTransform = self.transform
		---窗口对象
		---@type UnityEngine.CanvasGroup
		self.canvasGroup = self.gameObject:GetComponent("CanvasGroup")
	end
end

---锚点全屏居中:UI
function DisplayObjectBase:SetAnchorFull()
	if self.rectTransform then
		self.rectTransform.anchorMin = Vector2.zero;
		self.rectTransform.anchorMax = Vector2.one;
		self.rectTransform.sizeDelta = Vector2.zero;
	end
end

---@param namePath string
---@return UnityEngine.GameObject
function DisplayObjectBase:FindGameObject(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf.gameObject end
	return nil
end

---@return UnityEngine.Transform| UnityEngine.RectTransform
function DisplayObjectBase:FindTransform(namePath)
	return self.transform:Find(namePath)
end

---@param namePath string
---@return Hukiry.UI.AtlasImage
function DisplayObjectBase:FindAtlasImage(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("AtlasImage") end
	return nil
end

---@param namePath string
---@return Hukiry.UI.MeshGraphic
function DisplayObjectBase:FindMeshGraphic(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("MeshGraphic") end
	return nil
end

---@param namePath string
---@return UnityEngine.UI.ContentSizeFitter
function DisplayObjectBase:FindContentSizeFitter(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("ContentSizeFitter") end
	return nil
end

---@param namePath string
---@return Hukiry.UI.UIProgressbarMask
function DisplayObjectBase:FindProgressbarMask(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("UIProgressbarMask") end
	return nil
end

---@param namePath string
---@return UnityEngine.UI.Text
function DisplayObjectBase:FindText(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("Text") end
	return nil
end

---@param namePath string
---@return UnityEngine.UI.ScrollRect
function DisplayObjectBase:FindScrollRect(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("ScrollRect") end
	return nil
end

---@param namePath string
---@return Hukiry.HukirySupperText
function DisplayObjectBase:FindHukirySupperText(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("HukirySupperText") end
	return nil
end

---@param namePath string
---@return UnityEngine.UI.InputField
function DisplayObjectBase:FindInputField(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("InputField") end
	return nil
end

---@param namePath string
---@return UnityEngine.UI.RawImage
function DisplayObjectBase:FindRawImage(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("RawImage") end
	return nil
end

---@param namePath string
---@return UnityEngine.SpriteRenderer
function DisplayObjectBase:FindSpriteRenderer(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("SpriteRenderer") end
	return nil
end

---@param namePath string
---@return UnityEngine.TextMesh
function DisplayObjectBase:FindTextMesh(namePath)
	local tf = self.transform:Find(namePath)
	if tf~=nil then return tf:GetComponent("TextMesh") end
	return nil
end

--从自身获取某个组件
function DisplayObjectBase:GetComponent(classType)
	if classType then
		return self.transform:GetComponent(classType)
	end
end

---添加点击按钮事件：普通通用
---@param go UnityEngine.GameObject
---@param func function<UnityEngine.GameObject> 函数回调
---@param isAddScale boolean
function DisplayObjectBase:AddClick(go, func, isAddScale, isNotPlaySound, isPlayCustom)
	UIEventListener.Get(go).onClick = function(...)
		if isPlayCustom then
			Single.Sound():PlaySound(ESoundResType.ClickPage)
		elseif not isNotPlaySound then
			Single.Sound():PlaySound(ESoundResType.ClickButton)
		end

		func(...)
	end

	if isAddScale then
		UIEventListener.Get(go):AddUIScale()
	end
end

---添加按下按钮事件：普通通用
function DisplayObjectBase:AddClickDown(go, func, isAddScale)
	UIEventListener.Get(go).onClickDown = function(...)
		func(...)
	end

	if isAddScale then
		UIEventListener.Get(go):AddUIScale()
	end
end

---添加按钮事件:滚动视图内用
---@param go UnityEngine.GameObject
---@param func function<UnityEngine.GameObject> 函数回调
function DisplayObjectBase:AddButtonClick(go, func, isAddScale)
	UIButtonListener.Get(go).onClickExtand = function(...)
		func(...)
	end

	if isAddScale then
		UIButtonListener.Get(go):AddUIScale()
	end
end

---执行按钮事件回调
---@param gameObject UnityEngine.GameObject
function DisplayObjectBase:ExecuteClickHandle(gameObject)
	UIEventListener.ExecuteEvent(gameObject, ETriggerType.PointerClick)
end

---@private
function DisplayObjectBase:_GetFileRoleKeyName(keyName)
	--..Single.Role().roleId
	return keyName
end

---读取二进制字节表
---@param msgTable table 字节表结构
---@return table 返回已经读取好的数据
function DisplayObjectBase:ReadBinaryTable(keyName, msgTable)
	---@type BaseProtobuf
	local msg = protobuf.ConvertMessage(msgTable)
	local fileName = self:_GetFileRoleKeyName(keyName)
	Single.Binary():ReadBinary(fileName,function(byteBlock)
		msg:ParseFromString(byteBlock)
	end)
	return msg
end

---保存二进制字节表
---@param keyName string 文件名
---@param msg table 字节表结构
function DisplayObjectBase:SaveBinaryTable(keyName, msg)
	local fileName = self:_GetFileRoleKeyName(keyName)
	---@param byteBlock Hukiry.Socket.ByteBlock
	Single.Binary():SaveBinary(fileName,function(byteBlock)
		msg:SerializeToString(byteBlock)
		--local tempBuffer, len = byteBlock.Buffer, byteBlock.Length
		--local url = SingleData.Login():GetOssUrl(fileName)
		--PutAliyunSdk.PutUploadUrl(url, tempBuffer, len)
	end)
end

---读取oss云数据字节
---@param keyName string
---@param msgTable table 字节表结构
---@param callFunc function<boolean, table>
---@return table 返回已经读取好的数据
function DisplayObjectBase:GetCloudOSS(keyName, msgTable, callFunc)
	local fileName = self:_GetFileRoleKeyName(keyName)
	local ossUrl = SingleData.Login():GetOssUrl(fileName, true)
	local msg = protobuf.ConvertMessage(msgTable)
	Single.Http():HttpGetBytes(ossUrl,  function(isOk, byteBlock)
		if isOk then
			msg:ParseFromString(byteBlock)
		end
		callFunc(isOk, msg)
	end)
end

---删除oss云数据字节
---@param keyName string 文件名
function DisplayObjectBase:DeleteCloudOSS(keyName)
	local fileName = self:_GetFileRoleKeyName(keyName)
	Single.Binary():DeleteBinary(fileName)

	local url = SingleData.Login():GetOssUrl(fileName)
	PutAliyunSdk.DeleteUploadUrl(url)
end
