-- -----------------------------------------------------------------


-- *
-- * Filename:    ActivityRulesController.lua
-- * Summary:     活动信息说明界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/25/2017 1:55:58 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ActivityRulesController")



-- 界面名称
local wName = "ActivityRules"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("ActivityRules", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
