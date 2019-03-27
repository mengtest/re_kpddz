-- -----------------------------------------------------------------


-- *
-- * Filename:    UserAgreeWinMono.lua
-- * Summary:     UserAgreeWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/16/2018 10:50:23 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("UserAgreeWinMono")



-- 界面名称
local wName = "UserAgreeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _scrollview
local _btnSure
local _btnBack
--- [ALD END]




--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _contentText = UnityTools.FindGo(gameObject.transform, "Container/bg/ScrollView/contentText"):GetComponent("UILabel")
    _scrollview = UnityTools.FindGo(gameObject.transform, "Container/bg/ScrollView")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, CloseWin)

    _btnBack = UnityTools.FindGo(gameObject.transform, "Container/Back")
    UnityTools.AddOnClick(_btnBack.gameObject, CloseWin)
    _contentText.text = GameText.GetStr("login_win_tip3")
--- [ALB END]



end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("UserAgreeWinMono")
end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable


-- 返回当前模块
return M
