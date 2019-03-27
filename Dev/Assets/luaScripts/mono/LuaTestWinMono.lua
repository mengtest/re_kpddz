-- -----------------------------------------------------------------


-- *
-- * Filename:    LuaTestWinMono.lua
-- * Summary:     LuaTestWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/15/2017 2:05:53 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("LuaTestWinMono")



-- 界面名称
local wName = "LuaTestWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


local function Awake(gameObject)

end


local function Start(gameObject)

end


local function OnDestroy(gameObject)

end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end




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
