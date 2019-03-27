-- -----------------------------------------------------------------


-- *
-- * Filename:    SettingWinController.lua
-- * Summary:     设置界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/9/2017 2:28:57 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("SettingWinController")



-- 界面名称
local wName = "SettingWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("SettingWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
