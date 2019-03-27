-- -----------------------------------------------------------------


-- *
-- * Filename:    MainLoginTourisTipController.lua
-- * Summary:     游客用户的提示
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/27/2017 2:53:22 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainLoginTourisTipController")



-- 界面名称
local wName = "MainLoginTourisTip"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("MainLoginTourisTip", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
