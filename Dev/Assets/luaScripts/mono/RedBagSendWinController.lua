-- -----------------------------------------------------------------


-- *
-- * Filename:    RedBagSendWinController.lua
-- * Summary:     红包发送界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/21/2017 4:20:47 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedBagSendWinController")
local UnityTools = IMPORT_MODULE("UnityTools");


-- 界面名称
local wName = "RedBagSendWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();


local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

--- desc:
--- YQ.Qu
function OnRedPackCreateReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        UnityTools.DestroyWin("RedBagSendWin");
        UnityTools.ShowMessage(LuaText.GetString("redBagSendSucc"));
    else
        UnityTools.ShowMessage(tMsgData.err);
    end
end


UI.Controller.UIManager.RegisterLuaWinFunc("RedBagSendWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_create_reply, "OnRedPackCreateReply")


-- 返回当前模块
return M
