-- -----------------------------------------------------------------


-- *
-- * Filename:    GameCenterWinMono.lua
-- * Summary:     GameCenterWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        7/14/2017 4:00:11 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GameCenterWinMono")



-- 界面名称
local wName = "GameCenterWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _platformMgr = IMPORT_MODULE("PlatformMgr")

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local GameMgr = IMPORT_MODULE("GameMgr")
local _scrollView
local _btn5
local _btn3
local _btn4
local _btn1
local _grid
--- [ALD END]

local panels={}
local _go



--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnClickRichCar(gameObject)
    local protobuf = sluaAux.luaProtobuf.getInstance();
    protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
    registerScriptEvent(EVENT_IS_IN_GAME_FOR_CAR, "IsCanEnterRichCar")
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
local function OnClickFruit(gameObject)
    local protobuf = sluaAux.luaProtobuf.getInstance();
    protobuf:sendMessage(protoIdSet.cs_niu_query_player_room_info_req, {})
    registerScriptEvent(EVENT_IS_IN_GAME, "IsCanEnterFruitRoom")
end
local function OnTopChangeMoney()
    --    UnityTools.ShowMessage("功能开发中...");
    UnityTools.CreateLuaWin("ShopWin");
end
local function OnClickNormalCow(gameObject)
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
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _scrollView = UnityTools.FindGo(gameObject.transform, "Container/ScrollView"):GetComponent("UIPanel")

    _btn5 = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell5")
    UnityTools.AddOnClick(_btn5.gameObject, OnClickRichCar)
    _btn3 = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell3")
    UnityTools.AddOnClick(_btn3.gameObject, OnClickRed)
    _btn4 = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell4")
    UnityTools.AddOnClick(_btn4.gameObject, OnClickFruit)
    _btn1 = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell1")
    UnityTools.AddOnClick(_btn1.gameObject, OnClickNormalCow)
    if CTRL.OpenType == 1 then
        _btn3.gameObject:SetActive(false)
    elseif CTRL.OpenType == 2 then
        _btn1.gameObject:SetActive(false)
        _btn4.gameObject:SetActive(false)
        _btn5.gameObject:SetActive(false)
    end

    panels[1] = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell5/icon/Panel"):GetComponent("UIPanel")
    panels[2] = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell3/icon/Panel"):GetComponent("UIPanel")
    panels[3] = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell4/icon/Panel"):GetComponent("UIPanel")
    panels[4] = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid/cell1/icon/Panel"):GetComponent("UIPanel")
    _grid = UnityTools.FindGo(gameObject.transform, "Container/ScrollView/grid"):GetComponent("UIGrid")

--- [ALB END]


    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        _btn3:SetActive(false)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽千人红包按钮")
    end
    _grid:Reposition()
end

local function OnRenderChange()
    _controller:SetScrollViewRenderQueue(_scrollView.gameObject);
    for i=1,#panels do
        _controller.SetScrollViewRenderQueue(panels[i].gameObject)
        panels[i].startingRenderQueue = _scrollView.startingRenderQueue+1
    end
    
end

local function Awake(gameObject)
    _go = gameObject
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    
end
function OnGameCenterRenderChange()
    OnRenderChange()
end

local function Start(gameObject)
    registerScriptEvent(EVENT_GAMECENTERR_ENDER_CHANGE_WIN, "OnGameCenterRenderChange")
    OnRenderChange()
end


local function OnDestroy(gameObject)
    
    unregisterScriptEvent(EVENT_GAMECENTERR_ENDER_CHANGE_WIN, "OnGameCenterRenderChange")
    CLEAN_MODULE("GameCenterWinMono")
end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable


-- 返回当前模块
return M
