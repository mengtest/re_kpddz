-- -----------------------------------------------------------------


-- *
-- * Filename:    ExchangeSureWinMono.lua
-- * Summary:     确认兑换界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 5:15:38 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ExchangeSureWinMono")



-- 界面名称
local wName = "ExchangeSureWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local exchangePhoneCtrl = IMPORT_MODULE("RechargePhoneWinController")
local _platformMgr = IMPORT_MODULE("PlatformMgr");
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _btnClose
local _iconSpr
local _nameLb
local _needLb
local _btnSure
local _leftNumLb
local _btnGray
local _titleSpr
local _costIconSpr
local _changeNumLb
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnSureHandler(gameObject)

    UnityTools.ShowMessage("功能开发中....");
end

--- [ALF END]


--- desc:兑换物品
-- YQ.Qu:2017/3/31 0031
-- @param queryData
-- @param saveData
-- @return
local function SureButtonClick(queryData, saveData)
    LogWarn("[ExchangeSureWinMono.SureButtonClick]saveData.cls=" .. saveData.cls);
    if saveData.localCls ~= nil then
        saveData.cls = saveData.localCls
    end 
    LogError("saveData.cls="..saveData.cls)
    
    if saveData.cls == 1 then -- 红包兑换
        local req = {}
        req.obj_id = saveData.obj_id;
        protobuf:sendMessage(protoIdSet.cs_prize_exchange_req, req);
    elseif saveData.cls == 2 then --话费兑换
        local rechangeType = IMPORT_MODULE("RechargeTypeWinController");
        rechangeType.Open(saveData, queryData);
        UnityTools.DestroyWin("ExchangeSureWin");
    elseif saveData.cls == 3 then
        local baseCtrl = IMPORT_MODULE("ExchangeWinController");
        if baseCtrl.IsSetDefaultList() then
            local addr = baseCtrl.GetDefaultAddress();
            local req = {}
            req.obj_id = saveData.obj_id;
            req.address_id = addr.id;
            protobuf:sendMessage(protoIdSet.cs_prize_exchange_req, req);
        else
            UnityTools.CreateLuaWin("AddressWin");
        end
    elseif saveData.cls == 4 then -- 金币兑换
        local req = {}
        req.obj_id = saveData.obj_id;
        req.phone_card_charge_type = saveData.cls
        protobuf:sendMessage(protoIdSet.cs_prize_exchange_req, req);
    elseif saveData.cls == 5 then -- 金币兑换
        if saveData.obj_id == 99200 then
            local qbCtrl = IMPORT_MODULE("RechargeQBWinController")
            qbCtrl.obj_id=saveData.obj_id
            qbCtrl.saveData = saveData
            UnityTools.CreateLuaWin("RechargeQBWin")
            return
        end
        local req = {}
        req.obj_id = saveData.obj_id;
        req.phone_card_charge_type = saveData.cls
        protobuf:sendMessage(protoIdSet.cs_prize_exchange_req, req);
    end
end

--- desc:更新物品显示
-- YQ.Qu:2017/3/18 0018
-- @param
-- @return 
local function UpdateWin()
    local queryData = CTRL.QueryData();
    local saveData = CTRL.GetSaveData();
    if queryData ~= nil then
        _leftNumLb.text = LuaText.Format("exchangeSureLeft", queryData.store_num);
        _changeNumLb.text = LuaText.Format("exchangeSureLimit", queryData.day_times_config - queryData.today_exchange_times)
        
        if queryData.store_num > 0 and queryData.day_times_config > queryData.today_exchange_times then
            _btnSure:SetActive(true);
            _btnGray:SetActive(false);
            UnityTools.AddOnClick(_btnSure, function(go)
                LogWarn("[ExchangeSureWinMono.anon]sssssssssssssss");
                _platformMgr.MoneyIsEnough(saveData.need_item_id, saveData.need_item_num,
                    function() SureButtonClick(queryData, saveData)
                    end);
            end)
            _titleSpr.spriteName = "sureExchangeTitle"
        else
            _titleSpr.spriteName = "exchangeNoEnoughTitle";
            _btnSure:SetActive(false);
            _btnGray:SetActive(true);
        end
    end
end




-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _iconSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/icon")

    _nameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/name")

    _needLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/need")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnSureHandler)

    _leftNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/left")

    _btnGray = UnityTools.FindGo(gameObject.transform, "Container/btnGray")

    _titleSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/bg/Sprite")

    _costIconSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/costIcon")

    _changeNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/changeNum")

    --- [ALB END]
end

function OnExchangeQureyBack(msgId, value)
    --    LogWarn("[ExchangeSureWinMono.OnExchangeQureyBack]");
    UpdateWin();
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);
    registerScriptEvent(EXCHANGE_QUERY_BACK, "OnExchangeQureyBack")

    LogWarn("[ExchangeSureWinMono.Awake]");
end


local function Start(gameObject)
    local saveData = CTRL.GetSaveData();

    _btnGray:SetActive(true);
    if saveData == nil then
        return;
    end
    local req = {}
    --    PrintTable(saveData)
    req.obj_id = saveData.obj_id
    protobuf:sendMessage(protoIdSet.cs_prize_query_one_req, req);

    LogWarn("[ExchangeSureWinMono.Start]显示确认界面");
    _iconSpr.spriteName = saveData.icon;
    _nameLb.text = saveData.name;
    if saveData.need_item_id ~= 109 then
        _needLb.text = LuaText.Format("exchangeSureNeed", saveData.need_item_num)
    elseif saveData.need_item_num % 10 == 0 then
        _needLb.text = LuaText.Format("exchangeSureNeed", saveData.need_item_num / 10)
    else
        _needLb.text = LuaText.Format("exchangeSureNeed", string.format("%.1f", saveData.need_item_num / 10))
    end

    UnityTools.SetCostIcon(_costIconSpr, saveData.need_item_id)
    UpdateWin();
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EXCHANGE_QUERY_BACK, "OnExchangeQureyBack")
    CLEAN_MODULE("ExchangeSureWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


M.UpdateWin = UpdateWin


-- 返回当前模块
return M
