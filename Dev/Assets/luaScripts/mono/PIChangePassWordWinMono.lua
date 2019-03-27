-- -----------------------------------------------------------------


-- *
-- * Filename:    PIChangePassWordWinMono.lua
-- * Summary:     重置密码
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/7/2017 2:51:04 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PIChangePassWordWinMono")



-- 界面名称
local wName = "PIChangePassWordWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _btnClose
local _phoneInput
local _newPasswordInput
local _btnSure
local _btnGetVerify
local _btnGetVerifyGray
local _waitLb
local _verifyInput
--- [ALD END]










local function OnCloseHandler(gameObject)
end

local function OnSureChangePassword(gameObject)
end

local function OnGetVerify(gameObject)
end

local function OnGetVerifyGray(gameObject)
end

--- [ALF END]







local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _phoneInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/phone/Label")

    _newPasswordInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/newPassWord/Label")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnSureChangePassword)

    _btnGetVerify = UnityTools.FindGo(gameObject.transform, "Container/verify/btnGetVerify")
    UnityTools.AddOnClick(_btnGetVerify.gameObject, OnGetVerify)

    _btnGetVerifyGray = UnityTools.FindGo(gameObject.transform, "Container/verify/btnGetVerifyGray")
    UnityTools.AddOnClick(_btnGetVerifyGray.gameObject, OnGetVerifyGray)

    _waitLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/verify/btnGetVerifyGray/Label")

    _verifyInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/verify/Label")

--- [ALB END]









end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    _phoneInput.defaultText = LuaText.GetString("login_name_default");
    _newPasswordInput.defaultText = LuaText.GetString("reset_passWord_default");
    _verifyInput.defaultText = LuaText.GetString("verify_default");
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("PIChangePassWordWinMono")
end






-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
