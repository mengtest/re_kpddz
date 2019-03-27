-- -----------------------------------------------------------------


-- *
-- * Filename:    FirstPayWinMono.lua
-- * Summary:     首充界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/14/2017 10:16:52 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FirstPayWinMono")



-- 界面名称
local wName = "FirstPayWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _winBg
local _btnBuy
local _btnClose
--- [ALD END]
local function OnBuyHandler(gameObject)
    _platformMgr.OpenPay(50001)
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnBuy = UnityTools.FindGo(gameObject.transform, "Container/Texture/btnBuy")
    UnityTools.AddOnClick(_btnBuy.gameObject, OnBuyHandler)

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("FirstPayWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
