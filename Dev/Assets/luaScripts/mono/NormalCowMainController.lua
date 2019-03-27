-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowMainController.lua
-- * Summary:     普通牛主界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/17/2017 10:07:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowMainController")



-- 界面名称
local wName = "NormalCowMain"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end


LogWarn("NormalCowMainController")

UI.Controller.UIManager.RegisterLuaWinFunc("NormalCowMain", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
