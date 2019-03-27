-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowUIMono.lua
-- * Summary:     看牌UI
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/9/2017 10:20:15 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowUIMono")



-- 界面名称
local wName = "NormalCowUI"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _flags={}
--- [ALD END]
local _go

--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    for i=1,5 do
        _flags[i] = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell"..i.."/flag"):GetComponent("UIPanel")
    end

--- [ALB END]

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    _go = gameObject
    AutoLuaBind(gameObject)

end
local function ResetRedWinRenderQ(go)
    if UnityTools.IsWinShow(wName) == false then return nil end
    local stRender=_go:GetComponent("UIPanel").startingRenderQueue
    for i=1,5 do
        _flags[i].startingRenderQueue = stRender+ 100
    end
    
end

local function Start(gameObject)
    triggerScriptEvent(NORMAL_COW_UI_WIN_START, gameObject)
    ResetRedWinRenderQ(nil)
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("NormalCowUIMono")
end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end

UI.Controller.UIManager.RegisterLuaWinRenderFunc("NormalCowUI", ResetRedWinRenderQ)



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable


-- 返回当前模块
return M
