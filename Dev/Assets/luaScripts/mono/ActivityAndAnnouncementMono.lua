-- -----------------------------------------------------------------


-- *
-- * Filename:    ActivityAndAnnouncementMono.lua
-- * Summary:     活动和公告信息界面
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        3/25/2017 1:56:29 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ActivityAndAnnouncementMono")

-- 界面名称
local wName = "ActivityAndAnnouncement"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _platformMgr = IMPORT_MODULE("PlatformMgr")
-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local GameMgr = IMPORT_MODULE("GameMgr")

-- 活动公告数据管理
local actsMgr = CTRL.ActivitiesManager

-- 窗口对象
local _winGameObj = nil

-- 当前选中的tab按钮
local _toggle = nil

-- 常规活动奖励列表cells
local _normalAwardCells = {}
setmetatable(_normalAwardCells, {__mode = "v"})        -- 设置为弱表1

-- 按钮列表cells
local _tabBtns = {}
setmetatable(_tabBtns, {__mode = "v"})        -- 设置为弱表

-- ///////////////////////////////////////////////////////////////////////////////////////////////////////
-- 界面逻辑部分

-- 活动tab按钮列表显示
local function onActivitesTabBtnsShow(cellbox, index, item)
    local idx = index+1
    local act = actsMgr:getActivityByIndex(idx)
    if act == nil then
        return
    end

    -- 选中状态
    local toggle = UnityTools.FindCo(item.transform, "UIToggle", "")
    if toggle ~= nil  then
        if idx == actsMgr.activeActivityIdx then
            toggle.value = true
            local box = item:GetComponent("BoxCollider")
            if box ~= nil then
                box.enabled = false
            end
            
            -- 保存引用
            _toggle = item
        else
            toggle.value = false
            local box = item:GetComponent("BoxCollider")
            if box ~= nil then
                box.enabled = true
            end
        end
    end

    -- 活动名称
    local strName = act:name()
    local lblChkedName = UnityTools.FindCo(item.transform, "UILabel", "checked/text")
    if lblChkedName ~= nil then
        lblChkedName.text = strName
    end

    local lblUnchkedName = UnityTools.FindCo(item.transform, "UILabel", "unchecked/text")
    if lblUnchkedName ~= nil then
        lblUnchkedName.text = strName
    end

    -- 活动标记
    local tag = act:tag()
    local hotTagImg = UnityTools.FindGo(item.transform, "hotTag")
    if hotTagImg ~= nil then
        hotTagImg:SetActive(tag == CTRL.EActivityTag.eHot)
    end

    local newTagImg = UnityTools.FindGo(item.transform, "newTag")
    if newTagImg ~= nil then
        newTagImg:SetActive(tag == CTRL.EActivityTag.eNew)
    end

    -- 红点提示
    local redDotImg = UnityTools.FindGo(item.transform, "redDot")
    if redDotImg ~= nil then
        redDotImg:SetActive(act:isRedDotVisible())
    end
    
    -- 保存按钮数据
    local data = UnityTools.FindCo(item.transform, "ComponentData", "")
    if data ~= nil then
        data.Value = idx
    end

    -- 响应点击
    UnityTools.AddOnClick(item, M.onActivityTabBtnClick)

    -- 保存引用
    _tabBtns[act:id()] = item
end

-- 公告tab按钮列表显示
local function onAnnoncementsTabBtnsShow(cellbox, index, item)
    local idx = index+1
    local announcement = actsMgr:getAnnoucementyIndex(idx)
    if announcement == nil then
        return
    end

    -- 选中状态
    local toggle = UnityTools.FindCo(item.transform, "UIToggle", "")
    if toggle ~= nil  then
        if idx == actsMgr.activeAnnoucementIdx then
            toggle.value = true
            local box = item:GetComponent("BoxCollider")
            if box ~= nil then
                box.enabled = false
            end
            
            -- 保存引用
            _toggle = item
         else
            toggle.value = false
            local box = item:GetComponent("BoxCollider")
            if box ~= nil then
                box.enabled = true
            end
        end
    end

    -- 公告名称
    local strTitle = announcement:title()
    local lblChkedName = UnityTools.FindCo(item.transform, "UILabel", "checked/text")
    if lblChkedName ~= nil then
        lblChkedName.text = strTitle
    end

    local lblUnchkedName = UnityTools.FindCo(item.transform, "UILabel", "unchecked/text")
    if lblUnchkedName ~= nil then
        lblUnchkedName.text = strTitle
    end

    -- 标记
    local hotTagImg = UnityTools.FindGo(item.transform, "hotTag")
    if hotTagImg ~= nil then
        hotTagImg:SetActive(false)
    end
    local newTagImg = UnityTools.FindGo(item.transform, "newTag")
    if newTagImg ~= nil then
        newTagImg:SetActive(false)
    end

     -- 红点提示
     local redDotImg = UnityTools.FindGo(item.transform, "redDot")
     if redDotImg ~= nil then
         redDotImg:SetActive(false)
     end

    -- 保存按钮数据
    local data = UnityTools.FindCo(item.transform, "ComponentData", "")
    if data ~= nil then
        data.Value = idx
    end

    -- 响应点击
    UnityTools.AddOnClick(item, M.onAnnouncementTabBtnClick)
end

-- 初始化活动tab切换按钮
local function initActivitesTabBtns()
    if _winGameObj == nil then
        return
    end

    local winActiveStatus = actsMgr:getWinActiveStatus()
    if winActiveStatus ~= CTRL.EWinActiveStatus.eActivity then
        return
    end

    local scrollView = UnityTools.FindCo(_winGameObj.transform, "UIScrollView", "Container/commonBtnList/ScrollView")
    if scrollView ~= nil then
        _controller:SetScrollViewRenderQueue(scrollView.gameObject)
        local gridCellMgr = UnityTools.FindCoInChild(scrollView, "UIGridCellMgr")
        if gridCellMgr ~= nil then
            gridCellMgr.onShowItem = onActivitesTabBtnsShow
        end
    end
end

-- 初始化公告tab切换按钮
local function initAnnouncementsTabBtns()
    if _winGameObj == nil then
        return
    end

    local winActiveStatus = actsMgr:getWinActiveStatus()
    if winActiveStatus ~= CTRL.EWinActiveStatus.eAnnouncement then
        return
    end

    local scrollView = UnityTools.FindCo(_winGameObj.transform, "UIScrollView", "Container/commonBtnList/ScrollView")
    if scrollView ~= nil then
        _controller:SetScrollViewRenderQueue(scrollView.gameObject)
        local gridCellMgr = UnityTools.FindCoInChild(scrollView, "UIGridCellMgr")
        if gridCellMgr ~= nil then
            gridCellMgr.onShowItem = onAnnoncementsTabBtnsShow
        end
    end
end

-- 刷新Tab按钮列表
local function updateTabBtns()
    local scrollView = UnityTools.FindCo(_winGameObj.transform, "UIScrollView", "Container/commonBtnList/ScrollView")
    if scrollView == nil then
        return
    end

    local gridCellMgr = UnityTools.FindCoInChild(scrollView, "UIGridCellMgr")
    if gridCellMgr ~= nil then
        -- 因为共用一个tab scroll view，所以需要先清空
        _toggle = nil
        gridCellMgr:ClearCells()
        _tabBtns = {}

        local winActiveStatus = actsMgr:getWinActiveStatus()
        if winActiveStatus == CTRL.EWinActiveStatus.eActivity then
            for k, v in actsMgr:activitiesIter() do
                gridCellMgr:NewCellsBox(gridCellMgr.Go)
            end
        elseif winActiveStatus == CTRL.EWinActiveStatus.eAnnouncement then
             for k, v in actsMgr:annoucementsIter() do
                gridCellMgr:NewCellsBox(gridCellMgr.Go)
            end
        end
        -- 对scrollview、gridCellMgr重新排序
        gridCellMgr.Grid:Reposition()
        gridCellMgr:UpdateCells()
    end
end

-- 常规活动奖励列表显示
local function onNormalActivityAwardItemsShow(cellbox, index, item)
    local act = actsMgr:activeActivity()
    if act == nil then
        return
    end

    local awardData = act:getAwardDataByIndex(index+1)
    if awardData ~= nil then
        -- 设置奖励物品,最多2个
        for i=1, 2 do
            local strAwardChildName = "award_" .. tostring(i)
            local awardItem = awardData:awardItemByIndex(i)
            local goItem = UnityTools.FindGo(item.transform, strAwardChildName)
            if goItem ~= nil then
                if awardItem ~= nil then
                    goItem:SetActive(true)

                    -- 设置奖励物品icon
                    local itemIcon = UnityTools.FindCo(goItem.transform, "UISprite", "icon")
                    if itemIcon ~= nil then
                        itemIcon.spriteName = "C" .. tostring(awardItem.idItem)
                    end

                    -- 设置数量
                    local lblCount = UnityTools.FindCo(goItem.transform, "UILabel",  "amount")
                    if lblCount ~= nil then
                        local strCount = tostring(awardItem.nCount)
                        if awardItem.idItem == 109 then
                            strCount = awardItem.nCount/10 .. "元"
                        end
                        lblCount.text = strCount
                    end
                else
                    goItem:SetActive(false)
                end
            end
        end -- for
        
        -- 设置描述
        local lblDesc = UnityTools.FindCo(item.transform, "UILabel", "description")
        if lblDesc ~= nil then
            lblDesc.text = awardData:titleName()
        end

        -- 设置进度值和显示
        local sldProgress = UnityTools.FindCo(item.transform, "UISlider", "progress")
        if sldProgress ~= nil then
            sldProgress.value = awardData:progressNumber()
        end

        local lblProgress = UnityTools.FindCo(item.transform, "UILabel", "progress/value")
        if lblProgress ~= nil then
            lblProgress.text = awardData:progressString()
        end

        -- 绑定领取按钮事件
        local getBtn = UnityTools.FindGo(item.transform, "btnOperate")
        local gotFlag = UnityTools.FindGo(item.transform, "gotFlag")
        if getBtn ~= nil and gotFlag ~= nil then
            UnityTools.AddOnClick(getBtn, M.onNormalActivityGetAwardBtnClick)

            -- 设置按钮状态
            local status = awardData:status()
            local lblBtnText = UnityTools.FindCo(item.transform, "UILabel", "btnOperate/text")
            if lblBtnText ~= nil then
                 if status == CTRL.EAwardGetStatus.eGot then
                    lblBtnText.text = LuaText.GetString("actAndAnnc_btn_awardGot")
                    getBtn:SetActive(false)
                    gotFlag:SetActive(true)
                elseif status == CTRL.EAwardGetStatus.eUnderway then
                    lblBtnText.text = LuaText.GetString("actAndAnnc_btn_awardUnderay")
                    getBtn:SetActive(true)
                    gotFlag:SetActive(false)
                elseif status == CTRL.EAwardGetStatus.eComplete then
                    lblBtnText.text = LuaText.GetString("actAndAnnc_btn_awardComplete")
                    getBtn:SetActive(true)
                    gotFlag:SetActive(false)
                end
            end

            -- 保存状态数据
            local data = UnityTools.FindCo(item.transform, "ComponentData", "btnOperate")
            if data ~= nil then
                data.Value = status
                data.Id = awardData:idTask()
            end
        end

        -- 保存引用
        _normalAwardCells[awardData:idTask()] = item
    end

end

-- 公告显示
-- params: idx 活动索引
local function announcementShow(idx)
    local announcement = actsMgr:getAnnoucementyIndex(idx)
    if announcement ~= nil and _winGameObj ~= nil then
        local lblTitle = UnityTools.FindCo(_winGameObj.transform, "UILabel", "Container/announcementPage/title")
        if lblTitle ~= nil then
            lblTitle.text = announcement:title()
        end

        local lblContent = UnityTools.FindCo(_winGameObj.transform, "UILabel", "Container/announcementPage/contentWidget/ScrollView/text")
        if lblContent ~= nil then
            lblContent.text = announcement:content()
        end

        -- 重置位置
        local scrollView = UnityTools.FindCo(_winGameObj.transform, "UIScrollView", "Container/announcementPage/contentWidget/ScrollView")
        if scrollView ~= nil then
            scrollView:ResetPosition()
        end

        -- 设置当前公告索引
        actsMgr.activeAnnoucementIdx = idx 
    end
end

-- 常规活动展示
local function normalActivityShow(activityData)
    if _winGameObj == nil or activityData == nil then
        return
    end

    -- 设置广告图
    local imgAd = UnityTools.FindCo(_winGameObj.transform, "UITexture", "Container/activityPage/normal/imgAd")
    if imgAd ~= nil then
        local rgb = activityData:strAdImgName(false)
        UtilTools.loadTexture(imgAd, "UI/Texture/activity/" .. rgb, true)
    end

    -- 设置主标题
    -- local imgMainTitle = UnityTools.FindCo(_winGameObj.transform, "UITexture", "Container/activityPage/normal/imgMainTitle")
    -- if imgMainTitle ~= nil then
    --     local rgb = activityData:mainTitleImgName(false)
    --     UtilTools.loadTexture(imgMainTitle, "UI/Texture/activity/" .. rgb, true)
    -- end

    -- 设置副标题
    local lblSubTitle = UnityTools.FindCo(_winGameObj.transform, "UILabel", "Container/activityPage/normal/lblSubTitle")
    if lblSubTitle ~= nil then
        lblSubTitle.text = activityData:strSubTitle()
    end

    -- 设置数据标题
    local lblDataTitle = UnityTools.FindCo(_winGameObj.transform, "UILabel", "Container/activityPage/normal/dataTitle/text")
    if lblDataTitle ~= nil then
        lblDataTitle.text = activityData:dataTitle()
    end
    
    -- 更新奖励列表
    local scrollView = UnityTools.FindCo(_winGameObj.transform, "UIScrollView", "Container/activityPage/normal/contentWidget/ScrollView")
    if scrollView ~= nil then
        scrollView:ResetPosition()
        _controller:SetScrollViewRenderQueue(scrollView.gameObject)
        local gridCellMgr = UnityTools.FindCoInChild(scrollView, "UIGridCellMgr")
        if gridCellMgr ~= nil then
            -- 共用scrollview，所以每次需要清空
            gridCellMgr:ClearCells()
            _normalAwardCells = {}

            gridCellMgr.onShowItem = onNormalActivityAwardItemsShow
            for k, v in activityData:awardDatasIter() do
                gridCellMgr:NewCellsBox(gridCellMgr.Go)
            end

            -- 对scrollview、gridCellMgr重新排序
            gridCellMgr.Grid:Reposition()
            gridCellMgr:UpdateCells()
        end
    end
end

-- 其他活动参与按钮点击
local function onOtherActivityJoinBtnClick(gameObject)
    -- local mainWin = IMPORT_MODULE("MainWinMono");
    -- if mainWin ~= nil then
    --     _platformMgr.Config.StartOpenList = {}
    --     _platformMgr.Config.isInitMainWin = true;
    --     mainWin.OnClickNormalCow();
    --     UnityTools.DestroyWin(wName);
    -- end
    local activityData = actsMgr:getActivityByIndex(actsMgr.activeActivityIdx)
    if activityData == nil then
        return
    end
    if activityData:strAdImgName() == "ad_qhb.png" then
        if _platformMgr.GetDiamond() >= 18  then
                GameMgr.EnterGame(1,10,function()
                    UnityTools.DestroyWin("MainCenterWin")
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("GameCenterWin")
                    UnityTools.DestroyWin(wName);
                end)
        else
            UnityTools.CreateLuaWin("RedConditionWin")
        end
    elseif activityData:strAdImgName() == "ad_redpacket.png" then
        if _platformMgr.GetDiamond() >= 18  then
                GameMgr.EnterGame(1,10,function()
                    UnityTools.DestroyWin("MainCenterWin")
                    UnityTools.DestroyWin("MainWin")
                    UnityTools.DestroyWin("GameCenterWin")
                    UnityTools.DestroyWin(wName);
                end)
        else
            UnityTools.CreateLuaWin("RedConditionWin")
        end
    elseif activityData:strAdImgName() == "ad_newvision.png" then

    elseif activityData:strAdImgName() == "ad_friend.png" then
        UnityTools.DestroyWin(wName);
        UnityTools.CreateLuaWin("NewShareWin")
    elseif activityData:strAdImgName() == "ad_buydiamond.png" then
        local shopCtrl = IMPORT_MODULE("ShopWinController")
        if shopCtrl ~= nil then
            UnityTools.DestroyWin(wName);
            shopCtrl.OpenShop(2)
        end
    elseif activityData:strAdImgName() == "ad_superfruit.png" then
        local fruitCtrl = IMPORT_MODULE("FruitWinController")
        fruitCtrl.isJumpToSuper = true
        GoTo(113,wName)
        UnityTools.DestroyWin(wName);
    end
    
    -- if actsMgr.activeActivityIdx == 1 then
        
    -- elseif actsMgr.activeActivityIdx == 3 then
    --     local shopCtrl = IMPORT_MODULE("ShopWinController")
    --     if shopCtrl ~= nil then
    --         UnityTools.DestroyWin(wName);
    --         shopCtrl.OpenShop(2)
    --     end
    -- elseif actsMgr.activeActivityIdx == 2 then
        
    -- elseif actsMgr.activeActivityIdx == 4 then
    --     if _platformMgr.GetDiamond() >= 18  then
    --             GameMgr.EnterGame(1,10,function()
    --                 UnityTools.DestroyWin("MainCenterWin")
    --                 UnityTools.DestroyWin("MainWin")
    --                 UnityTools.DestroyWin("GameCenterWin")
    --                 UnityTools.DestroyWin(wName);
    --             end)
    --     else
    --         UnityTools.CreateLuaWin("RedConditionWin")
    --     end
    -- end
    
end
-- 其他活动展示
local function otherActivityShow(activityData)
    if _winGameObj == nil or activityData == nil then
        return
    end

    -- 设置广告图
    local sprAd = UnityTools.FindCo(_winGameObj.transform, "UITexture", "Container/activityPage/other/imgAd")
    if sprAd ~= nil then
        UtilTools.loadTexture(sprAd,"UI/Texture/activity/"..activityData:strAdImgName(),true)
    end
    local btnRules = UnityTools.FindGo(_winGameObj.transform, "Container/activityPage/btnRules")
    local btnJoin = UnityTools.FindGo(_winGameObj.transform, "Container/activityPage/other/btnJoin")
    local btnJoin2 = UnityTools.FindGo(_winGameObj.transform, "Container/activityPage/other/btnJoin2")
    if activityData:rules() == nil or activityData:rules() == "" then
        btnRules.gameObject:SetActive(false)
        if btnJoin ~= nil then
            btnJoin.gameObject:SetActive(false)
            btnJoin2.gameObject:SetActive(false)
            -- LogError(activityData:strAdImgName())
            if activityData:strAdImgName() == "ad_qhb.png" then
                btnJoin.transform.localPosition = UnityEngine.Vector3(168,-163,0)
                btnJoin.gameObject:SetActive(true)
            elseif activityData:strAdImgName() == "ad_redpacket.png" then
                btnJoin.transform.localPosition = UnityEngine.Vector3(0,-176,0)
                btnJoin.gameObject:SetActive(true)
            elseif activityData:strAdImgName() == "ad_newvision.png" then

            elseif activityData:strAdImgName() == "ad_friend.png" then
                btnJoin.transform.localPosition = UnityEngine.Vector3(0,-151,0)
                btnJoin.gameObject:SetActive(true)
            elseif activityData:strAdImgName() == "ad_buydiamond.png" then
                btnJoin2.transform.localPosition = UnityEngine.Vector3(0,-171,0)
                btnJoin2.gameObject:SetActive(true)
            elseif activityData:strAdImgName() == "ad_superfruit.png" then
                
                btnJoin2.transform.localPosition = UnityEngine.Vector3(0,-155,0)
                btnJoin2.gameObject:SetActive(true)
            end
            
            
        end
    else
        btnRules.gameObject:SetActive(true)
        if btnJoin ~= nil then
            btnJoin.gameObject:SetActive(false)
        end
    end
    -- 绑定立即参与按钮
    
    
end

-- 活动显示
-- params: idx 活动索引
local function activityShow(idx)
    if _winGameObj == nil then
        return
    end
    
    local act = actsMgr:getActivityByIndex(idx)
    if act ~= nil then
        -- 设置当前活动索引
        actsMgr.activeActivityIdx = idx

        local actType = act:getType()
        local pageNormal = UnityTools.FindGo(_winGameObj.transform,  "Container/activityPage/normal")
        local pageOther = UnityTools.FindGo(_winGameObj.transform,  "Container/activityPage/other")
   
        if actType == CTRL.EActivityType.eNormal then  -- 常规活动
            if pageNormal ~= nil then
                pageNormal:SetActive(true)
            end

            if pageOther ~= nil then
                pageOther:SetActive(false)
            end

            normalActivityShow(act)
        elseif actType == CTRL.EActivityType.eOther then -- 其他活动
            if pageNormal ~= nil then
                pageNormal:SetActive(false)
            end

            if pageOther ~= nil then
                pageOther:SetActive(true)
            end
            otherActivityShow(act)
        end
    end
end


-- ///////////////////////////////////////////////////////////////////////////////////////////////////////
-- UI事件响应事件

-- 活动规则按钮点击
local function onActivityRuleBtnClick(gameObject)
     UnityTools.CreateLuaWin("ActivityRules")
end

-- 活动按钮点击
local function onActivityBtnClick(gameObject)
    local winActiveStatus = actsMgr:getWinActiveStatus()
    if winActiveStatus == CTRL.EWinActiveStatus.eActivity then
        return
    end
    
    actsMgr:setWinActiveStatus(CTRL.EWinActiveStatus.eActivity)
    initActivitesTabBtns()
    updateTabBtns()
    activityShow(actsMgr.activeActivityIdx)
end

-- 公告按钮点击
local function onAnnouncementBtnClick(gameObject)
    local winActiveStatus = actsMgr:getWinActiveStatus()
    if winActiveStatus == CTRL.EWinActiveStatus.eAnnouncement then
        return
    end

    actsMgr:setWinActiveStatus(CTRL.EWinActiveStatus.eAnnouncement)
    initAnnouncementsTabBtns()
    updateTabBtns()
    announcementShow(actsMgr.activeAnnoucementIdx)
end


-- 设置Tab按钮切换状态
local function setTabBtnChangeStatus(gameObject)
    if gameObject == nil then
        return
    end

    -- 设置按钮变化
    if _toggle ~= nil then
        --还原上次选中的按钮
        local box = _toggle:GetComponent("BoxCollider")
        if box ~= nil then
            box.enabled = true
        end
    end

    local curBox = gameObject:GetComponent("BoxCollider")
    if curBox ~= nil then
        curBox.enabled = false;
    end
    _toggle = gameObject
end


-- 活动切换按钮点击
local function onActivityTabBtnClick(gameObject)
    local data = gameObject:GetComponent("ComponentData")
    if data == nil then
        return
    end
    -- 活动数据
    local idx = data.Value
    -- if idx >2 or idx == nil then
    --     return
    -- end
    
    local act = actsMgr:getActivityByIndex(idx)
        
        activityShow(idx)
    setTabBtnChangeStatus(gameObject)
end

-- 公告切换按钮点击
local function onAnnouncementTabBtnClick(gameObject)
    local data = gameObject:GetComponent("ComponentData")
    if data == nil then
        return
    end

    -- 公告数据
    local idx = data.Value;
    announcementShow(idx)
    setTabBtnChangeStatus(gameObject)
end

-- 常规活动领取按钮点击
local function onNormalActivityGetAwardBtnClick(gameObject)
    local saveData = gameObject:GetComponent("ComponentData")
    if saveData == nil then
        return
    end

    -- 检查领取状态
    local status = saveData.Value
    if status == CTRL.EAwardGetStatus.eGot then  -- 已领取
        UnityTools.ShowMessage(LuaText.GetString("actAndAnnc_msg_awardGot"))
        return
    elseif status == CTRL.EAwardGetStatus.eUnderway then -- 进行中
        UnityTools.ShowMessage(LuaText.GetString("actAndAnnc_msg_awardUnderay"))
        return
    end

    -- 可领取，发送领取请求
    --TODO: 发送领取请求
    local act = actsMgr:activeActivity()
    if act ~= nil then
        actsMgr:sendActivityDrawReq(act:id(), saveData.Id)
    end
end


-- 窗口关闭点击
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    -- 活动规则
    local btnRules = UnityTools.FindGo(gameObject.transform, "Container/activityPage/btnRules")
    UnityTools.AddOnClick(btnRules.gameObject, onActivityRuleBtnClick)
    local btnJoin = UnityTools.FindGo(_winGameObj.transform, "Container/activityPage/other/btnJoin")
    local btnJoin2 = UnityTools.FindGo(_winGameObj.transform, "Container/activityPage/other/btnJoin2")
    UnityTools.AddOnClick(btnJoin.gameObject, onOtherActivityJoinBtnClick)
    UnityTools.AddOnClick(btnJoin2.gameObject, onOtherActivityJoinBtnClick)
    
    -- 关闭
    local btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(btnClose.gameObject, CloseWin)

    -- 活动按钮
    local btnActivity =  UnityTools.FindGo(gameObject.transform, "Container/btnActivityTab")
    UnityTools.AddOnClick(btnActivity.gameObject, onActivityBtnClick)

    -- 公告按钮点击
    local btnAnnouncement =  UnityTools.FindGo(gameObject.transform, "Container/btnAnnoucementTab")
    UnityTools.AddOnClick(btnAnnouncement.gameObject, onAnnouncementBtnClick)
end

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 响应UI刷新事件

-- 活动页面更新
function onActAndAnnWinActivityPageUpdateEvent(eventId, idActivity)
    local idx = actsMgr:id2Index(idActivity)
    if idx ~= nil then
        activityShow(idx)
    end

    local tabBtn = _tabBtns[idActivity]
    local act = actsMgr:getActivityByIndex(idx)
    if tabBtn ~= nil and act ~= nil then
        -- 红点提示
        local redDotImg = UnityTools.FindGo(tabBtn.transform, "redDot")
        if redDotImg ~= nil then
            redDotImg:SetActive(act:isRedDotVisible())
        end
    end
end


-- 活动页面奖励对象刷新
function onActAndAnnWinActivityPageAwardItemUpdateEvent(eventId, idActivity, idAward)
    if _winGameObj == nil then
        return
    end

    -- 非当前激活的活动说明没有显示，不做UI更新处理
    local act = actsMgr:activeActivity()
    if act == nil or act:id() ~= idActivity then
        return
    end

    -- 更新指定的cell
    -- local item = _normalAwardCells[idAward]
    -- local awardData = act:getAwardDataById(idAward)
    -- if item ~= nil and awardData ~= nil then
    --     local getBtn = UnityTools.FindGo(item.transform, "btnOperate")
    --     if getBtn ~= nil then

    --         -- 设置按钮状态
    --         local status = awardData:status()
    --         local lblBtnText = UnityTools.FindCo(item.transform, "UILabel", "btnOperate/text")
    --         if lblBtnText ~= nil then
    --              if status == CTRL.EAwardGetStatus.eGot then
    --                 lblBtnText.text = LuaText.GetString("actAndAnnc_btn_awardGot")
    --             elseif status == CTRL.EAwardGetStatus.eUnderway then
    --                 lblBtnText.text = LuaText.GetString("actAndAnnc_btn_awardUnderay")
    --             elseif status == CTRL.EAwardGetStatus.eComplete then
    --                 lblBtnText.text = LuaText.GetString("actAndAnnc_btn_awardComplete")
    --             end
    --         end
    --     end
    -- end

    -- 更新活动Tab按钮
    local tabBtn = _tabBtns[act:id()]
    if tabBtn ~= nil then
        -- 红点提示
        local redDotImg = UnityTools.FindGo(tabBtn.transform, "redDot")
        if redDotImg ~= nil then
            redDotImg:SetActive(act:isRedDotVisible())
        end
    end

    -- 更新奖励列表
    local scrollView = UnityTools.FindCo(_winGameObj.transform, "UIScrollView", "Container/activityPage/normal/contentWidget/ScrollView")
    if scrollView ~= nil then
        _controller:SetScrollViewRenderQueue(scrollView.gameObject)
        local gridCellMgr = UnityTools.FindCoInChild(scrollView, "UIGridCellMgr")
        if gridCellMgr ~= nil then
            -- 共用scrollview，所以每次需要清空
            gridCellMgr:ClearCells()
            _normalAwardCells = {}

            gridCellMgr.onShowItem = onNormalActivityAwardItemsShow
            for k, v in act:awardDatasIter() do
                gridCellMgr:NewCellsBox(gridCellMgr.Go)
            end

            -- 对scrollview、gridCellMgr重新排序
            gridCellMgr.Grid:Reposition()
            gridCellMgr:UpdateCells()
        end
    end
end


-- ///////////////////////////////////////////////////////////////////////////////////////////////////////
-- MONO回调函数

local function Awake(gameObject)
    _winGameObj = gameObject

    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    registerScriptEvent(ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_PAGE, "onActAndAnnWinActivityPageUpdateEvent")
    registerScriptEvent(ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_AWARD_ITEM, "onActAndAnnWinActivityPageAwardItemUpdateEvent")
end

local function Start(gameObject)
    -- 初始化Tab按钮列表
    local winActiveStatus = actsMgr:getWinActiveStatus()
    local activityPage = UnityTools.FindGo(gameObject.transform, "Container/activityPage")
    local annoucePage = UnityTools.FindGo(gameObject.transform, "Container/announcementPage")
    if winActiveStatus == CTRL.EWinActiveStatus.eActivity then
        -- 显示活动页面
        if activityPage ~= nil then
            activityPage:SetActive(true)
        end
        -- 隐藏公告页面
        if annoucePage ~= nil then
            annoucePage:SetActive(false)
        end

        -- 活动激活状态图标
        local goActiveActivity =  UnityTools.FindGo(gameObject.transform, "Container/btnActivityTab/active")
        if goActiveActivity ~= nil then
            goActiveActivity:SetActive(true)
        end

        -- 公告激活状态图标
        local goActiveAnnounce =  UnityTools.FindGo(gameObject.transform, "Container/btnAnnoucementTab/active")
        if goActiveAnnounce ~= nil then
            goActiveAnnounce:SetActive(false)
        end

        initActivitesTabBtns()
        updateTabBtns()

        local idx = actsMgr.activeActivityIdx
        local act = actsMgr:getActivityByIndex(idx)
        
            activityShow(idx)

    elseif winActiveStatus == CTRL.EWinActiveStatus.eAnnouncement then
        -- 隐藏活动页面
        if activityPage ~= nil then
            activityPage:SetActive(false)
        end
        -- 显示公告页面
        if annoucePage ~= nil then
            annoucePage:SetActive(true)
        end

         -- 活动激活状态图标
        local goActiveActivity =  UnityTools.FindGo(gameObject.transform, "Container/btnActivityTab/active")
        if goActiveActivity ~= nil then
            goActiveActivity:SetActive(false)
        end

        -- 公告激活状态图标
        local goActiveAnnounce =  UnityTools.FindGo(gameObject.transform, "Container/btnAnnoucementTab/active")
        if goActiveAnnounce ~= nil then
            goActiveAnnounce:SetActive(true)
        end

        initAnnouncementsTabBtns()
        updateTabBtns()
        announcementShow(actsMgr.activeAnnoucementIdx)
    end
end

local function OnDestroy(gameObject)
    CLEAN_MODULE("ActivityAndAnnouncementMono")

    _winGameObj = nil
    _toggle = nil
    _normalAwardCells = {}
    _tabBtns = {}

    -- 设置数据脏标记，下次打开界面时需要重新取数据
    actsMgr:setActivityDirty()

    unregisterScriptEvent(ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_PAGE, "onActAndAnnWinActivityPageUpdateEvent")
    unregisterScriptEvent(ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_AWARD_ITEM, "onActAndAnnWinActivityPageAwardItemUpdateEvent")
end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.onActivityTabBtnClick = onActivityTabBtnClick
M.onAnnouncementTabBtnClick = onAnnouncementTabBtnClick
M.onNormalActivityGetAwardBtnClick = onNormalActivityGetAwardBtnClick


-- 返回当前模块
return M
