-- -----------------------------------------------------------------


-- *
-- * Filename:    HundredCowMainMono.lua
-- * Summary:     百人牛牛
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/28/2017 10:36:04 AM
-- -----------------------------------------------------------------


--[[
-- 
-- 辅助调试函数

function trackLog(...)
    local t = {...}
    local str = table.concat(t, " ----- ")
    LogError(str)
end

-- 下注金币统计
_______betGoldStatistic = {}

function ____addGoldStatisticData(pos, count)
    if _______betGoldStatistic[pos] == nil then
        _______betGoldStatistic[pos] = 0
    end

    _______betGoldStatistic[pos] = _______betGoldStatistic[pos] + count
end

function ___logGoldStatistic()
    local str = "押注统计: "
    for k, v in pairs(_______betGoldStatistic) do
        str = str .. "位置-" .. tostring(k) .. " -> " .. tostring(v) .. "; "
    end
    LogError("================================================================")
    LogError(str)
end

function ____ckearGoldStatistic()
    _______betGoldStatistic = {}
end


-- 结束金币统计
_______resultGoldStatistic = {}


function ____addResultGoldStatisticData(pos, count)
     if _______resultGoldStatistic[pos] == nil then
        _______resultGoldStatistic[pos] = 0
    end

    _______resultGoldStatistic[pos] = _______resultGoldStatistic[pos] + count
end

function ___logResultGoldStatistic()
    local str = "结束统计: "
    for k, v in pairs(_______resultGoldStatistic) do
        str = str .. "位置-" .. tostring(k) .. " -> " .. tostring(v) .. "; "
    end
    LogError(str)
end

function ____ckearResultGoldStatistic()
    _______resultGoldStatistic = {}
end

-- ]]


-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("HundredCowMainMono")


-- 界面名称
local wName = "HundredCowMain"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local BigRwardCtrl=IMPORT_MODULE("BigRewardWinController")
-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local pokerMgr = IMPORT_MODULE("PokerMgr")
local roomMgr = IMPORT_MODULE("roomMgr")
local gameMgr = IMPORT_MODULE("GameMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()

local _betBtnList = {}
local _betBtnColliders = {}
local _betBtnSprites = {}
local _betBtn
local _goldPrefab
local _goldLayer
local _betLayer
local _myPlayerCell
local _myBetCnt1
local _myBetCnt2
local _myBetCnt3
local _myBetCnt4
local _getGoldBtn
local _dealerCell
local _chatBtn
local _playerListBtn
local _trendBtn
local _playerLayer
local _pokerCenter
local _pokerLayer
local _getDealerBtn
local _menuBtn
local _startEffect
local _menuLayer
local _closeBtn
local _rewardPoolLb
local _operLayer
local _stateLayer
local _timeBar
local _timeLb
local _stateTip
local _trendLayer
local _trendAction
local _trendCloseBtn
local _playerListLayer
local _pListLayerAction
local _pListLayerClose
local _sv_playerList
local _sv_playerList_mgr
local _pListTitle
local _poolLayer
local _poolLayerAction
local _poolLayerClose
local _betPoolBtn
local _totalPoolMoney
local _maxPoolPlayer
local _poolEffLayer
local _pEffMoney
local _showRoleBtn
local _poolLeftSp
local _poolRightSp
local _dealerLayer
local _dealerLayerAction
local _dealerLayerCloseBtn
local _sv_dealerLayer
local _sv_dealerLayer_mgr
local _dealerSureBtn
local _dealerSureLabel
local _chatCell1
local _chatCell2
local _chatCell3
local _emojiCell
local _TB = {_pChatR = nil, _pChatL = nil, redSp = nil, betEvtBack = true, manager = nil, _mainDealerBtnLb = nil, _dealerHeadBtn = nil, _tbStartCompt = {}, _myHasBets = false}
local _setBtn
local _bringGoldLb
local _moneySlider
local _moneySliderCollider = nil
local _uiLayer
local _resultMask
local _betPosTotalGoldLbl = {}
--- [ALD END]




local _clickBetBtn = nil
local _clickBetIndex = 1
local _myBetList = {}
local _allGoldList = {{}, {}, {}, {}}
local _freeBetList = {}   -- 保存自由下注时对应的开始UI对象，包括VIP座位2-7，玩家列表按钮
local _freeCellList = {}
local _clickBetType = 1
local _goldActionFactory = 55
local _pokerOrginPos = {}
local _menuState = 0
local _canBet = false
local _totalLastTime = 0
local _lastTime = 0
local _mySeatIndex = 7
local _trendDataList = {}
local _maxTrendDataCnt = 9
local _playerInfoList = nil
local _dealerPList = nil
local _efcTb={}
_efcTb._pEffTotalTime = 800
_efcTb._pEffPerTime = 50
local _myMoneyChangeVal = 0
local _dealerSureState = 0
local _dealerBtnState = 0
_efcTb._waitServerBack = false
local _waitResultAction = false
local _myCanBetMoney = 0
local _soundEff = nil
local _mainTransfrom = nil
local _myLastGold = 0
local _selectBringGold = 0
local _goldActionFuncs = {}
local _goldFActionFuncs = {}
local _currMainRenderQ = 0
local _upPokerLayer = nil
local _upUILayer = nil
local _upSVPlayerList = nil
local _upSVDealerLayer = nil
local _upMainTrans = nil
local _mrGoldPrefab = nil
local _pokerObjTable = {}
local _pokerList = nil
local _betBtnPos = {}
local _allCellList = {}
local _taskObj = {}
local _boxTaskTb={}
_boxTaskTb.getValue= 0
function _boxTaskTb.OnClickBoxTask(gameObject)
    platformMgr.OpenBoxWin(2,_boxTaskTb.getValue)
end
function sendCheckTrendMsg(index)
    roomMgr.sendMsg(protoIdSet.cs_hundred_query_winning_rec_req, {})
end

function sendSitDownMsg(index)
    roomMgr.sendMsg(protoIdSet.cs_hundred_niu_sit_down_req, {pos = index})
end

function sendMaxPoolMsg()
    roomMgr.sendMsg(protoIdSet.cs_hundred_query_pool_win_player_req, {})
end

function sendCheckFreeMsg(index)
    roomMgr.sendMsg(protoIdSet.cs_hundred_niu_player_list_query_req, {type = index})
end

function sendDealerListMsg()
    roomMgr.sendMsg(protoIdSet.cs_hundred_query_master_list_req, {})
end

function sendGetDealerMsg(oper, max)
    roomMgr.sendMsg(protoIdSet.cs_hundred_be_master_req, {flag = oper, set_max = max})
end

function sendCheckPlayerListMsg()
    roomMgr.sendMsg(protoIdSet.cs_hundred_be_master_req, {})
end

local function setBetTypeSprite(light)
    for i = 1, 4, 1 do
        local sp = UnityTools.FindCo(_betLayer.transform, "UISprite", "bet" .. i .. "/type")
        if light == true then
            sp.spriteName = "type_" .. i .. "_1"
        else
            sp.spriteName = "type_" .. i .. "_0"
        end
    end
end

local function changeBetBtn(gameObject, select)
    if select == false then
        gameObject:GetComponent("UISprite").spriteName = "btn_yellow_small_normal"
        UnityTools.FindGo(gameObject.transform, "l2"):SetActive(false)
        UnityTools.FindGo(gameObject.transform, "l1"):SetActive(true)
    else
        gameObject:GetComponent("UISprite").spriteName = "btn_red_normal"
        UnityTools.FindGo(gameObject.transform, "l2"):SetActive(true)
        UnityTools.FindGo(gameObject.transform, "l1"):SetActive(false)
    end
end

local function ClickBetBtn(gameObject)
    if _clickBetBtn ~= nil then
        changeBetBtn(_clickBetBtn, false)
    end
    if gameObject ~= nil then
        changeBetBtn(gameObject, true)
        _clickBetBtn = gameObject
        local nameIndex = tonumber(string.sub(gameObject.name, 4, 4))
        _clickBetIndex = nameIndex
    else
        _clickBetIndex = 0
        _clickBetBtn = nil
    end
end

local function checkBetMoney(btnTb)
    local myPlayerInfo = roomMgr.GetPlayerInfo(8)
    local dealerInfo = roomMgr.GetPlayerInfo(1)
    if dealerInfo == nil then return end
    if myPlayerInfo == nil then return end
    local myMoney = myPlayerInfo.gold
    local canMoney = (myMoney - (_myCanBetMoney * 2)) / 3
    local dCanMoney = 0
    if dealerInfo.icon_url ~= "0000" then
         if dealerInfo.gold/16 <=30000000 then
            dCanMoney = 1000000-1
         else
            dCanMoney = 1000001
         end
    else
        dCanMoney = (dealerInfo.gold / 3) - _myCanBetMoney
    end
    
    
    if dCanMoney < canMoney and dealerInfo.icon_url ~= "0000" then
        canMoney = dCanMoney
    end
    LogError("canMoney="..canMoney)
    -- UnityEngine.Debug.Log(_myCanBetMoney .. " ------- " .. dealerInfo.gold .. " +++++++++++++ " .. dCanMoney)
    for i = 5, 1, -1 do
        local btn = btnTb[i]
        local btnCollider = _betBtnColliders[i]
        local btnSprite = _betBtnSprites[i]
        if btn ~= nil and btnCollider ~= nil and btnSprite ~= nil then
            -- UnityEngine.Debug.Log(canMoney .. "  " .. (10 * math.pow(10, i)))
            if canMoney > 10 * math.pow(10, i) then
                btnCollider.enabled = true
                btnSprite.alpha = 1
            else
                btnCollider.enabled = false
                btnSprite.alpha = 0.5
                if btn == _clickBetBtn then
                    ClickBetBtn(btnTb[i - 1])
                end
            end
        end
    end
end

local function cleanSoundEff()
    if _soundEff ~= nil then
        _soundEff.Stop()
        _soundEff = nil
    end
end

local function changeCanBet(can)
    _canBet = can
    _TB.betEvtBack = true
    LogError("change="..os.time())
    if _canBet == true then
        cleanSoundEff()
        _soundEff = UnityTools.PlaySound("Sounds/hundred", {loop = true, target = _mainTransfrom.gameObject, perTime = 5})
        _myCanBetMoney = 0
        if _clickBetIndex == 0 then
            ClickBetBtn(_betBtnList[1])
        end
        checkBetMoney(_betBtnList)
        -- 非庄家位
        if _dealerSureState ~= 1 then
            -- _operLayer:SetActive(true)
            UnityTools.SetActive(_operLayer, true)
        end
        setBetTypeSprite(true)
    else
        cleanSoundEff()
        -- _operLayer:SetActive(false)
        UnityTools.SetActive(_operLayer, false)
        setBetTypeSprite(false)
    end
end

local function getLongNumber(num)
    return UnityTools.GetLongNumber(num)
end

local function getGoldCntByIndex(index)
    local goldCnt = 0
    if index == 1 then
        goldCnt = 1
    elseif index == 2 then
        goldCnt = 5
    elseif index == 3 then
        goldCnt = 10
    elseif index == 4 then
        goldCnt = 15
    elseif index == 5 then
        goldCnt = 25
    end
    return goldCnt
end

-- 根据下注数计算出需要的金币数量
local function getGoldCntByChips(chips)
    local goldCnt = 1
    if chips < 10000 then
        goldCnt = math.ceil(chips / (1000 / getGoldCntByIndex(2)))
    elseif chips < 100000 then
        goldCnt = math.ceil(chips / (10000 / getGoldCntByIndex(3)))
    elseif chips < 1000000 then
        goldCnt = math.ceil(chips / (100000 / getGoldCntByIndex(4)))
    elseif chips < 10000000 then
        goldCnt = math.ceil(chips / (1000000 / getGoldCntByIndex(5)))
    else
        goldCnt = 1
    end
    return goldCnt
end

-- --------------------------------------
-- 获取指定对象的孩子列表
-- 格式： key .. index

-- key 孩子节点名字前缀
-- endIndex 结束索引
-- startIndex 开始索引
-- -------------------------------------
local function getChilds(parent, key, endIndex, startIndex)
    if startIndex == nil then
        startIndex = 1
    end

    local list = {}
    local count = 0
    for i = startIndex, endIndex do
       local child = UnityTools.FindGo(parent.transform, key .. tostring(i))
		   
       if child ~= nil then
            count = count + 1
            list[count] = child
       end
    end

    return list
end


local function setRewardPool(num, eff)
    local numStr = getLongNumber(num)
    _rewardPoolLb.text = numStr
    _totalPoolMoney.text = numStr
    if eff ~= nil then
        local eff = UnityTools.AddEffect(_rewardPoolLb.transform.parent,"jiangchi01",{loop = false, complete=
        function(gameObject)
            UtilTools.SetEffectRenderQueueByUIParent(_mainTransfrom,gameObject.EffectGameObj.transform,0)
            gameObject.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,0,0)
            gameObject.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
        end})
        eff = nil
    end
end 

local function setExPlayerIcon(playerInfo, pCell, reset)
    local tf = pCell.transform
    local pImg = UnityTools.FindGo(tf, "player_img_bg")
    if pImg == nil then return nil end
    local hImg = UnityTools.FindCo(pImg.transform, "UITexture", "img")
    local defaultImg = UnityTools.FindCo(pImg.transform,"UISprite","img/head")
    if playerInfo == nil then 
    -- 没有头像
        hImg.mainTexture = nil
        if defaultImg ~= nil then
            defaultImg.spriteName = "player_empty"
            -- defaultImg.spriteName = platformMgr.PlayerDefaultHead(playerInfo.sex)
        end
    else
    -- 设置头像
        hImg.mainTexture = nil
        
        if playerInfo.icon_url == "0000" then  
            defaultImg.spriteName = "head_img"
        else
            if playerInfo.player_uuid == platformMgr.PlayerUuid() then
                UnityTools.SetPlayerHead(playerInfo.icon_url, hImg, true)
            else
                UnityTools.SetPlayerHead(playerInfo.icon_url, hImg, false)
            end
            -- if defaultImg ~= nil and (playerInfo.icon_url == nil or playerInfo.icon_url == "") then
                defaultImg.spriteName = platformMgr.PlayerDefaultHead(playerInfo.sex)
            -- end
        end
        
    end
    -- 设置名字与金币信息
    local name = UnityTools.FindCo(tf, "UILabel", "name") 
    if name ~= nil and playerInfo ~= nil then
        -- name.gameObject:SetActive(true)
        name.text = playerInfo.player_name
        local money = UnityTools.FindCo(name.transform, "UILabel", "money") 
        if money ~= nil then
            if playerInfo.icon_url == "0000" then
                money.text = UnityTools.GetShortNum(30000000)
            else

                money.text = UnityTools.GetShortNum(playerInfo.gold)
            end
        end
    else 
        name.text = ""
        -- name.gameObject:SetActive(false)
    end
    if reset ~= nil then
        local getNum = UnityTools.FindCo(tf, "UILabel", "get")
        local costNum = UnityTools.FindCo(tf, "UILabel", "cost")
        UnityTools.SetActive(getNum, false)
        UnityTools.SetActive(costNum, false)
        getNum.text = ""
        costNum.text = ""
    end

    local vip = UnityTools.FindGo(pImg.transform, "vip/vipBox")
    if vip ~= nil then
        
        if playerInfo ~= nil then
            UnityTools.SetNewVipBox(vip, playerInfo.vip_level,"vip",_taskObj.thisObj)
        else
            UnityTools.SetNewVipBox(vip, -1,"vip")
        end
    end

    local bgSp = UnityTools.FindGo(pImg.transform, "bg")
    UnityTools.AddOnClick(bgSp, function(btn) 
        -- 审核版本屏蔽内容
        if version.VersionData.IsReviewingVersion() then
            LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
            LogError("审核版本：屏蔽百人房间对局内获补充金币")
            return
        elseif version.VersionData.isAppStoreVersion() then 
            local shopCtrl = IMPORT_MODULE("ShopWinController");
            if shopCtrl ~= nil then
                shopCtrl.OpenShop(1)
            end
            
            return
        end
        local shopCtrl = IMPORT_MODULE("ShopWinController");
        if shopCtrl ~= nil then
            shopCtrl.OpenShop(1)
        end
        -- platformMgr.OpenFastAddGold()
    end)
end

local function resetPokers(record)
    for k = 1, #_pokerObjTable, 1 do
    -- for k, pokerObj in pairs(_pokerObjTable) do
        local pokerObj = _pokerObjTable[k]
        if record == true then
            _pokerOrginPos[k] = {}
        end
        for j = 1, 5, 1 do
            local p = pokerObj[j].o
            pokerMgr.SetPokerIcon(pokerObj[j], nil)
            if record == true then
                _pokerOrginPos[k][j] = p.transform.position
            end
            p.transform.position = _pokerCenter.transform.position
            if record == true then
                UnityTools.CallFuncInChildren(p, "UISprite", function(go) 
                    print("resetPokers:UIWidget")
                    local wid = go.gameObject:GetComponent("UIWidget")
                    wid.depth = 806 + j
                end)
            end
        end

        UnityTools.SetActive(pokerObj.ptsp, false)
        local tip = pokerObj.tip
        if tip ~= nil then
            UnityTools.SetActive(tip, false)
        end
    end
    _upPokerLayer.startingRenderQueue = 0
end

local function resetGoldInfo()
    local tb = {_myBetCnt1, _myBetCnt2, _myBetCnt3, _myBetCnt4}
    for i = 1, 4, 1 do
        local betCnt = tb[i]
        betCnt.text = 0
        betCnt.gameObject:SetActive(false)
    end
    _myBetList = {}
    for i = 1, 4, 1 do
        local lb = _betPosTotalGoldLbl[i]   
        if lb == nil then
            lb = UnityTools.FindCo(_betLayer.transform, "UILabel", "bet" .. i .. "/total")
        end

        if lb ~= nil then
            lb.text = UnityTools.GetShortNum(0)
        end
    end
    for k, v in pairs(_allGoldList) do
        for i, j in pairs(v) do
            UnityTools.ReleasePoolObj(_goldPrefab, j)
        end
    end

    _allGoldList = nil
    _allGoldList = {{}, {}, {}, {}}
end

local function updateDealerBtn()
    if _dealerSureState == 1 or _dealerBtnState == 1 then
        _dealerSureLabel.text = LuaText.hundred_dealer_get_btn_down
        _TB._mainDealerBtnLb.text = LuaText.hundred_dealer_get_btn_down
        if _moneySliderCollider ~= nil then
            _moneySliderCollider.enabled = false
        end
    else
        _dealerSureLabel.text = LuaText.hundred_dealer_get_btn_up
        _TB._mainDealerBtnLb.text = LuaText.hundred_dealer_get_btn_up
        if _moneySliderCollider ~= nil then
            _moneySliderCollider.enabled = true
        end
    end
end

local function resetFreeCell()
    _freeBetList = {}
    setExPlayerIcon(roomMgr.GetPlayerInfo(1), _dealerCell, true)
    setExPlayerIcon(roomMgr.GetPlayerInfo(8), _myPlayerCell, true)
    local dealerInfo = roomMgr.GetPlayerInfo(1)
    if dealerInfo ~= nil then
        if dealerInfo.player_uuid == platformMgr.PlayerUuid() then
            _dealerSureState = 1
            updateDealerBtn()
        else
            _dealerSureState = 0
            updateDealerBtn()
        end
    end
    for k, v in pairs(_freeCellList) do
        roomMgr.SetPlayerIcon(_taskObj.thisObj,v, roomMgr.GetPlayerInfo(k + 1))
    end
end

local function setMoneyWithAction(reward, cell, index, extraDelay)
    if reward == 0 then return nil end
   
    local getNum = UnityTools.FindCo(cell.transform, "UILabel", "get")
    local costNum = UnityTools.FindCo(cell.transform, "UILabel", "cost")
    
    if getNum == nil then return nil end
    costNum.text = ""
    getNum.text = ""
    if index == 1 then
        local dealerInfo = roomMgr.GetPlayerInfo(1)
        if dealerInfo ~= nil and dealerInfo.icon_url == "0000" then
            return
        end
    end
    local showLb = nil

    -- LogError(index .. " -> " .. reward)

    if reward > 0 then
        showLb = getNum
        showLb.text = "+" ..reward -- UnityTools.GetShortNum(reward)
    else
    
        showLb = costNum
        showLb.text = "-" ..math.abs(reward)-- UnityTools.GetShortNum(math.abs(reward))
    end
    extraDelay = extraDelay or 0
    if showLb ~= nil then
        UnityTools.SetActive(showLb, true)
        local scale = 0.15
        local time = 300
        showLb.transform.localScale = UnityEngine.Vector3(scale, scale, 1.0)
        local action = TweenScale.Begin(showLb.gameObject, 0.5, UnityEngine.Vector3(1, 1, 1))
        action.delay = 0 --(extraDelay + 300) / 1000
        local timer = gTimer.registerOnceTimer(3000, function(ob)   --time + 1500 + extraDelay
            if UnityTools.IsWinShow(wName) == false then return nil end
            ob.text = ""
            -- LogError("1111")
            UnityTools.SetActive(ob, false)
        end, showLb)
        -- gTimer.setRecycler(wName, timer)
    end
end

local function doStartEffects(cnt)
    -- triggerScriptEvent(EVENT_GAME_START_EFFECT, 1)
    -- _trendLayer:SetActive(false)
    -- _playerListLayer:SetActive(false)
    -- _dealerLayer:SetActive(false)
    -- _poolLayer:SetActive(false)
    cnt = cnt or 0

    local ct = _TB._tbStartCompt.ct
    local bg = _TB._tbStartCompt.bg 
    local lb0 = _TB._tbStartCompt.lb0
    local lb1 = _TB._tbStartCompt.lb1
    local lb3 = _TB._tbStartCompt.lb3

    ct:SetOnFinished(function (ob) 
        -- _startEffect:SetActive(false)
        UnityTools.SetActive(_startEffect, false)
     end)
    UnityTools.PlaySound("Sounds/gameStart",{target = _mainTransfrom.gameObject})
    UnityTools.SetActive(_startEffect, true)
    -- _startEffect:SetActive(true)
    ct:ResetToBeginning()
    bg:ResetToBeginning()
    lb3:ResetToBeginning()
    ct:Play(true)
    bg:Play(true)
    lb0:Play(true)
    lb1:Play(true)
    if cnt > 0 then
        UnityTools.FindCo(_startEffect.transform, "UILabel", "Container/Label").text = LuaText.GetStr(LuaText.hundred_start_tip, tostring(cnt))
        lb3.gameObject:SetActive(true)
        lb3:Play(true)
    else
        lb3.gameObject:SetActive(false)
    end

    local timer = gTimer.registerOnceTimer(1000, function () 
        if UnityTools.IsWinShow(wName) == false then return nil end
        lb0:Play(false)
        lb1:Play(false)    
    end)
    gTimer.setRecycler(wName, timer)
end

-- 发牌动作表现
local function doPokerAction()
    local centerTf = _pokerCenter.transform
    local actionTime = 250
    local delay = 40
    local pPos1 = centerTf.position
    -- _pokerLayer:SetActive(true)
    local maxTime = 0
    _upPokerLayer.startingRenderQueue = _currMainRenderQ + 60

    -- 卡牌动作参数
    local tActionParams = {}
    for i, pokerObj in pairs(_pokerObjTable) do
        local pPos1 = centerTf.position
        for j = 1, 5, 1 do
            local poker = pokerObj[j]
            pokerMgr.SetPokerIcon(poker, nil)

            -- --------- v1.0  begin -------------------------------- 
            -- poker.tp:SetStartPos(pPos1.x, pPos1.y, pPos1.z)
            -- poker.tp:SetEndPos(_pokerOrginPos[i][j].x, _pokerOrginPos[i][j].y, _pokerOrginPos[i][j].z)
            -- poker.tp:SetParams(actionTime, delay, true, true)
            -- _TB.manager:Begin(poker.tp)
            -- --------- v1.0  end -------------------------------- 

            if maxTime < delay then
                maxTime = delay
            end
            -- delay = delay + 40

            -- --------- v1.2  begin -------------------------------- 
            -- Add by WP.Chu

            local act = {}
            table.insert(act, poker.tp)                               -- goTarget
            table.insert(act, _upPokerLayer.startingRenderQueue)      -- renderQ
            table.insert(act, pPos1.x)                                -- startX
            table.insert(act, pPos1.y)                                -- startY
            table.insert(act, pPos1.z)                                -- startZ
            table.insert(act, _pokerOrginPos[i][j].x)                 -- endX
            table.insert(act, _pokerOrginPos[i][j].y)                 -- endY
            table.insert(act, _pokerOrginPos[i][j].z)                 -- endZ
            table.insert(act, actionTime)                             -- duration
            table.insert(act, delay)                                  -- delay
            table.insert(act, true)                                   -- world
            table.insert(act, true)                                   -- noEvt

            table.insert(tActionParams, act)
            -- --------- v1.2  end -------------------------------- 
        end

        local timer = gTimer.registerOnceTimer(delay, function() 
            UnityTools.PlaySound("Sounds/poker", {target = _mainTransfrom.gameObject})
        end)
        gTimer.setRecycler(wName, timer)
        delay = delay + 300
    end

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, #tActionParams)
    tActionParams = nil

    return actionTime + maxTime
end

-- 发牌结束开牌表现
local function doOpenPokerAction(pokerDataList, delay, last_win_rec)
    if pokerDataList == nil then return 0 end
    delay = delay or 0
    for i, pokerObj in pairs(_pokerObjTable) do    
        for j = 1, 5, 1 do
            local v = pokerObj[j].o
            local pokerInfo = pokerDataList[i].card[j]
            pokerMgr.SetPokerIcon(pokerObj[j], pokerInfo, true, delay)
            -- delay = delay + 40 -- 每张牌的间隔
        end
        local t1 = gTimer.registerOnceTimer(delay, function() 
            UnityTools.PlaySound("Sounds/poker", {target = _mainTransfrom.gameObject})
        end)
        gTimer.setRecycler(wName, t1)
        -- UnityTools.PlaySound("Sounds/poker", {delTime = 2+(delay / 1000), target = _mainTransfrom.gameObject, delay = delay / 1000})
        delay = delay + 50 -- 牌与牌型的间隔
        local pokerType = pokerObj.ptsp
        local action = pokerObj.pts

        local timer = gTimer.registerOnceTimer(delay, function() 
            if UnityTools.IsWinShow(wName) == false then return nil end
            local type = pokerDataList[i].type
            if i ~= 1 then 
                local myBetTip = pokerObj.tip
                -- myBetTip.gameObject:SetActive(true)
                UnityTools.SetActive(myBetTip, true)
                local myBet = _myBetList[i-1]
                if myBet == nil or myBet == 0 then
                    myBetTip.effectColor = UnityEngine.Color(0.18,0.192,0.203)
                    myBetTip.text = LuaText.hundred_my_bet_tip_0
                else
                    local numStr = nil
                    local ratio = 1
                    if last_win_rec["win_" .. (i - 1)] == true then
                        ratio = pokerMgr.PokerTypeRatio[type]
                        numStr = "+" .. UnityTools.GetShortNum2(myBet * ratio)
                    else
                        ratio = pokerMgr.PokerTypeRatio[pokerDataList[1].type]
                        numStr = "-" .. UnityTools.GetShortNum2(myBet * ratio)
                    end
                    myBetTip.effectColor = UnityEngine.Color(0.317,0.129,0.023)
                    myBetTip.text = LuaText.GetStr(LuaText.hundred_my_bet_tip_1, ratio, numStr)
                end                
                if i == 5 then
                    _upPokerLayer.startingRenderQueue = _currMainRenderQ + 5
                end
            end

            
            -- pokerType.gameObject:SetActive(true)
            UnityTools.SetActive(pokerType, true)
            if type>=0 and type <=10 then
                pokerType.width = 86
                pokerType.height = 48
            else
                pokerType.width = 126
                pokerType.height = 46
            end
            pokerType.spriteName = "niu_" .. tostring(type)
            action:ResetToBeginning()
            action:Play(true)
            UnityTools.PlaySound("Sounds/girl/niu_" .. tostring(type), {target = _mainTransfrom.gameObject})
        end)
        gTimer.setRecycler(wName, timer)

        delay = delay + 1000 -- 每组牌的间隔
    end
    return delay
end

-- --------------------
-- 押注时金币飞行表现
--
-- ###参数：
--  goldCnt 金币数量
--  type    下注位置 1-4
--  pCell   下注起始点对应的UI对象  
-- -------------------
local function doBetGoldAction(goldCnt, type, pCell)
    local factory = _goldActionFactory
    local endTf = _betBtnPos[type]
    local pCell = pCell or _myPlayerCell   -- 起飞位置游戏对象
    local fromPos = pCell.transform.localPosition  -- 起飞位置
    local eX, eY, eZ = endTf.x, endTf.y, endTf.z  -- 目的地
    local delay = 0
    local noEvt = false
    local world = false

    -- 所有的金币数据
    -- { {goTarget, renderQ, startX, startY, startZ, endX, endY, endZ, duration, delay, world, noEvt}, ... }
    local tActionParams = {}
    for i = 1, goldCnt, 1 do
        local gold = UnityTools.GetPoolObj(_goldPrefab, _goldLayer)

        -- ------------------------- v1.0 begin -----------------------------
        -- gold:SetRenderQ(_currMainRenderQ + 22)
        -- gold:SetStartPos(fromPos.x + math.random(-3, 3), fromPos.y + math.random(-3, 3), fromPos.z)
        -- gold:SetEndPos(eX + math.random(-factory, factory), eY + math.random(-factory-10, factory-10), eZ)

        -- if #_allGoldList[type] >= 150 then
        --     gold:SetParams(300, delay, false, false)
        -- else
        --     gold:SetParams(300, delay, false, true)
        --     _allGoldList[type][#_allGoldList[type] + 1] = gold
        -- end

        -- _TB.manager:Begin(gold)
        -- ------------------------- v1.0 end ----------------------------- 

        -- 当飞行到目的的金币数量达到150时，飞行结束自动由默认事件处理，不需要加入_allGoldList
        local nTypeGoldCount = #_allGoldList[type]
        if nTypeGoldCount >= 150 then
            noEvt = false
        else
            noEvt = true
            _allGoldList[type][nTypeGoldCount + 1] = gold
        end

        -- ------------------------- v1.1 begin-----------------------------
        -- Add by WP.Chu 
        --
        -- _TB.manager:actionBegin(gold,
        --     _currMainRenderQ + 22,
        --     fromPos.x + math.random(-3, 3),
        --     fromPos.y + math.random(-3, 3),
        --     fromPos.z,
        --     eX + math.random(-factory, factory),
        --     eY + math.random(-factory-10, factory-10),
        --     eZ,
        --     300,
        --     delay,
        --     world,
        --     noEvt
        -- )

        -- delay = delay + math.random(-2, 25)
        -- ------------------------- v1.1 end ----------------------------- 

        -- ------------------------- v1.2 begin-----------------------------
        -- Add by WP.Chu 
        
        local act = {}
        table.insert(act, gold)                                   -- goTarget
        table.insert(act, _currMainRenderQ + 22)                  -- renderQ
        table.insert(act, fromPos.x + math.random(-3, 3))         -- startX
        table.insert(act, fromPos.y + math.random(-3, 3))         -- startY
        table.insert(act, fromPos.z)                              -- startZ
        table.insert(act, eX + math.random(-factory, factory))    -- endX
        table.insert(act, eY + math.random(-factory-10, factory-10)) -- endY
        table.insert(act, eZ)                                     -- endZ
        table.insert(act, 300)                                    -- duration
        table.insert(act, delay)                                  -- delay
        table.insert(act, world)                                  -- world
        table.insert(act, noEvt)                                  -- noEvt
        
        table.insert(tActionParams, act)
        delay = delay + math.random(-2, 25)

        -- --------- v1.2  end -------------------------------- 
    end

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, goldCnt)
    tActionParams = nil

    UnityTools.PlaySound("Sounds/getGold", {delay = delay * 0.5 / 1000, target = _mainTransfrom.gameObject})
end


-- ---------------------
-- 结算阶段，闲家赔庄家表现, 押注按钮到庄家
-- ---------------------
local function doGoldFTDAction(sendList, delay)
    -- return delay
    delay = delay or 20
    local initDelay = delay
    local endPos = _dealerCell.transform.localPosition
    local maxTime = 0

    local tActionParams = {}
    for i, j in pairs(sendList) do
        for k, v in pairs(_allGoldList[j]) do
            local t = math.random(10, 250)
            
            -- ------------------------- v1.0 begin -----------------------------
            -- v:SetStartPos()
            -- v:SetEndPos(endPos.x,endPos.y,endPos.z)
            -- v:SetParams(300, (delay + t), false, false)
            -- _TB.manager:Begin(v)
            -- ------------------------- v1.0 end -----------------------------

            if (t + 300) > maxTime then
                maxTime = t + 300
            end
            _allGoldList[j][k] = nil


            -- ------------------------- v1.2 begin-----------------------------
            -- Add by WP.Chu 

            local act = {}
            table.insert(act, v)                  -- goTarget
            table.insert(act, 9999)               -- renderQ
            table.insert(act, 9999)               -- startX
            table.insert(act, 9999)               -- startY
            table.insert(act, 9999)               -- startZ
            table.insert(act, endPos.x)           -- endX
            table.insert(act, endPos.y)           -- endY
            table.insert(act, endPos.z)           -- endZ
            table.insert(act, 300)                -- duration
            table.insert(act, delay + t)          -- delay
            table.insert(act, false)              -- world
            table.insert(act, false)              -- noEvt

            table.insert(tActionParams, act)
            -- --------- v1.2  end -------------------------------- 

            -- ____addResultGoldStatisticData(j, 1)
        end
        -- indexCnt = 0
    end

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, #tActionParams)
    tActionParams = nil

    return maxTime
end

-- ----------------------------
-- 结算阶段，庄家赔闲家表现, 庄家飞到押注按钮
-- ----------------------------
local function doGoldDTFAction(sendList, delay, typeTb)
    delay = delay or 20
    local initDelay = delay 
    local factory = _goldActionFactory
    local fromPos = _dealerCell.transform.localPosition   -- 以庄家位置为起点
    local maxTime = 0
    local goldATime = 300
    local world = false
    local noEvt = false

    local tActionParams = {}
    for i, j in pairs(sendList) do
        local endlp = _betBtnPos[j]
        local goldCnt = #_allGoldList[j]
        
        if goldCnt > 100 then
            goldCnt = 100
        end
        
        for i = 1, goldCnt, 1 do
            local t = math.random(20, 250)
            local gold = UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
            
            -- ------------------------- v1.0 begin -----------------------------
            -- gold:SetRenderQ(_currMainRenderQ + 22)
            -- gold:SetStartPos(fromPos.x + math.random(-10, 10), fromPos.y + math.random(-10, 10), fromPos.z)
            -- gold:SetEndPos(endlp.x + math.random(-factory, factory), endlp.y + math.random(-factory-10, factory-10), endlp.z)
            -- if #_allGoldList[j] >= 150 then
            --     gold:SetParams(goldATime, (delay + t), false, false)
            -- else
            --     gold:SetParams(goldATime, (delay + t), false, true)
            --     _allGoldList[j][#_allGoldList[j] + 1] = gold
            -- end
            -- _TB.manager:Begin(gold)
            -- ------------------------- v1.1 end -----------------------------

            -- 当飞行到目的的金币数量达到150时，飞行结束自动由默认事件处理，不需要加入_allGoldList
            local nTypeGoldCount = #_allGoldList[j]
            if nTypeGoldCount >= 150 then
                noEvt = false
            else
                noEvt = true
                _allGoldList[j][nTypeGoldCount + 1] = gold
            end

            -- ------------------------- v1.1 begin -----------------------------
            -- Add by WP.Chu 
            --
            -- _TB.manager:actionBegin(gold,
            --     _currMainRenderQ + 22,
            --     fromPos.x + math.random(-10, 10),
            --     fromPos.y + math.random(-10, 10),
            --     fromPos.z,
            --     endlp.x + math.random(-factory, factory),
            --     endlp.y + math.random(-factory-10, factory-10),
            --     endlp.z,
            --     goldATime,
            --     delay + t,
            --     world,
            --     noEvt
            -- )
            -- ------------------------- v1.1 end -----------------------------

            -- ------------------------- v1.2 begin-----------------------------
            -- Add by WP.Chu 

            local act = {}
            table.insert(act, gold)                                         -- goTarget
            table.insert(act, _currMainRenderQ + 22)                        -- renderQ
            table.insert(act, fromPos.x + math.random(-10, 10))             -- startX
            table.insert(act, fromPos.y + math.random(-10, 10))             -- startY
            table.insert(act, fromPos.z)                                    -- startZ
            table.insert(act, endlp.x + math.random(-factory, factory))     -- endX
            table.insert(act, endlp.y + math.random(-factory-10, factory-10)) -- endY
            table.insert(act, endlp.z)                                      -- endZ
            table.insert(act, goldATime)                                    -- duration
            table.insert(act, delay + t)                                    -- delay
            table.insert(act, world)                                        -- world
            table.insert(act, noEvt)                                        -- noEvt

            table.insert(tActionParams, act)
            -- --------- v1.2  end -------------------------------- 

            if (t + goldATime) > maxTime then
                maxTime = t + goldATime
            end
        end
        delay = delay + 50
    end

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, #tActionParams)
    tActionParams = nil

    UnityTools.PlaySound("Sounds/getGold", {delay = (initDelay / 1000), target = _mainTransfrom.gameObject})
    return maxTime
end

-- ----------------------------------
-- 牌局结束，金币飞向玩家列表按钮表现
-- ----------------------------------
local function ____doGoldFTPLAction(sendList, delay)
    local delay = delay or 20
    local initDelay = delay
    local endPos = _playerListBtn.transform.localPosition
    local maxTime = 0
    local goldATime = 300
    
    local tActionParams = {}
    for i, j in pairs(sendList) do
        for k, v in pairs(_allGoldList[j]) do
            local t = math.random(10, 250)

            -- ------------------------- v1.0 begin -----------------------------
            -- v:SetStartPos()
            -- v:SetEndPos(endPos.x,endPos.y,endPos.z)
            -- v:SetParams(goldATime, (delay + t), false, false)
            -- _TB.manager:Begin(v)
            -- ------------------------- v1.0 end -----------------------------

            if (t + goldATime) > maxTime then
                maxTime = t + goldATime
            end
            
            _allGoldList[j][k] = nil

            -- ------------------------- v1.2 begin-----------------------------
            -- Add by WP.Chu 

            local act = {}
            table.insert(act, v)                  -- goTarget
            table.insert(act, 9999)               -- renderQ
            table.insert(act, 9999)               -- startX
            table.insert(act, 9999)               -- startY
            table.insert(act, 9999)               -- startZ
            table.insert(act, endPos.x)           -- endX
            table.insert(act, endPos.y)           -- endY
            table.insert(act, endPos.z)           -- endZ
            table.insert(act, goldATime)          -- duration
            table.insert(act, delay + t)          -- delay
            table.insert(act, false)              -- world
            table.insert(act, false)              -- noEvt

            table.insert(tActionParams, act)
            -- --------- v1.2  end -------------------------------- 

            -- ____addResultGoldStatisticData(j, 1)
        end
    end

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, #tActionParams)
    tActionParams = nil

    UnityTools.PlaySound("Sounds/getGold", {delay = (initDelay / 1000), target = _mainTransfrom.gameObject})
    return maxTime
end

-- -------------------------
-- 发放赢家钱币，从押注部分飞到赢钱的玩家位置
-- -------------------------
local function doGoldFTPAction(sendList, delay, typeTb)
    -- 飞到玩家列表按钮
    ____doGoldFTPLAction(sendList, delay)
    
    -- 飞到VIP座位
    local maxTime = 0
    local initDelay = delay
    local factory = _goldActionFactory
    local goldATime = 300
    local world = false
    local noEvt = false

    local tActionParams = {}
    for k, v in pairs(sendList) do
        local fromlp = _betBtnPos[v]
        for i, j in pairs(_freeBetList) do
            if j[v] ~= nil and i ~= 7 then
                local freeCell = _allCellList[2 + i]
                local endPos = freeCell.transform.localPosition
                local goldCnt = j[v] * typeTb[v + 1]
                if goldCnt > 100 then
                    goldCnt = 100
                end
                for index = 1, goldCnt, 1 do
                    local t = math.random(10, 250)
                    local gold = UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
                    
                    -- ------------------------- v1.0 begin-----------------------------
                    -- gold:SetRenderQ(_currMainRenderQ + 22)
                    -- gold:SetStartPos(fromlp.x + math.random(-factory, factory), fromlp.y + math.random(-factory-10, factory-10), fromlp.z)
                    -- gold:SetEndPos(endPos.x,endPos.y,endPos.z)
                    -- gold:SetParams(goldATime, (delay + t), false, false)
                    -- _TB.manager:Begin(gold)
                    -- ------------------------- v1.2 end-----------------------------

                    -- ------------------------- v1.1 begin-----------------------------
                    -- Add by WP.Chu
                    -- 
                    -- _TB.manager:actionBegin(gold,
                    --     _currMainRenderQ + 22,
                    --     fromlp.x + math.random(-10, 10),
                    --     fromlp.y + math.random(-10, 10),
                    --     fromlp.z,
                    --     endPos.x,
                    --     endPos.y,
                    --     endPos.z,
                    --     goldATime,
                    --     delay + t,
                    --     world,
                    --     noEvt
                    -- )
                    -- ------------------------- v1.1 begin-----------------------------
                    
                    -- ------------------------- v1.2 begin-----------------------------
                    -- Add by WP.Chu 

                    local act = {}
                    table.insert(act, gold)                                 -- goTarget
                    table.insert(act, _currMainRenderQ + 22)                -- renderQ
                    table.insert(act, fromlp.x + math.random(-10, 10))      -- startX
                    table.insert(act, fromlp.y + math.random(-10, 10))      -- startY
                    table.insert(act, fromlp.z)                             -- startZ
                    table.insert(act, endPos.x)                             -- endX
                    table.insert(act, endPos.y)                             -- endY
                    table.insert(act, endPos.z)                             -- endZ
                    table.insert(act, goldATime)                            -- duration
                    table.insert(act, delay + t)                            -- delay
                    table.insert(act, world)                                -- world
                    table.insert(act, noEvt)                                -- noEvt
                
                    table.insert(tActionParams, act)
                    -- --------- v1.2  end -------------------------------- 

                    if (t + goldATime) > maxTime then
                        maxTime = t + goldATime
                    end
                end  --  for: nested2
            end
        end  -- for: nested2
    end  -- for: 1

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, #tActionParams)
    tActionParams = nil


    UnityTools.PlaySound("Sounds/getGold", {delay = (initDelay / 1000), target = _mainTransfrom.gameObject})
    return maxTime
end

local function getPlayerCellByPos(pos)
    local pIndex = pos
    if pIndex == 7 then
        return _playerListBtn
    elseif pIndex > 0 then
        pIndex = pIndex + 1
    else
        pIndex = 0
    end
    local freeCell = _allCellList[pIndex + 1]
    -- UnityTools.FindGo(_playerLayer.transform, "playerCell" .. tostring(pIndex))
    return freeCell
end

local function doGoldPoTPAction(rewardList)
    local fromlp = _betPoolBtn.transform.localPosition
    local factory = _goldActionFactory
    local maxTime = 0
    local goldATime = 300
    local moneyList = {}
    local world = false
    local noEvt = false

    local tActionParams = {}
    for index, rewardInfo in pairs(rewardList) do
        local pIndex = rewardInfo.player_pos
        local freeCell = getPlayerCellByPos(pIndex)
        if freeCell == nil then
            freeCell = _playerListBtn
        end
        local endPos = freeCell.transform.localPosition
        local goldCnt = getGoldCntByChips(rewardInfo.reward_num)
        local delay = 20
        if goldCnt > 120 then
            goldCnt = 150
        end
        for index = 1, goldCnt, 1 do
            local gold = UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
            
            -- ------------------------- v1.0 begin-----------------------------
            -- gold:SetRenderQ(_currMainRenderQ + 22)
            -- gold:SetStartPos(fromlp.x + math.random(-factory, factory), fromlp.y + math.random(-factory-10, factory-10), fromlp.z)
            -- gold:SetEndPos(endPos.x,endPos.y,endPos.z)
            -- gold:SetParams(goldATime, delay, false, false)
            -- _TB.manager:Begin(gold)
            -- ------------------------- v1.0 end-----------------------------

            -- ------------------------- v1.1 begin-----------------------------
            -- Add by WP.Chu
            -- 
            -- _TB.manager:actionBegin(gold,
            --             _currMainRenderQ + 22,
            --             fromlp.x + math.random(-10, 10),
            --             fromlp.y + math.random(-10, 10),
            --             fromlp.z,
            --             endPos.x,
            --             endPos.y,
            --             endPos.z,
            --             goldATime,
            --             delay,
            --             world,
            --             noEvt
            --         )
            -- ------------------------- v1.1 end-----------------------------

            -- ------------------------- v1.2 begin-----------------------------
            -- Add by WP.Chu 
           
            local act = {}
            table.insert(act, gold)                                 -- goTarget
            table.insert(act, _currMainRenderQ + 22)                -- renderQ
            table.insert(act, fromlp.x + math.random(-10, 10))      -- startX
            table.insert(act, fromlp.y + math.random(-10, 10))      -- startY
            table.insert(act, fromlp.z)                             -- startZ
            table.insert(act, endPos.x)                             -- endX
            table.insert(act, endPos.y)                             -- endY
            table.insert(act, endPos.z)                             -- endZ
            table.insert(act, goldATime)                            -- duration
            table.insert(act, delay)                                -- delay
            table.insert(act, world)                                -- world
            table.insert(act, noEvt)                                -- noEvt
            
            table.insert(tActionParams, act)
            -- --------- v1.2  end --------------------------------     

            delay = delay + math.random(-5, 12)
        end  -- for : nested 2

        if maxTime < delay then
            maxTime = delay + goldATime
        end
        moneyList[pIndex] = {rewardInfo.reward_num, freeCell}
        
        -- if pIndex ~= 7 then
        --     setMoneyWithAction(rewardInfo.reward_num, freeCell, roomMgr.PosToIndex(pIndex), maxTime)
        -- else
        --     setMoneyWithAction(_myMoneyChangeVal, _myPlayerCell, 8, maxTime)
        --     _myMoneyChangeVal = 0
        -- end

    end -- for

    -- 动画表现 v1.2
    _TB.manager:groupActionBegin(tActionParams, #tActionParams)
    tActionParams = nil

    local timer1 = gTimer.registerOnceTimer(maxTime, function() 
        UnityTools.SetActive(_resultMask, true) 
        for k, v in pairs(moneyList) do
            if k ~= 7 then
                setMoneyWithAction(v[1], v[2], roomMgr.PosToIndex(k))
            else
                setMoneyWithAction(_myMoneyChangeVal, _myPlayerCell, 8)
                _myMoneyChangeVal = 0
            end
        end
        moneyList = nil
    end)
    gTimer.setRecycler(wName, timer1)
    local timer = gTimer.registerOnceTimer(maxTime + 2000, function() UnityTools.SetActive(_resultMask, false) end)
    gTimer.setRecycler(wName, timer)

    UnityTools.PlaySound("Sounds/getGold", {delay = maxTime * 0.5 / 1000, target = _mainTransfrom.gameObject})
end

local function ClickBetTypeBtn(gameObject)    
    if _canBet == false then
        UnityTools.ShowMessage("只能在下注阶段下注哦！")
        return nil
    end
    if _TB.betEvtBack == false then return nil end
    LogError("bet="..os.time())
    if roomMgr.GetPlayerInfo(8).gold == 0 then
        UnityTools.ShowMessage("金币不足，无法下注！")
        return nil
    elseif roomMgr.GetPlayerInfo(1).icon_url ~= "0000" and (_myCanBetMoney * 3) >= roomMgr.GetPlayerInfo(1).gold then
        
        UnityTools.ShowMessage("下注已达上限，即将开牌")
        return nil
    elseif _clickBetIndex < 1 then
        UnityTools.ShowMessage("下注额已达到个人上限")
        return nil
    end
    
    -- LogError((_myCanBetMoney * 3) .. "   " .. roomMgr.GetPlayerInfo(1).gold)
    if _dealerSureState == 1 or _canBet == false or _clickBetIndex < 1 then 
        return nil 
        
    end
    if roomMgr.State() ~= 20 then 
        return nil 
    end
    local nameIndex = tonumber(string.sub(gameObject.name, 4, 4))
    _clickBetType = nameIndex
    if _clickBetIndex == 2 then
        roomMgr.SendRateMsg(1000, _clickBetType)
    elseif _clickBetIndex == 3 then
        roomMgr.SendRateMsg(10000, _clickBetType)
    elseif _clickBetIndex == 4 then
        roomMgr.SendRateMsg(100000, _clickBetType)
    elseif _clickBetIndex == 5 then
        roomMgr.SendRateMsg(1000000, _clickBetType)
    else
        roomMgr.SendRateMsg(100, _clickBetType)
    end
    _TB.betEvtBack = false
end

local function ClickGetGoldBtn(gameObject)
    -- 苹果商店版本
    if version.VersionData.isAppStoreVersion() then 
        local shopCtrl = IMPORT_MODULE("ShopWinController");
        if shopCtrl ~= nil then
            shopCtrl.OpenShop(1)
        end
        
        return
    end
    local shopCtrl = IMPORT_MODULE("ShopWinController");
    if shopCtrl ~= nil then
        shopCtrl.OpenShop(1)
    end
    -- platformMgr.OpenFastAddGold()
end

local function ClickChatBtn(gameObject)
    platformMgr.OpenChatWin()
end

local function updateTrendLayer()
    for i = 1, _maxTrendDataCnt, 1 do
        local cell = UnityTools.FindGo(_trendLayer.transform, "Content/Container/cell" .. i)
        local trendData = _trendDataList[i]
        if trendData == nil then
            cell:SetActive(false)
        else 
            cell:SetActive(true)
            for j = 1, 4, 1 do
                local flag = UnityTools.FindCo(cell.transform, "UISprite", "res" .. j)
                if trendData[j] == 1 then
                    flag.spriteName = "sp_win"
                elseif trendData[j] == 2 then
                    flag.spriteName = "sp_pool"
                else
                    flag.spriteName = "sp_lose"
                end
            end
        end
    end
end

local function ClickTrendBtn(gameObject)
    _trendLayer:SetActive(true)
    _trendAction:Play(true)
    updateTrendLayer()
end

local function updatePlayerListLayer()
    -- 清空GridCellMgr中的数据
    

    -- 对scrollview、gridCellMgr重新排序
    -- _sv_playerList:ResetPosition()
    
    if _playerListLayer.activeSelf == true then
        _sv_playerList_mgr:ClearCells()


        if _playerInfoList == nil or #_playerInfoList == 0 then
            return nil 
        end

        -- 遍历增加新的元素
        for i = 1, #_playerInfoList, 1 do
            _sv_playerList_mgr:NewCellsBox(_sv_playerList_mgr.Go)
        end

        _sv_playerList_mgr.Grid:Reposition()
        _sv_playerList_mgr:UpdateCells()
    end

    -- _sv_playerList_mgr:ScrollCellToIndex(0, 4, 400)
end

local function updateDealerPlayerLayer()
    -- 清空GridCellMgr中的数据
    _sv_dealerLayer_mgr:ClearCells()


    if _dealerPList == nil or #_dealerPList == 0 then
        return
    end
    -- LogError(#_dealerPList)
    local layer = UnityTools.FindGo(_sv_dealerLayer.transform, "Container/layer")
    -- 遍历增加新的元素
    _dealerBtnState = 0
    for i = 1, #_dealerPList, 1 do
        local dInfo = _dealerPList[i]
        if dInfo.player_uuid == platformMgr.PlayerUuid() then
            _dealerBtnState = 1
        end
        _sv_dealerLayer_mgr:NewCellsBox(layer)
    end

    -- 对scrollview、gridCellMgr重新排序
    if _sv_dealerLayer_mgr.Grid == nil then return nil end
    
    if _dealerLayer.activeSelf == true then
        _sv_dealerLayer_mgr.Grid:Reposition()
        _sv_dealerLayer_mgr:UpdateCells()
        _sv_dealerLayer:ResetPosition()
    end
    -- _sv_playerList_mgr:ScrollCellToIndex(0, 4, 400)
end

local function ClickPlayerListBtn(gameObject)
    -- UnityTools.SetGuassBlur(_playerListLayer)
    _playerListLayer:SetActive(true)
    _pListLayerAction:Play(true)
    sendCheckFreeMsg(0)
end

local function ClickPlayerCellBtn(gameObject)
    local nameIndex = tonumber(string.sub(gameObject.name, 11, 11))
    local playerInfo = roomMgr.GetPlayerInfo(nameIndex)
    if playerInfo == nil then
        -- if _waitResultAction == true then
     
        if _waitResultAction then --and roomMgr.State() ~= 10 then 
            UnityTools.ShowMessage(LuaText.hundred_seat_state_limit_tip)
        else
            if roomMgr.GetPlayerInfo(8).gold >= 500000 then
                sendSitDownMsg(nameIndex - 1)
            else
                UnityTools.ShowMessage(LuaText.hundred_seat_limit_tip)
            end
        end
    else
        if playerInfo.player_uuid == platformMgr.PlayerUuid then
            platformMgr.OpenShowWin()
        else
            platformMgr.OpenPlayerInfoInGame(playerInfo.player_uuid)
        end

    end
end

local function checkBringGold()
    if _myLastGold <= 0 then
        sliderValue = 0
    else
        sliderValue = _myLastGold * _moneySlider.value
        if sliderValue < 100000 then
            sliderValue = 0
            _moneySlider.value = 0
        else
            sliderValue = math.floor(sliderValue / 100000)
            sliderValue = sliderValue * 100000
        end
    end
    _selectBringGold = sliderValue + 30000000
end

local function ClickGetDealerBtn(gameObject)
    LogError(_dealerSureState .. "  " .. _dealerBtnState)
    if roomMgr.GetPlayerInfo(8) ~= nil then
        _myLastGold = roomMgr.GetPlayerInfo(8).gold - 30000000
    else
        _myLastGold = 0
    end
    if _myLastGold > 0 then
        checkBringGold()
        if _moneySliderCollider ~= nil then
            _moneySliderCollider.enabled = true
        end
        _bringGoldLb.text = LuaText.Format("hundred_pool_desc2",tostring(_selectBringGold))
        UnityTools.SetActive(_moneySlider.gameObject,true)
    else
        _bringGoldLb.text = LuaText.GetStr(LuaText.hundred_pool_desc3)
        UnityTools.SetActive(_moneySlider.gameObject,false)
        
    --     UnityTools.ShowMessage("携带的金币小于500万，不能上庄")
    --     return nil
        -- _moneySlider.value = 1
        -- _bringGoldLb.text = "[u][e36500]10000000[-][-]"
        -- _moneySlider.gameObject:GetComponent("BoxCollider").enabled = false
    end
    
    _dealerLayer:SetActive(true)
    _dealerLayerAction:ResetToBeginning()
    _dealerLayerAction:Play(true)
    sendDealerListMsg()
end

local function ClickMenuBtn(gameObject)
    if _menuState == 0 then
        _menuLayer:SetActive(true)
        _menuState = 1
    else
        _menuLayer:SetActive(false)
        _menuState = 0
    end
end

-- local function CloseWin(gameObject)
--     -- UnityTools.RemoveAllSound()
--     UnityTools.DestroyWin(wName)
-- end

local function closeFunc()
    if UI.Controller.UIManager.IsWinShow("HundredCowMain") then
        roomMgr.bExiting = true
    end
    roomMgr.SendLeaveRoomMsg()
    platformMgr.gameMgr.closeActiveFun = function()
        -- UtilTools.RemoveAllWinExpect()
        UI.Controller.UIManager.RemoveAllWinExpect({"Waiting","MainCenterWin","MainWin"})
        --UnityTools.DestroyWin(wName)
    end
    UnityTools.ReturnToMainCity()
end
function HundredCowMainCloseFunc()
    if UI.Controller.UIManager.IsWinShow("HundredCowMain") then
        roomMgr.bExiting = true
    end
    platformMgr.gameMgr.closeActiveFun = function()
        roomMgr.SendLeaveRoomMsg()
        -- UtilTools.RemoveAllWinExpect()
        UI.Controller.UIManager.RemoveAllWinExpect({"Waiting","MainCenterWin","MainWin"})
        
    end
    UnityTools.DestroyWin(wName)
    -- UnityTools.ReturnToMainCity()
    -- UnityTools.CallLoadingWin(true)
    -- UnityTools.ReturnToMainCity()
end
local function exitBtnCall(gameObject)

    if _TB._myHasBets == true then
        UnityTools.MessageDialog("中途退出将由系统为您自动打完",{okCall=function(f)
            closeFunc()
        end})
    else
        closeFunc()
    end
end

local function trendCloseBtnCall(gameObject)
    _trendAction:Play(false)
    local timer = gTimer.registerOnceTimer(200, function() 
        if UnityTools.IsWinShow(wName) == false then return nil end
        _trendLayer:SetActive(false)
    end)
    gTimer.setRecycler(wName, timer)
end

local function playerListLayerClose(gameObject)
    _pListLayerAction:Play(false)
    local timer = gTimer.registerOnceTimer(200, function() 
        if UnityTools.IsWinShow(wName) == false then return nil end
        _playerListLayer:SetActive(false)
    end)
    gTimer.setRecycler(wName, timer)
end

local function onShowPlayerCells(cellbox, index, item)
    index = index + 1
    item:SetActive(true)
    local pInfo = _playerInfoList[index]
    local icon = UnityTools.FindGo(item.transform, "icon")
    local hImg = UnityTools.FindCo(icon.transform, "UITexture", "img")
    local defaultImg = UnityTools.FindCo(icon.transform,"UISprite","img/head")
    if pInfo == nil then 
    -- 没有头像
        hImg.mainTexture = nil
        if defaultImg ~= nil then
            defaultImg.spriteName = "player_empty"
        end
    else
    -- 设置头像
        hImg.mainTexture = nil
        UnityTools.SetPlayerHead(pInfo.icon_url, hImg, platformMgr.PlayerUuid() == pInfo.player_uuid)
--        if defaultImg ~= nil and (pInfo.icon_url == nil or pInfo.icon_url == "") then
            defaultImg.spriteName = platformMgr.PlayerDefaultHead(pInfo.sex)
--        end
    end

    -- 设置名字与金币信息
    local name = UnityTools.FindCo(icon.transform, "UILabel", "name") 
    if name ~= nil and pInfo ~= nil then
        name.gameObject:SetActive(true)
        name.text = pInfo.player_name
        local money = UnityTools.FindCo(icon.transform, "UILabel", "money") 
        if money ~= nil then
            money.text = UnityTools.GetShortNum(pInfo.gold)
        end
    else 
        name.gameObject:SetActive(false)
    end 
    local vip = UnityTools.FindGo(item.transform, "vip/vipBox")
    if vip ~= nil then
        if pInfo ~= nil then
            UnityTools.SetNewVipBox(vip, pInfo.vip_level,"vip")
        else
            
            UnityTools.SetNewVipBox(vip, -1,"vip")
        end
    end
end

local function onShowDealerPlayerCells(cellbox, index, item)
    index = index + 1
    item:SetActive(true)
    local pInfo = _dealerPList[index]

    local dealer = UnityTools.FindGo(item.transform, "dealer")
    local bg = UnityTools.FindCo(item.transform, "UISprite", "bg")
    -- 设置名字与金币信息
    local name = UnityTools.FindCo(item.transform, "UILabel", "name")     
    local money = UnityTools.FindCo(item.transform, "UILabel", "money") 

    if index == 1 then
        dealer:SetActive(true)
        bg.gameObject:SetActive(true)
        money.text = getLongNumber(pInfo.gold)
        name.text = pInfo.player_name 
    else
        dealer:SetActive(false)
        bg.gameObject:SetActive(false)
        money.text = getLongNumber(pInfo.gold) 
        name.text =  pInfo.player_name 
    end
end

local function poolLayerCloseCall(gameObject)
    -- _poolLayerAction:Play(false)
    -- local timer = gTimer.registerOnceTimer(200, function() 
    --     if UnityTools.IsWinShow(wName) == false then return nil end
    --     _poolLayer:SetActive(false)
    -- end)
    -- gTimer.setRecycler(wName, timer)
    _poolLayer:SetActive(false)
end

local function updatePoolLayer(pInfo)
    if pInfo == nil then
        _maxPoolPlayer:SetActive(false)
        return nil 
    end
    _maxPoolPlayer:SetActive(true)
    local hImg = UnityTools.FindCo(_maxPoolPlayer.transform, "UITexture", "img")
    local defaultImg = UnityTools.FindCo(_maxPoolPlayer.transform,"UISprite","img/head")

    if pInfo == nil then 
    -- 没有头像
        hImg.mainTexture = nil
        if defaultImg ~= nil then
            defaultImg.spriteName = "player_empty"
        end
    else
        hImg.mainTexture = nil
        if pInfo.icon_url == "0000" then  
            defaultImg.spriteName = "head_img"
        else
            UnityTools.SetPlayerHead(pInfo.icon_url, hImg, platformMgr.PlayerUuid() == pInfo.player_uuid)
            defaultImg.spriteName = platformMgr.PlayerDefaultHead(pInfo.sex)
        end
    -- 设置 头像
        
    end
    local money = UnityTools.FindCo(_maxPoolPlayer.transform, "UILabel", "money") 
    if money ~= nil then
        money.text = getLongNumber(pInfo.gold)
    end
    local name = UnityTools.FindCo(_maxPoolPlayer.transform, "UILabel", "name") 
    name.text = pInfo.player_name

    local vip = UnityTools.FindGo(_maxPoolPlayer.transform, "vip/vipBox")
    if vip ~= nil then
        if pInfo ~= nil then
            UnityTools.SetNewVipBox(vip, pInfo.vip_level,"vip",_taskObj.thisObj)
        else
            UnityTools.SetNewVipBox(vip, -1,"vip")
        end
    end
end

local function clickPoolInfo(gameObject)
    _poolLayer:SetActive(true)
    -- _poolLayerAction:ResetToBeginning()
    -- _poolLayerAction:Play(true)
    sendMaxPoolMsg()
    -- updatePoolLayer()
end

function HundredCowMainPoolNumberTimer(curMoney, totalMoney)   
    local times = _efcTb._pEffTotalTime / _efcTb._pEffPerTime
    local change = math.ceil(totalMoney / times)
    change = math.random(math.ceil(change * 0.9), math.ceil(change * 1.3))
    curMoney = curMoney + change
    if curMoney >= totalMoney and UI.Controller.UIManager.IsWinShow("BigRewardWin") then --_poolEffLayer.activeSelf == true then
        curMoney = totalMoney
    else
        local timer = gTimer.registerOnceTimer(_efcTb._pEffPerTime, "HundredCowMainPoolNumberTimer", curMoney, totalMoney)        
        gTimer.setRecycler(wName, timer)
    end
    _pEffMoney.text = getLongNumber(curMoney)
end

local function doPoolAction(poolInfo)

    local totalMoney, rewardList = poolInfo.total_reward_num, poolInfo.seat_reward_num
    BigRwardCtrl.LabelValue = totalMoney
    BigRwardCtrl.bDelay=false
    BigRwardCtrl.bMask=true
    BigRwardCtrl.StillTime=3000
    BigRwardCtrl.bCloseTitle=true
    BigRwardCtrl.Type=0
    BigRwardCtrl.Sound=""

    
    -- _poolEffLayer:SetActive(true)
    local typePos = poolInfo.set_pos
    -- UnityTools.PrintTable(poolInfo.seat_reward_num)
--     LogError(" ->Type = " .. typePos)
    UnityTools.PlaySound("Sounds/Laba/pool1")
    UnityTools.PlaySound("Sounds/Laba/pool2",{target=_taskObj.thisObj})
    if typePos <= 0 then
        _poolLeftSp.spriteName = "pool_bg_3"
        _poolRightSp.spriteName = "pool_bg_3"
        BigRwardCtrl.PoolPos = 5
    else
        _poolLeftSp.spriteName = "type_" .. typePos .. "_1"
        _poolRightSp.spriteName = "type_" .. typePos .. "_1"
        BigRwardCtrl.PoolPos = typePos
    end
    UnityTools.CreateLuaWin("BigRewardWin")
    local curMoney = 0
    -- local timer = gTimer.registerOnceTimer(_efcTb._pEffPerTime, "HundredCowMainPoolNumberTimer", curMoney, totalMoney)  
    -- gTimer.setRecycler(wName, timer)

    timer = gTimer.registerOnceTimer(3000, function() 
        if UnityTools.IsWinShow(wName) == false then return nil end
        _poolEffLayer:SetActive(false)
        doGoldPoTPAction(rewardList)
    end)
    gTimer.setRecycler(wName, timer)
end

local function clickRoleBtn(gameObject)
    platformMgr.OpenTaskByGame()
    -- local rewardList = {}
    -- for i = 0, 7, 1 do
    --     rewardList[i + 1] = {player_pos = i, reward_num = math.random(3000, 500000)}
    -- end
    -- doPoolAction({set_pos = 0, total_reward_num = 500000, seat_reward_num = rewardList})
end


local function dealerLayerCloseCall(gameObject)
    -- _dealerLayerAction:Play(false)
    -- local timer = gTimer.registerOnceTimer(200, function() 
    --     if UnityTools.IsWinShow(wName) == false then return nil end
    --     _dealerLayer:SetActive(false)
    -- end)
    -- gTimer.setRecycler(wName, timer)
    _dealerLayer:SetActive(false)
end

local function clickDealerSureBtn(gameObject)
    if _myLastGold <= 0 then
        UnityTools.ShowMessage("携带的金币小于3000万，不能上庄")
        return
    end
    if _efcTb._waitServerBack == false then
        
        _efcTb._waitServerBack = true
        sendGetDealerMsg(_dealerBtnState, _selectBringGold)
        _dealerLayer:SetActive(false)
    end
end

local function ClickDealerHead(gameObject)
    local dealerInfo = roomMgr.GetPlayerInfo(1)
    if dealerInfo ~= nil and dealerInfo.icon_url ~= "0000" then
        platformMgr.OpenPlayerInfoInGame(dealerInfo.player_uuid)
    end
end

-- todo: SVN还原该函数 WP.Chu
local function ClickSetBtnCall(gameObject)
    platformMgr.OpenSetWin()
end

local function onMoneySliderDragEnd(data)
    local sliderValue = _moneySlider.value
    checkBringGold()
    _bringGoldLb.text = LuaText.Format("hundred_pool_desc2",tostring(_selectBringGold))
end

local function ClicMoneySlider(gameObject)
    onMoneySliderDragEnd(nil)
end

--- [ALF END]




function _taskObj.OnClickTask(gameObject)
    -- UnityTools.SetActive(_taskObj._taskBg.gameObject,_taskObj._taskBg.transform.localPosition.x<-5000)
end
function _taskObj.OnClickGetAward(gameObject)
    -- local id = ComponentData.Get(gameObject).Id
    -- if id == 1 then
    --     if _taskObj == nil or _taskObj.FruitCtrl == nil then
    --         return
    --     end

    --     if _taskObj.FruitCtrl.CowTable.status ~= 1 then
    --         return
    --     end
    --     UtilTools.ShowWaitFlag()
    --     protobuf:sendMessage(protoIdSet.cs_game_task_draw_req,{task_id = _taskObj.FruitCtrl.CowTable.taskId,game_type = 1})
    -- else
    --     id = id - 1
    --     if _taskObj.taskInfo ~= nil and id <= #_taskObj.taskInfo  then
    --         if _taskObj.taskInfo[id].state ==1 then
    --             protobuf:sendMessage(protoIdSet.cs_draw_mission_request,{id = _taskObj.taskInfo[id].id})
    --         end
    --     end

    -- end
end
function _taskObj.UpdateHundredCowTaskInfo()
    UnityTools.SetActive(_taskObj.go.gameObject,false)
    
    -- local _itemMgr = IMPORT_MODULE("ItemMgr")
    -- local isShowRed=false
    -- if _taskObj == nil or _taskObj.FruitCtrl == nil then
    --     return
    -- end
    -- _taskObj.taskInfo= _itemMgr.GetMissionListByType(5)
    
    -- if _taskObj.FruitCtrl.CowTable.taskId == nil then
    --     UnityTools.SetActive(_taskObj.go,false)
    --     return
    -- end
    -- local taskConfigData = LuaConfigMgr.CowTaskConfig[tostring(_taskObj.FruitCtrl.CowTable.taskId)]
    -- if taskConfigData==nil then
    --     UnityTools.SetActive(_taskObj.go,false)
    --     return
    -- end
    -- if _taskObj.FruitCtrl.CowTable.status==2 then
    --     UnityTools.SetActive(_taskObj.comObj[1].gameObject,true)
    --     _taskObj.cell[1].width = 477
    --     _taskObj.comObj[1].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[1].width/2,0,0)
    --     UnityTools.SetActive(_taskObj.taskSlider[1].gameObject,false)
    --     UnityTools.SetActive(_taskObj.taskStatus[1].gameObject,false)
    --     UnityTools.SetActive(_taskObj.taskDesc[1].gameObject,false)
    --     UnityTools.SetActive(_taskObj.taskIcon[1].gameObject,false)
    --     UnityTools.SetActive(_taskObj.taskNum[1].gameObject,false)
    --     --UnityTools.SetActive(_taskObj._taskRed.gameObject,false)
    -- else
    --     UnityTools.SetActive(_taskObj.go,true)
    --     UnityTools.SetActive(_taskObj.taskIcon[1].gameObject,true)
    --     UnityTools.SetActive(_taskObj.taskDesc[1].gameObject,true)
    --     UnityTools.SetActive(_taskObj.taskNum[1].gameObject,true)
    --     UnityTools.SetActive(_taskObj.comObj[1].gameObject,false)
    --     if _taskObj.FruitCtrl.CowTable.remaindTime ~= nil then
    --         _taskObj._lbCoolDown:SetEndTime(_taskObj.FruitCtrl.CowTable.remaindTime)
    --     end
    --     _taskObj.taskIcon[1].spriteName = "C"..taskConfigData.item1_id
    --     _taskObj.taskNum[1].text = UnityTools.GetShortNum(tonumber(taskConfigData.item1_num))
    --     _taskObj.taskDesc[1].text = taskConfigData.desc
    --     _taskObj.cell[1].width = 105 + _taskObj.taskDesc[1].width
    --     _taskObj.collider[1].size=UnityEngine.Vector3(_taskObj.cell[1].width,_taskObj.collider[1].size.y,_taskObj.collider[1].size.z)
    --     _taskObj.collider[1].center=UnityEngine.Vector3(-_taskObj.cell[1].width/2,0,0)
    --     _taskObj.itemTr[1].localPosition = UnityEngine.Vector3(-_taskObj.cell[1].width+50,14,0)
    --     _taskObj.taskDesc[1].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[1].width+100,15.6,0)
    --     _taskObj.line[1].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[1].width/2,40,0)
    --     _taskObj.line[1].width = _taskObj.cell[1].width - 50
    --     if _taskObj.FruitCtrl.CowTable.status == 1 then
    --         UnityTools.SetActive(_taskObj.taskSlider[1].gameObject,false)
    --         UnityTools.SetActive(_taskObj.taskStatus[1].gameObject,true)
    --         isShowRed = true
            
    --     elseif _taskObj.FruitCtrl.CowTable.status==0 then
    --         UnityTools.SetActive(_taskObj.taskSlider[1].gameObject,true)
    --         UnityTools.SetActive(_taskObj.taskStatus[1].gameObject,false)
    --         _taskObj.taskSliderSprite[1].width=_taskObj.taskDesc[1].width-7
    --         _taskObj.taskBackSprite[1].width = _taskObj.taskDesc[1].width-7
    --         _taskObj.sliderNum[1].text = _taskObj.FruitCtrl.CowTable.process.."/"..taskConfigData.par
    --         _taskObj.sliderNum[1].transform.localPosition=UnityEngine.Vector3(_taskObj.taskDesc[1].width/2-_taskObj.sliderNum[1].width/2,_taskObj.sliderNum[1].transform.localPosition.y,_taskObj.sliderNum[1].transform.localPosition.z)
    --         _taskObj.taskSlider[1].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[1].width+100,-15,0)
    --         _taskObj.taskSlider[1].value=_taskObj.FruitCtrl.CowTable.process/tonumber(taskConfigData.par)
            
    --     end
    -- end
    -- _taskObj._taskBg.width = _taskObj.cell[1].width
    -- if _taskObj.taskInfo == nil then
    --     _taskObj.taskInfo= {}
    -- end
    -- LogError("step7  count ="..tostring(#_taskObj.taskInfo))
    -- if #_taskObj.taskInfo == 1 and _taskObj.taskInfo[1].id == 511002 then
    --     _taskObj.taskInfo[2] = _taskObj.taskInfo[1]
    --     _taskObj.taskInfo[1] = {}
    --     _taskObj.taskInfo[1].id = 511001
    --     _taskObj.taskInfo[1].state = 2 
    -- end
    -- for i=1,2 do
    --     local tindex = i+1
    --     if i > #_taskObj.taskInfo then
    --         UnityTools.SetActive(_taskObj.comObj[tindex].gameObject,true)
    --         _taskObj.cell[tindex].width = _taskObj.cell[1].width
    --         _taskObj.comObj[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width/2,0,0)
    --         if tindex == 2 then
    --             _taskObj.line[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width/2,40,0)
    --             _taskObj.line[tindex].width = _taskObj.cell[tindex].width - 50
    --         end
    --         UnityTools.SetActive(_taskObj.taskSlider[tindex].gameObject,false)
    --         UnityTools.SetActive(_taskObj.taskStatus[tindex].gameObject,false)
    --         UnityTools.SetActive(_taskObj.taskDesc[tindex].gameObject,false)
    --         UnityTools.SetActive(_taskObj.taskIcon[tindex].gameObject,false)
    --         UnityTools.SetActive(_taskObj.taskNum[tindex].gameObject,false)
    --     else
    --         if _taskObj.taskInfo[i].state == 2 then
    --             UnityTools.SetActive(_taskObj.comObj[tindex].gameObject,true)
    --             _taskObj.cell[tindex].width = _taskObj.cell[1].width
    --             _taskObj.comObj[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width/2,0,0)
    --             if tindex == 2 then
    --                 _taskObj.line[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width/2,40,0)
    --                 _taskObj.line[tindex].width = _taskObj.cell[tindex].width - 50
    --             end
    --             UnityTools.SetActive(_taskObj.taskSlider[tindex].gameObject,false)
    --             UnityTools.SetActive(_taskObj.taskStatus[tindex].gameObject,false)
    --             UnityTools.SetActive(_taskObj.taskDesc[tindex].gameObject,false)
    --             UnityTools.SetActive(_taskObj.taskIcon[tindex].gameObject,false)
    --             UnityTools.SetActive(_taskObj.taskNum[tindex].gameObject,false)
    --         else
    --             local taskconfig = LuaConfigMgr.DailyTaskConfig[tostring(_taskObj.taskInfo[i].id)]
    --             if taskconfig ~=nil then
    --                 UnityTools.SetActive(_taskObj.taskIcon[tindex].gameObject,true)
    --                 UnityTools.SetActive(_taskObj.taskDesc[tindex].gameObject,true)
    --                 UnityTools.SetActive(_taskObj.taskNum[tindex].gameObject,true)
    --                 UnityTools.SetActive(_taskObj.comObj[tindex].gameObject,false)
    --                 _taskObj.taskIcon[tindex].spriteName = "C101"
    --                 _taskObj.taskNum[tindex].text = UnityTools.GetShortNum(tonumber(taskconfig.item1_num))
    --                 _taskObj.taskDesc[tindex].text = taskconfig.desc
    --                 _taskObj.cell[tindex].width = _taskObj.cell[1].width
    --                 _taskObj.collider[tindex].size=UnityEngine.Vector3(_taskObj.cell[tindex].width,_taskObj.collider[tindex].size.y,_taskObj.collider[tindex].size.z)
    --                 _taskObj.collider[tindex].center=UnityEngine.Vector3(-_taskObj.cell[tindex].width/2,0,0)
    --                 _taskObj.itemTr[tindex].localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width+50,14,0)
    --                 _taskObj.taskDesc[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width+100,15.6,0)
    --                 if tindex == 2 then
    --                     _taskObj.line[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width/2,40,0)
    --                     _taskObj.line[tindex].width = _taskObj.cell[tindex].width - 50
    --                 end
    --                 if _taskObj.taskInfo[i].state == 1 then
    --                     UnityTools.SetActive(_taskObj.taskSlider[tindex].gameObject,false)
    --                     UnityTools.SetActive(_taskObj.taskStatus[tindex].gameObject,true)
    --                     isShowRed = true
    --                 elseif _taskObj.FruitCtrl.CowTable.status==0 then
    --                     UnityTools.SetActive(_taskObj.taskSlider[tindex].gameObject,true)
    --                     UnityTools.SetActive(_taskObj.taskStatus[tindex].gameObject,false)
    --                     _taskObj.taskSliderSprite[tindex].width=_taskObj.taskDesc[tindex].width-7
    --                     _taskObj.taskBackSprite[tindex].width = _taskObj.taskDesc[tindex].width-7
    --                     _taskObj.sliderNum[tindex].text = "0/1"
    --                     _taskObj.sliderNum[tindex].transform.localPosition=UnityEngine.Vector3(_taskObj.taskDesc[tindex].width/2-_taskObj.sliderNum[tindex].width/2,_taskObj.sliderNum[tindex].transform.localPosition.y,_taskObj.sliderNum[tindex].transform.localPosition.z)
    --                     _taskObj.taskSlider[tindex].transform.localPosition = UnityEngine.Vector3(-_taskObj.cell[tindex].width+100,-15,0)
    --                     _taskObj.taskSlider[tindex].value= 0
    --                 end
    --             end
    --         end
    --     end
    -- end
    -- UnityTools.SetActive(_taskObj._taskRed.gameObject,isShowRed)
end
function _taskObj.OnClickRank(gameObject)
    UnityTools.CreateLuaWin("RankWin")
end
function _boxTaskTb.UpdateBoxTask()
    if _boxTaskTb.boxConfig == nil then
        return
    end
    if _boxTaskTb.getValue>= tonumber(_boxTaskTb.boxConfig.condition) then
        _boxTaskTb.boxTween.enabled = true
        UnityTools.SetActive(_boxTaskTb.undoObj.gameObject,false)
        UnityTools.SetActive(_boxTaskTb.comObj.gameObject,true)
        
    else
        _boxTaskTb.boxTween.enabled = false
        UnityTools.SetActive(_boxTaskTb.undoObj.gameObject,true)
        UnityTools.SetActive(_boxTaskTb.comObj.gameObject,false)
        _boxTaskTb.sliderVal.text = _boxTaskTb.getValue.."/".._boxTaskTb.boxConfig.condition
        _boxTaskTb.slider.value = _boxTaskTb.getValue/tonumber(_boxTaskTb.boxConfig.condition)
    end
end



function _boxTaskTb.resetBgmValue()
    if _boxTaskTb.recordCount <=0 then
        _boxTaskTb.recordCount =0
        local bgm = UnityEngine.GameObject.Find("Scene"):GetComponent("AudioSource")
        -- bgm.enabled = true
        -- bgm:Play()
        local bgmValue = UnityEngine.PlayerPrefs.GetFloat("bgmValue", 50);
        
        bgm.volume = bgmValue/100.0;
        UnityEngine.PlayerPrefs.SetFloat("gameValue",_boxTaskTb.nEffectVolum);
    end
end
function _boxTaskTb.chatHiddenComplete()
	local curChat = TweenAlpha.current.transform;
    UnityTools.Destroy(curChat.gameObject)
	_boxTaskTb.recordCount = _boxTaskTb.recordCount - 1;
	_boxTaskTb.resetBgmValue();
end
function _boxTaskTb.chatHiddenComplete2()
	local curChat = TweenAlpha.current.transform;
    curChat.gameObject:SetActive(false)
	_boxTaskTb.recordCount = _boxTaskTb.recordCount - 1;
	_boxTaskTb.resetBgmValue();
end

function _boxTaskTb.resetRecordData()
    _boxTaskTb.sAudioName = "";
    _boxTaskTb.bStartRecord = false;
    _boxTaskTb.recordCurTime= 0;
    _boxTaskTb.startRecordTime = 0;
    _boxTaskTb.resetBgmValue()
end

function _boxTaskTb.recordOver()
    _boxTaskTb.AudioPanel.from = 1 
    _boxTaskTb.AudioPanel.to = 0
    _boxTaskTb.AudioPanel.duration = 0.2
    _boxTaskTb.AudioPanel.enabled = true
    _boxTaskTb.AudioPanel:ResetToBeginning();
    _boxTaskTb.AudioPanel:PlayForward()
    gTimer.removeTimer("AudioLineActionUpdate");
    UnityTools.stopRecord();
    local nEndTime = UtilTools.GetCurrentTime();
    if nEndTime - _boxTaskTb.startRecordTime > 1 then
		--UtilTools.asyncAudioUploadFile(_sAudioName);
		ComponentData.Get(_boxTaskTb.AudioPanel.gameObject).Value = 111;
		UtilTools.UploadAudioFileAndToText(_boxTaskTb.sAudioName .. "," .. (nEndTime - _boxTaskTb.startRecordTime),_boxTaskTb.AudioPanel.gameObject);
	else
		UnityTools.deleteAudio(_boxTaskTb.sAudioName);
	end

	_boxTaskTb.recordCount = _boxTaskTb.recordCount - 1;
	_boxTaskTb.resetRecordData()
end
function AudioLineActionUpdate()
	_boxTaskTb.recordCurTime = _boxTaskTb.recordCurTime + 500;
	_boxTaskTb.CountDownTime.text = math.ceil((_boxTaskTb.recordMaxTime - _boxTaskTb.recordCurTime) / 1000) .. "s";
	if _boxTaskTb.recordMaxTime - _boxTaskTb.recordCurTime <= 0 then
		_boxTaskTb.recordOver()
	end
end

function _boxTaskTb.startAudioLineAction()
    _boxTaskTb.CountDownTime.text = math.ceil((_boxTaskTb.recordMaxTime - _boxTaskTb.recordCurTime) / 1000) .. "s";
    gTimer.registerRepeatTimer(500, "AudioLineActionUpdate")
end
function _boxTaskTb.onAudioPress(gameObject,isPress)
    if isPress then
        _boxTaskTb.resetRecordData()
        _boxTaskTb.startRecordTime = UtilTools.GetCurrentTime();
        _boxTaskTb.sAudioName = wName.."_"..platformMgr.PlayerUuid().."_"..math.ceil(_boxTaskTb.startRecordTime)..".amr";
      
        if UnityTools.startRecord(_boxTaskTb.sAudioName) then
            _boxTaskTb.startAudioLineAction();
            _boxTaskTb.AudioPanel.from = 0 
            _boxTaskTb.AudioPanel.to = 1
            _boxTaskTb.AudioPanel.duration = 0.2
            _boxTaskTb.AudioPanel.enabled = true
            _boxTaskTb.AudioPanel:ResetToBeginning();
            _boxTaskTb.AudioPanel:PlayForward()
            local bgm = UnityEngine.GameObject.Find("Scene"):GetComponent("AudioSource")
            bgm.volume = 0
            -- bgm.enabled = false;
            _boxTaskTb.bStartRecord = true
            _boxTaskTb.recordCount = _boxTaskTb.recordCount+1

            _boxTaskTb.nEffectVolum = UnityEngine.PlayerPrefs.GetFloat("gameValue", 50);
            UnityEngine.PlayerPrefs.SetFloat("gameValue",0);
        else
            _boxTaskTb.resetRecordData()
        end
    else
        if _boxTaskTb.bStartRecord then
            _boxTaskTb.recordOver();
        end
    end
end
function audioReadyToStart(EventID, tMsgData)
	if tMsgData.chat ~= nil then
		-- local chatPanel = tfContainer:Find("ChatPanel");
		-- local playerContentPanel = tMsgData.chat.transform:Find("TextContentPanel");
		-- local playerEmojiPanel = tMsgData.chat.transform:Find("Emoji");
		-- local playerAudioPanel = tMsgData.chat.transform:Find("AudioContentPanel");
		-- tMsgData.chat.gameObject:SetActive(true);
		-- playerContentPanel.gameObject:SetActive(false);
		-- playerEmojiPanel.gameObject:SetActive(false);
		-- playerAudioPanel.gameObject:SetActive(true);

		-- local chatWidget = tMsgData.chat:GetComponent("UIWidget");
		-- chatWidget.alpha = 1;

		-- local twAlpha = tMsgData.chat:GetComponent("TweenAlpha");
		-- twAlpha.from = 1;
		-- twAlpha.to = 0;
		-- twAlpha.duration = 0.5;
        local tag =ComponentData.Get(tMsgData.chat.gameObject).Tag
        if tag ~= 30 and tag ~= 31 then
            return
        end
        local bgm = UnityEngine.GameObject.Find("Scene"):GetComponent("AudioSource")
        bgm.volume = 0
        _boxTaskTb.nEffectVolum = UnityEngine.PlayerPrefs.GetFloat("gameValue", 50);
        UnityEngine.PlayerPrefs.SetFloat("gameValue",0);
        local twAlpha = tMsgData.chat.gameObject:GetComponent("TweenAlpha")
        local sAudioInfo = ComponentData.Get(tMsgData.chat.gameObject).Text;
		local sAudioList = stringToTable(sAudioInfo, ",");
        UnityTools.playAudio(sAudioList[1]);
        if twAlpha == nil then
            twAlpha = tMsgData.chat.gameObject:AddComponent("TweenAlpha")
        end
        twAlpha.from = 1;
        twAlpha.to = 0;
        twAlpha.duration = 0.5;
        twAlpha.delay = tonumber(sAudioList[2]);
        twAlpha.enabled = true;
        twAlpha:ResetToBeginning();
        twAlpha:PlayForward()
            
		if tag == 30 then
            EventDelegate.Add(twAlpha.onFinished,_boxTaskTb.chatHiddenComplete2,true);
        else
            EventDelegate.Add(twAlpha.onFinished,_boxTaskTb.chatHiddenComplete,true);
        end

		_boxTaskTb.recordCount = _boxTaskTb.recordCount + 1;
		
	end
end
local function binLuaData_4(gameObject)
    _taskObj.go = UnityTools.FindGo(gameObject.transform, "Container/UILayer/btnFreeGold")
    
    -- UnityTools.AddOnClick(_taskObj.go.gameObject, _taskObj.OnClickTask)
    -- _taskObj._lbCoolDown = UnityTools.FindGo(gameObject.transform, "Container/UILayer/btnFreeGold/cool"):GetComponent("CooldownUpdate")
    -- _taskObj._taskBg = UnityTools.FindGo(gameObject.transform, "Container/UILayer/btnFreeGold/bg"):GetComponent("UISprite")
    -- _taskObj._taskRed = UnityTools.FindGo(gameObject.transform, "Container/UILayer/btnFreeGold/red")
    -- _taskObj.spIcon=UnityTools.FindGo(gameObject.transform, "Container/UILayer/btnFreeGold/Sprite"):GetComponent("UISprite")
    -- _taskObj.cell={}
    -- _taskObj.itemTr={}
    -- _taskObj.taskIcon={}
    -- _taskObj.taskNum={}
    -- _taskObj.taskDesc={}
    -- _taskObj.taskSlider={}
    -- _taskObj.taskSliderSprite={}
    -- _taskObj.taskBackSprite={}
    -- _taskObj.sliderNum={}
    -- _taskObj.taskStatus={}
    -- _taskObj.comObj={}
    -- _taskObj.collider={}
    -- _taskObj.line = {}
    -- for i=1,3 do 
    --     _taskObj.cell[i] = UnityTools.FindGo(_taskObj._taskBg.transform, "cell"..i):GetComponent("UIWidget") 
    --     ComponentData.Get(_taskObj.cell[i].gameObject).Id = i
    --     UnityTools.AddOnClick(_taskObj.cell[i].gameObject, _taskObj.OnClickGetAward)
    --     _taskObj.itemTr[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "item").transform
    --     _taskObj.taskIcon[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "item/Sprite"):GetComponent("UISprite")
    --     _taskObj.taskNum[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "item/num"):GetComponent("UILabel")
    --     _taskObj.taskDesc[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "desc"):GetComponent("UILabel")
    --     _taskObj.taskSlider[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "slider"):GetComponent("UISlider")
    --     _taskObj.collider[i] = _taskObj.cell[i]:GetComponent("BoxCollider")
    --     _taskObj.taskSliderSprite[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "slider"):GetComponent("UISprite")
    --     if i<=2 then
    --         _taskObj.line[i] =UnityTools.FindGo(_taskObj.cell[i].transform, "line"):GetComponent("UISprite")
    --     end
    --     _taskObj.taskBackSprite[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "slider/thumb"):GetComponent("UISprite")

        
    --     _taskObj.sliderNum[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "slider/Label"):GetComponent("UILabel")
    --     _taskObj.taskStatus[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "status")
    --     _taskObj.comObj[i] = UnityTools.FindGo(_taskObj.cell[i].transform, "com")
    -- end
    -- UnityTools.SetActive(_taskObj._taskBg.gameObject,false)
    _taskObj.rankBtn = UnityTools.FindGo(gameObject.transform, "Container/btnRank")
    -- _taskObj.FruitCtrl = IMPORT_MODULE("FruitWinController")
    UnityTools.SetActive(_taskObj.rankBtn.gameObject,false)

    UnityTools.AddOnClick(_taskObj.rankBtn.gameObject, _taskObj.OnClickRank)
    _taskObj.UpdateHundredCowTaskInfo()

    _boxTaskTb.undoObj = UnityTools.FindGo(gameObject.transform, "Container/box/undo")
    _boxTaskTb.boxTween = UnityTools.FindGo(gameObject.transform, "Container/box/box"):GetComponent("TweenRotation")
    _boxTaskTb.boxTween.enabled = false
    _boxTaskTb.comObj = UnityTools.FindGo(gameObject.transform, "Container/box/com")
    _boxTaskTb.sliderVal = UnityTools.FindGo(gameObject.transform, "Container/box/undo/sliderbg/sliderval"):GetComponent("UILabel")
    _boxTaskTb.slider = UnityTools.FindGo(gameObject.transform, "Container/box/undo/sliderbg"):GetComponent("UISlider")
    _boxTaskTb.boxConfig = LuaConfigMgr.ChestConfig[tostring(3)]
    
    UnityTools.AddOnClick(_boxTaskTb.boxTween.gameObject, _boxTaskTb.OnClickBoxTask)
    _boxTaskTb.UpdateBoxTask()

    _boxTaskTb.btnAudio = UnityTools.FindGo(gameObject.transform, "Container/bg/downLayer/btn4")
    UIEventListener.Get(_boxTaskTb.btnAudio.gameObject).onPress = _boxTaskTb.onAudioPress

    _boxTaskTb.AudioPanel = UnityTools.FindGo(gameObject.transform, "Container/UILayer/AudioPanel"):GetComponent("TweenAlpha")
    _boxTaskTb.CountDownTime = UnityTools.FindGo(gameObject.transform, "Container/UILayer/AudioPanel/Back/CountDownTime"):GetComponent("UILabel")
    _boxTaskTb.recordCount = 0
    _boxTaskTb.recordMaxTime = 10000;
    _boxTaskTb.nEffectVolum = UnityEngine.PlayerPrefs.GetFloat("gameValue", 50);
end




local function binLuaData_3(gameObject)
    _dealerSureLabel = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/UILayer/dealerLayer/Content/sureBtn/Label")

    _chatCell1 = UnityTools.FindGo(gameObject.transform, "Container/UILayer/chatLayer/cellLayer/cell1")

    _chatCell2 = UnityTools.FindGo(gameObject.transform, "Container/UILayer/chatLayer/cellLayer/cell2")

    _chatCell3 = UnityTools.FindGo(gameObject.transform, "Container/UILayer/chatLayer/cellLayer/cell3")

    _TB._pChatR = UnityTools.FindGo(gameObject.transform, "Container/UILayer/chatLayer/pChatR")

    _TB._pChatL = UnityTools.FindGo(gameObject.transform, "Container/UILayer/chatLayer/pChatL")

    _emojiCell = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/UILayer/chatLayer/emoji")

    _TB._mainDealerBtnLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/playerLayer/playerCell0/btn/l2")

    _TB._dealerHeadBtn = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell0/player_img_bg/img")
    UnityTools.AddOnClick(_TB._dealerHeadBtn.gameObject, ClickDealerHead)

    _setBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer/btn3")
    UnityTools.AddOnClick(_setBtn.gameObject, ClickSetBtnCall)

    _bringGoldLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/UILayer/dealerLayer/Content/tip1")

    _moneySlider = UnityTools.FindCo(gameObject.transform, "UISlider", "Container/UILayer/dealerLayer/Content/moneySlider")
    UIEventListener.Get(_moneySlider.gameObject).onDrag = onMoneySliderDragEnd
    UnityTools.AddOnClick(_moneySlider.gameObject, ClicMoneySlider)
    if _moneySlider ~= nil then
        _moneySliderCollider = _moneySlider.gameObject:GetComponent("BoxCollider")
    end

    _uiLayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer")

    _resultMask = UnityTools.FindGo(gameObject.transform, "Container/bg/mask")
    UnityTools.SetActive(_resultMask, false)
 
    _getGoldBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn4")
    UnityTools.AddOnClick(_getGoldBtn.gameObject, ClickGetGoldBtn)
    
    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        _getGoldBtn:SetActive(false)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽百人房间对局内获取金币按钮")
    end

    _chatBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/downLayer/btn1")
    UnityTools.AddOnClick(_chatBtn.gameObject, ClickChatBtn)

    _playerListBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/downLayer/btn2")
    UnityTools.AddOnClick(_playerListBtn.gameObject, ClickPlayerListBtn)

    _trendBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/downLayer/btn3")
    UnityTools.AddOnClick(_trendBtn.gameObject, ClickTrendBtn)

    _TB.redSp = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn5/red")

    _upPokerLayer = _pokerLayer:GetComponent("UIPanel")
    _upUILayer = _uiLayer:GetComponent("UIPanel")
    _upSVPlayerList = _sv_playerList:GetComponent("UIPanel")
    _upSVDealerLayer = _sv_dealerLayer:GetComponent("UIPanel")
    _upMainTrans = _mainTransfrom.gameObject:GetComponent("UIPanel")
    _TB.manager = _mainTransfrom.gameObject:GetComponent("FastMoveManager")
    gTimer.registerOnceTimer(50, binLuaData_4, gameObject)
end

local function binLuaData_2(gameObject)

    _menuLayer = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer")

    _getDealerBtn = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell0/btn")
    UnityTools.AddOnClick(_getDealerBtn.gameObject, ClickGetDealerBtn)

    _menuBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1")
    UnityTools.AddOnClick(_menuBtn.gameObject, ClickMenuBtn)

    _closeBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer/btn2")
    UnityTools.AddOnClick(_closeBtn.gameObject, exitBtnCall)

    _trendLayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer/trendLayer")

    _trendAction = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/UILayer/trendLayer/Content")

    _trendCloseBtn = UnityTools.FindGo(gameObject.transform, "Container/UILayer/trendLayer/Content/bg/close")
    UnityTools.AddOnClick(_trendCloseBtn.gameObject, trendCloseBtnCall)

    _playerListLayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer/playerListLayer")

    _pListLayerAction = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/UILayer/playerListLayer/Content")

    _pListLayerClose = UnityTools.FindGo(gameObject.transform, "Container/UILayer/playerListLayer/Content/bg/close")
    UnityTools.AddOnClick(_pListLayerClose.gameObject, playerListLayerClose)

    _sv_playerList = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/UILayer/playerListLayer/Content/ScrollView")
    _sv_playerList_mgr = UnityTools.FindCoInChild(_sv_playerList, "UIGridCellMgr")
    _sv_playerList_mgr.onShowItem = onShowPlayerCells
    

    _pListTitle = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/UILayer/playerListLayer/Content/bg/bg/Sprite/title")

    _poolLayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer/poolLayer")

    _poolLayerAction = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/UILayer/poolLayer/Content")

    _poolLayerClose = UnityTools.FindGo(gameObject.transform, "Container/UILayer/poolLayer/Content/bg/close")
    UnityTools.AddOnClick(_poolLayerClose.gameObject, poolLayerCloseCall)

    _betPoolBtn = UnityTools.FindGo(gameObject.transform, "Container/betLayer/betPool")
    UnityTools.AddOnClick(_betPoolBtn.gameObject, clickPoolInfo)

    _poolEffLayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer/resultEffect")

    _pEffMoney = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/UILayer/resultEffect/money/label")

    _showRoleBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn5")
    UnityTools.AddOnClick(_showRoleBtn.gameObject, clickRoleBtn)
    if version.VersionData.IsReviewingVersion() then
        _showRoleBtn.gameObject:SetActive(false)
    end
    _poolLeftSp = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/UILayer/resultEffect/typel")

    _poolRightSp = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/UILayer/resultEffect/typer")

    _dealerLayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer/dealerLayer")

    _dealerLayerAction = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/UILayer/dealerLayer/Content")

    _dealerLayerCloseBtn = UnityTools.FindGo(gameObject.transform, "Container/UILayer/dealerLayer/Content/bg/close")
    UnityTools.AddOnClick(_dealerLayerCloseBtn.gameObject, dealerLayerCloseCall)

    _sv_dealerLayer = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/UILayer/dealerLayer/Content/scrollview")
    _sv_dealerLayer_mgr = UnityTools.FindCoInChild(_sv_dealerLayer, "UIGridCellMgr")
    _sv_dealerLayer_mgr.onShowItem = onShowDealerPlayerCells
    -- _controller:SetScrollViewRenderQueue(_sv_dealerLayer.gameObject)

    _dealerSureBtn = UnityTools.FindGo(gameObject.transform, "Container/UILayer/dealerLayer/Content/sureBtn")
    UnityTools.AddOnClick(_dealerSureBtn.gameObject, clickDealerSureBtn)
    gTimer.registerOnceTimer(50, binLuaData_3, gameObject)
end

local function binLuaData_1(gameObject)
    _betBtnList[1] = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn1")
    UnityTools.AddOnClick(_betBtnList[1] .gameObject, ClickBetBtn)
    _clickBetBtn = _betBtnList[1] 
    _clickBetIndex = 1

    _betBtnList[2] = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn2")
    UnityTools.AddOnClick(_betBtnList[2].gameObject, ClickBetBtn)

    _betBtnList[3] = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn3")
    UnityTools.AddOnClick(_betBtnList[3].gameObject, ClickBetBtn)

    _betBtnList[4] = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn4")
    UnityTools.AddOnClick(_betBtnList[4].gameObject, ClickBetBtn)

    _betBtnList[5] = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn5")
    UnityTools.AddOnClick(_betBtnList[5].gameObject, ClickBetBtn)

    local betBtnTemp = nil
    for i=1, 5 do
        betBtnTemp = _betBtnList[i]
        if betBtnTemp ~= nil then
            _betBtnColliders[i] = betBtnTemp:GetComponent("BoxCollider")
            _betBtnSprites[i] = betBtnTemp:GetComponent("UISprite")
        end
    end

    _myBetCnt1 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/betLayer/bet1/my")
    _myBetCnt2 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/betLayer/bet2/my")
    _myBetCnt3 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/betLayer/bet3/my")
    _myBetCnt4 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/betLayer/bet4/my")
    _taskObj.lights={}
    _taskObj.lights[1] = UnityTools.FindGo(gameObject.transform,  "Container/betLayer/bet1/light")
    _taskObj.lights[2] = UnityTools.FindGo(gameObject.transform, "Container/betLayer/bet2/light")
    _taskObj.lights[3] = UnityTools.FindGo(gameObject.transform,  "Container/betLayer/bet3/light")
    _taskObj.lights[4] = UnityTools.FindGo(gameObject.transform,  "Container/betLayer/bet4/light")
    for i=1,4 do
        UnityTools.SetActive(_taskObj.lights[i],false)
    end
    _goldPrefab = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/goldLayer/goldPrefab")
    _mrGoldPrefab = _goldPrefab:GetComponent("MeshRenderer")

    _goldLayer = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/goldLayer")

    _betLayer = UnityTools.FindGo(gameObject.transform, "Container/betLayer")
    for i = 1, 4, 1 do
        _betBtnPos[i] = UnityTools.FindTf(_betLayer.transform, "bet" .. tostring(i)).localPosition
    end


    _myPlayerCell = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1")

    
    -- 庄家位置
    _dealerCell = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell0")


    _playerLayer = UnityTools.FindGo(gameObject.transform, "Container/playerLayer")

    _pokerCenter = UnityTools.FindGo(gameObject.transform, "Container/pokerLayer/Container")

    _pokerLayer = UnityTools.FindGo(gameObject.transform, "Container/pokerLayer")

    

    _startEffect = UnityTools.FindGo(gameObject.transform, "Container/startEffect")
    _TB._tbStartCompt.ct = _TB._tbStartCompt.ct or UnityTools.FindCo(_startEffect.transform, "TweenAlpha", "Container")
    _TB._tbStartCompt.bg = _TB._tbStartCompt.bg or UnityTools.FindCo(_startEffect.transform, "TweenScale", "Container/bg")
    _TB._tbStartCompt.lb0 = _TB._tbStartCompt.lb0 or UnityTools.FindCo(_startEffect.transform, "TweenPosition", "Container/lb_0")
    _TB._tbStartCompt.lb1 = _TB._tbStartCompt.lb1 or UnityTools.FindCo(_startEffect.transform, "TweenPosition", "Container/lb_1")
    _TB._tbStartCompt.lb3 = _TB._tbStartCompt.lb3 or UnityTools.FindCo(_startEffect.transform, "TweenAlpha", "Container/Label")
    UnityTools.SetActive(_startEffect, false)

    _rewardPoolLb = UnityTools.FindCo(gameObject.transform, "UILabel","Container/betLayer/betPool/Label")

    _operLayer = UnityTools.FindGo(gameObject.transform, "Container/operLayer")

    _stateLayer = UnityTools.FindGo(gameObject.transform, "Container/stateLayer")
    UnityTools.SetActive(_stateLayer, false)

    _timeBar = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/stateLayer/timeBar/bar")

    _timeLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/stateLayer/timeBar/timeLb")

    _stateTip = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/stateLayer/tip")

    _totalPoolMoney = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/UILayer/poolLayer/Content/bg/pBg/money")

    _maxPoolPlayer = UnityTools.FindGo(gameObject.transform, "Container/UILayer/poolLayer/Content/player")
    gTimer.registerOnceTimer(50, binLuaData_2, gameObject)

    for i=1, 4 do 
        _betPosTotalGoldLbl[i] = UnityTools.FindCo(_betLayer.transform, "UILabel", "bet" .. i .. "/total")
    end

end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    -- gTimer.registerOnceTimer(50, binLuaData_1, gameObject)
    
    
    -- _closeBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer/btn2")
    -- UnityTools.AddOnClick(_closeBtn.gameObject, exitBtnCall)

    binLuaData_1(gameObject)
    -- binLuaData_2(gameObject)
    -- binLuaData_3(gameObject)

    

    

--- [ALB END]

end
function HundredCowTaskUpdate()
    _taskObj.UpdateHundredCowTaskInfo()
end
function HundredRecvGameTimesReply(idmsg,result)
    if result ~= nil then
        _boxTaskTb.getValue = result.times
        _boxTaskTb.UpdateBoxTask()
    end
end

local function initListener()
    registerScriptEvent(EVENT_NIU_ENTER_ROOM, "HundredCowMainRecvEnterResult")
    registerScriptEvent(EVENT_NIU_UPDATE_PLAYER_INFO, "HundredCowMainRecvUpdatePlayer")
    registerScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, "HundredCowMainRecvUpdateState")
    registerScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, "HundredCowMainRecvChatInfo")
    registerScriptEvent(UPDATE_MAIN_WIN_RED, "HundredCowMainUpdateTaskRed")
    registerScriptEvent(EVENT_AUDIOLOAD_COMPLETE, "audioReadyToStart");
    registerScriptEvent(EVENT_RECONNECT_SOCKET, "HundredCowMainCloseFunc")
    
    -- registerScriptEvent(EVENT_TASK_INFO_UPDATE, "HundredCowTaskUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_niu_free_set_chips_reply,"HCRRecvFreeBetReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_niu_free_set_chips_update,"HCRRecvFreeBetUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_query_winning_rec_reply,"HCRRecvTrendUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_niu_player_list_query_reply,"HCRRecvPlayerListUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_player_gold_change_update,"HCRRecvMyGoldChangeUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_query_pool_win_player_reply,"HCRRecvMaxPoolPlayerUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_query_master_list_reply,"HCRRecvDealerListUpdate")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_be_master_reply,"HCRRecvGetDealerReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_room_chest_info_update,"HundredRecvGameTimesReply")
    
end

local function initData()
    UnityTools.AddOnClick(UnityTools.FindGo(_playerLayer.transform, "playerCell2"), ClickPlayerCellBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_playerLayer.transform, "playerCell3"), ClickPlayerCellBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_playerLayer.transform, "playerCell4"), ClickPlayerCellBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_playerLayer.transform, "playerCell5"), ClickPlayerCellBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_playerLayer.transform, "playerCell6"), ClickPlayerCellBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_playerLayer.transform, "playerCell7"), ClickPlayerCellBtn)

    UnityTools.AddOnClick(UnityTools.FindGo(_betLayer.transform, "bet1"), ClickBetTypeBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_betLayer.transform, "bet2"), ClickBetTypeBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_betLayer.transform, "bet3"), ClickBetTypeBtn)
    UnityTools.AddOnClick(UnityTools.FindGo(_betLayer.transform, "bet4"), ClickBetTypeBtn)

    _pokerList = getChilds(_pokerLayer, "pokerCell", 5)
    _freeCellList = getChilds(_playerLayer.gameObject, "playerCell", 7, 2)
    _allCellList = getChilds(_playerLayer.gameObject, "playerCell", 7, 0)
    pokerMgr.GetPokersTable(_pokerObjTable, _pokerList)

    setRewardPool(0)
    resetGoldInfo()
    resetFreeCell()
    changeCanBet(false)
end

local function Awake(gameObject)
    _mainTransfrom = gameObject.transform
    _taskObj.thisObj = gameObject
    _taskObj.zhuang = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell0/player_img_bg/zhuang"):GetComponent("UIPanel")
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    
    initListener()
    initData()

end

function HundredWinGoldActionTimerCall()
    if _goldActionFuncs ~= nil then
        local index = 50
        local delay = 0
        for k, v in pairs(_goldActionFuncs) do
            if v ~= nil and k ~= nil then
                if index == 0 then
                    return nil 
                end
                v(delay, k)
                delay = delay + math.random(-2, 25)
                _goldActionFuncs[k] = nil
                index = index - 1
            end
        end
    end
    
end 

local function Start(gameObject)
    UnityTools.RecordOpenTime(true)
    local t1 = gTimer.registerOnceTimer(200, gameMgr.CallSucc)
    UnityTools.CreatePool(_goldPrefab, _goldLayer, 3500, false, 0, UnityEngine.Vector3(-10000, -10000, -10000))
    
    local t2 = gTimer.registerOnceTimer(400, roomMgr.SendJoinRoomMsg)
    
    gTimer.setRecycler(wName, t1)
    gTimer.setRecycler(wName, t2)

end
 

local function OnDestroy(gameObject)
    cleanSoundEff()
    UnityTools.RemoveDeactiveList()
    _mainTransfrom = nil
    _trendDataList = nil
    _myBetList = nil
    _allGoldList = nil
    _freeBetList = nil
    _freeCellList = nil
    _pokerOrginPos = nil
    _playerInfoList = nil
    _dealerPList = nil
    _goldFActionFuncs = nil
    _goldActionFuncs = nil
    _upPokerLayer = nil
    _upUILayer = nil
    _upSVPlayerList = nil
    _upSVDealerLayer = nil
    _upMainTrans = nil
    _tbStartCompt = nil
    _betBtnList = nil
    _betBtnColliders = nil
    _betBtnSprites = nil
    _pokerObjTable = nil
    _pokerList = nil
    _betBtnPos = nil
    _allCellList = nil
    _TB = nil
    _moneySliderCollider = nil
    _betPosTotalGoldLbl = nil
    unregisterScriptEvent(EVENT_RECONNECT_SOCKET, "HundredCowMainCloseFunc")
    unregisterScriptEvent(UPDATE_MAIN_WIN_RED, "HundredCowMainUpdateTaskRed")
    unregisterScriptEvent(EVENT_NIU_ENTER_ROOM, "HundredCowMainRecvEnterResult")
    unregisterScriptEvent(EVENT_NIU_UPDATE_PLAYER_INFO, "HundredCowMainRecvUpdatePlayer")
    unregisterScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, "HundredCowMainRecvUpdateState")
    unregisterScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, "HundredCowMainRecvChatInfo")
    unregisterScriptEvent(EVENT_AUDIOLOAD_COMPLETE, "audioReadyToStart");
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_niu_free_set_chips_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_niu_free_set_chips_update)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_query_winning_rec_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_niu_player_list_query_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_player_gold_change_update)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_query_pool_win_player_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_query_master_list_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_be_master_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_room_chest_info_update)
    -- unregisterScriptEvent(EVENT_TASK_INFO_UPDATE, "HundredCowTaskUpdate")
    gTimer.removeTimer("HundredCowMainStateTimeCall")
    gTimer.removeTimer(HundredCowMainLastTimerCall)
    gTimer.removeTimer(HundredWinGoldActionTimerCall)
    gTimer.recycling(wName)

    UnityTools.DelPools()
    CLEAN_MODULE("HundredCowMainMono")
    gameMgr.ExitGame()
end


local function chatCellAction(cell, func, msgData)
    cell:SetActive(true)
    local action = cell:GetComponent("TweenPosition")
    local hImg = UnityTools.FindCo(cell.transform, "UITexture", "img")
    local defaultImg = UnityTools.FindCo(cell.transform,"UISprite","img/head")
    local audio =  UnityTools.FindCo(cell.transform,"UISprite","audio")
    -- LogError(msgData.player_icon)
    if msgData.player_icon == nil then 
    -- 没有头像
        hImg.mainTexture = nil
        --[[if defaultImg ~= nil then 
            defaultImg.spriteName = "player_empty"
        end]]
        defaultImg.spriteName = platformMgr.PlayerDefaultHead(msgData.sex)
    else
    -- 设置头像
        hImg.mainTexture = nil
        UnityTools.SetPlayerHead(msgData.player_icon, hImg, platformMgr.PlayerUuid() == msgData.player_uuid)
        defaultImg.spriteName = platformMgr.PlayerDefaultHead(msgData.sex)
    end

    local sContent, type, key = GetShowChatContent(msgData.content)
    local content = UnityTools.FindCo(cell.transform, "UILabel", "content") 
    local icon = UnityTools.FindCo(cell.transform, "UISprite", "icon")
    content.gameObject:SetActive(true)
    icon.gameObject:SetActive(false)
    if msgData.content_type == 1 then
        hImg.gameObject:SetActive(true)
        audio.gameObject:SetActive(false)
        icon.gameObject:SetActive(true)
        icon.spriteName = sContent
        content.text = "        "
    elseif msgData.content_type == 3 then --语音
        hImg.gameObject:SetActive(false)
        audio.gameObject:SetActive(true)
        content.text = sContent
        if platformMgr.PlayerUuid() ~= msgData.player_uuid then
            ComponentData.Get(cell.gameObject).Tag = 30
            UtilTools.LoadAudioFile(msgData.des_player_uuid,cell.gameObject);
        end 
    else
        hImg.gameObject:SetActive(true)
        audio.gameObject:SetActive(false)
        content.text = sContent
    end

    if type == "0" or type == "1" then
        local emojiData = LuaConfigMgr.ChatEmojiConfig[key]
        if emojiData ~= nil then
            UnityTools.PlaySound("Sounds/" .. emojiData.sound, {target = _mainTransfrom.gameObject})
        end
    end
    action.to = UnityEngine.Vector3(-679,action.to.y,action.to.z)
    action:Play(true)
    action.onFinished.Clear()
    
    gTimer.removeTimer(func)
    if msgData.content_type == 3 and platformMgr.PlayerUuid() ~= msgData.player_uuid then
       
        return nil
    else
        local timer = gTimer.registerOnceTimer(5000, func, action)
        gTimer.setRecycler(wName, timer)
    end
end

local function freeChatAction(msgData)
    local cell1Action = function(action) 
        action:Play(false)
        action:SetOnFinished(function() 
            _chatCell1:SetActive(false)
        end)
    end

    local cell2Action = function(action) 
        action:Play(false)
        action:SetOnFinished(function() 
            _chatCell2:SetActive(false)
        end)
    end

    local cell3Action = function(action) 
        action:Play(false)
        action:SetOnFinished(function() 
            _chatCell3:SetActive(false)
        end)
    end
    if _chatCell1.activeSelf ~= true then
        chatCellAction(_chatCell1, cell1Action, msgData)
    elseif _chatCell2.activeSelf ~= true then
        chatCellAction(_chatCell2, cell2Action, msgData)
    elseif _chatCell3.activeSelf ~= true then
        chatCellAction(_chatCell3, cell3Action, msgData)
    else
        chatCellAction(_chatCell1, cell1Action, msgData)
    end
end

local function playMagicEmoji(msgData, parent, key)
    local toIndex = roomMgr.GetIndexByUUID(msgData.des_player_uuid)
    if toIndex == nil or toIndex ==0  then
        toIndex= 1
    end

    local fromPos = nil
    local toPos = nil
    if msgData.player_uuid == platformMgr.PlayerUuid() then
        fromPos = _myPlayerCell.transform.position
    elseif msgData.player_seat_pos == 0 then
        fromPos = _dealerCell.transform.position
    elseif msgData.player_seat_pos == 7 then
        fromPos = _playerListBtn.transform.position
    else
        fromPos = _freeCellList[msgData.player_seat_pos].transform.position
    end
    if msgData.des_player_uuid == platformMgr.PlayerUuid() then
        toPos = _myPlayerCell.transform.position
    elseif toIndex - 1 == 0 then
        toPos = _dealerCell.transform.position
    else
        toPos = _freeCellList[toIndex - 1].transform.position
    end
    -- UnityEngine.Debug.Log(msgData.player_seat_pos .. " -- " .. toIndex )
    AddMagicEmoji(parent, fromPos, toPos, _emojiCell, key)
end

local function playerChatAction(msgData)
    local parent = _TB._pChatL.transform.parent.gameObject
    local tf = parent.transform:FindChild("chatCell" .. msgData.player_seat_pos)
    if tf ~= nil then
        UnityTools.Destroy(tf.gameObject)
    end
    local chatCell = nil
    local sContent, type, key = GetShowChatContent(msgData.content)
    if msgData.content_type == 1 then
        local initPos = nil
        local layer = nil
        if msgData.player_seat_pos == 0 then
            layer = UnityTools.FindTf(_dealerCell.transform, "player_img_bg/img")
        else
            layer = UnityTools.FindTf(_freeCellList[msgData.player_seat_pos].transform, "icon/img")
        end
        initPos = layer.localPosition
        chatCell = UtilTools.AddChild(layer.gameObject, _emojiCell.gameObject, initPos)
        chatCell.name = "chatCell" .. msgData.player_seat_pos
        chatCell:SetActive(true)
        local content = chatCell:GetComponent("UISprite")
        local sAction = chatCell:GetComponent("TweenScale")
        content.spriteName = sContent
        sAction:ResetToBeginning()
        chatCell.transform.localPosition = UnityEngine.Vector3(initPos.x, initPos.y-7, initPos.z)
        local dAction = TweenPosition.Begin(chatCell, 0.6, UnityEngine.Vector3(initPos.x, initPos.y+7, initPos.z), false)
        dAction.style = 2
        sAction:Play(true)
        gTimer.setRecycler(wName, gTimer.registerOnceTimer(4000, function(go) UnityTools.Destroy(go) end, chatCell))
        return nil 
    elseif msgData.content_type == 2 then
        playMagicEmoji(msgData, parent, key)
        -- UnityEngine.Debug.Log("playMagicEmoji")
        return nil

    end
    


    if msgData.player_seat_pos >= 1 and msgData.player_seat_pos <= 3 then 
        local initPos = _freeCellList[msgData.player_seat_pos].transform.localPosition
        initPos = UnityEngine.Vector3(initPos.x + 60, initPos.y + 20, initPos.z)
        chatCell = UtilTools.AddChild(parent, _TB._pChatL, initPos)
        sContent="    "..sContent.."  "
    else
        local initPos = nil
        if msgData.player_seat_pos == 0 then
            initPos = _dealerCell.transform.localPosition
            initPos = UnityEngine.Vector3(initPos.x - 40, initPos.y - 20, initPos.z)
        else
            initPos = _freeCellList[msgData.player_seat_pos].transform.localPosition
            initPos = UnityEngine.Vector3(initPos.x - 40, initPos.y + 20, initPos.z)
        end
        
        chatCell = UtilTools.AddChild(parent, _TB._pChatR, initPos)
        sContent=sContent.."    "
    end
    if type == "0" or type == "1" then
        local emojiData = LuaConfigMgr.ChatEmojiConfig[key]
        if emojiData ~= nil then
            UnityTools.PlaySound("Sounds/" .. emojiData.sound,{target = _mainTransfrom.gameObject})
        end
    end
    chatCell.name = "chatCell" .. msgData.player_seat_pos
    chatCell:SetActive(true)
    local content = UnityTools.FindCo(chatCell.transform, "UILabel", "content")
    local sAction = chatCell:GetComponent("TweenScale")
    local eAction = chatCell:GetComponent("TweenAlpha")
    local audio = UnityTools.FindGo(chatCell.transform,  "audio")
    content.text = sContent
    if msgData.content_type == 3 then -- 语音   
        audio.gameObject:SetActive(true)
        sAction:ResetToBeginning()
        sAction:Play(true)
        ComponentData.Get(chatCell.gameObject).Tag = 31
        UtilTools.LoadAudioFile(msgData.des_player_uuid,chatCell.gameObject);
        return nil
    else
        audio.gameObject:SetActive(false)
        
    end
    sAction:ResetToBeginning()
    eAction:ResetToBeginning()
    sAction:Play(true)
    eAction:Play(true)
    eAction:SetOnFinished(function() UnityTools.Destroy(chatCell) end)
end

function HundredCowMainRecvChatInfo(msgID, msgData)
    if UnityTools.IsWinShow(wName) == false then return nil end
    if msgData.player_seat_pos == 7 and msgData.content_type ~= 2 then
        freeChatAction(msgData)
    else
        playerChatAction(msgData)
    end
end
local function InitTrendData(data)
    local tb={}
    for i=1,4 do
        if data.pool_win ~= nil then
            local isFind=false
            for j=1,#data.pool_win do
                if data.pool_win[j] == i then
                    isFind= true
                    tb[i]=2
                    break
                end
            end     
            if isFind == false then
                if data["win_"..i] == true then
                    tb[i]=1
                else
                    tb[i]=0
                end
            end
        else
            if data["win_"..i] == true then
                tb[i]=1
            else
                tb[i]=0
            end
        end
    end
    return tb
end
function HCRRecvTrendUpdate(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        _trendDataList = {}
        if msgData.list ~= nil then
            for i = 1, #msgData.list, 1 do
                local trendInfo = msgData.list[i]
                _trendDataList[i] = InitTrendData(trendInfo)  --{trendInfo.win_1, trendInfo.win_2, trendInfo.win_3, trendInfo.win_4}
            end
        end
    else
        LogError("Recv nil HCRRecvTrendUpdate")
    end 
end

function HundredCowMainLastTimerCall()
     
    if _timeBar == nil or _timeBar.fillAmount <= 0 then
        -- _stateLayer:SetActive(false)
        UnityTools.SetActive(_stateLayer, false)
        gTimer.removeTimer(HundredCowMainLastTimerCall)
    else
        --_timeBar.fillAmount = _timeBar.fillAmount - (1 / _totalLastTime / 20 * 1.5)
        _timeBar.fillAmount = _timeBar.fillAmount - (0.1 / _totalLastTime )
    end
end

function HundredCowMainStateTimeCall()
    if _timeLb == nil or UnityTools.IsWinShow(wName) == false then
        gTimer.removeTimer("HundredCowMainStateTimeCall")
        return nil 
    end
    _timeLb.text = tostring(_lastTime)
    _lastTime = _lastTime - 1
    if _lastTime <= 0 then
        _lastTime = 0
    end
    
end

local function setTimerClock(content)
    local serverLastTime = roomMgr.LastTime()
    if roomMgr.State() == 20 then
        serverLastTime = serverLastTime - 2
        local t = gTimer.registerOnceTimer(serverLastTime * 1000, function() 
            changeCanBet(false)
        end)
        gTimer.setRecycler(wName, t)
    end
    _totalLastTime = serverLastTime
    _timeBar.fillAmount = serverLastTime / _totalLastTime
    -- _stateLayer:SetActive(true)
    UnityTools.SetActive(_stateLayer, true)
    gTimer.removeTimer(HundredCowMainLastTimerCall)
    gTimer.registerRepeatTimer(100, HundredCowMainLastTimerCall)
    _lastTime = _totalLastTime
    gTimer.removeTimer("HundredCowMainStateTimeCall")
    gTimer.registerDelayTimerEvent(0, 1000, "HundredCowMainStateTimeCall")
    _stateTip.text = content
end

local function updateTaskRed()
    local _itemMgr = IMPORT_MODULE("ItemMgr")
    local taskCompleteNum = _itemMgr.GetTaskComplete()
    _TB.redSp:SetActive(taskCompleteNum > 0)
end

function HundredCowMainRecvEnterResult(msgID, gameData)
    resetPokers(true)
    ResetHundredCowWinRenderQ(_mainTransfrom.gameObject, true)
    updateTaskRed()
    -- 状态设置
    if roomMgr.State() == 20 and roomMgr.bContinued~=1 then
        _TB._myHasBets = false
        doStartEffects(0)
        _waitResultAction = false
        setTimerClock(LuaText.hundred_state_tip_1)
        changeCanBet(true)
    else
        changeCanBet(false)
        if roomMgr.State() == 10 then
            setTimerClock(LuaText.hundred_state_tip_0)
        else
            setTimerClock(LuaText.hundred_state_tip_0)
        end
    end
    -- 奖池设置
    setRewardPool(gameData.reward_pool_num)
    -- LogError(gameData.reward_pool_num)

    -- 查询趋势
    sendCheckTrendMsg()

    -- 查询庄家列表
    sendDealerListMsg()

    -- 下注情况设置
    if gameData.my_set_chips_info ~= nil then
        local tb = {_myBetCnt1, _myBetCnt2, _myBetCnt3, _myBetCnt4}
        _myCanBetMoney = 0
        for i = 1, #gameData.my_set_chips_info, 1 do
            local chip = gameData.my_set_chips_info[i]
            if chip.total_chips > 0 then
                _myCanBetMoney = _myCanBetMoney + chip.total_chips
                _myBetList[chip.pos] = chip.total_chips
                tb[chip.pos].text = UnityTools.GetShortNum2(chip.total_chips)
                tb[chip.pos].gameObject:SetActive(true)
                doBetGoldAction(getGoldCntByChips(chip.total_chips), chip.pos, _playerListBtn)
            else
                tb[chip.pos].gameObject:SetActive(false)
            end
        end
        for i = 1, 4, 1 do
            doBetGoldAction(50, i, _playerListBtn)
        end
    end
end

function HundredCowMainRecvUpdatePlayer(msgID, index, playerInfo)
    -- LogError("Rev .. " .. index)
    -- UnityEngine.Debug.Log("HundredCowMainRecvUpdatePlayer")
    if _waitResultAction == true then
        return nil
    end

    if index == 8 then
        setExPlayerIcon(playerInfo, _myPlayerCell)
        if playerInfo ~= nil then
            checkBetMoney(_betBtnList)
            if playerInfo.gold <= 100 then
                platformMgr.SubsidyOpen(1, function() 

                end, 
                function ()
                
                end)
            end
        end
    elseif index == 1 then
        setExPlayerIcon(playerInfo, _dealerCell)
        if playerInfo ~= nil and playerInfo.player_uuid == platformMgr.PlayerUuid() then
            _dealerSureState = 1
            updateDealerBtn()
        else
            _dealerSureState = 0
            updateDealerBtn()
        end
        checkBetMoney(_betBtnList)
    else

        
        roomMgr.SetPlayerIcon(_taskObj.thisObj,_freeCellList[index-1], playerInfo)
    end
end

-- --------------------------
-- 结算阶段：结算结果处理
-- ---------------------------
local function settlementProcess(msgData)
    -- _resultMask:SetActive(true)
    UnityTools.SetActive(_resultMask, true)
    local timer = gTimer.registerOnceTimer(2000, function() 
        UnityTools.SetActive(_resultMask, false) 
        _waitResultAction = false        
    end)
    gTimer.setRecycler(wName, timer)
    sendCheckFreeMsg(1)
    local resultInfo = msgData.settle_list
    for k, v in pairs(resultInfo) do
        local index = roomMgr.PosToIndex(v.player_pos)
        local reward = v.reward_num
        local cell = nil
        if index == 8 then
            cell = _playerListBtn
            setMoneyWithAction(_myMoneyChangeVal, _myPlayerCell, index)
            _myMoneyChangeVal = 0
        elseif index == 1 then
            cell = _dealerCell
            setMoneyWithAction(reward, cell, index)
        else
            cell = _freeCellList[index - 1]
            setMoneyWithAction(reward, cell, index)
        end
        
    end 
    _TB._myHasBets = false
end

-- ------------------
-- 结算阶段表现
-- ------------------
local function resultAction(msgData)
    local pDataList = {}
    local winFree = {}
    local loseFree = {}
    local tTypeRatio = {}

    for k, v in pairs(msgData.last_card_info) do
        pDataList[v.seat_pos] = {card = v.card_list, type = v.card_type}  -- 1-5的牌面数据, 其中1为庄家牌面
        tTypeRatio[v.seat_pos] = pokerMgr.PokerTypeRatio[v.card_type]   -- 倍数
        
        if v.is_win ~= nil then
            if v.is_win == true then
                table.insert(winFree, v.seat_pos - 1)
            else
                table.insert(loseFree, v.seat_pos - 1)
            end
        end
    end

    local delayTime = doPokerAction() + 400  -- 发牌
    delayTime = doOpenPokerAction(pDataList, delayTime, msgData.last_win_rec) + 250    -- 开牌

    -- delayTime = 1000
    if #loseFree == 0 then
    -- 庄家通赔
        local timer = gTimer.registerOnceTimer(delayTime, function()
            for i=1,4 do
                local myBet = _myBetList[i]
                if myBet == nil or myBet == 0 then
                    UnityTools.SetActive(_taskObj.lights[i],false)
                else
                    UnityTools.SetActive(_taskObj.lights[i],msgData.last_win_rec["win_"..i])
                end
                
            end
            if UnityTools.IsWinShow(wName) == false then return nil end
            delayTime = doGoldDTFAction(winFree, 0, tTypeRatio) + 500     -- 庄家赔闲家
            -- delayTime = 500
            gTimer.registerOnceTimer(delayTime, function() 
                
                delayTime = doGoldFTPAction(winFree, 0, tTypeRatio) + 300  -- 发放赢家金币
                gTimer.registerOnceTimer(delayTime, settlementProcess, msgData)
            end)
        end)
        gTimer.setRecycler(wName, timer)
    elseif #winFree == 0 then
    -- 庄家通吃
        delayTime = delayTime + doGoldFTDAction(loseFree, delayTime) + 500    -- 闲家输庄家钱
        local timer = gTimer.registerOnceTimer(delayTime, function()
            for i=1,4 do
                UnityTools.SetActive(_taskObj.lights[i],false)
            end 
            if UnityTools.IsWinShow(wName) == false then return nil end
            settlementProcess(msgData)
        end)
        gTimer.setRecycler(wName, timer)
    else
        local timer2 = gTimer.registerOnceTimer(delayTime, function()
            for i=1,4 do
                local myBet = _myBetList[i]
                if myBet == nil or myBet == 0 then
                    UnityTools.SetActive(_taskObj.lights[i],false)
                else
                    UnityTools.SetActive(_taskObj.lights[i],msgData.last_win_rec["win_"..i])
                end
                
            end
        end)
        delayTime = delayTime + doGoldFTDAction(loseFree, delayTime) + 500    -- 闲家输庄家钱
        local timer = gTimer.registerOnceTimer(delayTime, function()
           
            delayTime = doGoldDTFAction(winFree, 0, tTypeRatio) + 500     -- 庄家赔闲家
            -- delayTime = 500
            gTimer.registerOnceTimer(delayTime, function() 
                
                if UnityTools.IsWinShow(wName) == false then return nil end
                delayTime = doGoldFTPAction(winFree, 0, tTypeRatio) + 300
                gTimer.registerOnceTimer(delayTime, settlementProcess, msgData)
            end)
        end)
        gTimer.setRecycler(wName, timer)
        gTimer.setRecycler(wName, timer2)
    end
end

local function addTrendData(trendInfo)
    if _trendDataList ~= nil then
        if #_trendDataList == _maxTrendDataCnt then
            local tempTrend = {}
            for i = 2, _maxTrendDataCnt, 1 do
                tempTrend[i - 1] = _trendDataList[i]
            end
            _trendDataList = tempTrend
        end

        _trendDataList[#_trendDataList + 1] = InitTrendData(trendInfo) --{trendInfo.win_1, trendInfo.win_2, trendInfo.win_3, trendInfo.win_4}
    end
end

function HundredCowMainUpdateTaskRed(msgId, type)
    if type == "task" then
        updateTaskRed()
    end
end


-- ------------------------------------------------------------------------
-- #region 房间阶段

-- 休息阶段
function _______restStageHundredCowMain(msgID, msgData)
    -- _taskObj.UpdateHundredCowTaskInfo()        
    sendDealerListMsg()
    _waitResultAction = false
    resetPokers()
    resetGoldInfo()
    resetFreeCell()
    -- print("wait state")
    changeCanBet(false)
    if gTimer.hasTimer(HundredCowMainLastTimerCall) == false or _lastTime == 0 then
        setTimerClock(LuaText.hundred_state_tip_0)
    end
end

-- 下注阶段
function _______bettingStageHundredCowMain(msgID, msgData)
    _TB._myHasBets = false
    local timer = gTimer.registerOnceTimer(100, doStartEffects, msgData.master_continuous)
    gTimer.setRecycler(wName, timer)
    -- doStartEffects(msgData.master_continuous)
    changeCanBet(true)
    setTimerClock(LuaText.hundred_state_tip_1)
end

-- 结算阶段
function _______settlementStageHundredCowMain(msgID, msgData)
    -- if _taskObj._taskBg.transform.localPosition.x>-5000 then
    --     UnityTools.SetActive(_taskObj._taskBg.gameObject,false)
    -- end

    UnityTools.SetActive(_stateLayer, false)
    gTimer.removeTimer(HundredCowMainLastTimerCall)
    
    _waitResultAction = true
    addTrendData(msgData.last_win_rec)
    
    changeCanBet(false)
    roomMgr.SendInGameMsg()
    if msgData.last_card_info ~= nil then
        
        local timer = gTimer.registerOnceTimer(400, function() 
            resultAction(msgData)
           
            local t1 = gTimer.registerOnceTimer(7000, function (num, flag)
                    setRewardPool(num, flag)
                    if _trendLayer.activeSelf then
                        updateTrendLayer()
                    end
                end, 
                msgData.reward_pool_num, 
                true)
            gTimer.setRecycler(wName, t1)
        end)
        gTimer.setRecycler(wName, timer)
    end
end

-- 奖池
function _______jackpotStageHundredCowMain(msgID, msgData)
    roomMgr.SendInGameMsg()
    setRewardPool(msgData.reward_pool_num, true)
    if msgData.pool_reward ~= nil and msgData.pool_reward[1] ~= nil then
        local poolInfo = msgData.pool_reward[1]
        doPoolAction(poolInfo)
    end
end

-- #endregion 房间阶段
-- ------------------------------------------------------------------------

-- --------------------
-- 接收房间状态同步消息
-- --------------------
function HundredCowMainRecvUpdateState(msgID, msgData)
    if roomMgr.State() == 10 then       -- 1.休息阶段
        for i=1,4 do
            UnityTools.SetActive(_taskObj.lights[i],false)
        end
        roomMgr.bContinued = 0 
        if _TB.manager ~= nil then
            _TB.manager:ResetRender()
        end
        -- ____ckearGoldStatistic()
        -- ____ckearResultGoldStatistic()
        _______restStageHundredCowMain(msgID, msgData)
    elseif roomMgr.State() == 20 then   -- 2.下注阶段
        if roomMgr.bContinued == 1 then
            _______restStageHundredCowMain(msgID, msgData)
        else
            _______bettingStageHundredCowMain(msgID, msgData)
        end
    elseif roomMgr.State() == 30 then   -- 3.结算阶段:    30 结算
        if roomMgr.bContinued == 1 then
            _______restStageHundredCowMain(msgID, msgData)
            roomMgr.SendInGameMsg()
        else
            _______settlementStageHundredCowMain(msgID, msgData)
        end
        -- ___logGoldStatistic()
        
    elseif roomMgr.State() == 40 then   -- 4.奖池阶段
        -- ___logResultGoldStatistic()
        if roomMgr.bContinued == 1 then
            _______restStageHundredCowMain(msgID, msgData)
            roomMgr.SendInGameMsg()
        else
            _______jackpotStageHundredCowMain(msgID, msgData)
        end
        
    end
end

function HCRRecvMyGoldChangeUpdate(msgID, msgData)
    _myMoneyChangeVal = 0
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.alter_num ~= nil then
            _myMoneyChangeVal = msgData.alter_num
        end
    else
        LogError("Recv nil HCRRecvMyGoldChangeUpdate")
    end 
end

function HCRRecvPlayerListUpdate(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.type == 1 then
            for i = 1, 8, 1 do
                roomMgr.PlayerExitPosition(i)
            end
            HUCRecvPlayerInfoReply(msgID, {seat_list = msgData.list})
        else
            _playerInfoList = msgData.list
            table.sort(_playerInfoList, function(a, b) 
                -- if a.gold == b.gold then
                --     return a.vip_level > b.vip_level
                -- else
                --     return a.gold > b.gold
                -- end

                if a.vip_level == b.vip_level then
                    return a.gold > b.gold
                else
                    return a.vip_level > b.vip_level
                end
            end)
            _pListTitle.text = LuaText.GetStr(LuaText.hundred_player_list_layer_title, #_playerInfoList)
            updatePlayerListLayer()
        end
        
    else
        LogError("Recv nil HCRRecvPlayerListUpdate")
    end 
end

function HCRRecvDealerListUpdate(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.list ~= nil then
            _dealerPList = msgData.list
            updateDealerPlayerLayer()
            updateDealerBtn()
        end
    else
        LogError("Recv nil HCRRecvDealerListUpdate")
    end 
end

function HCRRecvFreeBetReply(msgID, msgData)
    _TB.betEvtBack = true
    LogError("back"..os.time())
    if UnityTools.CheckMsg(msgID, msgData) then
        local tb = {_myBetCnt1, _myBetCnt2, _myBetCnt3, _myBetCnt4}
        tb[msgData.pos].text = UnityTools.GetShortNum2(msgData.total_chips_num)
        tb[msgData.pos].gameObject:SetActive(true)
        _myBetList[_clickBetType] = msgData.total_chips_num
        -- _myCanBetMoney = msgData.total_chips_num
        _TB._myHasBets = true
        _myCanBetMoney = 0
        for k, v in pairs(_myBetList) do 
            _myCanBetMoney = _myCanBetMoney + v
        end
    else
        LogError("Recv nil HCRRecvFreeBetReply")
    end 
end

function HCRRecvMaxPoolPlayerUpdate(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.list ~= nil and msgData.list[1] ~= nil then
            updatePoolLayer(msgData.list[1])
        else
            updatePoolLayer(nil)
        end
    else
        LogError("Recv nil HCRRecvMaxPoolPlayerUpdate")
    end 
end

function HCRRecvGetDealerReply(msgID, msgData)
    _efcTb._waitServerBack = false
    sendDealerListMsg()
    if UnityTools.CheckMsg(msgID, msgData) then
        if _dealerBtnState == 0 then
            _dealerBtnState = 1
        else
            _dealerBtnState = 0
        end
        updateDealerBtn()
    else
        LogError("Recv nil HCRRecvGetDealerReply")
    end 
end

-- -----------------------
-- 收到闲家下注更新消息
-- -----------------------
function HCRRecvFreeBetUpdate(msgID, msgData)
    if roomMgr.State() ~= 20 then
        return
    end
    if UnityTools.CheckMsg(msgID, msgData) then
        local updList = msgData.upd_list
        for i = 1, #updList, 1 do
            local updInfo = updList[i]
            local lb = _betPosTotalGoldLbl[updInfo.pos]   
            if lb == nil then
                lb = UnityTools.FindCo(_betLayer.transform, "UILabel", "bet" .. updInfo.pos .. "/total")
            end

            if lb ~= nil then
                lb.text = UnityTools.GetShortNum2(updInfo.total_chips)
            end
            -- LogError("chips = " .. updInfo.total_chips)
            _freeCellList[7] = _playerListBtn
            
            if updInfo.seat_pos_list ~= nil then
                for j = 1, #updInfo.seat_pos_list, 1 do
                    local seatInfo = updInfo.seat_pos_list[j]
                    local goldCnt = getGoldCntByChips(seatInfo.set_chips_num)  -- 计算应飞金币数量
                    if _freeBetList[seatInfo.seat_pos] == nil then
                        _freeBetList[seatInfo.seat_pos] = {}
                    end
                    _freeBetList[seatInfo.seat_pos][updInfo.pos] = goldCnt

                    local pCell = _freeCellList[seatInfo.seat_pos]
                    doBetGoldAction(goldCnt, updInfo.pos, pCell)
                    
                    -- ____addGoldStatisticData(updInfo.pos, goldCnt)
                end
            end
        end
        _freeCellList[7] = nil
    else
        LogError("Recv nil HCRRecvFreeBetUpdate")
    end 
end

function ResetHundredCowWinRenderQ(go, goldRender)
    -- if UnityTools.IsWinShow(wName) == false or _upMainTrans == nil then return nil end
    -- _currMainRenderQ = _upMainTrans.startingRenderQueue
    _currMainRenderQ = 2400
    local renderQ = _currMainRenderQ + 22
    if _mrGoldPrefab ~= nil and goldRender ~= nil then
        _mrGoldPrefab.sharedMaterial.renderQueue = renderQ
    end
    
    _upPokerLayer.startingRenderQueue = 0
    _taskObj.zhuang.startingRenderQueue = _currMainRenderQ+200
    _upUILayer.startingRenderQueue = _currMainRenderQ + 400
    _upSVPlayerList.startingRenderQueue = _currMainRenderQ + 410
    _upSVDealerLayer.startingRenderQueue = _currMainRenderQ + 410
    -- LogError(renderQ)
end

-- UI.Controller.UIManager.RegisterLuaWinRenderFunc("HundredCowMain", ResetHundredCowWinRenderQ)
-- UI.Controller.UIManager.RegisterLuaFuncCall("OnApplicationFocus", closeFunc)
-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.CloseWin = closeFunc


-- 返回当前模块
return M
