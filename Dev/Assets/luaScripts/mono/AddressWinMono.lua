-- -----------------------------------------------------------------


-- *
-- * Filename:    AddressWinMono.lua
-- * Summary:     设置收货地址
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 4:27:15 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("AddressWinMono")



-- 界面名称
local wName = "AddressWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local protobuf = sluaAux.luaProtobuf.getInstance();


local _exchangeCtrl = IMPORT_MODULE("ExchangeWinController");


local _oldInfo;


local _winBg
local _btnClose
local _nameInputBg
local _cityInputBg
local _addrInputBg
local _phoneInputBg
local _btnSure
local _boyToggle
local _girlToggle
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnSureHandler(gameObject)
    local isChange = false;
    if _oldInfo == nil then
        isChange = true;
    end
    if _nameInputBg.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("addr_name_empty"));
        if _oldInfo ~= nil then
            if _nameInputBg.value ~= _oldInfo.name then
                isChange = true;
            end
        end
        return;
    end

    if _cityInputBg.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("addr_city_empty"));
        if _oldInfo ~= nil then
            if _cityInputBg.value ~= _oldInfo.city_name then
                isChange = true;
            end
        end
        return;
    end

    if _addrInputBg.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("addr_addr_empty"));

        if _oldInfo ~= nil then
            if _addrInputBg.value ~= _oldInfo.address then
                isChange = true;
            end
        end
        return;
    end

    if _phoneInputBg.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("addr_phone_empty"));
        if _oldInfo ~= nil then
            if _phoneInputBg.value ~= _oldInfo.phone_number then
                isChange = true;
            end
        end
        return;
    end

    if UnityTools.MatchPhone(_phoneInputBg.value) == false then
        UnityTools.ShowMessage(LuaText.GetString("addr_phone_check"));
        return;
    end
    --text = GameText.Instance:StrFilter(text, 42)
    --- TODO 接入奖品兑换
    -- UnityTools.ShowMessage("功能开发中...");
    local addrObj = {}
    addrObj.id = 1;
    addrObj.name = _nameInputBg.value;
    addrObj.city_name = _cityInputBg.value;
    addrObj.address = _addrInputBg.value;
    addrObj.address = _addrInputBg.value;
    addrObj.phone_number = _phoneInputBg.value;
    addrObj.province_name = "";
    if _boyToggle.value then
        addrObj.sex = 1;
    else
        addrObj.sex = 2;
    end
    local req = {}
    req.new_info = addrObj;
    protobuf:sendMessage(protoIdSet.cs_prize_address_change_req, req);
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

    _nameInputBg = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/name/inputBg")

    _cityInputBg = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/city/inputBg")

    _addrInputBg = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/addr/inputBg")

    _phoneInputBg = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/phone/inputBg")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnSureHandler)

    _boyToggle = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/sex/toggle1")

    _girlToggle = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/sex/toggle2")

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);
end


local function Start(gameObject)
    local haveAddress = _exchangeCtrl.IsSetDefaultList();
    if haveAddress then
        local info = _exchangeCtrl.GetDefaultAddress();
        _oldInfo = info;
        _nameInputBg.value = info.name;
        _cityInputBg.value = info.city_name;
        _addrInputBg.value = info.address;
        _phoneInputBg.value = info.phone_number;
        _boyToggle.value = info.sex == 1;
        _girlToggle.value = info.sex == 2;
    else
        _nameInputBg.defaultText = LuaText.GetString("addr_name_empty");
        _cityInputBg.defaultText = LuaText.GetString("addr_city_empty");
        _addrInputBg.defaultText = LuaText.GetString("addr_addr_empty");
        _phoneInputBg.defaultText = LuaText.GetString("addr_phone_empty");
    end
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("AddressWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
