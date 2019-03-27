-- -----------------------------------------------------------------


-- *
-- * Filename:    FirstPayWinController.lua
-- * Summary:     首充界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/14/2017 10:16:52 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FirstPayWinController")



-- 界面名称
local wName = "FirstPayWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("FirstPayWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
