-- -----------------------------------------------------------------
-- * Copyright (c) 2016 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    timer.lua
-- * Summary:     计时器
-- *
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2016-11-21 17:34:31
-- -----------------------------------------------------------------


gTimer = {

}

local _deltaTime = 0 
local _timerCnt = 0
local _timerTable = {}

local _recyclingTable = {}
local _tempTimeoutTable = {}

function gTimer.setRecycler(object, timer)
    if timer ~= nil then
        if _recyclingTable[object] == nil then
            _recyclingTable[object] = {}
        end
        _recyclingTable[object][#_recyclingTable[object] + 1] = timer
    end
end

function gTimer.recycling(object)
    if _recyclingTable[object] ~= nil then
        for k, v in pairs(_recyclingTable[object]) do
            _timerTable[v] = nil
        end
    end
end

-- delay : 第一次延时调用  repeatTime : 间隔毫秒调用次数 funcStr ： 调用函数 ... ：参数列表
function gTimer.registerDelayTimerEvent(delay, repeatTime, funcStr, ...)
    if funcStr ~= nil and _timerTable[funcStr] == nil and delay >= 0 then
        _timerTable[funcStr] = {[1] = 0, [2] = delay, [3] = repeatTime, [4] = funcStr, [5] = {...}}
        return funcStr
    else
        error("function is nil or function is exsited")
        return nil
    end
end

function gTimer.registerOnceTimer(repeatTime, funcStr, ...)
    return gTimer.registerDelayTimerEvent(repeatTime, 0, funcStr, ...)
end

function gTimer.registerRepeatTimer(repeatTime, funcStr, ...)
    return gTimer.registerDelayTimerEvent(repeatTime, repeatTime, funcStr, ...)
end

function gTimer.removeTimer(funcStr)
    if funcStr ~= nil and _timerTable[funcStr] ~= nil then
        _timerTable[funcStr] = nil
    end
end

function gTimer.hasTimer(funcStr)
    if funcStr ~= nil and _timerTable[funcStr] ~= nil then
        return true
    else
        return false
    end
end

function gTimer.timerCount()
    return _timerCnt
end

function gTimer.doTimer(deltaTime)
    _deltaTime = deltaTime
    local cnt = 0
	local timeoutCount = 0
    for i, v in pairs(_timerTable) do
        cnt = cnt + 1
        v[1] = v[1] + deltaTime * 1000
        -- 第二次调用
        if v[2] == -1 then
            if v[1] >= v[3] then
                v[1] = 0
				timeoutCount = timeoutCount + 1
				_tempTimeoutTable[timeoutCount] = {deltaTime,v[4],v[5]}
            end
        -- 首次延迟调用
        elseif v[1] >= v[2] then
            v[2] = -1
            v[1] = 0
            if v[3] <= 0 then
                _timerTable[i] = nil
            end
			timeoutCount = timeoutCount + 1
			_tempTimeoutTable[timeoutCount] = {deltaTime,v[4],v[5]}
        end
    end
	
	if #_tempTimeoutTable > 0 then
		for i,v in pairs(_tempTimeoutTable) do
            gTimer.callTimeOutEvent(v[1], v[2], unpack(v[3]))
		end
		_tempTimeoutTable = {}
	end
    _timerCnt = cnt
end

function gTimer.callTimeOutEvent(deltaTime, funcStr, ...)
    if type(funcStr) ~= "string" then
        pcall(funcStr(...))
        return
    end
    if _G[funcStr] == nil then
        error("gTimer can not find " .. funcStr)
    else
        _G[funcStr](...)
    end
end

function gTimer.deltaTime()
    return _deltaTime
end
