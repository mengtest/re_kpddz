-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareTipsWinController.lua
-- * Summary:     分享规则介绍
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/25/2017 5:15:23 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareTipsWinController")



-- 界面名称
local wName = "ShareTipsWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("ShareTipsWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
