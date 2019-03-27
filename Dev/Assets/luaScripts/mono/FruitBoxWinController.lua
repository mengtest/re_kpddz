-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitBoxWinController.lua
-- * Summary:     FruitBoxWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        4/7/2017 3:44:19 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitBoxWinController")



-- 界面名称
local wName = "FruitBoxWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("FruitBoxWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
