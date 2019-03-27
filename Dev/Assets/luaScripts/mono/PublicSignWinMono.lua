-- -----------------------------------------------------------------


-- *
-- * Filename:    PublicSignWinMono.lua
-- * Summary:     公告界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/10/2017 11:08:45 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PublicSignWinMono")



-- 界面名称
local wName = "PublicSignWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _scrollView
local _content
local _btnClose
--- [ALD END]


--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _scrollView = UnityTools.FindGo(gameObject.transform, "Container/dragBg/ScrollView")
    _controller:SetScrollViewRenderQueue(_scrollView)
    _content = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/dragBg/ScrollView/Grid/cell/desc")
    _content.text = g_announcementStr
    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)
    
--- [ALB END]



end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)

end


local function OnDestroy(gameObject)
    CLEAN_MODULE("PublicSignWinMono")
end




function SetPublicSignContent(text)
    _content.text = text
end

-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
