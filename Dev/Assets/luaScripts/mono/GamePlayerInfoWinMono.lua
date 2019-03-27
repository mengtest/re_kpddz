-- -----------------------------------------------------------------


-- *
-- * Filename:    GamePlayerInfoWinMono.lua
-- * Summary:     牌局内查看玩家信息
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/24/2017 11:21:56 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GamePlayerInfoWinMono")



-- 界面名称
local wName = "GamePlayerInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _roomMgr = IMPORT_MODULE("roomMgr")
local _ctrl = IMPORT_MODULE("GamePlayerInfoWinController")

local _winBg
local _mack
local _lvLb
local _nameLb
local _sexSpr
local _btnAddFriend
local _winLoseLb
local _winRateLb
local _weekProfitLb
local _moneyLb
local faceScrollView
local faceScrollView_mgr
local _headTexture
local _head
local _magicGrid
local _cell
local _headDefaultImg
local _accountLb
local _vipBox
local _bClose=false
local _thisObj
--- [ALD END]

--过滤机器人的ID
local function getAcount(acount)
    if string.upper(string.sub(acount, 1, 1)) == "I" then
        return string.sub(acount, 2, string.len(acount))
    end
    return acount
end



--desc:界面刷新
--YQ.Qu:2017/2/23 0023
local function UpdateWin(value)
    local otherPlayerData = value or CTRL.GetOtherPlayerData()
    if otherPlayerData == nil then
        return;
    end
    _lvLb.text = LuaText.Format("otherPlayerLv", otherPlayerData.level .. "");
    _nameLb.text = otherPlayerData.obj_name;
    _winLoseLb.text = LuaText.Format("OtherWinLose", otherPlayerData.win_count, otherPlayerData.defeated_count);
    LogWarn("[GamePlayerInfoWinMono.UpdateWin]" .. otherPlayerData.win_rate);
    _winRateLb.text = LuaText.Format("OtherWinRate", otherPlayerData.win_rate or 0);
    _weekProfitLb.text = LuaText.Format("OtherWeekProfit", UnityTools.GetShortNum(otherPlayerData.week_profit));
    _moneyLb.text = UnityTools.GetShortNum(otherPlayerData.gold);
    if otherPlayerData.sex == 1 then
        _sexSpr.spriteName = "girl";
    end
    local nameWidget = _nameLb.gameObject:GetComponent("UIWidget")
    _sexSpr.transform.localPosition = UnityEngine.Vector3(_nameLb.transform.localPosition.x + nameWidget.width + 36, 1, 0);

    --    _btnAddFriend:SetActive(_platformMgr.IsMyFriend(otherPlayerData.obj_player_uuid) == false);
    _btnAddFriend:SetActive(false);
    if otherPlayerData.icon ~= "" then
        UnityTools.SetPlayerHead(otherPlayerData.icon , _headTexture )
    else
        _headDefaultImg.spriteName = _platformMgr.PlayerDefaultHead(otherPlayerData.sex);
    end
    UnityTools.SetNewVipBox(_vipBox, otherPlayerData.vip_level,"vip",_thisObj);

    if otherPlayerData.account ~= nil and otherPlayerData.account ~= "" then
        _accountLb.text = LuaText.Format("playerAccount", getAcount(otherPlayerData.account))
    end
end





local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

--添加为好友
local function OnAddFriend(gameObject)
end
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local function CloseWin2()
    UnityTools.DestroyWin(wName)
end
local function CloseSelfWin()
    if _bClose then
        return
    end
    _bClose  =true
    gTimer.registerOnceTimer(500,CloseWin2)

end
--desc:发表情给其他玩家
--YQ.Qu:2017/2/24 0024
local function OnSendFaceToOther(go)
    local cData = go:GetComponent("ComponentData");
    if cData ~= nil then
        _roomMgr.SendChatMsg(2, "{#emoji}" .. (cData.Id + 300), _ctrl.OtherPlayerUuid());
        CloseSelfWin()
        -- UnityTools.DestroyWin("GamePlayerInfoWin");
    end
end

--显示表情的Item
local function OnFaceShowItem(cellbox, index, item)
    local faceSrp = item:GetComponent("UISprite");
    local cData = item:GetComponent("ComponentData")
    if cData ~= nil then cData.Id = index; end
    faceSrp.spriteName = "magic" .. (index + 1);
    UnityTools.AddOnClick(item, OnSendFaceToOther)
end


--- [ALF END]


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _mack = UnityTools.FindGo(gameObject.transform, "mack")
    UnityTools.AddOnClick(_mack.gameObject, OnCloseHandler)

    _lvLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/lv/Label")

    _nameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/name")

    _sexSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/name/sex")

    _btnAddFriend = UnityTools.FindGo(gameObject.transform, "Container/btnAddFriend")
    UnityTools.AddOnClick(_btnAddFriend.gameObject, OnAddFriend)

    _winLoseLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/winLose")

    _winRateLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/winRate")

    _weekProfitLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/weekProfit")

    _moneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/money/Label")

    faceScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/faceBg/faceScrollView")
    faceScrollView_mgr = UnityTools.FindCoInChild(faceScrollView, "UIGridCellMgr")
    faceScrollView_mgr.onShowItem = OnFaceShowItem

    _headTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "Container/head/icon/Texture")
    _head = UnityTools.FindGo(gameObject.transform, "Container/head")
    _vipBox = UnityTools.FindGo(gameObject.transform, "Container/head/vip/vipBox")
    _magicGrid = UnityTools.FindGo(gameObject.transform, "Container/faceBg/faceScrollView/Grid")

    _cell = UnityTools.FindGo(gameObject.transform, "cell")

    _headDefaultImg = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/head/icon")

    _accountLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/account")

    -- 先隐藏，否则会VIP会闪一下
    UnityTools.SetNewVipBox(_vipBox, -1,"vip");
    --- [ALB END]
end

function OnShowPlayerInfo(msgId, saveData)
    LogWarn("[GamePlayerInfoWinMono.OnShowRankPlayerInfo]" .. msgId);
    UpdateWin(saveData);
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    _thisObj = gameObject
    AutoLuaBind(gameObject)

    registerScriptEvent(SHOW_PLAYER_INFO, "OnShowPlayerInfo")

    local scroll = UnityTools.FindGo(gameObject.transform, "Container/faceBg/faceScrollView");
    _controller:SetScrollViewRenderQueue(scroll);
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    --[[if CTRL.GetOtherPlayerData() ~= nil then
        UpdateWin();
    end]]


    faceScrollView_mgr:ClearCells();
    local len = 4;
    for i = 1, len do
        faceScrollView_mgr:NewCellsBox(faceScrollView_mgr.Go)
    end
    faceScrollView_mgr.Grid:Reposition();
    faceScrollView_mgr:UpdateCells();
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(SHOW_PLAYER_INFO, "OnShowPlayerInfo")
    gTimer.removeTimer(CloseWin2)
    CLEAN_MODULE(wName .. "Mono");
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.UpdateWin = UpdateWin


-- 返回当前模块
return M
