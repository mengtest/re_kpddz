-- -----------------------------------------------------------------


-- *
-- * Filename:    RedBagSendWinMono.lua
-- * Summary:     红包发送界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/21/2017 4:20:47 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedBagSendWinMono")



-- 界面名称
local wName = "RedBagSendWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local sendType = 1;

local _winBg
local _mack
local _btnClose
local _beforeDesc
local _btnSend
local _sendContaienr
local _titleInput
local _sendNumInput
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function Strlen(str)
    local bytes = { string.byte(str, 1, #str) }
    local length, begin = 0, false
    for _, byte in ipairs(bytes) do
        if byte < 128 or byte >= 192 then

            begin = false
            length = length + 1
        elseif not begin then
            begin = true
            length = length + 1
        end
    end
    return length
end

local function OnSendHandler(gameObject)
    if sendType == 1 then
        --VIP限制（VIP3才能发红包）
        if _platformMgr.GetVipLv() >= 3 then
            sendType = 2;
            _beforeDesc:SetActive(false);
            _sendContaienr:SetActive(true);
        else
            UnityTools.ShowMessage(LuaText.GetString("redBagSendVipNoEnough"))
        end
        return
    else
        if _titleInput.value == "" then
            UnityTools.ShowMessage(LuaText.GetString("noTitle"))
            return;
        end
        if Strlen(_titleInput.value) > 20 then
            UnityTools.ShowMessage(LuaText.GetString("noTitle2"))
            return;
        end

        if _sendNumInput.value == "" then
            UnityTools.ShowMessage(LuaText.GetString("redBagSendNumTip"))
            return;
        end

        local num = tonumber(_sendNumInput.value);
        --红包的金额
        if num < 100000 or num > 100000000 then
            UnityTools.ShowMessage(LuaText.GetString("redBagSendNumNoCorrect"))
            return
        end

        --发完红包后身上的钱不能少于30万
        if _platformMgr.GetGod() - num < 300000 then
            UnityTools.ShowMessage(LuaText.GetString("redBagSendLeftMoney"))
            return;
        end

        --确认框
        UnityTools.MessageDialog(LuaText.Format("redBagSendSure", comma_value(num)), {
            okCall = function()
                --发送红包
                local req = {}
                req.set_num = num
                req.des = _titleInput.value;
                protobuf:sendMessage(protoIdSet.cs_red_pack_create_req, req);
            end
        })
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _mack = UnityTools.FindGo(gameObject.transform, "mack")
    UnityTools.AddOnClick(_mack.gameObject, OnCloseHandler)

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _beforeDesc = UnityTools.FindGo(gameObject.transform, "Container/beforeDesc")

    _btnSend = UnityTools.FindGo(gameObject.transform, "Container/btnGuess")
    UnityTools.AddOnClick(_btnSend.gameObject, OnSendHandler)

    _sendContaienr = UnityTools.FindGo(gameObject.transform, "Container/send")

    _titleInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/send/title")

    _sendNumInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/send/sendNum")

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    _titleInput.value = LuaText.GetString("redBagSendDefaultTitle");
    _sendNumInput.defaultText = LuaText.GetString("redBagSendNumTip");
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("RedBagSendWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
