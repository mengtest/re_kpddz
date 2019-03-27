-- -----------------------------------------------------------------


-- *
-- * Filename:    cowCalculator.lua
-- * Summary:     牛牛计算器
-- *              
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-2-9 10:41:59
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("cowCalculator")

local UnityTools = IMPORT_MODULE("UnityTools")

local _minCalCnt = 3    -- 最小计牌数
local _maxCalCnt = 5    -- 最大计牌数
local _littleMax = 11   -- 11表示五小牛小于等于10，10表示五小牛小于10
local _littleType, _flowerType, _boomType = 13, 12, 11    -- 五小牛>五花牛>炸弹牛>牛牛(10)
local function getNumVal(poker)
    if poker.Num >= 10 then
        return 10
    else 
        return poker.Num
    end
end

-- 计算牌的价值
-- return type, maxPoker
-- type:高牌0，牛一1~牛牛10，炸弹11，花牛12，小牛13
local function calValue(pokers, exceptNormal)
    local pokerCnt = pokers.Count
    -- 数量不符
    if pokerCnt < _maxCalCnt and pokerCnt ~= _minCalCnt then
        return -1
    end
    -- 三张直接计算结果
    local typeValue = getNumVal(pokers[0]) + getNumVal(pokers[1]) + getNumVal(pokers[2])
    --print(typeValue%10)
    if pokerCnt == _minCalCnt then
        return typeValue
    end

    -- 去除高牌剩余计算
    if exceptNormal ~= nil and typeValue > (_littleMax - 3) and typeValue % 10 ~= 0 and (pokers[0].Num ~= pokers[1].Num and pokers[0].Num ~= pokers[2].Num and pokers[1].Num ~= pokers[2].Num) then
        return 0
    end
    
    local highestPoker = nil
    local judgeBoom = {}
    local boomIndex = 0
    local judgeFlower = true
    local judgeLittle = 0
    for i = 0, _maxCalCnt - 1, 1 do
        
        local poker = pokers[i]
        judgeLittle = judgeLittle + poker.Num
        if poker.Num < _littleMax then 
            judgeFlower = false
        end
        if judgeBoom[tostring(poker.Num)] == nil then
            judgeBoom[tostring(poker.Num)] = 0
            boomIndex = boomIndex + 1
        end
        judgeBoom[tostring(poker.Num)] = judgeBoom[tostring(poker.Num)] + 1
        if highestPoker == nil then
            highestPoker = poker
        else 
            if highestPoker.Num < poker.Num then
                highestPoker = poker
            else 
                if highestPoker.Num == poker.Num and tonumber(highestPoker.Type) > tonumber(poker.Type) then
                    highestPoker = poker
                end
            end
        end
    end

    if judgeLittle <= 10 and highestPoker.Num < 5 then
        return _littleType, highestPoker
    elseif judgeFlower == true then
        return _flowerType, highestPoker
    elseif boomIndex == 2 then
        return _boomType, highestPoker
    elseif typeValue % 10 == 0 then
        local val = getNumVal(pokers[3]) + getNumVal(pokers[4])
        if val % 10 == 0 then
            return 10, highestPoker
        elseif val > 10 then
            return val-10, highestPoker
        else
            return val, highestPoker
        end
        
    else 
        
        return 0, highestPoker
    end
end

local function exchangeTable(tb, index1, index2)
    local temp = tb[index2]
    tb[index2] = tb[index1]
    tb[index1] = temp
end

-- 比较扑克牌大小，poker1 > poker2 = 1,poker1 == poker2 = 0, poker1 < poker2 = -1
local function comparePoker(poker1, poker2)
    if poker1.Num > poker2.Num then
        return 1
    elseif poker1.Num == poker2.Num then
        if poker1.Type > poker2.Type then
            return -1
        elseif poker1.Type == poker2.Type then
            return 0
        else
            return 1
        end
    else
        return -1
    end
end

local function arrangementPokers(pokers)
    local allArrangement = {}
    allArrangement.Count = 0
    for i = 0, 2, 1 do 
        for j = 3, 4, 1 do 
            local arr = {[0] = pokers[0], pokers[1], pokers[2], pokers[3], pokers[4]}
            arr.Count = 5
            exchangeTable(arr, i, j)
            allArrangement[allArrangement.Count] = arr
            allArrangement.Count = allArrangement.Count + 1
        end
    end
    for k = 3, 4, 1 do 
        local arr = {[0] = pokers[3], pokers[4], pokers[0], pokers[1], pokers[2]}
        arr.Count = 5
        exchangeTable(arr, 2, k)
        allArrangement[allArrangement.Count] = arr
        allArrangement.Count = allArrangement.Count + 1
    end
    local arr1 = {[0] = pokers[0], pokers[1], pokers[2], pokers[3], pokers[4]}
    arr1.Count = 5
    local arr2 = {[0] = pokers[3], pokers[4], pokers[0], pokers[1], pokers[2]}
    arr2.Count = 5
    allArrangement[allArrangement.Count] = arr1
    allArrangement[allArrangement.Count + 1] = arr2
    allArrangement.Count = allArrangement.Count + 2
    return allArrangement
end

-- 计算牌库中最大的牌型
local function selectBest(combination)
    local combCnt = combination.Count
    local combPer = {}
    local hType, hPoker = 0, nil
    local hIndex = 0
    local fastCall = nil
    for i = 0, combCnt - 1, 1 do
        local type, poker = calValue(combination[i], fastCall)
        if combPer[type] == nil then
            combPer[type] = 0
        end
        combPer[type] = combPer[type] + 1

        if hPoker == nil then
            hIndex, hType, hPoker = i, type, poker
        else
            if type > hType then    
                hIndex, hType, hPoker = i, type, poker
            elseif type == hType and comparePoker(poker, hPoker) == 1 then
                hIndex, hType, hPoker = i, type, poker
            end
        end
        if hType > 0 then
            fastCall = 1
        end
    end
    for k, v in pairs(combPer) do
        if combPer[k] ~= nil then
            print(k .. " ->-> " .. combPer[k] / combCnt * 100 .. "%(" .. combPer[k] .. ")")
        end
    end
    return combination[hIndex], hType, hPoker
end

-- 计算牌面价值
M.CalculateValue = calValue
M.SelectBest = selectBest
M.ArrangementPokers = arrangementPokers
return M