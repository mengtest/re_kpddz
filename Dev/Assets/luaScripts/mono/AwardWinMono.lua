-- -----------------------------------------------------------------


-- *
-- * Filename:    AwardWinMono.lua
-- * Summary:     奖励通用界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/4/2017 10:42:16 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("AwardWinMono")



-- 界面名称
local wName = "AwardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _mack
local _scrollView
local _grid
local _countIndex = 1;
local _cell
local _btnSure
local _dragBg
local _dragBgBoxCollider
local _titleBgTween
local _go;
local _titleBg;
local _itemList = {}
local _awardData
local _btnShare
local _sharePic
--- [ALD END]

local function closeByAction(winBg, name, scale, time)
    scale = scale or 0.1
    time = time or 300
    if winBg == nil then
        UnityTools.DestroyWin(name)
        return
    end
    winBg.transform.localScale = UnityEngine.Vector3(scale, scale, 1)
    _titleBg.transform.localScale = UnityEngine.Vector3(scale, scale, 1)
    local hash = iTween.Hash("time", time / 1000, "scale", UnityEngine.Vector3(1, 1, 1), "luaEasetype", iTween.EaseType.easeInBack)
    iTween.ScaleFrom(winBg, hash)
    local ahash = iTween.Hash("time", (time / 1000), "scale", UnityEngine.Vector3(1, 1, 1), "luaEasetype", iTween.EaseType.easeInBack)
    iTween.ScaleFrom(_titleBg, ahash);
    gTimer.registerOnceTimer(time, function()
        UnityTools.DestroyWin(name)
    end)
end

local function CloseWin(gameObject)
    if CTRL.GetShowDataLen()>0 then
        UnityTools.DestroyWin(wName)
        return
    end
    closeByAction(_winBg, wName, 0.01, 500);
end

local function SetItemShow(item, info)
    local data = LuaConfigMgr.ItemBaseConfig[info.base_id .. ""]
    if data == nil then
        LogWarn("[AwardWinMono.SetItemShow]没找到物品：" .. info.base_id);
        return;
    end
    local lb = UnityTools.FindCo(item.transform, "UILabel", "Label");

    if lb ~= nil then
        local isInteger = (info.count + 0) % 10000 == 0;
        if info.base_id ~= 109 then
            lb.text = UnityTools.GetShortNum(info.count, isInteger) .. data.name;
        else
            lb.text = UnityTools.GetShortNum(info.count / 10, isInteger) .. data.name;
        end
    end
    local spr = item:GetComponent("UISprite");
    if spr ~= nil then
        spr.spriteName = data.icon;
        if info.base_id == 103 and info.count >= 5 and _awardData.isGameChange then --对局宝箱（显示三堆钞票）
            spr.spriteName = data.icon .. "_1";
        end
    end
    item.transform.localScale = UnityEngine.Vector3(1, 1, 1)
    local scale = 0.1
    local hash = iTween.Hash("time", 200 / 1000, "scale", UnityEngine.Vector3(scale, scale, 1.0), "luaEasetype", iTween.EaseType.easeOutBack)
    iTween.ScaleFrom(item, hash)
    local rect = item:GetComponent("UIRect");
    if rect ~= nil then
        rect.alpha = 0;
    end
    local alphaTween = TweenAlpha.Begin(item, 200 / 1000, 1);
end
local function OnClose(gameObject)
    if gTimer.hasTimer(OnClose) then
        gTimer.removeTimer(OnClose)
    end
    CloseWin(gameObject)
    UIEventListener.Get(gameObject).onClick = nil
end
local function addOneReward()
    --    LogWarn("[AwardWinMono.addOneReward]".._countIndex.."... lenght = "..#CTRL.AwardInfo);
    local awardInfo = _awardData.data
    if _countIndex > #awardInfo then
        gTimer.removeTimer(addOneReward);

    else
        local showObj = UtilTools.AddChild(_grid.gameObject, _cell);
        if showObj ~= nil then
            SetItemShow(showObj, awardInfo[_countIndex]);
            _itemList[#_itemList + 1] = showObj;
            _grid:Reposition();
        end
        _countIndex = _countIndex + 1;
        if _countIndex>#awardInfo then
            -- gTimer.registerOnceTimer(2000,OnClose,_go)
        end
    end

    _dragBgBoxCollider.enabled = _countIndex >= 5;
    -- if _countIndex >= 5 then
    --     _btnSure:SetActive(true);
    -- end
    if version.VersionData.IsReviewingVersion() then
        _btnShare:SetActive(false)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽VIP显示")
        return
    end
    _btnShare:SetActive(false)
    _btnSure:SetActive(true);
end




local function OnClickShare(gameObject)
    local descStr = "new_share_desc"..math.random(1,10)
    local picCount = BarcodeCam.getInstance().GetPicListLenth()
    local selecetIndex = math.random(2,picCount-1)
    UtilTools.ShareWeChatPic(1, tostring(GameDataMgr.PLAYER_DATA.Uuid), GameText.GetStr(descStr),BarcodeCam.getInstance().GetSharePic(0),BarcodeCam.getInstance().GetSharePic(1),BarcodeCam.getInstance().GetSharePic(selecetIndex),"","","")
    local protobuf = sluaAux.luaProtobuf.getInstance()
	protobuf:sendMessage(protoIdSet.cs_share_with_friends_req,{})
end

--- [ALF END]






-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _go = gameObject;
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")
    _titleBg = UnityTools.FindGo(gameObject.transform, "titleBg")

    _mack = UnityTools.FindGo(gameObject.transform, "Container/boxClick")
    UnityTools.AddOnClick(_mack.gameObject, OnClose)

    _scrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/dragBg/ScrollView")

    _grid = UnityTools.FindCo(gameObject.transform, "UIGrid", "Container/dragBg/ScrollView/Container/grid")

    _cell = UnityTools.FindGo(gameObject.transform, "cell")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, CloseWin)
    _dragBgBoxCollider = UnityTools.FindCo(gameObject.transform, "BoxCollider", "Container/dragBg")

    _titleBgTween = UnityTools.FindGo(gameObject.transform, "Container/titleBg")

        _btnShare = UnityTools.FindGo(gameObject.transform, "Container/btnShare")
    UnityTools.AddOnClick(_btnShare.gameObject, OnClickShare)
_sharePic =  UnityTools.FindGo(gameObject.transform, "Container/Sprite")
    if version.VersionData.IsReviewingVersion() then
        _sharePic:SetActive(false)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽VIP显示")
        return
    end
--- [ALB END]

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    --[[gTimer.registerOnceTime(400, function()
        _titleBgTween:SetActive(true);
    end)]]
	_sharePic:SetActive(false)
	_btnSure.transform.localPosition = UnityEngine.Vector3(0,_btnSure.transform.localPosition.y,0)
end

local function OpenAction()
    local scale = 0.1;
    local time = 500;
    UnityTools.OpenAction(_winBg, scale, time);
    _titleBg.transform.localScale = UnityEngine.Vector3(1, 1, 1)
    local hash = iTween.Hash("time", time * 0.8 / 1000, "scale", UnityEngine.Vector3(scale, scale, 1.0), "luaEasetype", iTween.EaseType.easeOutBack)
    iTween.ScaleFrom(_titleBg, hash)
end


local function ReplaceStart()
    _btnSure:SetActive(false);
    _btnShare:SetActive(false)
    OpenAction();

    UnityTools.AddEffect(_dragBgBoxCollider.transform, "effect_wupindaoju2_1", {
        scale = 1,
        complete = function(obj)
            if _dragBgBoxCollider ~= nil and _dragBgBoxCollider.transform ~= nil and obj.EffectGameObj ~= nil and obj.EffectGameObj.transform ~= nil then
                UtilTools.SetEffectRenderQueueByUIParent(obj.EffectGameObj.transform, obj.EffectGameObj.transform, 10);
            end
        end
    })

    if CTRL ~= nil then
        local rewardCount = 0;
        local awardInfos = CTRL.GetShowData();
        _awardData = awardInfos
        if awardInfos ~= nil then
            rewardCount = #awardInfos.data;
            addOneReward();
            --            UtilTools.HideWaitFlag()
            if rewardCount > 1 then
                gTimer.registerRepeatTimer(500, addOneReward);
            end
        else
            LogWarn("[AwardWinMono.Start]AwardInfo == ");
        end
    end
end

local function ReStart()
    if _winBg == nil then
        return
    end
    --重置初始化
    iTween.Stop(_winBg)
    _winBg.transform.localScale = UnityEngine.Vector3(0.1, 0.1, 1)
    iTween.Stop(_titleBg)
    _titleBg.transform.localScale = UnityEngine.Vector3(1, 1, 1)
    gTimer.removeTimer(addOneReward)
    _countIndex = 1


    for i = 1, #_itemList do
        if _itemList[i] ~= nil then
            iTween.Stop(_itemList[i])
            NGUITools.Destroy(_itemList[i])
        end
    end
    _itemList = {}

    ReplaceStart();
end

local function Start(gameObject)
    local scrollView = UnityTools.FindGo(gameObject.transform, "Container/dragBg/ScrollView")
    if scrollView ~= nil then
        _controller:SetScrollViewRenderQueue(scrollView);
    end
    ReplaceStart()
	
end





local function OnDestroy(gameObject)
    _winBg = nil
    _dragBgBoxCollider = nil
    gTimer.removeTimer(addOneReward);
    CLEAN_MODULE("AwardWinMono")
end






-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.ReStart = ReStart


-- 返回当前模块
return M
