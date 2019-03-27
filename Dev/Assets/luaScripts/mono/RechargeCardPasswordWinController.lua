-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargeCardPasswordWinController.lua
-- * Summary:     提取卡密
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/21/2017 10:23:14 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeCardPasswordWinController")
local UnityTools = IMPORT_MODULE("UnityTools");
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 界面名称
local wName = "RechargeCardPasswordWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _cardNum;
local _cardPsd;


function OnOpenRechargeCardPasswordWin(msgId, cardNum, cardPsd)
    UnityTools.CreateLuaWin("RechargeCardPasswordWin");
    _cardNum = cardNum;
    _cardPsd = cardPsd;
end


local function GetData()
    return _cardNum, _cardPsd;
end

registerScriptEvent(OPEN_RECHANGE_CARD_PASSWORD_WIN, "OnOpenRechargeCardPasswordWin")

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

--- desc:
--- YQ.Qu
function OnPrizeQueryPhoneCardKeyReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        if tMsgData.state == 1 then
            UnityTools.ShowMessage(LuaText.GetString("exchangeCardLooKWait"));
        else
--            LogWarn("[RechargeCardPasswordWinController.OnPrizeQueryPhoneCardKeyReply]......................." .. tMsgData.key);
            local tab = stringToTable(tMsgData.key, ",")
            if #tab > 1 then
                _cardNum = tab[1]
                _cardPsd = tab[2]
                UnityTools.CreateLuaWin("RechargeCardPasswordWin");
            end
        end
    else
        UnityTools.ShowMessage(tMsgData.err)
    end
end




UI.Controller.UIManager.RegisterLuaWinFunc("RechargeCardPasswordWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_query_phonecard_key_reply, "OnPrizeQueryPhoneCardKeyReply")

M.GetData = GetData;
-- 返回当前模块
return M
