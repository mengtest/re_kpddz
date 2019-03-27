-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerPicSelectWinController.lua
-- * Summary:     选择头像来源
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/3/2017 5:07:40 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerPicSelectWinController")



-- 界面名称
local wName = "PlayerPicSelectWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("PlayerPicSelectWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
