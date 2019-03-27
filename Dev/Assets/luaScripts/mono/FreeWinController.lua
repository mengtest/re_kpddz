-- -----------------------------------------------------------------


-- *
-- * Filename:    FreeWinController.lua
-- * Summary:     免费界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/4/2017 3:47:33 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FreeWinController")
local UnityTools = IMPORT_MODULE("UnityTools");
local _platformMgr = IMPORT_MODULE("PlatformMgr");
local protobuf = sluaAux.luaProtobuf.getInstance();
local roomMgr = IMPORT_MODULE("roomMgr")
local GameMgr = IMPORT_MODULE("GameMgr")

-- 界面名称
local wName = "FreeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _data = {}


local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function DestroyWin(winName)
    if UI.Controller.UIManager.IsWinShow(winName) then
        UnityTools.DestroyWin(winName);
    end
end

function OnClearFreeControllerData(value)
    _data = {}
end



registerScriptEvent(EXIT_CLEAR_ALL_DATA, "OnClearFreeControllerData")
--- desc:是否获取绑定手机的奖励
-- YQ.Qu:2017/5/5 0005
-- @param key
-- @return
local function IsShowBindingPhone(key)
    return key == "600005" and  _platformMgr.IsTouris() == false and _data.isDraw
end
---desc:获取手机信息
--YQ.Qu:2017/5/5 0005
-- @param
-- @return
local function GetData()
    return _data
end

--- desc:
--- YQ.Qu
function OnPlayerPhoneNumInfoUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    _data.phoneNum = tMsgData.phone_num;
    _data.isDraw = tMsgData.is_draw;
    GameDataMgr.PLAYER_DATA.IsTouris = _data.phoneNum == "0"
--    LogWarn("[FreeWinController.OnPlayerPhoneNumInfoUpdate]" .. _data.phoneNum .. " is draw = " .. tostring(_data.isDraw));
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"free")
end

---desc:领取手机绑定奖励
---YQ.Qu
function OnPlayerBindPhoneNumDrawReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        if tMsgData.rewards ~= nil and #tMsgData.rewards > 0 then
            ShowAwardWin(tMsgData.rewards)
        end
        _data.isDraw = true;
        if UnityTools.IsWinShow("FreeWin") then
            local mono = IMPORT_MODULE("FreeWinMono")
            if mono ~= nil then
                mono.UpdateBinding()
            end
        end
 
        triggerScriptEvent(UPDATE_MAIN_WIN_RED,"free")
    else
        UnityTools.ShowMessage(tMsgData.err)
    end
end
function IsCanEnterFruitRoom(idMsg,tMsgData)
    unregisterScriptEvent(EVENT_IS_IN_GAME, "IsCanEnterFruitRoom")
    if tMsgData.room_id <=0 then
        if _platformMgr.Config:CanIntoFruit() then
            
            DestroyWin(wName);
            DestroyWin("TaskWin")
            UnityTools.DestroyWin("RedConditionWin")
            local fruitCtrl = IMPORT_MODULE("FruitWinController")
            fruitCtrl.InitLis()
            protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{type = 1}) --进入拉霸
        else
            roomMgr.bExiting = false
        end
    else
        roomMgr.bExiting = false
        GameMgr.EnterGame(1,tMsgData.room_id,function()
            UnityTools.DestroyWin("MainCenterWin")
            UnityTools.DestroyWin("MainWin")
            UnityTools.DestroyWin("FreeWin")
            UnityTools.DestroyWin("RedConditionWin")
        end)
    end
end
function IsCanEnterRichCar(idMsg,tMsgData)
    unregisterScriptEvent(EVENT_IS_IN_GAME_FOR_CAR, "IsCanEnterRichCar")
 
    if tMsgData.room_id <=0 then
        if _platformMgr.Config:CanIntoCar() then
            DestroyWin(wName);
            DestroyWin("TaskWin")
            UnityTools.DestroyWin("RedConditionWin")
            protobuf:sendMessage(protoIdSet.cs_car_enter_req, {}) --进入豪车
        end
    else
        GameMgr.EnterGame(1,tMsgData.room_id,function()
            UnityTools.DestroyWin("MainCenterWin")
            UnityTools.DestroyWin("MainWin")
            UnityTools.DestroyWin("FreeWin")
            UnityTools.DestroyWin("RedConditionWin")
        end)
    end
end

function GoTo(value, winName)

    local delayDestroyTime = 100 --延迟删除界面
    winName = winName or "FreeWin";
    LogWarn("[FreeWinController.GoTo]" .. value .. type(value));
    local id = 0;
    if type(value) == "string" then
        id = value + 0;
    else
        id = value;
    end
    if id == 101 then --去百人大战
        local mainWin = IMPORT_MODULE("MainWinMono");
        if mainWin ~= nil then
            mainWin.OpenHundredGame();
        else
            mainWin = IMPORT_MODULE("NormalCowMainMono");
            if mainWin ~= nil then
                local mainCityCtrl = IMPORT_MODULE("MainWinController");
                mainCityCtrl.SkipId = 101 
                mainWin.CloseWin(0);
                UnityTools.DestroyWin("RedConditionWin")
            end
        end
        DestroyWin("TaskWin")
        DestroyWin(winName);
    elseif id == 102 then --去礼包界面
        UnityTools.ShowMessage(LuaText.funCreating);  --功能未开放

    elseif id == 103 then --到签到
        --        UnityTools.ShowMessage("功能开发中...");

        openLoginAwardWin();
        gTimer.registerOnceTimer(delayDestroyTime,DestroyWin,winName)
--        DestroyWin(winName);
    elseif id == 104 then --去游戏
        UnityTools.ShowMessage(LuaText.funCreating);  --功能未开放

        --if _platformMgr.Config.isInitMainWin == false then
        --    LogWarn("[MainWinMono.ToOpen]初始化还没完成就打开游戏了。。。。");
        --    return;
        --end
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
        --
        --                DestroyWin(winName);
        --                UnityTools.DestroyWin("MainCenterWin")
        --                UnityTools.DestroyWin("MainWin")
        --                UnityTools.DestroyWin("GameCenterWin")
        --            end)
        --        end
        --    else
        --        --TODO 点击弹出金币购买
        --        UnityTools.MessageDialog(LuaText.Format("room_select_gold_noEnough", needGod), { okCall = OnTopChangeMoney, okBtnName = LuaText.GetString("goto_lb") });
        --    end
        --    return
        --end
    elseif id == 105 then --去充值
        local shopCtrl = IMPORT_MODULE("ShopWinController");
        if shopCtrl ~= nil then
            shopCtrl.OpenShop(1)
--            DestroyWin(winName);
            gTimer.registerOnceTimer(delayDestroyTime,DestroyWin,winName)
        end

    elseif id == 106 then --去查看任务
        --        UnityTools.ShowMessage("功能开发中...");
        UnityTools.CreateLuaWin("TaskWin")
        gTimer.registerOnceTimer(delayDestroyTime,DestroyWin,winName)
--        DestroyWin(winName);
    elseif id == 107 then --去绑定界面
        --        UnityTools.CreateWin("RegisterBindingWin");
        UtilTools.BindingPhone();
--        DestroyWin(winName);
        gTimer.registerOnceTimer(delayDestroyTime,DestroyWin,winName)

    elseif id == 108 then --分享朋友圈
        DestroyWin(winName);
        UnityTools.CreateLuaWin("NewShareWin")
        -- UnityTools.ShowMessage("功能开发中...");
    elseif id == 109 then --修改头像
        UnityTools.CreateLuaWin("PlayerInfoWin");
        UnityTools.CreateLuaWin("PlayerPicSelectWin");
--        DestroyWin(winName);
        gTimer.registerOnceTimer(delayDestroyTime,DestroyWin,winName)
    elseif id == 110 then --修改昵称
        UnityTools.CreateLuaWin("PlayerInfoWin");
--        DestroyWin(winName);
        gTimer.registerOnceTimer(delayDestroyTime,DestroyWin,winName)
    elseif id == 111 then --添加好友
        UnityTools.ShowMessage(LuaText.funCreating);  --功能未开放

    elseif id == 112 then --赏金牛仔
        --        UnityTools.ShowMessage("功能开发中...");DestroyWin(winName);

        if _platformMgr.Config.isInitMainWin == false then
            LogWarn("[MainWinMono.ToOpen]初始化还没完成就打开游戏了。。。。");
            return;
        end
        local cfgData = LuaConfigMgr.BettingRoomConfig["1"]
        if cfgData ~= nil then
            local door = stringToTable(cfgData.doorsill, ",");
            local needGod = stringToTable(cfgData.doorsill, ",")[1]

            local playerGod = _platformMgr.GetGod() + 0;
            if playerGod + 1 >= needGod + 1 then
                -- if #door >= 2 and playerGod + 1 < door[2] + 1 then
                --     --                    LogWarn("[RoomLvSelectWinMono.OnEnterRoom]"..cData.Id.."  roomType = ".._roomType);
                --     CTRL.EnterGame(1, tonumber(1)
                -- end
                if #door == 1 then
                    -- LogWarn("[RoomLvSelectWinMono.OnEnterRoom]" .. _roomType .. "...   " .. cData.Id);
                    GameMgr.EnterGame(1,1,function()

                        DestroyWin(winName);
                        UnityTools.DestroyWin("MainCenterWin")
                        UnityTools.DestroyWin("MainWin")
                        UnityTools.DestroyWin("GameCenterWin")
                    end)
                end
            else
                --TODO 点击弹出金币购买
                UnityTools.MessageDialog(LuaText.Format("room_select_gold_noEnough", needGod), { okCall = OnTopChangeMoney, okBtnName = LuaText.GetString("goto_lb") });
            end
            return
        end

    elseif id == 113 then --水果狂欢
        --UnityTools.ShowMessage("功能开发中...");
        --        protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{})
        local mainWin = IMPORT_MODULE("NormalCowMainMono");
        if mainWin ~= nil then
            local mainCityCtrl = IMPORT_MODULE("MainWinController");
            mainCityCtrl.SkipId = 113 
            mainWin.CloseWin(0);
            UnityTools.DestroyWin("RedConditionWin")
            return
        end
        

        local protobuf = sluaAux.luaProtobuf.getInstance();
        protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
        registerScriptEvent(EVENT_IS_IN_GAME, "IsCanEnterFruitRoom")
        -- if _platformMgr.Config:CanIntoFruit() then
        --     DestroyWin(winName);
        --     protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req, {}) --进入拉霸
        -- end
    elseif id == 114 then --水果狂欢
        --UnityTools.ShowMessage("功能开发中...");
        --        protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{})
        local protobuf = sluaAux.luaProtobuf.getInstance();
        protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
        registerScriptEvent(EVENT_IS_IN_GAME_FOR_CAR, "IsCanEnterRichCar")
        -- if _platformMgr.Config:CanIntoFruit() then
        --     DestroyWin(winName);
        --     protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req, {}) --进入拉霸
        -- end
    elseif id == 1001 then --领取破产补助
        _platformMgr.SubsidyOpen(1, function()
            DestroyWin(winName);
        end);
    elseif id == 1002 then --分享朋友圈
        UnityTools.ShowMessage("功能开发中...");
    elseif id == 1003 then --打开每日登陆礼包
        openLoginAwardWin();
        DestroyWin(winName);
    elseif id == 1004 then
        local req ={}
        protobuf:sendMessage(protoIdSet.cs_player_bind_phone_num_draw,req);
    elseif id == 120 then
        DestroyWin(winName);
        UnityTools.CreateLuaWin("DiamondBagWin");
        
    elseif id == 121 then
        DestroyWin(winName);
        UnityTools.CreateLuaWin("ExchangeWin");
    elseif id == 122 then
        DestroyWin(winName);
        if _platformMgr.GetDiamond() >= 18  then
                GameMgr.EnterGame(1,10,function()
                    UnityTools.DestroyWin("MainCenterWin")
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("GameCenterWin")
                end)
        else
            UnityTools.CreateLuaWin("RedConditionWin")
        end
    elseif id == 123 then
        DestroyWin(winName);
        UnityTools.CreateLuaWin("LuckyCowWin");
    end

    
end




UI.Controller.UIManager.RegisterLuaWinFunc("FreeWin", OnCreateCallBack, OnDestoryCallBack)

protobuf:registerMessageScriptHandler(protoIdSet.sc_player_phone_num_info_update, "OnPlayerPhoneNumInfoUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_player_bind_phone_num_draw_reply, "OnPlayerBindPhoneNumDrawReply")
-- 返回当前模块
M.IsShowBindingPhone = IsShowBindingPhone
M.Data = GetData
return M
