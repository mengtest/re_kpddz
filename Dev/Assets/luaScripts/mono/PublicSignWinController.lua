-- -----------------------------------------------------------------


-- *
-- * Filename:    PublicSignWinController.lua
-- * Summary:     公告界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/10/2017 11:08:45 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PublicSignWinController")



-- 界面名称
local wName = "PublicSignWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

g_announcementStr = ""
g_openAnnouncement = false

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

UI.Controller.UIManager.RegisterLuaWinFunc("PublicSignWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
