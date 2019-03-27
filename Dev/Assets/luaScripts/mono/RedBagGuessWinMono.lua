-- -----------------------------------------------------------------


-- *
-- * Filename:    RedBagGuessWinMono.lua
-- * Summary:     红包猜测界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/20/2017 6:09:53 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedBagGuessWinMono")



-- 界面名称
local wName = "RedBagGuessWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _saveData;

local _mack
local _btnClose
local _headTexture
local _head
local _playerNameLb
local _descLb
local _btnGuess
local _winBg
local _startContainer
local _inputContainer
local _numInput
local _btnGuessNum
local _err
local _err1
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnGuessHandler(gameObject)
    _startContainer:SetActive(false);
    _inputContainer:SetActive(true);
    -- UnityTools.SetActive(_err1.gameObject,true)
    -- _err1.gameObject:SetActive(true)
    -- _err1.enabled = true
    -- _err1:ResetToBeginning()
    -- _err1:PlayForward()
end

local function OnGuessNumHandler(gameObject)

    if _numInput.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("redBagGuessDefault"));
        return;
    end

    if _platformMgr.GetVipLv() < 0 then
        UnityTools.ShowMessage(LuaText.GetString("redBagGuessVip"));
        return
    end
    local req = {}
    req.uid = _saveData.uid
    req.check_num = tonumber(_numInput.value);
    protobuf:sendMessage(protoIdSet.cs_red_pack_open_req, req);
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _mack = UnityTools.FindGo(gameObject.transform, "mack")
    UnityTools.AddOnClick(_mack.gameObject, OnCloseHandler)

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _headTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "Container/start/head/Texture")
    _head = UnityTools.FindCo(gameObject.transform,"UISprite", "Container/start/head")

    _playerNameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/start/head/name")

    _descLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/start/desc")

    _btnGuess = UnityTools.FindGo(gameObject.transform, "Container/start/btnGuess")
    UnityTools.AddOnClick(_btnGuess.gameObject, OnGuessHandler)

    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _startContainer = UnityTools.FindGo(gameObject.transform, "Container/start")

    _inputContainer = UnityTools.FindGo(gameObject.transform, "Container/input")

    _numInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/input/input")

    _btnGuessNum = UnityTools.FindGo(gameObject.transform, "Container/input/btnGuess")
    UnityTools.AddOnClick(_btnGuessNum.gameObject, OnGuessNumHandler)

    -- _err = UnityTools.FindGo(gameObject.transform, "Container/input/err")
    -- _err1 = UnityTools.FindGo(gameObject.transform, "Container/input/err1"):GetComponent("TweenAlpha")
    -- _err1.enabled=false
    --- [ALB END]
end

function OnRedBagGuessUpdate(msgId, type)
    -- if type then

    -- else
    --     _err:SetActive(true);
    -- end
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    registerScriptEvent(RED_BAG_GUESS_UPDATE, "OnRedBagGuessUpdate")
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg)
    local info = CTRL.GetSaveData();
    _saveData = info;
    if info ~= nil then
--        LogWarn("[RedBagGuessWinMono.Start]"..info.player_icon.."    player_icon === "..tostring(info.player_icon=="")..info.account);
--        LogWarn("[RedBagGuessWinMono.Start]player self uuid = = ".._platformMgr.PlayerUuid().."  other uuid = "..info.uid);
        UnityTools.SetPlayerHead(info.player_icon, _headTexture);
        _playerNameLb.text = info.player_name;
        _descLb.text = info.des;
        UnityTools.SetNewVipBox(_head, 0,"vip");
        if info.player_icon == nil or info.player_icon == "" then
--            LogWarn("[RedBagGuessWinMono.Start]info.sex ==== "..info.sex);
            _head.spriteName = _platformMgr.PlayerDefaultHead(info.sex)
        end
        UnityTools.AddOnClick(_head.gameObject,function (go)
            local rankInfoCtrl = IMPORT_MODULE("PlayerRankInfoWinController");
            if rankInfoCtrl ~= nil then
                rankInfoCtrl.Open(info.player_id);
            end
         end)
    end

    _numInput.defaultText = LuaText.GetString("redBagGuessDefault");
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(RED_BAG_GUESS_UPDATE, "OnRedBagGuessUpdate")
    CLEAN_MODULE("RedBagGuessWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
