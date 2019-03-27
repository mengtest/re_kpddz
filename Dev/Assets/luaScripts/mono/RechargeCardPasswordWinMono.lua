-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargeCardPasswordWinMono.lua
-- * Summary:     提取卡密
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/21/2017 10:23:13 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeCardPasswordWinMono")



-- 界面名称
local wName = "RechargeCardPasswordWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _btnClose
local _btnSure
local _cardLb
local _passWordLb
local _cardLink
local _passwordLink
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnSureHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnCopyCardNum(gameObject)
    if UtilTools.CopyTextToPhone(_cardLb.text) then
        UnityTools.ShowMessage(LuaText.GetString("copyCardSucc"));
    end
end

local function OnCopyPassword(gameObject)

    if UtilTools.CopyTextToPhone(_passWordLb.text) then
        UnityTools.ShowMessage(LuaText.GetString("copyPsdSucc"));
    end
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

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnSureHandler)

    _cardLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/card/input")

    _passWordLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/password/input")

    _cardLink = UnityTools.FindGo(gameObject.transform, "Container/card/link")
    UnityTools.AddOnClick(_cardLink.gameObject, OnCopyCardNum)

    _passwordLink = UnityTools.FindGo(gameObject.transform, "Container/password/link")
    UnityTools.AddOnClick(_passwordLink.gameObject, OnCopyPassword)

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);
end


local function Start(gameObject)
    local cardNum, psd = CTRL.GetData();
    _cardLb.text = cardNum;
    _passWordLb.text = psd;

end


local function OnDestroy(gameObject)
    CLEAN_MODULE("RechargeCardPasswordWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
