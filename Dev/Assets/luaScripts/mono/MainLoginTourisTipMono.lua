-- -----------------------------------------------------------------


-- *
-- * Filename:    MainLoginTourisTipMono.lua
-- * Summary:     游客用户的提示
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/27/2017 2:53:22 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainLoginTourisTipMono")



-- 界面名称
local wName = "MainLoginTourisTip"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _contentLb
local _cancelButton
local _oKButton
--- [ALD END]





local function OnCancelHandler(gameObject)
    UnityTools.DestroyWin(wName);
end

local function OnOkHandler(gameObject)
    --TODO 绑定手机
--    UnityTools.ShowMessage("功能开发中。。。。")
    UtilTools.BindingPhone();
    UnityTools.DestroyWin(wName);

end

--- [ALF END]





local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _contentLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Label")

    _cancelButton = UnityTools.FindGo(gameObject.transform, "Container/Grid/CancelButton")
    UnityTools.AddOnClick(_cancelButton.gameObject, OnCancelHandler)

    _oKButton = UnityTools.FindGo(gameObject.transform, "Container/Grid/OKButton")
    UnityTools.AddOnClick(_oKButton.gameObject, OnOkHandler)

--- [ALB END]




end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    _contentLb.text =LuaText.GetString("touris_login_tip");
    UnityTools.OpenAction(_winBg);
end


local function OnDestroy(gameObject)
    CLEAN_MODULE(wName.."Mono");
end





-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
