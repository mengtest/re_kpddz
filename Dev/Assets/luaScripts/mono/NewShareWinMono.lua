-- -----------------------------------------------------------------
-- * Copyright (c) 2018 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    NewShareWinMono.lua
-- * Summary:     NewShareWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        1/25/2018 4:43:21 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NewShareWinMono")



-- 界面名称
local wName = "NewShareWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local protobuf=sluaAux.luaProtobuf.getInstance()

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local shareCtrl = IMPORT_MODULE("ShareWinController")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local LottoCenterDesc = nil

local pageBtnPanel = nil

local PagesLb = nil

local totalPage = 1

local curPage = 1

local _SearchFriendInput = nil

local _RedPacketLb = nil

local redPacketPanel = nil
local RedPacketPanelSprite = nil
local topTabPanel = nil

local topTitlePanel = nil

local bottomPanel = nil

local LottoCountLb = nil

local topTabBtns = {}

local leftTabBtns = {}

local rewardIcons = {}

local curLeftTab= 1

local curTopTab = 1

local flashLight = nil

local isLotto = false

local curLottoIndex = 1

local clickLabel = nil

local NewUserTip = nil

local rightPanels = {} 

local QRCodeMask = nil

local _ShareRankCellMgr = nil

local _ShareHistoryCellMgr = nil

local _tShareRanks = {}

local _tShareHistorys = {}

local _SearchFriendHistory = {}

local _ShareRankScrollView = nil

local _ShareHistoryScrollView = nil

local isSearch = false

local effect = nil
-- local _shareScrollview
local _btnShareQQ
local _btnShareWeBo
local _maskPanel
local function UpdateShareRankList()
    local tAllHistory = {}

    local StartIndex = 1 + (curPage - 1) * 7
    local EndIndex = 7 + (curPage - 1) * 7

    for i = StartIndex,EndIndex do
        table.insert(tAllHistory, _tShareRanks[i])
    end

    local delCount = _ShareRankCellMgr.CellCount - #tAllHistory
    
    if delCount>=0 then
        for i=1,delCount do
            _ShareRankCellMgr:DelteLastNode()
        end
        _ShareRankCellMgr.Grid:Reposition()
        _ShareRankCellMgr:UpdateCells()
    else
        _ShareRankCellMgr:ClearCells()
        for i=1,#tAllHistory do
            _ShareRankCellMgr:NewCellsBox(_ShareRankCellMgr.Go)
                --mainScrollView:Reposition()
        end
        _ShareRankCellMgr.Grid:Reposition()
        _ShareRankCellMgr:UpdateCells()
    end
    _ShareRankScrollView:ResetPosition()
end

local function UpdateShareHistoryList()
    local tAllHistory = {}

    if isSearch then
        tAllHistory = _SearchFriendHistory
    else
        local StartIndex = 1 + (curPage - 1) * 7
        local EndIndex = 7 + (curPage - 1) * 7
    
        for i = StartIndex,EndIndex do
            table.insert(tAllHistory, _tShareHistorys[i])
        end
    end

    local delCount = _ShareHistoryCellMgr.CellCount - #tAllHistory
    
    if delCount>=0 then
        for i=1,delCount do
            _ShareHistoryCellMgr:DelteLastNode()
        end
        _ShareHistoryCellMgr.Grid:Reposition()
        _ShareHistoryCellMgr:UpdateCells()
    else
        _ShareHistoryCellMgr:ClearCells()
        for i=1,#tAllHistory do
            _ShareHistoryCellMgr:NewCellsBox(_ShareHistoryCellMgr.Go)
                --mainScrollView:Reposition()
        end
        _ShareHistoryCellMgr.Grid:Reposition()
        _ShareHistoryCellMgr:UpdateCells()
    end
    _ShareHistoryScrollView:ResetPosition()
end

local function UpdateRedPoint()
    local red = UnityTools.FindGo(topTabPanel.transform, "Tab_2/red")
    local redSeven = UnityTools.FindGo(topTabPanel.transform, "Tab_3/red")
    local redOne = UnityTools.FindGo(topTabPanel.transform, "Tab_1/red")
    if redOne ~= nil then
        if CTRL.lottoCountOne > 0 then
            redOne:SetActive(true)
        else
            redOne:SetActive(false)
        end
    end
    if CTRL.lottoCount > 0 then
        red:SetActive(true)
    else
        red:SetActive(false)
    end

    if CTRL.lottoCountSeven > 0 then
        redSeven:SetActive(true)
    else
        redSeven:SetActive(false)
    end
end


local function SetTabBtnStatus(gameObject, isSelected)

    local normal = UnityTools.FindGo(gameObject.transform, "Normal")
    local selected = UnityTools.FindGo(gameObject.transform, "Selected")
    local boxCollider = gameObject:GetComponent("BoxCollider")
    if isSelected then
        normal:SetActive(false)
        selected:SetActive(true)
        boxCollider.enabled = false
    else    
        normal:SetActive(true)
        selected:SetActive(false)
        boxCollider.enabled = true
    end
end

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local function SetIconAndNum(transform,item1_id,num)
    LogWarn(tostring("item_id=" .. tostring(item1_id) .. ",num=" .. tostring(num)))
    local sp = UnityTools.FindGo(transform, "img"):GetComponent("UISprite")
    sp.spriteName = "C"..item1_id
    local lbNum = UnityTools.FindGo(transform, "num"):GetComponent("UILabel")
    lbNum.text = num/10 .. "元"
end

local function SetRewardIconValue(index)
    local rewardData = LuaConfigMgr.ShareRedpackConfig[tostring(index)]

    if rewardData ~= nil then
        local rewards = rewardData.reward2
        for i,v in pairs(rewardIcons) do
            local reward = rewards[i]
            if reward ~= nil then
                LogWarn("name=" .. tostring(v.name))
                local Normal = UnityTools.FindGo(v.transform, "Normal")
                local Selected = UnityTools.FindGo(v.transform, "Selected")
                SetIconAndNum(Normal.transform, tonumber(reward[2]), tonumber(reward[3]))
                SetIconAndNum(Selected.transform, tonumber(reward[2]), tonumber(reward[3]))
            end
        end
    end
end

local function recoverLottoIcon()
    LogWarn("recoverLottoIcon curLottoIndex=" .. curLottoIndex)
    if curLottoIndex > 0 then
        LogWarn("recoverLottoIcon-------------2")
        local icon = rewardIcons[curLottoIndex]
        icon.transform.localScale = UnityEngine.Vector3(1, 1, 1)
        local selected = UnityTools.FindGo(icon.transform, "Selected/box"):GetComponent("UISprite")
        selected.alpha = 0
    end
end

local function OnLottoClick(gameObject)
    if isLotto then
        return
    end

    LogWarn("OnLottoClick")

    if curTopTab == 2 and CTRL.lottoCount < 1 then
        UnityTools.MessageDialog("次数不足，分享给好友可获得更多次数");
        return
    end

    if curTopTab == 3 and CTRL.lottoCountSeven < 1 then
        UnityTools.MessageDialog("次数不足，分享给好友可获得更多次数");
        return
    end
    if curTopTab == 1 and CTRL.lottoCountOne < 1 then
        UnityTools.MessageDialog("次数不足，分享给好友可获得更多次数");
        return
    end
    local tMsg = {}
    tMsg.flag = curTopTab
    protobuf:sendMessage(protoIdSet.cs_share_draw_request, tMsg)     

    if curTopTab == 1 then
        LottoCountLb.text = CTRL.lottoCountOne - 1
    elseif curTopTab == 2 then
        LottoCountLb.text = CTRL.lottoCount - 1
    elseif curTopTab == 3 then
        LottoCountLb.text = CTRL.lottoCountSeven - 1
    end

end

local function OnScaleQRCodeClick(gameObject)
    QRCodeMask:SetActive(true)
    local QRcodeIcon = UnityTools.FindGo(QRCodeMask.transform, "Texture"):GetComponent("UITexture")
    BarcodeCam.getInstance().SetQRIToUITexture(QRcodeIcon.transform);
    -- UtilTools.LoadUrlPoc(QRcodeIcon, GameDataMgr.PLAYER_DATA.SharePicUrl)
end

local function OnNewUserTipBtnClick(gameObject)
    local NewUserTip = UnityTools.FindGo(rightPanels[1].transform, "LottoCenter/NewUserTip")
    NewUserTip:SetActive(true)
    local tweenPos = NewUserTip:GetComponent("TweenPosition")
    tweenPos:ResetToBeginning()
    tweenPos.enabled = true
end

local function UpdatePageLb()
    if totalPage == 0 then
        pageBtnPanel:SetActive(false)
    else
        pageBtnPanel:SetActive(true)
    end
    PagesLb.text = curPage .. "/" .. totalPage
end

local function OnNextPageClick(gameObject)
    if curPage >= totalPage then
        return
    end

    curPage = curPage + 1
    if curLeftTab == 2 then
        local max = curPage * 7
        if max <= #_tShareRanks then
            UpdatePageLb()
            UpdateShareRankList()
        else
            local tMsg = {}
            tMsg.page = curPage
            protobuf:sendMessage(protoIdSet.cs_share_rank_request, tMsg)     
        end
       
    elseif curLeftTab == 4 then
        local max = curPage * 7
        if max <= #_tShareHistorys then
            UpdatePageLb()
            UpdateShareHistoryList()
        else
            local tMsg = {}
            tMsg.page = curPage
            protobuf:sendMessage(protoIdSet.cs_share_friend_request, tMsg)    
        end
    end
end

local function OnpreviousPageClick(gameObject)
    if curPage == 1 then
        return
    end

    curPage = curPage - 1
    if curLeftTab == 2 then
        UpdatePageLb()
        UpdateShareRankList()
        -- local tMsg = {}
        -- tMsg.page = curPage
        -- protobuf:sendMessage(13116, tMsg)     
    elseif curLeftTab == 4 then
        UpdatePageLb()
        UpdateShareHistoryList()
        -- local tMsg = {}
        -- tMsg.page = curPage
        -- tMsg.userId = "1"
        -- protobuf:sendMessage(13115, tMsg)    
    end
end

local function OnQRCodeMaskClick(gameObject)
    QRCodeMask:SetActive(false)
end

local function OnShareBtnClick(gameObject)
    NewUserTip:SetActive(false)
    topTitlePanel:SetActive(false)
    topTabPanel:SetActive(false)
    bottomPanel:SetActive(false)
    redPacketPanel:SetActive(false)

    rightPanels[curLeftTab]:SetActive(false)
    rightPanels[5]:SetActive(true)
    
    local linkLb = UnityTools.FindGo(rightPanels[5].transform, "WebLink/Bg/Label"):GetComponent("UILabel")
    linkLb.text =  string.gsub(GameDataMgr.PLAYER_DATA.ShareURL, "\\/", "/") 


    local QRcodeIcon = UnityTools.FindGo(rightPanels[5].transform, "QRcode/Bg/Icon"):GetComponent("UITexture")
    BarcodeCam.getInstance().SetQRIToUITexture(QRcodeIcon.transform);
    -- UtilTools.LoadUrlPoc(QRcodeIcon, GameDataMgr.PLAYER_DATA.SharePicUrl)
end

local function ShowNewUserTip(gameObject)
    NewUserTip:SetActive(true)
end

local function OnNewUserTipClose(gameObject)
    NewUserTip:SetActive(false)
    local NewUserTip_ = UnityTools.FindGo(rightPanels[1].transform, "LottoCenter/NewUserTip")
    NewUserTip_:SetActive(false)
end

local function showRuleText()
    
    local RuleText = UnityTools.FindGo(rightPanels[3].transform, "ScrollView/Label"):GetComponent("UILabel")

    local maxRewardNum = 0
    local getTelCostValue = 0
    local startDay = tonumber(os.date("%d", CTRL.beginTime))
    local data = LuaConfigMgr.ShareRedpackConfig[tostring(1)]
    if data ~= nil then
        getTelCostValue = tonumber(data.reward2[1][3]) * 0.01
    end

    local firstDayReward = 0
    local threeDayReward = 0
    local sevenDayReward = 0

    local rewardData_1 = LuaConfigMgr.LoginDaytaskConfig[tostring(560001)]
    if rewardData_1 ~= nil then
        firstDayReward = rewardData_1.item1_num * 0.1
    end
    local rewardData_3 = LuaConfigMgr.LoginDaytaskConfig[tostring(560003)]
    if rewardData_3 ~= nil then
        threeDayReward = rewardData_3.item1_num * 0.1
    end
    local rewardData_7 = LuaConfigMgr.LoginDaytaskConfig[tostring(560007)]
    if rewardData_7 ~= nil then
        sevenDayReward = rewardData_7.item1_num * 0.1
    end

    -- local str = ""

    -- for i=1,10 do
    --     local shareRankData = LuaConfigMgr.ShareRankingConfig[tostring(i)]
    --     if shareRankData ~= nil then
    --         if i == 1 then
    --             maxRewardNum = shareRankData.item_reward[1][3] * 0.01
    --         end
    --         str = str .. LuaText.GetStr(LuaText.rule_reward_text, i, shareRankData.item_reward[1][3] * 0.01)
    --     end
    -- end

    -- local shareRankData = LuaConfigMgr.ShareRankingConfig[tostring(11)]
    -- if shareRankData ~= nil then
    --     str = str .. LuaText.GetStr(LuaText.rule_reward_text, "11-20", shareRankData.item_reward[1][3] * 0.01)
    -- end

    -- local shareRankData = LuaConfigMgr.ShareRankingConfig[tostring(21)]
    -- if shareRankData ~= nil then
    --     str = str .. LuaText.GetStr(LuaText.rule_reward_text, "21-30", shareRankData.item_reward[1][3] * 0.01)
    -- end

    -- local shareRankData = LuaConfigMgr.ShareRankingConfig[tostring(31)]
    -- if shareRankData ~= nil then
    --     str = str .. LuaText.GetStr(LuaText.rule_reward_text, "31-50", shareRankData.item_reward[1][3] * 0.01)
    -- end

    RuleText.text = LuaText.GetStr(LuaText.rule_content_1) .. LuaText.GetStr(LuaText.rule_content_2_1, firstDayReward, getTelCostValue) 
                                            .. LuaText.GetStr(LuaText.rule_content_2_2, threeDayReward) 
                                                .. LuaText.GetStr(LuaText.rule_content_2_3, sevenDayReward)
                                                        --  ..LuaText.GetStr(LuaText.rule_content_2_4, maxRewardNum) 
                                                                .. LuaText.GetStr(LuaText.rule_content_4, threeDayReward) 
                                                                .. LuaText.GetStr(LuaText.rule_content_5) .. LuaText.GetStr(LuaText.rule_content_7)
                                                                --   ..LuaText.GetStr(LuaText.rule_content_8) .. LuaText.GetStr(LuaText.rule_content_9) 
                                                                --  .. str

    local ScrollView = UnityTools.FindGo(rightPanels[3].transform, "ScrollView"):GetComponent("UIScrollView")
    ScrollView:ResetPosition()
end

local function OnLeftTabClick(gameObject)

    if curLeftTab == 1 then
        local NewUserTip_ = UnityTools.FindGo(rightPanels[1].transform, "LottoCenter/NewUserTip")
        NewUserTip_:SetActive(false)
    end
    
    curPage = 1
    NewUserTip:SetActive(false)
    rightPanels[5]:SetActive(false)
    local lastSelected = leftTabBtns[curLeftTab]
    SetTabBtnStatus(lastSelected, false)
    rightPanels[curLeftTab]:SetActive(false)

    SetTabBtnStatus(gameObject, true)
    curLeftTab = ComponentData.Get(gameObject).Tag
    rightPanels[curLeftTab]:SetActive(true)
    if curLeftTab == 1 then
        redPacketPanel:SetActive(true)
        topTitlePanel:SetActive(false)
        topTabPanel:SetActive(true)
        bottomPanel:SetActive(false)
    elseif curLeftTab == 2 then
        redPacketPanel:SetActive(false)
        topTitlePanel:SetActive(true)
        topTabPanel:SetActive(false)
        _tShareRanks = {}
        local tMsg = {}
        tMsg.page = 1
        protobuf:sendMessage(protoIdSet.cs_share_rank_request, tMsg)     
    elseif curLeftTab == 3 then
        redPacketPanel:SetActive(false)
        topTitlePanel:SetActive(false)
        bottomPanel:SetActive(false)
        topTabPanel:SetActive(false)

        if CTRL.beginTime > 0 then
            showRuleText()
        else
            local tMsg = {}
            tMsg.page = 1
            protobuf:sendMessage(protoIdSet.cs_share_rank_request, tMsg)    
        end
        
    elseif curLeftTab == 4 then
        redPacketPanel:SetActive(false)
        topTabPanel:SetActive(false)
        topTitlePanel:SetActive(false)
        _tShareHistorys = {}
        local tMsg_ = {}
        tMsg_.page = 1
        protobuf:sendMessage(protoIdSet.cs_share_friend_request, tMsg_)     
    end

end

local function updateLottoCenter(curTopTab)
    local taskData = LuaConfigMgr.LoginDaytaskConfig
    
    if curTopTab == 1 then
        LottoCountLb.text = CTRL.lottoCountOne
        local taskData = LuaConfigMgr.LoginDaytaskConfig["560001"]
        if taskData ~= nil then
            LottoCenterDesc.text = LuaText.GetStr(LuaText.share_lotto_desc, "一", taskData.item1_num * 0.1)
        end
    elseif curTopTab == 2 then
        LogWarn("lb=" .. tostring(LottoCountLb) .. ",count=" .. tostring(CTRL.lottoCountSeven))
        LottoCountLb.text = CTRL.lottoCount
        local taskData = LuaConfigMgr.LoginDaytaskConfig["560003"]
        if taskData ~= nil then
            LottoCenterDesc.text = LuaText.GetStr(LuaText.share_lotto_desc, "三", taskData.item1_num * 0.1)
        end
    elseif curTopTab == 3 then
        LottoCountLb.text = CTRL.lottoCountSeven
        local taskData = LuaConfigMgr.LoginDaytaskConfig["560007"]
        if taskData ~= nil then
            LottoCenterDesc.text = LuaText.GetStr(LuaText.share_lotto_desc, "七", taskData.item1_num * 0.1)
        end
    end

end

local function updateRedPacket()
    local _itemMgr = IMPORT_MODULE("ItemMgr")
    _RedPacketLb.text = string.format("%.1f", _itemMgr.GetItemNum(109)/10)
    -- if curTopTab == 1 then
    --     _RedPacketLb.text = string.format("%.2f", GameDataMgr.PLAYER_DATA.NewRedPacket/100)
    -- elseif curTopTab == 2 then
    --     _RedPacketLb.text = string.format("%.2f", GameDataMgr.PLAYER_DATA.NewRedPacket/100)
    -- elseif curTopTab == 3 then
    --     _RedPacketLb.text = string.format("%.2f", GameDataMgr.PLAYER_DATA.RMB)
    -- end
    
end

local function OnTopTabClick(gameObject)
    if isLotto then
        return
    end
    local SelectId = ComponentData.Get(gameObject).Tag
    for i=1,#topTabBtns do
        if SelectId == i then
            SetTabBtnStatus(gameObject, true)
        else
            SetTabBtnStatus(topTabBtns[i], false)
        end
    end
    -- local lastSelected = topTabBtns[curTopTab]
    -- SetTabBtnStatus(lastSelected, false)
    
    -- SetTabBtnStatus(gameObject, true)
    curTopTab = ComponentData.Get(gameObject).Tag

    SetRewardIconValue(curTopTab)

    recoverLottoIcon()
    curLottoIndex = 1

    updateLottoCenter(curTopTab)
    
end

local isPlayEffect = false
local totalTime = 0
local timeSpeed = 1
local playedTime = 0

local function OnPlayEffectFinish()
    isPlayEffect = false
    timeSpeed = 1
    playedTime = 0 
    effect:SetActive(false)

    isLotto = false

    local rewardData = LuaConfigMgr.ShareRedpackConfig[tostring(curTopTab)]
    if rewardData ~= nil then
        ShowAwardWin({{base_id = rewardData.reward2[curLottoIndex][2], count = tonumber(rewardData.reward2[curLottoIndex][3])/10}})
    end
    
    updateRedPacket()

    if curTopTab == 1 then
        LottoCountLb.text = CTRL.lottoCountOne
    elseif curTopTab == 2 then
        LottoCountLb.text = CTRL.lottoCount
    elseif curTopTab == 3 then
        LottoCountLb.text = CTRL.lottoCountSeven
    end
end

local function StartPlayEffect()
    totalTime = UnityEngine.Vector3.Distance(rewardIcons[curLottoIndex].transform.localPosition, UnityEngine.Vector3(254, 212, 0)) / 400
    effect.transform.localPosition = rewardIcons[curLottoIndex].transform.localPosition
    effect:SetActive(true)
    isPlayEffect = true
end


local function OnFlashLightFinish()
    LogWarn("----OnFlashLightFinish-----")
    -- isLotto = false

    -- local rewardData = LuaConfigMgr.ShareRedpackConfig[tostring(curTopTab)]
    -- if rewardData ~= nil then
    --     local itemIdList = {rewardData.reward2[curLottoIndex][2]}
    --     local itemCountList = {rewardData.reward2[curLottoIndex][3]}
    --     UtilTools.ShowAwardWinWithLua(itemIdList, itemCountList, 0)
    -- end

    StartPlayEffect()
end

local function GoToTask(url)
    NewUserTip:SetActive(true)
end

local function OnSearchFriendFinish()
    local userId = _SearchFriendInput.value 
    if userId ~= nil  then
        local tMsg = {}
        if userId == "" then
            UpdateShareHistoryList()
            UpdatePageLb()
            return
        end
    end
end

local function OnSearchFriendCommit()
    local userId = _SearchFriendInput.value 
    if userId ~= nil  then
        local tMsg = {}
        if userId == "" then
            LogWarn("userId=1-----")
            UpdateShareHistoryList()
            UpdatePageLb()
            return
        else
            LogWarn("userId=2-----")
            isSearch = true
            tMsg.page = 0
            tMsg.user_id = tostring(userId)
        end
        UtilTools.ShowWaitFlag()
        protobuf:sendMessage(protoIdSet.cs_share_friend_request, tMsg)
    end
end

local function OnSearchFriendBtnClick(gameObject)
    OnSearchFriendCommit()
end

local function OnCopyWebLinkBtnClick(gameObject)
    local root = UnityEngine.GameObject.Find("UIRoot")
    local jarUtilTools = UnityTools.FindGo(root.transform, "UICamera"):GetComponent("JARUtilTools")
    jarUtilTools:copyString2Clipboard(string.gsub(GameDataMgr.PLAYER_DATA.ShareURL, "\\/", "/") )
    UtilTools.ShowMessage("复制成功","[FFFFFF]")  

end

local function OnShareToWeChatFriendClick(gameObject)
    CTRL.ShareToWeChatFriend()
end
local function OnShareToWeBoFriendClick(gameObject)
    CTRL.ShareToWeBoFriend()
end
local function OnShareToWeQQFriendClick(gameObject)
    CTRL.ShareToWeQQFriend()
end


local function OnShareToMomentsClick(gameObject)
    CTRL.ShareToMoments()
    -- local descStr = "new_share_desc"..math.random(1,10)
    -- local picCount = BarcodeCam.getInstance().GetPicListLenth()
    -- local selecetIndex = math.random(2,picCount-1)
    -- UtilTools.ShareWeChatPic(1, tostring(GameDataMgr.PLAYER_DATA.UserId), GameText.GetStr(descStr),BarcodeCam.getInstance().GetSharePic(1),BarcodeCam.getInstance().GetSharePic(2),BarcodeCam.getInstance().GetSharePic(selecetIndex),"","","")
end


local function initPirateFlashLight(gameObject)
    flashLight = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/RewardPanel"):GetComponent("NewShareCircle")
    local len = #rewardIcons
    for i = 1,len do
        local seletedBg = UnityTools.FindGo(rewardIcons[i].transform, "Selected/box"):GetComponent("UISprite")
        local TweenAlpha = UnityTools.FindGo(rewardIcons[i].transform, "Selected/box"):GetComponent("TweenAlpha")
        flashLight:AddTweenAlpha(TweenAlpha, seletedBg, rewardIcons[i])
        seletedBg.alpha = 0
    end
    flashLight:SetParam(0.2, 4)
    EventDelegate.Add(flashLight.onComplete,OnFlashLightFinish)
end

function OnNewShareShareRankResponse(eventId, tMsg)
    LogWarn("OnNewShareShareRankResponse")
    if curLeftTab == 3 then
        showRuleText()
        return
    end
    if curLeftTab ~= 2 then
        return
    end
    local list = tMsg.list or {}
    for k,v in pairs(list) do
        table.insert(_tShareRanks, v)
    end
    UpdateShareRankList()

    local MyShareFriendLb = UnityTools.FindGo(topTitlePanel.transform, "MyShareFriend/Label"):GetComponent("UILabel")
    local MyRankLb = UnityTools.FindGo(topTitlePanel.transform, "MyRank/Label"):GetComponent("UILabel")
    MyShareFriendLb.text = tostring(tMsg.count)
    if tMsg.rank == 0 then
        MyRankLb.text = "未上榜"
    else
        MyRankLb.text = tostring(tMsg.rank)
    end
    

    local startTime = os.date("%Y.%m.%d", tMsg.beginTime) 
    local endTime = os.date("%Y.%m.%d", tMsg.endTime) 
    local timeLb = UnityTools.FindGo(rightPanels[2].transform, "BottomPanel/TimePeriodLb"):GetComponent("UILabel")
    timeLb.text = "本月周期: " .. startTime .. "--" .. endTime

    totalPage = tMsg.pages
    -- if totalPage == 0 then
    --     pageBtnPanel:SetActive(false)
    -- else
    --     pageBtnPanel:SetActive(true)
    -- end
    -- PagesLb.text = curPage .. "/" .. totalPage
    UpdatePageLb()

end

function OnNewShareHistoryResponse(eventId, tMsg)
    LogWarn("OnNewShareHistoryResponse")
   
    if curLeftTab ~= 4 then
        isSearch = false
        return
    end

    if isSearch then
        _SearchFriendHistory = tMsg.list or {}
        UpdateShareHistoryList()
        pageBtnPanel:SetActive(false)
        isSearch = false
        return
    end
    local list = tMsg.list or {}
    for k,v in pairs(list) do
        table.insert(_tShareHistorys, v)
    end

    UpdateShareHistoryList()

    local MyShareFriendLb = UnityTools.FindGo(rightPanels[4].transform, "TopPanel/MyShareFriend/Label"):GetComponent("UILabel")
    
    local GetTelCostLb = UnityTools.FindGo(rightPanels[4].transform, "TopPanel/GetTelCost/Label"):GetComponent("UILabel")
    local ThreeLottoCountLb = UnityTools.FindGo(rightPanels[4].transform, "TopPanel/ThreeLottoCount/Label"):GetComponent("UILabel")
    local SevenLottoCountLb = UnityTools.FindGo(rightPanels[4].transform, "TopPanel/SevenLottoCount/Label"):GetComponent("UILabel")

    MyShareFriendLb.text = tMsg.count
    ThreeLottoCountLb.text = tMsg.threeDraw
    SevenLottoCountLb.text = tMsg.sevenDraw

    local data = LuaConfigMgr.ShareRedpackConfig[tostring(1)]
    if data ~= nil then
        GetTelCostLb.text = tMsg.oneDraw --* tonumber(data.reward2[1][3]) * 0.01
    end

    totalPage = tMsg.pages
    -- if totalPage == 0 then
    --     pageBtnPanel:SetActive(false)
    -- else
    --     pageBtnPanel:SetActive(true)
    -- end
    -- PagesLb.text = curPage .. "/" .. totalPage
    UpdatePageLb()
end

function OnNewShareLottoResponse(eventId, index)
    LogWarn("OnNewShareLottoResponse")
    if index == nil then
        return
    end

    recoverLottoIcon()
    flashLight:StartCircle(curLottoIndex - 1, index - 1, 6)
    curLottoIndex = index
    isLotto = true
    UpdateRedPoint()
end

local function OnShowItem2(cellbox, index, item)
    local id = index + 1 + (curPage - 1) * 7
    local data = _tShareHistorys[id]

    if isSearch then
        id = index + 1
        data = _SearchFriendHistory[index + 1]
    end
    
    local OrderLb = UnityTools.FindGo(item.transform, "OrderLb"):GetComponent("UILabel")
    
    if id < 10 then
        OrderLb.text = "0" .. tostring(id)
    else
        OrderLb.text = tostring(id)
    end

    local IDLb = UnityTools.FindGo(item.transform, "IDLb"):GetComponent("UILabel")
    IDLb.text = data.userId
    local NickName = UnityTools.FindGo(item.transform, "NickName"):GetComponent("UILabel")
    NickName.text = data.name
    local CreateTimeLb = UnityTools.FindGo(item.transform, "CreateTimeLb"):GetComponent("UILabel")
    CreateTimeLb.text = os.date("%m-%d", data.create_time)
    local FirstDayRewardLb = UnityTools.FindGo(item.transform, "FirstDayRewardLb"):GetComponent("UILabel")
    if data.first_day == 0 then
        FirstDayRewardLb.text = "[ff4d40]未领取[-]"
        FirstDayRewardLb.effectColor = UnityEngine.Color(25/255,79/255,3/255)--[194f03]
    elseif data.first_day == 1 then
        FirstDayRewardLb.text = "[81ff2d]已领取[-]" -- 
        FirstDayRewardLb.effectColor = UnityEngine.Color(97/255,30/255,11/255)--[611e0b]
    end
    local ThreeDayRewardLb = UnityTools.FindGo(item.transform, "ThreeDayRewardLb"):GetComponent("UILabel")
    if data.three_day == 0 then
        ThreeDayRewardLb.text = "[ff4d40]未领取[-]"
        ThreeDayRewardLb.effectColor = UnityEngine.Color(25/255,79/255,3/255)--[194f03]
    elseif data.three_day == 1 then
        ThreeDayRewardLb.text = "[81ff2d]已领取[-]"
        ThreeDayRewardLb.effectColor = UnityEngine.Color(97/255,30/255,11/255)--[611e0b]
    end
    local SevenDayRewardLb = UnityTools.FindGo(item.transform, "SevenDayRewardLb"):GetComponent("UILabel")
    if data.seven_day == 0 then
        SevenDayRewardLb.text = "[ff4d40]未领取[-]"
        SevenDayRewardLb.effectColor = UnityEngine.Color(25/255,79/255,3/255)--[194f03]
    elseif data.seven_day == 1 then
        SevenDayRewardLb.text = "[81ff2d]已领取[-]"
        SevenDayRewardLb.effectColor = UnityEngine.Color(97/255,30/255,11/255)--[611e0b]
    end
    local ExchangeLb = UnityTools.FindGo(item.transform, "ExchangeLb"):GetComponent("UILabel")
    if data.is_red == 0 then
        ExchangeLb.text = "[ff4d40]未兑换[-]"
        ExchangeLb.effectColor = UnityEngine.Color(25/255,79/255,3/255)--[194f03]
    elseif data.is_red == 1 then
        ExchangeLb.text = "[81ff2d]已兑换[-]"
        ExchangeLb.effectColor = UnityEngine.Color(97/255,30/255,11/255)--[611e0b]
    end
end

local cupNames = {"rank1", "rank2", "rank3"}

local function OnShowItem(cellbox, index, item)
    local index = index + 1 + (curPage - 1) * 7
    local data = _tShareRanks[index]

    local nickName = UnityTools.FindGo(item.transform, "NickName"):GetComponent("UILabel")
    nickName.text = data.name
    LogWarn("name=" .. tostring(data.name))

    local shareCount = UnityTools.FindGo(item.transform, "ShareCountNum"):GetComponent("UILabel")
    shareCount.text = data.count

    -- local rewardNumLb = UnityTools.FindGo(item.transform, "RewardNum"):GetComponent("UILabel")

    -- local shareRankData = LuaConfigMgr.ShareRankingConfig[tostring(data.rank)]
    -- if shareRankData ~= nil then
    --     local rewardNum = tonumber(shareRankData.item_reward[1][3])
    --     if rewardNum ~= nil then
    --         rewardNumLb.text = "[fffc2a]" .. rewardNum * 0.01 .. "[-]" .. "元红包奖励"
    --     else
    --         rewardNumLb.text = ""
    --     end
        
    -- end

    local orderNmu = data.rank
    local cupIcon = UnityTools.FindGo(item.transform, "RankItem/cupIcon"):GetComponent("UISprite")
    local rankLb = UnityTools.FindGo(item.transform, "RankItem/Label"):GetComponent("UILabel")
    if orderNmu < 4 and orderNmu > 0 then
        cupIcon.gameObject:SetActive(true)
        rankLb.gameObject:SetActive(false)
        cupIcon.spriteName = cupNames[orderNmu]
    else
        cupIcon.gameObject:SetActive(false)
        rankLb.gameObject:SetActive(true)
        -- if orderNmu < 10 then
        --     rankLb.text = "0" .. tostring(index)
        -- else
           
        -- end
        rankLb.text = tostring(index)
    end
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)

    _SearchFriendInput = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/MySharePanel/BottomPanel/SearchPanel/InputBg/Label"):GetComponent("UIInput")
    EventDelegate.Add(_SearchFriendInput.onSubmit, OnSearchFriendCommit)
    EventDelegate.Add(_SearchFriendInput.onChange, OnSearchFriendFinish)

    local SearchFriendBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/MySharePanel/BottomPanel/SearchPanel/Icon")
    UIEventListener.Get(SearchFriendBtn).onClick = OnSearchFriendBtnClick

    local CopyWebLinkBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/WebLink/Bg/CopyBtn")
    UIEventListener.Get(CopyWebLinkBtn).onClick = OnCopyWebLinkBtnClick

    local ShareToMoments = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/WeChatMoments")
    UIEventListener.Get(ShareToMoments).onClick = OnShareToMomentsClick

    local ShareToWeChatFriend = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/WeChat")
    UIEventListener.Get(ShareToWeChatFriend).onClick = OnShareToWeChatFriendClick
    -- local btnShareQQ = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/WeQQ")
    -- UIEventListener.Get(btnShareQQ).onClick = OnShareToWeQQFriendClick
    -- local btnShareWeBo = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/WeBo")
    -- UIEventListener.Get(btnShareWeBo).onClick = OnShareToWeBoFriendClick

    -- _shareScrollview = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/list/scrollview")

    redPacketPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RedPacketPanel")
    RedPacketPanelSprite = UnityTools.FindGo(gameObject.transform, "Container/Background/RedPacketPanel/Sprite"):GetComponent("UISprite")
    topTabPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/TopPanel")
    topTitlePanel = UnityTools.FindGo(gameObject.transform, "Container/Background/TopTitlePanel")
    bottomPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/BottomPanel")
    
    -- NewShareWin(Clone)/Container/Background/RightPanel/LottoPanel/LottoCenter/LottoBtn/Num
    LottoCountLb = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/LottoCenter/LottoBtn/Num"):GetComponent("UILabel")
    LottoCenterDesc = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/LottoCenter/desc"):GetComponent("UILabel")
    local CloseBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/CloseBtn")
    UIEventListener.Get(CloseBtn).onClick = CloseWin

    local mask = UnityTools.FindGo(gameObject.transform, "mask")
    UIEventListener.Get(mask).onClick = OnNewUserTipClose

    local CloseNewUserTip = UnityTools.FindGo(gameObject.transform, "Container/Background/Panel/NewUserTip/Close")
    UIEventListener.Get(CloseNewUserTip).onClick = OnNewUserTipClose
    -- local startX = 147
    for i=1,3 do
        local topTabBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/TopPanel/Tab_" .. i)
        -- local spNormal =   UnityTools.FindGo(topTabBtn.transform, "Normal/Sprite"):GetComponent("UISprite")
        -- local spSelect =   UnityTools.FindGo(topTabBtn.transform, "Selected/Sprite"):GetComponent("UISprite")
        -- local redObj =   UnityTools.FindGo(topTabBtn.transform, "red")
        -- local boxC = topTabBtn:GetComponent("BoxCollider")
        -- ComponentData.Get(topTabBtn).Tag = i
        -- spNormal.width = 150
        -- spSelect.width = 150
        -- boxC.size = UnityEngine.Vector3(150,54)
        -- redObj.transform.localPosition = UnityEngine.Vector3(69,21)
        UIEventListener.Get(topTabBtn).onClick = OnTopTabClick
        table.insert(topTabBtns, topTabBtn)
        -- topTabBtn.transform.localPosition = UnityEngine.Vector3(startX+(i-1)*177,0,0)
    end
    -- local newBtn = NGUITools.AddChild(topTabPanel.gameObject, topTabBtns[1].gameObject);
    -- newBtn.gameObject.name = "Tab_0"
    -- local lbOne =   UnityTools.FindGo(newBtn.transform, "Selected/Label"):GetComponent("UILabel")
    -- lbOne.text = "1日奖励"
    -- lbOne =   UnityTools.FindGo(newBtn.transform, "Normal/Label"):GetComponent("UILabel")
    -- lbOne.text = "1日奖励"
    -- newBtn.transform.localPosition = UnityEngine.Vector3(147-177,0,0)
    -- ComponentData.Get(newBtn).Tag = 3
    -- UIEventListener.Get(newBtn).onClick = OnTopTabClick
    
    -- table.insert(topTabBtns, newBtn)
    for i=1,4 do
        local leftTabBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/LeftPanel/Tab_" .. i)
        UIEventListener.Get(leftTabBtn).onClick = OnLeftTabClick
        table.insert(leftTabBtns, leftTabBtn)
    end

    for i=1,14 do
        local rewardIcon = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/RewardPanel/Reward_" .. i)
        table.insert(rewardIcons, rewardIcon)
    end

    local LottoBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/LottoCenter/LottoBtn")
    UIEventListener.Get(LottoBtn).onClick = OnLottoClick

    clickLabel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/RulePanel/ScrollView/Label"):GetComponent("LabelClick")
    clickLabel.goToTask = GoToTask

    NewUserTip = UnityTools.FindGo(gameObject.transform, "Container/Background/Panel")
    
    local NewUserTaskTipBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/LottoCenter/GoToNewUser")
    UIEventListener.Get(NewUserTaskTipBtn).onClick = ShowNewUserTip

    local lottoPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel")
    local ShareRankPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareRankPanel")
    local MySharePanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/MySharePanel")
    local ShareTypePanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel")
    local RulePanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/RulePanel")

    table.insert(rightPanels, lottoPanel)
    table.insert(rightPanels, ShareRankPanel)
    table.insert(rightPanels, RulePanel)
    table.insert(rightPanels, MySharePanel)
    table.insert(rightPanels, ShareTypePanel)

    local GetTelCostLb = UnityTools.FindGo(rightPanels[4].transform, "TopPanel/GetTelCost"):GetComponent("UILabel")
    GetTelCostLb.text = "一日抽奖次数："
    GetTelCostLb = UnityTools.FindGo(rightPanels[4].transform, "TopPanel/GetTelCost/Label")
    GetTelCostLb.transform.localPosition = UnityEngine.Vector3(64,0,0)
    local ShareBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/ShareBtn")
    UIEventListener.Get(ShareBtn).onClick = OnShareBtnClick

    local ScaleQRCodeBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/QRcode")
    UIEventListener.Get(ScaleQRCodeBtn).onClick = OnScaleQRCodeClick
    -- _maskPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/maskPanel"):GetComponent("UIPanel")
    QRCodeMask = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareTypePanel/QRCodePanel")
    UIEventListener.Get(QRCodeMask).onClick = OnQRCodeMaskClick

    _ShareRankCellMgr = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareRankPanel/list/scrollView/grid"):GetComponent("UIGridCellMgr")
    _ShareRankCellMgr.onShowItem = OnShowItem

    _ShareHistoryCellMgr = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/MySharePanel/list/scrollView/grid"):GetComponent("UIGridCellMgr")
    _ShareHistoryCellMgr.onShowItem = OnShowItem2

    _RedPacketLb = UnityTools.FindGo(gameObject.transform, "Container/Background/RedPacketPanel/Label"):GetComponent("UILabel")

    pageBtnPanel = UnityTools.FindGo(gameObject.transform, "Container/Background/BottomPanel")
    local previousPage = UnityTools.FindGo(gameObject.transform, "Container/Background/BottomPanel/LastPageBtn")
    local nextPage = UnityTools.FindGo(gameObject.transform, "Container/Background/BottomPanel/NextPageBtn")
    UIEventListener.Get(previousPage).onClick = OnpreviousPageClick
    UIEventListener.Get(nextPage).onClick = OnNextPageClick

    PagesLb = UnityTools.FindGo(gameObject.transform, "Container/Background/BottomPanel/PageNumLb"):GetComponent("UILabel")

    _ShareRankScrollView = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareRankPanel/list/scrollView"):GetComponent("UIScrollView")
    _ShareHistoryScrollView = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/MySharePanel/list/scrollView"):GetComponent("UIScrollView")

    local NewUserTipBtn = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/LottoCenter/GoToNewUserTask")
    UIEventListener.Get(NewUserTipBtn).onClick = OnNewUserTipBtnClick

    effect = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/LottoPanel/RewardPanel/Effect")

    
--- [ALB END]
end

local function Awake(gameObject)
    AutoLuaBind(gameObject)

    registerScriptEvent(EVENT_NEW_SHARE_LOTTO_RESPONSE, "OnNewShareLottoResponse")
    registerScriptEvent(EVENT_SHARE_RANK_RESPONSE, "OnNewShareShareRankResponse")
    registerScriptEvent(EVENT_SHARE_HISTORY_RESPONSE, "OnNewShareHistoryResponse")
end


local function Start(gameObject)
    SetRewardIconValue(curTopTab)
    initPirateFlashLight(gameObject)

    UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform, effect.transform, 30)

    -- if LuaConfigMgr.ShareRankingConfig["1"].item_reward[1][3] == nil then
    --     local rewardLb= UnityTools.FindGo(rightPanels[2].transform, "InnerTitle/RewardNum")
    --     rewardLb:SetActive(false)
    -- end

    updateLottoCenter(curTopTab)
    updateRedPacket()

    local _scrollView = UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/RulePanel/ScrollView")
    _controller:SetScrollViewRenderQueue(_scrollView.gameObject)
    _controller:SetScrollViewRenderQueue(UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/MySharePanel/list/scrollView"))
    _controller:SetScrollViewRenderQueue(UnityTools.FindGo(gameObject.transform, "Container/Background/RightPanel/ShareRankPanel/list/scrollView"))
    -- _controller:SetScrollViewRenderQueue(_shareScrollview.gameObject)
    -- _controller:SetScrollViewRenderQueue(_maskPanel.gameObject)
    -- _maskPanel.startingRenderQueue = _maskPanel.startingRenderQueue + 10 
    UpdateRedPoint()
    -- OnTopTabClick(topTabBtns[3].gameObject)
    if CTRL.TabIndex == 5 then
        OnShareBtnClick(nil)
    end

end


local function Update()

    if isPlayEffect then

        local nextPos = UnityEngine.Vector3.Lerp(rewardIcons[curLottoIndex].transform.localPosition, 
                                                    UnityEngine.Vector3(254, 212, 0), 
                                                        playedTime/totalTime)
        effect.transform.localPosition = nextPos

        if playedTime >= totalTime then
            OnPlayEffectFinish()
            return 
        end

        playedTime = playedTime + gTimer.deltaTime() * timeSpeed
        timeSpeed = timeSpeed + 0.02

    end

end

local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_NEW_SHARE_LOTTO_RESPONSE, "OnNewShareLottoResponse")
    unregisterScriptEvent(EVENT_SHARE_RANK_RESPONSE, "OnNewShareShareRankResponse")
    unregisterScriptEvent(EVENT_SHARE_HISTORY_RESPONSE, "OnNewShareHistoryResponse")
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
M.Update = Update
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable


-- 返回当前模块
return M
