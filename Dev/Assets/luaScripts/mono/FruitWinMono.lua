-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitWinMono.lua
-- * Summary:     FruitWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/17/2017 10:18:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitWinMono")



-- 界面名称
local wName = "FruitWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _go;

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local BigRwardCtrl=IMPORT_MODULE("BigRewardWinController")
local _mainObj={}
local _pointCell
local _pointPanel
local _btnClose
local _btnPool
local _scrollview
local _lbAward
local _lbGold
local _lbName
local _head
local _btnTip
local _lbFreeTime
local _winBg
local _moveTable={}
local _rowcell
local _headImg
local _headtex
local _btn2
local _btn3
local _menuLayer
local _poolnumrun

local _btnFreeGold
local _lbCoolDown
local _taskObj={}
local _btnBox
local _boxRed
local _redTaskData 
local _redTaskObj={}
local _poolRed
--- [ALD END]

local roomMgr = IMPORT_MODULE("roomMgr")









local _platformMgr = IMPORT_MODULE("PlatformMgr")


local _beforeAuto=false

local _winStartRenderQueue

local points={} -- 5行  7列 
local lines={}
local _fruitIcons={} -- 5列 5行


local _pressNormalTime=0

local _perLine={}
local _lineNum={}
local _allLineNum=0
local _pressTime=0
local _statusTable={}
local _positions={} -- 5列 5行
--local PerRowCost=0.09
local PerRowCost=0.08
local DefautRuns=1
local _rewardLineArray={}
local thisObj
local _tempBackArray={}
local _PlayIndex=0
local _bRoll=false
local _freeObj
local _menuState=0
local _freeGet=0
local _boxTaskTb={}
local _changeTb = {}
_changeTb.bPlay = false


local function ClickMenuBtn(gameObject)
    if _menuState == 0 then
        _menuLayer:SetActive(true)
        _menuState = 1
    else
        _menuLayer:SetActive(false)
        _menuState = 0
    end
end
function FruitRecvGameTimesUpdate()
    if _bRoll then return end
    _boxTaskTb.UpdateBoxTask()
end
-- local function OnClickCloseRedTask(gameObject)
--     _redTaskObj.isShow=false
--     _redTaskObj.close.gameObject:SetActive(false)
--     _redTaskObj.tweenPosition:ResetToBeginning()
--     _redTaskObj.taskbg.transform.localPosition=UnityEngine.Vector3(-412,0,0)
--     _redTaskObj.tweenPosition.from = UnityEngine.Vector3(-412,0,0)
--     _redTaskObj.tweenPosition.to = UnityEngine.Vector3(-665,0,0)
--     _redTaskObj.tweenPosition.duration = 0.5
--     _redTaskObj.tweenPosition:PlayForward()
-- end
-- local function OnClickRedTask(gameObject)
--     if _redTaskData==nil then return end
--     if _redTaskData.status == 2 and  _redTaskObj.isShow then
--         if _bRoll then
--             return
--         end 
--         local req={}
--         req.game_type = 2
--         req.task_id = _redTaskData.index
--         local protobuf = sluaAux.luaProtobuf.getInstance()
--         UtilTools.ShowWaitFlag()
--         protobuf:sendMessage(protoIdSet.cs_redpack_task_draw_req, req)
--         return
--     end
--     if _redTaskObj.isShow then
--         return
--     end
--     _redTaskObj.isShow= true
--     _redTaskObj.tweenPosition:ResetToBeginning()
--     _redTaskObj.taskbg.transform.localPosition=UnityEngine.Vector3(-665,0,0)
--     _redTaskObj.tweenPosition.from = UnityEngine.Vector3(-665,0,0)
--     _redTaskObj.tweenPosition.to = UnityEngine.Vector3(-412,0,0)
--     _redTaskObj.tweenPosition.duration =  0.5
--     _redTaskObj.tweenPosition:PlayForward()
-- end
-- local function OnMoveRedFinish()
--     if _redTaskData==nil then return end
--     if _redTaskObj.isShow then
--         _redTaskObj.close.gameObject:SetActive(true)
--         _redTaskObj.red.gameObject:SetActive(false)
--     else
--         _redTaskObj.close.gameObject:SetActive(false)
--         if _redTaskData.status==2 then
--             _redTaskObj.red.gameObject:SetActive(true)
--         else
--             _redTaskObj.red.gameObject:SetActive(false)
--         end
--     end
-- end

local function UpdatePlyaerGold(num)
    _lbGold.text = num
    _boxTaskTb.UpdateBoxTask()
end
function UpdateLabaRedTask()
    -- if _bRoll then return end
    -- UpdatePlyaerGold(_platformMgr.GetGod())
    -- for i=1,#CTRL.RedTable do
    --     if  CTRL.RedTable[i].status~=1 then
    --         _redTaskData = CTRL.RedTable[i]
    --         break
    --     end
    -- end

    -- if _redTaskData == nil then
    --     _redTaskObj.tweenPosition.enabled=false
    --     _redTaskObj.taskbg.transform.localPosition=UnityEngine.Vector3(-30000,0,1)
    --     return
    -- end
    -- if _redTaskObj.taskbg.transform.localPosition.z == 1 then
    --     _redTaskObj.taskbg.transform.localPosition=UnityEngine.Vector3(-665,0,0)
    -- end
    -- if _redTaskData.status == 2 then
    --     _redTaskObj.status.gameObject:SetActive(true)
    --     _redTaskObj.slider.gameObject:SetActive(false)
        
    -- else
    --     _redTaskObj.status.gameObject:SetActive(false)
    --     _redTaskObj.slider.gameObject:SetActive(true)
    --     _redTaskObj.slider.value = CTRL.TotalGold/tonumber(_redTaskData.total_gold)
    --     _redTaskObj.sliderNum.text = LuaText.Format("strength_desc11",UnityTools.GetShortNum(CTRL.TotalGold),UnityTools.GetShortNum(_redTaskData.total_gold))
    -- end
    -- if _redTaskObj.isShow then
    --     _redTaskObj.red.gameObject:SetActive(false)
    --     _redTaskObj.close.gameObject:SetActive(true)
    -- else
    --     if _redTaskData.status == 2 then
    --         _redTaskObj.red.gameObject:SetActive(true)
    --     else
    --         _redTaskObj.red.gameObject:SetActive(false)
    --     end
    --     _redTaskObj.close.gameObject:SetActive(false)
    -- end
    
    -- _redTaskObj.desc.text =  LuaText.Format("fruit_box_tip5",UnityTools.GetShortNum(_redTaskData.total_gold))
    -- _redTaskObj.num.text = tostring(_redTaskData.red_packet)
end


local function LoopLabaLine()
    for i=1,#CTRL.AwardInfo.RewardList do
         if CTRL.AwardInfo.RewardList[i].line_id<=#lines then
            local go= lines[CTRL.AwardInfo.RewardList[i].line_id].line.gameObject
            if i ~= _PlayIndex then
                go:SetActive(false)
                if _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id] ~=nil then
                    for j=1,#_tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].backObj do
                        _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].backObj[j].gameObject:SetActive(false)
                    end
                    --LogError("#_tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj.."..#_tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj)
                    for k=1,#_tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj do
                        _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj[k].gameObject:SetActive(false)
                    end                        
                end 
            else
                
                go:SetActive(true)
                --local comDate = go:GetComponent("ComponentData")
                local tweenAlpha =  go:GetComponent("TweenAlpha")
                tweenAlpha.enabled=true
                tweenAlpha:ResetToBeginning()
                --tweenAlpha.style=TweenAlpha.Style.Loop
                tweenAlpha.from = 1
                tweenAlpha.to=0
                tweenAlpha.duration = 1
                tweenAlpha.delay = 0
            end
         end
    end
    if _tempBackArray[CTRL.AwardInfo.RewardList[_PlayIndex].line_id] ~=nil then
        for k=1,#_tempBackArray[CTRL.AwardInfo.RewardList[_PlayIndex].line_id].backObj do
            _tempBackArray[CTRL.AwardInfo.RewardList[_PlayIndex].line_id].backObj[k].gameObject:SetActive(true)
        end
        for k=1,#_tempBackArray[CTRL.AwardInfo.RewardList[_PlayIndex].line_id].effectobj do
            _tempBackArray[CTRL.AwardInfo.RewardList[_PlayIndex].line_id].effectobj[k].gameObject:SetActive(true)
        end                        
    end
end
local function OnAlphaComplete()
    local comData = TweenAlpha.current.gameObject:GetComponent("ComponentData")
    if comData==nil then
        return
    end
    local tweenAlpha =  comData.gameObject:GetComponent("TweenAlpha")  
    if tweenAlpha == nil then
        return
    end 
    if comData.Tag == 1 then
        tweenAlpha.enabled=true
        tweenAlpha:ResetToBeginning()
        comData.gameObject:GetComponent("UIWidget").alpha=1
        tweenAlpha.delay = 0.2
        --tweenAlpha.style=TweenAlpha.Style.Once
        tweenAlpha.from = 1
        tweenAlpha.to = 0
        tweenAlpha.duration = 0.2
        comData.Tag=comData.Tag+1
        tweenAlpha:PlayForward()   
    elseif comData.Tag == 2 then
        tweenAlpha.enabled =false
        comData.gameObject:SetActive(false)
        comData.gameObject:GetComponent("UIWidget").alpha=1
        comData.Tag=comData.Tag+1
    elseif comData.Tag == 3 then
        _PlayIndex = _PlayIndex + 1
        if _PlayIndex > #CTRL.AwardInfo.RewardList then
            _PlayIndex=1
        end
        LoopLabaLine()
        
    end
end
function ShowBigRwardWin()
    BigRwardCtrl.LabelValue = CTRL.AwardInfo.RewardNum
    BigRwardCtrl.bDelay=false
    BigRwardCtrl.bMask=true
    BigRwardCtrl.StillTime=3000
    BigRwardCtrl.bCloseTitle=true
    if CTRL.AwardInfo.bPool then
        BigRwardCtrl.Type=0
        BigRwardCtrl.Sound=""
        UnityTools.PlaySound("Sounds/Laba/pool1",{target=thisObj.gameObject})
        UnityTools.PlaySound("Sounds/Laba/pool2",{target=_winBg.gameObject})
    elseif CTRL.AwardInfo.RewardNum ~= 0 then--CTRL.AwardInfo.bFree==0 then
        local maxSame=0
        for i=1,#CTRL.AwardInfo.RewardList do
            if maxSame < CTRL.AwardInfo.RewardList[i].same_num then
                maxSame=CTRL.AwardInfo.RewardList[i].same_num
            end
        end
        if maxSame>=5 or  CTRL.AwardInfo.RewardNum/_allLineNum>=10 then
            BigRwardCtrl.Type=3
            BigRwardCtrl.Sound2="Sounds/Laba/type2"
            --UnityTools.PlaySound("Sounds/Laba/type3",thisObj.gameObject)
            BigRwardCtrl.Sound="Sounds/Laba/newgoldresult"
            --UnityTools.PlaySound("Sounds/Laba/gold",_winBg.gameObject)
        elseif maxSame >=4 or  (CTRL.AwardInfo.RewardNum/_allLineNum<10 and CTRL.AwardInfo.RewardNum>=_allLineNum)  then
            BigRwardCtrl.Type=2
            BigRwardCtrl.Sound2="Sounds/Laba/type2"
            --UnityTools.PlaySound("Sounds/Laba/type2",thisObj.gameObject)
            BigRwardCtrl.Sound="Sounds/Laba/newgoldresult"
            --UnityTools.PlaySound("Sounds/Laba/gold",_winBg.gameObject)
        else
            BigRwardCtrl.Type=1
            BigRwardCtrl.Sound2="Sounds/Laba/type1"
            --UnityTools.PlaySound("Sounds/Laba/type1",thisObj.gameObject)
            BigRwardCtrl.Sound="Sounds/Laba/newgoldresult"
            --UnityTools.PlaySound("Sounds/Laba/gold",_winBg.gameObject)
        end
    else
        UnityTools.PlaySound("Sounds/Laba/goldresult",{target=thisObj.gameObject})
        BigRwardCtrl.Free=CTRL.AwardInfo.bFree
        BigRwardCtrl.Sound=""
        CTRL.AwardInfo.bFree=0--只弹1次
    end
    UnityTools.CreateLuaWin("BigRewardWin")
end
local function LoopShowLine()
    if #CTRL.AwardInfo.RewardList >0 then
        _PlayIndex=1
    end
    for i=1,#CTRL.AwardInfo.RewardList do
        if CTRL.AwardInfo.RewardList[i].line_id<=#lines then
            local go= lines[CTRL.AwardInfo.RewardList[i].line_id].line.gameObject
            go:SetActive(true)
            lines[CTRL.AwardInfo.RewardList[i].line_id].renderLine:MoveEffect(1)
        end
    end
    gTimer.registerOnceTimer(1000,"ShowBigRwardWin")
    
end 
function FlashLabaLine()
    for i=1,15 do
        if i <= _lineNum.num then
            lines[i].line.gameObject:SetActive(true)
            local tweenAlpha =  lines[i].line.gameObject:GetComponent("TweenAlpha")
            local comData = lines[i].line.gameObject:GetComponent("ComponentData")
            if comData~=nil then
                comData.Tag=1
            end
            tweenAlpha.enabled=true
            --tweenAlpha.style=TweenAlpha.Style.Once
            tweenAlpha:ResetToBeginning()
            tweenAlpha.from = 0
            tweenAlpha.delay = 0
            tweenAlpha.to = 1
            tweenAlpha.duration = 0.3
            tweenAlpha:PlayForward()
        else
            lines[i].line.gameObject:SetActive(false)
        end
        --lines[i].linePoint.gameObject:SetActive(false)
    end
end
local function StartRoolFlashLine()
    gTimer.registerOnceTimer(200,"FlashLabaLine")
    for i=1,15 do
        if i <= _lineNum.num then
            lines[i].line.gameObject:SetActive(false)
        else
            lines[i].line.gameObject:SetActive(false)
        end
    end
end


local function OnLoadComplete(efoObj)
    
    efoObj.EffectGameObj.gameObject.name="effectzhongjiang"
    UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform,efoObj.EffectGameObj.transform,4)
    efoObj.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
    efoObj.EffectGameObj.gameObject:SetActive(false)
    
    --_tempBackArray[efoObj.param_int].effectobj[#_tempBackArray[efoObj.param_int].effectobj+1]=efoObj.EffectGameObj.transform
end
local function HideEffect()
    for k,v in pairs(_tempBackArray) do
        --LogError(k.."#backObj"..#v.effectobj )
        for j=1,#v.backObj do 
            v.backObj[j].gameObject:SetActive(false)
        end
        for i=1,#v.effectobj do
            v.effectobj[i].gameObject:SetActive(false)
        end
    end
    _tempBackArray={}
end
local function ShowEffect()
    for i=1,#CTRL.AwardInfo.RewardList do
        if CTRL.AwardInfo.RewardList[i].line_id <= #_rewardLineArray then
            local samenum=CTRL.AwardInfo.RewardList[i].same_num
            for j=1,samenum do
                if j<=#_rewardLineArray[CTRL.AwardInfo.RewardList[i].line_id].icons then
                    local backObj = _rewardLineArray[CTRL.AwardInfo.RewardList[i].line_id].icons[j].transform:Find("kuang")
                    if backObj~=nil then
                        if _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id] == nil then
                            _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id] = {}
                            _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].backObj={}
                            _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj={}
                        end
                        _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].backObj[#_tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].backObj+1] =backObj
                        --_tempBackArray[#_tempBackArray+1]=backObj
                        backObj.gameObject:SetActive(true)
                    end
                    local effectobj=_rewardLineArray[CTRL.AwardInfo.RewardList[i].line_id].icons[j].transform:Find("effectzhongjiang")
                    if _rewardLineArray[CTRL.AwardInfo.RewardList[i].line_id].icons[j].transform:Find("effectzhongjiang") == nil then
                        local obj= UnityTools.AddEffect(_rewardLineArray[CTRL.AwardInfo.RewardList[i].line_id].icons[j].transform,"effect_kuang",{destroy = false,complete=OnLoadComplete})
                        obj.param_int = CTRL.AwardInfo.RewardList[i].line_id
                    else
                        _tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj[#_tempBackArray[CTRL.AwardInfo.RewardList[i].line_id].effectobj+1]=effectobj
                        UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform,effectobj.transform,4)
                        effectobj.gameObject:SetActive(true)
                    end
                end
            end
        end
    end
end
local function SetFruitIcon(sprite,id,index)
    if id <=0 or id >11 then
        math.randomseed(tonumber(tostring(os.time()+index):reverse():sub(1,6)))
        id = math.random(11)
        --id = math.random(4)
        local configData = LuaConfigMgr.LabaFruitConfig[tostring(id)]
        if configData ~=nil then
            sprite.spriteName = configData.icon
        end
    else
        local configData = LuaConfigMgr.LabaFruitConfig[tostring(id)]
        if configData ~=nil then
            sprite.spriteName = configData.icon
        end
    end
    
end
local function InitWin()

    _lbName.text = _platformMgr.UserName()
    local playerIcon = _platformMgr.GetIcon();
    if playerIcon ~= nil and playerIcon ~= "" then
        UnityTools.SetPlayerHead(playerIcon, _headtex, true);
    else
        _headImg.spriteName = _platformMgr.PlayerDefaultHead()
    end
    -- UnityTools.SetNewVipBox(_headImg.transform:Find("vip/vipBox"), _platformMgr.GetVipLv(),"vip",thisObj);
    -- UnityTools.SetNewVipBox(_headImg.transform:Find("vip/vipBox"), 10,"vip",thisObj);
end
local function UpdateAwardNum()
    _lbAward.text = CTRL.AwardInfo.RewardNum
end
local function RollCells()
    HideEffect()
    StartRoolFlashLine()
    UnityTools.PlaySound("Sounds/Laba/roll",{delTime=2,target=thisObj.gameObject})
    _bRoll=true
end

local function UpdateButtonStuats()
    _mainObj._btnFree.gameObject:SetActive(_statusTable.Free)
    _mainObj._freeStick.gameObject:SetActive(_statusTable.Free)

    _mainObj._btnNormal.gameObject:SetActive(_statusTable.Normal)
    _mainObj._normalStick.gameObject:SetActive(_statusTable.Normal or _statusTable.Auto)

    _mainObj._btnAuto.gameObject:SetActive(_statusTable.Auto)
    
end

local function OnClickFree(gameObject)
    if _bRoll then return end
    UnityTools.PlaySound("Sounds/Laba/spin",{target=gameObject})
    _mainObj._freeStick.PlayAction()
end
function freeShowBigRwardWin()
    UnityTools.CreateLuaWin("BigRewardWin")
end
local function InitStatusTable()
    if _bRoll then
        return
    end
    _lbFreeTime.text=CTRL.BaseInfo.FreeTimes
    if _statusTable.Free then
        if _freeGet>=0 then
            _freeGet = _freeGet+CTRL.AwardInfo.RewardNum
        end
        if CTRL.BaseInfo.FreeTimes == 0 and _freeGet > 0 then
            BigRwardCtrl.LabelValue = _freeGet
            BigRwardCtrl.bDelay=false
            BigRwardCtrl.bMask=true
            BigRwardCtrl.StillTime=3000
            BigRwardCtrl.bCloseTitle=true
            BigRwardCtrl.Type=4
            BigRwardCtrl.Sound="Sounds/Laba/newgoldresult"
            BigRwardCtrl.Sound2="Sounds/Laba/type2"
            _freeGet = -1
            gTimer.registerOnceTimer(1000,"freeShowBigRwardWin") 
            _bRoll = true
            return 
        end
    end
    if CTRL.BaseInfo.FreeTimes >0 then
        _statusTable.Free=true
        _statusTable.Normal=false
        if _beforeAuto==false then
            if _statusTable.Auto then
                _beforeAuto=true
            else 
                _beforeAuto=false
            end
        end
        _statusTable.Auto=false
        _freeObj:SetActive(true)
        OnClickFree(nil)
    elseif _statusTable.Free == true then
        _freeGet = 0
        _statusTable.Free=false
        if _beforeAuto then
            _statusTable.Normal=false
            _statusTable.Auto=true
        else
            _statusTable.Normal=true
            _statusTable.Auto=false
        end
        _freeObj:SetActive(false)
    elseif _statusTable.Auto == true then
        _statusTable.Normal=false
        _statusTable.Free=false
        _freeObj:SetActive(false)
        return
    else
        _statusTable.Free=false
        _statusTable.Normal=true
        _statusTable.Auto=false
        _freeObj:SetActive(false)
    end
    UpdateButtonStuats()
end

function UpdateLabaPoolNum()
    if _bRoll then return end
    _mainObj._lbPoolNum:AddValue(CTRL.BaseInfo.PoolNum-_mainObj._lbPoolNum:GetValue())
    --_mainObj._lbPoolNum.text = tostring(CTRL.BaseInfo.PoolNum)
    
end

function LabaWinAutoSpin()
    if CTRL.AwardInfo.bFree>0 then
        BigRwardCtrl.bDelay=false
        BigRwardCtrl.bMask=true
        BigRwardCtrl.StillTime=3000
        BigRwardCtrl.bCloseTitle=true
        BigRwardCtrl.Free=CTRL.AwardInfo.bFree
        CTRL.AwardInfo.bFree = 0
        BigRwardCtrl.Sound=""
        UnityTools.PlaySound("Sounds/Laba/pool1",{target=thisObj.gameObject})
        UnityTools.PlaySound("Sounds/Laba/pool2",{target=_winBg.gameObject})
        gTimer.registerOnceTimer(1000,"freeShowBigRwardWin")
        -- UnityTools.CreateLuaWin("BigRewardWin")
        return
    end
    UpdatePlyaerGold(_platformMgr.GetGod())
    UpdateAwardNum()
    _bRoll=false
    UpdateLabaPoolNum()
    UpdateLabaRedTask()
    InitStatusTable()
    if _statusTable.Free then
        OnClickFree(nil)
        return
    end
    if _statusTable.Auto then
        _mainObj._normalStick:SetMode(true)
    else
        if #CTRL.AwardInfo.RewardList >0 then
            ShowEffect()
        end
        if #CTRL.AwardInfo.RewardList > 1 then
            LoopLabaLine()
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
local function UpdateLines(isFirst)
    if #lines == 0 then
        return
    end
    for i=1,15 do
        local tweenAlpha = lines[i].line.gameObject:GetComponent("TweenAlpha")
        lines[i].line.gameObject:GetComponent("UIWidget").alpha=1
        if tweenAlpha ~=nil then
            tweenAlpha.enabled=false
        end
        if isFirst == true then
            lines[i].line.gameObject:SetActive(false)
        elseif i <= _lineNum.num then
            lines[i].line.gameObject:SetActive(true)
            --lines[i].linePoint.gameObject:SetActive(true)
        else
            lines[i].line.gameObject:SetActive(false)
            --lines[i].linePoint.gameObject:SetActive(false)
        end
    end
end
local function UpdateLabels(isFirst)
    if _lineNum.lbNum ~= nil then
        _lineNum.lbNum.text = GetSplitNumStr(_lineNum.num)
    end
    if _perLine.lbNum ~= nil then
        _perLine.lbNum.text = GetSplitNumStr(_perLine.num)
    end
    _allLineNum = _lineNum.num*_perLine.num
    _mainObj._lbAllLineNum.text = GetSplitNumStr(_allLineNum)
    UpdateLines(isFirst)
end
_lineNum.OnClickLeft = function (gameObject)
    if _bRoll then return end
    if _statusTable.Free==true then
        UtilTools.ShowMessage(LuaText.GetString("fruit_win_info2"),"[FFFFFF]")
        return
    end
    local maxLine = 9 
    if CTRL.isSuperShow then
        maxLine = 15
    end

    if _lineNum.num<=maxLine and _lineNum.num>1 then
        _lineNum.num=_lineNum.num-1
        UpdateLabels()
    elseif _lineNum.num == 1 then
        _lineNum.num=maxLine
        UpdateLabels()
    end 
end
_lineNum.OnClickRight = function (gameObject)
    if _bRoll then return end
    if _statusTable.Free==true then
        UtilTools.ShowMessage(LuaText.GetString("fruit_win_info2"),"[FFFFFF]")
        return
    end
    local maxLine = 9 
    if CTRL.isSuperShow then
        maxLine = 15
    end
    if _lineNum.num<maxLine and _lineNum.num>=0 then
        _lineNum.num=_lineNum.num+1
        UpdateLabels()
    elseif _lineNum.num == maxLine then
        _lineNum.num=1
        UpdateLabels()
    end 
end

_perLine.OnClickLeft = function (gameObject)
    if _bRoll then return end
    if _statusTable.Free==true then
        UtilTools.ShowMessage(LuaText.GetString("fruit_win_info2"),"[FFFFFF]")
        return
    end
    local lineCfg = LuaConfigMgr.LineNumConfig
    local maxIndex = LuaConfigMgr.LineNumConfigLen
    if CTRL.isSuperShow then
       lineCfg = LuaConfigMgr.SuperLineConfig
       maxIndex = LuaConfigMgr.SuperLineConfigLen
    end
    if _perLine.keyIndex<=maxIndex and _perLine.keyIndex>1 then
        _perLine.keyIndex = _perLine.keyIndex-1
        local lineConfig = lineCfg[tostring(_perLine.keyIndex)]
        _perLine.num = tonumber(lineConfig.gold_bet)
        UpdateLabels()
    elseif _perLine.keyIndex==1 then 
        _perLine.keyIndex = maxIndex
        local lineConfig = lineCfg[tostring(_perLine.keyIndex)]
        _perLine.num = tonumber(lineConfig.gold_bet)
        UpdateLabels()
    end 
end

_perLine.OnClickRight = function (gameObject)
    if _bRoll then return end
    if _statusTable.Free==true then
        UtilTools.ShowMessage(LuaText.GetString("fruit_win_info2"),"[FFFFFF]")
        return
    end
    local lineCfg = LuaConfigMgr.LineNumConfig
    local maxIndex = LuaConfigMgr.LineNumConfigLen
    if CTRL.isSuperShow then
       lineCfg = LuaConfigMgr.SuperLineConfig
       maxIndex = LuaConfigMgr.SuperLineConfigLen
    end
    if _perLine.keyIndex<maxIndex and _perLine.keyIndex>=1 then
        _perLine.keyIndex = _perLine.keyIndex+1
        local lineConfig = lineCfg[tostring(_perLine.keyIndex)]
        _perLine.num = tonumber(lineConfig.gold_bet)
        UpdateLabels()
    elseif _perLine.keyIndex== maxIndex then 
        _perLine.keyIndex = 1
        local lineConfig = lineCfg[tostring(_perLine.keyIndex)]
        _perLine.num = tonumber(lineConfig.gold_bet)
        UpdateLabels()
    end 
end


local function Update(gameObject)
    if _pressNormalTime > 0 then
        _pressNormalTime=_pressNormalTime+gTimer.deltaTime()
        if _pressNormalTime >= 0.6 then
            _statusTable.Auto = true
            _statusTable.Normal = false
            _statusTable.Free = false
            if not _bRoll then _mainObj._normalStick:SetMode(true) end
            
            UpdateButtonStuats()
            _pressNormalTime=0
        end
    end
end
local function OnClickPool(gameObject)
    UnityEngine.PlayerPrefs.SetInt("PoolWinRedIsShow",1)
    UnityTools.SetActive( _poolRed.gameObject,false)
    local protobuf = sluaAux.luaProtobuf.getInstance()
    if CTRL.isSuperShow then
        protobuf:sendMessage(protoIdSet.cs_win_player_list,{type = 2})
    else
        protobuf:sendMessage(protoIdSet.cs_win_player_list,{type = 1})
    end
    UnityTools.CreateLuaWin("FruitPoolWin")
end

local function OnClickTip(gameObject)
    if CTRL.isSuperShow then
        UnityTools.CreateLuaWin("FruitSuperAwardWin")
    else
        UnityTools.CreateLuaWin("FruitAwardWin")
    end
end

local function ClickSetBtnCall(gameObject)
    _platformMgr.OpenSetWin()
end
--- [ALF END]





local function CloseWin(gameObject)
    if _bRoll then return end
    _platformMgr.gameMgr.closeActiveFun = function()
        UnityTools.CallLoadingWin(false)
        UnityTools.DestroyWin("FruitWin");

    end
    local type =1 
        if CTRL.isSuperShow then
            type = 2
        end
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_laba_leave_room_req,{type=type})
    
    UnityTools.ReturnToMainCity()
    --[[gTimer.registerOnceTimer(500,function()
        UnityTools.CallLoadingWin(false)
        UnityTools.DestroyWin(wName);
     end)]]

end
function FruitCloseFunc()
    if UI.Controller.UIManager.IsWinShow("FruitWin") then
        roomMgr.bExiting = true
    end
    UnityTools.CallLoadingWin(true)
    
    _platformMgr.gameMgr.closeActiveFun = function()
        -- UtilTools.RemoveAllWinExpect()
        local type =1 
        if CTRL.isSuperShow then
            type = 2
        end
        local protobuf = sluaAux.luaProtobuf.getInstance()
        protobuf:sendMessage(protoIdSet.cs_laba_leave_room_req,{type=type})
        roomMgr.bExiting = false
        UnityTools.CallLoadingWin(false)
        UI.Controller.UIManager.RemoveAllWinExpect({"Waiting","MainCenterWin","MainWin"})
        --UnityTools.DestroyWin(wName)
    end
    UnityTools.DestroyWin("FruitWin");
    -- UnityTools.ReturnToMainCity()
    
end
local function OnFreeDragOver()
    if _bRoll then return end
    UtilTools.ShowWaitFlag()
    local protobuf = sluaAux.luaProtobuf.getInstance()
    local req ={}
    req.line_num = _lineNum.num
    req.line_set_chips = _perLine.keyIndex
    if CTRL.isSuperShow then
        req.type = 2
    else
        req.type = 1
    end
    protobuf:sendMessage(protoIdSet.cs_laba_spin_req,req)
    --LogError("OnFreeDragOver"..gameObject)
end
local function openGoldShopPanel()
    local shopCtrl = IMPORT_MODULE("ShopWinController");
    if shopCtrl ~= nil then
        shopCtrl.OpenShop(1)
    end
end

local function OnClickAuto(gameObject)
    _statusTable.Auto = false
    _statusTable.Normal = true
    _statusTable.Free = false
    _mainObj._normalStick:SetMode(false)
    UpdateButtonStuats()
end
local function OnNormalDragOver()
    if _bRoll then return end
    if _changeTb.UpdateSuperBtn() == true then
        return
    end
    if _allLineNum == 0 then
        UtilTools.ShowMessage(LuaText.GetString("fruit_win_info1"),"[FFFFFF]")
        if _statusTable.Auto then
            OnClickAuto(nil)
        end
        return
    elseif _allLineNum > _platformMgr.GetGod() then
        UnityTools.MessageDialog(LuaText.GetString("fruit_win_info4"),{okCall=openGoldShopPanel})
        if _statusTable.Auto then
            OnClickAuto(nil)
        end
        return
    end
    _mainObj._normalStick:SetMode(false)
    local protobuf = sluaAux.luaProtobuf.getInstance()
    UpdatePlyaerGold(_platformMgr.GetGod()-_allLineNum)
    UtilTools.ShowWaitFlag()
    local req ={}
    req.line_num = _lineNum.num
    req.line_set_chips = _perLine.keyIndex
    if CTRL.isSuperShow then

        req.type = 2
    else
        req.type = 1
    end

    protobuf:sendMessage(protoIdSet.cs_laba_spin_req,req)
end
local function OnClickNormal(gameObject)
    if _bRoll then return end
    UnityTools.PlaySound("Sounds/Laba/spin",{target=gameObject})
    if _allLineNum == 0 then
        UtilTools.ShowMessage(LuaText.GetString("fruit_win_info1"),"[FFFFFF]")
        return
    elseif _allLineNum > _platformMgr.GetGod() then
        UnityTools.MessageDialog(LuaText.GetString("fruit_win_info4"),{okCall=openGoldShopPanel})
        return
    end
    OnNormalDragOver()
    --_mainObj._normalStick:PlayAction()
end
local function OnPressNormal(gameObject,status)
    if status == true then
        _pressNormalTime=0.1
    else   
        _pressNormalTime=0
    end
end
local function OnClickShop(gameObject)
    if _bRoll then return end
    openGoldShopPanel()
end

local function UpdateFruitTaskInfo()
    _boxRed.gameObject:SetActive(false)
    local isBoxTaskAllGet= true
    -- for i=1,#CTRL.BoxTable.boxStatus do
    --     if CTRL.BoxTable.boxStatus[i] == 1 then
    --         _boxRed.gameObject:SetActive(true)
    --         isBoxTaskAllGet = false
    --         break
    --     end
    -- end    
    if CTRL.TaskTable.taskId == nil then
        _btnFreeGold.gameObject:SetActive(false)
        return
    end
    local taskConfigData = LuaConfigMgr.FruitDayConfig[tostring(CTRL.TaskTable.taskId)]
    if taskConfigData==nil then
         _btnFreeGold.gameObject:SetActive(false)
        return
    end
    
    if _platformMgr.GetVipLv()<tonumber(taskConfigData.account_level) then
        
        _taskObj.comObj.gameObject:SetActive(true)
        _taskObj._taskSlider.gameObject:SetActive(false)
        _taskObj._taskStatus.gameObject:SetActive(false)
        _taskObj._taskDesc.gameObject:SetActive(false)
        _taskObj._taskIcon.gameObject:SetActive(false)
        _taskObj._taskNum.gameObject:SetActive(false)
        _taskObj._taskRed.gameObject:SetActive(false)
        if isBoxTaskAllGet then --宝箱任务奖励领取完 提示领取宝箱任务
            _taskObj.comObj.text = LuaText.Format("fruit_win_info10",tonumber(taskConfigData.account_level))
        else
            _taskObj.comObj.text = LuaText.GetString("fruit_win_info9")
        end
    end
    if CTRL.TaskTable.status==2 then
        _btnFreeGold.gameObject:SetActive(false)
        _taskObj.comObj.gameObject:SetActive(true)
        _taskObj._taskSlider.gameObject:SetActive(false)
        _taskObj._taskStatus.gameObject:SetActive(false)
        _taskObj._taskDesc.gameObject:SetActive(false)
        _taskObj._taskIcon.gameObject:SetActive(false)
        _taskObj._taskNum.gameObject:SetActive(false)
        _taskObj._taskRed.gameObject:SetActive(false)
        return
    end

    _btnFreeGold.gameObject:SetActive(true)
    
    _taskObj._taskIcon.gameObject:SetActive(true)
    _taskObj._taskDesc.gameObject:SetActive(true)
    _taskObj._taskNum.gameObject:SetActive(true)
    _taskObj.comObj.gameObject:SetActive(false)
    _taskObj._lbCoolDown.gameObject:SetActive(false)
    -- if CTRL.TaskTable.remaindTime ~= nil then
    --     _taskObj._lbCoolDown:SetEndTime(CTRL.TaskTable.remaindTime)
    -- end
    _taskObj._taskIcon.spriteName = "C"..taskConfigData.item1_id
    _taskObj._taskNum.text = UnityTools.GetShortNum(tonumber(taskConfigData.item1_num))
    _taskObj._taskDesc.text = taskConfigData.desc
    -- _taskObj._taskBg.width = 105 + _taskObj._taskDesc.width
    if _taskObj.collider==nil then
        _taskObj.collider = _taskObj._taskBg:GetComponent("BoxCollider")
    end
    -- _taskObj.collider.size=UnityEngine.Vector3(_taskObj._taskBg.width,_taskObj.collider.size.y,_taskObj.collider.size.z)
    -- _taskObj.collider.center=UnityEngine.Vector3(_taskObj._taskBg.width/2,0,0)
    --_taskObj._taskBg:ResizeCollider()
    if CTRL.TaskTable.status == 1 then
        _taskObj._taskSlider.gameObject:SetActive(false)
        _taskObj._taskStatus.gameObject:SetActive(true)
        -- _taskObj._taskStatus.transform.localPosition = UnityEngine.Vector3(_taskObj._taskBg.width-20,_taskObj._taskStatus.transform.localPosition.y,_taskObj._taskStatus.transform.localPosition.z)
        _taskObj._taskRed.gameObject:SetActive(true)
        
    elseif CTRL.TaskTable.status==0 then
        _taskObj._taskSlider.gameObject:SetActive(true)
        _taskObj._taskStatus.gameObject:SetActive(false)
        -- _taskObj._taskSliderSprite.width=_taskObj._taskDesc.width-7
        -- _taskObj._taskBackSprite.width = _taskObj._taskDesc.width-7
        _taskObj._sliderNum.text = CTRL.TaskTable.process.."/"..taskConfigData.achieve_condition_param1s
        -- _taskObj._sliderNum.transform.localPosition=UnityEngine.Vector3(_taskObj._taskDesc.width/2-_taskObj._sliderNum.width/2,_taskObj._sliderNum.transform.localPosition.y,_taskObj._sliderNum.transform.localPosition.z)
        
        _taskObj._taskSlider.value=CTRL.TaskTable.process/taskConfigData.achieve_condition_param1s
        _taskObj._taskRed.gameObject:SetActive(false)

    end
    
end
local function OnMoveFinsh()
    UpdateFruitTaskInfo()
    if CTRL.AwardInfo.RewardNum>0 or CTRL.AwardInfo.bFree>0 then
        LoopShowLine()
    else
        _bRoll=false
        UpdateLabaPoolNum()
        InitStatusTable()
        UpdatePlyaerGold(_platformMgr.GetGod())
        UpdateAwardNum()
        if _statusTable.Free then
            OnClickFree(nil)
        end
        if _statusTable.Auto then
            _mainObj._normalStick:SetMode(true)
        end
        UpdateLabaRedTask()
    end
end

local function StartRoll()
    for i=1 ,5 do
        for j = 1,5 do
            _moveTable[i].icons[j].transform.parent = _moveTable[i].cells[j].transform
            _moveTable[i].icons[j].transform.localPosition = UnityEngine.Vector3(0,0,0)
            _moveTable[i].nowIcon = j
        end
        _moveTable[i].round=0
        _moveTable[i].tweenPosition:ResetToBeginning()
        _moveTable[i].tweenPosition.gameObject:SetActive(true)
        _moveTable[i].tweenPosition.from = UnityEngine.Vector3(_moveTable[i].startPositon.x,_moveTable[i].startPositon.y,0)
        _moveTable[i].tweenPosition.to = UnityEngine.Vector3(_moveTable[i].startPositon.x,_moveTable[i].startPositon.y - _moveTable[i].distance,0)
        _moveTable[i].tweenPosition.duration = _moveTable[i].duration 
        _moveTable[i].tweenPosition.enabled=true
        _moveTable[i].tweenPosition.delay = _moveTable[i].delay
        _moveTable[i].tweenPosition:PlayForward()
        
    end
    
    
end

local function OnMoveUpdate(gameObject,value)
    local comData = gameObject:GetComponent("ComponentData")
    if comData==nil then
        return
    end
    local index = comData.Id
    local hasMoved =- gameObject.transform.localPosition.y+_moveTable[index].startPositon.y
    local rounds = math.floor(hasMoved/111)
    if rounds > _moveTable[index].round then -- 替换图标
        for i=1,rounds-_moveTable[index].round do 
               
            local rounddis= _moveTable[index].totalRound - _moveTable[index].round - i + 1
            if rounddis <=5 and rounddis >2 then
                local rd = (rounddis-3) * 5 + index
                SetFruitIcon(_moveTable[index].icons[_moveTable[index].nowIcon%5+1],CTRL.BaseInfo.FruitList[rd].fruit_type,0)
                if rounddis == 3 then
                    UnityTools.PlaySound("Sounds/Laba/col"..index,{target=gameObject})
                end
            else
                SetFruitIcon(_moveTable[index].icons[_moveTable[index].nowIcon%5+1],0,rounds*10+index)
            end 
            if _moveTable[index].nowIcon+1 > #_moveTable[index].cells then
                return
            end
            _moveTable[index].icons[_moveTable[index].nowIcon%5+1].transform.parent = _moveTable[index].cells[_moveTable[index].nowIcon+1].transform
            _moveTable[index].icons[_moveTable[index].nowIcon%5+1].transform.localPosition = UnityEngine.Vector3(0,0,0)
            _moveTable[index].nowIcon=_moveTable[index].nowIcon+1
        end
        _moveTable[index].round = rounds
    end 

end
local function OnClickGetAward(gameObject)
    if CTRL.TaskTable.status ~= 1 or _bRoll then
        return
    end
    UtilTools.ShowWaitFlag()
    local protobuf = sluaAux.luaProtobuf.getInstance()
    local req ={}
    req.task_id = CTRL.TaskTable.taskId
    req.game_type = 2
    protobuf:sendMessage(protoIdSet.cs_game_task_draw_req,req)
    _taskObj.isFresh=true
end
local function OnClickTask(gameObject)
    _taskObj._taskBg.gameObject:SetActive(not _taskObj._taskBg.gameObject.activeSelf)
end 

local function OnClickBox(gameObject)
    UnityTools.CreateLuaWin("FruitBoxWin")
end

local function DrawLines()
    for i=1,15 do
         lines[i].renderLine=UnityTools.FindGo(lines[i].line.transform,"line"):GetComponent("DrawLine")
         lines[i].renderLine:SetLineColor(lines[i].colorIndex,_winStartRenderQueue+80)
         for j=1,#lines[i].lineArray do
            lines[i].renderLine:SetPosition(lines[i].lineArray[j].transform.position)
         end
         lines[i].renderLine:DrawRenderLine()
    end
end

local function resetRenderQ()
    UtilTools.SetEffectRenderQueueByUIParent(_go.transform,_freeObj.transform,80)
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
    _winStartRenderQueue = _go:GetComponent("UIPanel").startingRenderQueue
    _pointPanel.startingRenderQueue=_winStartRenderQueue+60
    for i=1, 15 do
         lines[i].renderLine:SetLineColor(lines[i].colorIndex,_winStartRenderQueue+80)
    end
    UnityTools.SetNewVipBox(_headImg.transform:Find("vip/vipBox"), _platformMgr.GetVipLv(),"vip",thisObj);
 end
 function _changeTb.OnSuperShowFinish()
    if _changeTb.showIndex == 1 then
        
        _changeTb.superShowScale:ResetToBeginning()
        _changeTb.superShowScale.transform.localScale = UnityEngine.Vector3(1,1,1) 
        _changeTb.superShowScale.from = UnityEngine.Vector3(1.1,1.1,1.1)
        _changeTb.superShowScale.to = UnityEngine.Vector3(0,0,0)
        _changeTb.superShowScale.delay = 2
        _changeTb.superShowScale.enabled = true
        _changeTb.superShowScale.duration = 0.3
        _changeTb.superShowScale:PlayForward()
        
        _changeTb.showIndex = 2
        _changeTb.superShowPos:ResetToBeginning()
        _changeTb.superShowPos.transform.localPosition = UnityEngine.Vector3(0,0,0)
        _changeTb.superShowPos.from = _changeTb.superShowPos.transform.localPosition
        _changeTb.superShowPos.to = UnityEngine.Vector3(-30,365,0)
        _changeTb.superShowPos.delay = 2
        _changeTb.superShowPos:PlayForward()
    else
        _changeTb.superShowScale.enabled = false
        _changeTb.superShowPos.enabled = false
        _changeTb.superShowPos.transform.localPosition = UnityEngine.Vector3(-2000,0,0)
        _changeTb.superShowScale.transform.localScale = UnityEngine.Vector3(1,1,1) 
        _mainObj._lbPoolNum:SetValue(CTRL.BaseInfo.PoolNum,false)
        -- _changeTb.tip.gameObject:SetActive(true)
        _changeTb.bPlay = false
        UnityTools.PlaySound("Sounds/Laba/numscroll",{target = _mainObj._lbPoolNum.gameObject})
    end
end
function _changeTb.UpdateSuperMode()
    _lineNum.num=CTRL.BaseInfo.lineNum
    _perLine.keyIndex=CTRL.BaseInfo.lineSetChips
    
    if CTRL.isSuperShow then
        local lineConfig = LuaConfigMgr.SuperLineConfig[tostring(_perLine.keyIndex)]
        
        _perLine.num = tonumber(lineConfig.gold_bet)
        InitStatusTable()
        -- UpdateLabaPoolNum()
        UpdateLabels()
        InitWin()
        _changeTb.bPlay = true
        _changeTb.showIndex = 1
        _changeTb.superShowPos.from = UnityEngine.Vector3(-1113,0,0)
        _changeTb.superShowPos:ResetToBeginning()
        _changeTb.superShowPos.to = UnityEngine.Vector3(0,0,0)
        _changeTb.superShowPos.duration = 0.5
        _changeTb.superShowPos.delay = 0
        _changeTb.superShowPos:PlayForward()
        _changeTb.superShowPos.enabled = true
        UnityTools.PlaySound("Sounds/Laba/upupup",{target = _changeTb.superShowPos.gameObject})
        
        UtilTools.loadTexture(_changeTb.bg2,"UI/Texture/Fruit/new_bg2.png",true)
        _changeTb.numbg1.spriteName = "newnum"
        _changeTb.numbg2.spriteName = "newnum"
        _changeTb.btnSuperMode.gameObject:SetActive(true)
        -- _changeTb.normalbtn.gameObject:SetActive(true)
        _changeTb.super.gameObject:SetActive(false)
        _changeTb.btnSuperRank.gameObject:SetActive(true)
        _taskObj.grid:Reposition()
    else
        local lineConfig = LuaConfigMgr.LineNumConfig[tostring(_perLine.keyIndex)]
        _perLine.num = tonumber(lineConfig.gold_bet)
        InitStatusTable()
        -- UpdateLabaPoolNum()
        UpdateLabels()
        InitWin()
        UpdateLabaPoolNum()
        UtilTools.loadTexture(_changeTb.bg2,"UI/Texture/Fruit/bg2.png",true)
        _changeTb.numbg1.spriteName = "numbg"
        _changeTb.numbg2.spriteName = "numbg"
        _changeTb.btnSuperMode.gameObject:SetActive(false)
        _changeTb.super.gameObject:SetActive(true)
        _changeTb.btnSuperRank.gameObject:SetActive(false)
        _taskObj.grid:Reposition()
    end
end
function _changeTb.OnClickSuperMode(gameObject)
    if _changeTb.bPlay == true or _statusTable.Auto == true then
        return
    end
    if UtilTools.GetServerTime() >= CTRL.IsOpenSuper then
        return
    end
    if _bRoll then
        return
    end
    local protobuf = sluaAux.luaProtobuf.getInstance()
    if CTRL.isSuperShow then
        CTRL.ToRoomType = 1
        protobuf:sendMessage(protoIdSet.cs_laba_leave_room_req,{type = 2})
        -- protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{type = 1})
    else
        CTRL.ToRoomType = 2
        protobuf:sendMessage(protoIdSet.cs_laba_leave_room_req,{type = 1})
        -- protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{type = 2})
    end
end
function _changeTb.UpdateSuperBtn()
    if UtilTools.GetServerTime() >= CTRL.IsOpenSuper then
        _changeTb.btnSuperMode.gameObject:SetActive(false)
        _changeTb.super.gameObject:SetActive(false)
        if CTRL.isSuperShow == true then
            CTRL.isSuperShow = not CTRL.isSuperShow
            _lineNum.num=CTRL.BaseInfo.lineNum
            _perLine.keyIndex=CTRL.BaseInfo.lineSetChips
            local lineConfig = LuaConfigMgr.LineNumConfig[tostring(_perLine.keyIndex)]

            _perLine.num = tonumber(lineConfig.gold_bet)
            _statusTable.Auto = false
            _statusTable.Normal = true
            _statusTable.Free = false
            _normalStick:SetMode(false)
            UpdateButtonStuats()
            InitStatusTable()
            UpdateLabels()
            InitWin()
            _btnTip.gameObject:SetActive(true)
            _changeTb.tip.gameObject:SetActive(false)
            UpdateLabaPoolNum()
            local protobuf = sluaAux.luaProtobuf.getInstance()
            protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{type = 1})
            UtilTools.loadTexture(_changeTb.bg2,"UI/Texture/Fruit/bg2.png",true)

            _changeTb.numbg1.spriteName = "numbg"
            _changeTb.numbg2.spriteName = "numbg"
            _changeTb.normalbtn.gameObject:SetActive(false)
            _changeTb.super.gameObject:SetActive(true)
            return true
        else
            return false
        end
    end
    return false
end
function _changeTb.OnClickSuperRank(gameObject)
    -- local rankCtrl= IMPORT_MODULE("RankWinController")
    _changeTb.rankred.gameObject:SetActive(false)
    -- if rankCtrl ~= nil then
        -- rankCtrl.ShowIndex = 7
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_rank_query_req,{rank_type = 6})
    
    UnityTools.CreateLuaWin("FruitRankWin")
    -- end
end

local timerFun = {}

function timerFun.StartWait()
    InitStatusTable()
    _mainObj._lbPoolNum:SetValue(CTRL.BaseInfo.PoolNum,true)
    UpdateLabels(true)
    UpdateAwardNum()
    InitWin()
    UpdatePlyaerGold(_platformMgr.GetGod())
    UpdateFruitTaskInfo()
    UpdateLabaRedTask()
    _winStartRenderQueue = _go:GetComponent("UIPanel").startingRenderQueue
    DrawLines()
    resetRenderQ()
    registerScriptEvent(EVENT_RENDER_CHANGE_WIN, "OnFruitWinRenderQChange")
end

function timerFun.moveTable()
    local distance=111
    for i=1,5 do
        _moveTable[i]={}
        _moveTable[i].tweenPosition= UnityTools.FindGo(_go.transform, "Container/bg2/scrollview/points/move"..i):GetComponent("TweenPosition")
        --        _moveTable[i].tweenPosition.gameObject:SetActive(false)
        _moveTable[i].tweenPosition.enabled=false
        _moveTable[i].cells={}
        _moveTable[i].icons={}
        _moveTable[i].icons[1] = _fruitIcons[i][4]
        _moveTable[i].icons[2] = _fruitIcons[i][3]
        _moveTable[i].icons[3] = _fruitIcons[i][2]
        _moveTable[i].icons[4] = _fruitIcons[i][1]
        _moveTable[i].icons[5] = _fruitIcons[i][5]
        _moveTable[i].iconParent = {}
        _moveTable[i].iconParent[1] = points[4][i+1]
        _moveTable[i].iconParent[2] = points[3][i+1]
        _moveTable[i].iconParent[3] = points[2][i+1]
        _moveTable[i].iconParent[4] = points[1][i+1]
        _moveTable[i].iconParent[5] = points[5][i+1]
        _moveTable[i].totalRound=(DefautRuns+5) * 5
        _moveTable[i].distance=_moveTable[i].totalRound*111
        _moveTable[i].nowIcon = 1
        _moveTable[i].delay=0.3*(i-1)
        _moveTable[i].round = 0
        _moveTable[i].duration = PerRowCost * _moveTable[i].totalRound
        local twcom = _moveTable[i].tweenPosition.gameObject:AddComponent("ComponentData")
        twcom.Id=i
        _moveTable[i].startPositon = _moveTable[i].tweenPosition.transform.localPosition
        _moveTable[i].tweenPosition:SetOnUpdate(OnMoveUpdate)

        for j=1,_moveTable[i].totalRound+5 do
            _moveTable[i].cells[j] = NGUITools.AddChild(_moveTable[i].tweenPosition.gameObject,_rowcell)
            _moveTable[i].cells[j].name="row"..j
            if j == 1 then
                _moveTable[i].cells[j].transform.position = points[4][i+1].transform.position
            else
                _moveTable[i].cells[j].transform.localPosition = UnityEngine.Vector3(0,_moveTable[i].cells[j-1].transform.localPosition.y+111,0)
            end
        end


    end
    EventDelegate.Add(_moveTable[5].tweenPosition.onFinished,OnMoveFinsh)
    gTimer.registerOnceTimer(50,timerFun.StartWait)
 end

function timerFun.PositionList()
      for i=1,5 do
          for j=1,7 do
              if not (i>=4 and (j==1 or j==7)) then
--                  points[i][j]=UnityTools.FindGo(_go.transform, "Container/bg2/scrollview/points/column"..(j-1).."/row"..i-1)
                  if j>=2 and j<=6 then
                      if _positions[j-1] == nil then
                          _positions[j-1]={}
                      end
                      if _fruitIcons[j-1] ==nil then
                          _fruitIcons[j-1]={}
                      end
--                      _fruitIcons[j-1][i]=UnityTools.FindGo(points[i][j].transform, "icon"):GetComponent("UISprite")
                      if i==1 then
                          _positions[j-1].startPositon = points[i][j].transform.localPosition
                      elseif i==5 then
                          _positions[j-1].endPosition = points[i][j].transform.localPosition
                      else
                          -- LogError("   ............ ".._fruitIcons[j-1][i].transform.name)
                          UnityTools.AddEffect(_fruitIcons[j-1][i].transform,"effect_kuang",{destroy = false,complete=OnLoadComplete})
                      end
                      _positions[j-1][i] ={}
                      _positions[j-1][i].position = points[i][j].transform.localPosition
                      _positions[j-1][i].leftRun=DefautRuns
                      local comData = points[i][j]:AddComponent("ComponentData")
                      comData.Id = i --行
                      comData.Tag = j-1 --列1-5
                  end
              end
          end
      end
      for i=1,15 do
          _rewardLineArray[i]={}
          lines[i]={}
          lines[i].line=UnityTools.FindGo(_go.transform, "Container/pointPanel/lines/line"..i)
          lines[i].colorIndex=i
          --lines[i].linePoint = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/line"..i)
          local lineComData = lines[i].line:AddComponent("ComponentData")
          lineComData.Id = i
          lineComData.Tag = 1
          local twAlpha = lines[i].line.gameObject:AddComponent("TweenAlpha")
          twAlpha.enabled=false
          EventDelegate.Add(twAlpha.onFinished,OnAlphaComplete)
      end
      lines[1].lineArray={points[2][1],points[2+1][2],points[2+1][3],points[2+1][4],points[2+1][5],points[2+1][6],points[2][7]}
      lines[2].lineArray={points[1][1],points[1+1][2],points[1+1][3],points[1+1][4],points[1+1][5],points[1+1][6],points[1][7]}
      lines[3].lineArray={points[3][1],points[3+1][2],points[3+1][3],points[3+1][4],points[3+1][5],points[3+1][6],points[3][7]}
      lines[4].lineArray={points[1][1],points[1+1][2],points[2+1][3],points[3+1][4],points[2+1][5],points[1+1][6],points[1][7]}
      lines[5].lineArray={points[3][1],points[3+1][2],points[2+1][3],points[1+1][4],points[2+1][5],points[3+1][6],points[3][7]}
      lines[6].lineArray={points[1][1],points[1+1][2],points[1+1][3],points[2+1][4],points[3+1][5],points[3+1][6],points[3][7]}
      lines[7].lineArray={points[3][1],points[3+1][2],points[3+1][3],points[2+1][4],points[1+1][5],points[1+1][6],points[1][7]}
      lines[8].lineArray={points[2][1],points[2+1][2],points[3+1][3],points[2+1][4],points[1+1][5],points[2+1][6],points[2][7]}
      lines[9].lineArray={points[2][1],points[2+1][2],points[1+1][3],points[2+1][4],points[3+1][5],points[2+1][6],points[2][7]}

      lines[10].lineArray={points[1][1],points[1+1][2],points[2+1][3],points[2+1][4],points[2+1][5],points[1+1][6],points[1][7]}
      lines[11].lineArray={points[2][1],points[2+1][2],points[1+1][3],points[1+1][4],points[1+1][5],points[2+1][6],points[2][7]}
      lines[12].lineArray={points[2][1],points[2+1][2],points[3+1][3],points[3+1][4],points[3+1][5],points[2+1][6],points[2][7]}
      lines[13].lineArray={points[3][1],points[3+1][2],points[2+1][3],points[2+1][4],points[2+1][5],points[3+1][6],points[3][7]}
      lines[14].lineArray={points[1][1],points[1+1][2],points[3+1][3],points[1+1][4],points[3+1][5],points[1+1][6],points[1][7]}
      lines[15].lineArray={points[3][1],points[3+1][2],points[1+1][3],points[3+1][4],points[1+1][5],points[3+1][6],points[3][7]}



      _rewardLineArray[1].icons={_fruitIcons[1][3],_fruitIcons[2][3],_fruitIcons[3][3],_fruitIcons[4][3],_fruitIcons[5][3]}
      _rewardLineArray[2].icons={_fruitIcons[1][2],_fruitIcons[2][2],_fruitIcons[3][2],_fruitIcons[4][2],_fruitIcons[5][2]}
      _rewardLineArray[3].icons={_fruitIcons[1][4],_fruitIcons[2][4],_fruitIcons[3][4],_fruitIcons[4][4],_fruitIcons[5][4]}
      _rewardLineArray[4].icons={_fruitIcons[1][2],_fruitIcons[2][3],_fruitIcons[3][4],_fruitIcons[4][3],_fruitIcons[5][2]}
      _rewardLineArray[5].icons={_fruitIcons[1][4],_fruitIcons[2][3],_fruitIcons[3][2],_fruitIcons[4][3],_fruitIcons[5][4]}
      _rewardLineArray[6].icons={_fruitIcons[1][2],_fruitIcons[2][2],_fruitIcons[3][3],_fruitIcons[4][4],_fruitIcons[5][4]}
      _rewardLineArray[7].icons={_fruitIcons[1][4],_fruitIcons[2][4],_fruitIcons[3][3],_fruitIcons[4][2],_fruitIcons[5][2]}
      _rewardLineArray[8].icons={_fruitIcons[1][3],_fruitIcons[2][4],_fruitIcons[3][3],_fruitIcons[4][2],_fruitIcons[5][3]}
      _rewardLineArray[9].icons={_fruitIcons[1][3],_fruitIcons[2][2],_fruitIcons[3][3],_fruitIcons[4][4],_fruitIcons[5][3]}

      _rewardLineArray[10].icons={_fruitIcons[1][2],_fruitIcons[2][3],_fruitIcons[3][3],_fruitIcons[4][3],_fruitIcons[5][2]}
      _rewardLineArray[11].icons={_fruitIcons[1][3],_fruitIcons[2][2],_fruitIcons[3][2],_fruitIcons[4][2],_fruitIcons[5][3]}
      _rewardLineArray[12].icons={_fruitIcons[1][3],_fruitIcons[2][4],_fruitIcons[3][4],_fruitIcons[4][4],_fruitIcons[5][3]}
      _rewardLineArray[13].icons={_fruitIcons[1][4],_fruitIcons[2][3],_fruitIcons[3][3],_fruitIcons[4][3],_fruitIcons[5][4]}
      _rewardLineArray[14].icons={_fruitIcons[1][2],_fruitIcons[2][4],_fruitIcons[3][2],_fruitIcons[4][4],_fruitIcons[5][2]}
      _rewardLineArray[15].icons={_fruitIcons[1][4],_fruitIcons[2][2],_fruitIcons[3][4],_fruitIcons[4][2],_fruitIcons[5][4]}


      gTimer.registerOnceTimer(50,timerFun.moveTable)
end


function timerFun.PreLine()
    _perLine.btnLeft = UnityTools.FindGo(_go.transform, "Container/bg2/perline/left")
    UnityTools.AddOnClick(_perLine.btnLeft.gameObject, _perLine.OnClickLeft)

    _perLine.btnRight = UnityTools.FindGo(_go.transform, "Container/bg2/perline/right")
    UnityTools.AddOnClick(_perLine.btnRight.gameObject, _perLine.OnClickRight)

    
    _perLine.lbNum = UnityTools.FindGo(_go.transform, "Container/bg2/perline/num"):GetComponent("UILabel")
    _perLine.lbNum.text = tostring(_perLine.num)
    gTimer.registerOnceTimer(50,timerFun.PositionList)
end


-------------------------------
function timerFun.LineNumBind()
    _lineNum.btnLeft = UnityTools.FindGo(_go.transform, "Container/bg2/linenum/left")
    UnityTools.AddOnClick(_lineNum.btnLeft.gameObject, _lineNum.OnClickLeft)

    _lineNum.btnRight = UnityTools.FindGo(_go.transform, "Container/bg2/linenum/right")
    UnityTools.AddOnClick(_lineNum.btnRight.gameObject, _lineNum.OnClickRight)

    _lineNum.lbNum = UnityTools.FindGo(_go.transform, "Container/bg2/linenum/num"):GetComponent("UILabel")
    _lineNum.lbNum.text = tostring(_lineNum.num)
    gTimer.registerOnceTimer(50,timerFun.PreLine)

 end

function _boxTaskTb.OnClickBoxTask(gameObject)
    if CTRL.isSuperShow == true then
        _platformMgr.OpenBoxWin(4,CTRL.DuiJuBox.times)
    else
        _platformMgr.OpenBoxWin(3,CTRL.DuiJuBox.times)
    end
end
 ---------------------------------------
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    
    _mainObj._lbPoolNum = UnityTools.FindGo(gameObject.transform, "Container/bg2/poolnum"):GetComponent("LabelRunning")
    for i=1,5 do
        points[i]={}
        for j=1,7 do
            if not (i>=4 and (j==1 or j==7)) then
                points[i][j]=UnityTools.FindGo(_go.transform, "Container/bg2/scrollview/points/column"..(j-1).."/row"..i-1)
                if j>=2 and j<=6 then
                    if _fruitIcons[j-1] ==nil then
                        _fruitIcons[j-1]={}
                    end
                    _fruitIcons[j-1][i]=UnityTools.FindGo(points[i][j].transform, "icon"):GetComponent("UISprite")
                    local fruitIndex = #_fruitIcons*(j-2)+i
                    if fruitIndex>0 and fruitIndex<= #CTRL.BaseInfo.FruitList then
                        SetFruitIcon(_fruitIcons[j-1][i],CTRL.BaseInfo.FruitList[fruitIndex].fruit_type,0)
                    else
                        SetFruitIcon(_fruitIcons[j-1][i],0,i*10+j)
                    end
                end
            end
        end
    end


    _mainObj._lbAllLineNum = UnityTools.FindGo(gameObject.transform, "Container/bg2/allline/num"):GetComponent("UILabel")

    _mainObj._lbAllLineNum.text = tostring(_allLineNum)
    _mainObj._btnFree = UnityTools.FindGo(gameObject.transform, "Container/bg2/spin/free")
    _mainObj._btnFree.gameObject.tag = "noClickSound"
    --UnityTools.AddOnClick(_mainObj._btnFree.gameObject, OnClickFree)

    _mainObj._freeStick = UnityTools.FindGo(gameObject.transform, "Container/bg2/free"):GetComponent("UIDragDropFruitItem")
    EventDelegate.Add(_mainObj._freeStick.onDragOver,OnFreeDragOver)
    _mainObj._freeStick.gameObject.tag = "noClickSound"
    _freeObj = UnityTools.FindGo(gameObject.transform, "Container/bg2/effect_mianfei")
    _freeObj:SetActive(false)
    
    _mainObj._normalStick = UnityTools.FindGo(gameObject.transform, "Container/bg2/spin"):GetComponent("UIDragDropFruitItem")
    EventDelegate.Add(_mainObj._normalStick.onDragOver,OnNormalDragOver)
    _mainObj._normalStick.gameObject.tag = "noClickSound"
    UnityTools.AddOnClick(_mainObj._normalStick.gameObject, OnClickNormal)
    _mainObj._btnNormal = UnityTools.FindGo(gameObject.transform, "Container/bg2/spin/normal")
    UnityTools.AddOnClick(_mainObj._btnNormal.gameObject, OnClickNormal)
    _mainObj._btnNormal.gameObject.tag = "noClickSound"
    _mainObj._btnAuto = UnityTools.FindGo(gameObject.transform, "Container/bg2/spin/auto")
    UnityTools.AddOnClick(_mainObj._btnAuto.gameObject, OnClickAuto)
    _pointCell = UnityTools.FindGo(gameObject.transform, "point")

    _pointPanel = UnityTools.FindGo(gameObject.transform, "Container/pointPanel"):GetComponent("UIPanel")
    
    _pointPanel.renderQueue = 1
    _pointPanel.depth = 100
    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/btnclose")
    UnityTools.AddOnClick(_btnClose.gameObject, ClickMenuBtn)

    _btnPool = UnityTools.FindGo(gameObject.transform, "Container/bg2/btnPool")
    UnityTools.AddOnClick(_btnPool.gameObject, OnClickPool)

    _scrollview = UnityTools.FindGo(gameObject.transform, "Container/bg2/scrollview")

    _lbAward = UnityTools.FindGo(gameObject.transform, "Container/bg2/number"):GetComponent("UILabel")

    _lbGold = UnityTools.FindGo(gameObject.transform, "Container/bg2/bottomleft/gold/Label"):GetComponent("UILabel")

    _lbName = UnityTools.FindGo(gameObject.transform, "Container/bg2/bottomleft/name"):GetComponent("UILabel")
    _head = UnityTools.FindGo(gameObject.transform, "Container/bg2/bottomleft/head")
    _btnTip = UnityTools.FindGo(gameObject.transform, "Container/btntip")
    UnityTools.AddOnClick(_btnTip.gameObject, OnClickTip)

    _taskObj._btnGetGold = UnityTools.FindGo(gameObject.transform, "Container/grid/btnGetGold")
    UnityTools.AddOnClick(_taskObj._btnGetGold.gameObject, OnClickShop)

    _lbFreeTime = UnityTools.FindGo(gameObject.transform, "Container/bg2/free/Label"):GetComponent("UILabel")

    _winBg = UnityTools.FindGo(gameObject.transform, "Container")
    _rowcell = UnityTools.FindGo(gameObject.transform, "rowcell")

    _headImg = UnityTools.FindCo(gameObject.transform,"UISprite", "Container/bg2/bottomleft/head/headImg")

    _headtex = UnityTools.FindGo(gameObject.transform, "Container/bg2/bottomleft/head/headImg/Texture"):GetComponent("UITexture")

    _btn2 = UnityTools.FindGo(gameObject.transform, "Container/btn1Layer/btn2")
    UnityTools.AddOnClick(_btn2.gameObject, CloseWin)

    _btn3 = UnityTools.FindGo(gameObject.transform, "Container/btn1Layer/btn3")
    UnityTools.AddOnClick(_btn3.gameObject, ClickSetBtnCall)

    _menuLayer = UnityTools.FindGo(gameObject.transform, "Container/btn1Layer")
    if _menuState == 0 then
        _menuLayer:SetActive(false)
    end


    _btnFreeGold = UnityTools.FindGo(gameObject.transform, "Container/grid/btnFreeGold")
    UnityTools.AddOnClick(_btnFreeGold.gameObject, OnClickTask)
    _taskObj._lbCoolDown = UnityTools.FindGo(gameObject.transform, "Container/grid/btnFreeGold/cool"):GetComponent("CooldownUpdate")
    _taskObj.spTask =  UnityTools.FindGo(gameObject.transform, "Container/grid/btnFreeGold/Sprite")
    _taskObj._taskBg = UnityTools.FindGo(gameObject.transform, "Container/bg"):GetComponent("UISprite")
    UnityTools.AddOnClick(_taskObj._taskBg.gameObject, OnClickGetAward)
    _taskObj._taskBg.gameObject:SetActive(false)
    _taskObj._taskIcon = UnityTools.FindGo(gameObject.transform, "Container/bg/item/Sprite"):GetComponent("UISprite")

    _taskObj._taskNum = UnityTools.FindGo(gameObject.transform, "Container/bg/item/num"):GetComponent("UILabel")

    _taskObj._taskDesc = UnityTools.FindGo(gameObject.transform, "Container/bg/desc"):GetComponent("UILabel")

    _taskObj._taskSlider = UnityTools.FindGo(gameObject.transform, "Container/bg/slider"):GetComponent("UISlider")

    _taskObj._taskSliderSprite = UnityTools.FindGo(gameObject.transform, "Container/bg/slider"):GetComponent("UISprite")

    _taskObj._taskBackSprite = UnityTools.FindGo(gameObject.transform, "Container/bg/slider/thumb"):GetComponent("UISprite")

    _taskObj._taskRed = UnityTools.FindGo(gameObject.transform, "Container/grid/btnFreeGold/red")
    _taskObj.spIcon=UnityTools.FindGo(gameObject.transform, "Container/grid/btnFreeGold/Sprite"):GetComponent("UISprite")
    _taskObj._sliderNum = UnityTools.FindGo(gameObject.transform, "Container/bg/slider/Label"):GetComponent("UILabel")

    _taskObj._taskStatus = UnityTools.FindGo(gameObject.transform, "Container/bg/status")

    _btnBox = UnityTools.FindGo(gameObject.transform, "Container/grid/btnBox")
    _btnBox:SetActive(false)
    
    _btnFreeGold:SetActive(false)
    UnityTools.AddOnClick(_btnBox.gameObject, OnClickBox)
    _boxRed = UnityTools.FindGo(gameObject.transform, "Container/grid/btnBox/red")
    _boxRed.gameObject:SetActive(false)
    if CTRL.BoxTable.boxStart == 1 then
        _btnBox:SetActive(false)
        _taskObj.spIcon.spriteName="boxtask"
        for i=1,#CTRL.BoxTable.boxStatus do
            if CTRL.BoxTable.boxStatus[i] == 1 then
                _boxRed.gameObject:SetActive(true)
                break
            end
        end
        
    else
        _taskObj.spIcon.spriteName="newtask"
        _btnBox:SetActive(false)
        
    end

    -- _redTaskObj.taskbg = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg")
    -- _redTaskObj.tweenPosition = _redTaskObj.taskbg.gameObject:GetComponent("TweenPosition")
    -- EventDelegate.Add(_redTaskObj.tweenPosition.onFinished,OnMoveRedFinish)
    -- _redTaskObj.status = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/status")
    -- _redTaskObj.slider = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/slider"):GetComponent("UISlider")
    -- _redTaskObj.sliderNum = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/slider/Label"):GetComponent("UILabel")
    -- _redTaskObj.desc = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/desc"):GetComponent("UILabel")
    -- _redTaskObj.num = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/num"):GetComponent("UILabel")
    -- _redTaskObj.close = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/close")
    -- _redTaskObj.red = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/taskbg/red")
    -- UnityTools.AddOnClick(_redTaskObj.close.gameObject, OnClickCloseRedTask)
    -- UnityTools.AddOnClick(_redTaskObj.taskbg.gameObject, OnClickRedTask)
    _taskObj.comObj = UnityTools.FindGo(gameObject.transform, "Container/bg/com"):GetComponent("UILabel")

    _poolRed = UnityTools.FindGo(gameObject.transform, "Container/bg2/btnPool/red")

    _taskObj.grid = UnityTools.FindGo(gameObject.transform, "Container/grid"):GetComponent("UIGrid")
    _taskObj.grid:Reposition()
    _boxTaskTb.undoObj = UnityTools.FindGo(gameObject.transform, "Container/box/undo")
    _boxTaskTb.boxTween = UnityTools.FindGo(gameObject.transform, "Container/box/box"):GetComponent("TweenRotation")
    _boxTaskTb.boxTween.enabled = false
    _boxTaskTb.comObj = UnityTools.FindGo(gameObject.transform, "Container/box/com")
    _boxTaskTb.sliderVal = UnityTools.FindGo(gameObject.transform, "Container/box/undo/sliderbg/sliderval"):GetComponent("UILabel")
    _boxTaskTb.slider = UnityTools.FindGo(gameObject.transform, "Container/box/undo/sliderbg"):GetComponent("UISlider")
    _boxTaskTb.boxConfig = LuaConfigMgr.ChestConfig[tostring(3)]
    
    UnityTools.AddOnClick(_boxTaskTb.boxTween.gameObject, _boxTaskTb.OnClickBoxTask)
--- [ALB END]
    _changeTb.btnSuperMode= UnityTools.FindGo(gameObject.transform, "Container/btnSuperMode")
    UnityTools.AddOnClick(_changeTb.btnSuperMode.gameObject, _changeTb.OnClickSuperMode)
    _changeTb.bg2 = UnityTools.FindGo(gameObject.transform, "Container/bg2"):GetComponent("UITexture")
    _changeTb.numbg1 = UnityTools.FindGo(gameObject.transform, "Container/bg2/linenum/bg"):GetComponent("UISprite")

    _changeTb.numbg2 = UnityTools.FindGo(gameObject.transform, "Container/bg2/perline/bg"):GetComponent("UISprite")

    _changeTb.super = UnityTools.FindGo(gameObject.transform, "Container/super")
    UnityTools.AddOnClick(_changeTb.super.gameObject, _changeTb.OnClickSuperMode)
    _changeTb.normalbtn = UnityTools.FindGo(gameObject.transform, "Container/btnSuperMode/normal")
    _changeTb.superShowPos = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/superShow"):GetComponent("TweenPosition")

    _changeTb.superShowScale = UnityTools.FindGo(gameObject.transform, "Container/pointPanel/superShow"):GetComponent("TweenScale")


    EventDelegate.Add(_changeTb.superShowPos.onFinished,_changeTb.OnSuperShowFinish)

    _changeTb.coolTime = UnityTools.FindGo(gameObject.transform, "Container/super/cool"):GetComponent("CooldownUpdate")
    _changeTb.btnSuperRank = UnityTools.FindGo(gameObject.transform, "Container/grid/btnSuperRank")
    UnityTools.AddOnClick(_changeTb.btnSuperRank.gameObject, _changeTb.OnClickSuperRank)
    _changeTb.superRankCool = UnityTools.FindGo(gameObject.transform, "Container/grid/btnSuperRank/cool"):GetComponent("CooldownUpdate")
    _changeTb.rankred = UnityTools.FindGo(gameObject.transform, "Container/grid/btnSuperRank/super/red")

    UIEventListener.Get(_mainObj._btnNormal.gameObject).onPress = OnPressNormal
    gTimer.registerOnceTimer(50,timerFun.LineNumBind)
end
function _boxTaskTb.UpdateBoxTask()
    if CTRL.DuiJuBox.times ==nil then return end
    if CTRL.DuiJuBox.times>= tonumber(_boxTaskTb.boxConfig.condition) then
        _boxTaskTb.boxTween.enabled = true
        UnityTools.SetActive(_boxTaskTb.undoObj.gameObject,false)
        UnityTools.SetActive(_boxTaskTb.comObj.gameObject,true)
        
    else
        _boxTaskTb.boxTween.enabled = false
        UnityTools.SetActive(_boxTaskTb.undoObj.gameObject,true)
        UnityTools.SetActive(_boxTaskTb.comObj.gameObject,false)
        _boxTaskTb.sliderVal.text = CTRL.DuiJuBox.times.."/".._boxTaskTb.boxConfig.condition
        _boxTaskTb.slider.value = CTRL.DuiJuBox.times/tonumber(_boxTaskTb.boxConfig.condition)
    end
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    
    _lineNum.num=CTRL.BaseInfo.lineNum
    _perLine.keyIndex=CTRL.BaseInfo.lineSetChips
     if _perLine.keyIndex<1 or _perLine.keyIndex>7 then
         _perLine.keyIndex=1
     end
     if _perLine.keyIndex ~=0  then
         local lineConfig = LuaConfigMgr.LineNumConfig[tostring(_perLine.keyIndex)]
         _perLine.num = tonumber(lineConfig.gold_bet)
     else
         _perLine.num=0
     end
     
    _go = gameObject
    UnityTools.DestroyWin("MainWin")
    UnityTools.DestroyWin("MainCenterWin");
    UnityTools.DestroyWin("GameCenterWin");
    thisObj=gameObject
    AutoLuaBind(gameObject)
    
end
function LaBaSpinReply()
    _lbFreeTime.text=CTRL.BaseInfo.FreeTimes
    RollCells()
    StartRoll()
    InitStatusTable()
end

 function OnFruitWinRenderQChange(msgId,type)
     if type == wName and UnityTools.IsWinShow(wName) then
         resetRenderQ();
     end
 end
local function OnLoadIconComplete(effectobj)
    effectobj.EffectGameObj.transform.position = _btnBox.transform.position
end
function OnFruitWinGoldUpdate(msgId,type,data)
    if type == "labagold" then
        UpdatePlyaerGold(_platformMgr.GetGod())
    elseif type == "labamission" then
        FruitRecvGameTimesUpdate()
    elseif type == "labatask" and _bRoll == false then
        
        _boxRed.gameObject:SetActive(false)
        _taskObj.spIcon.spriteName="newtask"
        if CTRL.BoxTable.boxStart == 1 then
            _taskObj.spIcon.spriteName="boxtask"
            for i=1,#CTRL.BoxTable.boxStatus do
                --  LogError(i.."box="..CTRL.BoxTable.boxStatus[i])
                if CTRL.BoxTable.boxStatus[i] == 1 then
                    _boxRed.gameObject:SetActive(true)
                    break
                end
            end        
        end
        UpdateFruitTaskInfo()
        if data.isShow == true then
            UnityTools.AddEffect(thisObj.transform,"effect_iconshow",{complete=OnLoadIconComplete})
            _btnBox.gameObject:SetActive(false)
        end
        _taskObj.grid:Reposition()
    end
end

function LabaModeSwitch()
    _changeTb.UpdateSuperMode()
end

local function Start(gameObject)
    local value= UnityEngine.PlayerPrefs.GetInt("PoolWinRedIsShow",0)
    if value == 0 then 
        UnityTools.SetActive(_poolRed.gameObject,true)
    else
        UnityTools.SetActive(_poolRed.gameObject,false)
    end
    _boxTaskTb.UpdateBoxTask()
    registerScriptEvent(EVENT_LABA_POOL_UPDATE, "UpdateLabaPoolNum")
    registerScriptEvent(EVENT_LABA_SPIN_REPLY, "LaBaSpinReply")
    registerScriptEvent(EVENT_LABA_SPIN_AUTO, "LabaWinAutoSpin")
    registerScriptEvent(EVENT_RECONNECT_SOCKET, "FruitCloseFunc")

    registerScriptEvent(EVENT_LABA_GOLD_UPDATE, "OnFruitWinGoldUpdate")
    registerScriptEvent(EVENT_RED_WIN_UPDATE, "UpdateLabaRedTask")
    registerScriptEvent(EVENT_LABA_MODE_SWTICH, "LabaModeSwitch")
--    _go= gameObject;
    _changeTb.UpdateSuperBtn()
    if CTRL.isJumpToSuper then
        -- CTRL.isSuperShow = false
        _changeTb.OnClickSuperMode(nil)
    end
    -- _changeTb.coolTime:SetEndTime(CTRL.IsOpenSuper)
    -- _changeTb.superRankCool:SetEndTime(CTRL.IsOpenSuper)
    
--    InitWin()
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_LABA_POOL_UPDATE, "UpdateLabaPoolNum")
    unregisterScriptEvent(EVENT_LABA_SPIN_REPLY, "LaBaSpinReply")
    unregisterScriptEvent(EVENT_LABA_SPIN_AUTO, "LabaWinAutoSpin")
    unregisterScriptEvent(EVENT_RENDER_CHANGE_WIN, "OnFruitWinRenderQChange")
    unregisterScriptEvent(EVENT_LABA_GOLD_UPDATE, "OnFruitWinGoldUpdate")
    unregisterScriptEvent(EVENT_RED_WIN_UPDATE, "UpdateLabaRedTask")
    unregisterScriptEvent(EVENT_LABA_MODE_SWTICH, "LabaModeSwitch")
    unregisterScriptEvent(EVENT_RECONNECT_SOCKET, "FruitCloseFunc")
    gTimer.removeTimer("FlashLabaLine")
    gTimer.removeTimer("freeShowBigRwardWin")
    gTimer.removeTimer("ShowBigRwardWin")

    gTimer.removeTimer(timerFun.LineNumBind)
    gTimer.removeTimer(timerFun.PreLine)
    gTimer.removeTimer(timerFun.PositionList)
    gTimer.removeTimer(timerFun.moveTable)
    gTimer.removeTimer(timerFun.StartWait)

    CLEAN_MODULE(wName .. "Mono")
end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.Update=Update

-- 返回当前模块
return M
