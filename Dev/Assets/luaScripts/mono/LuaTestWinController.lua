-- -----------------------------------------------------------------


-- *
-- * Filename:    LuaTestWinController.lua
-- * Summary:     LuaTestWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/15/2017 2:05:53 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("LuaTestWinController")



-- 界面名称
local wName = "LuaTestWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("LuaTestWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
