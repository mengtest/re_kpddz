-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerRankInfoWinMono.lua
-- * Summary:     牌局外查其他玩家的信息
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/23/2017 6:11:22 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerRankInfoWinMono")



-- 界面名称
local wName = "PlayerRankInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local _ctrl = IMPORT_MODULE("PlayerRankInfoWinController")
-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _winBg
local _lvLb
local _nameLb
local _sex
local btnAddFriend
local _winLoseLb
local _winRateLb
local _weekProfitLb
local _moneyLb
local _mack
local _headTexture
local _headGo
local _headDefaultImg
local _accountLb
local _go
--- [ALD END]

local function OnAddFriend(gameObject)
end

--点击蒙版区关闭界面
local function OnCloseWin(gameObject)
    UnityTools.DestroyWin(wName);
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

--过滤机器人的ID
local function getAcount(acount)
    if string.upper(string.sub(acount,1,1)) == "I" then
        return string.sub(acount,2,string.len(acount))
    end
    return acount
 end


--desc:界面刷新
--YQ.Qu:2017/2/23 0023
local function UpdateWin(value)
    if value == nil then
        return;
    end
    local playerInfoData = value;
    _lvLb.text = LuaText.Format("otherPlayerLv", playerInfoData.level);
    _nameLb.text = playerInfoData.obj_name;
    _winLoseLb.text = LuaText.Format("OtherWinLose", playerInfoData.win_count, playerInfoData.defeated_count);
    _winRateLb.text = LuaText.Format("OtherWinRate", playerInfoData.win_rate);
    _weekProfitLb.text = LuaText.Format("OtherWeekProfit", UnityTools.GetShortNum(playerInfoData.week_profit));
    _moneyLb.text = UnityTools.GetShortNum(playerInfoData.gold);

    btnAddFriend:SetActive(false);
    if playerInfoData.sex == 1 then
        _sex.spriteName = "girl";
    end
    if playerInfoData.icon == "" or playerInfoData.icon == nil then
        _headDefaultImg.spriteName = _platformMgr.PlayerDefaultHead(playerInfoData.sex);
    end


    local nameWidget = _nameLb.gameObject:GetComponent("UIWidget")
    _sex.transform.localPosition = UnityEngine.Vector3(_nameLb.transform.localPosition.x + nameWidget.width + 36, 1, 0);
    local vip = playerInfoData.vip_level;
    if playerInfoData.obj_player_uuid == _platformMgr.PlayerUuid() then
        vip = _platformMgr.GetVipLv();
    end
    UnityTools.SetNewVipBox(_headGo.transform:Find("vip/vipBox"), vip,"vip",_go);
    UnityTools.SetPlayerHead(playerInfoData.icon, _headTexture, playerInfoData.obj_player_uuid == _platformMgr.PlayerUuid());
    if playerInfoData.account ~= nil and playerInfoData.account ~= "" then
        _accountLb.text = LuaText.Format("playerAccount", getAcount(playerInfoData.account))
    end
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _lvLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/lv/Label")

    _nameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/name")

    _sex = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/name/sex")

    btnAddFriend = UnityTools.FindGo(gameObject.transform, "Container/btnAddFriend")
    UnityTools.AddOnClick(btnAddFriend.gameObject, OnAddFriend)

    _winLoseLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/winLose")

    _winRateLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/winRate")

    _weekProfitLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/weekProfit")

    _moneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/money/Label")

    _mack = UnityTools.FindGo(gameObject.transform, "mack")
    UnityTools.AddOnClick(_mack.gameObject, OnCloseWin)

    _headTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "Container/head/headTexture")

    _headGo = UnityTools.FindGo(gameObject.transform, "Container/head")

    _headDefaultImg = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/head/icon")

        _accountLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/account")

--- [ALB END]

end

function OnShowRankPlayerInfo(msgId, value)
    --    LogWarn("[PlayerRankInfoWinMono.OnShowRankPlayerInfo]接到数据");
    UpdateWin(value)
end

local function Awake(gameObject)
    
    _go = gameObject
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    registerScriptEvent(SHOW_PLAYER_INFO, "OnShowRankPlayerInfo")
    UnityTools.OpenAction(_winBg);
end


local function Start(gameObject)
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(SHOW_PLAYER_INFO, "OnShowRankPlayerInfo")
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
