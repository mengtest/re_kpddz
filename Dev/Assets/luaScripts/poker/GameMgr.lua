-- -----------------------------------------------------------------


-- *
-- * Filename:    GameMgr.lua
-- * Summary:     游戏管理器
-- *              各游戏模式管理器、控制lua资源加载
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-2-8 12:06:50
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GameMgr")

local UnityTools = IMPORT_MODULE("UnityTools")
local roomMgr = IMPORT_MODULE("roomMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")

local _currentMgr = nil
local _currentMgrName = nil
local _currentIndex = 0

local _succFunCall = nil
-- 需要加载的游戏模块
local _loadedGameMgrs = {
    [1] = "normalCowMgr",
    [2] = "hundredCowMgr",
    [3] = "fruit",
}

local _gameBGM = {
    [1] = "Sounds/BGM/pokebgm",
    [2] = "Sounds/BGM/hundredbgm",
}

-- 加载游戏
local function loadingGame(gameName)
    LoadLuaFile("poker/game/" .. gameName)
    if _currentMgr == nil then
        _currentMgr = IMPORT_MODULE(gameName)
        _currentMgrName = gameName
        _currentMgr.InitMgr()
    end
end

-- 卸载游戏
local function unloadGame()
    if _currentMgr ~= nil then 
        _currentMgr.CleanMgr()
    end
    CLEAN_MODULE(_currentMgrName)
    _currentMgr = nil
    _currentMgrName = nil
    _succFunCall = nil
end

local function CallSucc() 
    if _succFunCall ~= nil then 
        _succFunCall() 
        roomMgr.bExiting =false
        _succFunCall = nil
        UtilTools.HideWaitFlag()
        gTimer.registerOnceTimer(500, function() 
            UnityTools.CallLoadingWin(false)
            UnityTools.Collect()
        end)
    end
end

-- 对外加载游戏
local function setupGame(index)
    if _loadedGameMgrs[index] ~= nil then
        _currentIndex = index
        loadingGame(_loadedGameMgrs[index])
    end
    gTimer.removeTimer(UnityTools.SetBGM)
    gTimer.registerOnceTimer(350, UnityTools.SetBGM, _gameBGM[index])
end
M.TipIndex = 0
M.EndTime =0
M.ToIndex = 0
-- 进入游戏
local function enterGame(index, roomID, sucCall, failCall)
    -- roomID = 10
    
    if roomMgr.bExiting == true then
        return
    end
    LogError("roomMgr.bExiting==="..tostring(roomMgr.bExiting))
    roomMgr.bExiting = true
    
    UnityTools.RecordOpenTime(false)
    -- UtilTools.CanTouchButton = false
    local enterCall = function () 
        if roomID == 10 then
            M.TipIndex = 13  
        end
        if UtilTools.GetServerTime() >= M.EndTime and M.EndTime~= 0 then
            roomMgr.ExitMgr()
            if failCall ~= nil then
                failCall()
            end
            roomMgr.bExiting = false
            return
        end
        UnityTools.CallLoadingWin(true)
        _succFunCall = sucCall
        setupGame(_currentIndex)
        _currentMgr.Start()
    end
    local exitCall = function ()
        roomMgr.ExitMgr()
        if failCall ~= nil then
            failCall()
        end
        roomMgr.bExiting = false
        -- UtilTools.CanTouchButton = true
    end
    roomMgr.InitMgr(index, roomID, function (result, gameType,coolTime,toIndex,EndTime) 
        M.EndTime = EndTime
        M.ToIndex = toIndex
        _currentIndex = gameType
        if result > 0 then
            LogError("coolTime="..coolTime)
            if coolTime <=0 then
                coolTime =0
                M.EndTime = 0
            end
            UnityTools.MessageDialog(LuaText.room_back_to_enter_tip, {okCall = enterCall, cancelCall = exitCall,closeSecond = coolTime})
        else
            enterCall()
        end
    end)
end

local function exitGame()
    roomMgr.ExitMgr()
    unloadGame()
    UnityTools.SetBGM(platformMgr.MainMusic);
    gTimer.registerOnceTimer(300, UnityTools.Collect)
    UnityTools.CallLoadingWin(false)
    roomMgr.bExiting = false
    -- gTimer.registerOnceTimer(1000, UnityTools.CallLoadingWin, false)
end

-- 初始化游戏模块(在main函数的luaLoaded中调用,otherwork之后)
local function initGameMgr()
    --setupGame(1)
end

-- local function GetLoadedMgrs()
--     return _loadedGameMgrs
-- end

_g_loading_tips = {
    "牛九 > 牛八 > 牛七 > 牛六 > 牛五 > 牛四 > 牛三 > 牛二 > 牛一",
    "五小牛 > 五花牛> 四炸 > 牛牛 > 有牛 > 没牛",
    "K > Q > J > 10 > 9 > 8 > 7 > 6 > 5 > 4 > 3 > 2 > A",
    "牌型数字相同时最大一张牌根据花色：黑桃♠ > 红桃♥ > 梅花♣ > 方块♦ 比较大小",
    "3张手牌（JQK按10计算）相加为10的倍数（如10、20），即为有牛，否则为没牛",
    "3张牌组成牛后再看剩下2张牌相加的个位数，该数字越大则牌型越大",
    "没牛：任意3张牌相加不为10的倍数",
    "有牛：任意3张牌相加为10的倍数",
    "牛牛：任意3张牌相加为10的倍数，另外2张牌相加为10的倍数。",
    "四炸：5张牌中有4张牌数字完全一样。",
    "五花牛：5张牌为花牌( JQK )",
    "五小牛：5张牌均小于5，且相加之和不大于10",
    "千人抢红包：连续游戏5局，就可100%抽取到一个红包。",
    "千人抢红包：连续游戏5局，必得0.2元、0.5元、1.0元红包之一。",
    -- "当钻石低于30颗时，进入“千人抢红包”系统将免费补足至30颗，每天一次。"
}


-- 审核版本屏蔽内容
-- if version.VersionData.IsReviewingVersion() then

-- _g_loading_tips = {
--     "牛九 > 牛八 > 牛七 > 牛六 > 牛五 > 牛四 > 牛三 > 牛二 > 牛一",
--     "五小牛 > 五花牛> 四炸 > 牛牛 > 有牛 > 没牛",
--     "K > Q > J > 10 > 9 > 8 > 7 > 6 > 5 > 4 > 3 > 2 > A",
--     "牌型数字相同时最大一张牌根据花色：黑桃♠ > 红桃♥ > 梅花♣ > 方块♦ 比较大小",
--     "3张手牌（JQK按10计算）相加为10的倍数（如10、20），即为有牛，否则为没牛",
--     "3张牌组成牛后再看剩下2张牌相加的个位数，该数字越大则牌型越大",
--     "没牛：任意3张牌相加不为10的倍数",
--     "有牛：任意3张牌相加为10的倍数",
--     "牛牛：任意3张牌相加为10的倍数，另外2张牌相加为10的倍数。",
--     "四炸：5张牌中有4张牌数字完全一样。",
--     "五花牛：5张牌为花牌( JQK )",
--     "五小牛：5张牌均小于5，且相加之和不大于10",
-- }

-- end


local function OnLoadingWinShow(gameObject)
    gTimer.registerOnceTimer(100, function(go) 
        local tip = go.transform:GetComponent("UILabel")
        if M.TipIndex == 0 and M.TipIndex <= #_g_loading_tips then
            tip.text = _g_loading_tips[math.random(1, #_g_loading_tips)]
        else
            tip.text = _g_loading_tips[M.TipIndex]
            M.TipIndex = 0
        end
    end, gameObject)
    -- tip.gameObject:SetActive(false)
    -- tip.gameObject:SetActive(true) 
    -- gTimer.registerOnceTimer(10, function(t) 
    --     t.gameObject:SetActive(true)
    -- end, tip)
end

UI.Controller.UIManager.RegisterLuaFuncCall("RandomTips:OnEnable", OnLoadingWinShow)

M.EnterGame = enterGame
M.InitGameMgr = initGameMgr
M.GetLoadedMgrs = _loadedGameMgrs
M.UnloadGame = unloadGame
M.ExitGame = exitGame
M.CallSucc = CallSucc
return M