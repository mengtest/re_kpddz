-- -----------------------------------------------------------------


-- *
-- * Filename:    ExchangeSureWinController.lua
-- * Summary:     确认兑换界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 5:15:38 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ExchangeSureWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local UnityTools = IMPORT_MODULE("UnityTools");
local _saveData;
local _queryData;


-- 界面名称
local wName = "ExchangeSureWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

--- desc:
-- YQ.Qu:2017/3/18 0018
-- @param
-- @return
local function GetQureData()
    if _queryData == nil then
        LogWarn("[ExchangeSureWinController.GetQureData]空的。。。。");
    end
    return _queryData;
end

local function GetSaveData()
    return _saveData;
end

local function Open(obj)
    --    PrintTable(obj)
    _saveData = obj;
    UnityTools.CreateLuaWin("ExchangeSureWin")
    if _saveData == nil then
        LogWarn("[ExchangeSureWinController.Open]过来的Obj是空的。。。。");
    end
end

--- desc:查询物品的Id
--- YQ.Qu
function OnPrizeQueryOneReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    LogWarn("[ExchangeSureWinController.OnPrizeQueryOneReply]查询物品的库存返回");
    _queryData = tMsgData;
    local exchangeWinCtrl = IMPORT_MODULE("ExchangeWinController");
    if exchangeWinCtrl ~= nil then
        exchangeWinCtrl.UpdateStoragetListUpdateByQuery(tMsgData.obj_id,tMsgData.store_num,tMsgData.crad_num,tMsgData.day_times_config - tMsgData.today_exchange_times)
    end
    triggerScriptEvent(EXCHANGE_QUERY_BACK, {});
end


UI.Controller.UIManager.RegisterLuaWinFunc("ExchangeSureWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_query_one_reply, "OnPrizeQueryOneReply")

M.Open = Open;
M.GetSaveData = GetSaveData;
M.QueryData = GetQureData;
-- 返回当前模块
return M
