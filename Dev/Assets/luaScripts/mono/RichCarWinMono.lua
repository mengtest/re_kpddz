-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RichCarWinMono.lua
-- * Summary:     RichCarWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        4/15/2017 11:23:42 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RichCarWinMono")



-- 界面名称
local wName = "RichCarWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _platformMgr = IMPORT_MODULE("PlatformMgr")

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local _thisObj
local UnityTools = IMPORT_MODULE("UnityTools")
local _playerMoney=0
local _topleft={}
local _bottom={}
_bottom.getNum=0
local _left={}
local _topright={}
local _right={}
local _center={}
local _mask
local _roadCell
local _tipTween
local _redtask={}
--- [ALD END]
local _redPacketCtrl = IMPORT_MODULE("RedPacketWinController")
local _masterCtrl = IMPORT_MODULE("RichCarMasterWinController")

local _posTb={-17,-80,43.6,106.7,74.7,-48.6,-111.2,13.4}
local _colorTb={1,2,4,3,1,4,2,3}
local _nowChipIndex=0
local _lastChipIndex=0
local _roadPosTabel={}
local _carIconTb={5,2,1,3,6,1,3,4,7,3,4,2,8,4,2,1}
local _stakeNum={100,1000,10000,100000,500000}
local _continuedStakeTb={}
local _continuedStakeTempTb={}
local DealerData = {}
local _rightIndex=1
local _parameter={}
local _noStakeCount=0
local _cool = 0
local _isStake=false
local _redTaskData=nil
local _isChangeBgm=false
local _preStatus=0
_parameter.duration = 0.5
_parameter.count = 6
_parameter.totalRounds=4

local function GetNumText2(num)
    local newNum=0
    num=tonumber(num)
    num=num*100
    if num < 10000 then
        return tostring(num)
    elseif num < 100000000 then --1亿
        newNum=num/10000
        if math.floor(newNum) <10 then
            newNum = math.floor(num/1000)
            return LuaText.Format("num_wan",newNum/10)
        else
            return LuaText.Format("num_wan",math.floor(newNum))
        end
    else
        newNum=num/100000000
        if math.floor(newNum) <10 then
            newNum = math.floor(num/10000000)
            return LuaText.Format("num_yi",newNum/10)
        else
            return LuaText.Format("num_yi",math.floor(newNum))
        end
    end
end
local function GetNumText(num)
    num=tonumber(num)
    num=num*100-- 放大1000倍
    local isMinus = 1
    if num< 0 then
        isMinus=-1
    end
    local num = num * isMinus
    if num < 1000 then
        return tostring(num*isMinus)
    else
        local strNum = tostring(num)
        local startPos = 1
        local strLen= string.len(strNum)
        local endPos=strLen %3
        
        local tempStr=""
        for i=endPos,strLen,3 do
            if i ~= 0 then
                if i == strLen then
                    tempStr = tempStr..string.sub(strNum,startPos,i)
                else
                    tempStr = tempStr..string.sub(strNum,startPos,i)..","
                end
                startPos = i+1
            end
        end
        if isMinus == 1 then
            return tempStr
        else
            return "-"..tempStr
        end
    end
end
local function UpdateChipBtn()
    local canClickIndex=-1
    local isChange=false
    for i=5,1,-1 do
        if _platformMgr.GetGod() < _stakeNum[i] or CTRL.MasterInfo.self == 1 or CTRL.MasterInfo.self == 3 then
            UtilTools.SetGray(_bottom.btnChips[i].gameObject,true,true)
            _bottom.boxChip[i].enabled=false
            if i == _nowChipIndex then
                _lastChipIndex = _nowChipIndex
            end
        else
            UtilTools.RevertGray(_bottom.btnChips[i].gameObject,true,true)
            _bottom.boxChip[i].enabled=true
            if _lastChipIndex==i then
                if _nowChipIndex>0 and _nowChipIndex<=#_bottom.btnChips then
                    _bottom.btnChips[_nowChipIndex].transform.localScale=UnityEngine.Vector3(1,1,1)
                    _bottom.chipSelect[_nowChipIndex].gameObject:SetActive(false)
                end
                _bottom.btnChips[i].transform.localScale=UnityEngine.Vector3(1.1,1.1,1.1)
                _bottom.chipSelect[i].gameObject:SetActive(true)
                isChange = true
                _nowChipIndex = i
                _lastChipIndex=0
            elseif isChange == false and _lastChipIndex >0 then
                if _nowChipIndex>0 and _nowChipIndex<=#_bottom.btnChips then
                    _bottom.btnChips[_nowChipIndex].transform.localScale=UnityEngine.Vector3(1,1,1)
                    _bottom.chipSelect[_nowChipIndex].gameObject:SetActive(false)
                end
                _bottom.btnChips[i].transform.localScale=UnityEngine.Vector3(1.1,1.1,1.1)
                _bottom.chipSelect[i].gameObject:SetActive(true)
                _nowChipIndex = i
                isChange = true
            end
            
        end
    end
end
local function UpdateRecordList()
    local delCount = 0
    delCount = _right.recordGridMgr.CellCount - #CTRL.History
    if delCount>=0 then
        for i=1,delCount do
            _right.recordGridMgr:DelteLastNode()
        end
        _right.recordGridMgr:UpdateCells()
    else
        _right.recordGridMgr:ClearCells()
        for i=1,#CTRL.History do
            _right.recordGridMgr:NewCellsBox(_right.recordGridMgr.Go)
        end
        _right.recordGridMgr.Grid:Reposition()
        _right.recordGridMgr:UpdateCells()
        --_right.recordScrollview.panel.clipOffset=UnityEngine.Vector2(0,#CTRL.History*-72)
        --_right.recordScrollview:ResetPosition()
        if #CTRL.History >5 then
            _right.recordScrollview.panel.clipOffset=UnityEngine.Vector2(0,(#CTRL.History-5)*-72)
            _right.recordScrollview.transform.localPosition = UnityEngine.Vector3(0,(#CTRL.History-5)*72,0)
        end
    end
end
local function UpdateGetNum()
    if _bottom.getNum >= 0 then
        _bottom.lbGetNum.text = LuaText.Format("rich_car_tip48",GetNumText(_bottom.getNum))
    else
        _bottom.lbGetNum.text = LuaText.Format("rich_car_tip49",GetNumText(_bottom.getNum))
    end
end
local function UpdateOnlineList()
    local delCount = 0
    delCount = _right.onlineGridMgr.CellCount - #CTRL.UserList
    if delCount>=0 then
        for i=1,delCount do
            _right.onlineGridMgr:DelteLastNode()
        end
        _right.onlineGridMgr:UpdateCells()
    else
        --_right.onlineGridMgr:ClearCells()
        for i=1,-delCount do
            _right.onlineGridMgr:NewCellsBox(_right.onlineGridMgr.Go)
        end
        _right.onlineGridMgr.Grid:Reposition()
        _right.onlineGridMgr:UpdateCells()
        --_right.recordScrollview.panel.clipOffset=UnityEngine.Vector2(0,#CTRL.History*-72)
        --_right.recordScrollview:ResetPosition()
    end
    _right.lbOnlineNum.text=LuaText.Format("rich_car_tip31",#CTRL.UserList)
end
local function OnAddRoadItem(index, item)
    local data = CTRL.History[index]
    if data == nil then
        return
    end
    local roadBg1 = UnityTools.FindGo(item.transform, "road1")
    local roadBg2 = UnityTools.FindGo(item.transform, "road2")
    local icon1 = UnityTools.FindGo(item.transform, "icon1"):GetComponent("UISprite")
    local icon2 = UnityTools.FindGo(item.transform, "icon2"):GetComponent("UISprite")
    local spLine = UnityTools.FindGo(item.transform, "line"):GetComponent("UISprite")
    if index % 2==0 then
        roadBg1:SetActive(true)
        roadBg2:SetActive(false)
    else
        roadBg1:SetActive(false)
        roadBg2:SetActive(true)
    end
    if data ~=nil then
        if data.result>=5 then
            icon1.gameObject:SetActive(true)
            if data.pool == 1 then 
                icon1.spriteName = "type5"
            else
                icon1.spriteName = "type".._colorTb[data.result]
            end
            icon1.transform.localPosition=UnityEngine.Vector3(_posTb[data.result],0,0)    
            _roadPosTabel[index]=icon1.transform
            icon2.gameObject:SetActive(false)
        else
            icon2.gameObject:SetActive(true)
            if data.pool == 1 then 
                icon2.spriteName = "type5"
            else
                icon2.spriteName = "type".._colorTb[data.result]
            end
            icon2.transform.localPosition=UnityEngine.Vector3(_posTb[data.result],0,0)
            icon1.gameObject:SetActive(false)
            _roadPosTabel[index]=icon2.transform
        end
        if index <= #CTRL.History and _roadPosTabel[index-1] ~= nil then
            local pre = _right.roadBg.transform:InverseTransformVector(_roadPosTabel[index-1].position)
            local now = _right.roadBg.transform:InverseTransformVector(_roadPosTabel[index].position)
            local v2 = UnityEngine.Vector2(pre.x-now.x,pre.y-now.y)
            local angle = 180 * math.atan2(v2.y,v2.x)/math.pi
            spLine.gameObject:SetActive(true)
            spLine.width = UnityEngine.Vector3.Distance(pre,now)
            spLine.transform.localEulerAngles = UnityEngine.Vector3(0, 0, angle)
            spLine.transform.position = _right.roadBg.transform:TransformVector(UnityEngine.Vector3.Lerp(pre,now,0.5))
        else
            spLine.gameObject:SetActive(false)
        end
    end
end
local function UpdateRoadList()
    local delCount = 0

    delCount = _right.roadGrid.transform.childCount - #CTRL.History
    
    _roadPosTabel = {}
    if delCount>=0 then
        for i=1,#CTRL.History do
            OnAddRoadItem(i,_right.roadGrid.transform:GetChild(i-1))
        end
    else
        local childCount=_right.roadGrid.transform.childCount
        for i=1,-delCount do
            NGUITools.AddChild(_right.roadGrid.gameObject, _roadCell)
        end
        
        _right.roadGrid:Reposition()
        if #CTRL.History >9 then
            _right.roadScrollview.panel.clipOffset=UnityEngine.Vector2(0,(#CTRL.History-9)*-32+12)
            _right.roadScrollview.transform.localPosition = UnityEngine.Vector3(0,(#CTRL.History-9)*32-12,0)
        end
        for i=1,#CTRL.History do
            OnAddRoadItem(i,_right.roadGrid.transform:GetChild(i-1))
        end
    end
end
local function UpdatePlayerGold(num)
    UpdateChipBtn()
    _bottom.lbGold.text = GetNumText(num)
end
local function SetData() 
 -- 庄家数据
    DealerData.self = CTRL.MasterInfo.self
    DealerData.Head = CTRL.MasterInfo.head
    DealerData.Sex = CTRL.MasterInfo.sex
    DealerData.VipLevel = CTRL.MasterInfo.vip
    DealerData.Gold = CTRL.MasterInfo.money
    DealerData.Score = CTRL.MasterInfo.score
    DealerData.Count = CTRL.MasterInfo.count
    DealerData.UserName = CTRL.MasterInfo.name
end
--right win
local function OnClickContinue(gameObject)
    
    if (CTRL.StatusInfo.status == 3 or CTRL.StatusInfo.status == 6) then
        if #_continuedStakeTb==0 then
            return
        end
        local minusMoney=0
        for i=1,#_continuedStakeTb do
            minusMoney=minusMoney+_continuedStakeTb[i].money
        end
        if _playerMoney < minusMoney then
            UnityTools.MessageDialog(LuaText.GetString("rich_car_tip22"),{okCall=openGoldShopPanel,alignment="CenterAll"})
            return
        end
        _noStakeCount=0
        UtilTools.ShowWaitFlag()
        local req ={}
        req.list = _continuedStakeTb
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(protoIdSet.cs_car_rebet_req,req)
    end
end
local function OnClickBtnRoad(gameObject)
    if _rightIndex ~= 2 then
        if _rightIndex == 1 then
            _right.recordTween:ResetToBeginning()
            _right.recordTween.enabled=false
        else
            _right.onlineTween:ResetToBeginning()
            _right.onlineTween.enabled=false
        end
        _rightIndex = 2
        _right.roadTween.enabled=true
        _right.roadTween:ResetToBeginning()
        _right.roadTween:PlayForward()
        UpdateRoadList()
    else -- 显示记录
        _right.roadTween:ResetToBeginning()
        _right.roadTween.enabled=false
        _rightIndex = 1
        _right.recordTween.enabled=true
        _right.recordTween:ResetToBeginning()
        _right.recordTween:PlayForward()
        UpdateRecordList()
    end
end

local function OnClickSwitch(gameObject)
    if _topright.toggle.value == true then
        UnityTools.CreateWin("BarrageWin")
        UnityEngine.PlayerPrefs.SetInt("RichCarBarrageSwitch", 2)
    else
        UnityTools.DestroyWin("BarrageWin")
        UnityEngine.PlayerPrefs.SetInt("RichCarBarrageSwitch", 1)
    end
 end

local function OnClickUserList(gameObject)
    if _rightIndex ~= 3 then
        if CTRL.UserListStatus == 0 then
            local protobuf = sluaAux.luaProtobuf.getInstance()
            protobuf:sendMessage(protoIdSet.cs_car_user_list_req, {}) 
            CTRL.UserListStatus = 1
        end
        if _rightIndex == 1 then
            _right.recordTween:ResetToBeginning()
            _right.recordTween.enabled=false
        else
            _right.roadTween:ResetToBeginning()
            _right.roadTween.enabled=false
        end
        _rightIndex = 3
        _right.onlineTween.enabled=true
        _right.onlineTween:ResetToBeginning()
        _right.onlineTween:PlayForward()
        UpdateOnlineList()
    else
        _right.onlineTween:ResetToBeginning()
        _right.onlineTween.enabled=false
        _rightIndex = 1
        _right.recordTween.enabled=true
        _right.recordTween:ResetToBeginning()
        _right.recordTween:PlayForward()
        UpdateRecordList()
    end
end
local function OnClickDown(gameObject)
    if CTRL.MasterInfo.self == 3 then
        UtilTools.ShowWaitFlag()
        local protobuf = sluaAux.luaProtobuf.getInstance()
        local req = {}
        req.flag = 3
        req.money = 0
        protobuf:sendMessage(protoIdSet.cs_car_master_req, req)
    else
        UtilTools.ShowWaitFlag()
        local protobuf = sluaAux.luaProtobuf.getInstance()
        local req = {}
        req.flag = 2
        req.money = 0
        protobuf:sendMessage(protoIdSet.cs_car_master_req, req)   
    end
end
local function OnShowUserItem(cellbox, index, item)
    index=index+1
    local data = CTRL.UserList[index]
    if data ~=nil then
        local spBg = UnityTools.FindGo(item.transform, "bg")
    
        local lbName = UnityTools.FindGo(item.transform, "name"):GetComponent("UILabel")
        local lbGold = UnityTools.FindGo(item.transform, "num"):GetComponent("UILabel")
        lbName.text = tostring(data.name)
        lbGold.text = GetNumText2(data.money)
        UnityTools.SetHead(UnityTools.FindGo(item.transform, "head").transform, data.head, data.vip,false,data.sex)   
        if index %2 == 0 then
            spBg:SetActive(true)
        else
            spBg:SetActive(false)
        end
    end
end
local function UpdateEmojiList()
    if _left.emojiGrid.CellCount == 0 then
        _left.emojiGrid:ClearCells()
        for i=1,24 do
            _left.emojiGrid:NewCellsBox(_left.emojiGrid.Go)
        end
        _left.emojiGrid.Grid:Reposition()
        _left.emojiGrid:UpdateCells()
        _left.emojiScroll:ResetPosition()
    else
        _left.emojiGrid.Grid:Reposition()
        _left.emojiScroll:ResetPosition()
    end
end


local function OnClickEmoji(gameObject)
    if _left.emojiSel.activeSelf == false then
        _left.quick:SetActive(false)
        _left.chatSel:SetActive(false)
        _left.emojiSel:SetActive(true)
        _left.emoji:SetActive(true)
        _left.emojiObj:SetActive(true)
        UpdateEmojiList()
    else
        _left.emojiSel:SetActive(false)
        _left.emoji:SetActive(false)
        _left.emojiObj:SetActive(false)
        --_left.chatSel:SetActive(false)
    end
end

local function OnClickEmojiItem(gameObject)
    local nowTime =UtilTools.GetServerTime()
    if _cool~=0 and _cool+3>nowTime then
        UtilTools.ShowMessage(LuaText.Format("rich_car_tip38",3-nowTime+_cool),"[FFFFFF]") 
        return
    end
    local comData =gameObject:GetComponent("ComponentData")
    if comData == nil then
        return
    end      
    local req ={}
    req.content= tostring(comData.Id)
    req.content_type = 3
    req.room_type = 3 
    req.obj_player_uuid = ""
    local protobuf = sluaAux.luaProtobuf.getInstance()
    _cool = nowTime
    protobuf:sendMessage(protoIdSet.cs_player_chat,req)
    --OnClickEmoji(nil)
    _left.btm.gameObject:SetActive(false)
    _left.emoji.gameObject:SetActive(false)
end
local function OnShowEmojiItem(cellbox, index, item)
    index = index + 1
    local sp = UnityTools.FindGo(item.transform, "sp"):GetComponent("UISprite")
    local comData = item.gameObject:GetComponent("ComponentData")
    comData.Id = index
    sp.spriteName = tostring(index)
    UnityTools.AddOnClick(item.gameObject, OnClickEmojiItem)
end
local function UpdateRedBg()
    --红包屏蔽
    -- for i=1,#_redPacketCtrl.RichCarTable do
    --     if  _redPacketCtrl.RichCarTable[i].status~=1 then
    --         _redTaskData = _redPacketCtrl.RichCarTable[i] 
    --         break
    --     end
    -- end
    -- if _redTaskData == nil then
    --     _redtask.go.gameObject:SetActive(false)
    --     return
    -- end
    -- if _redTaskData.status == 2 then
    --     _redtask.redObj.gameObject:SetActive(true)
    --     _redtask.status.gameObject:SetActive(true)
    --     _redtask.slider.gameObject:SetActive(false)
    -- else
    --     _redtask.redObj.gameObject:SetActive(false)
    --     _redtask.status.gameObject:SetActive(false)
    --     _redtask.slider.gameObject:SetActive(true)
    --     _redtask.slider.value = _redPacketCtrl.RichCarCount/tonumber(_redTaskData.total_gold)
    --     _redtask.lbValue.text = LuaText.Format("strength_desc11",GetNumText(_redPacketCtrl.RichCarCount),GetNumText(_redTaskData.total_gold))
    -- end
    -- _redtask.lbDesc.text = LuaText.Format("rich_car_tip41",GetNumText(_redTaskData.total_gold))
    -- _redtask.lbGetNum.text = LuaText.Format("item_107",_redTaskData.red_packet/10)
    -- _redtask.lbrednum.text = LuaText.Format("item_107",_redTaskData.red_packet/10)
end
local function OnClickRedObj(gameObject)
    _redtask.getBtn.gameObject:SetActive(not _redtask.getBtn.gameObject.activeSelf)
end

local function OnClickGetRed(gameObject)
    if _redTaskData==nil then return end
    if _redTaskData.status == 2 then
        local req={}
        req.index = _redTaskData.index-1
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(13568, req)
        _redtask.isRefresh=true
    end
end

local function OnClickAddMoney(gameObject)
    _masterCtrl.WinType = 3
    UnityTools.CreateLuaWin("RichCarMasterWin")
end

--- [ALF END]

--topleft
local function UpdateDealerGold()
    _topleft.lbGold.text = GetNumText(DealerData.Gold)
    if DealerData.Score < 0 then
        _topleft.lbScore.text = LuaText.Format("rich_car_tip7",GetNumText(DealerData.Score))
    else
        _topleft.lbScore.text = LuaText.Format("rich_car_tip8","+"..GetNumText(DealerData.Score))
    end
    --_topleft.lbScore:SetValue(DealerData.Score)
    _topleft.lbCount.text = tostring(DealerData.Count)
end
local function OnClickUpInfo(gameObject)
    UtilTools.ShowWaitFlag()
    local req ={}
    req.flag= 2
    local protobuf = sluaAux.luaProtobuf.getInstance()
    
    protobuf:sendMessage(protoIdSet.cs_car_master_list_req,req)
    if _left.btm ~= nil then
        if _left.btm.gameObject.activeSelf then
            _left.btm.gameObject:SetActive(false)
            _left.emoji.gameObject:SetActive(false)
        end
    end
end
local function OnClickUp(gameObject)
    local data = LuaConfigMgr.GoldCarryConfig["1"]
    if data ==nil then
        return
    end
    if _platformMgr.GetGod() < tonumber(data.gold_carry)  then
        UtilTools.ShowMessage(LuaText.Format("rich_car_tip25",GetNumText2(LuaConfigMgr.GoldCarryConfig["1"].gold_carry)),"[FFFFFF]")    
        return
    end
    UtilTools.ShowWaitFlag()
    local req ={}
    req.flag= 1
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_car_master_list_req,req)
    if _left.btm.gameObject.activeSelf then
        _left.btm.gameObject:SetActive(false)
        _left.emoji.gameObject:SetActive(false)
    end
end

local function UpdateContinueBtnStatus(isEnable)
    if CTRL.MasterInfo.self == 1 or CTRL.MasterInfo.self == 3 then
        _bottom.btnContinue.gameObject:SetActive(false)
        _bottom.btnAddMoney.gameObject:SetActive(true)
    else
        _bottom.btnContinue.gameObject:SetActive(true)
        _bottom.btnAddMoney.gameObject:SetActive(false)
        if isEnable and #_continuedStakeTb >0 then
            UtilTools.RevertGray(_bottom.btnContinue.gameObject,true,true)
            _bottom.btnContinueCollider.enabled=true
        else
            UtilTools.SetGray(_bottom.btnContinue.gameObject,true,true)
            _bottom.btnContinueCollider.enabled=false
        end
    end
end
function UpdateRichCarTopLeftBtn()
    if CTRL.MasterInfo.self == 1 or CTRL.MasterInfo.self == 2 or CTRL.MasterInfo.self == 3 then
        _noStakeCount=0
        -- LogError("CTRL.MasterInfo.self="..CTRL.MasterInfo.self)
        if CTRL.MasterInfo.self == 1 then
            _topleft.lbDownDesc.text= LuaText.GetString("rich_car_tip35")
        elseif CTRL.MasterInfo.self == 2 then
            _topleft.lbDownDesc.text= LuaText.GetString("rich_car_tip27")
        elseif CTRL.MasterInfo.self == 3 then
            _topleft.lbDownDesc.text= LuaText.GetString("rich_car_tip42")
        end
        _topleft.btnDown:SetActive(true)
        _topleft.up:SetActive(false)
    else
        _topleft.btnDown:SetActive(false)
        _topleft.up:SetActive(true)
    end
end
local function UpdateTopLeftWin()
    UpdateDealerGold()
    --设置头像 undo
    _topleft.lbName.text = DealerData.UserName
    UnityTools.SetHead(_topleft.head, DealerData.Head, DealerData.VipLevel,false,DealerData.Sex)   
    UpdateRichCarTopLeftBtn()
end
function UpdateRichCarMasterInfo()
    if CTRL.MasterInfo == nil or CTRL.MasterInfo.money ==nil then
        return
    end
    SetData()
    UpdateTopLeftWin()
    if CTRL.MasterInfo.self == 1 or CTRL.MasterInfo.self == 3 then
        _preStatus=1
        -- _playerMoney = CTRL.MasterInfo.money + GameDataMgr.PLAYER_DATA.Money
        _playerMoney = _platformMgr.GetGod()
        UpdatePlayerGold(_playerMoney)
    else
        if _preStatus == 1 then
            _preStatus = 0
            _playerMoney = _platformMgr.GetGod()
            UpdatePlayerGold(_playerMoney)
        end
        -- _playerMoney = GameDataMgr.PLAYER_DATA.Money
        -- UpdatePlayerGold(_playerMoney)
        if _masterCtrl.WinType == 3 then
            UnityTools.DestroyWin("RichCarMasterWin")
        end
    end
    --if CTRL.StatusInfo.status == 1 or CTRL.StatusInfo.status == 2 then--等待

        --_center.waitObj:SetActive(true)
    -- LogError("DealerData.Count="..DealerData.Count)
    if DealerData.Count > 0 then
        _center.changeMaster:SetActive(false)
        _center.continue:SetActive(true)
        _center.lbContinueNum.text = DealerData.Count
    else
        _center.changeMaster:SetActive(true)
        _center.continue:SetActive(false)
    end
   -- end
end

-- topleft end
--center
-- 
local function StakeResponse(tbData)
    if tbData==nil then return end
    if CTRL.StatusInfo.status == nil or (CTRL.StatusInfo.status ~= 3 and CTRL.StatusInfo.status ~= 6) then
        return
    end
    local isRefreshGold=false
    local disMoney=0
    for j=1,#tbData do
        if tbData[j].self == 1 then
            _isStake = true
            
            if isRefreshGold == false then
                UpdateContinueBtnStatus(false)
            end
            isRefreshGold=true
             _continuedStakeTempTb[tbData[j].index]=tbData[j].money
            if tbData[j].index<= 8 and tbData[j].index>=1 then
                local preMoney=0
                if _center.selfNum[tbData[j].index].text=="" then
                    preMoney = 0
                else
                    preMoney=tonumber(_center.selfNum[tbData[j].index].text)
                end
                _center.selfNum[tbData[j].index].text = tostring(tbData[j].money*100)
                disMoney = disMoney+tbData[j].money - preMoney/100
                -- LogError("disMoney="..disMoney)
                _center.selfBg[tbData[j].index].gameObject:SetActive(true)
            end
        else
            if tbData[j].index<= 8 and tbData[j].index>=1 then
                _center.allNum[tbData[j].index].text = tostring(tbData[j].money*100)
            end
        end
    end
    if isRefreshGold then
        _playerMoney = _platformMgr.GetGod() - disMoney
        UpdatePlayerGold(_playerMoney)
    end
end
function RichCarStakeResponse(idMsg,msg,tbData)
    if msg == "call" then
        StakeResponse(tbData)
    elseif msg == "error" then
        UpdateContinueBtnStatus(false)
    elseif msg == "normalerror" then
        _playerMoney = _platformMgr.GetGod()
    end

end
local function UpdateRoomInfo()
    UpdateRichCarMasterInfo()
    if CTRL.Result.poolSub < 0 then
    --彩池
        _bottom.getNum = _bottom.getNum + CTRL.Result.selfNum + CTRL.Result.pool
        if CTRL.Result.selfNum >= 0 then
            _center.color.lbSelfNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.selfNum))
        else
            _center.color.lbSelfNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.selfNum))
        end
        if CTRL.Result.masterNum >= 0 then
            _center.color.lbMasterNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.masterNum))
        else
            _center.color.lbMasterNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.masterNum))
        end
        _center.color.lbColorNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.poolSub))
        _center.color.lbRewardNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.pool))
    else
        _bottom.getNum = _bottom.getNum + CTRL.Result.selfNum
        if CTRL.Result.selfNum >= 0 then
            _center.lbSelfNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.selfNum))
        else
            _center.lbSelfNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.selfNum))
        end
        if CTRL.Result.masterNum >= 0 then
            _center.lbMasterNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.masterNum))
        else
            _center.lbMasterNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.masterNum))
        end
        UtilTools.loadTexture(_center.texCar,"UI/Texture/RichCar/tex"..CTRL.Result.result..".png",true)
        _center.spCarName.spriteName = tostring(CTRL.Result.result)
    end
    if CTRL.StatusInfo.status == nil or (CTRL.StatusInfo.status ~= 3 and CTRL.StatusInfo.status ~= 6) then
        return
    end
    if CTRL.RoomInfo.selfList~=nil then
        _continuedStakeTb = CTRL.RoomInfo.selfList
        for i=1,#CTRL.RoomInfo.selfList do
            if CTRL.RoomInfo.selfList[i].index>=1 and CTRL.RoomInfo.selfList[i].index<=8 then
                if not _isStake and CTRL.RoomInfo.selfList[i].money >0 then
                    _isStake=true
                end
                if CTRL.RoomInfo.selfList[i].money>0 then
                    _center.selfBg[CTRL.RoomInfo.selfList[i].index].gameObject:SetActive(true)  
                end
                _center.selfNum[CTRL.RoomInfo.selfList[i].index].text = tostring(CTRL.RoomInfo.selfList[i].money*100)
            end
        end
    end
    if CTRL.RoomInfo.allList~=nil then
        for i=1,#CTRL.RoomInfo.allList do
            if CTRL.RoomInfo.allList[i].index>=1 and CTRL.RoomInfo.allList[i].index<=8 then
                _center.allNum[CTRL.RoomInfo.allList[i].index].text = tostring(CTRL.RoomInfo.allList[i].money*100)
            end
        end
    end
end
local function openGoldShopPanel()
    local shopCtrl = IMPORT_MODULE("ShopWinController");
    if shopCtrl ~= nil then
        shopCtrl.OpenShop(1)
    end
end
function UpdateRichCarRoomInfo()
    UpdateRoomInfo()
end


local function OnClickSetChips(gameObject)
    if _nowChipIndex <=0 then
        return
    end

    if CTRL.StatusInfo.status == 3 or CTRL.StatusInfo.status == 6 then
        UnityTools.PlaySound("Sounds/RichCar/click",{target=gameObject})
        if DealerData.self== 1 then
            UtilTools.ShowMessage(LuaText.GetString("rich_car_tip39"),"[FFFFFF]")
            return
        end
        if _platformMgr.GetGod() < CTRL.Limit then 
            UnityTools.MessageDialog(LuaText.Format("game_center_limit_tip1",CTRL.Limit),{okCall=openGoldShopPanel,okBtnName=LuaText.GetString("game_center_limit_tip2")})
            return
        end
        if _playerMoney < _stakeNum[_nowChipIndex] then
            UnityTools.MessageDialog(LuaText.GetString("rich_car_tip22"),{okCall=openGoldShopPanel,alignment="CenterAll"})
            return
        end
        _playerMoney=_playerMoney-_stakeNum[_nowChipIndex]
        
        local comData = gameObject:GetComponent("ComponentData")
        if comData == nil or _nowChipIndex > #_stakeNum then
            return
        end
        _noStakeCount = 0
        local req ={}
        req.index= comData.Id
        req.money = _stakeNum[_nowChipIndex]
        
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(protoIdSet.cs_car_bet_req,req)
    end
end
local function ResultResponse()
    --LogError("poolSub"..CTRL.Result.poolSub)
    
    if CTRL.Result.poolSub < 0 then
    --彩池
        _bottom.getNum = _bottom.getNum + CTRL.Result.selfNum + CTRL.Result.pool
        if CTRL.Result.selfNum >= 0 then
            _center.color.lbSelfNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.selfNum))
        else
            _center.color.lbSelfNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.selfNum))
        end
        if CTRL.Result.masterNum >= 0 then
            _center.color.lbMasterNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.masterNum))
        else
            _center.color.lbMasterNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.masterNum))
        end
        _center.color.lbColorNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.poolSub))
        _center.color.lbRewardNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.pool))
        _center.icon[_center.nowSelect].enabled=false
        if CTRL.StatusInfo.status == 4  then
            _center.circle:StartColorCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1)
        end
    else
        _bottom.getNum = _bottom.getNum + CTRL.Result.selfNum
        if CTRL.Result.selfNum >= 0 then
            _center.lbSelfNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.selfNum))
        else
            _center.lbSelfNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.selfNum))
        end
        if CTRL.Result.masterNum >= 0 then
            _center.lbMasterNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.masterNum))
        else
            _center.lbMasterNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.masterNum))
        end
        UtilTools.loadTexture(_center.texCar,"UI/Texture/RichCar/tex"..CTRL.Result.result..".png",true)
        _center.spCarName.spriteName = tostring(CTRL.Result.result)
        
        if CTRL.StatusInfo.status == 4  then
            --_center.waitObj:SetActive(false)
            --_center.stakeObj:SetActive(false)
            --_center.awardObj:SetActive(false)
            _center.counterObj:SetActive(false)
            _center.icon[_center.nowSelect].enabled=false
            if CTRL.Result.result >=5 then
                _center.circle:StartCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1,6)
            else
                _center.circle:StartCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1,0)
            end
            _center.nowSelect = CTRL.Result.resultIndex
        end
    end
end
--center end

-- bottom win
local function OnClickGetGold(gameObject)
    openGoldShopPanel()
end
local function OnClickChip(gameObject)
    local comData= gameObject:GetComponent("ComponentData")
    if comData == nil then return end
    if _nowChipIndex == comData.Id then return end
    if _nowChipIndex > 0 and _nowChipIndex <=#_bottom.chipSelect then
        _bottom.btnChips[_nowChipIndex].transform.localScale=UnityEngine.Vector3(1,1,1)
        _bottom.chipSelect[_nowChipIndex].gameObject:SetActive(false)
    end
    _nowChipIndex = comData.Id
    if _nowChipIndex<=#_bottom.chipSelect then
        _bottom.chipSelect[_nowChipIndex].gameObject:SetActive(true)
        _bottom.btnChips[_nowChipIndex].transform.localScale=UnityEngine.Vector3(1.1,1.1,1.1)
    end
    _lastChipIndex=_nowChipIndex
end
local function UpdateBottomWin()
    --undo
    _playerMoney = _platformMgr.GetGod()
    UpdatePlayerGold(_playerMoney)
    _bottom.lbName.text = _platformMgr.UserName()
    UnityTools.SetHead(_bottom.head, _platformMgr.GetIcon(), _platformMgr.GetVipLv(),true,_platformMgr.getSex())   
    
end
-- bottom win end

local function CloseWinConfirm()
    _platformMgr.gameMgr.closeActiveFun = function()
        UnityTools.CallLoadingWin(false)
        UnityTools.DestroyWin(wName)
        UnityTools.DestroyWin("RichCarRuleWin")
        UnityTools.DestroyWin("RichCarMasterWin")
     end
    UnityTools.ReturnToMainCity()
    
end

local function CloseWin(gameObject)
    if _isStake == true then
        UnityTools.MessageDialog(LuaText.GetString("rich_car_tip40"),{okCall=CloseWinConfirm}) 
    elseif CTRL.MasterInfo.self == 1 then
        UnityTools.MessageDialog(LuaText.GetString("rich_car_tip50"),{okCall=CloseWinConfirm})
    else
        _platformMgr.gameMgr.closeActiveFun = function()
        UnityTools.CallLoadingWin(false)
        UnityTools.DestroyWin(wName)
        UnityTools.DestroyWin("RichCarRuleWin")
        UnityTools.DestroyWin("RichCarMasterWin")
        end
        UnityTools.ReturnToMainCity()
    end
end
local function GetPlayIndex(index)
    if index >=1 and index<=16 then 
        return index
    elseif index>=17 and index<=31 then
        return index%16
    elseif index<0 then
        return (index+16)%16
    elseif index == 0 then
        return 16
    else
        return index
    end
end

local function FlashCircle(playIndex)
    _center.tweemAlpha[playIndex].from=1
    _center.tweemAlpha[playIndex].to=0
    _center.tweemAlpha[playIndex].delay=0
    _center.tweemAlpha[playIndex].duration =0.7
    _center.tweemAlpha[playIndex]:ResetToBeginning()
    _center.tweemAlpha[playIndex].style= 2
    --_center.spSel[playIndex].alpha=startAlpha
    _center.tweemAlpha[playIndex].enabled=true
    _center.tweemAlpha[playIndex]:PlayForward()
    _center.icon[playIndex].enabled = true
end

local function StartTweenAlpha(playIndex,startAlpha,toAlpha)
    playIndex=GetPlayIndex(playIndex)
    if delay==nil then
        delay=0
    end
    if _center.delayCount[playIndex] ==nil then
        _center.delayCount[playIndex] =0
    end
    if _center.spSel[playIndex].alpha ~= 0 then
        toAlpha = _center.spSel[playIndex].alpha-startAlpha+toAlpha
        startAlpha = _center.spSel[playIndex].alpha
    end
    if toAlpha<0 then
        toAlpha=0
    end
    
    _center.tweemAlpha[playIndex].from=startAlpha
    _center.tweemAlpha[playIndex].to=toAlpha
    _center.tweemAlpha[playIndex].delay=0
    _center.tweemAlpha[playIndex].duration =_parameter.duration
    _center.tweemAlpha[playIndex].style=0
    _center.tweemAlpha[playIndex]:ResetToBeginning()
    _center.spSel[playIndex].alpha=startAlpha
    _center.tweemAlpha[playIndex].enabled=true
    _center.tweemAlpha[playIndex]:PlayForward()
end

--豪车转圈表现
local function InitParam()
    _center.nowSelect=1
    _center.startIndex=_center.nowSelect
    _center.leftRounds = _parameter.totalRounds
    _center.runCount = 0
    _parameter.count = 7
end
---豪车转圈表现


--left
local function OnInputSubmit()
--undo
    if _left.input.value ~="" then
        local nowTime =UtilTools.GetServerTime()
        if _cool~=0 and _cool+3>nowTime then
            UtilTools.ShowMessage(LuaText.Format("rich_car_tip38",3-nowTime+_cool),"[FFFFFF]") 
            return
        end
        local sendMsg = GameText.Instance:StrFilter(_left.input.value,42)
        local req ={}
        req.room_type = 3 
        req.obj_player_uuid = ""
        req.content= sendMsg
        if _platformMgr.GetVipLv() >=1 then
            req.content_type = 6
        else
            req.content_type = 1
        end
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(protoIdSet.cs_player_chat,req)
        _cool = nowTime
        _left.input.value=""
    end
end
local function OnClickSendMsg(gameObject)
    OnInputSubmit()
end


local function OnClickQuickChat(gameObject)
    if _left.quick.activeSelf == false then
        _left.quick:SetActive(true)
        _left.chatSel:SetActive(true)
        _left.emojiSel:SetActive(false)
        _left.emoji:SetActive(false)
        _left.emojiObj:SetActive(false)
    else
        _left.quick:SetActive(false)
        _left.chatSel:SetActive(false)
    end
end
local function OnClickShowChat(gameObject)
    _left.btm.gameObject:SetActive(not _left.btm.gameObject.activeSelf)
    if _left.btm.gameObject.activeSelf then
        _left.quick:SetActive(true)
        _left.chatSel:SetActive(true)
        _left.emojiSel:SetActive(false)        
        _left.emoji:SetActive(false) 
        _left.emojiObj:SetActive(false)
    else
        _left.emoji:SetActive(false) 
    end
end 
 local function OnClickQuickCell(gameObject)
    local comData=gameObject:GetComponent(ComponentData)
    if comData ~= nil then
        local nowTime =UtilTools.GetServerTime()
        if _cool~=0 and _cool+3>nowTime then
            UtilTools.ShowMessage(LuaText.Format("rich_car_tip38",3-nowTime+_cool),"[FFFFFF]") 
            return
        end
        local req ={}
        req.room_type = 3 
        req.obj_player_uuid = ""
        req.content= LuaText.GetString("rich_car_quick"..comData.Id)
        if _platformMgr.GetVipLv()>=1 then
            req.content_type = 6
        else
            req.content_type = 1
        end
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(protoIdSet.cs_player_chat,req)
        _cool = nowTime
        --OnClickQuickChat(nil)
        _left.btm.gameObject:SetActive(false)
    end
end
local function AddChatCell(userData,index)
    _left.chatCellList[index]={}
    _left.chatCellList[index].go = NGUITools.AddChild(_left.chatContent, _left.chatCell)
    local lastHigh=10
    if index ~= 1 then
        lastHigh = _left.chatCellList[index-1].height
    end
    local lbContent=UnityTools.FindGo(_left.chatCellList[index].go.transform, "name"):GetComponent("UILabel")
    local lbname=UnityTools.FindGo(_left.chatCellList[index].go.transform, "ename"):GetComponent("UILabel")
    local spEmoji=UnityTools.FindGo(_left.chatCellList[index].go.transform, "emoji"):GetComponent("UISprite")
    local spSprite=UnityTools.FindGo(_left.chatCellList[index].go.transform, "vip"):GetComponent("UISprite")
    local cellWid=_left.chatCellList[index].go:GetComponent("UIWidget")
    if userData.flag == 1 or userData.flag==6 then--文本
        if _topright.toggle.value == true and userData.flag == 6 then
            local danmuCtrl=UI.Controller.UIManager.GetControler("BarrageWin")
            danmuCtrl:OnRichCarMessageAdd(userData.content,userData.name..":",userData.vip,1500002)
        end

        lbContent.text=LuaText.Format("rich_car_tip9",userData.name,userData.content)
        spSprite.spriteName = tostring(userData.vip)
        cellWid.height = lbContent.height+3
        spEmoji.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        lbname.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        lbContent.transform.localPosition=UnityEngine.Vector3(0,-5,0)
        _left.chatCellList[index].go.transform.localPosition = UnityEngine.Vector3(0,-lastHigh,0)
        _left.chatCellList[index].height = lastHigh + cellWid.height
    elseif userData.flag == 2 then--语音

    elseif userData.flag == 3 then--表情
        lbname.text =LuaText.Format("rich_car_tip10",userData.name,"")
        spSprite.spriteName = tostring(userData.vip)
        spEmoji.spriteName = userData.content
        cellWid.height = 50
        spSprite.transform.localPosition = UnityEngine.Vector3(-106.5,-40,0) 
        lbname.transform.localPosition=UnityEngine.Vector3(-89.1,-50,0)
        spEmoji.transform.localPosition = UnityEngine.Vector3(-89.1+lbname.width+3,0,0) 
        lbContent.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        _left.chatCellList[index].go.transform.localPosition = UnityEngine.Vector3(0,-lastHigh,0)
        _left.chatCellList[index].height = lastHigh + cellWid.height
    elseif userData.flag == 4 then--玩家进入
        lbContent.text=LuaText.Format("rich_car_tip11",userData.name)
        cellWid.height = lbContent.height+10
        lbname.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        spEmoji.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        spSprite.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        lbContent.transform.localPosition=UnityEngine.Vector3(0,-5,0)
        _left.chatCellList[index].go.transform.localPosition = UnityEngine.Vector3(0,-lastHigh,0)
        _left.chatCellList[index].height = lastHigh + cellWid.height
        
    elseif userData.flag == 5 then--
        lbContent.text=LuaText.Format("rich_car_tip32",userData.content)
        cellWid.height = lbContent.height+3
        lbname.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        spEmoji.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        spSprite.transform.localPosition=UnityEngine.Vector3(30000,0,0)
        lbContent.transform.localPosition=UnityEngine.Vector3(0,-5,0)
        _left.chatCellList[index].go.transform.localPosition = UnityEngine.Vector3(0,-lastHigh,0)
        _left.chatCellList[index].height = lastHigh + cellWid.height
    end
    return _left.chatCellList[index].height,cellWid.height
end
local function OnAlphaFinish()
    _center.areaTween2:ResetToBeginning()
    _center.areaTween2.enabled=false
end
local function UpdateChatScroll()
    for i=1,#CTRL.ChatList do
         AddChatCell(CTRL.ChatList[i],i)
    end
end
local function ChatUpdate(data)
    if data.flag == 4 then
        if _topright.toggle.value == true then
            local danmuCtrl=UI.Controller.UIManager.GetControler("BarrageWin")
            danmuCtrl:OnRichCarMessageAdd(LuaText.GetString("rich_car_tip47"),data.name,data.vip,1500002)
        end
        return
    end
    if _left.chatContent.transform.childCount == 20 then
        local disHeight = _left.chatCellList[1].height
        local lastHigh = 0
        for i=1,#_left.chatCellList do
            _left.chatCellList[i].height = _left.chatCellList[i].height - disHeight
            if i~= 1 then
                _left.chatCellList[i].go.transform.localPosition = UnityEngine.Vector3(0,-lastHigh,0)
            end
            lastHigh = _left.chatCellList[i].height
        end
        
        UnityTools.Destroy(_left.chatCellList[1].go)
        table.remove(_left.chatCellList,1)
        local height = AddChatCell(data,20)
        --修改scrollview位置
        -- _right.chatscroll.panel.clipOffset=UnityEngine.Vector2(0,_right.recordScrollview.panel.clipOffset.y-height)
        -- _right.chatscroll.transform.localPosition = UnityEngine.Vector3(0,_right.recordScrollview.transform.localPosition.y+height,0)
    else
        local height,offsetY = AddChatCell(data,#_left.chatCellList+1)
        if height >265 and height-offsetY<265 then
            offsetY = height-265
        end
        if height > 265 then
            _left.chatScroll.panel.clipOffset=UnityEngine.Vector2(0,_left.chatScroll.panel.clipOffset.y-offsetY)
            _left.chatScroll.transform.localPosition = UnityEngine.Vector3(0,_left.chatScroll.transform.localPosition.y+offsetY,0)
        end
        
    end
end

local function OnClickPool(gameObject)
    local data1 ={}
    data1.content = LuaText.GetString("rich_car_tip43")
    data1.flag = 5
    table.insert(CTRL.ChatList,data1)
    ChatUpdate(data1)
end

function RichCarWinChatUpdate(idMsg,type,data)
    ChatUpdate(data)
end
--end
local function OnStartStakeComplete()
    _mask.gameObject:SetActive(false)
    _center.startTween.enabled=false
    _center.stakeObj:SetActive(true)
    _center.awardObj:SetActive(false)
    _center.colorObj:SetActive(false)
    _center.counterObj:SetActive(true)
    FlashCircle(_center.nowSelect)    
    
end
local function OnStopStakeComplete()
    _center.stopTween.enabled=false
    _mask.gameObject:SetActive(false)
    _center.icon[_center.nowSelect].enabled=false
    if CTRL.Result.poolSub < 0 then
        _center.circle:StartColorCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1)
    else
        if CTRL.Result.result >=5 then
            _center.circle:StartCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1,6)
        else
            _center.circle:StartCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1,0)
        end
    --_center.lbCool:SetEndTime(CTRL.StatusInfo.time)
    end
    _center.nowSelect = CTRL.Result.resultIndex
end
function UpdateRichCarList()
    if _rightIndex == 1 then
        UpdateRecordList()
    elseif _rightIndex == 2 then
        UpdateRoadList()
    elseif _rightIndex == 3 then
        UpdateOnlineList()
    end
end
local function OnComplete()
    _center.waitObj:SetActive(false)
    _center.stakeObj:SetActive(false)
    if CTRL.Result.poolSub < 0 then
        _center.awardObj:SetActive(false)
        _center.colorObj:SetActive(true)
    else
        _center.awardObj:SetActive(true)
        _center.colorObj:SetActive(false)
    end
    _center.counterObj:SetActive(false)
    FlashCircle(_center.nowSelect)
    _playerMoney = _platformMgr.GetGod()
    UpdatePlayerGold(_playerMoney)
        
    _topleft.lbColor.text = GetNumText(CTRL.PoolNum)
    gTimer.registerOnceTimer(300,"UpdateRichCarList")
    
    UpdateRedBg()
    UpdateGetNum()
    --LogError("test")
end 
local function OnShowRecordItem(cellbox, index, item)
    index=index+1
    if index <= #CTRL.History then
        local data = CTRL.History[index]
        local spIcon= UnityTools.FindGo(item.transform, "icon"):GetComponent("UISprite")
        local bgObj=UnityTools.FindGo(item.transform, "bg")
        spIcon.spriteName = "H10"..data.result
        if index%2 == 0 then   
            bgObj:SetActive(true)
        else
            bgObj:SetActive(false)
        end
    end
end
local function OnTipFinish()
    _tipTween.gameObject:SetActive(false)
    _mask.gameObject:SetActive(false)
end
function _left.Bind(gameObject)
    _left.chatScroll = UnityTools.FindGo(gameObject.transform, "Container/left/chatscroll/scrollview"):GetComponent("UIScrollView")
    _left.chatContent = UnityTools.FindGo(gameObject.transform, "Container/left/chatscroll/scrollview/content")

    _left.chatCell = UnityTools.FindGo(gameObject.transform, "chatcell")

    _left.btnShowChat = UnityTools.FindGo(gameObject.transform, "Container/left/showchat")
    UnityTools.AddOnClick(_left.btnShowChat.gameObject, OnClickShowChat)
    _left.btm = UnityTools.FindGo(gameObject.transform, "Container/left/btm"):GetComponent("UIPanel")
    _left.quick = UnityTools.FindGo(gameObject.transform, "Container/left/btm/quickbg")
    _left.emojiScroll = UnityTools.FindGo(gameObject.transform, "Container/left/emoji/scrollView"):GetComponent("UIScrollView")
    _left.emojiGrid = UnityTools.FindGo(gameObject.transform, "Container/left/emoji/scrollView/grid"):GetComponent("UIGridCellMgr")
    _left.emojiGrid.onShowItem = OnShowEmojiItem
    _left.emoji = UnityTools.FindGo(gameObject.transform, "Container/left/emoji")
    _left.btnQuickChat = UnityTools.FindGo(gameObject.transform, "Container/left/btm/btnchat")
    UnityTools.AddOnClick(_left.btnQuickChat.gameObject, OnClickQuickChat)
    _left.btnEmoji = UnityTools.FindGo(gameObject.transform, "Container/left/btm/btnemoji")
    UnityTools.AddOnClick(_left.btnEmoji.gameObject, OnClickEmoji)
    _left.emojiSel = UnityTools.FindGo(gameObject.transform, "Container/left/btm/btnemoji/sel")
    _left.emojiObj = UnityTools.FindGo(gameObject.transform, "Container/left/btm/emoji")
    _left.chatSel = UnityTools.FindGo(gameObject.transform, "Container/left/btm/btnchat/sel")
    for i=1,6 do
        local go =  UnityTools.FindGo(gameObject.transform, "Container/left/btm/quickbg/grid/word"..i)
        local lbText= UnityTools.FindGo(go.transform, "Label"):GetComponent("UILabel")
        lbText.text = LuaText.GetString("rich_car_quick"..i)
        local comData = go:AddComponent("ComponentData")
        comData.Id=i
        UnityTools.AddOnClick(go, OnClickQuickCell)
    end
    _left.input = UnityTools.FindGo(gameObject.transform, "Container/left/input"):GetComponent("UIInput")
    EventDelegate.Add(_left.input.onSubmit,OnInputSubmit)
    
    
    _left.btnSend = UnityTools.FindGo(gameObject.transform, "Container/left/btnsend")
    UnityTools.AddOnClick(_left.btnSend.gameObject, OnClickSendMsg)
    _tipTween = UnityTools.FindGo(gameObject.transform, "Container/tip"):GetComponent("TweenAlpha")
    _tipTween.enabled=false
    EventDelegate.Add(_tipTween.onFinished,OnTipFinish)
end
function _center.Bind(gameObject)
    _center.spIcon={}
    _center.tweemAlpha={}
    _center.leftTurn={}
    _center.spSel={}
    _center.icon={}
    _center.lbCool = UnityTools.FindGo(gameObject.transform, "Container/center/counter/Label"):GetComponent("CooldownUpdate")
    --_center.lbCool.isTicket=true
    _center.circle = UnityTools.FindGo(gameObject.transform, "Container/center/icons"):GetComponent("RichCarCircle")
    
    for i=1,16 do 
        _center.icon[i] = UnityTools.FindGo(gameObject.transform, "Container/center/icons/icon"..i):GetComponent("TweenScale")
        _center.icon[i].from=UnityEngine.Vector3(0.95,0.95,0.95)
        _center.icon[i].to=UnityEngine.Vector3(1.05,1.05,1.05)
        _center.icon[i].style= 2
        _center.icon[i].enabled=false
        _center.icon[i].duration=0.7
        _center.spIcon[i] = UnityTools.FindGo(gameObject.transform, "Container/center/icons/icon"..i.."/sp"):GetComponent("UISprite")
        _center.spSel[i] = UnityTools.FindGo(gameObject.transform, "Container/center/icons/icon"..i.."/sel"):GetComponent("UISprite")
        _center.leftTurn[i]=3
        local comData = _center.spSel[i].gameObject:AddComponent("ComponentData")
        comData.Id = i 
        _center.tweemAlpha[i] =_center.spSel[i].gameObject:GetComponent("TweenAlpha") 
        _center.tweemAlpha[i].enabled=false
        _center.spSel[i].alpha=0
        --EventDelegate.Add(_center.tweemAlpha[i].onFinished,OnAlphaComplete)
        _center.spIcon[i].spriteName = LuaConfigMgr.CardPowerConfig[tostring(_carIconTb[i])].icon
        _center.circle:AddTweenAlpha(_center.tweemAlpha[i],_center.spSel[i],_center.icon[i].gameObject)
    end
    _center.stakeObj = UnityTools.FindGo(gameObject.transform, "Container/center/stake")
    _center.stake={}
    _center.selfNum={}
    _center.allNum={}
    _center.color={}
    _center.selfBg={}
    for i=1,8 do
        _center.stake[i] = UnityTools.FindGo(gameObject.transform, "Container/center/stake/area"..i)
        _center.selfBg[i] = UnityTools.FindGo(_center.stake[i].transform, "sp")
        _center.selfNum[i] = UnityTools.FindGo(_center.stake[i].transform, "sp/num"):GetComponent("UILabel")
        _center.allNum[i] = UnityTools.FindGo(_center.stake[i].transform, "count"):GetComponent("UILabel")
        UnityTools.AddOnClick(_center.stake[i].gameObject, OnClickSetChips)
    end

    _center.counterObj = UnityTools.FindGo(gameObject.transform, "Container/center/counter")
    EventDelegate.Add(_center.circle.onComplete,OnComplete)
    

    _center.awardObj = UnityTools.FindGo(gameObject.transform, "Container/center/award")
    _center.lbMasterNum = UnityTools.FindGo(gameObject.transform, "Container/center/award/master/num"):GetComponent("UILabel")
    _center.lbSelfNum = UnityTools.FindGo(gameObject.transform, "Container/center/award/self/num"):GetComponent("UILabel")
    _center.texCar = UnityTools.FindGo(gameObject.transform, "Container/center/award/tex"):GetComponent("UITexture")
    _center.spCarName = UnityTools.FindGo(gameObject.transform, "Container/center/award/car"):GetComponent("UISprite")

    _center.waitObj = UnityTools.FindGo(gameObject.transform, "Container/center/wait")
    _center.changeMaster = UnityTools.FindGo(gameObject.transform, "Container/center/wait/bg/change")
    _center.continue = UnityTools.FindGo(gameObject.transform, "Container/center/wait/bg/continue")
    _center.lbContinueNum = UnityTools.FindGo(gameObject.transform, "Container/center/wait/bg/continue/num"):GetComponent("UILabel")
    _center.colorObj = UnityTools.FindGo(gameObject.transform, "Container/center/color")
    

    _center.color.lbSelfNum = UnityTools.FindGo(gameObject.transform, "Container/center/color/self/num"):GetComponent("UILabel")
    _center.color.lbMasterNum = UnityTools.FindGo(gameObject.transform, "Container/center/color/master/num"):GetComponent("UILabel")
    _center.color.lbRewardNum = UnityTools.FindGo(gameObject.transform, "Container/center/color/reward/num"):GetComponent("UILabel")
    _center.color.lbColorNum = UnityTools.FindGo(gameObject.transform, "Container/center/color/color/num"):GetComponent("UILabel")

    _center.startTween = UnityTools.FindGo(gameObject.transform, "Container/center/start"):GetComponent("TweenPosition")
    _center.stopTween = UnityTools.FindGo(gameObject.transform, "Container/center/stop"):GetComponent("TweenPosition")
    _center.stopTween.enabled = false
    EventDelegate.Add(_center.startTween.onFinished,OnStartStakeComplete)
    EventDelegate.Add(_center.stopTween.onFinished,OnStopStakeComplete)
    _center.startTween.enabled = false

    _mask = UnityTools.FindGo(gameObject.transform, "mask")

    

    _roadCell = UnityTools.FindGo(gameObject.transform, "roadcell")

    

    

    _center.areaTween2 = UnityTools.FindGo(gameObject.transform, "Container/center/stake/lines/area2"):GetComponent("TweenAlpha")
    
    _center.areaTween2.enabled=false
    EventDelegate.Add(_center.areaTween2.onFinished,OnAlphaFinish)

    _redtask.go = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask")
    UnityTools.AddOnClick(_redtask.go.gameObject, OnClickRedObj)
    _redtask.getBtn = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/redbg")
    UnityTools.AddOnClick(_redtask.getBtn.gameObject, OnClickGetRed)
    _redtask.lbGetNum = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/redbg/icon/num"):GetComponent("UILabel")
    _redtask.lbDesc = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/redbg/desc"):GetComponent("UILabel")
    _redtask.lbValue = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/redbg/slider/num"):GetComponent("UILabel")
    _redtask.slider = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/redbg/slider"):GetComponent("UISlider")
    _redtask.redObj = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/red")

    _redtask.status = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/redbg/status")

    _redtask.lbrednum = UnityTools.FindGo(gameObject.transform, "Container/topleft/redtask/num"):GetComponent("UILabel")
    

--- [ALB END]

end

function _topleft.Bind(gameObject)
    _topleft.lbGold = UnityTools.FindGo(gameObject.transform, "Container/topleft/gold/lb"):GetComponent("UILabel")
    _topleft.lbName = UnityTools.FindGo(gameObject.transform, "Container/topleft/name/lb"):GetComponent("UILabel")
    _topleft.lbScore = UnityTools.FindGo(gameObject.transform, "Container/topleft/score/Label"):GetComponent("UILabel")
    _topleft.lbCount = UnityTools.FindGo(gameObject.transform, "Container/topleft/count/Label"):GetComponent("UILabel")
    _topleft.up = UnityTools.FindGo(gameObject.transform, "Container/topleft/up")
    UnityTools.AddOnClick(_topleft.up.gameObject, OnClickUp)
    UnityTools.AddOnClick(_topleft.go.gameObject, OnClickUpInfo)
    _topleft.head = UnityTools.FindGo(gameObject.transform, "Container/topleft/head")
    _topleft.lbColor = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnPool/num"):GetComponent("UILabel")
    _topleft.btnDown = UnityTools.FindGo(gameObject.transform, "Container/topleft/down")
    _topleft.lbDownDesc = UnityTools.FindGo(gameObject.transform, "Container/topleft/down/Label"):GetComponent("UILabel")
    UnityTools.AddOnClick(_topleft.btnDown.gameObject, OnClickDown)
    _topleft.btnPool = UnityTools.FindGo(gameObject.transform, "Container/topleft/btnPool")
    UnityTools.AddOnClick(_topleft.btnPool.gameObject, OnClickPool)
end
function _bottom.Bind(gameObject)
    _bottom.btnContinue = UnityTools.FindGo(gameObject.transform, "Container/btm/btnContinue")
    _bottom.btnContinueCollider=_bottom.btnContinue:GetComponent("BoxCollider")
    UnityTools.AddOnClick(_bottom.btnContinue.gameObject,OnClickContinue)
    _bottom.lbName = UnityTools.FindGo(gameObject.transform, "Container/btm/name/lb"):GetComponent("UILabel")
    _bottom.lbGold = UnityTools.FindGo(gameObject.transform, "Container/btm/gold/lb"):GetComponent("UILabel")
    _bottom.btnGetGold = UnityTools.FindGo(gameObject.transform, "Container/btm/gold/addbtm")
    UnityTools.AddOnClick(_bottom.btnGetGold.gameObject, OnClickGetGold)
    
    _bottom.btnChips ={}
    _bottom.chipSelect ={}
    _bottom.boxChip={}
    for i=1,5 do
        _bottom.btnChips[i] = UnityTools.FindGo(gameObject.transform, "Container/btm/grid/chip"..i)
        _bottom.boxChip[i] =_bottom.btnChips[i]:GetComponent("BoxCollider")
        _bottom.chipSelect[i] = UnityTools.FindGo(_bottom.btnChips[i].transform, "sel")
        local comData = _bottom.btnChips[i]:AddComponent("ComponentData")
        comData.Id = i
        UnityTools.AddOnClick(_bottom.btnChips[i].gameObject, OnClickChip)
    end
    _bottom.head = UnityTools.FindGo(gameObject.transform, "Container/btm/head")
    _bottom.btnAddMoney = UnityTools.FindGo(gameObject.transform, "Container/btm/btnAddMoney")
    _bottom.lbGetNum = UnityTools.FindGo(gameObject.transform, "Container/btm/bg/num"):GetComponent("UILabel")
    UnityTools.AddOnClick(_bottom.btnAddMoney.gameObject, OnClickAddMoney)
end
function _topright.Bind(gameObject)
    _topright.btnRoad = UnityTools.FindGo(gameObject.transform, "Container/topright/road")
    UnityTools.AddOnClick(_topright.btnRoad.gameObject, OnClickBtnRoad)

    _topright.btnClose = UnityTools.FindGo(gameObject.transform, "Container/topright/back")
    UnityTools.AddOnClick(_topright.btnClose.gameObject, CloseWin)
    _topright.toggle = UnityTools.FindGo(gameObject.transform, "Container/topright/switch"):GetComponent("UIToggle")
    UnityTools.AddOnClick(_topright.toggle.gameObject, OnClickSwitch)
    local value= UnityEngine.PlayerPrefs.GetInt("RichCarBarrageSwitch", 2)
    if value == 2 then
        _topright.toggle.value = true
        UnityTools.CreateWin("BarrageWin")
    else
        _topright.toggle.value = false
        UnityTools.DestroyWin("BarrageWin")
    end
    
    _topright.btnUserList = UnityTools.FindGo(gameObject.transform, "Container/topright/user")
    UnityTools.AddOnClick(_topright.btnUserList.gameObject, OnClickUserList)
end
local function OnUserTweenComplete()
    _right.onlineGridMgr:UpdateCells()
end
local function OnRecordTweenCompete()
    _right.recordGridMgr:UpdateCells()
end
function _right.Bind(gameObject)
    
    _right.recordScrollview = UnityTools.FindGo(gameObject.transform, "Container/right/record/bg/scrollview"):GetComponent("UIScrollView")

    _right.recordGridMgr = UnityTools.FindGo(gameObject.transform, "Container/right/record/bg/scrollview/grid"):GetComponent("UIGridCellMgr")
    _right.recordGridMgr.onShowItem=OnShowRecordItem
    _right.recordTween = UnityTools.FindGo(gameObject.transform, "Container/right/record"):GetComponent("TweenPosition")
    EventDelegate.Add(_right.recordTween.onFinished,OnRecordTweenCompete)
    _right.onlineTween = UnityTools.FindGo(gameObject.transform, "Container/right/online"):GetComponent("TweenPosition")

    _right.onlineScrollview = UnityTools.FindGo(gameObject.transform, "Container/right/online/bg/scrollview")

    _right.onlineGridMgr = UnityTools.FindGo(gameObject.transform, "Container/right/online/bg/scrollview/grid"):GetComponent("UIGridCellMgr")
    _right.lbOnlineNum = UnityTools.FindGo(gameObject.transform, "Container/right/online/num"):GetComponent("UILabel")
    _right.onlineGridMgr.onShowItem=OnShowUserItem
    EventDelegate.Add(_right.onlineTween.onFinished,OnUserTweenComplete)
    _right.roadTween = UnityTools.FindGo(gameObject.transform, "Container/right/road"):GetComponent("TweenPosition")

    _right.roadScrollview = UnityTools.FindGo(gameObject.transform, "Container/right/road/bg/scrollview"):GetComponent("UIScrollView")
    _right.roadBg = UnityTools.FindGo(gameObject.transform, "Container/right/road/bg")
    _right.roadGrid = UnityTools.FindGo(gameObject.transform, "Container/right/road/bg/scrollview/grid"):GetComponent("UIGrid")
    --_right.roadGrid.onShowItem = OnShowRoadItem
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _left.chatCellList={}
    _topleft.go=UnityTools.FindGo(gameObject.transform, "Container/topleft")
    _bottom.go = UnityTools.FindGo(gameObject.transform, "Container/btm")
    _left.go = UnityTools.FindGo(gameObject.transform, "Container/left")
    _topright.go = UnityTools.FindGo(gameObject.transform, "Container/topright")
    _right.go = UnityTools.FindGo(gameObject.transform, "Container/right")
    _center.go = UnityTools.FindGo(gameObject.transform, "Container/center")
    
end
function RichCarStatusUpdate()
    -- LogError("ss"..CTRL.StatusInfo.status)
    if CTRL.StatusInfo.status == 1 then--等待
        -- _center.circle:ResetSelect()
        -- for i=1,#_center.selfNum do
        --     _center.selfNum[i]:SetValue(0,true)
        --     _center.allNum[i]:SetValue(0,true)
        -- end
        -- local temp={}
        -- for i=1,8 do
        --     if _continuedStakeTempTb[i] ~=nil then
        --         local tb={}
        --         tb.index = i
        --         tb.money = _continuedStakeTempTb[i]
        --         tb.self = 0
        --         temp[#temp+1] = tb
        --     end
        -- end
        -- if #temp>0 then
        --     _continuedStakeTb=temp
        -- else
        --     _continuedStakeTb={}
        -- end 
        -- _center.stakeObj:SetActive(false)
        -- _center.awardObj:SetActive(false)
        -- _center.colorObj:SetActive(false)
        -- _center.counterObj:SetActive(false)
        -- FlashCircle(_center.nowSelect)
    elseif CTRL.StatusInfo.status == 2 then--开始
        UpdateContinueBtnStatus(false)
        _center.circle:ResetSelect()
        _isStake=false
        for i=1,#_center.selfNum do
            _center.selfNum[i].text = "0"
            _center.allNum[i].text = "0"
            _center.selfBg[i].gameObject:SetActive(false)
        end
        local temp={}
        for i=1,8 do
            if _continuedStakeTempTb[i] ~=nil then
                local tb={}
                tb.index = i
                tb.money = _continuedStakeTempTb[i]
                tb.self = 0
                temp[#temp+1] = tb
            end
        end
        if #temp>0 then
            _continuedStakeTb=temp
            _continuedStakeTempTb={}
        else
            _continuedStakeTb={}
            _continuedStakeTempTb={} 
        end 
        _center.waitObj:SetActive(true)
        _center.stakeObj:SetActive(false)
        _center.awardObj:SetActive(false)
        _center.colorObj:SetActive(false)
        _center.counterObj:SetActive(true)
        --_center.circle:StartCircle(_center.nowSelect,10)
        FlashCircle(_center.nowSelect)
        if CTRL.StatusInfo.time ~= nil then
            _center.lbCool.cooldownSound = 0
            _center.lbCool:SetEndTime(CTRL.StatusInfo.time)
        end
    elseif CTRL.StatusInfo.status == 3 then --下注
        if _isChangeBgm then 
            UtilTools.SetBgm("Sounds/RichCar/bg")
        end
        UnityTools.PlaySound("Sounds/RichCar/startbet",{target=_center.go.gameObject})
        UnityTools.PlaySound("Sounds/RichCar/setchips",{target=_center.waitObj.gameObject})
        _center.startTween.enabled=true
        _center.startTween:ResetToBeginning()
        _center.startTween:PlayForward()
        _center.waitObj:SetActive(false)
        _center.areaTween2.enabled=true
        _center.areaTween2:PlayForward()
        UpdateContinueBtnStatus(true)
        
        -- _center.stakeObj:SetActive(true)
        -- _center.awardObj:SetActive(false)
        -- _center.colorObj:SetActive(false)
        -- _center.counterObj:SetActive(true)
        -- FlashCircle(_center.nowSelect)
        _center.counterObj:SetActive(false)
        _mask.gameObject:SetActive(true)
        if CTRL.StatusInfo.time ~= nil then
            _center.lbCool.cooldownSound = 5
            _center.lbCool:SetEndTime(CTRL.StatusInfo.time)
        end
    elseif CTRL.StatusInfo.status == 6 then --下注
        _center.waitObj:SetActive(false)
        _center.areaTween2.enabled=true
        _center.areaTween2:PlayForward()
        _center.stakeObj:SetActive(true)
        _center.awardObj:SetActive(false)
        _center.colorObj:SetActive(false)
        _center.counterObj:SetActive(true)
        FlashCircle(_center.nowSelect)
        _mask.gameObject:SetActive(false)

        UpdateContinueBtnStatus(true)

        if CTRL.StatusInfo.time ~= nil then
            _center.lbCool.cooldownSound = 5
            _center.lbCool:SetEndTime(CTRL.StatusInfo.time)
        end
    elseif CTRL.StatusInfo.status == 4 then --结算
        _isChangeBgm = true
        _noStakeCount = _noStakeCount + 1
        if _noStakeCount >=20 then
            CloseWin(nil)
        end
        UpdateContinueBtnStatus(false)
        if CTRL.Result.resultIndex == nil then 
            _center.waitObj:SetActive(true)
            _center.stakeObj:SetActive(false)
            _center.awardObj:SetActive(false)
            _center.colorObj:SetActive(false)
            _center.counterObj:SetActive(false)
            FlashCircle(_center.nowSelect)
            return
        end
        _mask.gameObject:SetActive(true)
        UnityTools.PlaySound("Sounds/RichCar/stopbet",{target=_center.go.gameObject})
        if CTRL.Result.result >=5 or CTRL.Result.poolSub~=0 then
            --UnityTools.PlaySound("Sounds/RichCar/roulette20f",_center.awardObj.gameObject)
            
        else
            UtilTools.SetBgm("Sounds/RichCar/roulette5")
        end
        _center.stopTween.enabled=true
        _center.stopTween:ResetToBeginning()
        _center.stopTween:PlayForward()
        _center.counterObj:SetActive(false)
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(protoIdSet.cs_car_syn_in_game_state_req,{})
    elseif CTRL.StatusInfo.status == 5 then
        _center.waitObj:SetActive(false)
        _center.stakeObj:SetActive(false)
        _center.counterObj:SetActive(false)
        UpdateContinueBtnStatus(false)
        if CTRL.Result.poolSub ~= nil then
            if CTRL.Result.poolSub < 0 then
        --彩池
                _center.awardObj:SetActive(false)
                _center.colorObj:SetActive(true)
                if CTRL.Result.selfNum >= 0 then
                    _center.color.lbSelfNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.selfNum))
                else
                    _center.color.lbSelfNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.selfNum))
                end
                if CTRL.Result.masterNum >= 0 then
                    _center.color.lbMasterNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.masterNum))
                else
                    _center.color.lbMasterNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.masterNum))
                end
                _center.color.lbColorNum = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.poolSub))
                _center.color.lbRewardNum = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.pool))
                _center.circle:StartColorCircle(_center.nowSelect-1,CTRL.Result.resultIndex-1)
            else
                _center.awardObj:SetActive(true)
                _center.colorObj:SetActive(false)
                if CTRL.Result.selfNum >= 0 then
                    _center.lbSelfNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.selfNum))
                else
                    _center.lbSelfNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.selfNum))
                end
                if CTRL.Result.masterNum >= 0 then
                    _center.lbMasterNum.text = LuaText.Format("rich_car_tip17",GetNumText(CTRL.Result.masterNum))
                else
                    _center.lbMasterNum.text = LuaText.Format("rich_car_tip18",GetNumText(CTRL.Result.masterNum))
                end
                UtilTools.loadTexture(_center.texCar,"UI/Texture/RichCar/tex"..CTRL.Result.result..".png",true)
                _center.spCarName.spriteName = tostring(CTRL.Result.result)
            end
        else
            _center.awardObj:SetActive(false)
            _center.colorObj:SetActive(false)
        end
    end
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    _thisObj=gameObject
    for i=1,5 do
        local cfg=LuaConfigMgr.GoldChipConfig[tostring(i)]
        if cfg ~=nil then
            _stakeNum[i] = tonumber(cfg.gold_chip)
        end
         
    end
    UtilTools.HideWaitFlag()
    AutoLuaBind(gameObject)
    InitParam()
    _center.Bind(_thisObj)
    gTimer.registerOnceTimer(100,"RichCarStep1")
    UnityTools.DestroyWin("MainWin")
    UnityTools.DestroyWin("MainCenterWin");
    UnityTools.DestroyWin("GameCenterWin");
    --_center.circle:StartCircle(1,10)
    --FlashCircle(_center.nowSelect)
end
function RichCarResultResponse()
    ResultResponse()
end
function RichCarUserListUpdate()
    UpdateOnlineList()
end
function RichCarTipUpdate()
    _tipTween.gameObject:SetActive(true)
    _tipTween.enabled = true
    _tipTween:PlayForward()
    _mask.gameObject:SetActive(true)
end
function UpdateRichCarRed()
    if _redtask.isRefresh == true then
        UpdateRedBg()
    end
end
local function Start(gameObject)
    
    
    
end
function RichCarStep1()
    _topleft.Bind(_thisObj)
    _topright.Bind(_thisObj)
    
    _center.circle:SetParam(0.15,5)
    _center.circle:SetEffectRender(_thisObj.transform,300)
    gTimer.registerOnceTimer(100,"RichCarStep2")
end
function RichCarStep2()
    _bottom.Bind(_thisObj)
    _left.Bind(_thisObj)
    _right.Bind(_thisObj)
    gTimer.registerOnceTimer(100,"RichCarStep3")
end
function RichCarStep3()
    local data1 ={}
    data1.content = LuaText.GetString("rich_car_tip33")
    data1.flag = 5
    table.insert(CTRL.ChatList,data1)
    data1={}
    data1.content = LuaText.GetString("rich_car_tip34")
    data1.flag = 5
    table.insert(CTRL.ChatList,data1)

    UpdateChatScroll()
    _controller:SetScrollViewRenderQueue(_left.chatScroll.gameObject)
    _controller:SetScrollViewRenderQueue(_right.recordScrollview.gameObject)
    _controller:SetScrollViewRenderQueue(_right.roadScrollview.gameObject)
    _controller:SetScrollViewRenderQueue(_right.onlineScrollview.gameObject)


    
    
    
    RichCarStatusUpdate()
    UpdateRoomInfo()
    _topleft.lbColor.text = GetNumText(CTRL.PoolNum)
    -- LogError("CTRL.PoolNum="..CTRL.PoolNum)
    UpdateRecordList()
    --UpdateRichCarTopLeftBtn()
    _right.recordTween.enabled=true
    _right.recordTween:PlayForward()
    OnClickBtnRoad(nil)
    UpdateBottomWin()
    UpdateRedBg()
    OnClickChip(_bottom.btnChips[1])
    UpdateGetNum()
    gTimer.registerOnceTimer(100,"RichCarStep4")
end
function UpdateRichCarPlayerInfo()
    UpdateBottomWin()
end
function RichCarStep4()
    _left.btm.startingRenderQueue = _left.chatScroll.panel.startingRenderQueue+10
    _left.emojiScroll.gameObject:GetComponent("UIPanel").startingRenderQueue=_left.btm.startingRenderQueue+1
    registerScriptEvent(EVENT_UPDATE_CHAT, "RichCarWinChatUpdate")
    registerScriptEvent(EVENT_RICHCAR_MASTER_UPDATE , "UpdateRichCarMasterInfo")
    registerScriptEvent(EVENT_RICHCAR_STAKE_RESPONSE , "RichCarStakeResponse")
    registerScriptEvent(EVENT_RICHCAR_STATUS_UPDATE , "RichCarStatusUpdate")
    registerScriptEvent(EVENT_RICHCAR_RESULT_RESPONSE , "RichCarResultResponse")
    registerScriptEvent(EVENT_RICHCAR_ROOM_RESPONSE , "UpdateRichCarRoomInfo")
    registerScriptEvent(EVENT_UPDATE_RICH_CAR_STATUS , "UpdateRichCarTopLeftBtn")
    registerScriptEvent(EVENT_UPDATE_RICH_CAR_USERLIST , "RichCarUserListUpdate")
    registerScriptEvent(EVENT_UPDATE_RICH_CAR_TIP , "RichCarTipUpdate")
    registerScriptEvent(EVENT_RED_WIN_UPDATE , "UpdateRichCarRed")
    registerScriptEvent(EVENT_RENDER_CHANGE_WIN , "RichCarWinRenderReset")
    registerScriptEvent(EVENT_RESCOURCE_UDPATE , "UpdateRichCarPlayerInfo")
    
    gTimer.registerOnceTimer(100,"RichCarStep5")
end
function RichCarStep5()
    UnityTools.DestroyWin("GameCenterWin")
    UnityTools.CreateLuaWin("RichCarRuleWin")
    UnityTools.DestroyWin("BroadcastWin")
end
function RichCarRenderChange()
    _left.btm.startingRenderQueue = _left.chatScroll.panel.startingRenderQueue+10
    _left.emojiScroll.gameObject:GetComponent("UIPanel").startingRenderQueue=_left.btm.startingRenderQueue+1
end
function RichCarWinRenderReset()
    _controller:SetScrollViewRenderQueue(_left.chatScroll.gameObject)
    _controller:SetScrollViewRenderQueue(_right.recordScrollview.gameObject)
    _controller:SetScrollViewRenderQueue(_right.roadScrollview.gameObject)
    _controller:SetScrollViewRenderQueue(_right.onlineScrollview.gameObject)
    gTimer.registerOnceTimer(100,"RichCarRenderChange")

end
local function OnDestroy(gameObject)
    gTimer.removeTimer("UpdateRichCarList")
    gTimer.removeTimer("RichCarStep1")
    gTimer.removeTimer("RichCarStep2")
    gTimer.removeTimer("RichCarStep3")
    gTimer.removeTimer("RichCarStep4")
    gTimer.removeTimer("RichCarStep5")
    gTimer.removeTimer("RichCarRenderChange")
    unregisterScriptEvent(EVENT_UPDATE_CHAT, "RichCarWinChatUpdate")
    unregisterScriptEvent(EVENT_RICHCAR_MASTER_UPDATE , "UpdateRichCarMasterInfo")
    unregisterScriptEvent(EVENT_RICHCAR_STAKE_RESPONSE , "RichCarStakeResponse")
    unregisterScriptEvent(EVENT_RICHCAR_STATUS_UPDATE , "RichCarStatusUpdate")
    unregisterScriptEvent(EVENT_RICHCAR_RESULT_RESPONSE , "RichCarResultResponse")
    unregisterScriptEvent(EVENT_RICHCAR_ROOM_RESPONSE , "UpdateRichCarRoomInfo")
    unregisterScriptEvent(EVENT_UPDATE_RICH_CAR_STATUS , "UpdateRichCarTopLeftBtn")
    unregisterScriptEvent(EVENT_UPDATE_RICH_CAR_USERLIST , "RichCarUserListUpdate")
    unregisterScriptEvent(EVENT_UPDATE_RICH_CAR_TIP , "RichCarTipUpdate")
    unregisterScriptEvent(EVENT_RED_WIN_UPDATE , "UpdateRichCarRed")
    unregisterScriptEvent(EVENT_RENDER_CHANGE_WIN , "RichCarWinRenderReset")
    unregisterScriptEvent(EVENT_RESCOURCE_UDPATE , "UpdateRichCarPlayerInfo")
    CLEAN_MODULE(wName .. "Mono")
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
