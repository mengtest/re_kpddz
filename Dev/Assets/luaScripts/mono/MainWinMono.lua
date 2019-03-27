-- -----------------------------------------------------------------


-- *
-- * Filename:    MainWinMono.lua
-- * Summary:     主界面功能Win
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/18/2017 9:46:28 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainWinMono")



-- 界面名称
local wName = "MainWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();
local GameMgr = IMPORT_MODULE("GameMgr")
local _platformMgr = IMPORT_MODULE("PlatformMgr");
local roomMgr = IMPORT_MODULE("roomMgr")

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _itemMgr = IMPORT_MODULE("ItemMgr")

local _leftPos

local btnFree
local btnFeedback
local btnShare
local _btnShareRed
local _btnShareRedLb


local centerIndex = 0;
local _rankOpen = false;


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local btnLuckDraw

local _bottom
local _left
local _right
-- local _leftRankW = 388
-- local _leftMoveX = 200
-- local _bottomMoveY = 300
local _rankTabs;
local _rankTabIndex = 0;
local _go
local _taskRed
local _taskRedLb
local _rankTabGrid
local _rankScrollView
local _rankScrollView_mgr
local _freeRed
local _freeRedLb
local _btnExchangeRed
local _btnExchangeRedLb

local _mailRedSpr
local _mailRedLb
local _activeRedSpr
local _activeRedLb
local _lbname
local _lblevel
local _lbgold
local _lbdiamond
local _goldAdd
local _diamondAdd
local _headmask
local btnRechargeTask
--- [ALD END]



local _headTexture
local _playerHeadIcon
local _redBagHint
local _redBagHintLb
local _mainbtns={}
local _noticeTable={}

_noticeTable.isShowNotice = false

local function ShowNotice()
    if _noticeTable.isShowNotice then return end
    if #CTRL.NoticeList >0 then
        _noticeTable.isShowNotice= true
        _noticeTable.label.text = CTRL.NoticeList[1]
        _noticeTable.tween.from = UnityEngine.Vector3(297.5,0,0)
        _noticeTable.tween.to   = UnityEngine.Vector3(-297.5-_noticeTable.label.width,0,0) 
        _noticeTable.tween.duration = (526+_noticeTable.label.width) / 100
        _noticeTable.tween:PlayForward()
    end
end
local function OnMoveFinished()
    _noticeTable.tween:ResetToBeginning()
    _noticeTable.isShowNotice= false
    if #CTRL.NoticeList>0 then
        table.remove(CTRL.NoticeList,1)
        ShowNotice()
    end
end
function OnNoticeShow()
    ShowNotice()
end
local function SetTopBtnShow()
    local firstPayShopList = _itemMgr.GetShopListByType(5);
    if firstPayShopList ~= nil and #firstPayShopList > 0 then
        
        if firstPayShopList[1].left_times > 0 then
            _mainbtns.spFirst.spriteName = "btnLuckDraw"
            _mainbtns.btntarget.normalSprite = "btnLuckDraw"
        else
            _mainbtns.spFirst.spriteName = "btnLuckDraw2"
            _mainbtns.btntarget.normalSprite = "btnLuckDraw2"
        end
    else
        _mainbtns.spFirst.spriteName = "btnLuckDraw2"
        _mainbtns.btntarget.normalSprite = "btnLuckDraw2"
    end
end
function OnEventFirstPay(eventId, value)
    if value > 0 then
        _mainbtns.spFirst.spriteName = "btnLuckDraw"
        _mainbtns.btntarget.normalSprite = "btnLuckDraw"
    else
        _mainbtns.spFirst.spriteName = "btnLuckDraw2"
        _mainbtns.btntarget.normalSprite = "btnLuckDraw2"
    end
end

function UpdateMainWinLuckyCowRed()
    local luckyCtrl = IMPORT_MODULE("LuckyCowWinController")
    if luckyCtrl ~=nil then
        LogError("luckyCtrl.Data.leftTime ="..luckyCtrl.Data.leftTime )
        UnityTools.SetActive(_mainbtns.redGetMoney.gameObject,luckyCtrl.Data.leftTime >0 and luckyCtrl.Data.isOpen == true)
    end
end
function UpdateMainWinMonthCardRed()
    local monthCtrl = IMPORT_MODULE("MonthCardWinController")
    if monthCtrl ~=nil then
        LogError("monthCtrl.Data.flag="..monthCtrl.Data.flag)
        UnityTools.SetActive(_mainbtns.redMonthCard.gameObject,monthCtrl.Data.flag == 1)
    end

end

--- desc:任务红点更新
-- YQ.Qu:2017/3/15 0015
local function TaskRedUpdate()
    local taskCompleteNum = _itemMgr.GetTaskComplete();
    _taskRed:SetActive(taskCompleteNum > 0);
    if taskCompleteNum > 0 and _taskRedLb ~= nil then
        _taskRedLb.text = taskCompleteNum .. "";
    end
end

--- desc:免费功能的红点
-- YQ.Qu:2017/4/8 0008
-- @param
-- @return 
local function FreeRedUpdate()
    local count = 0;
    if _platformMgr.GetGod() < 2000 and _platformMgr.SubsidyLeftTime() > 0 then
        count = count + 1;
    end
    if _platformMgr.IsTouris() == false then
        local freeCtrl = IMPORT_MODULE("FreeWinController");

        if freeCtrl ~= nil then
            local freeData = freeCtrl.Data()
            if freeData.isDraw ~= nil and freeData.isDraw == false then
                count = count + 1
            end
        end
    end
    _freeRed:SetActive(count > 0)
    if count > 0 then
        _freeRedLb.text = count;
    end
end

local function ExchangeRedUpdate()
    local exchangeCtrl = IMPORT_MODULE("ExchangeWinController");
    if exchangeCtrl ~= nil then
        local count = exchangeCtrl.GetRedNum();
        _btnExchangeRed:SetActive(count > 0);
        if count > 0 then
            _btnExchangeRedLb.text = count;
        end
    end
end

--- 邮件红点显示
local function MailRedUpdate()
    local redCount = _itemMgr.mailRedCount()
    _mailRedSpr:SetActive(redCount > 0)
    if redCount > 0 then
        _mailRedLb.text = redCount .. ""
    end
end

---活动红点显示
local function ActivityRedUpdate()
    local ctrl = IMPORT_MODULE("ActivityAndAnnouncementController")
    local count = ctrl.ActivitiesManager:MainWinRedCount()
--    local count = ctrl.ActivitiesManager:redDotCount()
    -- _activeRedSpr:SetActive(count>0)
    _activeRedSpr:SetActive(false)
    if count > 0 then
        _activeRedLb.text = count..""
    end
 end

-- 分享红点提示
local function ShareAwardRedUpdate()
    local shareCtrl = IMPORT_MODULE("NewShareWinController")
    if shareCtrl ~= nil then
        local count = shareCtrl.availableShareAwardCount()
        _btnShareRed:SetActive(count > 0)
        if count > 0 then
            _btnShareRedLb.text = tostring(count)
        end
    end
end

--desc:
--YQ.Qu:2017/2/18 0018
local function actionMove(isShow)
  --local moveTime = 500 / 1000;
  --if _leftPos ==nil then
  --    _leftPos = _left.transform.localPosition
  --end
  --if isShow then
  --    local leftHash = iTween.Hash("time", moveTime, "x", _leftPos.x - _leftMoveX, "islocal", true, "luaeasetype", iTween.EaseType.linear)
  --    iTween.MoveTo(_left, leftHash)
  --    UnityTools.SetActive(_right.gameObject,false)
  --    triggerScriptEvent(HIDE_OR_SHOW_GIRL,false)
  --    
  --else
  --    local leftX = _leftPos.x;
  --    if _rankOpen then
  --        leftX = _leftPos.x + _leftRankW;
  --    end
  --    local leftHash = iTween.Hash("time", moveTime, "x", leftX, "islocal", true, "luaeasetype", iTween.EaseType.linear)
  --    iTween.MoveTo(_left, leftHash)
  --    UnityTools.SetActive(_right.gameObject,true)
  --    triggerScriptEvent(HIDE_OR_SHOW_GIRL,true)
  --end
end

local function OnClickShare(gameObject)
    local sevenTaskCtrl = IMPORT_MODULE("SevenDailyTaskWinController")
    if sevenTaskCtrl.taskId == 0 then
        UnityTools.ShowMessage(LuaText.funCreating);
        return
    end
    
	UnityTools.CreateLuaWin("NewShareWin")
    UtilTools.InitSharePic(GameDataMgr.PLAYER_DATA.Account)
	--UtilTools.OpenWebView("http://byblwx.76y.com/share?code=111");
end

function OnShowByAction(value)
    --actionMove(false)
end

local function OnOpenFree(gameObject)
    UnityTools.CreateLuaWin("FreeWin");
end

local function OnOpenMail(gameObject)
    --    UnityTools.MessageDialog("邮件" .. "功能开发中....")
    UnityTools.CreateLuaWin("MailWin");
end
function _mainbtns.OnClickSeven(gameObject)
    UnityTools.CreateLuaWin("SevenDailyTaskWin");
end
local function OnOpenTask(gameObject)
    --    UnityTools.MessageDialog("任务" .. "功能开发中....")
    local taskCtrl = IMPORT_MODULE("TaskWinController");
    taskCtrl.Open();
end

local function OnOpenActivity(gameObject)
    UnityTools.CreateLuaWin("ActivityAndAnnouncement")
end

local function OnOpenExchange(gameObject)
    local exchangeCtrl = IMPORT_MODULE("ExchangeWinController");
    if not exchangeCtrl.HasGetMsg then
        protobuf:sendMessage(protoIdSet.cs_prize_query_phonecard_key_req,{rec_id="0"})
    end
    UnityTools.CreateLuaWin("ExchangeWin");
end

local function OnOpenShop(gameObject)
    UnityTools.CreateLuaWin("ShopWin");
end

local function OnOpenSetting(gameObject)
    UnityTools.CreateLuaWin("SettingWin");
end

-- local function OnOpenFeedback(gameObject)
--     --    UnityTools.MessageDialog("反馈" .. "功能开发中....")
--     UnityTools.CreateLuaWin("MainBugFeedBack");
-- end


local function OnOpenShare(gameObject)
    OnClickShare()
end

--- desc:查询排行数据
-- YQ.Qu:2017/3/15 0015
local function ReqRankData(rankType)
    rankType = rankType or 1;
    local req = {}
    req.rank_type = rankType
    protobuf:sendMessage(protoIdSet.cs_rank_query_req, req);
end

--- desc:
-- YQ.Qu:2017/3/15 0015
local function UpdateRankList(rankType)
    if version.VersionData.IsReviewingVersion() then
        return
    end
    if rankType ~= _rankTabIndex or _platformMgr.config_vip == false then return end;
    _rankScrollView_mgr:ClearCells();
    local len = #CTRL.RankList[rankType];
    for i = 1, len do
        _rankScrollView_mgr:NewCellsBox(_rankScrollView_mgr.Go)
    end
    _rankScrollView_mgr.Grid:Reposition();
    _rankScrollView_mgr:UpdateCells();
    _rankScrollView:ResetPosition();
end

--- desc:更新RankTab显示
-- YQ.Qu:2017/3/15 0015
local function ShowRank(tabId)
    if _rankTabIndex == tabId then return end;
    _rankTabIndex = tabId;
    for i = 1, #_rankTabs do
        local select = UnityTools.FindGo(_rankTabs[i].transform, "Sprite");
        select:SetActive(tabId == i);
    end
   -- local tipLb = UnityTools.FindCo(_left.transform, "UILabel", "rank/tip/Label");
   -- tipLb.text = LuaText.GetString("rankTip" .. tabId);
    ReqRankData(tabId);
end

--- rank初始化
local function InitRank()
    --    _rankTabGrid
    _rankTabs = {};
    for i = 1, 3 do
        local tab = UnityTools.FindGo(_rankTabGrid.transform, "tab" .. i);
        _rankTabs[i] = tab;
        if i== 2 then
            _rankTabs[i].gameObject:SetActive(false)
        end
        if i==3 then
            _rankTabs[i].transform.position = _rankTabs[2].transform.position
        end
        UnityTools.AddOnClick(tab, function(go)
            ShowRank(i);
        end)
    end
end



local function ToOpen(go)
    --- 游戏数据没接入完成
    if _platformMgr.Config.isInitMainWin == false then
        LogWarn("[MainWinMono.ToOpen]初始化还没完成就打开游戏了。。。。");
        return;
    end


    local cData = go:GetComponent("ComponentData")
    if cData == nil then return end
    local curData = activeGo:GetComponent("ComponentData");
    if curData ~= nil and curData.Id ~= cData.Id then
        return;
    end

    local roomSelect = IMPORT_MODULE("RoomLvSelectWinController")
    if roomSelect ~= nil then
        local key = cData.Value;
        local roomTypes = GameMgr.GetLoadedMgrs
        local roomName = roomTypes[cData.Value];
        if roomName == "normalCowMgr" then
            roomSelect.OpenWin(cData.Value);
            _platformMgr.SetOpenWinName("RoomLvSelectWin");
           
        elseif roomName == "hundredCowMgr" then
            GameMgr.EnterGame(cData.Value, 1,
                function()
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("MainCenterWin");
                    UnityTools.DestroyWin("GameCenterWin");
                end);
        elseif roomName == "fruit" then
            local protobuf = sluaAux.luaProtobuf.getInstance();
            protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
            registerScriptEvent(EVENT_IS_IN_GAME, "IsCanEnterFruitRoom")
        elseif cData.Value == 5 then
            local protobuf = sluaAux.luaProtobuf.getInstance();
            protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
            registerScriptEvent(EVENT_IS_IN_GAME_FOR_CAR, "IsCanEnterRichCar")
        elseif cData.Value == 4 then
            -- UnityTools.ShowMessage(LuaText.funCreating);
         
            if _platformMgr.GetDiamond() >= 18  then
                GameMgr.EnterGame(1,10,function()
                    UnityTools.DestroyWin("MainCenterWin")
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("GameCenterWin");
                end)
            --[[elseif _platformMgr.Config.redBagRoomResetNum > 0 then
                UnityTools.CreateLuaWin("RedConditionWin")]]
            else
                --[[local shopCtrl = IMPORT_MODULE("ShopWinController");
                if shopCtrl ~= nil then
                    UnityTools.ShowMessage(LuaText.noEnough102);
                    shopCtrl.OpenShop(2)
                end]]
                UnityTools.CreateLuaWin("RedConditionWin")
            end

        end
    end
end
local function IsShowSelect(isShow)
    UnityTools.SetActive(_mainbtns.btnMail.gameObject,not isShow)
    UnityTools.SetActive(_mainbtns.btnSetting.gameObject,not isShow)
    if isShow then
        _mainbtns.btnBack.transform.localPosition = UnityEngine.Vector3(-505,51,0)
    else
        _mainbtns.btnBack.transform.localPosition = UnityEngine.Vector3(-10000,0,0)
    end
    
end
--- 直接快捷打开房间选择界面
local function OpenRoomLvSelect()
    for k, v in pairs(GameMgr.GetLoadedMgrs) do
        if v == "normalCowMgr" then
            local roomSelect = IMPORT_MODULE("RoomLvSelectWinController")
            if roomSelect ~= nil then
                roomSelect.OpenWin(k);
                _platformMgr.SetOpenWinName("RoomLvSelectWin");
                IsShowSelect(true)
                
            end
        end
    end
end

--- 直接快捷打开百人大战界面
local function OpenHundredGame()
    for k, v in pairs(GameMgr.GetLoadedMgrs) do
        if v == "hundredCowMgr" then
            local roomSelect = IMPORT_MODULE("RoomLvSelectWinController")
            if roomSelect ~= nil then
                GameMgr.EnterGame(k, 1,
                    function()
                        UnityTools.DestroyWin("MainWin")
                        UnityTools.DestroyWin("MainCenterWin");
                        UnityTools.DestroyWin("GameCenterWin");
                    end);
            end
        end
    end
end
local function UpdateRedBagHint()
    local ctrl = IMPORT_MODULE("RedBagWinController");
    if ctrl ~= nil then
        _redBagHint:SetActive(ctrl.RedBagMainBtnHint() > 0);
        _redBagHintLb.text = ctrl.RedBagMainBtnHint();
    end
end
function OnUpdateMainWinRed(msgId, type)
    --    LogWarn("[MainWinMono.OnUpdateMainWinRed]"..type);
    if type == "task" then
        TaskRedUpdate();
    elseif type == "free" then
        FreeRedUpdate();
    elseif type == "exchange" then
        ExchangeRedUpdate();
    elseif type == "mail" then
        MailRedUpdate();
    elseif type == "activity" then
        ActivityRedUpdate()
    elseif type == "shareAward" then
        ShareAwardRedUpdate()
    elseif type == "redBag" then
        UpdateRedBagHint()
    end
end
function _mainbtns.OnRankItemHide(cellbox,index)
    -- LogError("index="..cellbox.name)
    local vipBox = UnityTools.FindCo(cellbox.transform,"UISprite", "Cell/head/vipCt/vipBox/vip");
    if vipBox == nil then
        return
    end
    vipBox.enabled = true
    local effect = UnityTools.FindGo(cellbox.transform, "Cell/head/vipCt/vipBox/effect_vip");
    if effect ~= nil then
        effect.gameObject:SetActive(false)
    end
end
local function OnRankItemShow(cellbox, index, item)
    local key = index + 1;
    local info = CTRL.RankList[_rankTabIndex][key];
    local myRank = CTRL.MyRank[_rankTabIndex];
    if info == nil then
        return;
    end
    local bg = UnityTools.FindCo(item.transform, "UISprite", "bg");
    local rankIcon = UnityTools.FindCo(item.transform, "UISprite", "rankIcon");
    local headTexture = UnityTools.FindCo(item.transform, "UITexture", "head/Texture");
    local headDefaultSpr = UnityTools.FindCo(item.transform, "UISprite", "head");
    local nameLb = UnityTools.FindCo(item.transform, "UILabel", "name");
    local moneyIcon = UnityTools.FindCo(item.transform, "UISprite", "moneyIcon");
    local moneyLb = UnityTools.FindCo(item.transform, "UILabel", "money");
    local vipBox = UnityTools.FindGo(item.transform, "head/vipCt/vipBox");
    nameLb.text = info.player_name
														   



    if myRank == info.rank then
								 
															 
						  
											   
												
									  
		   

        UnityTools.SetNewVipBox(vipBox,_platformMgr.GetVipLv(),"vip",nil,0.59*0.6)
    else
        UnityTools.SetNewVipBox(vipBox,info.player_vip,"vip",nil)--_rankScrollView.gameObject,0.59*0.6)
								  
															 

							 
															  
												
											   
									  
		   
    end

    if info.rank <= 3 then
											 
											 
        rankIcon.gameObject:SetActive(true);
        rankIcon.spriteName = "rank" .. info.rank;
    else
        rankIcon.gameObject:SetActive(false);
    end

    if _rankTabIndex == 3 then
        moneyIcon.spriteName = "diamond"
        moneyLb.text = "+" ..comma_value(info.cash_num);
    else
        moneyIcon.spriteName = "money"
        if _rankTabIndex == 1 then
            moneyLb.text = comma_value(info.gold_num);
        else
            moneyLb.text = "+" .. comma_value(info.win_gold_num);
        end
    end
    
    headTexture.mainTexture = nil;
    UnityTools.SetPlayerHead(info.player_icon, headTexture, _platformMgr.PlayerUuid() == info.player_uuid);
    if info.player_icon == nil or info.player_icon == "" then
        if info.sex ~= nil then
            headDefaultSpr.spriteName = _platformMgr.PlayerDefaultHead(info.sex);
        end
    end
    UnityTools.AddOnClick(item, function(go)
        local rankInfoCtrl = IMPORT_MODULE("PlayerRankInfoWinController");
        if rankInfoCtrl ~= nil then
            rankInfoCtrl.Open(info.player_uuid);
        end
    end)
end



local function InitAllScroll()
    _controller:SetScrollViewRenderQueue(_mainbtns._rankSrollView.gameObject);
    UtilTools.SetEffectRenderQueueByUIParent(_go.transform, _mainbtns.effect_zhaocaijinniu, 1);
    _controller:SetScrollViewRenderQueue(_noticeTable.panel.gameObject);
	_noticeTable.panel.startingRenderQueue = _mainbtns._rankSrollView.startingRenderQueue+1
    _controller:SetScrollViewRenderQueue(_headmask.gameObject);
    _controller:SetScrollViewRenderQueue(_mainbtns.redpanel.gameObject);
    _mainbtns.redpanel.startingRenderQueue = _headmask.startingRenderQueue+10
    UtilTools.SetEffectRenderQueueByUIParent(_go.transform, _mainbtns.effect_shangcheng, 10);
    UtilTools.SetEffectRenderQueueByUIParent(_go.transform, _mainbtns.effect_hongbao, 50);
    UtilTools.SetEffectRenderQueueByUIParent(_go.transform, _mainbtns.sevenEffect.transform, 50);
end
-- 卡片入口初始化

--- 钞票兑换
local function OnDiamondExchange(gameObject)
    --    UnityTools.CreateLuaWin("ExchangeWin");
    if GameDataMgr.PLAYER_DATA.Block ~= 0 then
        return
    end
    local shopCtrl = IMPORT_MODULE("ShopWinController")
    if shopCtrl ~= nil then
        shopCtrl.OpenShop(2)
    end
end

--- 金币兑换（添加）
local function OnMoneyAdd(gameObject)
    if GameDataMgr.PLAYER_DATA.Block ~= 0 then
        return
    end
    UnityTools.CreateLuaWin("ShopWin");
end
local function OnOpenPlayerInfo(gameObject)
    
    UnityEngine.PlayerPrefs.SetInt("MainWinRedIsShow",1)
    UnityTools.SetActive( _mainbtns.mainWinRed.gameObject,false)
    UnityTools.CreateLuaWin("PlayerInfoWin");
end
--打开月卡
local function OnOpenCardWin(gameObject)
    UnityTools.CreateLuaWin("MonthCardWin");
end
--- 红包广场
local function OnOpenRed(gameObject)
    UnityTools.CreateLuaWin("RedBagWin");
end
--- 金币彩池
local function OnOpenGoldPool(gameObject)
    UnityTools.CreateLuaWin("GoldPoolWin");
    
    
end
--- 招财金牛
local function OnOpenGetMoney(gameObject)
    local protobuf = sluaAux.luaProtobuf.getInstance();
    protobuf:sendMessage(protoIdSet.cs_golden_bull_mission, {})
    UnityTools.CreateLuaWin("LuckyCowWin");
end
---钻石福袋
local function OnOpenDiamondBag(gameObject)
    UnityTools.CreateLuaWin("DiamondBagWin")
end
local function OnOpenLuckDraw(gameObject)
    local firstPayShopList = _itemMgr.GetShopListByType(5);
    --    UnityTools.MessageDialog("幸运奖励功能开发中....")
    if firstPayShopList ~= nil and #firstPayShopList > 0 then
        
        if firstPayShopList[1].left_times > 0 then
            UnityTools.CreateLuaWin("FirstPayWin");
        else
            local shopCtrl = IMPORT_MODULE("ShopWinController")
            if shopCtrl ~= nil then
                shopCtrl.CtrlData.startTab = 4
            end
            UnityTools.CreateLuaWin("ShopWin");
        end
    end
end

local function OnClickBacktoMainWin()
    if UI.Controller.UIManager.IsWinShow("RoomLvSelectWin") then
        UnityTools.DestroyWin("RoomLvSelectWin")
        local gcCtrl = IMPORT_MODULE("GameCenterWinController")
        gcCtrl.OpenWinByType(2)
    else
        IsShowSelect(false)
        -- UnityTools.DestroyWin("RoomLvSelectWin")
        UnityTools.DestroyWin("GameCenterWin")
        actionMove(false)
    end
end
local function OnClickNormalCow(gameObject)
    UnityTools.ShowMessage(LuaText.funCreating);
    --if _platformMgr.Config.isInitMainWin == false then
    --    LogWarn("[MainWinMono.ToOpen]初始化还没完成就打开游戏了。。。。");
    --    return;
    --end
    --if roomMgr.bExiting == true then
    --    return
    --end
    ---- OpenRoomLvSelect()
    --
    --local cfgData = LuaConfigMgr.BettingRoomConfig["1"]
    --if cfgData ~= nil then
    --    local door = stringToTable(cfgData.doorsill, ",");
    --    local needGod = stringToTable(cfgData.doorsill, ",")[1]
    --
    --    local playerGod = _platformMgr.GetGod() + 0;
    --    if playerGod + 1 >= needGod + 1 then
    --        -- if #door >= 2 and playerGod + 1 < door[2] + 1 then
    --        --     --                    LogWarn("[RoomLvSelectWinMono.OnEnterRoom]"..cData.Id.."  roomType = ".._roomType);
    --        --     CTRL.EnterGame(1, tonumber(1)
    --        -- end
    --        if #door == 1 then
    --            -- LogWarn("[RoomLvSelectWinMono.OnEnterRoom]" .. _roomType .. "...   " .. cData.Id);
    --            GameMgr.EnterGame(1,1,function()
    --                UnityTools.DestroyWin("MainCenterWin")
    --                UnityTools.DestroyWin("MainWin")
    --            end)
    --        end
    --    else
    --        --TODO 点击弹出金币购买
    --        UnityTools.MessageDialog(LuaText.Format("room_select_gold_noEnough", needGod), { okCall = OnTopChangeMoney, okBtnName = LuaText.GetString("goto_lb") });
    --    end
    --    return
    --end
end
local function OnClickRed(gameObject)
    if _platformMgr.GetDiamond() >= 18  then
                GameMgr.EnterGame(1,10,function()
                    UnityTools.DestroyWin("MainCenterWin")
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("GameCenterWin")
                end)
    else
        UnityTools.CreateLuaWin("RedConditionWin")
    end
end
local function OnClickHundredCow(gameObject)
    if _platformMgr.Config.isInitMainWin == false then
        LogWarn("[MainWinMono.ToOpen]初始化还没完成就打开游戏了。。。。");
        return;
    end
    GameMgr.EnterGame(2, 1,
                function()
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("MainCenterWin");
                    UnityTools.DestroyWin("GameCenterWin");
                end);

end
local function OnClickFruit(gameObject)
    if _platformMgr.Config.isInitMainWin == false then
        LogWarn("[MainWinMono.ToOpen]初始化还没完成就打开游戏了。。。。");
        return;
    end
    LogError("roomMgr.bExiting="..tostring(roomMgr.bExiting))
    if roomMgr.bExiting == true then
        return
    end
    roomMgr.bExiting = true
    local protobuf = sluaAux.luaProtobuf.getInstance();
    protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
    registerScriptEvent(EVENT_IS_IN_GAME, "IsCanEnterFruitRoom")
end
local function OnClickGameCenter(gameObject)
    UnityTools.CreateLuaWin("GameCenterWin")
    IsShowSelect(true)
end
function _mainbtns.OnClickBtnRechargeTask(gameObject)
    UnityTools.CreateLuaWin("RechargeTaskWin");
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    local tComponentsOfReviewUnvisible = {}


    btnFree = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnFree")
    UnityTools.AddOnClick(btnFree.gameObject, OnOpenFree)
    _bottom = UnityTools.FindGo(gameObject.transform, "Container/bottom")
    _left = UnityTools.FindGo(gameObject.transform, "Container/left")
    _right = UnityTools.FindGo(gameObject.transform, "Container/right")
    _mainbtns.mainObj =  UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet")
    _mainbtns.btnSeven =  UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnseven") 
    _mainbtns.sevenEffect = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnseven/hongbaofashe")
    UnityTools.AddOnClick(_mainbtns.btnSeven.gameObject, _mainbtns.OnClickSeven)
    _mainbtns.btnBack = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnback")
    UnityTools.AddOnClick(_mainbtns.btnBack.gameObject, OnClickBacktoMainWin)
    _mainbtns.btnMail = UnityTools.FindGo(gameObject.transform, "Container/bottom/btnMail")
    UnityTools.AddOnClick(_mainbtns.btnMail.gameObject, OnOpenMail)

    _mainbtns.btnTask = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnTask")
    UnityTools.AddOnClick(_mainbtns.btnTask.gameObject, OnOpenTask)

    _mainbtns.btnActivity = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnActivity")
    UnityTools.AddOnClick(_mainbtns.btnActivity.gameObject, OnOpenActivity)

    _mainbtns.btnExchange = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnExchange")
    UnityTools.AddOnClick(_mainbtns.btnExchange.gameObject, OnOpenExchange)

    _mainbtns.btnMonthCard = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnMonthCard")
    _mainbtns.redMonthCard = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnMonthCard/red")
    UnityTools.AddOnClick(_mainbtns.btnMonthCard.gameObject, OnOpenCardWin)
    _mainbtns.btnRed = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnRed")
    UnityTools.AddOnClick(_mainbtns.btnRed.gameObject, OnOpenRed)
    
    _mainbtns.btnDiamondBag = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnDiamondBag")
    UnityTools.AddOnClick(_mainbtns.btnDiamondBag.gameObject, OnOpenDiamondBag)
    _mainbtns.btnGetMoney = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnGetMoney")
    _mainbtns.redGetMoney = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnGetMoney/red")
    UnityTools.AddOnClick(_mainbtns.btnGetMoney.gameObject, OnOpenGetMoney)

    _mainbtns.btnGoldPool = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btngoldpool")
    UnityTools.AddOnClick(_mainbtns.btnGoldPool.gameObject, OnOpenGoldPool)

    
    _mainbtns.btn1 = UnityTools.FindGo(gameObject.transform, "Container/right/btn1")
    _mainbtns.effect_hongbao = UnityTools.FindCo(gameObject.transform, "Transform", "Container/right/btn1/hongbaosaoguangtexiao02")
    _mainbtns.btn2 = UnityTools.FindGo(gameObject.transform, "Container/right/btn2")
    _mainbtns.btn3 = UnityTools.FindGo(gameObject.transform, "Container/right/btn3")
    _mainbtns.btn4 = UnityTools.FindGo(gameObject.transform, "Container/right/btn4")
    UnityTools.AddOnClick(_mainbtns.btn1.gameObject, OnClickRed)
    UnityTools.AddOnClick(_mainbtns.btn2.gameObject, OnClickHundredCow)
    UnityTools.AddOnClick(_mainbtns.btn3.gameObject, OnClickNormalCow)
    UnityTools.AddOnClick(_mainbtns.btn4.gameObject, OnClickFruit)

    _mainbtns.diamondBagEffect = UnityTools.FindCo(gameObject.transform, "Transform", "Container/left/rank/mainBtnSet/btnDiamondBag/effect")
    _mainbtns.effect_zhaocaijinniu = UnityTools.FindCo(gameObject.transform, "Transform", "Container/top/bg/mainBtnSet/btnGetMoney/effect_zhaocaijinniu")
    _mainbtns.effect_shangcheng = UnityTools.FindCo(gameObject.transform, "Transform", "Container/bottom/btnShop/shangchenglizi")
    _redBagHint = UnityTools.FindGo(gameObject.transform, "Container/left/rank/mainBtnSet/btnRed/red")
    _redBagHintLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/left/rank/mainBtnSet/btnRed/red/Label")


    _mainbtns.btnShop = UnityTools.FindGo(gameObject.transform, "Container/bottom/btnShop")
    UnityTools.AddOnClick(_mainbtns.btnShop.gameObject, OnOpenShop)

    _mainbtns.btnSetting = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnset")
    UnityTools.AddOnClick(_mainbtns.btnSetting.gameObject, OnOpenSetting)

    -- btnFeedback = UnityTools.FindGo(gameObject.transform, "Container/bottom/pop/btnFeedback")
    -- UnityTools.AddOnClick(btnFeedback.gameObject, OnOpenFeedback)
    btnLuckDraw = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnLuckDraw")
    _mainbtns.spFirst = btnLuckDraw:GetComponent("UISprite")
    _mainbtns.btntarget = btnLuckDraw:GetComponent("UIButton")
    UnityTools.AddOnClick(btnLuckDraw.gameObject, OnOpenLuckDraw)
    


    _taskRed = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnTask/red")

    _taskRedLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bottom/bg/btnTask/red/Label")
    _rankTabGrid = UnityTools.FindGo(gameObject.transform, "Container/left/rank/Grid")

    _rankScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/left/rank/scrollBg/ScrollView")
    _rankScrollView_mgr = UnityTools.FindCoInChild(_rankScrollView, "UIGridCellMgr")
    _rankScrollView_mgr.onShowItem = OnRankItemShow
    _rankScrollView_mgr.onHideItem = _mainbtns.OnRankItemHide
    -- _controller.SetScrollViewRenderQueue(_rankScrollView)

    _freeRed = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnFree/red")

    _freeRedLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bottom/bg/btnFree/red/Label")

    _btnExchangeRed = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnExchange/red")

    _btnExchangeRedLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bottom/bg/btnExchange/red/Label")


    _mailRedSpr = UnityTools.FindGo(gameObject.transform, "Container/bottom/btnMail/red")

    _mailRedLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bottom/btnMail/red/Label")

    _activeRedSpr = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnActivity/red")

    _activeRedLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bottom/bg/btnActivity/red/Label")
    _mainbtns._rankSrollView = UnityTools.FindGo(_go.transform, "Container/left/rank/scrollBg/ScrollView"):GetComponent("UIPanel");
    -- 分享
    btnShare = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnShare")
    UnityTools.AddOnClick(btnShare.gameObject, OnOpenShare)
    _btnShareRed = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg/btnShare/red")
    _btnShareRedLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bottom/bg/btnShare/red/Label")
    _mainbtns._vipBg = UnityTools.FindGo(gameObject.transform, "Container/topleft/vipsp/vip"):GetComponent("UISprite")
    _lbname = UnityTools.FindGo(gameObject.transform, "Container/topleft/lbname"):GetComponent("UILabel")

    _lblevel = UnityTools.FindGo(gameObject.transform, "Container/topleft/lblevel"):GetComponent("UILabel")

    _lbgold = UnityTools.FindGo(gameObject.transform, "Container/topleft/gold/num"):GetComponent("UILabel")

    _lbdiamond = UnityTools.FindGo(gameObject.transform, "Container/topleft/diamond/num"):GetComponent("UILabel")

    _goldAdd = UnityTools.FindGo(gameObject.transform, "Container/topleft/gold/btnadd")
    UnityTools.AddOnClick(_goldAdd.gameObject, OnMoneyAdd)

    _diamondAdd = UnityTools.FindGo(gameObject.transform, "Container/topleft/diamond/btnadd")
    UnityTools.AddOnClick(_diamondAdd.gameObject, OnDiamondExchange)
    _playerInfo = UnityTools.FindGo(gameObject.transform, "Container/topleft/headmask/headBg")
    UnityTools.AddOnClick(_playerInfo.gameObject, OnOpenPlayerInfo)
    _headTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "Container/topleft/headmask/headBg/Texture")
    _playerHeadIcon  = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/topleft/headmask/headBg/headIcon")

    _noticeTable.panel = UnityTools.FindCo(gameObject.transform, "UIPanel", "Container/topleft/Panel")
    _noticeTable.label = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/topleft/Panel/word")
    _noticeTable.tween = _noticeTable.label.gameObject:GetComponent("TweenPosition")
    EventDelegate.Add(_noticeTable.tween.onFinished,OnMoveFinished)
    _mainbtns.redpanel =  UnityTools.FindGo(gameObject.transform, "Container/topleft/redpanel"):GetComponent("UIPanel")
    _mainbtns.mainWinRed = UnityTools.FindGo(gameObject.transform, "Container/topleft/redpanel/red")
    _mainbtns.showGirl =  UnityTools.FindGo(gameObject.transform, "Container/girl")
    _mainbtns.bottombg = UnityTools.FindGo(gameObject.transform, "Container/bottom/bg"):GetComponent("UISprite")

    local value= UnityEngine.PlayerPrefs.GetInt("MainWinRedIsShow",0)
    if value == 0 then 
        UnityTools.SetActive(_mainbtns.mainWinRed.gameObject,true)
    else
        UnityTools.SetActive(_mainbtns.mainWinRed.gameObject,false)
    end

    _headmask = UnityTools.FindGo(gameObject.transform, "Container/topleft/headmask"):GetComponent("UIPanel")

    _mainbtns.btnRechargeTask = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnRechageTask")
    _mainbtns.btnRechargeTaskRed = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnRechageTask/red")
    UnityTools.AddOnClick(_mainbtns.btnRechargeTask.gameObject, _mainbtns.OnClickBtnRechargeTask)

--- [ALB END]


    _mainbtns.btnGoldPool.gameObject:SetActive(false)
    
    
    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        -- 调整位置
        -- _mainbtns.btnActivity.transform.position =  btnShare.transform.position
        table.insert(tComponentsOfReviewUnvisible, _left)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btn1)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnTask)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnActivity)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnGetMoney)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnDiamondBag)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnRed)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnMonthCard)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnExchange) -- 兑换
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnSeven)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnRechargeTask)
        
        table.insert(tComponentsOfReviewUnvisible, btnShare) -- 分享
        table.insert(tComponentsOfReviewUnvisible, btnLuckDraw)   -- 首冲礼包
        table.insert(tComponentsOfReviewUnvisible, btnFree)  -- 免费
        for k, v in pairs(tComponentsOfReviewUnvisible) do
            v:SetActive(false)
        end
        _mainbtns.btnMail.transform.localPosition = UnityEngine.Vector3(-88,_mainbtns.btnMail.transform.localPosition.y,0)
        _mainbtns.btnShop.transform.localPosition = UnityEngine.Vector3(59,_mainbtns.btnShop.transform.localPosition.y,0)
        _mainbtns.bottombg.width = 494
        _mainbtns.showGirl.gameObject:SetActive(true)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError(" 审核版本：屏蔽主界面部分功能")
    -- elseif version.VersionData.isAppStoreVersion() then  -- 苹果商店版本
        
    --     table.insert(tComponentsOfReviewUnvisible, btnLuckDraw)   -- 首冲礼包
    --     for k, v in pairs(tComponentsOfReviewUnvisible) do
    --         v:SetActive(false)
    --     end
    end
    tComponentsOfReviewUnvisible = nil
    if GameDataMgr.PLAYER_DATA.Block ~= 0 then
        local blockTb = {}
        table.insert(blockTb, _left)
        table.insert(blockTb, _right)
        table.insert(blockTb, btnLuckDraw)   -- 首冲礼包
        table.insert(blockTb, _mainbtns.btnMonthCard)
        table.insert(blockTb, _mainbtns.mainObj)
        for k, v in pairs(blockTb) do
            v:SetActive(false)
        end
        blockTb = nil
    end
end
local function ChangeHeadIcon()
    local playerIcon = _platformMgr.GetIcon();
    if playerIcon ~= nil and playerIcon ~= "" then
        UnityTools.SetPlayerHead(_platformMgr.GetIcon(), _headTexture, true);
        --[[else
            _playerHeadIcon.spriteName = PlatformMg.PlayerDefaultHead();]]
    end
    _playerHeadIcon.spriteName = _platformMgr.PlayerDefaultHead();
end
local function InitResource()
    GameDataMgr.PLAYER_DATA:LoginGameServer();
    _lbname.text = _platformMgr.UserName()
    _lblevel.text = LuaText.Format("LvText", _platformMgr.Lv());
    _lbgold.text = UnityTools.GetShortNum(_platformMgr.GetGod())
    _lbdiamond.text = UnityTools.GetShortNum(_platformMgr.GetDiamond())
    _mainbtns._vipBg.spriteName = "v".._platformMgr.GetVipLv()
    -- UnityTools.SetNewVipBox(_vipBg, _platformMgr.GetVipLv())
    ChangeHeadIcon();
end
function OnMainWinResourceUpdate(msgId, value)
    FreeRedUpdate();
    ExchangeRedUpdate();
    if value == 1 then
        InitResource()
    end
end

--- 不显示的功能
local function HideFun()

    local configVip = _platformMgr.config_vip;

    if version.VersionData.IsReviewingVersion() then
        -- 调整位置
        -- _mainbtns.btnActivity.transform.position =  btnShare.transform.position
        local tComponentsOfReviewUnvisible = {}
        table.insert(tComponentsOfReviewUnvisible, _left)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btn1)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnTask)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnActivity)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnGetMoney)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnDiamondBag)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnRed)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnMonthCard)
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnExchange) -- 兑换
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnSeven) -- 兑换
        table.insert(tComponentsOfReviewUnvisible, _mainbtns.btnRechargeTask)
        table.insert(tComponentsOfReviewUnvisible, btnShare) -- 分享
        table.insert(tComponentsOfReviewUnvisible, btnLuckDraw)   -- 首冲礼包
        table.insert(tComponentsOfReviewUnvisible, btnFree)  -- 免费
        for k, v in pairs(tComponentsOfReviewUnvisible) do
            v:SetActive(false)
        end
        _mainbtns.btnMail.transform.localPosition = UnityEngine.Vector3(-88,_mainbtns.btnMail.transform.localPosition.y,0)
        _mainbtns.btnShop.transform.localPosition = UnityEngine.Vector3(59,_mainbtns.btnShop.transform.localPosition.y,0)
        _mainbtns.bottombg.width = 494
        _mainbtns.showGirl.gameObject:SetActive(true)
        tComponentsOfReviewUnvisible = nil
    -- 审核版本屏蔽内容
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError(" 审核版本：屏蔽主界面兑换功能")
    else
        _mainbtns.btnExchange:SetActive(configVip);
         _mainbtns.btnActivity:SetActive(configVip);
        _mainbtns.btnMail:SetActive(configVip);
        _mainbtns.btnTask:SetActive(configVip);
        btnFree:SetActive(configVip);
        _left:SetActive(configVip);
    end
    if GameDataMgr.PLAYER_DATA.Block ~= 0 then
        local blockTb = {}
        table.insert(blockTb, _left)
        table.insert(blockTb, _right)
        table.insert(blockTb, btnLuckDraw)   -- 首冲礼包
        table.insert(blockTb, _mainbtns.btnMonthCard)
        table.insert(blockTb, _mainbtns.mainObj)
        table.insert(blockTb, _mainbtns.btnRechargeTask)
        for k, v in pairs(blockTb) do
            v:SetActive(false)
        end
        blockTb = nil
    end
   
end

function OnChangeFunOpen(msgId, config_vip)
    HideFun();
end
function OnMainWinRenderChange()
    InitAllScroll()
end

function OnPlayerHeadChangeHandler(msgId, value)
    ChangeHeadIcon();
end
function OnTopDiamondBag(eventId,value)
    if value then
        _mainbtns.diamondBagEffect.gameObject:SetActive(false)
    elseif _mainbtns.diamondBagEffect.gameObject.activeSelf == false then
        _mainbtns.diamondBagEffect.gameObject:SetActive(true)
    end
 end
function OnUpdateRechargeTask()
    local rechargeTaskCtrl = IMPORT_MODULE("RechargeTaskWinController")
    if rechargeTaskCtrl.IsOpen == 2 then
        _mainbtns.btnRechargeTask.gameObject:SetActive(false)
    else
        _mainbtns.btnRechargeTask.gameObject:SetActive(true)
    end
    _mainbtns.btnRechargeTaskRed.gameObject:SetActive(rechargeTaskCtrl.IsShowRed)
end
local function Awake(gameObject)
    _go = gameObject;
    if GameDataMgr.LOGIN_DATA.IsConnectGamerServer then 
        local protobuf = sluaAux.luaProtobuf.getInstance();
        protobuf:sendMessage(protoIdSet.cs_task_pay_info_request, {})
    end
    _platformMgr.Config.centerAwake = true;
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    registerScriptEvent(UPDATE_MAIN_WIN_RED, "OnUpdateMainWinRed")
    registerScriptEvent(EVENT_RESCOURCE_UDPATE, "OnMainWinResourceUpdate")
    registerScriptEvent(EVNET_HIDE_FUNCTION_FLAG_UDPATE, "OnChangeFunOpen")
    registerScriptEvent(EVENT_RENDER_CHANGE_WIN, "OnMainWinRenderChange")
    registerScriptEvent(EVENT_FIRST_PAY, "OnEventFirstPay")
    registerScriptEvent(DIAMOND_BAG_UPDATE, "OnTopDiamondBag");
    registerScriptEvent(END_CHANGE_PLAYER_HEAD, "OnPlayerHeadChangeHandler");
    registerScriptEvent(EVENT_ADD_NOTICE, "OnNoticeShow");
    registerScriptEvent(UPDATE_LUCKY_COW_WIN, "UpdateMainWinLuckyCowRed");
    registerScriptEvent(EVENT_MONTH_CARD_UPDATE, "UpdateMainWinMonthCardRed");
    registerScriptEvent(EVENT_UPDATE_RECHARGE_TASK, "OnUpdateRechargeTask")
    UnityTools.DestroyWin("BroadcastWin")
    
    --    HideFun();
    --    gTimer.registerOnceTimer(500,HideFun);
end

local function NextWait()
    --- 更新红点显示
    TaskRedUpdate();
    ExchangeRedUpdate();
    FreeRedUpdate()
    MailRedUpdate()
    ActivityRedUpdate()
    ShareAwardRedUpdate()
    -- UI.Controller.UIManager.RemoveAllWinExpect({"Waiting","MainCenterWin","MainWin"})
    if _platformMgr.gameMgr.closeActiveFun ~= nil then
        _platformMgr.gameMgr.closeActiveFun()
        _platformMgr.gameMgr.closeActiveFun = nil;
    end
    if _platformMgr.GetGuideStep() == 2 then
        actionMove(false)
    end
    LogError("roomMgr.bExiting="..tostring(roomMgr.bExiting))
end

local function initWin()
    
    gTimer.registerOnceTimer(200, NextWait);
end
local function cardInit(gameObject)
    InitAllScroll()
    gTimer.registerOnceTimer(150, initWin)
end

local function waitStart()
    
    UpdateRedBagHint();
    
    ---隐藏钻石福袋上的特效
    local diamondBagCtrl = IMPORT_MODULE("DiamondBagWinController")
    if diamondBagCtrl ~= nil and diamondBagCtrl.data.todayIsBuy then
        _mainbtns.diamondBagEffect.gameObject:SetActive(false)
    end
    if CTRL.SkipId ~= 0 then
        GoTo(CTRL.SkipId,wName)
        CTRL.SkipId = 0
    end
    ShowRank(1);
    
end

local function ResetEffectRenderQ(go)
    UtilTools.SetEffectRenderQueueByUIParent(go.transform, _mainbtns.effect_zhaocaijinniu, 1);
    UtilTools.SetEffectRenderQueueByUIParent(go.transform, _mainbtns.effect_shangcheng, 10);
    UtilTools.SetEffectRenderQueueByUIParent(go.transform, _mainbtns.effect_hongbao, 50);
    UtilTools.SetEffectRenderQueueByUIParent(go.transform, _mainbtns.sevenEffect.transform, 50);
    
    
end

function OnMainCitySevenTaskStatusUpdate()
    local sevenTaskCtrl = IMPORT_MODULE("SevenDailyTaskWinController")
    if sevenTaskCtrl.taskId == 0 then
        _mainbtns.btnSeven.gameObject:SetActive(false)
        return
    end
    _mainbtns.btnSeven.gameObject:SetActive(true)
    local red = UnityTools.FindGo(_mainbtns.btnSeven.gameObject.transform, "red")
    if sevenTaskCtrl.status == 1 and sevenTaskCtrl.award ~= 1 then
        red:SetActive(true)
    else
        red:SetActive(false)
    end
end

local function Start(gameObject)
    
    IsShowSelect(false)
    ResetEffectRenderQ(gameObject)
    InitResource()
    SetTopBtnShow()
    gTimer.registerOnceTimer(50, cardInit, gameObject);
    gTimer.registerOnceTimer(500, waitStart);
    InitRank();
    
    roomMgr.bExiting =false
    UpdateMainWinLuckyCowRed()
    UpdateMainWinMonthCardRed()
    OnUpdateRechargeTask()
    OnMainCitySevenTaskStatusUpdate()

    --- 牌局弹出时会被调用
    if _platformMgr.Config.isInitMainWin then
        HideFun()
    end
    local protobuf = sluaAux.luaProtobuf.getInstance();
    protobuf:sendMessage(protoIdSet.cs_real_name_req, {})

    registerScriptEvent(EVENT_SEVEN_TASK_STATUS_UPDATE, "OnMainCitySevenTaskStatusUpdate")
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_UPDATE_RECHARGE_TASK, "OnUpdateRechargeTask")
    unregisterScriptEvent(UPDATE_MAIN_WIN_RED, "OnUpdateMainWinRed")
    unregisterScriptEvent(EVENT_RESCOURCE_UDPATE, "OnMainWinResourceUpdate")
    unregisterScriptEvent(EVNET_HIDE_FUNCTION_FLAG_UDPATE, "OnChangeFunOpen")
    unregisterScriptEvent(EVENT_RENDER_CHANGE_WIN, "OnMainWinRenderChange")
    unregisterScriptEvent(EVENT_SEVEN_TASK_STATUS_UPDATE, "OnMainCitySevenTaskStatusUpdate")
    unregisterScriptEvent(EVENT_FIRST_PAY, "OnEventFirstPay")
    unregisterScriptEvent(DIAMOND_BAG_UPDATE, "OnTopDiamondBag");
    unregisterScriptEvent(END_CHANGE_PLAYER_HEAD, "OnPlayerHeadChangeHandler");
    unregisterScriptEvent(EVENT_ADD_NOTICE, "OnNoticeShow");
    unregisterScriptEvent(UPDATE_LUCKY_COW_WIN, "UpdateMainWinLuckyCowRed");
    unregisterScriptEvent(EVENT_MONTH_CARD_UPDATE, "UpdateMainWinMonthCardRed");
    gTimer.removeTimer(waitStart);
    gTimer.removeTimer(initWin)
    gTimer.removeTimer(NextWait)
    gTimer.removeTimer(HideFun)
    gTimer.removeTimer(cardInit)
    CLEAN_MODULE(wName .. "Mono")
end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
--M.OnSetIndex = OnSetIndex;
M.OpenRoomLvSelect = OpenRoomLvSelect;
M.OpenHundredGame = OpenHundredGame;
M.UpdateRankList = UpdateRankList;
--UI.Controller.UIManager.RegisterLuaFuncCall("MainWinMono:OnSetIndex", OnSetIndex);
-- 返回当前模块
return M
