-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitSuperAwardWinController.lua
-- * Summary:     超级奖励
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/26/2018 10:20:45 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitSuperAwardWinController")



-- 界面名称
local wName = "FruitSuperAwardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("FruitSuperAwardWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
