-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowMainMono.lua
-- * Summary:     普通牛主界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/17/2017 10:07:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowMainMono")



-- 界面名称
local wName = "NormalCowMain"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance()

local roomMgr = IMPORT_MODULE("roomMgr")
local pokerMgr = IMPORT_MODULE("PokerMgr")
local gameMgr = IMPORT_MODULE("GameMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _playerLayer
local _pokerLayer
local _myPokerLayer

local _playerList
local _pokerList
local _myPokerList
local _operLayer
local _stateLayer
local _moneyLb
local _goldLb
local _state_tip
local _dealerbtn1
local _dealerbtn2
local _dealerbtn3
local _dealerbtn4
local _dealerbtn5
local _cowBtn
local _noCowBtn
local _poker_img1
local _poker_img2
local _poker_img3
local _poker_img4
local _poker_img5
local _helpNum1
local _helpNum2
local _helpNum3
local _helpNum4
local _helperTip
local _betBtn1
local _betBtn2
local _betBtn3
local _betBtn4
local _betBtn5
local _menuBtn
local _menuLayer
local _exitBtn
local _minBet
local _pokerCenter
local _flopTimeLb
local _flopTimeBar
local _flopStateLayer
local _goldLayer
local _dealerEffs
local _startEffect
local _normalWin
local _normalLose
local _bothWin
local _bothLose
local _goldPrefab
local _chatBtn
local _pChatR
local _pChatL
local _emojiCell
local _openBoxBtn
local _gameTimesLabel
local _setBtn
local _taskBtn
local _restartLayer
local _restartBtn
local _getGoldBtn
local _btn7Layer
local _helpBtn
local _resultMask
local _taskRedSp
--- [ALD END]





M._helpNum1 = _helpNum1
M._helpNum2 = _helpNum2
M._helpNum3 = _helpNum3
M._helpNum4 = _helpNum4

local _menuState = 0
local _dealerEffTime = 150
local _lastTime = 0
local _stateTip = ""
local _temp_dealer_rate = {}
local _best_poker_type = 0
local _click_poker_list = {}
local _click_poker_cnt = 0
local _best_pokers = {}
local _helperTipStr = nil
local _pokerOrginPos = {}
local _gameTimes = 0
local _cellList = {}
local _mainTransfrom = nil
local _clickClosed = false
local _mainUIPanel = nil
local _goldPreMeshRender = nil
local _pokerObjTable = {}
local _compsTable = {}
local _operObjTb = {}
setmetatable(_compsTable, { __mode = "k" })
local _playerLabelTb = {}
setmetatable(_playerLabelTb, { __mode = "k" })
local _manager = nil
local _winTb = {ui = nil, top = nil, titleLb = nil, redpackTimer = nil, redHelpLayer = nil, redpackEff = nil, redPro_1 = nil, redPro_2 = nil, rebProLb_1 = nil, redProLb_2 = nil, redGou_1 = nil, redGou_2 = nil, redTimeSp = nil, redTimePro = nil, redMyNeedTime = 0, redComeTime = 0, redCondition = 0, redTitleLayer = nil}
local Red_MyTime = 112
local Red_Coming = 120

local function OnLoadEffComplete(gameObject)
    UtilTools.SetEffectRenderQueueByUIParent(_mainTransfrom,gameObject.EffectGameObj.transform,20)
    gameObject.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,0,0)
    gameObject.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
end

local function addGameTimes()
    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽房间对局内宝箱任务奖励")
        return
    end

    -- _gameTimes = _gameTimes + 1
    if roomMgr.RoomType() == 10 then return nil end
    local boxConfig = LuaConfigMgr.ChestConfig[tostring(roomMgr.RoomType())]
    local cnt = tonumber(boxConfig.condition) - _gameTimes
    local action = _openBoxBtn:GetComponent("TweenRotation")
    if cnt > 0 then
        _gameTimesLabel.text = LuaText.GetStr(LuaText.cow_game_times_tip, cnt)
        _gameTimesLabel.gameObject:SetActive(true)
        _openBoxBtn.transform.localRotation = UnityEngine.Quaternion.Euler(0 , 0, 0)
        action.enabled = false
    else
        _gameTimesLabel.gameObject:SetActive(false)
        action.enabled = true
    end
end

local function chooseDealerBtnCall(gameObject)
    local nameIndex = tonumber(string.sub(gameObject.name, 4, 4)) - 1
    roomMgr.SendDealerRateMsg(nameIndex)
    M.showOperate(4)
end

local function submitPokerBtnCall(gameObject)
    if gameObject == nil or (_best_poker_type > 0 and gameObject.name == "btn1") or (_best_poker_type == 0 and gameObject.name == "btn2") then
        roomMgr.SendSubmitMsg()
        M.showOperate(4)
        UnityTools.SetActive(_myPokerLayer, false)
        -- local poker = _pokerList[1]
        local poker = _pokerObjTable[1]
        UnityTools.SetActive(poker.parent, true)
        -- poker:SetActive(true)
        for j = 1, 5, 1 do
            local pokerInfo = _best_pokers[j - 1]
            local pImg = poker[j] --UnityTools.FindGo(poker.transform, "poker_img" .. tostring(j))

            pokerMgr.SetPokerIcon(pImg, {color = 5 - tonumber(pokerInfo.Type), number = pokerInfo.Num})
        end
        _best_pokers = nil
    end
end

local function resetHelperNum()
    _helpNum1.text = ""
    _helpNum2.text = ""
    _helpNum3.text = ""
    _helpNum4.text = ""
    _helperTipStr = LuaText.GetStr(LuaText.cow_helper_tip, LuaText["cow_type_" .. tostring(_best_poker_type)])
    _helperTip.text = _helperTipStr
end

local function getClickPokerBag()
    local pokerBag = {}
    local cIndex, nIndex = 0, 3
    local myPokerbag = pokerMgr.GetPokerBag(1)
    local pokerList = myPokerbag:FinalPokers()
    for k, v in pairs(_myPokerList) do
        if v.transform.localPosition.y == 0 then
            pokerBag[nIndex] = pokerList[tonumber(k) - 1]
            nIndex = nIndex + 1
        else
            pokerBag[cIndex] = pokerList[tonumber(k) - 1]
            cIndex = cIndex + 1
        end
    end
    pokerBag.Count = 5
    return pokerBag
end

local function setHelperNum()
    local myPokerbag = pokerMgr.GetPokerBag(1)
    local pokerList = myPokerbag:FinalPokers()
    local numIndex = 1
    local cardlist = {}
    -- local isSelfSelect = false
    -- for i=1,5 do
    --     cardlist[i] = {}
    --     cardlist[i].number = pokerList[i-1].Num
    --     -- cardlist[i].
    --     LogError("i="..i..",num="..pokerList[i-1].Num)
    -- end
    -- local cardlist,isSpecial = pokerMgr.SortMaxNiuNiu(cardlist)
    -- LogError("ssss")
    resetHelperNum()
    local helperNums = {_helpNum1, _helpNum2, _helpNum3, _helpNum4}
    local bothSelect = true
    for i = 1, 3, 1 do
        local name = _click_poker_list[i]
        if name ~= nil then
            isSelfSelect = true
            local index = tonumber(string.sub(name, 10, 10))
            local num = pokerList[index - 1].Num
            if num > 10 then
                num = 10
            end
            helperNums[i].text = tostring(num)
        else
            helperNums[i].text = ""
            bothSelect = false
        end
    end
    -- if isSelfSelect == false then
    --     for i = 1, 3, 1 do
    --         local num = cardlist[i].number
    --         if num > 10 then
    --             num = 10
    --         end
    --         helperNums[i].text = tostring(num)
    --     end
    --     bothSelect = true
    -- end
    if bothSelect == true then
        _helpNum4.text = tostring(tonumber(_helpNum1.text) + tonumber(_helpNum2.text) + tonumber(_helpNum3.text))
        -- 计算牌型
        -- local hType, hPoker = pokerMgr.CalculatePokers(getClickPokerBag())
        _helperTipStr = LuaText.GetStr(LuaText.cow_helper_tip, LuaText["cow_type_" .. tostring(_best_poker_type)])
        _helperTip.text = _helperTipStr
    else
        
    end
end

local function saveClickPoker(name)
    for i = 1, 3, 1 do 
        if _click_poker_list[i] == nil then
            _click_poker_list[i] = name
            _click_poker_cnt = _click_poker_cnt + 1
            break
        end
    end
end

local function removeClickPoker(name)
    for i = 1, 3, 1 do 
        if _click_poker_list[i] == name then
            _click_poker_list[i] = nil
            _click_poker_cnt = _click_poker_cnt - 1
            break
        end
    end
end

-- ------------------
-- 金币飞行动作表现
-- ------------------
local function goldAction(toIndex, sendPoss, givePoss, wDelay)
    if _playerList == nil or _goldLayer == nil then return end
    -- _goldLayer:SetActive(true)
    local delay = wDelay
    local factory = 45
    local toPos = _cellList[toIndex].transform.localPosition
    local maxTime = 0
    local goldCnt = 80
    if roomMgr.RoomType() == 10 then
        goldCnt = 5
    end

    -- 卡牌动作参数
    local tActionParams = {}
    for k = 1, 5, 1 do
        -- local sDelay = 0
        delay = wDelay + ((k - 1) * 30)
        if k ~= toIndex and givePoss[k] ~= nil then
            local orginPos = _cellList[k].transform.localPosition
            for i = 1, goldCnt, 1 do 
                
                local gold = UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
                
                -- ------------------------- v1.0 begin ----------------------------
                -- gold:SetStartPos(orginPos.x + math.random(-factory, factory), orginPos.y + math.random(-factory, factory), orginPos.z)
                -- gold:SetEndPos(toPos.x, toPos.y, toPos.z)
                -- gold:SetParams(650, delay + math.random(-10, 250), false, false)
                -- _manager:Begin(gold)
                -- ------------------------- v1.0 end ----------------------------

                -- --------- v1.1  begin -------------------------------- 
                -- Add by WP.Chu
                local act = {}
                table.insert(act, gold)                                         -- goTarget
                table.insert(act, 9999)                                         -- renderQ
                table.insert(act, orginPos.x + math.random(-factory, factory))  -- startX
                table.insert(act, orginPos.y + math.random(-factory, factory))  -- startY
                table.insert(act, orginPos.z)                                   -- startZ
                table.insert(act, toPos.x)                                      -- endX
                table.insert(act, toPos.y)                                      -- endY
                table.insert(act, toPos.z)                                      -- endZ
                table.insert(act, 650)                                          -- duration
                table.insert(act, delay + math.random(-10, 250))                -- delay
                table.insert(act, false)                                        -- world
                table.insert(act, false)                                        -- noEvt

                table.insert(tActionParams, act)
                -- --------- v1.1  end -------------------------------- 

                -- local creator = function ()
                --     return UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
                -- end

                -- local fromPos = UnityEngine.Vector3(orginPos.x + math.random(-factory, factory), orginPos.y + math.random(-factory, factory))

                -- UnityTools.ActionPos(nil, 350, {creator = creator, from = fromPos, to = toPos, world = false}, function (obj) 
                --     UnityTools.ReleasePoolObj(_goldPrefab, obj)
                -- end, delay + math.random(-10, 250), wName)
                
                local tempT = 300 + delay
                if tempT > maxTime then
                    maxTime = tempT
                end
                -- delay = delay + math.random(10, 70)
            end
            -- delay = delay - 540
        end
    end

    -- 动画表现 v1.1
    local actionCount = #tActionParams
    if actionCount > 0 then
        _manager:groupActionBegin(tActionParams, actionCount)
        tActionParams = {}
    end

    maxTime = maxTime + 500
    wDelay = delay
    UnityTools.PlaySound("Sounds/getGold", {delay = delay * 0.5 / 1000, target = _mainTransfrom.gameObject})
    local tt = gTimer.registerOnceTimer(maxTime, function () 
        if _playerList == nil then return nil end
        maxTime = 0
        local orginPos = _cellList[toIndex].transform.localPosition
        for k, v in pairs(sendPoss) do
            for i = 1, goldCnt, 1 do 
                local gold = UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
                local endPos = _playerList[k].transform.localPosition
                
                -- ------------------------- v1.0 begin ----------------------------
                -- gold:SetStartPos(orginPos.x + math.random(-factory, factory), orginPos.y + math.random(-factory, factory), orginPos.z)
                -- gold:SetEndPos(endPos.x, endPos.y, endPos.z)
                -- gold:SetParams(650, delay + math.random(-10, 200), false, false)
                -- _manager:Begin(gold)
                -- --------- v1.0  end -------------------------------- 

                -- --------- v1.1  begin -------------------------------- 
                -- Add by WP.Chu
                local act = {}
                table.insert(act, gold)                                         -- goTarget
                table.insert(act, 9999)                                         -- renderQ
                table.insert(act, orginPos.x + math.random(-factory, factory))  -- startX
                table.insert(act, orginPos.y + math.random(-factory, factory))  -- startY
                table.insert(act, orginPos.z)                                   -- startZ
                table.insert(act, endPos.x)                                      -- endX
                table.insert(act, endPos.y)                                      -- endY
                table.insert(act, endPos.z)                                      -- endZ
                table.insert(act, 650)                                          -- duration
                table.insert(act, delay + math.random(-10, 200))                -- delay
                table.insert(act, false)                                        -- world
                table.insert(act, false)                                        -- noEvt

                table.insert(tActionParams, act)
                -- --------- v1.1  end -------------------------------- 

                -- local creator = function ()
                --     return UnityTools.GetPoolObj(_goldPrefab, _goldLayer)
                -- end
                -- local fromPos = UnityEngine.Vector3(orginPos.x + math.random(-factory, factory), orginPos.y + math.random(-factory, factory))
                -- local endPos = _playerList[k].transform.localPosition

                -- UnityTools.ActionPos(nil, 300, {creator = creator, from = fromPos, to = endPos, world = false}, function (obj) 
                --     UnityTools.ReleasePoolObj(_goldPrefab, obj)
                -- end, delay + math.random(-10, 200), wName)
                
                local tempT = 300 + delay
                if tempT > maxTime then
                    maxTime = tempT
                end
            end
            delay = wDelay + ((k - 1) * 30)
        end

        -- 动画表现 v1.1
        actionCount = #tActionParams
        if actionCount > 0 then
            _manager:groupActionBegin(tActionParams, actionCount)
            tActionParams = nil
        end

        UnityTools.PlaySound("Sounds/getGold", {delay = delay * 0.5 / 1000, target = _mainTransfrom.gameObject})

    end)
    gTimer.setRecycler(wName, tt)
end

local function clickMyPokerCall(gameObject)
    if roomMgr.State() == roomMgr.TState.Flop then
        local pX = gameObject.transform.localPosition.x
        if gameObject.transform.localPosition.y < 10 then
            if _click_poker_cnt < 3 then
                gameObject.transform.localPosition = UnityEngine.Vector3(pX, 25, 0)
                saveClickPoker(gameObject.name)
            else
                return nil
            end
        else
            gameObject.transform.localPosition = UnityEngine.Vector3(pX, 0, 0)
            removeClickPoker(gameObject.name)
        end
        setHelperNum()
    end
end

local function clickBetBtnCall(gameObject)
    local nameIndex = tonumber(string.sub(gameObject.name, 4, 4))
    roomMgr.SendRateMsg(nameIndex * 5)
    M.showOperate(4)
end

local _goldPools = {}
local barSide = -1
local function clickMenuBtnCall(gameObject)
    
    if _menuState == 0 then
        _menuLayer:SetActive(true)
        _menuState = 1
    else
        _menuLayer:SetActive(false)
        _menuState = 0
    end
end

local function doResultEffects(isWin, isBoth)
    if _clickClosed == true then return nil end
    if isWin == true then 
        
        local ctrl = IMPORT_MODULE("ResultLayerController")
        ctrl.IsBoth = isBoth
        ctrl = nil
        UnityTools.CreateLuaWin("ResultLayer")
    else
        
        local ctrl = IMPORT_MODULE("ResultLoseLayerController")
        ctrl.IsBoth = isBoth
        ctrl = nil
        UnityTools.CreateLuaWin("ResultLoseLayer")
    end

    -- local ct = resultLayer:GetComponent("TweenScale")
    -- local bg = UnityTools.FindCo(resultLayer.transform, "TweenScale", "bg")
    -- local sp = UnityTools.FindCo(resultLayer.transform, "TweenScale", "sp")
    
    -- UnityTools.SetActive(_resultMask, true)
    -- ct:SetOnFinished(function (ob) 
    
    --     UnityTools.SetActive(resultLayer, false)
    --     UnityTools.SetActive(_resultMask, false)
    
    -- end)
    -- UnityTools.SetActive(resultLayer, true)
    
    -- ct:ResetToBeginning()
    -- bg:ResetToBeginning()
    -- sp:ResetToBeginning()
    -- ct:Play(true)
    -- bg:Play(true)
    -- sp:Play(true)

end

local function closeFunc(flag)
    roomMgr.SendLeaveRoomMsg()
    platformMgr.gameMgr.closeActiveFun = function()
        -- UnityTools.DestroyWin("NormalCowTop")
        -- UnityTools.DestroyWin("NormalCowUI")
        -- UnityTools.DestroyWin("NormalCowRedUI")
        -- UnityTools.DestroyWin(wName)
        UI.Controller.UIManager.RemoveAllWinExpect({"Waiting","MainCenterWin","MainWin"})
    end
    _clickClosed = true
    UnityTools.DestroyWin("ResultLayerMono")
    UnityTools.DestroyWin("ResultLoseLayerMono")
    UnityTools.ReturnToMainCity()
end
function NormalCowCloseFunc()
    platformMgr.gameMgr.closeActiveFun = function()
        -- UnityTools.DestroyWin("NormalCowTop")
        -- UnityTools.DestroyWin("NormalCowUI")
        -- UnityTools.DestroyWin("NormalCowRedUI")
        -- UnityTools.DestroyWin(wName)
        roomMgr.SendLeaveRoomMsg()
        UI.Controller.UIManager.RemoveAllWinExpect({"Waiting","MainCenterWin","MainWin"})
    end
    _clickClosed = true
    UnityTools.DestroyWin("ResultLayerMono")
    UnityTools.DestroyWin("ResultLoseLayerMono")
    -- UnityTools.DestroyWin(wName)
end
local function exitBtnCall(gameObject)
    if _restartLayer.activeSelf == false then
        if roomMgr.RoomType() == 10 then
            if roomMgr.State() > 0 then
                UnityTools.MessageDialog("退出牌局将有可能导致累计局数清空，确认退出？",{okCall=function(f) 
                    closeFunc()
                end})
                return nil
            end
        else
            if roomMgr.State() > 11 and roomMgr.State() < 50 then
                UnityTools.MessageDialog("中途退出将由系统为您自动打完",{okCall=function(f) 
                    closeFunc()
                end})
                return nil
            end
        end
    end
    closeFunc()
end

local function clickChatBtn(gameObject)
    platformMgr.OpenChatWin()
    -- MemorySampleLog(true)
end

local function openBoxBtnCall(gameObject)
    platformMgr.OpenBoxWin(1,_gameTimes)
end

local function ClickSetBtnCall(gameObject)
    platformMgr.OpenSetWin()
end

local function ClickTaskBtnCall(gameObject)
    if roomMgr.RoomType() == 10 then
        _winTb.redHelpLayer:SetActive(true)
        -- NormalCowMainRecvRedNoticeUpdate(0, {close_draw_second = 0})
    else
        platformMgr.OpenTaskByGame()
    end
end

local function ClickRestartCall(gameObject)
    -- UnityTools.ShowMessage("金币不足，请前往商城充值")
    if roomMgr.RoomType() == 10 then
        if platformMgr.GetDiamond() >= 18 then
            _winTb.redTitleLayer:SetActive(true)
            _getGoldBtn:SetActive(true)
            roomMgr.SendJoinRoomMsg()
            _restartLayer:SetActive(false)
        else
            platformMgr.OpenRedConditionWin()
            -- local shopCtrl = IMPORT_MODULE("ShopWinController");
            -- if shopCtrl ~= nil then
            --     UnityTools.ShowMessage(LuaText.noEnough102);
            --     shopCtrl.OpenShop(2)
            -- end
        end
    else
        local mix, max = roomMgr.GetLimitGold()
        if platformMgr.GetGod() >= mix then
            if max == 0 or platformMgr.GetGod() < max then
                roomMgr.SendJoinRoomMsg()
                _restartLayer:SetActive(false)
            else
                UnityTools.ShowMessage("金币超过房间限制，请退出房间前往其他场")
            end
        else
            platformMgr.OpenShopWin()
        end
    end
end

local function ClickGetGoldBtn(gameObject)
    if roomMgr.RoomType() == 10 then
        if _winTb.redpackEff.activeSelf == true then
            -- roomMgr.sendMsg(protoIdSet.cs_redpack_room_draw_req, {})
            UnityTools.CreateLuaWin("OpenRedpackWin")
            -- gTimer.removeTimer(_winTb.redpackTimer)
            _getGoldBtn:GetComponent("TweenScale").enabled = false
--            UtilTools.SetGray(_getGoldBtn, false, true)
            -- _winTb.redTimePro:SetActive(true)
            _winTb.redpackEff:SetActive(false)
            _gameTimesLabel.gameObject:SetActive(false)
        else
            UnityTools.ShowMessage("连续完成5局游戏，才可领取红包！")
        end
    else
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
end

local function ClickHelpLayer(gameObject)
    _btn7Layer:SetActive(true)
    -- doResultEffects(false, true)
    -- roomMgr.SetPlayerIcon(_playerList[1], roomMgr.GetPlayerInfo(1))
end

local function CloseHelpLayer(gameObject)
    _btn7Layer:SetActive(false)
end

--- [ALF END]













-- --------------------------------------
-- 获取指定对象的孩子列表
-- 格式： key .. index

-- key 孩子节点名字前缀
-- startIndex 结束索引
-- endIndex 开始索引
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


-- local function getChilds(parent, key, cnt, index)
--     local list = {}
--     index = index or 1
--     for i = index, cnt, 1 do 
--         local child = UnityTools.FindGo(parent.transform, key .. tostring(i))
--         list[i] = child
--     end
--     return list
-- end

local function GetBcAndSp(obj)
    _compsTable[obj] = {}
    _compsTable[obj].BC = obj:GetComponent("BoxCollider")
    _compsTable[obj].SP = obj:GetComponent("UISprite")
end

local function resetPlayer()
    for j = 1, 5, 1 do
        local playerInfo = roomMgr.GetPlayerInfo(j)
        local player = _playerList[j]
        roomMgr.SetPlayerIcon(_winTb.thisObj,player, playerInfo)
        local board = UnityTools.FindCo(player.transform, "UISprite", "player_img_bg/board")
        board.spriteName = ""

        UnityTools.SetActive(_playerLabelTb[player].dealer, false)
        UnityTools.SetActive(_playerLabelTb[player].get, false)
        UnityTools.SetActive(_playerLabelTb[player].cost, false)
        UnityTools.SetActive(_playerLabelTb[player].bet, false)
    end
end

local function resetPokers()
    for k, poker in pairs(_pokerObjTable) do
        for j = 1, 5, 1 do
            local p = poker[j] --UnityTools.FindGo(poker.transform, "poker_img" .. j)
            p.o.transform.localPosition = _pokerOrginPos[k][j]
            pokerMgr.SetPokerIcon(p, nil)
        end
        if poker.ptsp ~= nil then
            UnityTools.SetActive(poker.ptsp, false)
        end
        -- UnityTools.FindGo(poker.transform, "pokerType"):SetActive(false)
    end
end

local function initPokerOrginPos()
    -- _pokerOrginPos[0] = {}
    -- for k, poker in pairs(_myPokerList) do
    --      _pokerOrginPos[0][k] = poker.transform.localPosition
    -- end
    for k, poker in pairs(_pokerObjTable) do
        _pokerOrginPos[k] = {}
        for j = 1, 5, 1 do
            -- local p = UnityTools.FindGo(poker.transform, "poker_img" .. j)
            local p = poker[j]
            _pokerOrginPos[k][j] = p.o.transform.localPosition
        end
    end
end

local function updateTaskRed()
    if _taskRedSp ~= nil then
        local _itemMgr = IMPORT_MODULE("ItemMgr")
        local taskCompleteNum = _itemMgr.GetTaskComplete()
        _taskRedSp:SetActive(taskCompleteNum > 0)
    end
    if _winTb.taskRed~= nil then
        local _itemMgr = IMPORT_MODULE("ItemMgr")
        local taskCompleteNum = _itemMgr.GetTaskComplete()
        _winTb.taskRed.gameObject:SetActive(taskCompleteNum > 0)
    end
end

local function initBoard()
    
    -- _winTb.top:SetActive(true)
    _cellList = getChilds(_goldLayer, "cell", 5)
    _playerList = getChilds(_playerLayer, "playerCell", 5)
    _pokerList = getChilds(_pokerLayer, "pokerCell", 5)
    _myPokerList = getChilds(_myPokerLayer, "poker_img", 5)
    

    if roomMgr.RoomType() == 10 then
        local temp = _pokerList[3]
        _pokerList[3] = _pokerList[5]
        _pokerList[5] = temp
        temp = nil
    end

    pokerMgr.GetPokersTable(_pokerObjTable, _pokerList)
    _pokerObjTable[0] = pokerMgr.GetPokerObjectTable(_myPokerLayer)

    _minBet.text = LuaText.Format("normal_cow_desc1",roomMgr.MinBet()) 

    initPokerOrginPos()

    for k, player in pairs(_playerList) do
        roomMgr.SetPlayerIcon(_winTb.thisObj,player, nil)
        _playerLabelTb[player] = {}
        _playerLabelTb[player].dealer = UnityTools.FindGo(player.transform, "dealer")
        _playerLabelTb[player].get = UnityTools.FindCo(player.transform, "UILabel","get")
        _playerLabelTb[player].cost = UnityTools.FindCo(player.transform, "UILabel","cost")
        _playerLabelTb[player].bet = UnityTools.FindCo(player.transform, "UISprite", "bet")
        UnityTools.SetActive(_playerLabelTb[player].dealer, false)
        UnityTools.SetActive(_playerLabelTb[player].get, false)
        UnityTools.SetActive(_playerLabelTb[player].cost, false)
        UnityTools.SetActive(_playerLabelTb[player].bet, false)
    end

    UnityTools.SetActive(_resultMask, false)
    UnityTools.SetActive(_operObjTb.dealer, false)
    UnityTools.SetActive(_operObjTb.bets, false)
    UnityTools.SetActive(_operObjTb.cow, false)
    UnityTools.SetActive(_operLayer, false)
    UnityTools.SetActive(_flopStateLayer, false)
    UnityTools.SetActive(_pokerLayer, false)
    UnityTools.SetActive(_myPokerLayer, false)
    UnityTools.SetActive(_startEffect, false)
    -- _operLayer:SetActive(false)
    _stateLayer:SetActive(true)

    -- _moneyLb.text = UnityTools.GetShortNum(platformMgr.GetGod())
    -- _goldLb.text = UnityTools.GetShortNum(platformMgr.GetDiamond())

    
    resetPokers()
    updateTaskRed()

end

function NormalCowRedUIWinBind(evtID, gameObject)
    _winTb.thisObj = gameObject
    NormalCowUIWinBind(evtID, gameObject)
   
    if  _minBet ~= nil then
        UnityTools.SetActive(_minBet.gameObject,false)
    end
    _winTb.titleLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/titleLayer/title")
    _winTb.redHelpLayer = UnityTools.FindGo(gameObject.transform, "Container/bg/btn5Layer")
    UnityTools.AddOnClick(UnityTools.FindGo(gameObject.transform, "Container/bg/btn5Layer/Container"), function() 
        _winTb.redHelpLayer:SetActive(false)
    end)
    _winTb.taskBtn = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/bg/topLayer/btn8")
    UnityTools.AddOnClick(_winTb.taskBtn.gameObject, function() 
        local ctrl = IMPORT_MODULE("TaskWinController");
        if ctrl ~= nil then
            ctrl.TabIndex = 5
            ctrl.OpenByGame();
        end
    end)
    _winTb.taskRed = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/bg/topLayer/btn8/red")
    _winTb.redpackEff = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn6/Sprite")
    _winTb.redPro_1 = UnityTools.FindCo(gameObject.transform, "UISlider", "Container/bg/titleLayer/pro1")
    _winTb.redPro_2 = UnityTools.FindCo(gameObject.transform, "UISlider", "Container/bg/titleLayer/pro2")
    _winTb.redProLb_1 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/topLayer/btn6/left")
    _winTb.redProLb_2 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/topLayer/btn6/left/Label")
    _winTb.redGou_1 = UnityTools.FindGo(gameObject.transform, "Container/bg/titleLayer/pro1/Sprite")
    _winTb.redGou_2 = UnityTools.FindGo(gameObject.transform, "Container/bg/titleLayer/pro2/Sprite")
    _winTb.redTitleLayer = UnityTools.FindGo(gameObject.transform, "Container/bg/titleLayer")
    _winTb.redTimeSp = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/bg/topLayer/btn6/timeBar/bar")
    _winTb.redTimePro = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn6/timeBar")
   
--    UtilTools.SetGray(_getGoldBtn, false, true)
end

function NormalCowUIWinBind(evtID, gameObject)
    _winTb.thisObj = gameObject
    _winTb.ui = gameObject
    _mainUIPanel = gameObject:GetComponent("UIPanel")
    _playerLayer = UnityTools.FindGo(gameObject.transform, "Container/playerLayer")
    _operLayer = UnityTools.FindGo(gameObject.transform, "Container/operLayer")
    _stateLayer = UnityTools.FindGo(gameObject.transform, "Container/stateLayer")
    _goldLayer = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/goldLayer")
    -- _moneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/platformInfo/money/label")
    -- _goldLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/platformInfo/gold/label")

    _dealerEffs = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/dealerEffs")

    _state_tip = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/stateLayer/tip")
    _flopTimeLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/stateLayer/timeBar/timeLb")
    _flopTimeBar = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/stateLayer/timeBar/bar")
    _flopStateLayer = UnityTools.FindGo(gameObject.transform, "Container/stateLayer/timeBar")

    _dealerbtn1 = UnityTools.FindCo(gameObject.transform, "BoxCollider", "Container/operLayer/dealers/btn1")
    UnityTools.AddOnClick(_dealerbtn1.gameObject, chooseDealerBtnCall)
    GetBcAndSp(_dealerbtn1)
    _dealerbtn2 = UnityTools.FindCo(gameObject.transform, "BoxCollider", "Container/operLayer/dealers/btn2")
    UnityTools.AddOnClick(_dealerbtn2.gameObject, chooseDealerBtnCall)
    GetBcAndSp(_dealerbtn2)
    _dealerbtn3 = UnityTools.FindCo(gameObject.transform, "BoxCollider", "Container/operLayer/dealers/btn3")
    UnityTools.AddOnClick(_dealerbtn3.gameObject, chooseDealerBtnCall)
    GetBcAndSp(_dealerbtn3)
    _dealerbtn4 = UnityTools.FindCo(gameObject.transform, "BoxCollider", "Container/operLayer/dealers/btn4")
    UnityTools.AddOnClick(_dealerbtn4.gameObject, chooseDealerBtnCall)
    GetBcAndSp(_dealerbtn4)
    _dealerbtn5 = UnityTools.FindCo(gameObject.transform, "BoxCollider", "Container/operLayer/dealers/btn5")
    UnityTools.AddOnClick(_dealerbtn5.gameObject, chooseDealerBtnCall)
    GetBcAndSp(_dealerbtn5)

    _cowBtn = UnityTools.FindGo(gameObject.transform, "Container/operLayer/cow/btn1")
    UnityTools.AddOnClick(_cowBtn.gameObject, submitPokerBtnCall)

    _noCowBtn = UnityTools.FindGo(gameObject.transform, "Container/operLayer/cow/btn2")
    UnityTools.AddOnClick(_noCowBtn.gameObject, submitPokerBtnCall)

    _betBtn1 = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn1")
    UnityTools.AddOnClick(_betBtn1.gameObject, clickBetBtnCall)
    GetBcAndSp(_betBtn1)

    _betBtn2 = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn2")
    UnityTools.AddOnClick(_betBtn2.gameObject, clickBetBtnCall)
    GetBcAndSp(_betBtn2)

    _betBtn3 = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn3")
    UnityTools.AddOnClick(_betBtn3.gameObject, clickBetBtnCall)
    GetBcAndSp(_betBtn3)
    
    _betBtn4 = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn4")
    UnityTools.AddOnClick(_betBtn4.gameObject, clickBetBtnCall)
    GetBcAndSp(_betBtn4)

    _betBtn5 = UnityTools.FindGo(gameObject.transform, "Container/operLayer/bets/btn5")
    UnityTools.AddOnClick(_betBtn5.gameObject, clickBetBtnCall)
    GetBcAndSp(_betBtn5)
 if version.VersionData.IsReviewingVersion() then
        _betBtn5.gameObject:SetActive(false)
    end
    _helpNum1 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/operLayer/cow/cowHelper/num1")

    _helpNum2 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/operLayer/cow/cowHelper/num2")

    _helpNum3 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/operLayer/cow/cowHelper/num3")

    _helpNum4 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/operLayer/cow/cowHelper/equal")

    _helperTip = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/operLayer/cow/cowHelper/tip")

    _menuBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1")
    UnityTools.AddOnClick(_menuBtn.gameObject, clickMenuBtnCall)

    _menuLayer = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer")

    _exitBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer/btn2")
    UnityTools.AddOnClick(_exitBtn.gameObject, exitBtnCall)

    _chatBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/leftLayer/btn1")
    UnityTools.AddOnClick(_chatBtn.gameObject, clickChatBtn)

    _goldPrefab = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/goldLayer/goldPrefab")
    _goldPreMeshRender = _goldPrefab:GetComponent("MeshRenderer")

    _operObjTb.dealer = UnityTools.FindGo(_operLayer.transform, "dealers")
    _operObjTb.bets = UnityTools.FindGo(_operLayer.transform, "bets")
    _operObjTb.cow = UnityTools.FindGo(_operLayer.transform, "cow")
    
    _openBoxBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn4")
    --_openBoxBtn.gameObject:SetActive(false)
    UnityTools.AddOnClick(_openBoxBtn.gameObject, openBoxBtnCall)

    _gameTimesLabel = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/topLayer/gameTimes")
    --_gameTimesLabel.gameObject:SetActive(false)
    _setBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn1Layer/btn3")
    UnityTools.AddOnClick(_setBtn.gameObject, ClickSetBtnCall)

    _taskBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn5")
    UnityTools.AddOnClick(_taskBtn.gameObject, ClickTaskBtnCall)
 if version.VersionData.IsReviewingVersion() then
        _taskBtn.gameObject:SetActive(false)
    end
    _taskRedSp = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn5/red") 

    _getGoldBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn6")
    UnityTools.AddOnClick(_getGoldBtn.gameObject, ClickGetGoldBtn)

    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        _getGoldBtn:SetActive(false)
        _gameTimesLabel.gameObject:SetActive(false)
        _openBoxBtn:SetActive(false)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽房间对局内获取金币按钮和宝箱按钮")
    end

    _btn7Layer = UnityTools.FindGo(gameObject.transform, "Container/bg/btn7Layer")
    UnityTools.AddOnClick(UnityTools.FindGo(gameObject.transform, "Container/bg/btn7Layer/Container"), CloseHelpLayer)

    _helpBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn7")
    UnityTools.AddOnClick(_helpBtn, ClickHelpLayer)

    UnityTools.CreatePool(_goldPrefab, _goldLayer, 1000, false, 0, UnityEngine.Vector3(-1000, -1000, 0))

    local timer = gTimer.registerOnceTimer(100, function() 
        UnityTools.CreateLuaWin("NormalCowTop")
        
        if roomMgr.RoomType() == 10 and _myPokerLayer ~= nil and _cowBtn ~= nil then
            local isAct = _myPokerLayer.transform.localPosition.x >=2000
            UnityTools.SetActive(_myPokerLayer, true) 
            _myPokerLayer.transform.localPosition = UnityEngine.Vector3(_myPokerLayer.transform.localPosition.x,_cowBtn.transform.localPosition.y-60,_myPokerLayer.transform.localPosition.z)
            UnityTools.SetActive(_myPokerLayer, isAct)
        end
    end)
    gTimer.setRecycler(wName, timer)

end

local function mainWinBind(gameObject)
    
    _winTb.pokerBg = UnityTools.FindGo(gameObject.transform, "Container/bg"):GetComponent("UITexture") 
    _pokerLayer = UnityTools.FindGo(gameObject.transform, "Container/pokerLayer")
    _pokerLayer:SetActive(false)
    _myPokerLayer = UnityTools.FindGo(gameObject.transform, "Container/myPokerLayer")
    _myPokerLayer:SetActive(false)
    _poker_img1 = UnityTools.FindGo(gameObject.transform, "Container/myPokerLayer/poker_img1")
    UnityTools.AddOnClick(_poker_img1.gameObject, clickMyPokerCall)

    _poker_img2 = UnityTools.FindGo(gameObject.transform, "Container/myPokerLayer/poker_img2")
    UnityTools.AddOnClick(_poker_img2.gameObject, clickMyPokerCall)

    _poker_img3 = UnityTools.FindGo(gameObject.transform, "Container/myPokerLayer/poker_img3")
    UnityTools.AddOnClick(_poker_img3.gameObject, clickMyPokerCall)

    _poker_img4 = UnityTools.FindGo(gameObject.transform, "Container/myPokerLayer/poker_img4")
    UnityTools.AddOnClick(_poker_img4.gameObject, clickMyPokerCall)

    _poker_img5 = UnityTools.FindGo(gameObject.transform, "Container/myPokerLayer/poker_img5")
    UnityTools.AddOnClick(_poker_img5.gameObject, clickMyPokerCall)

    _minBet = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/minBet")

    _pokerCenter = UnityTools.FindCo(gameObject.transform, "Transform", "Container/bg/pokerCenter")

    _manager = _mainTransfrom.gameObject:GetComponent("FastMoveManager")
end

function NormalCowTopWinBind(evtID, gameObject)
    _winTb.top = gameObject
    _startEffect = UnityTools.FindGo(gameObject.transform, "Container/startEffect")

    _pChatR = UnityTools.FindGo(gameObject.transform, "Container/chatLayer/pChatR")

    _pChatL = UnityTools.FindGo(gameObject.transform, "Container/chatLayer/pChatL")

    _emojiCell = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/chatLayer/emoji")

    _restartLayer = UnityTools.FindGo(gameObject.transform, "Container/restartLayer")

    _restartBtn = UnityTools.FindGo(gameObject.transform, "Container/restartLayer/restart")
    UnityTools.AddOnClick(_restartBtn.gameObject, ClickRestartCall)

    _resultMask = UnityTools.FindGo(gameObject.transform, "Container/resultEffect/mask")

    initBoard()
 
    _winTb.ui.transform:Find("Container").gameObject:SetActive(true)
    _winTb.top.transform:Find("Container").gameObject:SetActive(true)
    _pokerLayer:SetActive(true)
    _myPokerLayer:SetActive(true)

    ResetNormalCowWinRenderQ(_mainTransfrom.gameObject)
    local t2 = gTimer.registerOnceTimer(400, roomMgr.SendJoinRoomMsg)
    gTimer.setRecycler(wName, t2)
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    mainWinBind(gameObject)
    if roomMgr.RoomType() == 10 then
        UtilTools.loadTexture(_winTb.pokerBg,"UI/Texture/res_table_2.png",true)
    end
--- [ALB END]

end

-- -------------------
-- 扑克牌飞行动作表现
-- -------------------
local function doPokerAction()
    local centerTf = _pokerCenter
    local actionTime = 550
    local delay = 50
    local pPos1 = centerTf.position

    -- 卡牌动作参数
    local tActionParams = {}
    for k = 5, 1, -1 do
        
        local poker = _pokerObjTable[0][k]
        local oldPos = poker.o.transform.position
        pokerMgr.SetPokerIcon(poker, nil)

        -- --------- v1.0  begin -------------------------------- 
        -- poker.tp:SetStartPos(pPos1.x + (0.01 * 5), pPos1.y, pPos1.z)
        -- poker.tp:SetEndPos(oldPos.x, oldPos.y, oldPos.z)
        -- poker.tp:SetParams(actionTime, delay, true, true)
        -- poker.tp:SetPositionToStartPos()
        -- _manager:Begin(poker.tp)
        -- --------- v1.0  end -------------------------------- 

        -- --------- v1.1  begin -------------------------------- 
        -- Add by WP.Chu 多次提交改为单次提交
        local act = {}
        table.insert(act, poker.tp)                     -- goTarget
        table.insert(act, 9999)                         -- renderQ
        table.insert(act, pPos1.x + (0.01 * 5))         -- startX
        table.insert(act, pPos1.y)                      -- startY
        table.insert(act, pPos1.z)                      -- startZ
        table.insert(act, oldPos.x)                     -- endX
        table.insert(act, oldPos.y)                     -- endY
        table.insert(act, oldPos.z)                     -- endZ
        table.insert(act, actionTime)                   -- duration
        table.insert(act, delay)                        -- delay
        table.insert(act, true)                         -- world
        table.insert(act, true)                         -- noEvt

        table.insert(tActionParams, act)
        -- --------- v1.1  end -------------------------------- 

        poker.o.transform.localScale = UnityEngine.Vector3(0.595, 0.595, 0.595)
        local act2 = TweenScale.Begin(poker.o, actionTime * 0.001 * 0.4, UnityEngine.Vector3(0.85, 0.85, 0.85))        
        act2.delay = delay / 1000

        delay = delay + 50
    end

    -- 动画表现 v1.1
    local actionCount = #tActionParams
    if actionCount > 0 then
        _manager:groupActionBegin(tActionParams, actionCount)
        tActionParams = {}
    end

    delay = delay - 150
    for i = 2, 5, 1 do
        if roomMgr.GetPlayerInfo(i) ~= nil then
            local pokerObj = _pokerObjTable[i]
            for k = 5, 1, -1 do
                local poker = pokerObj[k]
                local oldPos = poker.o.transform.position
                pokerMgr.SetPokerIcon(poker, nil)

                -- ------------------------- v1.0 begin ----------------------------
                -- poker.tp:SetStartPos(pPos1.x + (0.01 * (5 - i)), pPos1.y, pPos1.z)
                -- poker.tp:SetEndPos(oldPos.x, oldPos.y, oldPos.z)
                -- poker.tp:SetParams(actionTime, delay, true, true)
                -- poker.tp:SetPositionToStartPos()
                -- _manager:Begin(poker.tp)
                -- ------------------------- v1.0 end ----------------------------

                -- --------- v1.1  begin -------------------------------- 
                -- Add by WP.Chu
                local act = {}
                table.insert(act, poker.tp)                     -- goTarget
                table.insert(act, 9999)                         -- renderQ
                table.insert(act, pPos1.x + (0.01 * (5 - i)))   -- startX
                table.insert(act, pPos1.y)                      -- startY
                table.insert(act, pPos1.z)                      -- startZ
                table.insert(act, oldPos.x)                     -- endX
                table.insert(act, oldPos.y)                     -- endY
                table.insert(act, oldPos.z)                     -- endZ
                table.insert(act, actionTime)                   -- duration
                table.insert(act, delay)                        -- delay
                table.insert(act, true)                         -- world
                table.insert(act, true)                         -- noEvt

                table.insert(tActionParams, act)
                -- --------- v1.1  end -------------------------------- 

                delay = delay + 50
            end
            delay = delay - 80
            UnityTools.PlaySound("Sounds/poker", {delTime = 2+(delay / 1000), target = _mainTransfrom.gameObject, delay = delay / 1000})
        end
    end

    -- 动画表现 v1.1
    actionCount = #tActionParams
    if actionCount > 0 then
        _manager:groupActionBegin(tActionParams, actionCount)
        tActionParams = nil
    end
end

local function doStartEffects()
    -- triggerScriptEvent(EVENT_GAME_START_EFFECT, 1)
    local ct = UnityTools.FindCo(_startEffect.transform, "TweenAlpha", "Container")
    local bg = UnityTools.FindCo(_startEffect.transform, "TweenScale", "Container/bg")
    local lb0 = UnityTools.FindCo(_startEffect.transform, "TweenPosition", "Container/lb_0")
    local lb1 = UnityTools.FindCo(_startEffect.transform, "TweenPosition", "Container/lb_1")
   
    ct:SetOnFinished(function (ob)  
        UnityTools.SetActive(_startEffect, false)
        -- _startEffect:SetActive(false)
     end)
    UnityTools.PlaySound("Sounds/gameStart", {target = _mainTransfrom.gameObject})
    -- _startEffect:SetActive(true)
    UnityTools.SetActive(_startEffect, true)
    ct:ResetToBeginning()
    bg:ResetToBeginning()
    ct:Play(true)
    bg:Play(true)

    lb0:Play(true)
    lb1:Play(true)
    local timer = gTimer.registerOnceTimer(1000, function () 
        lb0:Play(false)
        lb1:Play(false)    
    end)
    gTimer.setRecycler(wName, timer)
end



local function Awake(gameObject)
    _mainTransfrom = gameObject.transform
    AutoLuaBind(gameObject)
     
    registerScriptEvent(EVENT_RECONNECT_SOCKET, "NormalCowCloseFunc")
    registerScriptEvent(NORMAL_COW_UI_WIN_START, "NormalCowUIWinBind")
    registerScriptEvent(NORMAL_COW_RED_UI_WIN_START, "NormalCowRedUIWinBind")
    registerScriptEvent(NORMAL_COW_TOP_WIN_START, "NormalCowTopWinBind")
    if roomMgr.RoomType() == 10 then
        UnityTools.CreateLuaWin("NormalCowRedUI")
        protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_room_player_times_update,"NormalCowMainRecvRedTimesUpdate")
        -- protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_room_redpack_notice_update,"NormalCowMainRecvRedNoticeUpdate")
        protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_room_draw_reply,"NormalCowMainRecvRedRewardUpdate")
        -- protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_redpack_timer_sec_update,"NormalCowMainRecvRedTimerUpdate")
        
    else
        UnityTools.CreateLuaWin("NormalCowUI")
    end
    
    -- Lua Editor 自动绑定
    -- AutoLuaBind(gameObject)
    
    
    -- initBoard()
    registerScriptEvent(UPDATE_MAIN_WIN_RED, "NormalCowMainUpdateTaskRed")
    registerScriptEvent(EVENT_NIU_UPDATE_PLAYER_INFO, "NormalCowMainRecvUpdatePlayer")
    registerScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, "NormalCowMainRecvUpdateState")
    registerScriptEvent(EVENT_NIU_UPDATE_RESULT, "NormalCowMainRecvResult")
    -- registerScriptEvent(EVENT_NIU_ENTER_ROOM, "NormalCowMainRecvEnterResult")
    registerScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, "NormalCowMainRecvChatInfo")
    
    
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_player_choose_master_rate_update,"NormalCowMainRecvDealerRateReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_player_choose_free_rate_update,"NormalCowMainRecvRateReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_player_submit_card_update,"NormalCowMainRecvSubmitReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_room_chest_info_update,"NormalCowMainRecvGameTimesReply")
    
end



function NormalCowMainChangeGameStateCall()
    
    _state_tip.text = ""
    if roomMgr.State() == roomMgr.TState.Flop and _helperTipStr ~= nil then
        local lTime = _lastTime 
        if _lastTime  == 0 and _best_pokers ~= nil then  
            submitPokerBtnCall()
        elseif lTime < 0 then
            lTime = 0
        end
        _helperTip.text = _helperTipStr .. "[e55c09]" .. tostring(lTime) .. "[-]"
        -- _flopTimeBar.fillAmount = _lastTime / 8.0
        
        _flopTimeLb.text = tostring(_lastTime)
        _state_tip.text = ""
    else
        if (roomMgr.State() == roomMgr.TState.WaitStart and _lastTime <= 0) or _stateTip == nil then
            _state_tip.text = ""
        else
            _state_tip.text = LuaText.GetStr(_stateTip, tostring(_lastTime))
        end
    end
    _lastTime = _lastTime - 1
    if _lastTime <= 0 then
        _lastTime = 0
    end
end

local function Start(gameObject)
    
    local t1 = gTimer.registerOnceTimer(200, gameMgr.CallSucc)
    gTimer.setRecycler(wName, t1)
   
    -- UnityTools.SetActive(_pokerLayer, true)
    -- UnityTools.SetActive(_myPokerLayer, true)
    
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(NORMAL_COW_RED_UI_WIN_START, "NormalCowRedUIWinBind")
    unregisterScriptEvent(NORMAL_COW_UI_WIN_START, "NormalCowUIWinBind")
    unregisterScriptEvent(NORMAL_COW_TOP_WIN_START, "NormalCowTopWinBind")
    unregisterScriptEvent(EVENT_NIU_UPDATE_PLAYER_INFO, "NormalCowMainRecvUpdatePlayer")
    unregisterScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, "NormalCowMainRecvUpdateState")
    unregisterScriptEvent(EVENT_NIU_UPDATE_RESULT, "NormalCowMainRecvResult")
    -- unregisterScriptEvent(EVENT_NIU_ENTER_ROOM, "NormalCowMainRecvEnterResult")
    unregisterScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, "NormalCowMainRecvChatInfo")
    unregisterScriptEvent(UPDATE_MAIN_WIN_RED, "NormalCowMainUpdateTaskRed")
    unregisterScriptEvent(EVENT_RECONNECT_SOCKET, "NormalCowCloseFunc")
    -- protobuf:removeMessageHandler(protoIdSet.sc_redpack_redpack_timer_sec_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_room_chest_info_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_player_choose_free_rate_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_player_submit_card_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_player_choose_master_rate_update)
    protobuf:removeMessageHandler(protoIdSet.sc_redpack_room_player_times_update)
    -- protobuf:removeMessageHandler(protoIdSet.sc_redpack_room_redpack_notice_update)
    protobuf:removeMessageHandler(protoIdSet.sc_redpack_room_draw_reply)

    gTimer.removeTimer("NormalCowMainChangeGameStateCall")
    gTimer.recycling(wName)
    UnityTools.RemoveDeactiveList()

            
    _cellList = nil
    _playerList = nil
    _playerLabelTb = nil
    _pokerList = nil
    _myPokerList = nil
    _stateTip = nil
    _temp_dealer_rate = nil
    _best_pokers = nil
    _pokerOrginPos = nil
    _mainTransfrom = nil
    _mainUIPanel = nil
    _goldPreMeshRender = nil
    _compsTable = nil
    _pokerObjTable = nil
    _operObjTb = nil
    _winTb = nil
    UnityTools.DelPools()
    CLEAN_MODULE("NormalCowMainMono")
    gameMgr.ExitGame()

end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end

function NormalCowChooseDealerTimerCall(layers, dealers, callTime, index)
    if UnityTools.IsWinShow(wName) == false then return nil end
    local pIndex = dealers[index]
    local lastIndex = index - 1
    if lastIndex <= 0 then
        lastIndex = #dealers
    end
    local lPIndex = dealers[lastIndex]
    local eff = UnityTools.FindGo(layers.transform, "b" .. pIndex)
    local lEff = UnityTools.FindGo(layers.transform, "b" .. lPIndex)
    UnityTools.PlaySound("Sounds/dealerChosing", {target = _mainTransfrom.gameObject})
    eff:SetActive(true)
    lEff:SetActive(false)
    index = index + 1
    if index > #dealers then
        index = 1
    end
    if callTime <= 80 then
        callTime = callTime + 2.5
    elseif callTime <= 250 then
        callTime = callTime + 25

    end
    if roomMgr.State() == roomMgr.TState.OverDealer then
        local timer = gTimer.registerOnceTimer(callTime, "NormalCowChooseDealerTimerCall", layers, dealers, callTime, index)
        gTimer.setRecycler(wName, timer)
    else
        layers:SetActive(false)
        eff:SetActive(false)
    end
end

local function chooseDealerAction(dealers)
    
    local callTime = 30
    local index = 1
    local count = #dealers
    if count > 1 then
        _dealerEffs:SetActive(true)
        local timer = gTimer.registerOnceTimer(callTime, "NormalCowChooseDealerTimerCall", _dealerEffs, dealers, callTime, index)
        gTimer.setRecycler(wName, timer)
    end
end

local function setDealerRate(player, rate)
    local dealerLayer = _playerLabelTb[player].dealer
    local dealerHas = UnityTools.FindGo(dealerLayer.transform, "has")
    local dealerNo = UnityTools.FindGo(dealerLayer.transform, "no")
    if rate == -1 then 
        -- UnityTools.SetActive(_playerLabelTb[player].dealer, false)
        return    
    end
    UnityTools.SetActive(_playerLabelTb[player].dealer, true)
    if rate == 0 then
        dealerHas:SetActive(false)
        dealerNo:SetActive(true)
    else
        dealerHas:SetActive(true)
        dealerNo:SetActive(false)
        local dealerNum = UnityTools.FindCo(dealerHas.transform, "UISprite", "num")
        dealerNum.spriteName = "y_" .. tostring(rate)
    end
end

local function checkDealerMoney(btnTb)
    local myPlayerInfo = roomMgr.GetPlayerInfo(1)
    if myPlayerInfo == nil then return end
    local myMoney = myPlayerInfo.gold_num
    for i = 1, 3, 1 do
        local btn = btnTb[i]
        if myMoney >= (i + 1) * 100 * roomMgr.MinBet() then
            _compsTable[btn].BC.enabled = true
            _compsTable[btn].SP.alpha = 1
        else
            _compsTable[btn].BC.enabled = false
            _compsTable[btn].SP.alpha = 0.5
        end
    end
end

local function checkBetMoney(btnTb)
    local myPlayerInfo = roomMgr.GetPlayerInfo(1)
    if myPlayerInfo == nil then return end
    local dealerBet = roomMgr.GetPlayerInfo(roomMgr.DealerIndex()).master_rate
    if dealerBet == nil then 
        dealerBet = 1 
        LogError("服务端庄家押注数据异常")
    end
    local myMoney = myPlayerInfo.gold_num
    for i = 1, 4, 1 do
        local btn = btnTb[i]
        if myMoney >= (i * 20 + 20) * dealerBet * roomMgr.MinBet() then
            _compsTable[btn].BC.enabled = true
            _compsTable[btn].SP.alpha = 1
        else
            _compsTable[btn].BC.enabled = false
            _compsTable[btn].SP.alpha = 0.5
        end
    end
end

local function showOperate(index)
    local showOper = true
    if index == 1 then -- 抢庄
        UnityTools.SetActive(_operObjTb.dealer, true)
        UnityTools.SetActive(_operObjTb.bets, false)
        UnityTools.SetActive(_operObjTb.cow, false)
    
        checkDealerMoney({_dealerbtn3, _dealerbtn4, _dealerbtn5})
    elseif index == 2 then -- 下注
        UnityTools.SetActive(_operObjTb.dealer, false)
        UnityTools.SetActive(_operObjTb.bets, true)
        UnityTools.SetActive(_operObjTb.cow, false)
        checkBetMoney({_betBtn2, _betBtn3, _betBtn4, _betBtn5})
    elseif index == 3 then
        UnityTools.SetActive(_operObjTb.dealer, false)
        UnityTools.SetActive(_operObjTb.bets, false)
        UnityTools.SetActive(_operObjTb.cow, true)
    else
        showOper = false
        UnityTools.SetActive(_operObjTb.dealer, false)
        UnityTools.SetActive(_operObjTb.bets, false)
        UnityTools.SetActive(_operObjTb.cow, false)
    end
    UnityTools.SetActive(_operLayer, showOper)
end

local function setPlayerFreeRate(index, rate)
    local player = _playerList[index]
    local betLb = _playerLabelTb[player].bet
    if rate == -1 then
        -- UnityTools.SetActive(betLb, false)
        return
    else
        UnityTools.SetActive(betLb, true)
        betLb.spriteName = rate
    end
end

function NormalCowMainFlopTimerCall()
    
    if _flopTimeBar == nil or roomMgr.State() ~= roomMgr.TState.Flop or _flopTimeBar.fillAmount <= 0 then
        gTimer.removeTimer(NormalCowMainFlopTimerCall)
    else
        _flopTimeBar.fillAmount = _flopTimeBar.fillAmount - 0.00825
    end
end

function NormalCowMainRestartLayerCall(show)
    _restartLayer:SetActive(true)
    _winTb.redTitleLayer:SetActive(false)
    _getGoldBtn:SetActive(true)
    _playerList[1].transform:Find("name").gameObject:SetActive(true)
    -- UnityTools.MessageDialog("钻石不足，您已自动离开牌桌！是否前往商城购买钻石？",{okCall=function(f) 
    --     local shopCtrl = IMPORT_MODULE("ShopWinController");
    --     if shopCtrl ~= nil then
    --         shopCtrl.OpenShop(2)
    --     end
    -- end, cancelCall = function(f) 
    --     closeFunc()
    -- end})
    LogError("OpenRedConditionWin")
    platformMgr.OpenRedConditionWin()
end

local function removeRestartLayerCall()
    gTimer.removeTimer(NormalCowMainRestartLayerCall)
    if UnityTools.IsWinShow("RedConditionWin") == false then return nil end
    _restartLayer:SetActive(false)
    _winTb.redTitleLayer:SetActive(true)
    _getGoldBtn:SetActive(true)
    UnityTools.DestroyWin("RedConditionWin")
end

function NormalCowMainRecvUpdatePlayer(msgID, index, playerInfo)
    if _clickClosed == true then return nil end
    -- if 1 then return nil end
    if index == 1 and playerInfo == nil then
        
        roomMgr.RemoveAllPlayerPosition()
        
        UnityTools.SetActive(_pokerLayer, false)
        UnityTools.SetActive(_myPokerLayer, false)
        UnityTools.SetActive(_flopStateLayer, false)
        showOperate(4)
        resetPokers()
        resetPlayer()
        if roomMgr.RoomType() ~= 10 then
            _restartLayer:SetActive(true)
            _gameTimes = 0
            addGameTimes()
            UnityTools.MessageDialog("金币不足，您已自动离开牌桌！是否前往商城购买金币？",{okCall=platformMgr.OpenShopWin, cancelCall = platformMgr.OpenFastAddGold})
            return nil
        else
                    
            local timer = gTimer.registerOnceTimer(1000, NormalCowMainRestartLayerCall, true)
            gTimer.setRecycler(wName, timer)
            return nil
        end
        
    end
    local player = _playerList[index]
    if player ~= nil then 
        if roomMgr.RoomType() == 10 and playerInfo ~= nil then  -- 红包模式刷新用户数据
            if roomMgr.State() == roomMgr.TState.Over then  -- 只处理结算时的数据
                local t = gTimer.registerOnceTimer(3200, function(p, pInfo)
                    roomMgr.SetPlayerIcon(_winTb.thisObj,_winTb.thisObj,_playerList[p], pInfo)
                end, index, playerInfo)
                gTimer.setRecycler(wName, t)
            else
                roomMgr.SetPlayerIcon(_winTb.thisObj,player, playerInfo)
                -- LogError(index .. "  ----" .. roomMgr.State())
            end
            removeRestartLayerCall()
        else
            -- LogError(index .. "  +++")
            roomMgr.SetPlayerIcon(_winTb.thisObj,player, playerInfo)
        end
        if playerInfo ~= nil then
            
            if playerInfo.master_rate ~= nil and (roomMgr.DealerIndex() == 0 or roomMgr.DealerIndex() == index) then
                setDealerRate(player, playerInfo.master_rate)
                -- LogError("ID = " .. playerInfo.player_uuid)
            else
                setDealerRate(player, -1)
            end
            if playerInfo.free_rate ~= nil then
                if roomMgr.DealerIndex() ~= index then
                    setPlayerFreeRate(index, playerInfo.free_rate)
                else
                    setPlayerFreeRate(index, -1)
                end
            end
            if playerInfo.open_card_list ~= nil then
                if index == 1 then
                    roomMgr.SetMyPokerInfo(playerInfo.open_card_list)
                    triggerScriptEvent(EVENT_NIU_UPDATE_MY_POKER, playerInfo.open_card_list)
                else
                    NormalCowMainRecvSubmitReply(protoIdSet.sc_niu_player_submit_card_update, {card_type = playerInfo.card_type, card_list = playerInfo.open_card_list}, index)
                end
            end
        end
    end
end

local function changeBoardSprite(board, name)
    if roomMgr.State() == roomMgr.TState.OverDealer then
        if board.spriteName == "light_yellow" then
            board.spriteName = ""
        else
            board.spriteName = "light_yellow"
        end
        local timer = gTimer.registerOnceTimer(_dealerEffTime, name, board)
        gTimer.setRecycler(wName, timer)
    end
end

function NormalCowMainChangeBoardSprite1(board)
    changeBoardSprite(board, "NormalCowMainChangeBoardSprite1")
end

function NormalCowMainChangeBoardSprite2(board)
    changeBoardSprite(board, "NormalCowMainChangeBoardSprite2")
end

function NormalCowMainChangeBoardSprite3(board)
    changeBoardSprite(board, "NormalCowMainChangeBoardSprite3")
end

function NormalCowMainChangeBoardSprite4(board)
    changeBoardSprite(board, "NormalCowMainChangeBoardSprite4")
end

function NormalCowMainChangeBoardSprite5(board)
    changeBoardSprite(board, "NormalCowMainChangeBoardSprite5")
end

local function specialDealerState()
    UnityTools.SetActive(_pokerLayer, true)
    UnityTools.SetActive(_myPokerLayer, true)
    local myPokerbag = pokerMgr.GetPokerBag(1)
    local pokerList = myPokerbag:FinalPokers()
    -- 设置玩家的牌
    for k, pokerObj in pairs(_pokerObjTable) do
        if k ~= 0 and (roomMgr.GetPlayerInfo(k) == nil or k == 1) then
            UnityTools.SetActive(pokerObj.parent, false)
        else
            UnityTools.SetActive(pokerObj.parent, true)
        end
    end
    -- 设置我的大牌
    if _myPokerList == nil then return nil end
    for i = 0, pokerList.Count - 1, 1 do
        pokerMgr.SetPokerIcon(_pokerObjTable[0][i+1], {color = 5 - tonumber(pokerList[i].Type), number = pokerList[i].Num}, true)
    end

end

local function dealerChoseOver()
    _temp_dealer_rate = nil
    _dealerEffs:SetActive(false)
    for k, v in pairs(_playerList) do 
        local player = _playerList[k]
        if k == roomMgr.DealerIndex() then
            roomMgr.SetPlayerIcon(_winTb.thisObj,player, roomMgr.GetPlayerInfo(roomMgr.DealerIndex()), true)
            local dealerLayer = _playerLabelTb[player].dealer
            
            local dealerNo = UnityTools.FindGo(dealerLayer.transform, "no")
            if dealerNo.activeSelf == true then
                setDealerRate(player, 1)
            end
        else
            local board = UnityTools.FindCo(player.transform, "UISprite", "player_img_bg/board")
            board.spriteName = ""
            local dealerLayer =  _playerLabelTb[player].dealer
            UnityTools.SetActive(_playerLabelTb[player].dealer, false)
        end
    end
end

function NormalCowMainRecvUpdateState(msgID, state)
    _lastTime = roomMgr.LastTime()
    local tState = "room_state_" .. tostring(roomMgr.State())
    _stateTip = LuaText[tState]
    gTimer.removeTimer("NormalCowMainChangeGameStateCall")
    if state ~= roomMgr.TState.Over then
        gTimer.registerDelayTimerEvent(0, 1000, "NormalCowMainChangeGameStateCall")
    end
    if state == roomMgr.TState.WaitJoin then  
        UnityTools.SetActive(_pokerLayer, false)
        UnityTools.SetActive(_myPokerLayer, false)
        
    elseif state == roomMgr.TState.WaitStart then
        
        if roomMgr.RoomType() == 10 then
            _stateTip = ""
        end
        if _temp_dealer_rate ~= nil then
            _temp_dealer_rate = nil
        end
        UnityTools.SetActive(_pokerLayer, false)
        UnityTools.SetActive(_myPokerLayer, false)
        UnityTools.SetActive(_flopStateLayer, false)
        _temp_dealer_rate = {}
        _best_poker_type = 0
        resetPokers()
        resetPlayer()
    elseif state == roomMgr.TState.WaitDealer then
        
        if roomMgr.State() == roomMgr.TState.WaitDealer then
            doStartEffects()
            local delayTime = 1500
            local tt = gTimer.registerOnceTimer(delayTime, function () 
                local oT = gTimer.registerOnceTimer(1500, showOperate, 1)
                gTimer.setRecycler(wName, oT)
                UnityTools.SetActive(_pokerLayer, true)
                UnityTools.SetActive(_myPokerLayer, true)
                
                
                -- 设置玩家的牌
                for k, pokerObj in pairs(_pokerObjTable) do
                    if k ~= 0 and (roomMgr.GetPlayerInfo(k) == nil or k == 1) then
                        UnityTools.SetActive(pokerObj.parent, false)
                    else
                        UnityTools.SetActive(pokerObj.parent, true)
                    end
                end
                local timer = gTimer.registerOnceTimer(700, function () 
                    -- 设置我的大牌
                    if _myPokerList == nil then return nil end
                    local myPokerbag = pokerMgr.GetPokerBag(1)
                    local pokerList = myPokerbag:FinalPokers()
                    -- myPokerbag:PrintPokers(pokerList)
                    for i = 0, pokerList.Count - 1, 1 do
                        pokerMgr.SetPokerIcon(_pokerObjTable[0][i+1], {color = 5 - tonumber(pokerList[i].Type), number = pokerList[i].Num}, true)
                    end
                end)
                gTimer.setRecycler(wName, timer)    
                doPokerAction()
            end)
            
            gTimer.setRecycler(wName, tt)
        else
            specialDealerState()
        end
        
    elseif state == roomMgr.TState.OverDealer then
        showOperate(4)
        for i = 4, 0, -1 do
            if _temp_dealer_rate[i] ~= nil then
                chooseDealerAction(_temp_dealer_rate[i])
                break
            end
        end
    elseif state == roomMgr.TState.BetRounds then
        dealerChoseOver()
        

        if roomMgr.DealerIndex() == 1 then
            showOperate(4)
        else
            showOperate(2)
        end
    elseif state == roomMgr.TState.Flop then
        if roomMgr.RoomType() == 10 then
            dealerChoseOver()
        end
        _flopTimeBar.fillAmount = roomMgr.LastTime() / 8.0
        UnityTools.SetActive(_flopStateLayer, true)
        gTimer.registerRepeatTimer(50, NormalCowMainFlopTimerCall)

        _click_poker_list = {}
        _click_poker_cnt = 0
        resetHelperNum()
        local myPokerbag = pokerMgr.GetPokerBag(1)
        local pokerList = myPokerbag:FinalPokers()
        -- myPokerbag:PrintPokers(pokerList)
        pokerMgr.SetPokerIcon(_pokerObjTable[0][5], {color = 5 - tonumber(pokerList[4].Type), number = pokerList[4].Num}, true)
        local hPokers, hType, hPoker = pokerMgr.GetBestSelect(myPokerbag:Combinations(5))
        _best_pokers = hPokers
        _best_poker_type = hType
        
        local t1 = gTimer.registerOnceTimer(500, function() 
            if hType > 0 and hType < 11 then
                
                for k = 0, 2, 1 do
                    for i = 0 , pokerList.Count - 1, 1 do
                        
                        if hPokers[k]:toString() == pokerList[i]:toString() then
                            clickMyPokerCall(_myPokerList[i + 1])
                        end
                    end
                end
            end
        end)
        gTimer.setRecycler(wName, t1)
        showOperate(3)

    elseif state == roomMgr.TState.Over then
        UnityTools.SetActive(_flopStateLayer, false)
        _best_pokers = nil
        _click_poker_list = nil
        _click_poker_cnt = 0
        showOperate(4)
        roomMgr.SendInGameMsg()
        if _manager ~= nil then
            _manager:ResetRender()
        end
    end
end

local function playerChatAction(msgData)
    local parent = _pChatL.transform.parent.gameObject
    local tf = parent.transform:FindChild("chatCell" .. msgData.player_seat_pos)
    if tf ~= nil then
        gTimer.removeTimer(UnityTools.Destroy)
        UnityTools.Destroy(tf.gameObject)
    end
    local chatCell = nil
    local sContent, type, key = GetShowChatContent(msgData.content)
    if msgData.content_type == 1 then
        local layer = UnityTools.FindTf(_playerList[msgData.player_seat_pos].transform, "player_img_bg/img")
        local initPos = layer.localPosition
        chatCell = UtilTools.AddChild(layer.gameObject, _emojiCell.gameObject, initPos)
        chatCell.name = "chatCell" .. msgData.player_seat_pos
        chatCell:SetActive(true)
        local content = chatCell:GetComponent("UISprite")
        local sAction = chatCell:GetComponent("TweenScale")
        content.spriteName = sContent
        sAction.delay = 0
        sAction:ResetToBeginning()
        sAction:Play(true)
        chatCell.transform.localPosition = UnityEngine.Vector3(initPos.x, initPos.y-7, initPos.z)
        local dAction = TweenPosition.Begin(chatCell, 0.6, UnityEngine.Vector3(initPos.x, initPos.y+7, initPos.z), false)
        dAction.style = 2
        local timer = gTimer.registerOnceTimer(4000, function (ob) UnityTools.Destroy(ob) end, chatCell)
        gTimer.setRecycler(wName, timer)
        return nil 
    elseif msgData.content_type == 2 then
        local toIndex = roomMgr.GetIndexByUUID(msgData.des_player_uuid)
        if toIndex == nil or _playerList[msgData.player_seat_pos] == nil or _playerList[toIndex] == nil then return nil end
        local fromPos = UnityTools.FindTf(_playerList[msgData.player_seat_pos].transform, "player_img_bg/img").position
        local toPos = UnityTools.FindTf(_playerList[toIndex].transform, "player_img_bg/img").position
        AddMagicEmoji(parent, fromPos, toPos, _emojiCell, key)
        return nil
    end

    if msgData.player_seat_pos == 1 or msgData.player_seat_pos >= 4 then 
        local initPos = _playerList[msgData.player_seat_pos].transform.localPosition
        initPos = UnityEngine.Vector3(initPos.x + 20, initPos.y + 40, initPos.z)
        chatCell = UtilTools.AddChild(parent, _pChatL, initPos)
    else
        local initPos = nil
        initPos = _playerList[msgData.player_seat_pos].transform.localPosition
        initPos = UnityEngine.Vector3(initPos.x + 10, initPos.y + 40, initPos.z)
    
        
        chatCell = UtilTools.AddChild(parent, _pChatR, initPos)
    end
    if type == "0" or type == "1" then
        local emojiData = LuaConfigMgr.ChatEmojiConfig[key]
        if emojiData ~= nil then
            UnityTools.PlaySound("Sounds/" .. emojiData.sound, {target = _mainTransfrom.gameObject})
        end
    end
    chatCell.name = "chatCell" .. msgData.player_seat_pos
    chatCell:SetActive(true)
    local content = UnityTools.FindCo(chatCell.transform, "UILabel", "content")
    local sAction = chatCell:GetComponent("TweenScale")
    local eAction = chatCell:GetComponent("TweenAlpha")
    content.text = sContent
    sAction:ResetToBeginning()
    eAction:ResetToBeginning()
    sAction:Play(true)
    eAction:Play(true)
    eAction:SetOnFinished(function() UnityTools.Destroy(chatCell) end)
end

function NormalCowMainRecvChatInfo(msgID, msgData)
    if UnityTools.IsWinShow(wName) == false then return nil end
    msgData.player_seat_pos = roomMgr.PosToIndex(msgData.player_seat_pos)
    playerChatAction(msgData)
end

function NormalCowMainRecvGameTimesReply(msgID, result)
    -- LogError("Receive .." .. result.times)
    _gameTimes = result.times
    if _gameTimes == 0 and result.times ~= 0 then
        _gameTimes = 5
    end
    addGameTimes()
end

-- function NormalCowMainRecvEnterResult(msgID, result)
--     --LogError("Into Room----------------")
--     -- addGameTimes()
-- end



function NormalCowMainRecvResult(msgID, resultInfo)
    -- addGameTimes()
    UnityTools.SetActive(_myPokerLayer, false)
    local giveTb = {}
    local gCnt = 0
    local sendTb = {}
    local sCnt = 0
    local isMyWin = false
    
    for k, v in pairs(resultInfo) do
        local index = roomMgr.PosToIndex(v.player_pos)

        local reward = v.reward_num
        local player = _playerList[index]
        local getNum = _playerLabelTb[player].get
        local costNum = _playerLabelTb[player].cost

        local showLb = nil
        -- LogError(index .. "  " .. reward)
        if tonumber(v.reward_num) > 0 then
            if roomMgr.DealerIndex() ~= index then
                sendTb[index] = v.reward_num 
                sCnt = sCnt + 1
            end
            if index == 1 then
                isMyWin = true
            end
            UnityTools.SetActive(getNum, true)
            UnityTools.SetActive(costNum, false)
            getNum.text = "+" .. v.reward_num
            showLb = getNum
        elseif tonumber(v.reward_num) < 0 then
            if roomMgr.DealerIndex() ~= index then
                giveTb[index] = v.reward_num 
                gCnt = gCnt + 1
            end
            if index == 1 then
                isMyWin = false
            end
            UnityTools.SetActive(getNum, false)
            UnityTools.SetActive(costNum, true)
            costNum.text = "-" .. math.abs(v.reward_num)
            showLb = costNum
            
        end
        
        if roomMgr.RoomType() ~= 10 then
            roomMgr.ChangePlayerGold(index, v.reward_num)
        end

        if showLb ~= nil then
            local scale = 0
            local time = 500
            showLb.transform.localScale = UnityEngine.Vector3(scale, scale, scale)
            local act1 = TweenScale.Begin(showLb.gameObject, time / 1000, UnityEngine.Vector3(1.15, 1.15, 1))
            local action = showLb:GetComponent("TweenAlpha")
            local waitTime = 4000

            act1.delay = 4
            action.delay = 5
            -- end
            
            action:ResetToBeginning()
            action:Play(true)
            
            local t1 = gTimer.registerOnceTimer(waitTime, function () UnityTools.SetActive(_resultMask, true) end)
            local t2 = gTimer.registerOnceTimer(waitTime + 1400, function () 
                UnityTools.SetActive(_resultMask, false) 
            end)
            if roomMgr.RoomType() == 10 then
                waitTime = 4200
            else 
                waitTime = 3700
            end
            local t3 = gTimer.registerOnceTimer(waitTime, function (obj)
                UnityTools.SetActive(obj.bet, false)
                UnityTools.SetActive(obj.dealer, false)
            end, _playerLabelTb[player])
            gTimer.setRecycler(wName, t1)
            gTimer.setRecycler(wName, t2)
            gTimer.setRecycler(wName, t3)
        end
    end

    if sCnt == 0 then
        UnityTools.PlaySound("Sounds/bothWin", {target = _mainTransfrom.gameObject})
    elseif gCnt == 0 then
        UnityTools.PlaySound("Sounds/bothLose", {target = _mainTransfrom.gameObject})
    else
        if isMyWin == true then
            UnityTools.PlaySound("Sounds/win", {target = _mainTransfrom.gameObject})
        else
            UnityTools.PlaySound("Sounds/failed", {target = _mainTransfrom.gameObject})
        end
    end
    if roomMgr.DealerIndex() == 1 then
        if isMyWin == true then
            if sCnt == 0 then
                doResultEffects(true, true)
            else
                doResultEffects(true, false)
            end
        else
            if gCnt == 0 then
                doResultEffects(false, true)
            else
                doResultEffects(false, false)
            end
        end
    else
        if isMyWin == true then
            doResultEffects(true, false)
        else
            doResultEffects(false, false)
        end
    end
    -- if roomMgr.RoomType() ~= 10 then
    local timer = gTimer.registerOnceTimer(2100, goldAction, roomMgr.DealerIndex(), sendTb, giveTb, 200)
    gTimer.setRecycler(wName, timer)
    -- end
end

-- 红包消息-----------------------------------------------------
function NormalCowMainRecvRedTimesUpdate(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        -- LogError("NormalCowMainRecvRedTimesUpdate" .. msgData.now_play_times)
        _winTb.redProLb_1.text = msgData.now_play_times
        _winTb.redProLb_2.text = "/5"
        --LuaText.GetStr(LuaText.normal_red_title, msgData.now_play_times)
        _winTb.redPro_1.value = msgData.now_play_times / 5
        _winTb.titleLb.text = LuaText.normal_red_tip1
        if msgData.now_play_times >= 5 then
            -- local eff = UnityTools.AddEffect(_winTb.top.transform.parent,"effect_hongbaoyu",{loop = false, complete=
            -- function(gameObject)
            --     UtilTools.SetEffectRenderQueueByUIParent(_winTb.top.transform,gameObject.EffectGameObj.transform,1)
            --     gameObject.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,0,0)
            --     gameObject.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
            --     UnityTools.PlaySound("Sounds/redcoming",{target = _mainTransfrom.gameObject})
            -- end})
            eff = nil
            UnityTools.PlaySound("Sounds/redcoming",{target = _mainTransfrom.gameObject})
            UnityTools.ShowMessage("已连续游戏5局，快来领取你的红包奖励！")
            _winTb.redpackEff:SetActive(true)
            
            _getGoldBtn:GetComponent("TweenScale").enabled = true
            UtilTools.RevertGray(_getGoldBtn, false, true)
            
            _winTb.redProLb_1.text = "5"
            _winTb.redGou_1:SetActive(true)
            if _winTb.redGou_2.activeSelf == true then
                _winTb.titleLb.text = LuaText.normal_red_tip2
            end
        else
            _winTb.redpackEff:SetActive(false)
            _winTb.redGou_1:SetActive(false)
            _getGoldBtn:GetComponent("TweenScale").enabled = false
        end
    else
        LogError("Recv nil NormalCowMainRecvRedTimesUpdate")
    end
end

-- function NormalCowMainRecvRedNoticeUpdate(msgID, msgData)
--     if UnityTools.CheckMsg(msgID, msgData) then
--         if msgData.close_draw_second == 0 then
--             UnityTools.ShowMessage("很遗憾，未达成红包领取条件，请等待下场红包雨")
--         else
--              local eff = UnityTools.AddEffect(_winTb.top.transform.parent,"effect_hongbaoyu",{loop = false, complete=
--             function(gameObject)
--                 UtilTools.SetEffectRenderQueueByUIParent(_winTb.top.transform,gameObject.EffectGameObj.transform,1)
--                 gameObject.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,0,0)
--                 gameObject.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
--                 UnityTools.PlaySound("Sounds/redcoming",{target = _mainTransfrom.gameObject})
--             end})
--             eff = nil
            
--             UnityTools.ShowMessage("5局游戏已完成，快领取你的奖励红包！")
--             _winTb.redpackEff:SetActive(true)
--             _gameTimesLabel.gameObject:SetActive(true)
--             _getGoldBtn:GetComponent("TweenScale").enabled = true
--             UtilTools.RevertGray(_getGoldBtn, false, true)
            
--             -- _winTb.redTimePro:SetActive(false)
--             local endTime = msgData.close_draw_second - UtilTools.GetServerTime()
--             local coolDown = _gameTimesLabel:GetComponent("CooldownUpdate")
--             coolDown:SetEndTime(msgData.close_draw_second)
--             _winTb.redpackTimer = gTimer.registerOnceTimer(endTime * 1000, function() 
--                 _getGoldBtn:GetComponent("TweenScale").enabled = false
-- --                UtilTools.SetGray(_getGoldBtn, false, true)
--                 -- _winTb.redTimePro:SetActive(true)
--                 _winTb.redpackEff:SetActive(false)
--                 _gameTimesLabel.gameObject:SetActive(false)
--                 if UnityTools.IsWinShow("OpenRedpackWinMono") == true then
--                     UnityTools.DestroyWin("OpenRedpackWinMono")
--                 end
--             end)
--             gTimer.setRecycler(wName, _winTb.redpackTimer)

--             _winTb.redComeTime = msgData.next_open_redpack_second - UtilTools.GetServerTime()
--             _winTb.redTimeSp.fillAmount = 1 - (_winTb.redComeTime / Red_Coming)
--             NormalCowMainRecvRedTimesUpdate(0, {now_play_times = 0})
--         end
--     else
--         LogError("Recv nil NormalCowMainRecvRedNoticeUpdate")
--     end
-- end

function NormalCowMainRecvRedRewardUpdate(msgID, msgData)

    if UnityTools.CheckMsg(msgID, msgData) then

        if UnityTools.IsWinShow("OpenRedpackWin") == true then
            ReceiveRedOpenReply(msgID, msgData)
            roomMgr.SetPlayerIcon(_winTb.thisObj,_playerList[1], roomMgr.GetPlayerInfo(1))
        end

        _winTb.redMyNeedTime = msgData.next_can_draw_second - UtilTools.GetServerTime()
        _winTb.redGou_2:SetActive(false)
        _winTb.redPro_2.value = 1 - (_winTb.redMyNeedTime / Red_MyTime)
        -- local coolDown = _winTb.redProLb_2.gameObject:GetComponent("CooldownUpdate")
        -- coolDown:SetEndTime(msgData.next_can_draw_second)
    else
        LogError("Recv nil NormalCowMainRecvRedRewardUpdate----->>")
--        PrintTable(msgData)
        if UnityTools.IsWinShow("OpenRedpackWin") then
            UnityTools.DestroyWin("OpenRedpackWin")
        end
    end
end

function NormalCowMainRedTimerCall()
    
    if _winTb.redMyNeedTime == 1 then
        _winTb.redGou_2:SetActive(true)
    end
    _winTb.redComeTime = _winTb.redComeTime - 1
    _winTb.redMyNeedTime = _winTb.redMyNeedTime - 1
    _winTb.redTimeSp.fillAmount = 1 - (_winTb.redComeTime / Red_Coming)
    _winTb.redPro_2.value = 1 - (_winTb.redMyNeedTime / Red_MyTime)
        
end

-- function NormalCowMainRecvRedTimerUpdate(msgID, msgData)
--     -- LogError("NormalCowMainRecvRedTimerUpdate")
--     if UnityTools.CheckMsg(msgID, msgData) then
--         _winTb.redComeTime = msgData.next_open_redpack_second - UtilTools.GetServerTime()
--         _winTb.redMyNeedTime = msgData.next_can_draw_second - UtilTools.GetServerTime()
      
--         -- LogError(_winTb.redMyNeedTime .. "  " .. _winTb.redComeTime)
--         local coolDown = _winTb.redProLb_2.gameObject:GetComponent("CooldownUpdate")
--         _winTb.redTimeSp.fillAmount = 1 - (_winTb.redComeTime / Red_Coming)
--         _winTb.redPro_2.value = 1 - (_winTb.redMyNeedTime / Red_MyTime)
--         coolDown:SetEndTime(msgData.next_can_draw_second)
--         _winTb.titleLb.text = LuaText.normal_red_tip1
--         if _winTb.redMyNeedTime <= 0 then
--             _winTb.redGou_2:SetActive(true)
--             if _winTb.redGou_1.activeSelf == true then
--                 _winTb.titleLb.text = LuaText.normal_red_tip2
--             end
--         else
--             _winTb.redGou_2:SetActive(false)
--         end
--         gTimer.removeTimer(NormalCowMainRedTimerCall)
--         local timer = gTimer.registerRepeatTimer(1000, NormalCowMainRedTimerCall)
--         gTimer.setRecycler(wName, timer)
--     else
--         LogError("Recv nil NormalCowMainRecvRedTimerUpdate")
--     end
-- end


-------------------------------------------------------

function NormalCowMainRecvDealerRateReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        local index = roomMgr.PosToIndex(msgData.pos)
        -- LogError(index .. "NormalCowMainRecvDealerRateReply")
        local rate = msgData.rate_num
        local playerInfo = roomMgr.GetPlayerInfo(index)
        if playerInfo ~= nil then
            playerInfo.master_rate = rate
        end
        local player = _playerList[index]
        setDealerRate(player, rate)
        if _temp_dealer_rate[rate] == nil then 
            _temp_dealer_rate[rate] = {}
        end
        _temp_dealer_rate[rate][#_temp_dealer_rate[rate] + 1] = index
        if rate ~= 0 and playerInfo ~= nil then
            if playerInfo.sex == 0 then
                UnityTools.PlaySound("Sounds/boy/rob", {target = _mainTransfrom.gameObject})
            else
                UnityTools.PlaySound("Sounds/girl/rob", {target = _mainTransfrom.gameObject})
            end
        end
    else
        LogError("Recv nil NormalCowMainRecvDealerRateReply")
    end
end

function NormalCowMainRecvRateReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        local index = roomMgr.PosToIndex(msgData.pos)
        local rate = msgData.rate_num
        setPlayerFreeRate(index, rate)
    else
        LogError("Recv nil NormalCowMainRecvRateReply")
    end
end

function NormalCowMainRecvSubmitReply(msgID, msgData, index)
    if UnityTools.CheckMsg(msgID, msgData) then
        if index == 1 then
            UnityTools.SetActive(_myPokerLayer, false)
        end
        index = index or roomMgr.PosToIndex(msgData.player_pos)
        -- local poker = _pokerList[index]
        local poker = _pokerObjTable[index]
        -- poker:SetActive(true)
        UnityTools.SetActive(poker.parent, true)

        local pokerList,isSpecial = pokerMgr.SortMaxNiuNiu(msgData.card_list)
        for j = 1, 5, 1 do
            -- local pokerInfo = msgData.card_list[j]
            local pokerInfo = pokerList[j]
            if pokerInfo ~= nil then
                pokerMgr.SetPokerIcon(poker[j], pokerInfo)
                if poker[j] ~= nil then
                    if isSpecial == false and j>=4 then
                        poker[j].o.transform.localPosition = UnityEngine.Vector3((j-1)*29+30,poker[j].o.transform.localPosition.y,poker[j].o.transform.localPosition.z)
                    elseif isSpecial == true and j>=4 then
                        poker[j].o.transform.localPosition = UnityEngine.Vector3((j-1)*29,poker[j].o.transform.localPosition.y,poker[j].o.transform.localPosition.z)
                    end
                end
            end
            
        end
        -- LogError(msgData.card_type .. "  " .. index)
        if msgData.card_type ~= nil then
            local type = msgData.card_type
            local pokerType = poker.ptsp  --UnityTools.FindCo(poker.transform, "UISprite", "pokerType")
            local action = poker.pts --pokerType.gameObject:GetComponent("TweenScale")
            -- pokerType.gameObject:SetActive(true)
            UnityTools.SetActive(pokerType, true)
            local pType = type
            if type>=0 and type <=10 then
                pokerType.width = 86
                pokerType.height = 48
            else
                pokerType.width = 126
                pokerType.height = 46
            end
            pokerType.spriteName = "niu_" .. tostring(pType)
            action:ResetToBeginning()
            action.delay = math.random(5, 25)/100
            action:Play(true)

            local pInfo = roomMgr.GetPlayerInfo(index)
            if pInfo ~= nil and pInfo.sex == 0 then
                UnityTools.PlaySound("Sounds/boy/niu_" .. tostring(pType), {target = _mainTransfrom.gameObject})
            else
                UnityTools.PlaySound("Sounds/girl/niu_" .. tostring(pType), {target = _mainTransfrom.gameObject})
            end
            
        end
        
    else
        LogError("Recv nil NormalCowMainRecvSubmitReply")
    end
end


function NormalCowMainUpdateTaskRed(msgId, type)
    if type == "task" then
        updateTaskRed()
    end
end

local function getGameTimes()
    return _gameTimes
end

function ResetNormalCowWinRenderQ(go)
    if UnityTools.IsWinShow(wName) == false or _clickClosed == true or _mainUIPanel == nil then return nil end
    local renderQ = _mainUIPanel.startingRenderQueue + 23
    if _goldPreMeshRender ~= nil then
        _goldPreMeshRender.sharedMaterial.renderQueue = renderQ
    end
    
end

UI.Controller.UIManager.RegisterLuaWinRenderFunc("NormalCowMain", ResetNormalCowWinRenderQ)
-- UI.Controller.UIManager.RegisterLuaFuncCall("OnApplicationFocus", closeFunc)

-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable
M.showOperate = showOperate
M.GetGameTimes = getGameTimes
M.ClickRestartCall = ClickRestartCall
M.CloseWin = closeFunc
-- 返回当前模块
return M
