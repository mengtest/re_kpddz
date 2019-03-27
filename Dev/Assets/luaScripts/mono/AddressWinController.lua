-- -----------------------------------------------------------------


-- *
-- * Filename:    AddressWinController.lua
-- * Summary:     设置收货地址
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 4:27:16 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("AddressWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local UnityTools = IMPORT_MODULE("UnityTools");


-- 界面名称
local wName = "AddressWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end


--- desc:设置地址返回
--- YQ.Qu
function OnPrizeAddressChangeReply(msgId, tMsgData)
    LogWarn("[AddressWinController.OnPrizeAddressChangeReply]设置地址返回");
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        UnityTools.ShowMessage(LuaText.GetString("addrSetSucc"));
        UnityTools.DestroyWin("AddressWin");
    else
        UnityTools.ShowMessage(tMsgData.err);
    end
end

UI.Controller.UIManager.RegisterLuaWinFunc("AddressWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_prize_address_change_reply, "OnPrizeAddressChangeReply")


-- 返回当前模块
return M
