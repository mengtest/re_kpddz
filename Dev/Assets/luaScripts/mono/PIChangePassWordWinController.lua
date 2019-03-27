-- -----------------------------------------------------------------


-- *
-- * Filename:    PIChangePassWordWinController.lua
-- * Summary:     重置密码
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/7/2017 2:51:04 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PIChangePassWordWinController")



-- 界面名称
local wName = "PIChangePassWordWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("PIChangePassWordWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
