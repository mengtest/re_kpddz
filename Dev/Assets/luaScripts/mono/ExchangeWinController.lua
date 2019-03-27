-- -----------------------------------------------------------------


-- *
-- * Filename:    ExchangeWinController.lua
-- * Summary:     兑换奖励
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/16/2017 5:58:13 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ExchangeWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 界面名称
local wName = "ExchangeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local UnityTools = IMPORT_MODULE("UnityTools");
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _itemMgr = IMPORT_MODULE("ItemMgr")

local baseCfgList = {};
local recordList = {};
local addrList = {};
local storageList = {};
local storageLeftTimeList = {};
M.HasGetMsg =false 
--- desc:清理数据
-- YQ.Qu:2017/3/20 0020
-- @param
-- @return
function ExchangeWinControllerClear(msgID, obj)
    LogWarn("[ExchangeWinController.ExchangeWinControllerClear]清理兑换记录数据");
    storageList = {}
    storageLeftTimeList = {}
    recordList = {};
    M.HasGetMsg =false 
end

registerScriptEvent(EXIT_CLEAR_ALL_DATA, "ExchangeWinControllerClear")

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function GetExchangeShop(type)
    if baseCfgList == {} or baseCfgList[type] == nil then
        return {}
    end
    return baseCfgList[type];
end

local function GetRecordList(type)
    if recordList == {} or recordList[type] == nil then
        return {}
    end
    return recordList[type];
end

--- 是否设置了默认地址
local function IsSetDefaultList()
    return #addrList > 0
end

--- 获取默认地址，可能为空
local function GetDefaultAddress()
    if IsSetDefaultList() then
        return addrList[1];
    end

    return nil;
end

local function IsWinShow(winName)
    return UI.Controller.UIManager.IsWinShow(winName);
end

local function SortByBaseId(a, b)
    return a.obj_id < b.obj_id;
end

local function SortBySortId(a, b)
    return a.sort_id < b.sort_id;
end
local function CheckStorage(objId)
    for i = 1, #storageList do

        if storageList[i].obj_id == objId and (storageList[i].store_num > 0 or storageList[i].crad_num > 0) and storageLeftTimeList[objId] ~= nil and storageLeftTimeList[objId] > 0 then
            
            return true;
        end
    end
    return false;
end

--- 获取主界面上红点显示的个数
local function GetRedNum()
    local count = 0;
    local cash = _platformMgr.GetCash();
    local vipLv = _platformMgr.GetVipLv();
    local redNum = _itemMgr.GetItemNum(109)
    for i = 1, #baseCfgList do
        for j = 1, #baseCfgList[i] do
            if (
                    (baseCfgList[i][j].need_item_id == 103 and baseCfgList[i][j].need_item_num <= cash) --现金
                    or (baseCfgList[i][j].need_item_id == 109 and baseCfgList[i][j].need_item_num <= redNum)--红包
                ) and baseCfgList[i][j].need_vip_level <= vipLv then
                if CheckStorage(baseCfgList[i][j].obj_id) then
--                    LogWarn("[ExchangeWinController.GetRedNum]" .. baseCfgList[i][j].need_item_id .. "   num = " .. baseCfgList[i][j].need_item_num);
                    count = count + 1;
                end
            end
        end
    end

    return count;
end

local function checkExchangeBackCls(objId)
    for k, v in pairs(baseCfgList) do
        for i, m in pairs(v) do
            if m.obj_id == objId then
                return k;
            end
        end
    end
end

function OnPrizeConfigUpdate(msgId, tMsgData)
    --    LogWarn("[ExchangeWinController.OnPrizeConfigUpdate]兑换奖励数据下来");
    if tMsgData == nil then return; end
    baseCfgList = {};
    if tMsgData.list == nil then return end;
    for i = 1, #tMsgData.list do

        local data = tMsgData.list[i];
        if baseCfgList[data.cls] == nil then
            --            LogWarn("[ExchangeWinController.OnPrizeConfigUpdate] data.tag = " .. data.cls);
            baseCfgList[data.cls] = {};
        end

        if data.cls == 4 then
            
            data.localCls = data.cls
            data.cls= 1
        end
        baseCfgList[data.cls][#baseCfgList[data.cls] + 1] = data;
        -- if storageLeftTimeList[data.obj_id] == nil then
            storageLeftTimeList[data.obj_id] = data.today_buy_times
            
        -- end
    end
    for i = 1, #baseCfgList do
        table.sort(baseCfgList[i], SortBySortId)
    end
    table.sort(baseCfgList[5], SortBySortId)
    triggerScriptEvent(UPDATE_MAIN_WIN_RED, "exchange");
    if IsWinShow("ExchangeWin") then
        local mono = IMPORT_MODULE("ExchangeWinMono");
        mono.UpdateList(true);
    end
    LogError("ss 99050="..storageLeftTimeList[99050])
end

--- desc:
-- YQ.Qu:2017/3/20 0020
-- @param baseCfg
-- @param info
-- @return
local function GetRecordNameFromBase(baseCfg, info)
    for k, v in pairs(baseCfg) do
        if v.obj_id == info.obj_id then
            return v.name, v.icon;
        end
    end
    return "", "";
    --    info.name = "";
end

--- desc:
-- YQ.Qu:2017/3/20 0020
-- @param info
-- @return
local function UpdateRecordList(info, list)
    for i = 1, #list do
        if list[i].id == info.id then
            list[i] = info;
            return;
        end
    end

    list[#list + 1] = info;
end


--- 兑换记录更新
function OnPrizeExchangeRecordUpdate(msgId, tMsgData)

    
    if tMsgData == nil then
        return;
    end
    --    recordList = {}
    if tMsgData.list == nil then return; end
    

    for i = 1, #tMsgData.list do
        local data = tMsgData.list[i];
        if recordList[data.record_type] == nil then
            recordList[data.record_type] = {};
        end
    
        data.name = ""
        local tmpName = "";
        local iconSpr = "";
        if baseCfgList ~= nil and #baseCfgList > 0 then
            for k, v in pairs(baseCfgList) do
                tmpName, iconSpr = GetRecordNameFromBase(v, data);
                if tmpName ~= "" then
    
                    data.name = tmpName;
                    data.icon = iconSpr;
                end
            end
        end
        if #recordList[data.record_type] == 0 then
            recordList[data.record_type][#recordList[data.record_type] + 1] = data;
        else
            UpdateRecordList(data, recordList[data.record_type])
        end
    end
    for k, v in pairs(recordList) do
        table.sort(recordList[k], SortByBaseId);
    end
    M.HasGetMsg =true
    if IsWinShow("ExchangeWin") then
        local mono = IMPORT_MODULE("ExchangeWinMono");
        if mono == nil then
            return
        end
        mono.UpdateList(true);
    end
end


--- desc:返回地址列表
--- YQ.Qu
function OnPreizeAddressInfoUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    addrList = {}
    if tMsgData.list == nil then return; end
    addrList = tMsgData.list;
    --    LogWarn("[ExchangeWinController.OnPreizeAddressInfoUpdate]" .. #addrList);
end

--- desc:
--- YQ.Qu
function OnPreizeExchangeReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        UnityTools.DestroyWin("RechargePhoneWin");
        UnityTools.DestroyWin("ExchangeSureWin");
        UnityTools.DestroyWin("RechargeQBWin");
        UnityTools.DestroyWin("RechargeTypeWin");
        
        if tMsgData.reward ~= nil and #tMsgData.reward > 0 then
            ShowAwardWin(tMsgData.reward)
        else
            local baskCls = checkExchangeBackCls(tMsgData.obj_id);
            if baskCls ~= 1 and baskCls ~= 5 then
                UnityTools.ShowMessage(LuaText.GetString("exchangeSucc"));
                return
            end
            if baskCls == 1 then
                UnityTools.CreateLuaWin("ExchangeTipWin")
            elseif baskCls == 5 then
                UnityTools.MessageDialog(LuaText.GetString("exchangeTip1"),"FFFFFF")
            end
            -- UnityTools.ShowMessage(LuaText.exchangeRedBagSucc);
        end
    else
        UnityTools.ShowMessage(tMsgData.err);
    end
end

local function UpdateStorageList(obj)
    local noHave = true;
    for i = 1, #storageList do
        if storageList[i].obj_id == obj.obj_id then
            noHave = false;
            storageList[i] = obj;
        end
    end

    if noHave then
        storageList[#storageList] = obj;
    end
end

local function UpdateStoragetListUpdateByQuery(objId, storeNum, cardNum, todayBuyTimes)
    --    LogWarn("[ExchangeWinController.UpdateStoragetListUpdateByQuery]" .. objId .. "  store_num = " .. storeNum .. "  cardNum = " .. cardNum.."   todayBuyTimes ="..todayBuyTimes);
    UpdateStorageList({ obj_id = objId, store_num = storeNum, crad_num = cardNum });
             
    LogError("ss1 99050="..storageLeftTimeList[99050])
    storageLeftTimeList[objId] = todayBuyTimes
    LogError("ss2 99050="..storageLeftTimeList[99050])
    triggerScriptEvent(UPDATE_MAIN_WIN_RED, "exchange");
end

--- desc:同步过来的红点数据
--- YQ.Qu
function OnPreizeStorageRedPointUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    --    LogWarn("[ExchangeWinController.OnPreizeStorageRedPointUpdate]后端更新过来的兑换库存数据：    ");
    if tMsgData.list ~= nil then
        if #storageList == 0 then
            storageList = tMsgData.list;
        else
            for i = 1, #tMsgData.list do
                UpdateStorageList(tMsgData.list[i]);
            end
        end
        --        LogWarn("[ExchangeWinController.OnPreizeStorageRedPointUpdate]后端更新过来的兑换库存数据：    " .. #storageList);
        triggerScriptEvent(UPDATE_MAIN_WIN_RED, "exchange");
    end
end
local function getStorageLeftTimeList()
    return storageLeftTimeList;
end

UI.Controller.UIManager.RegisterLuaWinFunc("ExchangeWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_config_update, "OnPrizeConfigUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_exchange_record_update, "OnPrizeExchangeRecordUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_address_info_update, "OnPreizeAddressInfoUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_exchange_reply, "OnPreizeExchangeReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_storage_red_point_update, "OnPreizeStorageRedPointUpdate")

M.GetExchangeShop = GetExchangeShop;
M.GetRecordList = GetRecordList;
M.IsSetDefaultList = IsSetDefaultList;
M.GetDefaultAddress = GetDefaultAddress;
M.GetRedNum = GetRedNum;
M.UpdateStoragetListUpdateByQuery = UpdateStoragetListUpdateByQuery;
M.GetStorageLeftTimeList = getStorageLeftTimeList
-- 返回当前模块
return M
