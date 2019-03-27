-- -----------------------------------------------------------------


-- *
-- * Filename:    HundredCowMainController.lua
-- * Summary:     百人牛牛
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/28/2017 10:36:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("HundredCowMainController")



-- 界面名称
local wName = "HundredCowMain"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


local protobuf = sluaAux.luaProtobuf.getInstance()
local function OnCreateCallBack(gameObject)
    gameObject:SetActive(true)
end
M.TaskTable = {}

local function OnDestoryCallBack(gameObject)

end
UI.Controller.UIManager.RegisterLuaWinFunc("HundredCowMain", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
