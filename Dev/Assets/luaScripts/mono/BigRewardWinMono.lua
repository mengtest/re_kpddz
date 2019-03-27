-- -----------------------------------------------------------------


-- *
-- * Filename:    BigRewardWinMono.lua
-- * Summary:     BigRewardWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        2/28/2017 3:29:12 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("BigRewardWinMono")



-- 界面名称
local wName = "BigRewardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

--local _lbNum
local _winBg
local _tex
local thisObj
local _running
local _running2
local _mask
local _type1
local _type2
local _type3
local _type0
local _effect
local _type4
local _spType1
local _spType2
--- [ALD END]




local _typeTable={
    type0={sp1="type0",width1=534,height1=162,sp2="word0",width2=372,height2=108,y2=108,isShowStar=true,fontSize = 37,wordY = 11,isFree = false,isShowCow=true},
    type1={sp1="type1",width1=376,height1=112,sp2="",isShowStar=false,fontSize=26,wordY = 11,isFree = false},
    type2={sp1="type2",width1=430,height1=158,sp2="word2",width2=202,height2=72,y2=72,isShowStar=false,fontSize = 32,wordY = 0,isFree = false},
    type3={sp1="type3",width1=538,height1=196,sp2="word3",width2=322,height2=88,y2=89,isShowStar=true,fontSize = 37,wordY = 0,isFree = false,isShowCow = true},
    type4={sp1="type2",width1=430,height1=158,isFree = true,isShowStar=true,fontSize = 32,wordY = 0},
}


local _freeTable={
    "free","free10","free15"
}


--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    --_lbNum = UnityTools.FindGo(gameObject.transform, "Container/Texture/num"):GetComponent("UILabel")
    _running = UnityTools.FindGo(gameObject.transform, "Container/type/Sprite/num"):GetComponent("LabelRunning")
    _running2 = UnityTools.FindGo(gameObject.transform, "Container/type/Sprite/num"):GetComponent("UILabel")
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")
    _mask = _winBg.gameObject:GetComponent("BoxCollider")
    _tex = UnityTools.FindGo(gameObject.transform, "Container/free"):GetComponent("UITexture")
    local sp1 = UnityTools.FindGo(gameObject.transform, "Container/type/Sprite"):GetComponent("UISprite")
    local sp2 = UnityTools.FindGo(gameObject.transform, "Container/type/sp2/word"):GetComponent("UISprite")
    local sp2Pos =  UnityTools.FindGo(gameObject.transform, "Container/type/sp2")
    local freeObj = UnityTools.FindGo(gameObject.transform, "Container/type/free")
    local cow = UnityTools.FindGo(gameObject.transform, "Container/type/cow")
    _effect = UnityTools.FindGo(gameObject.transform, "Container/shuiguo")
    -- _type0 = UnityTools.FindGo(gameObject.transform, "Container/type0")
    -- _type1 = UnityTools.FindGo(gameObject.transform, "Container/type1")
    -- _type2 = UnityTools.FindGo(gameObject.transform, "Container/type2")
    -- _type3 = UnityTools.FindGo(gameObject.transform, "Container/type3")
    
    -- _type4 = UnityTools.FindGo(gameObject.transform, "Container/type4")
    _spType1 = UnityTools.FindGo(gameObject.transform, "Container/hun/type1"):GetComponent("UISprite")

    _spType2 = UnityTools.FindGo(gameObject.transform, "Container/hun/type2"):GetComponent("UISprite")

--- [ALB END]

    if CTRL.PoolPos <=0 then
        _spType1.gameObject:SetActive(false)
        _spType2.gameObject:SetActive(false)
    else
        if CTRL.PoolPos == 5 then
            _spType1.spriteName = "zhuang"
            _spType2.spriteName = "zhuang"
        else
            _spType1.spriteName = "type_" .. CTRL.PoolPos .. "_1"
            _spType2.spriteName = "type_" .. CTRL.PoolPos .. "_1"
        end
    end

    if CTRL.Free>0 and CTRL.Free<=3 then
        UnityTools.FindGo(gameObject.transform, "Container/type").gameObject:SetActive(false)
        UtilTools.loadTexture(_tex,"UI/Texture/Fruit/".._freeTable[CTRL.Free]..".png",true)
        -- UtilTools.loadTexture(_tex,"UI/Texture/Fruit/".._freeTable[CTRL.Free].."_Alpha.png",false)
    else
        local data = _typeTable["type"..CTRL.Type]
        if data ~=nil then
            sp1.spriteName=data.sp1
            sp1.width = data.width1
            sp1.height = data.height1
            if data.isFree == true then
                sp2Pos.gameObject:SetActive(false)
                freeObj.gameObject:SetActive(true)
            else
                freeObj.gameObject:SetActive(false)
                sp2Pos.gameObject:SetActive(true)
                sp2.spriteName = data.sp2
                if data.sp2 ~= "" then
                    sp2.width = data.width2
                    sp2.height = data.height2
                    LogError(data.y2)
                    sp2Pos.transform.localPosition = UnityEngine.Vector3(0,data.y2,0)
                end
                _running.transform.localPosition = UnityEngine.Vector3(0,data.wordY,0)
                _running2.fontSize = data.fontSize
            end
            if data.isShowCow == true then
                cow.gameObject:SetActive(true)
            else
                cow.gameObject:SetActive(false)
            end 
        end
    end
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    thisObj=gameObject
    AutoLuaBind(gameObject)

end
function PlayRewardSoundEffect()
    UnityTools.PlaySound(CTRL.Sound2,{target=_winBg.gameObject})
end
local function openAction(winBg, scale, time) 
    scale = scale or 0.1
    time = time or 500
    winBg.transform.localScale = UnityEngine.Vector3(1, 1, 1)
    local hash = iTween.Hash("time", time / 1000, "scale", UnityEngine.Vector3(scale, scale, 1.0), "easetype", "easeOutElastic")
    iTween.ScaleFrom(winBg, hash)
end
function BigRewardShow()
    -- _tex.transform.localPosition=UnityEngine.Vector3(0,0,0)
    -- if CTRL.Free == 0 then
    --     _running.gameObject:SetActive(true)
    -- end
    if CTRL.Sound2 ~= "" then
        LogError("CTRL.Sound2="..CTRL.Sound2)
        -- PlayRewardSoundEffect()
        gTimer.registerOnceTimer(100,"PlayRewardSoundEffect")
    end
    -- openAction(_winBg)
    if CTRL.Sound ~= "" then
        UnityTools.PlaySound(CTRL.Sound,{target=thisObj.gameObject})
        -- UnityTools.PlaySound(CTRL.Sound)
    end
    _running:SetValue(CTRL.LabelValue,false)
    -- _running.text = tostring(CTRL.LabelValue)
end
local function OnLoadComplete(gameObject)
    UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform,gameObject.EffectGameObj.transform,-1)
    --UtilTools.SetEffectRenderQueueByUIParent(thisObj.transform,gameObject.EffectGameObj.transform:Find("glow_00176/guangmu2"),0)
    gameObject.EffectGameObj.transform.localPosition=UnityEngine.Vector3(0,0,0)
    gameObject.EffectGameObj.transform.localScale=UnityEngine.Vector3(1,1,1)
    
end
local function OnEffectDestroy(gameObject)
    CloseWin(nil)
end
function BigRewardClose()
    CloseWin(nil)
end
function BigRewardStart()
    _effect.gameObject:SetActive(true)
    -- local effect_icon = UnityTools.AddEffect(thisObj.transform,"effect_zhongdajiang",{loop = false,complete=OnLoadComplete,remove=OnEffectDestroy})
    BigRewardShow()
    --gTimer.registerOnceTimer(200,"BigRewardShow")
end
local function Start(gameObject)
    _mask.enabled=CTRL.bMask
    -- _tex.transform.localPosition=UnityEngine.Vector3(30000,0,0)
    UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,_effect.gameObject.transform,-1)
    
    if CTRL.bDelay then 
        gTimer.registerOnceTimer(1000,"BigRewardStart")
    else
        _effect.gameObject:SetActive(true)
        BigRewardStart()
    end
    if CTRL.StillTime ~= 0 then
        gTimer.registerOnceTimer(CTRL.StillTime,"BigRewardClose")
    end
end


local function OnDestroy(gameObject)
    CLEAN_MODULE(wName .. "Mono")
end





-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy



-- 返回当前模块
return M
