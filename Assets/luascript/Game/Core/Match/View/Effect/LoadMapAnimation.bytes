---
--- LoadMapAnimation       
--- Author  then hukiry     
--- DateTime 2024/7/16 15 then46   
---

---@class LoadMapAnimation
local LoadMapAnimation = Class()
local scale = 12;
local offsetX = 0;
local offsetY = 1;
local delayOffset = 5;
local delayCount = 4;
function LoadMapAnimation:ctor(matchView, backView)
    ---@type MatchView
    self.matchView = matchView
    ---@type MatchBackgroundView
    self.backView = backView
    self.Count = 1;
    self.vx = 0;
    self.vy = 0;
    self.vz = 0;
    ---生成点
    ---@type table<number, UnityEngine.Vector3>
    self.points = {}
    ---变化点
    ---@type table<number, UnityEngine.Vector3>
    self.tpoint = {}
    ---所有对象
    ---@type table<number, UnityEngine.Transform>
    self.balls={}

    self.waitTime = 0;
    self.beforeTime=0;
    self.frameCount = 0;
    self.tabAni = {0, 1,2,3,10,12,4,15}
    self.current = self:GetRandomIndex()
end

---@private
---@return number
function LoadMapAnimation:GetRandomIndex()
    local index = math.random(1,#self.tabAni)
    return self.tabAni[index]
end

function LoadMapAnimation:OnEnable()
    self:LoadView()
    Single.TimerManger():RemoveHandler(self)

    self.len = table.length(self.balls)

    self.Count = 1;

    self.current = self.current%6+1

    self:MakeShape(self.tabAni[self.current])
    for i = 0, self.len-1 do
        local p = self.points[i]
        self.tpoint[i] = p
        self.balls[i+1].transform.localScale = Vector3.New(0.3,0.3,0.3)
        self.balls[i+1].transform.localPosition = Vector3.New(p.x, p.y, 0);
    end
    self.waitTime = 0
    self.beforeTime = UnityEngine.Time.realtimeSinceStartup;
    self.backView.gameFrameBack:SetActive(false)
    self.gameFrameScale = self.backView.gameFrame.localScale
    self.backView.gameFrame.localScale = Vector3.New(15,15,15)
    self.backView.effectGo:SetActive(true)
    Single.TimerManger():DoFrame(self, self.Timer, 1, -1)
end

---加载地图视图
---@private
function LoadMapAnimation:LoadView()
    for _, vTab in pairs(self.matchView.itemList) do
        for _, v in pairs(vTab) do
            local t = {}
            t.transform = v.transform
            t.pos = v.info:GetWorldPos()
            table.insert(self.balls, t)
        end
    end

    for _, vTab in pairs(self.matchView.itemBottomList) do
        for _, v in pairs(vTab) do
            local t = {}
            t.transform = v.transform
            t.pos = v.info:GetWorldPos()
            table.insert(self.balls, t)
        end
    end

    for _, vTab in pairs(self.matchView.itemFloatList) do
        for _, v in pairs(vTab) do
            local t = {}
            t.transform = v.transform
            t.pos = v.info:GetWorldPos()
            table.insert(self.balls, t)
        end
    end

    for _, v in pairs(self.backView.mapSprite) do
        local t = {}
        t.transform = v.transform
        t.pos =  v.transform.localPosition
        table.insert(self.balls, t)
    end
end
---@private
function LoadMapAnimation:Timer()
    if self.canvasGroup == nil then
        local win = UIManager:GetActiveWindow(ViewID.LevelMain)
        if win and win.canvasGroup then
            self.canvasGroup = win.canvasGroup
            self.canvasGroup.alpha = 0
        end
    end

    if self.beforeTime < UnityEngine.Time.realtimeSinceStartup then
        self.waitTime = self.waitTime + (UnityEngine.Time.realtimeSinceStartup -  self.beforeTime);
        self.beforeTime = UnityEngine.Time.realtimeSinceStartup;
    end

    if self.waitTime >= 2.5 then
        Single.TimerManger():RemoveHandler(self)
        local isTwo = math.random(0,1)==1
        if isTwo then
            self:FinishTwo()
        else
            self:Finish()
        end
        return
        --self.waitTime = 0;
        --self.current = self.current%6+1
        --log(self.tabAni[self.current],"yellow")
        --self:MakeShape(self.tabAni[self.current])
    end

    self:UpdateAnimation()
end

function LoadMapAnimation:FinishTwo()
    local v = table.remove(self.balls)
    v.transform:DOScale(Vector3.one, 0.1)
    v.transform:DOMove(v.pos, 0.1):OnComplete(Handle(self, self.FinishMove, 0, #self.balls))
end

function LoadMapAnimation:Finish()
    for i, v in ipairs(self.balls) do
        local pos = v.pos
        v.transform:DOScale(Vector3.one, 0.5)
        v.transform:DOMove(pos, 0.5)
    end

    self.backView.gameFrame.localScale = self.gameFrameScale
    self.canvasGroup:DOFade(1, 0.5):OnComplete(function()
        self.canvasGroup.alpha = 1
        self.canvasGroup = nil
        self.matchView:FinishLoadAnimation()
    end)

    StartCoroutine(function()
        WaitForSeconds(0.6)
        self:OnDisable()
    end)
end

function LoadMapAnimation:FinishMove(index, len)

    if index == len then
        self.backView.gameFrame.localScale = self.gameFrameScale
        self.canvasGroup:DOFade(1, 0.5):OnComplete(function()
            self.canvasGroup.alpha = 1
            self.canvasGroup = nil
            self.matchView:FinishLoadAnimation()
        end)

        StartCoroutine(function()
            WaitForSeconds(0.6)
            self:OnDisable()
        end)
    else

        local t=0.05
        local max = math.random(8,16)
        local nextIndex = index
        if index+max>len then
            max = len-index
        end

        for i = 1, max-1 do
            local v = table.remove(self.balls)
            nextIndex = nextIndex+1
            v.transform:DOScale(Vector3.one, t)
            v.transform:DOMove(v.pos, t)
        end

        local v = table.remove(self.balls)
        nextIndex = nextIndex+1
        v.transform:DOScale(Vector3.one, t)
        v.transform:DOMove(v.pos, t):OnComplete(Handle(self, self.FinishMove, nextIndex, len))
    end
end

---生成形状坐标点
---@private
---@param t number
function LoadMapAnimation:MakeShape(t)
    local xd = 0;
    if t==0 then
        for i = 0,  self.len-1 do
            xd = -90 + math.rad(math.random(0.0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(xd) * 10);
            local y = (math.cos(i * 360 / self.len) * 10) * (math.sin(i * 360 / self.len) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i] = Vector3.New(x, y, z);
        end
    elseif t== 1 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(t * 360 / self.len) * 10);
            local y = (math.cos(xd) * 10) * (math.sin(t * 360 / self.len) * 10);
            local z = math.sin(xd) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 2 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(t * 360 / self.len) * 10);
            local y = (math.cos(xd) * 10) * (math.sin(t * 360 / self.len) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 3 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(xd) * 10);
            local y = (math.cos(xd) * 10) * (math.sin(xd) * 10);
            local z = math.sin(xd) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 4 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(xd) * 10);
            local y = (math.cos(xd) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 5 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(xd) * 10);
            local y = (math.cos(i * 360 / self.len) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 6 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(i * 360 / self.len) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.cos(i * 360 / self.len) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 7 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(i * 360 / self.len) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.cos(i * 360 / self.len) * 10) * (math.sin(i * 360 / self.len) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 8 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.cos(i * 360 / self.len) * 10) * (math.sin(i * 360 / self.len) * 10);
            local z = math.sin(xd) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 9 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.cos(i * 360 / self.len) * 10) * (math.sin(xd) * 10);
            local z = math.sin(xd) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 10 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(i * 360 / self.len) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.cos(xd) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 11 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.sin(xd) * 10) * (math.sin(i * 360 / self.len) * 10);
            local z = math.sin(xd) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 12 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(xd) * 10);
            local y = (math.sin(xd) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 13 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.sin(i * 360 / self.len) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i] = Vector3.New(x, y, z);
        end
    elseif t== 14 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.sin(xd) * 10) * (math.cos(xd) * 10);
            local y = (math.sin(xd) * 10) * (math.sin(i * 360 / self.len) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 15 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(i * 360 / self.len) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.sin(i * 360 / self.len) * 10) * (math.sin(xd) * 10);
            local z = math.sin(i * 360 / self.len) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 16 then
        for i=0, self.len-1 do
            xd = -90 + math.rad(math.random(0, 180));
            local x = (math.cos(xd) * 10) * (math.cos(i * 360 / self.len) * 10);
            local y = (math.sin(i * 360 / self.len) * 10) * (math.sin(xd) * 10);
            local z = math.sin(xd) * 100;
            self.points[i]=Vector3.New(x, y, z);
        end
    elseif t== 17 then
        for i=0, self.len-1 do
            local x = -50 + math.rad(math.random(0, 100));
            self.points[i] = (Vector3.New(x, 0, 0));
        end
    end
end
---更新点坐标动画
---@private
function LoadMapAnimation:UpdateAnimation()
    local tx, ty, tz=0, 0, 0
    if self.Count < 200 then
        self.Count =self.Count + delayCount
    end

    self.vx = self.vx + 0.0075
    self.vy = self.vy + 0.0075
    self.vz = self.vz + 0.0075

    for i = 0, self.len-1 do
        local p = self.tpoint[i];
        if self.points[i].x > self.tpoint[i].x then
            p.x = p.x + delayOffset
        end
 
        if self.points[i].x < self.tpoint[i].x then
            p.x = p.x - delayOffset
        end
    
        if self.points[i].y > self.tpoint[i].y then
            p.y = p.y + delayOffset
        end
    
        if self.points[i].y < self.tpoint[i].y then
            p.y = p.y - delayOffset
        end
    
        if self.points[i].z > self.tpoint[i].z then
            p.z = p.z + delayOffset
        end
    
        if self.points[i].z < self.tpoint[i].z then
            p.z = p.z - delayOffset
        end

        self.tpoint[i] = p;
        ty = (p.y * math.cos(self.vx)) - (p.z * math.sin(self.vx));
        tz = (p.y * math.sin(self.vx)) + (p.z * math.cos(self.vx));
        tx = (p.x * math.cos(self.vy)) - (tz * math.sin(self.vy));
        tz = (p.x * math.sin(self.vy)) + (tz * math.cos(self.vy));

        tx = (tx * math.cos(self.vz)) - (ty * math.sin(self.vz));
        ty = (tx * math.sin(self.vz)) + (ty * math.cos(self.vz));

        local x = (scale * tx) / (self.Count - tz) + offsetX;
        local y = offsetY - (scale * ty) / (self.Count - tz);
        self.balls[i+1].transform.localPosition = Vector3.New(x, y, 0);
    end
end

function LoadMapAnimation:OnDisable()
    Single.TimerManger():RemoveHandler(self)
    self.backView.effectGo:SetActive(false)
    self.points = {}
    self.tpoint = {}
    self.balls={}
end

return LoadMapAnimation