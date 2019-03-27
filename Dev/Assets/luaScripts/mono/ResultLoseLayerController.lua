-- -----------------------------------------------------------------


-- *
-- * Filename:    ResultLoseLayerController.lua
-- * Summary:     失败结算界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/8/2017 1:32:36 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ResultLoseLayerController")



-- 界面名称
local wName = "ResultLoseLayer"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _isBoth = false

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    _isBoth = false
end




UI.Controller.UIManager.RegisterLuaWinFunc("ResultLoseLayer", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
M.IsBoth = _isBoth
return M
