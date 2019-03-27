-- -----------------------------------------------------------------


-- *
-- * Filename:    FastAddGoldWinController.lua
-- * Summary:     补充金币
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/21/2017 4:49:38 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FastAddGoldWinController")



-- 界面名称
local wName = "FastAddGoldWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("FastAddGoldWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
