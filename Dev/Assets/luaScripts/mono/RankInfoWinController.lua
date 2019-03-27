-- -----------------------------------------------------------------


-- *
-- * Filename:    RankInfoWinController.lua
-- * Summary:     RankInfoWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        7/7/2017 9:53:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RankInfoWinController")


local UnityTools = IMPORT_MODULE("UnityTools")
-- 界面名称
local wName = "RankInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

M.WinType = 1

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.WinType = 1
end
local function OpenWinByType(type)
    M.WinType = type
    UnityTools.CreateLuaWin("RankInfoWin")
end



UI.Controller.UIManager.RegisterLuaWinFunc("RankInfoWin", OnCreateCallBack, OnDestoryCallBack)

M.OpenWinByType = OpenWinByType
-- 返回当前模块
return M
