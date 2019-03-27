-- -----------------------------------------------------------------
-- * Copyright (c) 2018 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    SevenDailyTaskWinMono.lua
-- * Summary:     七日狂欢
-- *
-- * Version:     1.0.0
-- * Author:      E9GGF9T1HRZTCS8
-- * Date:        1/10/2018 11:02:36 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("SevenDailyTaskWinMono")



-- 界面名称
local wName = "SevenDailyTaskWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local GetRewardBtn = nil

local _winBg = nil

local tTaskItemPanel = {}

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function JumpTo(skip_id)
    LogWarn("JumpTo" .. skip_id)
    GoTo(skip_id,wName)
end

local function OnGoToBtnClick(gameObject)
    local skipId = ComponentData.Get(gameObject).Tag
    JumpTo(skipId)
end

local function SetTipWidthAndHeight(width, height, width1, height1, tip, desc)
    desc.width = width
    desc.height = height
    tip.width = width1
    tip.height = height1
end

function OnTipAutoDisMiss()
    local tip = UnityTools.FindGo(_winBg.transform, "bg/TaskTip"):GetComponent("UISprite")
    tip.gameObject:SetActive(false)
end

local function ShowTip(gameObject, taskData, index)
    gTimer.removeTimer("OnTipAutoDisMiss")
    gTimer.registerOnceTimer(5000, "OnTipAutoDisMiss")

    local tip = UnityTools.FindGo(_winBg.transform, "bg/TaskTip"):GetComponent("UISprite")
    tip.gameObject:SetActive(true)
    tip.transform.position = gameObject.transform.position

    local descLb = UnityTools.FindGo(tip.transform, "Label"):GetComponent("UILabel")
    descLb.text = taskData.title

    local GoToBtn = UnityTools.FindGo(tip.transform, "GoToBtn")
    GoToBtn:GetComponent("ComponentData").Tag = tonumber(taskData.skip_id)
    UIEventListener.Get(GoToBtn).onClick = OnGoToBtnClick 
    
    tip.transform.position = gameObject.transform.position 
    tip.transform.localPosition = UnityEngine.Vector3(tip.transform.localPosition.x - 140, tip.transform.localPosition.y + 92, 1)
end

local function SetGetRewardBtnStatus(status)
    local boxCollider = GetRewardBtn:GetComponent("BoxCollider")
    local canClickBtn = UnityTools.FindGo(GetRewardBtn.transform, "GetPrizeBtn")
    local unCanClickBtn = UnityTools.FindGo(GetRewardBtn.transform, "GetPrizeBtn_Gray")
    local btnLb = UnityTools.FindGo(unCanClickBtn.transform, "Label"):GetComponent("UILabel")
    
    if status == 0 then
        boxCollider.enabled = false
        unCanClickBtn:SetActive(true)
        canClickBtn:SetActive(false)
    elseif status == 1 then
        if CTRL.award == 1 then
            boxCollider.enabled = false
            unCanClickBtn:SetActive(true)
            canClickBtn:SetActive(false)
            btnLb.text = "明日可领"
        else
            boxCollider.enabled = true
            unCanClickBtn:SetActive(false)
            canClickBtn:SetActive(true)
            btnLb.text = "领取"
        end
    elseif status == 2 then
        boxCollider.enabled = false
        unCanClickBtn:SetActive(true)
        canClickBtn:SetActive(false)
    end
end

local function OnRewardIconClick(gameObject)
    UnityTools.CreateLuaWin("ExchangeWin")
end

local function OnMaskClick(gameObject)
    local tip = UnityTools.FindGo(_winBg.transform, "bg/TaskTip")
    tip:SetActive(false)
end

local function OnTipIconClick(gameObject)
    local taskData = LuaConfigMgr.LoginDaytaskConfig[tostring(CTRL.taskId)]
    if taskData ~= nil then
        ShowTip(gameObject, taskData, CTRL.taskId - 560000)
    end
end

local function UpdateTaskItemPanel()
    local taskIndex = CTRL.taskId - 560000
    for i=1,7 do
        if taskIndex > i then
            tTaskItemPanel[i]:SetActive(true)
        else
            tTaskItemPanel[i]:SetActive(false)
        end
    end
    
    if CTRL.taskId == 0 then
        return 
    end

    if CTRL.status == 2 then
        tTaskItemPanel[taskIndex]:SetActive(true)
    else
        tTaskItemPanel[taskIndex]:SetActive(false)
    end
end


local function UpdateCurTaskStatus()
    LogWarn("UpdateCurTaskStatus-----------------")
    SetGetRewardBtnStatus(CTRL.status)
    
    local dateTag = UnityTools.FindGo(_winBg.transform, "bg/BottomPanel/DateTag/Label"):GetComponent("UILabel")
    local date = CTRL.taskId - 560000
    dateTag.text = date

    local taskData = LuaConfigMgr.LoginDaytaskConfig[tostring(CTRL.taskId)]
    if taskData ~= nil then
        local descLb = UnityTools.FindGo(_winBg.transform, "bg/BottomPanel/TaskSlider/TaskName"):GetComponent("UILabel")
        descLb.text = taskData.title

        local processLb = UnityTools.FindGo(_winBg.transform, "bg/BottomPanel/TaskSlider/Label"):GetComponent("UILabel")
        processLb.text = "(" .. CTRL.process .. "/" .. taskData.parameter3 .. ")"
        -- processLb.gameObject.transform.localPosition = processLb.gameObject.transform.localPosition.x + descLb.gameObject.transform.width
        -- processLb.gameObject.transform.localPosition = UnityEngine.Vector3(processLb.gameObject.transform.localPosition.x + descLb.width,
        --                                                                     processLb.gameObject.transform.localPosition.y,
        --                                                                 processLb.gameObject.transform.localPosition.z
        --                                                                 )

        -- local tipIcon = UnityTools.FindGo(_winBg.transform, "bg/BottomPanel/TaskSlider/TipIcon")
        -- tipIcon.transform.localPosition.x = tipIcon.transform.localPosition.x + descLb.gameObject.transform.width + processLb.gameObject.transform.width
        -- tipIcon.transform.localPosition = UnityEngine.Vector3(tipIcon.transform.localPosition.x + descLb.width + processLb.width,
        --                                                         tipIcon.transform.localPosition.y,
        --                                                         tipIcon.transform.localPosition.z
        --                                                                 )
        -- LogWarn("UpdateCurTaskStatus-----------------" .. ",descLb.width=" .. descLb.width .. ",processLb.width=" .. processLb.width)
        -- UIEventListener.Get(tipIcon).onClick = OnTipIconClick
    end
end





local function OnGetRewardBtnClick(gameObject)
    LogWarn("OnGetRewardBtnClick------------")
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_task_seven_award_request, {})       
end
local function SetIconAndNum(transform,item1_id,num)
    LogWarn("item1_id=" .. tostring(item1_id))
    local sp = UnityTools.FindGo(transform, "img"):GetComponent("UISprite")
    sp.spriteName = "C"..item1_id
    local lbNum = UnityTools.FindGo(transform, "num"):GetComponent("UILabel")
    local data = LuaConfigMgr.ItemBaseConfig[item1_id .. ""]
    LogError("item1_id"..data.name)
    lbNum.text = num .. data.name
end
local function AutoLuaBind(gameObject)

    local mask = UnityTools.FindGo(gameObject.transform, "mask")
    UIEventListener.Get(mask).onClick = OnMaskClick 

    _winBg = UnityTools.FindGo(gameObject.transform, "Container")
    local CloseBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/CloseBtn")
    UIEventListener.Get(CloseBtn).onClick = CloseWin 

    for i=1,7 do
        -- local TipIcon = UnityTools.FindGo(gameObject.transform, "Container/bg/Grid/TaskItem_" .. i .. "/Bg/TipIcon")
        -- UIEventListener.Get(TipIcon).onClick = OnTipIconClick 

        local taskItemPanel = UnityTools.FindGo(gameObject.transform, "Container/bg/Grid/TaskItem_" .. i .. "/Panel")
        LogError("taskItemPanel="..taskItemPanel.name)
        table.insert(tTaskItemPanel, taskItemPanel)

        local taskId = 560000 + i
        local taskData = LuaConfigMgr.LoginDaytaskConfig[tostring(taskId)]
        if taskData ~= nil then
            local rewardIcon = UnityTools.FindGo(gameObject.transform, "Container/bg/Grid/TaskItem_" .. i .. "/Bg")
            local num = tonumber(taskData.item1_num)

            

            if tonumber(taskData.item1_id[1][1]) == 109 then
                num = num/10
                -- local numLb = UnityTools.FindGo(rewardIcon.transform, "num"):GetComponent("UILabel")
                -- numLb.text = num

                local effect = UnityTools.FindGo(rewardIcon.transform, "Effect"):GetComponent("UISprite")
                effect.gameObject:SetActive(true)

                local tweenRolation = effect.gameObject:GetComponent("TweenRotation")
                tweenRolation.enabled = true
            end
            SetIconAndNum(rewardIcon.transform, tonumber(taskData.item1_id[1][1]), num)
            if tonumber(taskData.item1_id[1][1]) == 109 then
                UIEventListener.Get(rewardIcon).onClick = OnRewardIconClick
            end
        end

    end
    
    GetRewardBtn = UnityTools.FindGo(gameObject.transform, "Container/bg/BottomPanel/GetPrizeBtn")
    UIEventListener.Get(GetRewardBtn).onClick = OnGetRewardBtnClick

    local tipIcon = UnityTools.FindGo(gameObject.transform, "Container/bg/BottomPanel/TaskSlider/TipIcon")
    UIEventListener.Get(tipIcon).onClick = OnTipIconClick
end

function OnSevenTaskStatusUpdate()
    if CTRL.taskId == 0 then
        CloseWin()
        return
    end
    UpdateCurTaskStatus()
    UpdateTaskItemPanel()
end

local function Awake(gameObject)
    AutoLuaBind(gameObject)
    registerScriptEvent(EVENT_SEVEN_TASK_STATUS_UPDATE, "OnSevenTaskStatusUpdate")
end

local function Start(gameObject)
    UpdateTaskItemPanel()
    UpdateCurTaskStatus()
end


local function OnDestroy(gameObject)
    gTimer.removeTimer("OnTipAutoDisMiss")
    unregisterScriptEvent(EVENT_SEVEN_TASK_STATUS_UPDATE, "OnSevenTaskStatusUpdate")
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
