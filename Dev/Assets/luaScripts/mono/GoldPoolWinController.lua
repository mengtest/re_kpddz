-- -----------------------------------------------------------------


-- *
-- * Filename:    GoldPoolWinController.lua
-- * Summary:     GoldPoolWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        10/16/2017 2:38:49 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GoldPoolWinController")

local protobuf = sluaAux.luaProtobuf.getInstance();

-- 界面名称
local wName = "GoldPoolWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

M.ActInfo = nil
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end


UI.Controller.UIManager.RegisterLuaWinFunc("GoldPoolWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
