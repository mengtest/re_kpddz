-- -----------------------------------------------------------------


-- *
-- * Filename:    GameCenterWinController.lua
-- * Summary:     GameCenterWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        7/14/2017 4:00:11 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GameCenterWinController")



-- 界面名称
local wName = "GameCenterWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local UnityTools = IMPORT_MODULE("UnityTools");
M.OpenType=1

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.OpenType=1
end

local function ResetWinRenderQ(go)
    triggerScriptEvent(EVENT_GAMECENTERR_ENDER_CHANGE_WIN,wName)
end
function M.OpenWinByType(openType)
    M.OpenType=openType
    UnityTools.CreateLuaWin("GameCenterWin")
end

UI.Controller.UIManager.RegisterLuaWinFunc("GameCenterWin", OnCreateCallBack, OnDestoryCallBack)

UI.Controller.UIManager.RegisterLuaWinRenderFunc(wName, ResetWinRenderQ)
-- 返回当前模块
return M
