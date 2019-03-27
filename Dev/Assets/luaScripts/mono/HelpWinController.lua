-- -----------------------------------------------------------------


-- *
-- * Filename:    HelpWinController.lua
-- * Summary:     帮助文档
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/29/2017 6:21:40 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("HelpWinController")
local UnityTools = IMPORT_MODULE("UnityTools");

-- 界面名称
local wName = "HelpWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


local openFrom = { from = "Game", tabNum = 3 }

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

function openFrom:ResetData()
    self.from = "Game"
    self.tabNum = 3;
end

function OpenHelp(from, tabNum)
    from = from or "Game";
    local ctrl = IMPORT_MODULE("HelpWinController")
    ctrl.OpenFrom.tabNum = tabNum or 3;
    ctrl.OpenFrom.from = from or "Game";
--    ctrl.OpenForm:SetData(f)
    UnityTools.CreateLuaWin("HelpWin");
end


UI.Controller.UIManager.RegisterLuaWinFunc("HelpWin", OnCreateCallBack, OnDestoryCallBack)

M.OpenFrom = openFrom;

-- 返回当前模块
return M
