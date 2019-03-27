-- -----------------------------------------------------------------


-- *
-- * Filename:    RankWinMono.lua
-- * Summary:     RankWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        7/6/2017 1:28:50 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RankWinMono")



-- 界面名称
local wName = "RankWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local mainCityCtrl = IMPORT_MODULE("MainWinController")
local rankInfoCtrl = IMPORT_MODULE("RankInfoWinController")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local _leftgrid
local _type1
local _type2
local _lbrank
local _lbdesc
local _scrollview
local _cellMgr1
local _cellMgr2
local _leftbtncell
local _btnClose
local sdasdadad
local _lbDescRt
--- [ALD END]

local _go

local _tabTable={"icon1","icon3","icon2"}
local _tabGo={}
local _tabSp={}
local _tabLb={}
local _selectIndex=0
local _listData={}

local protobuf = sluaAux.luaProtobuf.getInstance()
local function GetNumText2(num)
    local newNum=0
    num=tonumber(num)
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

local function OnClickInfo(gameObject)
    rankInfoCtrl.OpenWinByType(_selectIndex)
    
end

--- [ALF END]

local function ShowWinByIndex(isFresh)
    if isFresh==nil then
        isFresh =false
    end
    local myRank = -1;
    local cellMgr = nil
    if _selectIndex == 1 then
        _listData=mainCityCtrl.RankList[4]
        if _listData == nil then
            _listData={}
        end
        myRank=mainCityCtrl.MyRank[4]    
        UnityTools.SetActive(_type1,true)
        UnityTools.SetActive(_type2,false)
        _lbdesc.text = LuaText.GetString("rank_desc2")
        _lbDescRt.text = LuaText.GetString("rank_desc12")
        cellMgr = _cellMgr1
        _cellMgr1.gameObject:SetActive(true)
        _cellMgr2.gameObject:SetActive(false)
    elseif _selectIndex == 2 then
        _listData=mainCityCtrl.RankList[5]
        if _listData == nil then
            _listData={}
        end
        myRank=mainCityCtrl.MyRank[5]    
        UnityTools.SetActive(_type1,true)
        UnityTools.SetActive(_type2,false)
        _lbdesc.text = LuaText.GetString("rank_desc3")
        _lbDescRt.text = LuaText.GetString("rank_desc4")
        cellMgr = _cellMgr1
        _cellMgr1.gameObject:SetActive(true)
        _cellMgr2.gameObject:SetActive(false)
    elseif _selectIndex == 3 then
        _listData=CTRL.RankTable
        UnityTools.SetActive(_type1,false)
        UnityTools.SetActive(_type2,true)
        _lbDescRt.text = LuaText.GetString("rank_desc4")
        cellMgr = _cellMgr2
        _cellMgr1.gameObject:SetActive(false)
        _cellMgr2.gameObject:SetActive(true)
    end
    if myRank~=nil and  myRank>0 and myRank <100 then
        _lbrank.text = myRank
    else
        _lbrank.text=LuaText.GetString("rank_desc10") 
    end
    if cellMgr ~= nil then
        local delCount = 0
        if _listData ~= nil then
            delCount = cellMgr.CellCount - #_listData
            if delCount>=0 then
                for i=1,delCount do
                    cellMgr:DelteLastNode()
                end
                cellMgr:UpdateCells()
            else
                cellMgr:ClearCells()
                for i=1,#_listData do
                    cellMgr:NewCellsBox(cellMgr.Go)
                end
                cellMgr.Grid:Reposition()
                cellMgr:UpdateCells()
            end
            if isFresh then
                _scrollview:ResetPosition()
            end
        end
    end
     
end


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local function OnClickTab(gameObject)
    local comData = gameObject:GetComponent("ComponentData")
    if comData==nil then
        return;
    end
    if _selectIndex ~= comData.Id then
        
        if _selectIndex~=0 and _tabSp[_selectIndex]~=nil then
            _tabSp[_selectIndex].spriteName = "tabBg1"
            _tabLb[_selectIndex].text = LuaText.GetString("rank_tab".._selectIndex)
        end
        _selectIndex = comData.Id
        if _tabSp[_selectIndex]~=nil then
            _tabSp[_selectIndex].spriteName = "tabBg2"
            _tabLb[_selectIndex].text = LuaText.GetString("rank_tab_sel".._selectIndex)
        end
        if _selectIndex == 1 then
            protobuf:sendMessage(protoIdSet.cs_rank_query_req, {rank_type=4})
        elseif _selectIndex == 2 then
            protobuf:sendMessage(protoIdSet.cs_rank_query_req, {rank_type=5})
        elseif _selectIndex == 3 then
            protobuf:sendMessage(protoIdSet.cs_hundred_last_week_rank_query_req, {})
        end
        ShowWinByIndex(true)
    end
end
local function SetHead(head,icon,vip,isSelf,sex)
    local headImg = head.transform:Find("headImg"):GetComponent("UISprite")
    if headImg == nil then
        return
    end
    if icon~=nil and icon ~="" then
        local headtex= headImg.transform:Find("Texture"):GetComponent("UITexture")
        if headtex == nil then
            return
        end
        headtex.mainTexture = nil
        UnityTools.SetPlayerHead(icon,headtex,isSelf)
    else
        local headtex= headImg.transform:Find("Texture"):GetComponent("UITexture")
        if headtex ~= nil then
            headtex.mainTexture = nil
        end
        local _platformMgr = IMPORT_MODULE("PlatformMgr");
        headImg.spriteName = _platformMgr.PlayerDefaultHead(sex)
    end
    
    UnityTools.SetNewVipBox(headImg.transform:Find("vip/vipBox"),vip,"vip")
end

local function OnShowItem(cellbox, index, item)
    index = index + 1
    local data = _listData[index]
    if data == nil then
        return
    end
    local rankIcon = UnityTools.FindGo(item.transform, "bg/rank"):GetComponent("UISprite")
    local lbRank = UnityTools.FindGo(item.transform, "bg/ranklb"):GetComponent("UILabel") 
    local type1 = UnityTools.FindGo(item.transform, "type1")
    local type2 = UnityTools.FindGo(item.transform, "type2")
    local lbnum = nil
    local lbName = nil
    if _selectIndex == 1 or _selectIndex == 2 then
        -- type1.transform.localPosition = UnityEngine.Vector3(0,0,0)
        -- type2.transform.localPosition = UnityEngine.Vector3(30000,0,0)
        lbnum = UnityTools.FindGo(type1.transform, "w1/num"):GetComponent("UILabel") 
        lbName = UnityTools.FindGo(type1.transform, "name"):GetComponent("UILabel") 
        local lbReward =  UnityTools.FindGo(type1.transform, "Label/num"):GetComponent("UILabel")
        local awardData =nil
        if _selectIndex == 1 then
            awardData = LuaConfigMgr.RewardRankingConfig[tostring(index)]
        else
            awardData = LuaConfigMgr.TotalRankingConfig[tostring(index)]
        end
        if awardData ~= nil then
            lbReward.text = GetNumText2(awardData.reward)
        else
            if _selectIndex == 1 then
                awardData = LuaConfigMgr.RewardRankingConfig[tostring(11)]
            else
                awardData = LuaConfigMgr.TotalRankingConfig[tostring(11)]
            end
            lbReward.text = GetNumText2(awardData.reward)
        end
        lbName.text = data.player_name
        lbnum.text = GetNumText(data.hundred_win)
        SetHead(UnityTools.FindGo(type1.transform, "head").transform, data.player_icon, data.player_vip,data.player_uuid == platformMgr.PlayerUuid(),data.sex)
        rankIcon.transform.localPosition= UnityEngine.Vector3(-288,15,0)
        lbRank.transform.localPosition = UnityEngine.Vector3(-288,7,0)
    elseif _selectIndex == 3 then
        -- type1.transform.localPosition = UnityEngine.Vector3(30000,0,0)
        -- type2.transform.localPosition = UnityEngine.Vector3(0,0,0)
        lbnum = UnityTools.FindGo(type2.transform, "ic/num"):GetComponent("UILabel") 
        --lbName1 = UnityTools.FindGo(type2.transform, "name1"):GetComponent("UILabel")
        local lbName2 = UnityTools.FindGo(type2.transform, "name2"):GetComponent("UILabel")
        lbnum.text = GetNumText2(data.reward_gold)
        --lbName1.text = data.name1_round_win
        lbName2.text = data.name2_total_win
        rankIcon.transform.localPosition= UnityEngine.Vector3(-288,6,0)
        lbRank.transform.localPosition = UnityEngine.Vector3(-288,-2,0)
    else
        -- type1.transform.localPosition = UnityEngine.Vector3(30000,0,0)
        -- type2.transform.localPosition = UnityEngine.Vector3(30000,0,0)
    end
    if index == 1 then
        lbRank.transform.localPosition = UnityEngine.Vector3(-26000,0,0)
        rankIcon.spriteName = "rank1"
    elseif index == 2 then
        lbRank.transform.localPosition = UnityEngine.Vector3(-26000,0,0)
        rankIcon.spriteName = "rank2"
    elseif index == 3 then
        lbRank.transform.localPosition = UnityEngine.Vector3(-26000,0,0)
        rankIcon.spriteName = "rank3"
    else
        rankIcon.transform.localPosition= UnityEngine.Vector3(-26000,7,0)
        lbRank.text = tostring(index)
    end

end
local function InitTab()
    local go = nil;
    local spIcon = nil;
    for i=1,#_tabTable do
        go = NGUITools.AddChild(_leftgrid.gameObject,_leftbtncell.gameObject)
        _tabSp[i] = UnityTools.FindGo(go.transform, "bg"):GetComponent("UISprite")
        _tabLb[i] = UnityTools.FindGo(go.transform, "name"):GetComponent("UILabel")
        spIcon = UnityTools.FindGo(go.transform, "icon"):GetComponent("UISprite")
        if i == 1 then
            UnityTools.SetActive(UnityTools.FindGo(go.transform, "Sprite"),true)
            _controller:SetScrollViewRenderQueue(UnityTools.FindGo(go.transform, "Sprite/Panel").gameObject)
        else
            UnityTools.SetActive(UnityTools.FindGo(go.transform, "Sprite"),false)
        end
        _tabLb[i].text = LuaText.GetString("rank_tab"..i)
        spIcon.spriteName = _tabTable[i]
        UnityTools.AddOnClick(go, OnClickTab)
        ComponentData.Get(go).Id = i
        _tabGo[i] = go
    end
    OnClickTab(_tabGo[1])
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _leftgrid = UnityTools.FindGo(gameObject.transform, "Container/left/grid")

    _type1 = UnityTools.FindGo(gameObject.transform, "Container/right/type1")

    _type2 = UnityTools.FindGo(gameObject.transform, "Container/right/type2")

    _lbrank = UnityTools.FindGo(gameObject.transform, "Container/right/type1/myrank/lbrank"):GetComponent("UILabel")

    _lbdesc = UnityTools.FindGo(gameObject.transform, "Container/right/type1/desc"):GetComponent("UILabel") 

    _scrollview = UnityTools.FindGo(gameObject.transform, "Container/right/bg/list/scrollview"):GetComponent("UIScrollView") 

    _cellMgr1 = UnityTools.FindGo(gameObject.transform, "Container/right/bg/list/scrollview/grid1"):GetComponent("UIGridCellMgr")
    _cellMgr1.onShowItem = OnShowItem
    _cellMgr2 = UnityTools.FindGo(gameObject.transform, "Container/right/bg/list/scrollview/grid2"):GetComponent("UIGridCellMgr")
    _cellMgr2.onShowItem = OnShowItem
    _leftbtncell = UnityTools.FindGo(gameObject.transform, "leftbtncell")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/close")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    
    UnityTools.AddOnClick(_lbdesc.gameObject, OnClickInfo)

    _lbDescRt = UnityTools.FindGo(gameObject.transform, "Container/right/desc"):GetComponent("UILabel")

--- [ALB END]




end
function OnRankInfoUpdate()
    ShowWinByIndex(false)
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    _go = gameObject
    AutoLuaBind(gameObject)
    registerScriptEvent(EVENT_UPDATE_RANK_INFO, "OnRankInfoUpdate")
end


local function Start(gameObject)
    InitTab()
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_UPDATE_RANK_INFO, "OnRankInfoUpdate")
    CLEAN_MODULE("RankWinMono")
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
