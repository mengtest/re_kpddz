-- -----------------------------------------------------------------


-- *
-- * Filename:    ResultLayerMono.lua
-- * Summary:     结算界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/6/2017 2:45:19 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ResultLayerMono")



-- 界面名称
local wName = "ResultLayer"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winSp
local _bothWinSp
local _effParent
local _effect
--- [ALD END]




--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winSp = UnityTools.FindGo(gameObject.transform, "Container/sp/win")

    _bothWinSp = UnityTools.FindGo(gameObject.transform, "Container/sp/bothWin")

    _effParent = UnityTools.FindGo(gameObject.transform, "Container/niu")
    _effect = UnityTools.FindGo(gameObject.transform, "Container/lizi")
--- [ALB END]



end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    -- UnityTools.AddEffect(_effParent.transform,"effect_win",{loop = true, complete=
    -- function(go)
    --     UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,go.EffectGameObj.transform,1)
    --     go.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,127,0)
    --     go.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
    -- end})
    -- UnityTools.AddEffect(_effParent.transform,"effect_win_star",{loop = true, complete=
    -- function(go)
    --     UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,go.EffectGameObj.transform,1)
    --     go.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,147,0)
    --     go.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
    -- end})
end


local function Start(gameObject)
    if CTRL.IsBoth == true then
        _winSp:SetActive(false)
        _bothWinSp:SetActive(true)
    else
        _winSp:SetActive(true)
        _bothWinSp:SetActive(false)
    end
    _effect.gameObject:SetActive(true)
    UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,_effect.transform,-1)
    
    local timer = gTimer.registerOnceTimer(2000, CloseWin)
    gTimer.setRecycler(wName, timer)
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("ResultLayerMono")
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
