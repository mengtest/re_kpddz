-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitPoolWinMono.lua
-- * Summary:     FruitPoolWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/18/2017 11:06:54 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitPoolWinMono")



-- 界面名称
local wName = "FruitPoolWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _poolNum
local _winBg
local _btnClose
local _lbGetNUm
local _lbName
local _head
local _headImg
local _headTexture
local _vip
local _rightObj
--- [ALD END]


local _thisObj

local _historyScrollview
local _cellmgr
local _lbTime
local _spVip
local _super
local _lbValues={}
local _arrow
local _rightObj
--- [ALD END]


local _bPlay = false


local _normal

local infoTb={}

local FruitController = IMPORT_MODULE("FruitWinController") 

local function GetNumText(num)
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
local function GetSplitNumStr(num)
    if num >= 1000000 then
        return math.floor(num/1000000)..","..string.format("%03d",math.floor((num%1000000)/1000))..","..string.format("%03d",num%1000000%1000)
    elseif num >=1000 then
        return math.floor(num/1000)..","..string.format("%03d",num%1000)
    else 
        return tostring(num)
    end
end
local function SetData()
    if #CTRL.PoolInfo>0 then
        infoTb.name=CTRL.PoolInfo[1].player_name
        infoTb.headImg=CTRL.PoolInfo[1].icon_url
        infoTb.getNum=CTRL.PoolInfo[1].win_gold
        LogError(" CTRL.PoolInfo[1].vip_level=".. CTRL.PoolInfo[1].vip_level)
        infoTb.vip = CTRL.PoolInfo[1].vip_level
        infoTb.timeStr =  os.date("%m-%d %H:%M",CTRL.PoolInfo[1].c_time)
    else
        infoTb.name=""
        infoTb.headImg=1
        infoTb.getNum=0
        infoTb.timeStr = ""
        infoTb.vip =0
        _lbGetNUm.gameObject:SetActive(false)
        _lbName.gameObject:SetActive(false)
        _head.gameObject:SetActive(false)
        _rightObj.gameObject:SetActive(false)
        
    end
end
local function OnAnimationFinish()
    _arrow.gameObject:SetActive(false)
    for j=1,3 do
        _lbValues[j].value1:AddValue(_lbValues[j].tovalue1,false)  
        _lbValues[j].value2:AddValue(_lbValues[j].tovalue2,false)  
        _lbValues[j].value3:AddValue(_lbValues[j].tovalue3,false)  
    end
    UnityTools.PlaySound("Sounds/Laba/numscroll",{target=_poolNum.gameObject})
end
local function UpdateSuperValue()
    _arrow.gameObject:SetActive(true)
    for i=5,7 do
        local data = LuaConfigMgr.SuperLineConfig[tostring(i)];
        if data ~= nil then
            local subdata =LuaConfigMgr.LineNumConfig[tostring(data.gear)]
            if subdata ~= nil then
                local j= i-4
                LogError("j="..j)
                _lbValues[j].tovalue1 = tonumber(data.rate1) - tonumber(subdata.rate1)
                _lbValues[j].tovalue2 = tonumber(data.rate2)- tonumber(subdata.rate2)
                _lbValues[j].tovalue3 = tonumber(data.rate3)- tonumber(subdata.rate3)
                _lbValues[j].num.text = data.gold_bet
                _lbValues[j].value1:SetValue(tonumber(subdata.rate1),true)  
                _lbValues[j].value2:SetValue(tonumber(subdata.rate2),true)  
                _lbValues[j].value3:SetValue(tonumber(subdata.rate3),true)
            end
        end
    end
    UnityTools.PlaySound("Sounds/Laba/upupup",{target=_super.gameObject})
    gTimer.registerOnceTimer(2000,OnAnimationFinish)
end
local function UpdateList()
    local delCount = _cellmgr.CellCount - #CTRL.PoolInfo-1
    if delCount>=0 then
        for i=1,delCount do
            _cellmgr:DelteLastNode()
        end
        _cellmgr:UpdateCells()
    else
        _cellmgr:ClearCells()
        for i=1,#CTRL.PoolInfo-1 do
            _cellmgr:NewCellsBox(_cellmgr.Go)    
                --mainScrollView:Reposition()
        end
        _cellmgr.Grid:Reposition()
        _cellmgr:UpdateCells()
    end
end
--- [ALF END]
local function UpdateWin()

    if FruitController.isSuperShow then
        _super.gameObject:SetActive(true)
        if not _bPlay then
            _bPlay =true
            gTimer.registerOnceTimer(200,UpdateSuperValue)
        end
        
        _normal.gameObject:SetActive(false)

    else
        _super.gameObject:SetActive(false)
        _normal.gameObject:SetActive(true)
    end
    _poolNum.text = GetSplitNumStr(FruitController.BaseInfo.PoolNum)
    LogError("infoTb.getNum="..infoTb.getNum)
    if infoTb.getNum == "" then

    end
    _lbGetNUm.text = GetSplitNumStr(tonumber(infoTb.getNum))
    _lbName.text = infoTb.name
--    if infoTb.icon_url ~= "" then
    local isPlayer = infoTb.player_uuid == _platformMgr.PlayerUuid();
    UnityTools.SetPlayerHead(infoTb.icon_url, _headTexture,isPlayer);
--    end
    UnityTools.SetNewVipBox(_vip, infoTb.vip,"vip",_thisObj,0.5*0.8);
    -- UnityTools.SetNewVipBox(_vip, 10,"vip",_thisObj,0.4);
    if isPlayer then
        _headImg.spriteName = _platformMgr.PlayerDefaultHead()
    end
    _lbTime.text =  infoTb.timeStr
    UpdateList()
end
local function OnShowHistoryItem(cellbox, index, item)
    index = index+2
    local data= CTRL.PoolInfo[index]
    if data ~= nil then
        UnityTools.FindGo(item.transform,"bg/vip"):GetComponent("UISprite").spriteName="v"..data.vip_level
        UnityTools.FindGo(item.transform,"bg/name"):GetComponent("UILabel").text=data.player_name
        UnityTools.FindGo(item.transform,"bg/money"):GetComponent("UILabel").text=GetNumText(data.win_gold)
        UnityTools.FindGo(item.transform,"bg/lbTime"):GetComponent("UILabel").text= os.date("%m-%d\n%H:%M",CTRL.PoolInfo[index].c_time)
    end
end

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _thisObj = gameObject
    _poolNum = UnityTools.FindGo(gameObject.transform, "Container/bg/pool/info"):GetComponent("UILabel")

    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/close")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _lbGetNUm = UnityTools.FindGo(gameObject.transform, "Container/bg/right/bg/Sprite/num"):GetComponent("UILabel")

    _lbName = UnityTools.FindGo(gameObject.transform, "Container/head/name"):GetComponent("UILabel")

    _head = UnityTools.FindGo(gameObject.transform, "Container/head")

    _headImg = UnityTools.FindCo(gameObject.transform,"UISprite", "Container/head/headImg")
    _vip  = UnityTools.FindGo(gameObject.transform,"Container/head/headImg/vip/vipBox")

    _headTexture = UnityTools.FindGo(gameObject.transform, "Container/head/headImg/Texture"):GetComponent("UITexture")

    _rightObj = UnityTools.FindGo(gameObject.transform, "Container/bg/right/bg/Sprite")

--- [ALB END]


    _historyScrollview = UnityTools.FindGo(gameObject.transform, "Container/bg/right/list/scrollview"):GetComponent("UIPanel")

    _cellmgr = UnityTools.FindGo(gameObject.transform, "Container/bg/right/list/scrollview/cellmgr"):GetComponent("UIGridCellMgr")
    _cellmgr.onShowItem = OnShowHistoryItem
    _lbTime = UnityTools.FindGo(gameObject.transform, "Container/bg/right/bg/Sprite/lbTime"):GetComponent("UILabel")
    
    _spVip = UnityTools.FindGo(gameObject.transform, "Container/bg/right/bg/Sprite/namebg/vip"):GetComponent("UISprite")

    _normal = UnityTools.FindGo(gameObject.transform, "Container/bg/btm")


    _super = UnityTools.FindGo(gameObject.transform, "Container/bg/super")

    for i=1,3 do
        _lbValues[i] = {}
        _lbValues[i].num = UnityTools.FindGo(gameObject.transform, "Container/bg/super/num"..i):GetComponent("UILabel")
        for j=1,3 do
            _lbValues[i]["value"..j] = UnityTools.FindGo(_lbValues[i].num.transform, "value"..j):GetComponent("LabelRunning")
        end
    end
    _arrow = UnityTools.FindGo(gameObject.transform, "Container/bg/super/arrow")

end
function UpdateFruitPoolNum()
    _poolNum.text = GetSplitNumStr(FruitController.BaseInfo.PoolNum)
    SetData()
    UpdateWin()
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg)
    SetData()
    UpdateWin()
    _controller:SetScrollViewRenderQueue(_historyScrollview.gameObject)
    registerScriptEvent(EVENT_LABA_POOL_UPDATE, "UpdateFruitPoolNum")
end


local function OnDestroy(gameObject)
    gTimer.removeTimer(OnAnimationFinish)
    gTimer.removeTimer(UpdateSuperValue)
    unregisterScriptEvent(EVENT_LABA_POOL_UPDATE, "UpdateFruitPoolNum")
    CLEAN_MODULE(wName .. "Mono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
