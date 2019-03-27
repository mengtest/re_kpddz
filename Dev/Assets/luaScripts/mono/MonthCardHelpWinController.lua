-- -----------------------------------------------------------------


-- *
-- * Filename:    MonthCardHelpWinController.lua
-- * Summary:     月卡帮助界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/13/2017 11:04:07 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MonthCardHelpWinController")



-- 界面名称
local wName = "MonthCardHelpWin"
local UnityTools = IMPORT_MODULE("UnityTools")

-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


M.TipWord = ""
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.TipWord = ""
end

local function ShowTipWin(str)
    M.TipWord = str
    UnityTools.CreateLuaWin(wName)
end


UI.Controller.UIManager.RegisterLuaWinFunc("MonthCardHelpWin", OnCreateCallBack, OnDestoryCallBack)

M.ShowTipWin=ShowTipWin
-- 返回当前模块
return M
