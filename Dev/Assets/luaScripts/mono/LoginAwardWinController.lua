-- -----------------------------------------------------------------


-- *
-- * Filename:    LoginAwardWinController.lua
-- * Summary:     登录奖励脚本逻辑
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        3/17/2017 10:30:52 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("LoginAwardWinController")

local platformMgr = IMPORT_MODULE("PlatformMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()
local UnityTools = IMPORT_MODULE("UnityTools")

-- 界面名称
local wName = "LoginAwardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

-- 最大登录天数
local MAX_LOGIN_DAYS = 7

-- 最大VIP等级
local MAX_VIP_LEVEL = 10

-- ----------------------
-- 每天奖励状态
-- ----------------------
local EDayAwardStatus = {
    eNone       = 0,    -- 无
    eActive     = 1,    -- 当前活跃
    eSign       = 2,    -- 可补签
    eReceived   = 3,    -- 已领取
}

-- ----------------------
-- VIP奖励状态
-- ----------------------
local EVipAwardStatus = {
    eNone       = 0,    -- 无
    eNotVip     = 1,    -- 非VIP
    eActive     = 2,    -- 可领取
    eReceived   = 3,    -- 已领取
}


-- ///////////////////////////////////////////////////////////////////////////////
-- // 登录奖励数据对象

-- 每天的登录奖励数据类
local CDailyAwardData = {
    _nIndex     = 0,    -- 天数索引
    _idItem     = 0,    -- 奖励物品ID
    _nAmount    = 0,    -- 奖励数量
    _eStatus    = EDayAwardStatus.eNone, -- 奖励状态
}

-- constructor
function CDailyAwardData:new(o)
    local o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

-- 索引
function CDailyAwardData:index()
    return self._nIndex
end

-- 奖励物品id
function CDailyAwardData:idItem()
    return self._idItem
end

-- 物品名字
function CDailyAwardData:idItem()
    return tostring(self._idItem)
end

-- 奖励物品图标
function CDailyAwardData:itemIcon()
    return "C" .. tostring(self._idItem)
end

-- 数量
function CDailyAwardData:Amount()
    return self._nAmount
end

-- 奖励状态
function CDailyAwardData:status()
    return self._eStatus
end

-- ////////////////////////////////////////////////////////////////////////////////////
-- VIP数据对象

local CVipData = {
    _nVipLev        = 0,    -- VIP等级
    _nNeedMoney     = 0,    -- 需要的人民币
    _idAwardItem    = 0,    -- 额外奖励的物品id
    _nAmount        = 0,    -- 额外奖励的数量
    _strTitle       = "",   -- 标题
}

-- constructor
function CVipData:new(O)
    local o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

-- VIP等级
function CVipData:vipLev()
    return self._nVipLev
end

-- 需要充值的RMB
function CVipData:needMoney()
    return self._nNeedMoney
end

-- 额外奖励物品
function CVipData:awardItem()
    return self._idAwardItem
end

-- 额外奖励数量
function CVipData:awardAmount()
    return self._nAmount
end

-- VIP标题
function CVipData:titleName()
    return self._strTitle
end

-- ////////////////////////////////////////////////////////////////////////////////////
-- // 每日登录奖励数据管理

-- 管理类
local LoginAwardDataMgr = {
    _bReceiveTodayAward = false, -- 领取当前奖励
    _eVipAwardStatus = EVipAwardStatus.eNone,   -- VIP奖励状态
    _tDailyAwardData = {}, -- 登录奖励数据
    _tVipData = {}, -- VIP数据
}


-- 初始化
function LoginAwardDataMgr:initialize()
    local vipCfgData = LuaConfigMgr.VipConfig
    if vipCfgData == nil then
        LogWarn("vip 配置数据错误")
        return
    end

    for k, v in pairs(vipCfgData) do
        local vipData = CVipData:new()
        vipData._nVipLev = tonumber(v.key)
        vipData._nNeedMoney = tonumber(v.need_gold)
        vipData._idAwardItem = tonumber(v.reward[1][2])
        vipData._nAmount = tonumber(v.reward[1][3])
        vipData._strTitle = v.title2

        self:addVipData(vipData)
    end

    -- 添加辅助VIP0的数据
    local vipData0 = CVipData:new()
    self:addVipData(vipData0)
end

-- 添加每天奖励数据
function LoginAwardDataMgr:addDayAwardData(objAwardData)
    if objAwardData == nil then return end

    -- 检测重复数据
    local idx = objAwardData:index()
    if self._tDailyAwardData[idx] ~= nil then
        return
    end

    self._tDailyAwardData[idx] = objAwardData
end

-- 添加VIP配置数据
function LoginAwardDataMgr:addVipData(objVipData)
    if objVipData == nil then return end

    local nVipLev = objVipData._nVipLev
    if self._tVipData[nVipLev] ~= nil then
        return
    end

    self._tVipData[nVipLev] = objVipData
end

-- 获取某一天的奖励数据
function LoginAwardDataMgr:getSpecificDailyAwardData(nDayIndex)
    if nDayIndex <= 0 or  nDayIndex > MAX_LOGIN_DAYS then return nil end
    return self._tDailyAwardData[nDayIndex]
end

-- 获取VIP奖励数据
function LoginAwardDataMgr:getSpecificVipData(nVipLev)
    if nVipLev < 0 or  nVipLev > MAX_VIP_LEVEL then return nil end
    return self._tVipData[nVipLev]
end

-- VIP奖励状态
function LoginAwardDataMgr:vipAwardStatus()
    return self._eVipAwardStatus;
end


local function iter(t, i)
    return next, t, nil
end

-- 每日奖励数据(迭代器)
function LoginAwardDataMgr:dayAwardData()
    return next, self._tDailyAwardData, nil
end

-- VIP奖励数据（迭代器）
function LoginAwardDataMgr:vipAwardData()
    return next, self._tVipData, nil
end

-- 下一级VIP
function LoginAwardDataMgr:nextVipLev()
    local nextLev = platformMgr.GetVipLv() + 1
    if nextLev <= MAX_VIP_LEVEL then
        return nextLev
    else
        return nil
    end
end

-- 最大VIP等级
function LoginAwardDataMgr:maxVipLev()
    return MAX_VIP_LEVEL
end


-- 是否领取当天的奖励
function LoginAwardDataMgr:isReceivedTodayAward()
    return self._bReceiveTodayAward
end

-- 是否有补签天数
function LoginAwardDataMgr:isHaveMakeupDays()
    for k, v in ipairs(self._tDailyAwardData) do
        if v._eStatus == EDayAwardStatus.eSign then
            return true
        end
    end

    return false   
end

-- 是否有补签卡
function LoginAwardDataMgr:isHaveMakeupCards()
    local itemMgr = IMPORT_MODULE("ItemMgr")
    if itemMgr ~= nil then
        local nCount = itemMgr.GetItemNum(100002)
        return nCount > 0
    end
    
    return false
end

-- 是否满足游戏开始打开登录奖励窗口条件
function LoginAwardDataMgr:isOpenLoginAwardWinOnGameStartup()
    if platformMgr.config_vip == false then
        return false;
    elseif platformMgr.GetGuideStep() ~= 2 then
        return false
    elseif not self._bReceiveTodayAward then
        return true
    elseif self._eVipAwardStatus == EVipAwardStatus.eActive then
        return true
    elseif self:isHaveMakeupDays() and self:isHaveMakeupCards() then
        return true
    else
        return false
    end
end

-- 当天签到请求
function LoginAwardDataMgr:sendTodayCheckInRequest()
    local nDayIdx = -1
    for k, v in ipairs(self._tDailyAwardData) do
        if v._eStatus == EDayAwardStatus.eActive then
            nDayIdx = v._nIndex
            break
        end
    end

    if nDayIdx == -1 then
        return
    end

    local tMsg = {}
    tMsg.flag = nDayIdx
    protobuf:sendMessage( protoIdSet.cs_daily_checkin_req, tMsg)
end

-- 补签请求
function LoginAwardDataMgr:sendMakeupCheckInRequest()
    local nMinDayIdx = 999
    for k, v in ipairs(self._tDailyAwardData) do
        if v._nIndex < nMinDayIdx and  v._eStatus == EDayAwardStatus.eSign then
            nMinDayIdx = v._nIndex
        end
    end

    if nMinDayIdx > MAX_LOGIN_DAYS then
        return
    end

    local tMsg = {}
    tMsg.flag = nMinDayIdx
    protobuf:sendMessage( protoIdSet.cs_make_up_for_checkin_req, tMsg)
end

-- 发送VIP奖励请求
function LoginAwardDataMgr:sendGetVipAwardRequest()
    local tMsg = {}
    protobuf:sendMessage( protoIdSet.cs_vip_daily_reward, tMsg)
end

-- 清理数据
function LoginAwardDataMgr:clear()
    self._bReceiveTodayAward = false -- 领取当前奖励
    self._eVipAwardStatus = EVipAwardStatus.eNone   -- VIP奖励状态
    self._tDailyAwardData = {} -- 登录奖励数据
    self._tVipData = {} -- VIP数据
end

-- ////////////////////////////////////////////////////////////////////////////////////
-- 服务端消息处理

-- 签到奖励信息更新
function onDailyCheckInInfoUpdate(msgId, tMsgData)
    if tMsgData == nil then return end
    local tDailyAwardList = tMsgData.list
    local nMaxDayIdx = tMsgData.all_checkin_day
    local bIsReceivedToday = tMsgData.is_checkin_today

    LoginAwardDataMgr._bReceiveTodayAward = bIsReceivedToday
    local nTodayIdx = 1
    for k, v in pairs(tDailyAwardList) do
        local daiyAwardData = CDailyAwardData:new()
        daiyAwardData._nIndex = v.day
        daiyAwardData._idItem = v.rewards[1].base_id
        daiyAwardData._nAmount = v.rewards[1].count

        -- 设置状态
        if v.is_draw then
            nTodayIdx = nTodayIdx + 1
            daiyAwardData._eStatus = EDayAwardStatus.eReceived
        elseif v.day <= nMaxDayIdx then
            daiyAwardData._eStatus = EDayAwardStatus.eSign
        else
            daiyAwardData._eStatus = EDayAwardStatus.eNone
        end

        -- 添加到管理器
        LoginAwardDataMgr:addDayAwardData(daiyAwardData)
    end

    -- 当天未领取时设置当天的状态为active，否则全部为补签状态
    local todayAwardData = LoginAwardDataMgr:getSpecificDailyAwardData(nTodayIdx)
    if todayAwardData ~= nil and not bIsReceivedToday then
        todayAwardData._eStatus = EDayAwardStatus.eActive
    end

    -- 初始化配置数据
    LoginAwardDataMgr:initialize()

    -- 初始化VIP状态
    local nVipLev = platformMgr.GetVipLv()
    if nVipLev <= 0 then
        LoginAwardDataMgr._eVipAwardStatus = EVipAwardStatus.eNotVip
    elseif tMsgData.vip_is_draw then
        LoginAwardDataMgr._eVipAwardStatus = EVipAwardStatus.eReceived
    else
        LoginAwardDataMgr._eVipAwardStatus = EVipAwardStatus.eActive
    end
    
--    local bOpenAwardWin = LoginAwardDataMgr:isOpenLoginAwardWinOnGameStartup()
    --[[if bOpenAwardWin then
        UnityTools.CreateLuaWin("LoginAwardWin")
    end]]
--    platformMgr.InitStartOpenWin(bOpenAwardWin);
end

-- 每日签到消息返回
function onDailyCheckInReplyMsg(msgId, tMsgData)
    if tMsgData == nil then return end

    local nRlt = tMsgData.result
    if nRlt == 0 then
        local nDayIdx = tMsgData.flag
        local awardData =  LoginAwardDataMgr:getSpecificDailyAwardData(nDayIdx)
        if awardData ~= nil then
            -- 如果有收到签到奖励，则当天奖励一定已经领取
            LoginAwardDataMgr._bReceiveTodayAward = true

            awardData._eStatus = EDayAwardStatus.eReceived
            ShowAwardWin(tMsgData.rewards)
            -- 触发UI更新事件
            triggerScriptEvent(LOGIN_AWARD_UI_EVENT_UPDATE_DAY, nDayIdx)
        end
    else
        local strErrMsg = tMsgData.err
        UnityTools.ShowMessage(strErrMsg)
    end
end

-- VIP奖励返回
function onVipDailyRewardReplyMsg(msgId, tMsgData)
    if tMsgData == nil then
        return
    end
    
    local nRlt = tMsgData.result
    if nRlt == 0 then 
        LoginAwardDataMgr._eVipAwardStatus = EVipAwardStatus.eReceived
        ShowAwardWin(tMsgData.rewards)

        -- 触发VIP更新事件
        triggerScriptEvent(LOGIN_AWARD_UI_EVENT_VIP_AWARD_STATUS_CHANGE)
    else
        local strErrMsg = tMsgData.err
        UnityTools.ShowMessage(strErrMsg)
    end
    
end


-- des:签到配置信息
protobuf:registerMessageScriptHandler(protoIdSet.sc_daily_checkin_info_update, "onDailyCheckInInfoUpdate")
-- des:每日签到返回
protobuf:registerMessageScriptHandler(protoIdSet.sc_daily_checkin_reply, "onDailyCheckInReplyMsg")
-- des:领取VIP奖励返回
protobuf:registerMessageScriptHandler(protoIdSet.sc_vip_daily_reward, "onVipDailyRewardReplyMsg")


-- ////////////////////////////////////////////////////////////////////////////////////
-- 窗口事件

-- 创建
local function OnCreateCallBack(gameObject)

    -- 刷新界面
    triggerScriptEvent(LOGIN_AWARD_UI_EVENT_REFRESH_WIN)
end

-- 销毁
local function OnDestoryCallBack(gameObject)

end

UI.Controller.UIManager.RegisterLuaWinFunc("LoginAwardWin", OnCreateCallBack, OnDestoryCallBack)


-- ////////////////////////////////////////////////////////////////////////////////////
-- 游戏事件

-- 清理数据
function onClearAllLoginAwardData()
    LoginAwardDataMgr:clear()
end


registerScriptEvent(EXIT_CLEAR_ALL_DATA, "onClearAllLoginAwardData")

-- ////////////////////////////////////////////////////////////////////////////////////
-- 全局函数

-- 打开界面
function openLoginAwardWin()
    -- 初始化VIP状态
    local nVipLev = platformMgr.GetVipLv()
    if nVipLev > 0 and  LoginAwardDataMgr._eVipAwardStatus == EVipAwardStatus.eNotVip  then
        LoginAwardDataMgr._eVipAwardStatus = EVipAwardStatus.eActive
    end
    
    UnityTools.CreateLuaWin("LoginAwardWin")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.LoginAwardDataMgr = LoginAwardDataMgr
M.EVipAwardStatus = EVipAwardStatus
M.EDayAwardStatus = EDayAwardStatus



-- 返回当前模块
return M
