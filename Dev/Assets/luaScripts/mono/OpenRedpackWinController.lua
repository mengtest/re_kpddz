-- -----------------------------------------------------------------


-- *
-- * Filename:    OpenRedpackWinController.lua
-- * Summary:     打开红包界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/15/2017 9:49:57 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("OpenRedpackWinController")



-- 界面名称
local wName = "OpenRedpackWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local redNum = 0
local openFun = nil

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    redNum = 0
    openFun = nil
end




UI.Controller.UIManager.RegisterLuaWinFunc("OpenRedpackWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
M.RedNum = redNum
M.OpenFun = openFun
return M
