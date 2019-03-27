-- -----------------------------------------------------------------


-- *
-- * Filename:    MonthCardWinMono.lua
-- * Summary:     至尊月卡
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/12/2017 6:23:47 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MonthCardWinMono")

local protobuf = sluaAux.luaProtobuf.getInstance();

-- 界面名称
local wName = "MonthCardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr");


local _winBg
local _mack
local _help
local _buyContainer
local _buyNowGetLb
local _buyDayGetLb
local _totalGetLb
local _btnBuy
local _dayContainer
local _leftDayLb
local _dayGetLb
local _btnGet
local _btnGeted
local _close
--- [ALD END]

local function OnOpenHelp(gameObject)
    UnityTools.CreateLuaWin("MonthCardHelpWin");
end

local function OnBuyHandler(gameObject)
    _platformMgr.OpenPay(60001);
end

local function OnDayGetHandler(gameObject)
    local req = {}
    protobuf:sendMessage(protoIdSet.cs_month_card_draw_req, req);
end

local function OnDayGetedHandler(gameObject)
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _mack = UnityTools.FindGo(gameObject.transform, "mack")
    UnityTools.AddOnClick(_mack.gameObject, CloseWin)

    _help = UnityTools.FindGo(gameObject.transform, "Container/help")
    UnityTools.AddOnClick(_help.gameObject, OnOpenHelp)

    _buyContainer = UnityTools.FindGo(gameObject.transform, "Container/buyContainer")

    -- _buyNowGetLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/buyContainer/buyGet/Label")

    -- _buyDayGetLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/buyContainer/dayBuyGet/Label")

    _totalGetLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/buyContainer/Label")

    _btnBuy = UnityTools.FindGo(gameObject.transform, "Container/buyContainer/btnBuy")
    UnityTools.AddOnClick(_btnBuy.gameObject, OnBuyHandler)

    _dayContainer = UnityTools.FindGo(gameObject.transform, "Container/dayContainer")

    _leftDayLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/dayContainer/leftDay")

    _dayGetLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/dayContainer/Label")

    _btnGet = UnityTools.FindGo(gameObject.transform, "Container/dayContainer/btnGet")
    UnityTools.AddOnClick(_btnGet.gameObject, OnDayGetHandler)

    _btnGeted = UnityTools.FindGo(gameObject.transform, "Container/dayContainer/btnGeted")
    UnityTools.AddOnClick(_btnGeted.gameObject, OnDayGetedHandler)

        _close = UnityTools.FindGo(gameObject.transform, "Container/close")
    UnityTools.AddOnClick(_close.gameObject, CloseWin)

--- [ALB END]

end
local function GetLeftDayStr(days)
    local day = days+0;
    if day < 10 then
        return "0"..day
    end
    return day..""
end

local function InitUI()
    _buyContainer:SetActive(CTRL.Data.flag == 2)
    _dayContainer:SetActive(CTRL.Data.flag ~= 2);
    if CTRL.Data.flag == 2 then --还未购买
        -- _buyNowGetLb.text = CTRL.Data.nowBuyGet .. "";
        -- _buyDayGetLb.text = CTRL.Data.dayGet .. "";
        _totalGetLb.text = CTRL.Data.nowBuyGet + CTRL.Data.dayGet * 30;
    else
        _leftDayLb.text = GetLeftDayStr(CTRL.Data.leftDays);
        _dayGetLb.text = CTRL.Data.dayGet .. "";
        _btnGet:SetActive(CTRL.Data.flag == 1);
        _btnGeted:SetActive(CTRL.Data.flag == 0);
    end
end

function OnMonthCardUpdateWin(msg, value,type)
    if type == "monthCard" then
        InitUI();
    end

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    registerScriptEvent(EVENT_MONTH_CARD_UPDATE, "OnMonthCardUpdateWin")
    InitUI();
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_MONTH_CARD_UPDATE, "OnMonthCardUpdateWin")
    CLEAN_MODULE("MonthCardWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
