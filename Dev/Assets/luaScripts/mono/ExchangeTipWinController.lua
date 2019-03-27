-- -----------------------------------------------------------------


-- *
-- * Filename:    ExchangeTipWinController.lua
-- * Summary:     ExchangeTipWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        12/15/2017 5:54:54 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ExchangeTipWinController")



-- 界面名称
local wName = "ExchangeTipWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("ExchangeTipWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
