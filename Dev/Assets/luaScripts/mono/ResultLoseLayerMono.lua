-- -----------------------------------------------------------------


-- *
-- * Filename:    ResultLoseLayerMono.lua
-- * Summary:     失败结算界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/8/2017 1:32:36 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ResultLoseLayerMono")



-- 界面名称
local wName = "ResultLoseLayer"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _loseSp
local _bothLoseSp
local _effParent
--- [ALD END]



--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _loseSp = UnityTools.FindGo(gameObject.transform, "Container/sp/lose")

    _bothLoseSp = UnityTools.FindGo(gameObject.transform, "Container/sp/bothLose")

    _effParent = UnityTools.FindGo(gameObject.transform, "Container/niu")
--- [ALB END]


end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    -- UnityTools.AddEffect(_effParent.transform,"effect_lose",{loop = true, complete=
    -- function(go)
    --     UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,go.EffectGameObj.transform,4)
    --     go.EffectGameObj.transform.localPosition=UnityEngine.Vector3(-16,9,0)
    --     go.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
    -- end})
    -- UnityTools.AddEffect(_effParent.transform,"effect_lose_circle",{loop = true, complete=
    -- function(go)
    --     UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,go.EffectGameObj.transform,1)
    --     go.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,147,0)
    --     go.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
    -- end})
end


local function Start(gameObject)
    if CTRL.IsBoth == true then
        _loseSp:SetActive(false)
        _bothLoseSp:SetActive(true)
    else
        _loseSp:SetActive(true)
        _bothLoseSp:SetActive(false)
    end
    local timer = gTimer.registerOnceTimer(2000, CloseWin)
    gTimer.setRecycler(wName, timer)
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("ResultLoseLayerMono")
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
