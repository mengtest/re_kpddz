-- -----------------------------------------------------------------


-- *
-- * Filename:    RoomLvSelectWinMono.lua
-- * Summary:     房间等级选择界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/18/2017 10:55:48 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RoomLvSelectWinMono")
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 界面名称
local wName = "RoomLvSelectWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local GameMgr = IMPORT_MODULE("GameMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local ScrollView
local ScrollView_mgr
local grid
local btnQuickStart
local _roomType = 1
local _roomPlayerNumList
local _roomLimits = {}
--- [ALD END]
local isShow = true
local roomKeyTb = {"1","3"}
--desc:
--YQ.Qu:2017/2/21 0021
local function OnTopChangeMoney()
    --    UnityTools.ShowMessage("功能开发中...");
    UnityTools.CreateLuaWin("ShopWin");
end

local function OnHaveToEnterRoom(playerGod)
    local roomType = 2;
    --[[for i = #_roomLimits, #_roomLimits, -1 do
        if _roomLimits[i].min + 0 < playerGod then
            roomType = i;
            break;
        end
    end]]
    for i = #roomKeyTb, 1, -1 do
        if _roomLimits[roomKeyTb[i]+0].min == _roomLimits[roomKeyTb[i]+0].max and _roomLimits[roomKeyTb[i]+0].min <= playerGod then
            roomType = i;
        elseif _roomLimits[roomKeyTb[i]+0].min <= playerGod and _roomLimits[roomKeyTb[i]+0].max > playerGod then
            roomType = i;
            break;
        end
    end
    return roomType;
end

--desc:点击房间反应
--YQ.Qu:2017/2/18 0018
local function OnEnterRoom(gameObj) 
    if _roomType == 2 then
        UnityTools.ShowMessage("功能开发中...");
        return;
    end
    local cData = gameObj:GetComponent("ComponentData")
    if cData ~= nil then
        local key = cData.Id
        local cfgData = LuaConfigMgr.BettingRoomConfig[roomKeyTb[key]]
        if cfgData ~= nil then
            local door = stringToTable(cfgData.doorsill, ",");
            local needGod = stringToTable(cfgData.doorsill, ",")[1]

            local playerGod = platformMgr.GetGod() + 0;
            if playerGod + 1 >= needGod + 1 then
                if #door >= 2 and playerGod + 1 < door[2] + 1 then
                    --                    LogWarn("[RoomLvSelectWinMono.OnEnterRoom]"..cData.Id.."  roomType = ".._roomType);
                    CTRL.EnterGame(_roomType, tonumber(roomKeyTb[cData.Id]))
                elseif #door >= 2 and playerGod + 1 >= door[2] + 1 then
                    local nextRoom = OnHaveToEnterRoom(playerGod);
                    local roomName = LuaConfigMgr.BettingRoomConfig[roomKeyTb[nextRoom]].name;
                    UnityTools.MessageDialog(LuaText.Format("room_select_gold_too_much", roomName), {
                        okBtnName = LuaText.to_room_go .. roomName,
                        okCall = function()
                            LogWarn("[RoomLvSelectWinMono.anon]_roomType= " .. _roomType .. "  nextRoom = " .. nextRoom);
                            CTRL.EnterGame(_roomType, tonumber(roomKeyTb[nextRoom]));
                        end
                    })
                end
                if #door == 1 then
                    LogWarn("[RoomLvSelectWinMono.OnEnterRoom]" .. _roomType .. "...   " .. cData.Id);
                    CTRL.EnterGame(_roomType, tonumber(roomKeyTb[cData.Id]))
                end
            else
                --TODO 点击弹出金币购买
                UnityTools.MessageDialog(LuaText.Format("room_select_gold_noEnough", needGod), { okCall = OnTopChangeMoney, okBtnName = LuaText.GetString("goto_lb") });
            end
            return
        end
        LogError("_roomType3=".._roomType..",roomKeyTb"..roomKeyTb[cData.Id])
        CTRL.EnterGame(_roomType, tonumber(roomKeyTb[cData.Id]))
    else
        LogWarn("[RoomLvSelectWinMono.OnEnterRoom]:出错了。。。。。");
        CTRL.EnterGame(1, 1)
    end
end
local function OnRoomShowItem(cellbox, index, item)
    local cData = item:GetComponent("ComponentData");
    if cData ~= nil then

        local itemSpr = item:GetComponent("UISprite");
        if itemSpr ~= nil then itemSpr.spriteName = "room" .. index end
        cData.Id = index + 1;
    end
    local key = index + 1;
    local info = LuaConfigMgr.BettingRoomConfig[roomKeyTb[key]]
    local effect = UnityTools.FindGo(item.transform, "effect_fangjian")
    UtilTools.SetEffectRenderQueueByUIParent(item.transform, effect.transform, 1);
    if info == nil then
        LogWarn("[RoomLvSelectWinMono.OnRoomShowItem]key = " .. key);
    end

    local peopleLb = UnityTools.FindCo(item.transform, "UILabel", "people/Label");
    peopleLb.text = "1000";
    if _roomPlayerNumList ~= nil then
        peopleLb.text = _roomPlayerNumList[key].player_num;
    end
    local needLb = UnityTools.FindCo(item.transform, "UILabel", "need");
    needLb.text = info.door_des;
    local sysPayLb = UnityTools.FindCo(item.transform, "UILabel", "sysPayLb");
    sysPayLb.text = info.score




    UnityTools.AddOnClick(item, OnEnterRoom)
    local effect = UnityTools.FindGo(item.transform, "effect_fangjian");
    effect:SetActive(true);
    if effect ~= nil then
        effect:SetActive(false);
        gTimer.registerOnceTimer(math.random(0, 1000), function()
            if UI.Controller.UIManager.IsWinShow("RoomLvSelectWin") then
                if isShow then
                    effect:SetActive(true);
                end
            end
        end)
    end
end

local function OnQuickStartGame(gameObject)
    if _roomType == 2 then
        UnityTools.ShowMessage("功能开发中...");
        return;
    end

    local playerGod = platformMgr.GetGod()
    local roomIndex = 1;
    for i = 1, 3 do
        --        LogWarn("[RoomLvSelectWinMono.OnQuickStartGame]"..LuaConfigMgr.BettingRoomConfig[i..""].name);
        local cfgData = LuaConfigMgr.BettingRoomConfig[roomKeyTb[i]]
        local door = stringToTable(cfgData.doorsill, ",");
        local needGod = stringToTable(cfgData.doorsill, ",")[1]
        if i == 1 and playerGod < tonumber(needGod) then
            UnityTools.MessageDialog(LuaText.Format("room_select_gold_noEnough", needGod), { okCall = OnTopChangeMoney, okBtnName = LuaText.GetString("goto_lb") });
            return
        end

        if #door > 1 and playerGod >= tonumber(needGod) and playerGod < tonumber(door[2]) then
            roomIndex = tonumber(roomKeyTb[i]);
            break
        end

        if #door == 1 and playerGod >= tonumber(door[1]) then
            roomIndex = tonumber(roomKeyTb[i]);
            break;
        end
    end
    -- LogWarn("[RoomLvSelectWinMono.OnQuickStartGame]快速进入的房间是：" .. LuaConfigMgr.BettingRoomConfig[roomIndex .. ""].name);
    CTRL.EnterGame(_roomType, roomIndex);
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    ScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/ScrollView")
    ScrollView_mgr = UnityTools.FindCoInChild(ScrollView, "UIGridCellMgr")
    ScrollView_mgr.onShowItem = OnRoomShowItem
    --    _controller:SetScrollViewRenderQueue(ScrollView)

    grid = UnityTools.FindCo(gameObject.transform, "UIGridCellMgr", "Container/ScrollView/grid")

    btnQuickStart = UnityTools.FindGo(gameObject.transform, "Container/btnQuickStart")
    UnityTools.AddOnClick(btnQuickStart.gameObject, OnQuickStartGame)

    --- [ALB END]
end


--desc:刷新列表
--YQ.Qu:2017/2/18 0018
local function UpdateList(isReset)
    ScrollView_mgr:ClearCells();
    for i = 1, LuaConfigMgr.BettingRoomConfigLen -1 do
        ScrollView_mgr:NewCellsBox(ScrollView_mgr.Go);
    end
    --[[for k, v in pairs(LuaConfigMgr.BettingRoomConfig) do
        ScrollView_mgr:NewCellsBox(ScrollView_mgr.Go);

    end]]
    if ScrollView_mgr.Grid == nil then
        return
    end
    ScrollView_mgr.Grid:Reposition()
    ScrollView_mgr:UpdateCells();
end


--自适应后快速开始的按钮位置设置
local function SetQuickStartBtnPos(go)
    local containerW = UnityTools.FindCo(go.transform, "UIWidget", "Container");
    local scrollPanel = UnityTools.FindCo(go.transform, "UIPanel", "Container/ScrollView");
    if containerW == nil or scrollPanel == nil then
        return;
    end
    local btnY = -(containerW.height - scrollPanel.height) / 4 - scrollPanel.height / 2;
    -- btnQuickStart.transform.localPosition = UnityEngine.Vector3(btnQuickStart.transform.localPosition.x, btnY, 0);
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    for k, v in pairs(LuaConfigMgr.BettingRoomConfig) do
        local door = stringToTable(v.doorsill, ",");
        _roomLimits[k + 0] = { min = door[1] + 0 };
        if #door > 1 then
            _roomLimits[k + 0].max = door[2] + 0;
        else
            _roomLimits[k + 0].max = door[1] + 0;
        end
    end
end


local function Start(gameObject)
    _roomType = CTRL.RoomType();

    local _scrollView = UnityTools.FindGo(gameObject.transform, "Container/ScrollView")
    _controller:SetScrollViewRenderQueue(_scrollView);
    UpdateList()
    if _roomType ~= 0 then
        local req = {}
        req.game_type = _roomType;
        protobuf:sendMessage(protoIdSet.cs_niu_query_in_game_player_num_req, req);
    end
    SetQuickStartBtnPos(gameObject);
end

function OnRoomPlayerNumUpdate(msgId, tMsgData)
    if tMsgData ~= nil then
        _roomPlayerNumList = tMsgData.list;
        table.sort(_roomPlayerNumList,function(a,b)
            return a.room_level<b.room_level
         end)
        UpdateList();
    end
end


local function OnDestroy(gameObject)
    isShow = false
    CLEAN_MODULE(wName .. "Mono");
end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy

protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_query_in_game_player_num_reply, "OnRoomPlayerNumUpdate")
-- 返回当前模块
return M
