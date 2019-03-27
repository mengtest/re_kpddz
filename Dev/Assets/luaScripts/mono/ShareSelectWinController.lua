-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareSelectWinController.lua
-- * Summary:     分享选择界面
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/18/2017 6:08:17 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareSelectWinController")



-- 界面名称
local wName = "ShareSelectWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("ShareSelectWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
