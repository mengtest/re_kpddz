-- -----------------------------------------------------------------


-- *
-- * Filename:    MainCenterWinMono.lua
-- * Summary:     游戏主界面场景
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/13/2017 4:15:16 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainCenterWinMono")

local PlatformMg = IMPORT_MODULE("PlatformMgr");
local _itemMgr = IMPORT_MODULE("ItemMgr");

-- 界面名称
local wName = "MainCenterWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local btnBack

local _bgTexture
local _girl
local _nightGirl
local _dayGirl
local _girlTexture
local _redBagHint
local _redBagHintLb
local _dayEye
local _nightEffect
local _go;
local _effect_zhaocaijinniu
local _mask
local _glowwormContainer
local _bigTexture = {}

local _diamond
local _money
local _isChangeTopHeadPos = true
--- [ALD END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end






--- 主界面顶部所有元件位置调整
--- @param isShow 是否移动位置
local function ResetMainTopPosition(isShow)
    local disX = 90;
    --    isShow = isShow or true
    if isShow == false then
        disX = -90
    end

    if _isChangeTopHeadPos == isShow then
        return
    end
    _isChangeTopHeadPos = isShow
    local disPos = UnityEngine.Vector3(disX, 0, 0)
    
    
    
end

function OnClose(gameObj)
    --LogWarn("[MainCenterWinMono.OnCloseWin]" .. ".......退出游戏");
    UtilTools.ReturnToLoginScene()
    UnityTools.DestroyWin("MainWin");
    CloseWin()
end

local function OnToLoginWin(gameObject)
    if PlatformMg.GetOpenWinName() == "" then
        UnityTools.MessageDialog(LuaText.GetString("exit_game_message"), { okCall = OnClose })
    else
        UnityTools.DestroyWin(PlatformMg.GetOpenWinName())
        PlatformMg.SetOpenWinName("")
        --        UnityTools.CreateLuaWin("MainWin");
        triggerScriptEvent(EVENT_SHOW_MAIN_WIN, {})
    end
end

--- [ALF END]

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _go = gameObject;
    -- btnBack = UnityTools.FindGo(gameObject.transform, "Container/top/btnBack")
    -- UnityTools.AddOnClick(btnBack.gameObject, OnToLoginWin)


    _bgTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "bg")

    _girl = UnityTools.FindGo(gameObject.transform, "bg/Texture")

    _nightGirl = UnityTools.FindCo(gameObject.transform, "UITexture", "bg/nightGirl")

    _dayGirl = UnityTools.FindCo(gameObject.transform, "UITexture", "bg/dayGirl")

    _girlTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "bg/Texture")

    _nightEffect = UnityTools.FindCo(gameObject.transform, "Transform", "bg/nightEffect")
    _glowwormContainer = UnityTools.FindGo(gameObject.transform, "bg/nightEffect/glowwormContainer")

    --- [ALB END]
end


--- desc:白天黑夜场景切换
-- YQ.Qu:2017/3/14 0014
local function ChangeBgTexture()
    UtilTools.loadTexture(_bgTexture, "UI/Texture/night.png", true);
    local issetbgm=false
    if PlatformMg.MainMusic ~= "Sounds/BGM/mainNight" then 
        issetbgm=true
    end
    PlatformMg.MainMusic = "Sounds/BGM/mainNight";
    UtilTools.loadTexture(_girlTexture, "UI/Texture/girlNight_Alpha.png", false);
    UtilTools.loadTexture(_girlTexture, "UI/Texture/girlNight_RGB.png", true);
    if issetbgm then
        UnityTools.SetBGM(PlatformMg.MainMusic);
    end
end

function OnGuideMainCenterHandler(msgId, type, step)
    step = step or PlatformMg.GetGuideStep();
    local mainCityCtrl = IMPORT_MODULE("MainWinController")
    if type == "hideMain" then --隐藏主界面
        --        UnityTools.DestroyWin("MainWin");
        mainCityCtrl.Hide();
    elseif type == "showMain" then --显示主界面
        mainCityCtrl.Show();
        triggerScriptEvent(EVENT_SHOW_MAIN_WIN, nil)
    elseif type == "checkTouris" then --打开游客提示界面
        PlatformMg.Config:OpenStartWin();
    end
end

function HideOrShowGirlInMainWin(id,value)
    if value == true then
        _girl.transform.localPosition = UnityEngine.Vector3(-271,-20,0)
    else
        _girl.transform.localPosition = UnityEngine.Vector3(-10000,-20,0)
    end
    
end
local function Awake(gameObject)
    -- registerScriptEvent(EVENT_CHAGNE_TOP, "OnTopChangeHandler")
    registerScriptEvent(HIDE_OR_SHOW_GIRL, "HideOrShowGirlInMainWin")
    registerScriptEvent(GUIDE_STEP_UPDATE, "OnGuideMainCenterHandler")
    -- Lua Editor 自动绑定
    
    AutoLuaBind(gameObject)

end



local function ResetEffectRenderQ(go)
    UtilTools.SetEffectRenderQueueByUIParent(go.transform, _nightEffect.transform, 0);
    
end

local function Start(gameObject)
    ResetEffectRenderQ(gameObject)
    ChangeBgTexture();
    _isChangeTopHeadPos = PlatformMg.GetOpenWinName() == "RoomLvSelectWin"
end

local function OnDestroy(gameObject)
    unregisterScriptEvent(GUIDE_STEP_UPDATE, "OnGuideMainCenterHandler")
    unregisterScriptEvent(HIDE_OR_SHOW_GIRL, "HideOrShowGirlInMainWin")

    
    _girlTexture.mainTexture = nil;
    _girlTexture.alphaTexture = nil;
    _bgTexture.mainTexture = nil;
    CLEAN_MODULE(wName .. "Mono")
end
-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.ChangeHeadIcon = ChangeHeadIcon
M.ResetEffectRenderQ = ResetEffectRenderQ

-- 返回当前模块
return M
