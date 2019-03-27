-- -----------------------------------------------------------------


-- *
-- * Filename:    MailWinController.lua
-- * Summary:     邮件界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/28/2017 4:37:32 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MailWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local _itemMgr = IMPORT_MODULE("ItemMgr");


-- 界面名称
local wName = "MailWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function IsWinShow()
    return UI.Controller.UIManager.IsWinShow(wName);
 end

function OnMailsInitUpdate(msgId, tMsgData)
--    LogWarn("[MailWinController.OnMailsInitUpdate]邮件初始化数据。。。。");
    if tMsgData == nil or tMsgData.sys_mails == nil then return end;
    _itemMgr.MailsInitUpdate(tMsgData);
end

function OnMailAdd(msgId, tMsgData)

    _itemMgr.MailAdd(tMsgData)
    if IsWinShow() then
        local mono = IMPORT_MODULE("MailWinMono");
        mono.UpdateLeft();
    end
end

--desc:删除邮件
--YQ.Qu:2017/2/28 0028
local function sc_delMail(mail_id)
    local req = {}
    req.mail_id = mail_id;
    req.request_mark = mail_id;
    protobuf:sendMessage(protoIdSet.cs_mail_delete_request, req);
end

function OnMailDelReply(msgId, tMsgData)
    if tMsgData == nil then return; end
    if tMsgData.result == 0 then
        _itemMgr.MailDel(tMsgData.request_mark)
        if IsWinShow() then
            local mono = IMPORT_MODULE("MailWinMono");
            mono.UpdateLeft(true);
        end
    else
        UtilTools.ShowMessage(tMsgData.err_msg, "[FFFFFF]");
    end
end

--desc:领取奖励
--YQ.Qu:2017/2/28 0028
local function sc_mailDrawRequst(mail_id)
    local req = {}
    req.mail_ids = {}
    req.mail_ids[1] = mail_id;
    req.request_mark = mail_id;
    protobuf:sendMessage(protoIdSet.cs_mail_draw_request, req);
end

function OnMailDrawReply(msgId, tMsgData)
    if tMsgData == nil then return; end
    if tMsgData.result == 0 then
        ShowAwardWin(tMsgData.reward_info_s);
        _itemMgr.MailDel(tMsgData.request_mark);
        if IsWinShow() then
            local mono = IMPORT_MODULE("MailWinMono");
            mono.UpdateLeft(true);
        end
    else
        UtilTools.ShowMessage(tMsgData.err_msg, "[FFFFFF]")
    end
end


UI.Controller.UIManager.RegisterLuaWinFunc("MailWin", OnCreateCallBack, OnDestoryCallBack)

protobuf:registerMessageScriptHandler(protoIdSet.sc_mails_init_update, "OnMailsInitUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_mail_add, "OnMailAdd")
protobuf:registerMessageScriptHandler(protoIdSet.sc_mail_delete_reply, "OnMailDelReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_mail_draw_reply, "OnMailDrawReply")

M.sc_mailDrawRequst = sc_mailDrawRequst;
M.sc_delMail = sc_delMail;

-- 返回当前模块
return M
