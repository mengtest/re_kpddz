-- --------------------------------------------------


-- *
-- * Summary: 	基础工具函数
-- * Version: 	1.0.0
-- * Author: 	WP.Chu
-- --------------------------------------------------


-- ---------------------
-- 功能：字符串按格式分解
-- 输入：字符串，格式符号
-- 输出：table（分割好的字符串）
-- ---------------
function stringToTable(strVal, strSplit)
    local t = {}
    if nil == strVal then
        return t
    end
    -- delete blank of head and tail
    local s = string.gsub(strVal, "^%s*(.-)%s*$", "%1")
    if nil == string.find(strSplit, " ") then
        local strTmp = string.gsub(s, " ", "")
        local strAll = string.gsub(strTmp, strSplit, " ")
        s = strAll
    end
    --create table
    for k, v in string.gmatch(s, "%S*") do
        if 0 ~= string.len(k) then
            t[#t + 1] = k
        end
    end
    return t
end


-- ---------------------
-- 功能：获取表中元素的个数
-- 输入：table
-- 输出：表中元素个数
-- ---------------------
function table_getn(tTable)
    local num = 0
    if tTable and type(tTable) == "table" then
        for i, v in pairs(tTable) do
            num = num + 1
        end
    end
    return num;
end

-- ---------------------
-- 功能：tTable中查找value,返回key
-- 输入：table
-- 输出：value对应的key，找不到value返回nil
-- ---------------------
function table_v2k(tTable, value)
    if value and type(tTable) == "table" then
        for i, v in pairs(tTable) do
            if value == v then
                return i
            end
        end
    end
    return nil
end


-- ---------------------
-- 功能：合并2个表
-- 输入：tTargetTbl, tSourceTbl
-- 输出：合并后的tTargetTbl
-- ---------------------
function table_combine(tTargetTbl, tSourceTbl)
    for i, v in pairs(tSourceTbl) do
        table.insert(tTargetTbl, v)
    end
end


-- ---------------------
-- 功能：时间转换
-- 输入：秒数
-- 输出：四个值 天数,小时,分钟,秒数
-- ---------------
function sec2DayHourMinSec(nSec)
    if nil ~= nSec and nil ~= string.find(nSec, "%D") then
        return 0, 0, 0, 0
    end

    return math.floor(nSec / (3600 * 24)), math.floor(nSec / 3600) % 24, math.floor(nSec / 60) % 60, nSec % 60
end



-- ------------------------------
-- 功能：时间戳转化时钟格式
-- 输入：时间戳
-- 输出：11:22:33
-- ------------------------------
function dwTimeToCurrentStrTime(dwTime)
    if nil ~= string.find(dwTime, "%D") then
        return nil
    end

    local nDay = 0
    local nHour = 0
    local nMin = 0
    local nSec = 0

    nDay, nHour, nMin, nSec = sec2DayHourMinSec(dwTime)
    if nDay > 0 then
        nHour = nHour + 24 * nDay
    end

    --时间如果是1位数的话 前面加0
    local strHour = tostring(nHour)
    local strMin = tostring(nMin)
    local strSec = tostring(nSec)
    if nHour < 10 then
        strHour = "0" .. tostring(nHour)
    end
    if nMin < 10 then
        strMin = "0" .. tostring(nMin)
    end
    if nSec < 10 then
        strSec = "0" .. tostring(nSec)
    end
    local str = strHour .. ":" .. strMin .. ":" .. strSec
    return str
end

--作者：songDF
--功能：时间戳转为具体的日期
--输入：时间戳
--输出：今日18:00 或者明日：18：00
function dwTimeToStrDate(dwTime)
    --当前日期
    local currentTab = os.date("*t")
    --目标日期
    local targetTab = os.date("*t", dwTime)
    --日期差
    local nNum = targetTab.day - currentTab.day
    --计算日期
    local strDay = "label_today"
    if nNum == 0 then
        --今天
        strDay = "label_today"
    elseif nNum == 1 then
        --明天
        strDay = "label_tomorrow"
    elseif nNum == 2 then
        --后天
        strDay = "label_afertomorrow"
    end
    --计算时间
    local nHour = targetTab.hour
    local nMin = targetTab.min
    local strHour = tostring(nHour)
    local strMin = tostring(nMin)
    if nHour < 10 then
        strHour = "0" .. tostring(nHour)
    end
    if nMin < 10 then
        strMin = "0" .. tostring(nMin)
    end
    --日期字符串
    local str = getGameText(strDay) .. strHour .. ":" .. strMin
    return str
end

--作者：songDF
--功能：时间戳转为具体的,日期 和时间
--输入：时间戳
--输出：22,2014年8月22号 和 18:55
function dwTimeToStrDateAndTime(dwTime)
    --目标日期
    local targetTab = os.date("*t", dwTime)
    --计算日期
    local nYear = targetTab.year
    local nMonth = targetTab.month
    local nDay = targetTab.day
    local strYear = tostring(nYear)
    local strMonth = tostring(nMonth)
    local strDay = tostring(nDay)
    --日期字符串
    local strDate = strYear .. getGameText("label_year") .. strMonth .. getGameText("label_month") .. strDay .. getGameText("label_day_name")

    --计算时间
    local nHour = targetTab.hour
    local nMin = targetTab.min
    local strHour = tostring(nHour)
    local strMin = tostring(nMin)
    if nHour < 10 then
        strHour = "0" .. tostring(nHour)
    end
    if nMin < 10 then
        strMin = "0" .. tostring(nMin)
    end
    --时间字符串
    local strTime = strHour .. ":" .. strMin
    return strDay, strDate, strTime
end

--实时serverTime
function getServerTime()
    local serTime = os.time() + MessageTick:getServerTimeDiff();
    return serTime;
end

-- ------------------------------
-- 功能：时间段转化汉字格式
-- 输入：时间段 比如 60s 800s
-- 输出：1小时1分0秒
-- ------------------------------
function nTimeQuantumToChineseTime(dwTime)
    return secTimeToChineseTime(dwTime)
end


-- ------------------------------
-- 功能：时间戳转化汉字格式
-- 输入：时间戳
-- 输出：1小时1分0秒
-- ------------------------------
function dwTimeToChineseTime(dwTime)
    dwTime = dwTime - os.time() - MessageTick:getServerTimeDiff()
    return secTimeToChineseTime(dwTime)
end


-- ------------------------------
-- 功能：时间戳转化汉字格式
-- 输入：时间戳
-- 输出：1小时1分0秒
-- ------------------------------
function dwTimeCountToChineseTime(dwTime)
    dwTime = os.time() - dwTime + MessageTick:getServerTimeDiff()
    return secTimeToChineseTime(dwTime)
end

-- ------------------------------
-- 功能：数转化成汉字天小时分钟秒表达式(只留2个单位的时间)
--比如12天32小时15分钟30秒，只显示12天32小时如果是32小时15分钟30秒，只显示32小时15分钟，以此类推
-- ------------------------------
function secTimeToChineseTime_TwoNum(dwTime)
    if nil ~= dwTime and nil ~= string.find(dwTime, "%D") then
        return ""
    end

    local nDay = 0
    local nHour = 0
    local nMin = 0
    local nSec = 0

    nDay, nHour, nMin, nSec = sec2DayHourMinSec(dwTime)
    local strDay = "%d天%d小时"
    local strHour = "%d小时%d分钟"
    local strMinute = "%d分钟%d秒"
    local strSec = "%d秒"
    local strReturn = ""
    if nDay ~= 0 then
        return string.format(strDay, nDay, nHour)
    elseif nHour ~= 0 then
        return string.format(strHour, nHour, nMin)
    elseif nMin ~= 0 then
        return string.format(strMinute, nMin, nSec)
    elseif nSec ~= 0 then
        return string.format(strSec, nSec)
    end
    return ""
end

-- ------------------------------
-- 功能：数转化成汉字天小时分钟秒表达式(只留1个单位的时间)
--比如12天32小时15分钟30秒，只显示12天 如果是32小时15分钟30秒，只显示32小时15分钟，以此类推
-- ------------------------------
function secTimeToChineseTime_OneNum(dwTime)
    if nil ~= dwTime and nil ~= string.find(dwTime, "%D") then
        return ""
    end

    local nDay = 0
    local nHour = 0
    local nMin = 0
    local nSec = 0

    nDay, nHour, nMin, nSec = sec2DayHourMinSec(dwTime)
    local strDay = "%d天"
    local strHour = "%d小时"
    local strMinute = "%d分钟"
    local strSec = "%d秒"
    local strReturn = ""
    if nDay ~= 0 then
        return string.format(strDay, nDay)
    elseif nHour ~= 0 then
        return string.format(strHour, nHour)
    elseif nMin ~= 0 then
        return string.format(strMinute, nMin)
    elseif nSec ~= 0 then
        return string.format(strSec, nSec)
    end
    return ""
end



-- --------------------------
-- 计算定比分点的坐标
--
-- 注： 在直角坐标系内，已知两点A(x1,y1),B(x2,y2);在两点连线上有一点P,设它的坐标为(x,y),且向量AP比向量PB的比值为λ,那么我们说P分有向线段AB的比为λ
--
--  且P的坐标为
--		x=(x1 + λ · x2) / (1 + λ)
--		y=(y1 + λ · y2) / (1 + λ)
--
-- --------------------------
function dividedPointsByDefiniteProportion(tStartPoint, tEndPoint, fProportion)
    local x1, y1 = tStartPoint[1], tStartPoint[2]
    local x2, y2 = tEndPoint[1], tEndPoint[2]

    local fRltX = (x1 + fProportion * x2) / (1 + fProportion)
    local fRltY = (y1 + fProportion * y2) / (1 + fProportion)

    return fRltX, fRltY
end



-- ------------------------------------------
-- 计算两直线交点（简单期间垂直和水平没考虑）
-- 直线方程：两点式
-- line1: A点：{x11, y11}, B点: {x12, y12}
-- line2: A点：{x21, y21}, B点: {x22, y22}
-- -----------------------------------------
function calcCrossPointOfLine(tLine1, tLine2)
    if tLine1 == nil or tLine1[1] == nil or tLine1[1][2] == nil then
        return 0, 0
    end

    -- y = a*x + b
    local a1 = (tLine1[1][2] - tLine1[2][2]) / (tLine1[1][1] - tLine1[2][1])
    local b1 = tLine1[1][2] - a1 * (tLine1[1][1])

    local a2 = (tLine2[1][2] - tLine2[2][2]) / (tLine2[1][1] - tLine2[2][1])
    local b2 = tLine2[1][2] - a2 * (tLine2[1][1])

    local x = (b1 - b2) / (a2 - a1)
    local y = a1 * x + b1

    return x, y
end

-- --------------------
-- 功能：输入颜色 --- 返回 RGB 值，默认红色。
-- --------------------
function getColorByName(strName)
    local tColor = { 255, 0, 0 }
    if strName == nil or strName == "red" then
        return tColor
    elseif strName == "green" then
        tColor = { 0, 255, 0 }
    elseif strName == "blue" then
        tColor = { 0, 0, 255 }
    elseif strName == "white" then
        tColor = { 255, 255, 255 }
    elseif strName == "black" then
        tColor = { 0, 0, 0 }
    elseif strName == "yellow" then
        tColor = { 255, 225, 14 }
    end
    return tColor
end

--将nValue转为二进制,取从左往右第nPos位的值
function getBinaryNumByPos(nValue, nPos)
    if nPos <= 0 then
        return 0
    end
    local nBinary = math.ldexp(1, nPos - 1) --2的(nPos-1)次方
    local nOffsetBinary = math.floor(nValue / nBinary) --二进制往右偏nPos-1位
    local nBinaryNum = math.mod(nOffsetBinary, 2) --取二进制末位
    return nBinaryNum
end

-- -------------
-- 模块名字
-- -------------
function MODULE_NAME(strModuleName)
    local strModuleName = "__" .. g_strGameName .. "_" .. strModuleName .. "__"
    return strModuleName
end

-- 创建一个全局弱引用表
local _weakGlobalTable = {}
setmetatable(_weakGlobalTable, { __mode = "v" })

-- -------------
-- 检查模块是否存在引用
-- -------------
function CHECK_MODULE(strModuleName)
    local strModuleName = MODULE_NAME(strModuleName)
    if _weakGlobalTable[strModuleName] ~= nil then return true else return false end
end

function GET_WEAK_GTABLE()
    return _weakGlobalTable
end

-- -------------
-- 生成模块(for lua5.1 or lower)
-- -------------
function GENERATE_MODULE(strModuleName)
    local version = _G._VERSION
    local strModuleName = MODULE_NAME(strModuleName)

    local M = {}
    _G[strModuleName] = M
    package.loaded[strModuleName] = M
    _weakGlobalTable[strModuleName] = M
    return M
end

function REQUIRE_MODULE(strModuleName)
    strModuleName = "luaScripts/extensions/" .. strModuleName .. ".lua"
    local M = require(strModuleName)

    _G[strModuleName] = M
    _weakGlobalTable[strModuleName] = M

    return M
end

function CLEAN_MODULE(strModuleName)
    local strModuleName = MODULE_NAME(strModuleName)
    _G[strModuleName] = nil
    package.loaded[strModuleName] = nil
end

-- -------------
-- 导入模块
-- -------------
function IMPORT_MODULE(strModuleName)
    local strModuleName = MODULE_NAME(strModuleName)
    return package.loaded[strModuleName]
end

-- -------------
-- 获取没有用的模块
-- -------------
function GET_UNUSE_MODULE()
    local unuseModule = {}
    for k, v in pairs(_weakGlobalTable) do
        if package.loaded[k] == nil and _G[k] == nil then
            unuseModule[#unuseModule + 1] = k
        end
    end
    return unuseModule
end

---
-- @function : 打印table的内容，递归
-- @param : tbl 要打印的table
-- @param : level 递归的层数，默认不用传值进来
-- @param : filteDefault 是否过滤打印构造函数，默认为是
-- @return : return
function PrintTable(tbl, level, filteDefault)
    local msg = ""
    filteDefault = filteDefault or true --默认过滤关键字（DeleteMe, _class_type）
    level = level or 1
    local indent_str = ""
    for i = 1, level do
        indent_str = indent_str .. "  "
    end

    LogError(indent_str .. "{")
    for k, v in pairs(tbl) do
        if filteDefault then
            if k ~= "_class_type" and k ~= "DeleteMe" then
                local item_str = string.format("%s%s = %s", indent_str .. " ", tostring(k), tostring(v))
                LogError(item_str)
                if type(v) == "table" then
                    PrintTable(v, level + 1)
                end
            end
        else
            local item_str = string.format("%s%s = %s", indent_str .. " ", tostring(k), tostring(v))
            LogError(item_str)
            if type(v) == "table" then
                PrintTable(v, level + 1)
            end
        end
    end
    LogError(indent_str .. "}")
end

-- 读取一个lua脚本
function LoadLuaFile(luaPath)
    if string.find(luaPath, ".lua") ~= nil then
        sluaAux.luaSvrManager.getInstance():loadLuaFile(luaPath)
    else
        sluaAux.luaSvrManager.getInstance():loadLuaFile(luaPath .. ".lua")
    end
end

function LogError(...)
    if g_logState == true then
        UnityEngine.Debug.LogError(...)
    end
end

function LogWarn(str)
    if g_logState == true then
        UnityEngine.Debug.LogWarning(str)
    end
end

function Log(str)
    if g_logState == true then
        UnityEngine.Debug.Log(str)
    end
end


local orignalPrint = _G["print"]

function print(str)
    if g_logState == true then
        orignalPrint(str)
    end
end

--- 给数据加逗号分格符(33333333=>33,333,333)
--- @param amount
function comma_value(amount)
    local formatted = amount .. ""
    local k;
    while true do
        formatted, k = string.gsub(formatted, "^(-?%d+)(%d%d%d)", '%1,%2')
        if (k == 0) then
            break
        end
    end
    return formatted
end

_G["print"] = print