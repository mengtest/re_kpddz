-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargeTypeWinMono.lua
-- * Summary:     充值方式
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 5:46:13 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeTypeWinMono")



-- 界面名称
local wName = "RechargeTypeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)




-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local exchangePhoneCtrl = IMPORT_MODULE("RechargePhoneWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();

local _winBg
local _btnClose
local _iconSpr
local _nameLb
local _descLb
local _btnSure
local _btnGet
local _btnGray
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnSureHandler(gameObject)
end

local function OnGetHandler(gameObject)
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

    _btnGet = UnityTools.FindGo(gameObject.transform, "Container/btnGet")
    UnityTools.AddOnClick(_btnGet.gameObject, OnGetHandler)

    _btnGray = UnityTools.FindGo(gameObject.transform, "Container/btnGray")

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);
end


local function Start(gameObject)
    local saveData = CTRL.GetSaveData();
    if saveData ~= nil then
        _iconSpr.spriteName = saveData.icon;
        _nameLb.text = saveData.name;
        _descLb.text = saveData.dsc;
        --直充点击
        UnityTools.AddOnClick(_btnSure, function(go)
            exchangePhoneCtrl.Open(saveData);
            UnityTools.DestroyWin("RechargeTypeWin");
        end);

        local queryData = CTRL.GetQueryData();
        if queryData ~= nil then
            if queryData.crad_num > 0 then --电话卡数量可提取卡密码
                _btnGet:SetActive(true);
                _btnGray:SetActive(false);
                UnityTools.AddOnClick(_btnGet, function(go)
                    local req ={}
                    req.obj_id = saveData.obj_id;
                    req.phone_card_charge_type = 2;
                    protobuf:sendMessage(protoIdSet.cs_prize_exchange_req,req);
                end)
            else
                _btnGet:SetActive(false);
                _btnGray:SetActive(true);
            end
        else
            _btnGet:SetActive(false);
            _btnGray:SetActive(true);
        end
    end
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("RechargeTypeWinMono")
end






-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
