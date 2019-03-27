-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargeQBWinController.lua
-- * Summary:     RechargeQBWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/5/2018 2:03:17 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeQBWinController")



-- 界面名称
local wName = "RechargeQBWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
M.obj_id = 0 
M.saveData = nil

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.saveData = nil
end




UI.Controller.UIManager.RegisterLuaWinFunc("RechargeQBWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
