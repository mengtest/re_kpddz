-- -----------------------------------------------------------------


-- *
-- * Filename:    LoginAwardWinMono.lua
-- * Summary:     登录奖励脚本逻辑
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        3/17/2017 10:30:52 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("LoginAwardWinMono")

-- 界面名称
local wName = "LoginAwardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local goWin = nil
local btnToRecharge
local btnGetVipAward
local btnGetNormalAward
local btnRecievedAward
local btnClose
local btnVipInfoClose
local btnVipAwardInfo
local vipAwardInfoWin = nil
local thisObj 
--- [ALD END]


-- 登录奖励数据
local loginAwardDataMgr = CTRL.LoginAwardDataMgr

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

-- 关闭窗口
local function onClickCloseWin(gameObject)
    CloseWin()
end

--  去充值按钮响应
local function onClickToRecharge(gameObject)
    -- UnityTools.ShowMessage(LuaText.GetString("open_lv_lmt_unknow_lv"))
    UnityTools.CreateLuaWin("ShopWin")
end

-- 领取VIP额外奖励
local function onClickGetVipAward(gameObject)
    local eVipAwardStatus = loginAwardDataMgr:vipAwardStatus()
    if eVipAwardStatus == CTRL.EVipAwardStatus.eActive then
        loginAwardDataMgr:sendGetVipAwardRequest()
    else
        UnityTools.ShowMessage(LuaText.GetString("loginAwardGotVipAward"))
    end
end

-- 打开商店
local function gotoShopWin()
    local shopCtrl = IMPORT_MODULE("ShopWinController")
    if shopCtrl ~= nil then
        shopCtrl.OpenShop(3)
    end
end


-- 获取常规登录奖励
local function onClickGetNormalAward(gameObject)
    local bTodayAward = loginAwardDataMgr:isReceivedTodayAward()
    if not bTodayAward then
        loginAwardDataMgr:sendTodayCheckInRequest()
    else
        local bMakeupDays = false
        
        if bMakeupDays then
            local bHaveMakeupCards = loginAwardDataMgr:isHaveMakeupCards()
            if bHaveMakeupCards then
                loginAwardDataMgr:sendMakeupCheckInRequest()
            else
                UnityTools.MessageDialog(LuaText.GetString("loginAwardNotEnoughMakeupCards"), { okCall = gotoShopWin })
            end
        else
            UnityTools.ShowMessage(LuaText.GetString("loginAwardGotNormalAward"))
        end
    end
end

-- 关闭VIP信息窗口
local function onClickVipAwardInfoClose(gameObject)
    if vipAwardInfoWin ~= nil then
        vipAwardInfoWin:SetActive(false)
    end
end

-- 打开VIP奖励信息说明窗口
local function onClickVipAwardInfo(gameObject)
    if vipAwardInfoWin ~= nil then
        vipAwardInfoWin:SetActive(true)
    end
end

--- [ALF END]

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    thisObj = gameObject
    btnToRecharge = UnityTools.FindGo(gameObject.transform, "Container/main/awardVip/btnToRecharge")
    UnityTools.AddOnClick(btnToRecharge.gameObject, onClickToRecharge)

    btnGetVipAward = UnityTools.FindGo(gameObject.transform, "Container/main/awardVip/btnGetVipAward")
    UnityTools.AddOnClick(btnGetVipAward.gameObject, onClickGetVipAward)
    local btnVipRecievedAward = UnityTools.FindGo(gameObject.transform, "Container/main/awardVip/btnVipRecievedAward")
    UnityTools.AddOnClick(btnVipRecievedAward.gameObject, onClickGetVipAward)
    btnRecievedAward = UnityTools.FindGo(gameObject.transform, "Container/main/btnRecievedAward")
    UnityTools.AddOnClick(btnRecievedAward.gameObject, onClickGetNormalAward)
    btnGetNormalAward = UnityTools.FindGo(gameObject.transform, "Container/main/btnGetNormalAward")
    UnityTools.AddOnClick(btnGetNormalAward.gameObject, onClickGetNormalAward)

    btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(btnClose.gameObject, onClickCloseWin)

    btnVipInfoClose = UnityTools.FindGo(gameObject.transform, "Container/vipAwardInfo/btnVipInfoClose")
    UnityTools.AddOnClick(btnVipInfoClose.gameObject, onClickVipAwardInfoClose)

    btnVipAwardInfo = UnityTools.FindGo(gameObject.transform, "Container/main/awardVip/btnVipAwardInfo")
    UnityTools.AddOnClick(btnVipAwardInfo.gameObject, onClickVipAwardInfo)

--- [ALB END]

    -- VIP额外奖励信息窗口
    vipAwardInfoWin = UnityTools.FindGo(gameObject.transform, "Container/vipAwardInfo")
end


-- /////////////////////////////////////////////////////////////////////////////////////////////
-- UI界面设置

-- 初始化常规登录奖励部分
local function normalLoginAwardInitialize()
    if goWin == nil then return end

    -- 刷新常规登录奖励
    for k, v in loginAwardDataMgr:dayAwardData() do
        local nDayIdx = v:index()

        local objAwardItem = UnityTools.FindGo(goWin.transform, "Container/main/awardNormal/awardItem_" .. nDayIdx)
        if objAwardItem ~= nil then
            -- 设置图标
            local sprIcon = UnityTools.FindCo(objAwardItem.transform, "UISprite", "icon")
            if sprIcon ~= nil then
                -- sprIcon.spriteName = v:itemIcon()
            end

            -- 控制状态显示
            local objActive = UnityTools.FindGo(objAwardItem.transform, "active")
            local objNormal = UnityTools.FindGo(objAwardItem.transform, "normal")
            -- local objSignTag = UnityTools.FindGo(objAwardItem.transform, "signTag")
            local objCurrent = nil
            local eNormalStatus = v:status()

            -- 补签标识
            -- objSignTag:SetActive(eNormalStatus == CTRL.EDayAwardStatus.eSign)

            if eNormalStatus == CTRL.EDayAwardStatus.eActive then
                objActive:SetActive(true)
                objNormal:SetActive(false)
                objCurrent = objActive
            elseif eNormalStatus == CTRL.EDayAwardStatus.eSign or eNormalStatus == CTRL.EDayAwardStatus.eNone then
                objActive:SetActive(false)
                objNormal:SetActive(true)
                
                objCurrent = objNormal
            elseif eNormalStatus == CTRL.EDayAwardStatus.eReceived then
                objActive:SetActive(false)
                objNormal:SetActive(true)
                objCurrent = objNormal

                -- 已领取标记
                local objReceived = UnityTools.FindGo(objAwardItem.transform, "received")
                if objReceived ~= nil then
                    objReceived:SetActive(true)
                end

                -- 置灰
       
            end

            -- 设置奖励数值
            local lblGoldAmountNormal = UnityTools.FindCo(objNormal.transform, "UILabel", "goldAmount")
            if lblGoldAmountNormal ~= nil then
                lblGoldAmountNormal.text = "x" .. v:Amount()
            end

            local lblGoldAmountActive = UnityTools.FindCo(objActive.transform, "UILabel", "goldAmount")
            if lblGoldAmountActive ~= nil then
                lblGoldAmountActive.text = "x" .. v:Amount()
            end
        end
    end
end

-- 初始化VIP奖励部分
local function vipLoginAwardInitialize()
    if goWin == nil then return end

    local vipWin = UnityTools.FindGo(goWin.transform, "Container/main/awardVip")
    if vipWin == nil then return end

    -- 设置按钮状态
    local notVipTips = UnityTools.FindGo(vipWin.transform, "notVipTips")
    local btnRecharge = UnityTools.FindGo(vipWin.transform, "btnToRecharge")
    local btnGetAward = UnityTools.FindGo(vipWin.transform, "btnGetVipAward")
    local btnRecievedAward = UnityTools.FindGo(vipWin.transform, "btnVipRecievedAward")
    if notVipTips == nil or btnRecharge == nil or btnGetAward == nil then return end

    local eVipStatus = loginAwardDataMgr:vipAwardStatus()
    if eVipStatus == CTRL.EVipAwardStatus.eNotVip then
        notVipTips:SetActive(true)
        btnRecharge:SetActive(true)
        btnGetAward:SetActive(false)
        btnRecievedAward:SetActive(false)
    elseif eVipStatus == CTRL.EVipAwardStatus.eActive then
       btnRecharge:SetActive(false)
       btnGetAward:SetActive(true) 
       btnRecievedAward:SetActive(false)
       local lblTxt = UnityTools.FindCo(btnGetAward.transform, "UILabel", "text")
       if lblTxt ~= nil then
           lblTxt.text = LuaText.GetString("loginAwardBtnGetAward")
       end
    elseif eVipStatus == CTRL.EVipAwardStatus.eReceived then
        btnRecharge:SetActive(false)
        btnGetAward:SetActive(false) 
        btnRecievedAward:SetActive(true)
        -- local lblTxt = UnityTools.FindCo(btnGetAward.transform, "UILabel", "text")
        -- if lblTxt ~= nil then
        --     lblTxt.text = LuaText.GetString("loginAwardBtnReceived")
        -- end
    elseif  eVipStatus == CTRL.EVipAwardStatus.eNone then
        notVipTips:SetActive(true)
        btnRecharge:SetActive(false)
        btnGetAward:SetActive(false)
        btnRecievedAward:SetActive(false)
    end

    -- 设置文字显示
    local nextLev = loginAwardDataMgr:nextVipLev()
    local vipLevelUpTips = UnityTools.FindGo(vipWin.transform, "vipLevelUpTips")
    local vipTopLevelTips = UnityTools.FindGo(vipWin.transform, "vipTopLevelTips")
    local vipAwardTips = UnityTools.FindGo(vipWin.transform, "vipAwardTips")
    if vipLevelUpTips == nil or vipAwardTips == nil then return end

    if nextLev == nil then
        vipLevelUpTips:SetActive(false)

        vipTopLevelTips:SetActive(true)
        vipAwardTips:SetActive(true)

        local nMaxVipLev = loginAwardDataMgr:maxVipLev()
        local curVipData = loginAwardDataMgr:getSpecificVipData(nMaxVipLev)
        if curVipData ~= nil then
            local nAwardAmount = curVipData:awardAmount()
            -- 设置可领取奖励
            local lblAwardAmount = UnityTools.FindCo(vipAwardTips.transform, "UILabel", "awardAmount")
            if lblAwardAmount ~= nil then
                lblAwardAmount.text = tostring(nAwardAmount)
            end
        end

        local lblNextVipLev = UnityTools.FindCo(vipTopLevelTips.transform, "UILabel", "nextVipLevel")
        if lblNextVipLev ~= nil then
            lblNextVipLev.text = tostring("VIP" .. nMaxVipLev)
        end

    else
        local curVipData = loginAwardDataMgr:getSpecificVipData(nextLev-1)
        local nextVipData = loginAwardDataMgr:getSpecificVipData(nextLev)
        if curVipData ~= nil and nextVipData ~= nil then
            local nNeedRecharge = nextVipData:needMoney() - curVipData:needMoney()
            local nNextAwardAmount = nextVipData:awardAmount()
            
            -- 设置下一级VIP升级条件
            local lblNeedRecharge = UnityTools.FindCo(vipLevelUpTips.transform, "UILabel", "needMoney")
            if lblNeedRecharge ~= nil then
                lblNeedRecharge.text = tostring(nNeedRecharge)
            end
            local lblNextVipLev = UnityTools.FindCo(vipLevelUpTips.transform, "UILabel", "nextVipLevel")
            if lblNextVipLev ~= nil then
                lblNextVipLev.text = tostring("VIP" .. nextLev)
            end

            -- 设置下一级VIP可领取奖励
            local lblNextAwardAmount = UnityTools.FindCo(vipAwardTips.transform, "UILabel", "awardAmount")
            if lblNextAwardAmount ~= nil then
                lblNextAwardAmount.text = tostring(nNextAwardAmount)
            end
        end
    end

end
local isSet= false
-- VIP信息说明界面
local function vipAwardInfoInitialize()
    if vipAwardInfoWin == nil then return end

    for k, v in loginAwardDataMgr:vipAwardData() do
        -- 设置VIP图标和外框
        -- local vipIcon = UnityTools.FindCo(vipAwardInfoWin.transform, "UISprite", "content/Grid/vip_" .. k .. "/icon")
        -- if vipIcon ~= nil then
        --     vipIcon.spriteName = "cash"
        -- end
        -- local vipIconOutline = UnityTools.FindCo(vipAwardInfoWin.transform, "UISprite", "content/Grid/vip_" .. k .. "/icon/outline")
        -- if vipIconOutline ~= nil then
        --     vipIconOutline.spriteName = "gameHeadBox"
        -- end
        local sp = UnityTools.FindGo(vipAwardInfoWin.transform, "content/Grid/vip_" .. k .. "/icon/lev"):GetComponent("UISprite")
        if k>=8 and k<=10 then
            local effect = UnityTools.FindGo(vipAwardInfoWin.transform, "content/Grid/vip_" .. k .. "/icon/lev/biangkuang01/v"..k)
            effect.gameObject:SetActive(true)
            if effect ~= nil then
                if k == 10 then
                    local effect1 = UnityTools.FindGo(effect.transform, "kuang")
                    local effect2 = UnityTools.FindGo(effect.transform, "v101")
                    local child=nil
                    UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform, effect1.transform, 50);
                    UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform, effect2.transform, 60);
                    if isSet == false then
                        effect1.transform.localScale = UnityEngine.Vector3(effect1.transform.localScale.x*0.37,effect1.transform.transform.localScale.y*0.37,effect1.transform.transform.localScale.z*0.37)
                        effect2.transform.localScale = UnityEngine.Vector3(effect2.transform.localScale.x*0.37,effect2.transform.transform.localScale.y*0.37,effect2.transform.transform.localScale.z*0.37)
                        for i =1,effect1.transform.childCount do
                            child = effect1.transform:GetChild(i-1)
                            child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*0.37,child.transform.localScale.y*0.37,child.transform.localScale.z*0.37)
                        end
                        for i =1,effect2.transform.childCount do
                            child = effect2.transform:GetChild(i-1)
                            child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*0.37,child.transform.localScale.y*0.37,child.transform.localScale.z*0.37)
                        end
                    end
                else
                    UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform, effect.transform, 100);
                    local child=nil
                    if isSet == false then
                        effect.transform.localScale = UnityEngine.Vector3(effect.transform.localScale.x*0.37,effect.transform.transform.localScale.y*0.37,effect.transform.transform.localScale.z*0.37)
                        
                        for i =1,effect.transform.childCount do
                            child = effect.transform:GetChild(i-1)
                            child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*0.37,child.transform.localScale.y*0.37,child.transform.localScale.z*0.37)
                        end
                    end
                end
                
            end
            sp.enabled = false
        else
            sp.spriteName = "vip"..k
        end
        
        -- 设置数值显示
        local lblAmount = UnityTools.FindCo(vipAwardInfoWin.transform, "UILabel", "content/Grid/vip_" .. k .. "/amount")
        if lblAmount ~= nil then
            lblAmount.text = tostring(v:awardAmount())
        end
    end
    isSet = true
end



-- 设置常规领奖按钮状态
local function setNormalGetAwardBtnState()
    -- 设置领取按钮状态
    local bGotTodayAward = loginAwardDataMgr:isReceivedTodayAward()
    local strTextKey = ""
    if not bGotTodayAward then
        strTextKey = "loginAwardBtnGetAward"
        UnityTools.SetActive(btnGetNormalAward.gameObject,true)
        UnityTools.SetActive(btnRecievedAward.gameObject,false)
    else
        local bHaveMakeupDays = false
        if bHaveMakeupDays then
            strTextKey = "loginAwardBtnMakeupGetAward"
            UnityTools.SetActive(btnGetNormalAward.gameObject,true)
            UnityTools.SetActive(btnRecievedAward.gameObject,false)
        else
            strTextKey = "loginAwardBtnReceived"
            UnityTools.SetActive(btnGetNormalAward.gameObject,false)
            UnityTools.SetActive(btnRecievedAward.gameObject,true)
        end
    end

    if btnGetNormalAward ~= nil then
        local lblBtnText = UnityTools.FindCo(btnGetNormalAward.transform, "UILabel", "text")
        if lblBtnText ~= nil then
            lblBtnText.text = LuaText.GetString(strTextKey)
        end
    end   
end

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 响应UI刷新事件

-- 主界面刷新
function onLoginAwardWinRefreshEvent()
    normalLoginAwardInitialize()
    vipLoginAwardInitialize()
    setNormalGetAwardBtnState()
    vipAwardInfoInitialize()
end

-- 更新指定天的UI事件
function onLoginAwardUpdateDayEvent(nEventId, nDayIndex)
    local objAwardItem = UnityTools.FindGo(goWin.transform, "Container/main/awardNormal/awardItem_" .. nDayIndex)
    if objAwardItem == nil then
        return 
    end

    local awardData = loginAwardDataMgr:getSpecificDailyAwardData(nDayIndex)
    if awardData == nil then
        return
    end
    
    -- 控制状态显示
    local objActive = UnityTools.FindGo(objAwardItem.transform, "active")
    local objNormal = UnityTools.FindGo(objAwardItem.transform, "normal")
    local objSignTag = UnityTools.FindGo(objAwardItem.transform, "signTag")
    local objCurrent = nil
    local eNormalStatus = awardData:status()

    -- 补签标识
    objSignTag:SetActive(eNormalStatus == CTRL.EDayAwardStatus.eSign)

    if eNormalStatus == CTRL.EDayAwardStatus.eActive then
        objActive:SetActive(true)
        objNormal:SetActive(false)
        objCurrent = objActive
    elseif eNormalStatus == CTRL.EDayAwardStatus.eSign then
        objActive:SetActive(false)
        objNormal:SetActive(true)
        objCurrent = objNormal
    elseif eNormalStatus == CTRL.EDayAwardStatus.eReceived then
        objActive:SetActive(false)
        objNormal:SetActive(true)
        objCurrent = objNormal      -- 已领取标记
        local objReceived = UnityTools.FindGo(objAwardItem.transform, "received")
        if objReceived ~= nil then
            objReceived:SetActive(true)
        end     
        

    end
    -- 设置领取按钮状态
    setNormalGetAwardBtnState()
end

-- VIP奖励状态改变事件
function onLoginAwardVipAwardStatusChangeEvent(eVipAwardStatus)
    if goWin == nil then return end
    local vipWin = UnityTools.FindGo(goWin.transform, "Container/main/awardVip")
    if vipWin == nil then return end

    -- 设置按钮状态
    local btnGetAward = UnityTools.FindGo(vipWin.transform, "btnGetVipAward")
    local btnRecievedAward = UnityTools.FindGo(vipWin.transform, "btnVipRecievedAward")
    if btnGetAward == nil or btnRecievedAward == nil then
        return
    end
    btnGetAward:SetActive(false)
    btnRecievedAward:SetActive(true)
    -- local lblBtnText = UnityTools.FindCo(btnGetAward.transform, "UILabel", "text")
    -- if lblBtnText ~= nil then
    --     lblBtnText.text = LuaText.GetString("loginAwardBtnReceived")
    -- end
end



-- /////////////////////////////////////////////////////////////////////////////////////////////
-- MonoBehaviour回调

local function Awake(gameObject)
    goWin = gameObject

    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    -- 注册UI事件
    registerScriptEvent(LOGIN_AWARD_UI_EVENT_REFRESH_WIN, "onLoginAwardWinRefreshEvent")
    registerScriptEvent(LOGIN_AWARD_UI_EVENT_UPDATE_DAY, "onLoginAwardUpdateDayEvent")
    registerScriptEvent(LOGIN_AWARD_UI_EVENT_VIP_AWARD_STATUS_CHANGE, "onLoginAwardVipAwardStatusChangeEvent")
end


local function Start(gameObject)
    vipAwardInfoInitialize()
end


local function OnDestroy(gameObject)
    goWin = nil
    CLEAN_MODULE("LoginAwardWinMono")

    -- 取消监听UI事件
    unregisterScriptEvent(LOGIN_AWARD_UI_EVENT_REFRESH_WIN, "onLoginAwardWinRefreshEvent")
    unregisterScriptEvent(LOGIN_AWARD_UI_EVENT_UPDATE_DAY, "onLoginAwardUpdateDayEvent")
    unregisterScriptEvent(LOGIN_AWARD_UI_EVENT_VIP_AWARD_STATUS_CHANGE, "onLoginAwardVipAwardStatusChangeEvent")
end


-- /////////////////////////////////////////////////////////////////////////////////////////////


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
