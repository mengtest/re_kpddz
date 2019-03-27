-- -----------------------------------------------------------------


-- *
-- * Filename:    ResultLayerController.lua
-- * Summary:     结算界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/6/2017 2:45:19 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ResultLayerController")



-- 界面名称
local wName = "ResultLayer"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _isBoth = false

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    _isBoth = false
end




UI.Controller.UIManager.RegisterLuaWinFunc("ResultLayer", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
M.IsBoth = _isBoth
return M
