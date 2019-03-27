-- -----------------------------------------------------------------


-- *
-- * Filename:    GoldPoolRuleWinController.lua
-- * Summary:     GoldPoolRuleWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        10/18/2017 11:41:40 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GoldPoolRuleWinController")



-- 界面名称
local wName = "GoldPoolRuleWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("GoldPoolRuleWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
