-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargePhoneWinMono.lua
-- * Summary:     充值手机号
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/18/2017 11:33:59 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargePhoneWinMono")
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 界面名称
local wName = "RechargePhoneWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _btnClose
local _iconSpr
local _nameLb
local _descLb
local _btnSure
local _phoneInput
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnSureHandler(gameObject)
    if _phoneInput.value == "" then
        UnityTools.ShowMessage("addr_phone_empty");
        return;
    end

    if UnityTools.MatchPhone(_phoneInput.value) then

        local saveData = CTRL.GetSaveData();
        local req = {}
        req.phone_number = _phoneInput.value
        req.obj_id = saveData.obj_id;
        req.phone_card_charge_type = 1; --1表示直充
        req.address_id = 1;
        protobuf:sendMessage(protoIdSet.cs_prize_exchange_req, req);
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

    _iconSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/icon")

    _nameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/name")

    _descLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/desc")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnSureHandler)

    _phoneInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/phone/inputBg")

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);

end


local function Start(gameObject)
    _phoneInput.defaultText = LuaText.GetString("addr_phone_empty");
    local saveData = CTRL.GetSaveData();
    _iconSpr.spriteName = saveData.icon;
    _nameLb.text = saveData.name;
    _descLb.text = saveData.dsc;
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("RechargePhoneWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
