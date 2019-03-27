-- -----------------------------------------------------------------


-- *
-- * Filename:    PokerMgr.lua
-- * Summary:     扑克管理器
-- *              解析牌、发牌、判定、计算
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-2-7 10:09:40
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PokerMgr")

local UnityTools = IMPORT_MODULE("UnityTools")

local _myPokerBag = PokerBase.PokerBag()
local _pokerBagList = {}

-- 比率
local _poker_type_ratio = {[0] = 1,1,1,1,1,1,1,2,2,2,3,4,5,8}

-- 计算器的名字、计算器
local _calName
local _calculator

-- 翻牌速度，毫秒
local _action_poker_time = 150



-- 清空背包列表
local function cleanBagList()
    for k, v in pairs(_pokerBagList) do 
        v:CleanPokers()
        _pokerBagList[k] = nil
    end
    _pokerBagList = {}
end

-- 清空计算器
local function cleanCalc()
    if _calName ~= nil then
        CLEAN_MODULE(_calName)
        _calName = nil
        _calculator = nil
    end
end

local function cleanMgr()
    cleanBagList()
    gTimer.recycling("PokerMgr")
end

-- 设置计算器
local function setCalculator(calName) 
    cleanCalc()
    _calName = calName
    LoadLuaFile("poker/calculator/" .. _calName)
    _calculator = IMPORT_MODULE(_calName)
end

local function getPokerBag(index)
    local pokerBag = nil
    if _pokerBagList[index] == nil then
       _pokerBagList[index] = PokerBase.PokerBag()
    end
    pokerBag = _pokerBagList[index]
    return pokerBag
end

-- 添加一张牌
local function addPokerToBag(index, poker, clean)
    local pokerBag = getPokerBag(index)
    if clean ~= nil then
        pokerBag:CleanPokers()
    end
    pokerBag:AddOwnPoker(5 - poker.color, poker.number)
end

-- 添加一套牌
local function addPokersToBag(index, pokers)
    for i = 1, pokers.Count, 1 do
        addPokerToBag(index, pokers[i])
    end
end



-- 初始化背包
local function initPokerBag(index)
    local pokerBag = getPokerBag(index)
    pokerBag:CleanPokers()
end

-- 收到服务器牌组信息
local function recvPokersFromServer(index, pokerList)
    initPokerBag(index)
    addPokersToBag(index, pokerList)
end

-- 返回 类型，最大牌
local function calculatePokers(pokers) 
    return _calculator.CalculateValue(pokers)
end

-- 筛选最优牌组
local function selectPokers(pokers)
    return _calculator.SelectBest(pokers)
end

-- 对牌型进行排列
local function arrangementPokers(pokers)
    return _calculator.ArrangementPokers(pokers)
end

-- 对所有组合进行排列
local function arrangementCombination(comb)
    local allArr = {}
    allArr.Count = 0
    for i = 0,comb.Count - 1, 1 do
        local arr = arrangementPokers(comb[i])
        for j = 0,arr.Count - 1, 1 do
            allArr[allArr.Count] = arr[j]
            allArr.Count = allArr.Count + 1
        end
    end
    return allArr
end

local function getBestSelect(pokerList)
    local allArrange = arrangementCombination(pokerList)
    local bestPokers, hType, hPoker = selectPokers(allArrange)
    return bestPokers, hType, hPoker
end

local function getPokerObjectTable(pokerObj)
    local pokerTb = {}
    for index = 1, 5, 1 do
        local p = UnityTools.FindGo(pokerObj.transform, "poker_img" .. index)
        local act = p:GetComponent("FastMove")
        local forward = UnityTools.FindGo(p.transform, "forward")
        local back = UnityTools.FindGo(p.transform, "back")
        local num = UnityTools.FindCo(forward.transform, "UISprite", "num")
        local sType = UnityTools.FindCo(forward.transform, "UISprite", "sType")
        local bType = UnityTools.FindCo(forward.transform, "UISprite", "bType")
        pokerTb[index] = {o = p, f = forward, b = back, n = num, s = sType, t = bType, tp = act}
    end
    local pType = UnityTools.FindGo(pokerObj.transform, "pokerType")
    local typeAct = nil  
    local typeSp = nil
    if pType ~= nil then
        typeAct = pType:GetComponent("TweenScale")
        typeSp = pType:GetComponent("UISprite")
    end
    local tipObj = UnityTools.FindGo(pokerObj.transform, "tip")
    local tipLb = nil
    if tipObj ~= nil then
        tipLb = tipObj:GetComponent("UILabel")
    end
    pokerTb.pts = typeAct
    pokerTb.ptsp = typeSp
    pokerTb.tip = tipLb
    pokerTb.parent = pokerObj
    return pokerTb
end

local function getPokersTable(pokerTableList, pokerList) 
    for k, poker in pairs(pokerList) do
        pokerTableList[k] = getPokerObjectTable(poker)
    end
    return pokerTableList
end

local function setPokerIconAltas(pokerObj, poker)
    
    local forward = pokerObj.f
    local back = pokerObj.b
    if poker == nil then
        UnityTools.SetActive(back, true)
        UnityTools.SetActive(forward, false)
    else
        UnityTools.SetActive(back, false)
        UnityTools.SetActive(forward, true)
        local num = pokerObj.n
        local type = 5 - tonumber(poker.color)
        if type == PokerBase.ePOKER_TYPE.spade or type == PokerBase.ePOKER_TYPE.club then
            num.spriteName = "b" .. poker.number
        else
            num.spriteName = "r" .. poker.number
        end
        local sType = pokerObj.s
        local bType = pokerObj.t
        sType.spriteName = "icon1" .. poker.color
        bType.spriteName = "icon2" .. poker.color
    end
end

local function actionPoker(pokerObj, poker, delay)
    local gameObject = pokerObj.o
    local time = _action_poker_time
    delay = delay or 0
    local hash = iTween.Hash("time", time / 1000, "y", 90, "delay", delay / 1000, "luaeasetype", iTween.EaseType.easeOutBack)
    iTween.RotateTo(gameObject, hash)
    local timer = gTimer.registerOnceTimer(time + delay, function () 
        if _calName == nil then return nil end
        setPokerIconAltas(gameObject, poker) 
        local time = _action_poker_time
        local hash = iTween.Hash("time", time / 1000, "y", 0, "luaeasetype", iTween.EaseType.easeOutBack)
        iTween.RotateTo(gameObject, hash)
    end)
    gTimer.setRecycler("PokerMgr", timer)
end

-- 设置扑克牌资源
local function setPokerIcon(gameObject, poker, action, delay)
    action = nil
    if action ~= nil then
        actionPoker(gameObject, poker, delay)
    else 
        if delay ~= nil and delay > 0 then
            local timer = gTimer.registerOnceTimer(delay, function () 
                setPokerIconAltas(gameObject, poker)
            end)
            gTimer.setRecycler("PokerMgr", timer)
        else
            setPokerIconAltas(gameObject, poker)
        end
    end
end

-------------------------事件监听
-- 收到牌组更新消息
function PokerMgrRecvRoomUpdatePoker(eID, index, pokerList)
    recvPokersFromServer(index, pokerList)
end

-- 收到牌组添加消息
function PokerMgrRecvRoomAddPoker(eID, index, pokerList)
    pokerList.Count =  #pokerList
    addPokersToBag(index, pokerList)
end

-- 收到删除牌组信息
function PokerMgrRecvRoomDelPoker(eID, index)
    initPokerBag(index)
end

local function getPokerValue(number)
	if number >= 10 then
		return 10
	end
	return number
end
--炸弹
local function isBomb(tPokers)
	local tNumCount = {}
	local nCount = 0
	for i,v in pairs(tPokers) do
		nCount = tNumCount[v.number] or 0
		tNumCount[v.number] = nCount + 1
		if nCount + 1 >= 4 then
			return true
		end
	end

	return false
end

--五花牛
local function isWuHuaNiu(tPokers)
	for i = 1,#tPokers do
		if tPokers[i].number <= 10 then
			return false
		end
	end

	return true
end

--五小牛
local function isWuXiaoNiu(tPokers)
	local allScore = 0
	for i = 1,#tPokers do
		allScore = allScore + getPokerValue(tPokers[i].number)
	end
	if allScore < 10 then
		return true
	end
end
local function isSpecialPokerCard(tMyPokers)
	if isWuXiaoNiu(tMyPokers) then
		return 13
	elseif isWuHuaNiu(tMyPokers) then
		return 12
	elseif isBomb(tMyPokers) then
		return 11
	end

	return 0 
end
--给5张牌排序, 前三张相加是10的倍数, 组成最大的牛牛
local function SortMaxNiuNiu(tPokers)
	--匹配最大牛牛
	if type(tPokers) ~= "table" or #tPokers ~= 5 then
		return {}
	end
	if isSpecialPokerCard(tPokers) ~= 0 then
		return tPokers,true
	else
		local newPokers = {}
		local tmpThreePokerValue = 0--三张牌的和（单纯相加）
		local tmpThreeNiuValue--三张牌的和（牛牛方式计算）
		local tmpThreePoker --用个十百中位表示3张
		local tmpNiuValue
		local maxThreePokerValue = 0
		local maxThreePoker
		local maxNiuNiuValue = 0
		for i = 1, 5 do
			for j = i+1, 5 do
				tmpThreePokerValue = 0 --三张牌的和（单纯相加）
				tmpThreeNiuValue = 0 --三张牌的和（牛牛方式计算）
				tmpThreePoker = 0--用个十百中位表示3张
				for ind = 1, 5 do
					if ind ~= i and ind ~= j then
						tmpThreePokerValue = tmpThreePokerValue + tPokers[ind].number
						tmpThreeNiuValue = tmpThreeNiuValue + getPokerValue(tPokers[ind].number)
						tmpThreePoker = tmpThreePoker*10 + ind
					end
				end
				if tmpThreeNiuValue%10 == 0 then--有牛
					tmpNiuValue = getPokerValue(tPokers[i].number) + getPokerValue(tPokers[j].number)
					tmpNiuValue = tmpNiuValue%10 --牛几
					if tmpNiuValue == 0 then
						tmpNiuValue = 10
					end
					if tmpNiuValue > maxNiuNiuValue then
						maxNiuNiuValue = tmpNiuValue
						maxThreePokerValue = tmpThreePokerValue
						maxThreePoker = tmpThreePoker
					elseif tmpNiuValue == maxNiuNiuValue and tmpThreePokerValue >= maxThreePokerValue then
						maxNiuNiuValue = tmpNiuValue
						maxThreePokerValue = tmpThreePokerValue
						maxThreePoker = tmpThreePoker
					end
				end
			end
		end
		if maxNiuNiuValue > 0 then
			-- LogError("牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛牛：" .. maxNiuNiuValue .. ", list:" .. maxThreePoker)
			local ind1 = math.floor(maxThreePoker/100)
			local ind2 = math.floor((maxThreePoker%100)/10)
			local ind3 = maxThreePoker%10
			newPokers[1] = tPokers[ind1]
			newPokers[2] = tPokers[ind2]
			newPokers[3] = tPokers[ind3]
			for k=1, 5 do
				if k ~= ind1 and  k ~= ind2 and k ~= ind3 then
					table.insert(newPokers, tPokers[k])
				end
			end
			return newPokers,false
		else
			return tPokers,true
		end
	end
end


registerScriptEvent(EVENT_ROOM_RECV_UPDATE_POKER, "PokerMgrRecvRoomUpdatePoker")
registerScriptEvent(EVENT_ROOM_RECV_ADD_POKER, "PokerMgrRecvRoomAddPoker")
registerScriptEvent(EVENT_ROOM_RECV_DEL_POKER, "PokerMgrRecvRoomDelPoker")

M.GetPokersTable = getPokersTable
M.GetPokerObjectTable = getPokerObjectTable
M.RecvPokersFromServer = recvPokersFromServer
M.SetPokerIcon = setPokerIcon
M.AddPokersToBag = addPokersToBag
M.AddPokerToBag = addPokerToBag
M.InitPokerBag = initPokerBag
M.GetPokerBag = getPokerBag
M.CleanCalculator = cleanCalc
M.MyPokerBag = _myPokerBag
M.SetCalculator = setCalculator
M.CalculatePokers = calculatePokers
M.SelectPokers = selectPokers
M.GetBestSelect = getBestSelect
M.ArrangementPokers = arrangementPokers
M.ArrangementCombination = arrangementCombination
M.CleanBagList = cleanBagList
M.PokerTypeRatio = _poker_type_ratio
M.CleanMgr = cleanMgr
M.SortMaxNiuNiu = SortMaxNiuNiu
return M