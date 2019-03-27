-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RichCarRuleWinController.lua
-- * Summary:     RichCarRuleWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        5/3/2017 8:21:45 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RichCarRuleWinController")



-- 界面名称
local wName = "RichCarRuleWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("RichCarRuleWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
