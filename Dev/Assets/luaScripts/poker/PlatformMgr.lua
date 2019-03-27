-- -----------------------------------------------------------------


-- *
-- * Filename:    PlatformMgr.lua
-- * Summary:     平台管理器
-- *              游戏平台调用，金币、砖石管理，活动、成就记录
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-2-8 12:06:50
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlatformMgr")
local protobuf = sluaAux.luaProtobuf.getInstance();

local UnityTools = IMPORT_MODULE("UnityTools")
local _itemMgr = IMPORT_MODULE("ItemMgr")

local _isInit = false

local config_vip = true; --- 当不显示Vip时设置成False

local _playerUuid
local _isTouris
local _gold
local _cash = 0
local _diamond
local _account = ""
local _lv
local _icon
local _exp
local _userName
local _activity
local _achievement
local _openWinName = ""
local _vipLv;
local _rmb; --已经充值多少人民币
local _sex = 0; --性别
local _winNum = 0; --胜
local _loseNum = 0; --负
local _maxMoney = 0; --最大财富
local _totalProfit = 0; --总盈利
local _weekProfit = 0; --周盈利
local _niuTenNum = 0; --牛牛次数
local _bombNum = 0; --四炸次数
local _fiftyNum = 0; --五花牛次数
local _fiveSmallNum = 0; --五小牛次数
local _otherNum = 0; --散牌次数
local _winRate = 0; --胜率
local _subsidyLeftTime = 0; --补助次数
local _subsidyGold = 0; --补助的金币

local _isShowTourisTip = false;
local _sound = -1; --音效（0，开，1关）
local _music = -1; --音乐（0，开，1关）


local _config = {
    isOpenGuide = false, --- 是否开启新手引导
    isHaveFirstPay = false, --- 是否已经首充过了,
    StartOpenList = {},
    isInitStartOpenList = false, --- 是否已经处理过初始化打开的界面,
    isLoginCompleted = false, --- 后端数据都接收完成了
    isInitMainWin = false, --- 弹出框是否都弹出来了
    centerAwake = false,
    redBagRoomResetNum = 0,
    redBagRoomResetTime = 0,
    msg = "",
}
--- -支付开关
local rechangeCfg = {
    VX_recharge = true, --微信
    ZFB_recharge = true, --支付宝
}

local gameMgr = {}

local _guildstep = 0;
M.MainMusic = "Sounds/BGM/mainDay"



local function SetGuideStep(value)
    if _config.isLoginCompleted == false then
        _guildstep = value;
        return;
    end
    if value == 0 and _guildstep == 0 and _config.centerAwake then --- 新手引导开始
        _guildstep = value;
        triggerScriptEvent(GUIDE_STEP_UPDATE, "hideMain", value);
        UnityTools.CreateLuaWin("GuideWin")
    elseif value == 2 then
        _guildstep = value;
        triggerScriptEvent(GUIDE_STEP_UPDATE, "checkTouris", value);
    end
end

local function GetGuideStep()
    return _guildstep;
end

--获取金币
local function getGold()
    return _gold or 0
end

--- desc:玩家Vip等级
-- YQ.Qu:2017/3/14 0014
local function GetVipLv()
    return _vipLv;
end

--- desc:已充值的人民币
-- YQ.Qu:2017/3/15 0015
local function RMB()
    return _rmb;
end

--desc:玩家PlayerUuid
--YQ.Qu:2017/2/17 0017
local function PlayerUuid()
    return _playerUuid;
end

--desc:帐号
--YQ.Qu:2017/2/17 0017
local function Account()
    return _account
end

--desc:经验
--YQ.Qu:2017/2/17 0017
local function Exp()
    return _exp
end

--desc:玩家名字
--YQ.Qu:2017/2/17 0017
local function UserName()
    return _userName
end

--desc:玩家名
--YQ.Qu:2017/2/23 0023
local function SetUserName(value)
    _userName = value;
end

--获取钻
local function getDiamond()
    return _diamond
end

--是否显示游客登陆提示
local function IsToShowTourisTip()
    return _isShowTourisTip == false and _isTouris
end

--已经打开过提示了
local function IsOpenTourisTip()
    _isShowTourisTip = true;
end

--desc:等级
--YQ.Qu:2017/2/17 0017
local function Lv()
    return _lv
end

local function Icon()
end

--desc:更新资源
--YQ.Qu:
local function updateResource(gold, diamond)
    _gold = gold
    _diamond = diamond
    triggerScriptEvent(EVENT_RESCOURCE_UDPATE, { 1 })
end

--desc:是否已经获取了玩家的基本数据
--YQ.Qu:2017/2/17 0017
local function IsInit()
    return _isInit;
end

--desc:设置开启界面的名字
--YQ.Qu:2017/2/18 0018
local function SetOpenWinName(value)
    _openWinName = value;
end

--desc:获取已经开启界面
--YQ.Qu:2017/2/18 0018
local function GetOpenWinName()
    return _openWinName;
end

--desc:
--YQ.Qu:2017/3/1 0001
local function GetIcon()
    return _icon;
end

--desc:现金
--YQ.Qu:2017/2/22 0022
local function GetCash()
    return _cash
end

--desc:性别
--YQ.Qu:2017/2/22 0022
local function sex()
    return _sex
end

--desc:设置性别
--YQ.Qu:2017/2/23 0023
local function SetSex(value)
    _sex = value;
end

local function winNum()
    return _winNum;
end

local function loseNum()
    return _loseNum;
end

local function maxMoney()
    return _maxMoney;
end

local function totalProfit()
    return _totalProfit;
end

local function weekProfit()
    return _weekProfit;
end

local function niuTenNum()
    return _niuTenNum;
end

local function bombNum()
    return _bombNum;
end

local function fiftyNum()
    return _fiftyNum;
end

local function fiveSmallNum()
    return _fiveSmallNum;
end

local function otherNum()
    return _otherNum;
end

local function winRate()
    return _winRate;
end

local function IsTouris()
    return _isTouris
end

local function SubsidyLeftTime()
    return _subsidyLeftTime or 0
end

local function SubsidyGold()
    return _subsidyGold;
end

--desc:玩家胜负信息
--YQ.Qu:2017/2/22 0022
local function UpdatePlayerWinningRec(tMsgData)
    if tMsgData ~= nil then
        _winNum = tMsgData.win_count;
        _loseNum = tMsgData.defeated_count;
        _winRate = tMsgData.win_rate;
        _maxMoney = tMsgData.max_property;
        _totalProfit = tMsgData.total_profit;
        _weekProfit = tMsgData.week_profit;
        _niuTenNum = tMsgData.niu_10;
        _bombNum = tMsgData.niu_11;
        _fiftyNum = tMsgData.niu_12;
        _fiveSmallNum = tMsgData.niu_13;
        _otherNum = tMsgData.niu_0_win;
    end
end



local function UpdateNameAndSex(newName, newSex)
    _userName = newName;
    _sex = newSex;
end

local function IsMyFriend(otherPlayerUuid)
    return false
end



--desc:
--YQ.Qu:2017/2/17 0017
local function UpdatePlayerInfo()
   
    _gold = GameDataMgr.PLAYER_DATA.Gold;
    _diamond = GameDataMgr.PLAYER_DATA.Diamond;
    _playerUuid = GameDataMgr.PLAYER_DATA.Uuid;

    if _account~="" and _account ~= GameDataMgr.PLAYER_DATA.Account and GameDataMgr.LOGIN_DATA.IsConnectGamerServer then
        LogError("3334")
        protobuf:sendMessage(protoIdSet.cs_task_pay_info_request, {})
    end
    _account = GameDataMgr.PLAYER_DATA.Account;
    _lv = GameDataMgr.PLAYER_DATA.Level;
    _icon = GameDataMgr.PLAYER_DATA.Icon;
    _exp = GameDataMgr.PLAYER_DATA.Exp;
    _userName = GameDataMgr.PLAYER_DATA.UserName;
    _cash = GameDataMgr.PLAYER_DATA.Cash;
    _isInit = true;
    _sex = GameDataMgr.PLAYER_DATA.Sex;
    _isTouris = GameDataMgr.PLAYER_DATA.IsTouris;
    LogError("GameDataMgr.PLAYER_DATA.IsTouris"..tostring(GameDataMgr.PLAYER_DATA.IsTouris))
    _vipLv = GameDataMgr.PLAYER_DATA.VipLevel;
    _rmb = GameDataMgr.PLAYER_DATA.Rmb;
    
end

--desc:清理所有数据
--YQ.Qu:2017/2/17 0017
local function ClearAll()
    _gold = 0;
    _diamond = 0;
    _cash = 0;
    _playerUuid = "";
    _account = "";
    _lv = 1;
    _icon = "";
    _exp = "";
    _userName = "";
    _isInit = false;
    _winNum = 0;
    _loseNum = 0;
    _maxMoney = 0;
    _totalProfit = 0;
    _weekProfit = 0;
    _niuTenNum = 0;
    _bombNum = 0;
    _fiftyNum = 0;
    _fiveSmallNum = 0;
    _otherNum = 0;
    _isTouris = false;
    _isShowTourisTip = false;
    _vipLv = 0;
    _guildstep = 0;
    _config.isHaveFirstPay = false;
    _config.StartOpenList = {};
    _config.isInitStartOpenList = false;
    _config.isLoginCompleted = false;
    _config.isInitMainWin = false;
    _config.centerAwake = false;
    _config.msg = "";
    _config.redBagRoomResetNum = 0
    _config.redBagRoomResetTime = 0
    gameMgr.closeActiveFun = nil
    UnityTools.RemoveDeactiveList()
    
    _itemMgr.ClearAll()
    local rechargeTaskCtrl = IMPORT_MODULE("RechargeTaskWinController")
    rechargeTaskCtrl.IsOpen = 2
    triggerScriptEvent(EXIT_CLEAR_ALL_DATA, "");
end

--破产补助信息更新
function OnNiuSubsidyInfoUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return
    end
    _subsidyLeftTime = tMsgData.left_times;
    _subsidyGold = tMsgData.subsidy_gold;

end

--牌局内打开玩家信息
local function OpenPlayerInfoInGame(otherPlayerUuid)
    local _gamePlayerInfoCtrl = IMPORT_MODULE("GamePlayerInfoWinController")
    if _gamePlayerInfoCtrl ~= nil then
        UnityTools.CreateLuaWin("GamePlayerInfoWin");
        _gamePlayerInfoCtrl.SetOpenPlayerUuid(otherPlayerUuid);
    end
end

--- desc:roomLv房间等级（1时为新手场），okFunc成功回调，cancelFunc失败回调
-- YQ.Qu:2017/3/2 0002
local function SubsidyOpen(roomLv, okFunc, cancelFunc)
    local subsidyCtrl = IMPORT_MODULE("SubsidyWinController")
    if subsidyCtrl ~= nil then
        subsidyCtrl.Open(roomLv, okFunc, cancelFunc);
    else
        LogWarn("[PlatformMgr.SubsidyOpen]controller没找到");
        if cancelFunc ~= nil then
            cancelFunc();
        end
    end
end

--- desc:货币是否足够
-- YQ.Qu:2017/3/8 0008
local function MoneyIsEnough(type, num, enoughFun)
    local value;
    if type == 101 then
        value = _gold;
    elseif type == 102 then
        value = _diamond;
    elseif type == 103 then
        value = _cash;
    elseif type == 109 then
        value = _itemMgr.GetItemNum(109);
    else
        value = nil;
    end
    if value ~= nil then
        if num <= value then
            if enoughFun ~= nil then
                enoughFun()
            end
        else
            --- TODO 调用不足的界面
            UnityTools.ShowMessage(LuaText.GetString("noEnough" .. type));
        end
    end
end


-------------- 设置--------------
--- desc:
-- YQ.Qu:2017/3/9 0009
local soundBase = 50
local function getSound()
    if _sound == -1 then
        if UnityEngine.PlayerPrefs.HasKey("gameValue") then
            _sound = UnityEngine.PlayerPrefs.GetFloat("gameValue");
        else
            _sound = soundBase;
        end
    end
    return _sound;
end

local function setSound(b)
    if b then
        _sound = soundBase;
    else
        _sound = 0;
    end

    UnityEngine.PlayerPrefs.SetFloat("gameValue", _sound);
    UnityEngine.PlayerPrefs.Save()
end
local function HideAllSound()
    
end
local function setMusic(b)
    _music = b;
    UnityEngine.PlayerPrefs.SetFloat("bgmValue", _music);
    UnityEngine.PlayerPrefs.Save()
end

local function getMusic()
    if _music == -1 then
        if UnityEngine.PlayerPrefs.HasKey("bgmValue") then
            _music = UnityEngine.PlayerPrefs.GetFloat("bgmValue");
        else
            _music = 50;
        end
    end
    return _music;
end

local function OnPlayerHeadUpdate(gameObject)
    UnityTools.DestroyWin("PlayerPicSelectWin");
    _icon = GameDataMgr.PLAYER_DATA.Icon;
    local ctrl = IMPORT_MODULE("PlayerInfoWinController");
    if ctrl ~= nil then
        ctrl.ChangePlayerHead();
    end
end

local function openChatWin()
    UnityTools.CreateLuaWin("ChatMainWin")
end

local function openFastAddGold()
    local roomMgr = IMPORT_MODULE("roomMgr")
    local rType = tonumber(roomMgr.RoomType())
    local gType = tonumber(roomMgr.GetGameType()) * 10
    local shopItem = M.GetGameShopItem(tostring(4000 + gType + rType))
    if shopItem.left_times ~= 0 then
        UnityTools.CreateLuaWin("FastAddGoldWin")
    else
        UnityTools.CreateLuaWin("ShopWin")
    end
end

local function openSetWin()
    UnityTools.CreateLuaWin("SettingWin")
end

local function openBoxWin(type,process)
    local boxCtrl = IMPORT_MODULE("OpenBoxWinController")
    if boxCtrl ~= nil then
        boxCtrl.WinType = type
        boxCtrl.Process = process
    end
    UnityTools.CreateLuaWin("OpenBoxWin")
end

local function openShopWin()
    UnityTools.CreateLuaWin("ShopWin")
end

--- desc:游戏中快速购买金币
-- YQ.Qu:2017/3/24 0024
-- @param key 4011,40类型，1游戏类型，1房间等级
-- @return {} or nil
local function GetGameShopItem(key)
    return _itemMgr.GetGameShopItem(key);
end

--- desc:牌局内打开任务面板
-- YQ.Qu:2017/3/24 0024
-- @param
-- @return 
local function OpenTaskByGame()
    --    OpenByGame
    local ctrl = IMPORT_MODULE("TaskWinController");
    if ctrl ~= nil then
        ctrl.OpenByGame();
    end
end


--- desc:打开充值界面
-- YQ.Qu:2017/4/12 0012
-- @param id 商城中的物品Id
-- @return
local function OpenPay(id)
    local rechangeOtherCtrl = UI.Controller.UIManager.GetControler("ShopRechargeOtherWin");
   -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        rechangeOtherCtrl:buyItemIAPImpl(id)
        LogError("审核版本：用苹果IAP充值")
        return
    elseif version.VersionData.isAppStoreVersion() then  -- 苹果商店版本
        
        local ids = tostring(id)
        if ids == "10001" or ids == "10002" or ids == "20001" or ids == "20002" then
            rechangeOtherCtrl:buyItemIAPImpl(id)
        else
            UnityTools.ShowMessage("该档位充值暂未开放");
            -- rechangeOtherCtrl._itemID = id;
            -- rechangeOtherCtrl.VX_recharge = rechangeCfg.VX_recharge;
            -- rechangeOtherCtrl.ZFB_recharge = rechangeCfg.ZFB_recharge;
            -- rechangeOtherCtrl:wxBuy()
        end 
        return
    end
    if rechangeOtherCtrl ~= nil then
        rechangeOtherCtrl._itemID = id;
        rechangeOtherCtrl.VX_recharge = rechangeCfg.VX_recharge;
        rechangeOtherCtrl.ZFB_recharge = rechangeCfg.ZFB_recharge;
        UI.Controller.UIManager.CreateWinByAction("ShopRechargeOtherWin", nil);
    end
end


------------------------------- 【初始弹出界面】-----------------------------

--- desc:登陆游戏时初始化要打开的界面
-- YQ.Qu:2017/4/18 0018
-- @param isOpenLoginAward
-- @return
local function InitStartOpenWin(isOpenLoginAward)
    --    LogWarn("[PlatformMgr.InitStartOpenWin]初始化所有登陆界面   ---->config_vip===="..tostring(M.config_vip));

    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        LogError(" 审核版本：进入游戏不弹窗")
        _config.isInitMainWin = true
        return
    end


    _config.StartOpenList = {}
    if M.config_vip == false then --- VIP功能不显示（提审时），不会引出默认窗口
        _config.isInitMainWin = true;
        return;
    end
    if GameDataMgr.PLAYER_DATA.Block ~= 0 then
        UnityTools.MessageDialog("帐号已被封禁","FFFFFF")
        _config.isInitMainWin = true;
        return;
    end
    -- protobuf:sendMessage(protoIdSet.cs_task_pay_info_request, {})
    --- 签到
    if isOpenLoginAward then
        _config.StartOpenList[#_config.StartOpenList + 1] = "LoginAwardWin"
    end
    local sevenCtrl = IMPORT_MODULE("SevenDailyTaskWinController")
    if sevenCtrl.taskId ~= 0 then
        _config.StartOpenList[#_config.StartOpenList + 1] = "SevenDailyTaskWin"
    end

    local rechargeTaskCtrl = IMPORT_MODULE("RechargeTaskWinController")
     --- 一本万利
    if rechargeTaskCtrl.IsOpen ~= 2 then
        _config.StartOpenList[#_config.StartOpenList + 1] = "RechargeTaskWin"
    end
    
    --- 活动
    -- _config.StartOpenList[#_config.StartOpenList + 1] = "ActivityAndAnnouncement"
    
    --- 月卡
    -- if not version.VersionData.isAppStoreVersion() then  -- 苹果商店版本不弹月卡
    --     local monthCardCtrl = IMPORT_MODULE("MonthCardWinController")
    --     if monthCardCtrl ~= nil and monthCardCtrl.IsShowMonthCardBtn() then
    --         _config.StartOpenList[#_config.StartOpenList + 1] = "MonthCardWin"
    --     end
	-- end


    if IsToShowTourisTip() then
        _config.StartOpenList[#_config.StartOpenList + 1] = "MainLoginTourisTip"
        IsOpenTourisTip();
    end
    if #_config.StartOpenList == 0 then
        _config.isInitMainWin = true;
    end
end

--- 打开初始弹出界面
function _config:OpenStartWin()
    --    LogWarn("[PlatformMgr._config:OpenStartWin]打开第一个默认界面");
    if self.isInitStartOpenList == false then
        local LoginAwardCtrl = IMPORT_MODULE("LoginAwardWinController")
        InitStartOpenWin(LoginAwardCtrl.LoginAwardDataMgr:isOpenLoginAwardWinOnGameStartup())
    end
    if GetGuideStep() == 2 and #_config.StartOpenList > 0 then
        UnityTools.CreateLuaWin(_config.StartOpenList[1]);
    end
end

--- 下一个要打开的弹出界面
function _config:NextOpenStartWin(winName)
    if GetGuideStep() < 2 then
        return;
    end
    if #_config.StartOpenList > 0 and winName == _config.StartOpenList[1] then
        table.remove(_config.StartOpenList, 1)
        if #_config.StartOpenList > 0 then
            UnityTools.CreateLuaWin(_config.StartOpenList[1])
        else
            --            LogWarn("[PlatformMgr._config:NextOpenStartWin]登陆流程：6.弹出界面都关闭，显示主界面排行");
            --            UtilTools.ShowMessage("6.弹出界面都关闭，显示主界面排行","[FF0000]")
            _config.isInitMainWin = true;
            local mainCity = IMPORT_MODULE("MainWinMono");
            if mainCity ~= nil then
                
            else
                local roomMgr = IMPORT_MODULE("roomMgr")
                local rType = tonumber(roomMgr.RoomType())
                if rType == 0 then
                    UnityTools.CreateWin("MainWinMono")
                    UnityTools.ShowMessage("MainWin事先没创建")
                end
            end
        end
    end
end

---------------------------------------------------------




--- 重置模糊计数器
function _config:ResetBlurCnt()
    UnityTools.ResetBlurCnt();
end

--- 水果机进入的条件
function _config:CanIntoFruit()
    -- if _lv >= 2 or _rmb > 0 then
    --     return true;
    -- end
    return true;
    -- UnityTools.ShowMessage(LuaText.fruit_into_limit);
    -- return false;
end
--- 豪车进入条件
function _config:CanIntoCar()
    return true;
    -- if _lv >= 2 and _rmb > 0 then
    --     return true;
    -- end
    -- UnityTools.ShowMessage(LuaText.car_into_limit);
    -- return false;
end

function _config.GuideStart()
    if _config.centerAwake == false then
        gTimer.registerOnceTimer(200, _config.GuideStart)
        return
    end
    gTimer.removeTimer(_config.GuideStart)
    if M.config_vip == false then
        triggerScriptEvent(GUIDE_STEP_UPDATE, "checkTouris", _guildstep);
        triggerScriptEvent(EVNET_HIDE_FUNCTION_FLAG_UDPATE, M.config_vip);
        return
    end
    if _guildstep ~= 2 then --- 新手引导开始
        triggerScriptEvent(GUIDE_STEP_UPDATE, "hideMain", _guildstep);
        UnityTools.CreateLuaWin("GuideWin")
    elseif _guildstep == 2 then
        --        LogWarn("[PlatformMgr.OnLoginCompletedReply]登陆流程：5.打开弹出界面");
        --        UtilTools.ShowMessage("登陆流程：5.打开弹出界面"..tostring(UnityTools.IsWinShow("MainCenterWin")),"[FF0000]")
        triggerScriptEvent(GUIDE_STEP_UPDATE, "checkTouris", _guildstep);
    end
end
local function HideLoadingWin()
    UnityTools.CallLoadingWin(false)
end

local function StartCreateMainWin()
    UnityTools.CallLoadingWin(false)
--    UnityTools.CreateLuaWin("MainWin")
    if UI.Controller.UIManager.IsWinShow("HundredCowMain") then
        return
    end
    _config.GuideStart()
end
local function CreateMainwin()
    if UI.Controller.UIManager.IsWinShow("HundredCowMain") then
        return
    end
    UnityTools.CreateLuaWin("MainWin")
end
--- desc:
--- YQ.Qu
function OnLoginCompletedReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    _config.isLoginCompleted = true;
    --    UnityTools.CallLoadingWin(false);
--    gTimer.registerOnceTimer(1500, UnityTools.CallLoadingWin, false)
    gTimer.registerOnceTimer(2000,  StartCreateMainWin)
    gTimer.registerOnceTimer(1300,CreateMainwin)
    
end


--- 获取默认头像（spriteName）
function M.PlayerDefaultHead(sex)
    sex = sex or _sex;
    if sex == 0 then
        return "boyHead";
    else
        return "girlHead";
    end
end

--- 打开千人红包条件界面
function openRedConditionWin()
    local ctrl = IMPORT_MODULE("RedConditionWinController")
    --    if M.Config.redBagRoomResetNum> 0 then
    --        if ctrl ~= nil and ctrl.Data:IsAllCompleted() == false then
    if ctrl ~= nil then
        ctrl.Data.isGame = true
    end
    UnityTools.CreateLuaWin("RedConditionWin")
    --        end
    --    end
end

----------------------

--- 红包牌局的重置条件
function RedPackRoomResetTimersUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return
    end
    M.Config.redBagRoomResetNum = tMsgData.left_reset_times
    M.Config.redBagRoomResetTime = tMsgData.reset_seconds
end


--UI.Controller.UIManager.RegisterLuaFuncCall("updatePlayerResource", updateResource)
UI.Controller.UIManager.RegisterLuaFuncCall("PlatformMgr:updateResource", updateResource)
UI.Controller.UIManager.RegisterLuaFuncCall("event_player_head_from_csharp", OnPlayerHeadUpdate)
protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_subsidy_info_update, "OnNiuSubsidyInfoUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_login_proto_complete, "OnLoginCompletedReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_room_reset_times_update, "RedPackRoomResetTimersUpdate")
M.OpenRedConditionWin = openRedConditionWin
M.OpenShopWin = openShopWin
M.OpenBoxWin = openBoxWin
M.OpenSetWin = openSetWin
M.OpenChatWin = openChatWin
M.OpenFastAddGold = openFastAddGold
M.GetGod = getGold
M.GetVipLv = GetVipLv
M.GetDiamond = getDiamond
M.PlayerUuid = PlayerUuid
M.Account = Account
M.Exp = Exp
M.UserName = UserName
M.Lv = Lv
M.Icon = Icon
M.UpdatePlayerInfo = UpdatePlayerInfo
M.IsInit = IsInit
M.ClearAll = ClearAll
M.SetOpenWinName = SetOpenWinName
M.GetOpenWinName = GetOpenWinName
M.GetCash = GetCash
M.RMB = RMB

M.OnPlayerHeadUpdate = OnPlayerHeadUpdate
M.getWinNum = winNum;
M.getLoseNum = loseNum;
M.getMaxMoney = maxMoney;
M.getTotalProfit = totalProfit;
M.getWeekProfit = weekProfit;
M.getNiuTenNum = niuTenNum;
M.getBombNum = bombNum;
M.getFiftyNum = fiftyNum;
M.getFiveSmallNum = fiveSmallNum;
M.getOtherNum = otherNum;
M.getSex = sex;
M.getWinRate = winRate;
M.UpdateNameAndSex = UpdateNameAndSex;
M.UpdatePlayerWinningRec = UpdatePlayerWinningRec;
M.SetUserName = SetUserName;
M.SetSex = SetSex;
M.config_vip = config_vip;
M.IsTouris = IsTouris;
M.IsMyFriend = IsMyFriend;
M.OpenPlayerInfoInGame = OpenPlayerInfoInGame;
M.IsToShowTourisTip = IsToShowTourisTip;
M.IsOpenTourisTip = IsOpenTourisTip;
M.SubsidyLeftTime = SubsidyLeftTime;
M.SubsidyGold = SubsidyGold;
M.GetIcon = GetIcon;
M.SubsidyOpen = SubsidyOpen;
M.MoneyIsEnough = MoneyIsEnough;

M.GetSound = getSound;
M.SetSound = setSound;
M.SetMusic = setMusic;
M.GetMusic = getMusic;

M.GetGameShopItem = GetGameShopItem;
M.OpenTaskByGame = OpenTaskByGame;
M.SetGuideStep = SetGuideStep;
M.GetGuideStep = GetGuideStep;

M.OpenPay = OpenPay;
M.Config = _config;
M.rechangeCfg = rechangeCfg;
M.gameMgr = gameMgr;
return M