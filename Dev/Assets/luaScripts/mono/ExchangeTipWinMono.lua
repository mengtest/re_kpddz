-- -----------------------------------------------------------------


-- *
-- * Filename:    ExchangeTipWinMono.lua
-- * Summary:     ExchangeTipWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        12/15/2017 5:54:54 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ExchangeTipWinMono")



-- 界面名称
local wName = "ExchangeTipWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnOk
local _close
--- [ALD END]



--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnOk = UnityTools.FindGo(gameObject.transform, "WinBox/OKButton")
    UnityTools.AddOnClick(_btnOk.gameObject, CloseWin)

    _close = UnityTools.FindGo(gameObject.transform, "Texture")
    UnityTools.AddOnClick(_close.gameObject, CloseWin)

--- [ALB END]


end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)

end


local function OnDestroy(gameObject)
    CLEAN_MODULE("ExchangeTipWinMono")
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
