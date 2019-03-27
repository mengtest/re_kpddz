-- -----------------------------------------------------------------


-- *
-- * Filename:    MainBugFeedBackMono.lua
-- * Summary:     输入反馈内容
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/27/2017 5:59:23 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainBugFeedBackMono")



-- 界面名称
local wName = "MainBugFeedBack"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _inputLb
local _cancelButton
local _oKButton
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnOkHandler(gameObject)
    if _inputLb.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("bug_feedback_empty"));
        return;
    end
    local str = string.gsub(_inputLb.value, "^%s*(.-)%s*$", "%1");
    local req = {}
    req.content = str;
    protobuf:sendMessage(protoIdSet.cs_common_bug_feedback, req);
end

function OnBugFeedBack(msgId, tMsgData)
    if tMsgData == nil then
        return
    elseif tMsgData.result == 1 then
        UnityTools.ShowMessage(LuaText.GetString("bug_feedback_succ"));
        UnityTools.DestroyWin(wName);
        return
    else
        UnityTools.ShowMessage(LuaText.GetString("bug_feedback_failed"..tMsgData.result));
    end

end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _inputLb = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/content")

    _cancelButton = UnityTools.FindGo(gameObject.transform, "Container/Grid/CancelButton")
    UnityTools.AddOnClick(_cancelButton.gameObject, OnCloseHandler)

    _oKButton = UnityTools.FindGo(gameObject.transform, "Container/Grid/OKButton")
    UnityTools.AddOnClick(_oKButton.gameObject, OnOkHandler)

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)
    _inputLb.defaultText = LuaText.GetString("bug_feedback_default");
    UnityTools.OpenAction(_winBg);
end


local function OnDestroy(gameObject)
end




protobuf:registerMessageScriptHandler(protoIdSet.sc_common_bug_feedback, "OnBugFeedBack");
-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy



-- 返回当前模块
return M
