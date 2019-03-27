-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitAwardWinController.lua
-- * Summary:     FruitAwardWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/18/2017 3:27:52 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitAwardWinController")



-- 界面名称
local wName = "FruitAwardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("FruitAwardWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
