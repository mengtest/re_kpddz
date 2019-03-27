-- -----------------------------------------------------------------


-- *
-- * Filename:    GuideWinController.lua
-- * Summary:     新手引导
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/10/2017 5:06:43 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GuideWinController")



-- 界面名称
local wName = "GuideWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("GuideWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
