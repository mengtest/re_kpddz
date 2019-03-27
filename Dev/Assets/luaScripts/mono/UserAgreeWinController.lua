-- -----------------------------------------------------------------


-- *
-- * Filename:    UserAgreeWinController.lua
-- * Summary:     UserAgreeWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/16/2018 10:50:23 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("UserAgreeWinController")



-- 界面名称
local wName = "UserAgreeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("UserAgreeWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
