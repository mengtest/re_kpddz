-- -----------------------------------------------------------------


-- *
-- * Filename:    unityTools.lua
-- * Summary:     unity工具集， 常用方法汇总
-- *
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2016-11-16 17:48:46
-- -----------------------------------------------------------------

local M = GENERATE_MODULE("UnityTools")

-- 取得UIManager
local UIManager = UI.Controller.UIManager



local _needBlurCnt = 0   --- 模糊计数
---
-- @function : 移除某个GameObject
-- @param : obj GameObject类型
local function destroy(obj)
    if obj ~= nil then
        UnityEngine.Object.Destroy(obj)
    end
end

---
-- @function : 获得所有组件
-- @param : parentGo GameObject类型
-- @param : compt String 要查询的子节点包含的component
-- @return list 组件列表
local function getAllComponents(parentGo, compt)
    local list = {}
    local compt = parentGo:GetComponentsInChildren(compt, true)
    local cnt = compt.Length
    for i = 1, cnt, 1 do
        list[i] = compt[i].gameObject
    end
    return list
end

---
-- @function : 将子节点作为参数，执行函数
-- @param : parentGo GameObject类型
-- @param : compt String 要查询的子节点包含的component
-- @return func 要运行的函数
local function callFuncInChildren(parentGo, compt, func, ...)
    local comps = parentGo:GetComponentsInChildren(compt, true)
    local cnt = comps.Length
    for i = 1, cnt, 1 do
        if func ~= nil then
            func(comps[i], ...)
        end
    end
end

---
-- @function : 移除某个GameObject下面名字为name的所有对象
-- @param : parentGo GameObject类型
-- @param : removeName String 要删除的字符串
-- @return 返回删除的个数
local function removeObjectByName(parentGo, removeName)
    -- 返回一个Component<UIButton>[] 的列表
    local comps = parentGo:GetComponentsInChildren("UIButton", true)

    -- C# 下的[]列表可以直接用for循环的方式去读取，但是不能当做table使用 pairs()
    local cnt = comps.Length

    -- 记录删除的个数
    local removeCnt = 0
    -- 此处检索序号要从1开始， 1对应c#原数组里的0
    for i = 1, cnt, 1 do
        if comps[i].name == removeName then
            -- 使用Object下的Destory函数移除某个对象
            destroy(comps[i].gameObject)
            cnt = cnt + 1
        end
    end
    -- 返回删除的个数
    return cnt
end

local function isWinShow(winName)
    return UI.Controller.UIManager.IsWinShow(winName)
end

---
-- @function : 获取UIManager下面的UI管理器
-- @param : ctrlName String 界面名称
-- @return 返回管理器的C#对象
local function getUIController(ctrlName)
    local ctrl = UIManager.GetControler(ctrlName)
    return ctrl
end

---
-- @function : 删除界面
-- @param : winName String 界面名称
local function destroyWin(winName)
    if isWinShow(winName) then
        M.SetGuassBlur(winName, false)
        UIManager.DestroyWin(winName, false, null)
        local platformMgr = IMPORT_MODULE("PlatformMgr");
        if platformMgr~=nil then
            platformMgr.Config:NextOpenStartWin(winName);
        end
    end
end

---
-- @function : 打开界面
-- @param : winName String 界面名称
local function createWin(winName, action, args)
    action = action or false
    args = args or nil
    UIManager.CreateWin(winName, false, nil)
end

-- @function: 打开界面, 带参数
local function createWinByAction(winName, action, args)
    UIManager.CreateWin(winName, action, args)
end

-- @function: 打开Lua界面, 带参数
local function createLuaWin(winName, action, args)
    action = false
    args = args or nil
    -- M.SetGuassBlur(winName)
    return UIManager.CreateLuaWin(winName, action, args)
end

local function openAction(winBg, scale, time)
    -- scale = scale or 0.1
    -- time = time or 300
    -- winBg.transform.localScale = UnityEngine.Vector3(1, 1, 1)
    -- local hash = iTween.Hash("time", time / 1000, "scale", UnityEngine.Vector3(scale, scale, 1.0), "luaEasetype", iTween.EaseType.easeOutBack)
    -- iTween.ScaleFrom(winBg, hash)
end

local function closeByAction(winBg, name, scale, time)
    scale = scale or 0.1
    time = time or 300
    winBg.transform.localScale = UnityEngine.Vector3(scale, scale, 1)
    local hash = iTween.Hash("time", time / 1000, "scale", UnityEngine.Vector3(1, 1, 1.0), "luaEasetype", iTween.EaseType.easeInBack)
    iTween.ScaleFrom(winBg, hash)
    gTimer.registerOnceTimer(time * 0.8, function() destroyWin(name) end)
end

---
-- @function : 判断对象是否存在，并打印
-- @param : Object obj 对象
-- @return 存在返回true
local function hasObj(obj)
    if obj ~= nil then
        print("has Obj")
        return true
    else
        print("has no found")
        return false
    end
end

local function printTable(tb)
    for i, v in pairs(tb) do
        print(i, v)
    end
end

--------------------------------------------------------------------------------------------
-- Find系列函数
-- @function : Transform下的Find函数
-- @param : Transform parent 父节点
-- @param : String path 查询路径
-- @param : String type Componet类型
--------------------------------------------------------------------------------------------

-- @return 返回Transform
local function findTf(parent, path)
    return parent:Find(path)
end

-- @return 返回GameObject
local function findGo(parent, path)
    local tf = findTf(parent, path)
    if tf ~= nil then return tf.gameObject
    else return nil
    end
end

-- @return 返回Component
local function findCo(parent, type, path)
    local go = findGo(parent, path)
    if go ~= nil then return go:GetComponent(type) else return nil end
end

-- @return 返回Component
local function findCoInChild(parent, type)
    return parent:GetComponentInChildren(type)
end

--------------------------------------------------------------------------------------------

-- @function: 添加一个UIEventListener的OnClick事件
-- @param: GameObject obj 对象
-- @param: Function path 响应函数
local function addOnClick(obj, func)
    UIEventListener.Get(obj).onClick = func
end

-- args为参数Table
-- color 颜色
-- alignment 对齐方式
-- okCall 确定回调函数
-- cancelCall 取消回调函数
-- toggle 是否显示复选框
-- okBtnName 确认按钮文本
-- closeSecond 关闭时间
-- isShowClose 是否显示右上关闭按钮
-- 例： UnityTools.MessageDialog("文本", {color = "000000", okBtnName = "OK"})
local function messageDialog(text, args)
    args = args or {}
    local color = args.color or "904c1d"
    local alignment = args.alignment or "Center"
    local okCall = args.okCall or nil
    local cancelCall = args.cancelCall or nil
    local toggle = args.toggle or false
    local okBtnName = args.okBtnName or ""
    local closeSecond = args.closeSecond or 0
    if args.isShowClose then
        LogWarn("[unityTools.MessageDialog]isShowClose=true");
    end
    local isShowClose = args.isShowClose or false
    UtilTools.MessageDialog(text, color, alignment, okCall, cancelCall, toggle, okBtnName, closeSecond, isShowClose)
end

-- 显示物品Tip界面
-- state true显示， false关闭
-- itemId 物品ID
-- args EventMultiArgs类型，用于额外显示内容，默认显示 Name，Desc
local function ShowItemTip(gameObject, state, itemID, args)
    if state then
        local cfg = ConfigDataMgr.getInstance().ItemBaseConfig:GetDataByKey(itemID)
        if cfg ~= nil then
            args = args or EventManager.EventMultiArgs()
            if args:ContainsKey("Title") == false then
                args:AddArg("Title", cfg.name)
            end
            if args:ContainsKey("Des") == false then
                args:AddArg("Des", cfg.desc)
            end
            UtilTools.ShowTips(args, gameObject, state)
        end
    else
        UtilTools.ShowTips(nil, gameObject, state)
    end
end

-- 添加长按显示Tip监听
-- 若对象没有BoxCollider会默认创建一个与UIWidget等大的Collider
-- gameObject 监听对象必须包含UIWidget
-- delay 长按延迟时间，默认200毫秒
local function addTipPress(gameObject, itemID, delay)
    local widget = gameObject:GetComponent("UIWidget")
    if widget == nil then return nil end

    delay = delay or 200
    local box = gameObject:GetComponent("BoxCollider")
    if box == nil then
        box = gameObject:AddComponent("UnityEngine.BoxCollider")
        box.size = widget.localSize
    end
    UIEventListener.Get(gameObject).onPress = function(go, isPressed)
        if isPressed then
            gTimer.registerOnceTimer(delay, ShowItemTip, go, isPressed, itemID)
        else
            gTimer.removeTimer(ShowItemTip)
            ShowItemTip(go, isPressed, itemID)
        end
    end
end

local _nowLevel = 0

local function OnClickCancelRelife()
    local req = {}
    req.stage = _nowLevel
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(12701, req)
end

local function openShopPanel()
    createWinByAction("Shop")
end


local function checkCanEnter()
    local bCanEnter = false

    for i = 1, 5 do
        local lvConf = LuaConfigMgr.LevelNeedConfig[tostring(i)]
        if lvConf ~= nil then
            if GameDataMgr.PLAYER_DATA.Money >= tonumber(lvConf.gold_need) and GameDataMgr.ITEM_DATA.GunData.lv >= tonumber(lvConf.lv_min) then
                return true
            end
        end
    end
    local lvConf = LuaConfigMgr.LevelNeedConfig[tostring(1)]
    if bCanEnter == false and lvConf ~= nil then
        if GameDataMgr.PLAYER_DATA.Money < tonumber(lvConf.gold_need) then
            UtilTools.ShowMessage(LuaText.GetString("task_desc9"), "[FF0000]")
        elseif GameDataMgr.ITEM_DATA.GunData.lv < tonumber(lvConf.lv_min) then
            UtilTools.ShowMessage(LuaText.GetString("task_desc10"), "[FF0000]")
        end
    end
    return false
end



-- parent 要加入父Transform
-- path 特效的地址，必须在"effect/Prefab/"路径下
-- args.loop 是否循环播放， 默认false
-- args.offset 特效层级偏移， 默认20
-- args.scale 特效缩放，默认1
-- args.speed 特效速度，默认1
-- args.destroy 是否播放完成自动销毁，默认true
-- args.complete 特效创建完成回调函数 
-- args.remove 特效移除回调
local function addEffect(parent, path, args)
    if parent == nil or path == nil then return nil end
    args = args or {}
    if string.find(path, "Effects/Prefab/") == nil then
        path = "Effects/Prefab/" .. path
    end
    if string.find(path, ".prefab") == nil then
        path = path .. ".prefab"
    end
    local loop = args.loop or false
    local offset = args.offset or 20
    local scale = args.scale or 1
    local speed = args.speed or 1
    local destroy = true
    if args.destroy ~= nil then
        destroy = args.destroy
    end
    local effObj = effect.EffectManager.getInstance():addEffect(parent, path, loop, offset, scale, speed, destroy)
    if args.complete ~= nil then
        effObj._loadComplete = args.complete
    end
    if args.remove ~= nil then
        effObj._removeComplete = args.remove
    end
    return effObj
end

local function checkMsg(msgID, msgData)
    if msgData ~= nil then
        if msgData.result ~= nil then
            if msgData.result == 0 then
                return true
            end
        else
            return true
        end
    end
    PrintTable(msgData)
    if msgData.err ~= nil then
        M.ShowMessage(msgData.err)
    end
    return false
end
---生成几位小数
---@param value 原小数
---@param num 需要几位
local function GetFloatNum(value,num)

    local tmp = math.pow(10,num)
    LogWarn("[unityTools.GetFloatNum]value = "..value.. "  num "..num);
    return math.floor(value*tmp)/tmp
 end

--desc:货币显示格式
--YQ.Qu:2017/2/17 0017
local function GetShortNum(value, decimals)
    decimals = decimals or false;
    local numType = "%d"
    if value == nil or value == 0 or value == "" then return 0 end
    value = tonumber(value)
    if value < 10000 then
        return value .. ""
    elseif value < 100000 then --万
        if decimals == false then
            numType = "%.3f";
        end
        return string.format(numType .. LuaText.GetString("wan"), GetFloatNum(value / 10000,3))
    elseif value < 1000000 then --10万
        if decimals == false then
            numType = "%.2f";
        end
        return string.format(numType .. LuaText.GetString("wan"), GetFloatNum(value / 10000,2))
    elseif value < 10000000 then --100万
        if decimals == false then
            numType = "%.1f";
        end
        return string.format(numType .. LuaText.GetString("wan"), GetFloatNum(value / 10000,1))
    elseif value < 100000000 then --1000万
        local num = value / 100000000
        --        LogWarn("[unityTools.GetShowNum3]" .. LuaText.GetString("wan") .. num);
        return string.format(numType .. LuaText.GetString("wan"), value / 10000)
    elseif value < 1000000000 then --亿
        if decimals == false then
            numType = "%.3f";
        end
        return string.format(numType .. LuaText.GetString("yi"), GetFloatNum(value / 100000000,3))
    elseif value < 100000000000 then --亿
        if decimals == false then
            numType = "%.1f";
        end
        return string.format(numType .. LuaText.GetString("yi"), GetFloatNum(value / 100000000,1))
    end
    LogError("[unityTools.GetShorNum]货币显示错误：" .. value);

    return 0
end

--desc:货币显示格式
local function GetShortNum2(num)
    local newNum=0
    num=tonumber(num)
    num=num
    if num < 10000 then
        return tostring(num)
    elseif num < 100000000 then --1亿
        newNum=num/10000
        local leftNum = math.floor(newNum)
        if leftNum <100 then
            newNum = math.floor(num/100)
            return LuaText.Format("num_wan",newNum/100)
        elseif leftNum <1000 then
            newNum = math.floor(num/1000)
            return LuaText.Format("num_wan",newNum/10)
        else
            return LuaText.Format("num_wan",math.floor(newNum))
        end
    else
        newNum=num/100000000
        local leftNum = math.floor(newNum)
        if leftNum <100 then
            newNum = math.floor(num/1000000)
            return LuaText.Format("num_yi",newNum/100)
        elseif leftNum <1000 then
            newNum = math.floor(num/10000000)
            return LuaText.Format("num_yi",newNum/10)
        else
            return LuaText.Format("num_yi",math.floor(newNum))
        end
    end
end
--desc:返回主界面
--YQ.Qu:2017/2/20 0020
local function ReturnToMainCity()
    local winKey = "MainWin";
    if UI.Controller.UIManager.IsWinShow("MainCenterWin") and  UI.Controller.UIManager.IsWinShow(winKey)then
        LogError("IsWinShow(winKey)=true333")
        return;
    end
    M.CallLoadingWin(true)
    if UI.Controller.UIManager.IsWinShow("MainCenterWin") == false then
        createLuaWin("MainCenterWin")
    end

    if UI.Controller.UIManager.IsWinShow(winKey) then
        LogError("IsWinShow(winKey)=true")
        triggerScriptEvent(EVENT_SHOW_MAIN_WIN, {});
    else
    LogError("IsWinShow(winKey)=false")
        createLuaWin(winKey, true);
    end

    ---请求红包数据
    local redBagCtrl = IMPORT_MODULE("RedBagWinController")
    if redBagCtrl ~= nil then
        redBagCtrl.GetRedBagListFromServer(1)
    end

    _needBlurCnt = 0
end

local _objPools = {}
local _createPools = {}
-- setmetatable(_createPools, { __mode = "kv" })

local function createObjsToPool(cnt, parent, prefab, initPos)
    if _objPools[prefab] == nil then 
        return nil 
    end

    -- local active = prefab.activeSelf
    -- prefab:SetActive(true)
    local scale = 20
    for i = 1, cnt, 1 do
        local cObj = UtilTools.AddChild(parent, prefab, initPos)
        cObj.transform.localScale = prefab.transform.localScale
        cObj.transform.localRotation = prefab.transform.localRotation --UnityEngine.Quaternion.Euler(-90 , 180, 0)
        local compt = cObj:GetComponent("FastMove")
        compt.IsOtherEvent = false
        compt:SetDefaultCall(function(obj)
            M.ReleasePoolObj(prefab, obj)
        end)
        _objPools[prefab].active[_objPools[prefab].active.Count + 1] = compt
        _objPools[prefab].active.Count = _objPools[prefab].active.Count + 1
        -- cObj.name = "active" .. _objPools[prefab].active.Count
    end
    -- prefab:SetActive(active)
end

local function delayCreateObjToPool(cnt, parent, prefab, initPos)
    local lastCnt = cnt - 50
    if lastCnt > 0 then
        if _createPools[prefab] == nil then
            _createPools[prefab] = { 0, parent, initPos }
        end
        _createPools[prefab][1] = _createPools[prefab][1] + lastCnt
        createObjsToPool(50, parent, prefab, initPos)
    else
        createObjsToPool(cnt, parent, prefab, initPos)
    end
end

-- 创建缓存池
local function createPool(prefab, parent, preCnt, limit, maxCount, initPos)
    if prefab == nil or parent == nil or preCnt == nil then return end
    limit = limit or false
    maxCount = maxCount or 0
    initPos = initPos or UnityEngine.Vector3(-10000, -10000, -10000)
    if _objPools[prefab] == nil then
        _objPools[prefab] = { PreCnt = preCnt, active = { Count = 0 }, deactive = { Count = 0 } }
    end
    local hasCnt = _objPools[prefab].active.Count + _objPools[prefab].deactive.Count
    delayCreateObjToPool(preCnt - hasCnt, parent, prefab, initPos)
    -- createObjsToPool(preCnt - hasCnt, parent, prefab, initPos)
end

-- 获取缓冲池对象
local function getPoolObj(prefab, parent)
    if prefab == nil or _objPools[prefab] == nil then return nil end
    if _objPools[prefab].active.Count == 0 then
        if parent ~= nil then
            LogError("C")
            -- createObjsToPool(1, parent, prefab, UnityEngine.Vector3(-10000, -10000, -10000))
        else
            return nil
        end
    end

    local cObj = _objPools[prefab].active[_objPools[prefab].active.Count]
    -- cObj:SetActive(true)
    _objPools[prefab].active[_objPools[prefab].active.Count] = nil
    _objPools[prefab].active.Count = _objPools[prefab].active.Count - 1
    _objPools[prefab].deactive[cObj] = 1
    _objPools[prefab].deactive.Count = _objPools[prefab].deactive.Count + 1
    -- cObj.name = "deactive" .. _objPools[prefab].deactive.Count

    -- cObj.IsOtherEvent = false
    return cObj
end

local function getPool(prefab)
    return _objPools[prefab]
end

local function releasePoolObj(prefab, obj)
    if obj == nil then return end

    obj:Stop()
    _objPools[prefab].deactive[obj] = nil
    _objPools[prefab].deactive.Count = _objPools[prefab].deactive.Count - 1
    -- obj:SetActive(false)
    obj.transform.position = UnityEngine.Vector3(-10000, -10000, -10000)
    _objPools[prefab].active[_objPools[prefab].active.Count + 1] = obj
    _objPools[prefab].active.Count = _objPools[prefab].active.Count + 1
    -- obj.name = "active" .. _objPools[prefab].active.Count
    -- if _objPools[prefab].PreCnt <= _objPools[prefab].active.Count then
    --     destroy(obj)
    -- else
    --     _objPools[prefab].active[_objPools[prefab].active.Count + 1] = obj
    --     _objPools[prefab].active.Count = _objPools[prefab].active.Count + 1
    -- end
end

local function removePoolObj(pool)
    if pool ~= nil then
        for i = 1, pool.active.Count, 1 do
            destroy(pool.active[i])
        end
    end
    pool = nil
end

-- 删除所有缓冲池对象
local function delPoolAllObj(prefab)
    if prefab == nil then return end
    local pool = _objPools[prefab]
    removePoolObj(pool)
end

-- 删除缓冲池
local function delPools()
    for k, v in pairs(_objPools) do
        removePoolObj(v)
    end
    _objPools = nil
    _objPools = {}
    _createPools = nil
    _createPools = {}
end

local function getPoolCnt(prefab)
    if _objPools[prefab] == nil then
        return 0
    end
    return _objPools[prefab].active.Count
end

local function ShowMessage(str, color)
    local textColor = color or "[FFFFFF]"
    UtilTools.ShowMessage(str, textColor);
end

local function ShowPlayerInfoTip(args, go, state)
    local wName = "PlayerInfoTipsWin";
    if state then
        args.go = go;
        createLuaWin(wName);
        local tipCtrl = IMPORT_MODULE(wName .. "Controller")
        if tipCtrl ~= nil then
            tipCtrl.ShowTips(args);
        end
    else
        --        UnitTools.DestroyWin(wName)
        --        UIManager.DestroyWin(wName, false, null)
        destroyWin(wName);
    end
end

-- 显示人物界面上的 物品Tip界面
-- state true显示， false关闭
-- itemId 物品ID
-- args tab类型，用于额外显示内容，默认显示 Name，Desc
local function ShowPlayerInfoItemTip(gameObject, state, itemID, args)
    if state then
        local cfg = ConfigDataMgr.getInstance().ItemBaseConfig:GetDataByKey(itemID)
        if cfg ~= nil then
            local args = args or {}
            if args.title == nil then
                args.title = cfg.name
            end
            if args.desc == nil then
                args.desc = cfg.desc;
            end
            ShowPlayerInfoTip(args, gameObject, state)
        end
    else
        ShowPlayerInfoTip(nil, gameObject, state)
    end
end

-- 导出local方法

local function createPoolOnUpdate()
    if _createPools ~= nil then
        for k, v in pairs(_createPools) do
            local parent = v[2]
            local prefab = k
            local cnt = v[1]
            local initPos = v[3]
            if parent ~= nil and prefab ~= nil then
                if cnt > 50 then
                    -- LogError("Create")
                    createObjsToPool(50, parent, prefab, initPos)
                    _createPools[k][1] = cnt - 50
                else
                    createObjsToPool(cnt, parent, prefab, initPos)
                    -- LogError(_objPools[prefab].active.Count .. "  " .. _objPools[prefab].deactive.Count)
                    _createPools[k] = nil
                end
            else
                LogError("Nil")
            end
        end
    end
end

--绑定手机
local function MatchPhone(str)
    return string.match(str, "^[1][3,4,5,7,8,9]%d%d%d%d%d%d%d%d%d$") == str
end

local function update(deltaTime)
    createPoolOnUpdate()
end


--desc:
--YQ.Qu:2017/3/1 0001
local function SetPlayerHead(path, headTexture, isPlayer)
    isPlayer = isPlayer or false;
    if isPlayer and GameDataMgr.PLAYER_DATA.PlayerHead ~= nil then
        LogWarn("[unityTools.SetPlayerHead]已经加载过头像了");
        if headTexture ~= nil then
            headTexture.mainTexture = GameDataMgr.PLAYER_DATA.PlayerHead;
        end
        return;
    end
    UtilTools.LoadHead(path, headTexture, isPlayer);
end

--- 设置按钮上的货币图标
--- @param spr
--- @param cost_type "101":金币，"102":钻石，"103":钞票，"107":话费卷，"109":红包
local function SetCostIcon(spr, cost_type)
    cost_type = cost_type or "101";
    cost_type = cost_type .. "";
    if cost_type == "101" then --金币
        spr.spriteName = "money"
    elseif cost_type == "102" then
        spr.spriteName = "diamond"
    elseif cost_type == "103" then
        spr.spriteName = "cash"
    elseif cost_type == "107" then
        spr.spriteName = "money"
    elseif cost_type == "109" then
        spr.spriteName = "redBag1"
    end
end
--- desc:设置Vip头像特效
-- YQ.Qu:2017/3/21 0021
-- @param sprContainer Vip的父窗口（vip,crown）
-- @param vip vip等级
-- @return
local function SetNewVipBox(sprContainer, vip,vipstr,parent,newscale)
    local _platformMgr = IMPORT_MODULE("PlatformMgr");
    if sprContainer == nil then
        return
    end
    local spVip = findCo(sprContainer.transform, "UISprite", "vip");
    if spVip == nil then
        sprContainer.gameObject:SetActive(false);
        return
    end
    if vip == nil then
        vip = -1;
    end
    
    if version.VersionData.IsReviewingVersion() then
        sprContainer.gameObject:SetActive(false);
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽VIP显示")
        return
    end
    --绿色包不显示Vip
    if _platformMgr.config_vip == false then
        sprContainer.gameObject:SetActive(false);
        return;
    end
    sprContainer.gameObject:SetActive(vip >= 0);
    if vipstr == nil or vipstr == "" then
        vipstr = "v"
    end
    spVip.spriteName = vipstr .. vip;
    -- local effect = findGo(sprContainer.transform, "effect_vip");
    -- if effect ~= nil then
    --     -- if vip == 10 then
    --     --     effect.gameObject:SetActive(true);
    --     -- else
    --     --     effect.gameObject:SetActive(false);
    --     -- end
    --     if parent ~= nil then
    --         LogError("2222")
    --         UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect.transform, 10);
    --     end
    -- else
    --     if vip >= 0 then
    --         local effect = addEffect(sprContainer.transform, "biangkuang01", {
    --             scale = 1,
    --             loop = true,
    --             complete = function(obj)
    --                 local gObj = obj.EffectGameObj
    --                 gObj.transform.name = "effect_vip";
    --                 if parent ~= nil then
    --                     LogError("3333")
    --                 -- local setSale = 320 * (vipSpr.width / 253);
    --                 -- gObj.transform.localScale = UnityEngine.Vector3(setSale, setSale, setSale)
    --                     UtilTools.SetEffectRenderQueueByUIParent(parent.transform, gObj.transform, 10);
    --                 end                    
    --             end
    --         })
    --         --            effect.transform.name = "effect_vip";
    --     end
    -- end
        if parent == nil then   
            return
        end
        if vip>=8 and vip<=10 then
            LogError("vip="..vip)
            local effect = findGo(sprContainer.transform, "effect_vip");
            if effect ~= nil then
                effect.gameObject:SetActive(true)
                findGo(effect.transform, "v8").gameObject:SetActive(false)
                findGo(effect.transform, "v9").gameObject:SetActive(false)
                findGo(effect.transform, "v10").gameObject:SetActive(false)
                if vip == 10 then
                    findGo(effect.transform, "v10").gameObject:SetActive(true)
                    local effect1 = findGo(effect.transform, "v10/kuang")
                    local effect2 = findGo(effect.transform, "v10/v101")
                    UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect1.transform, 20);
                    UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect2.transform, 22);
                else
                    findGo(effect.transform, "v"..vip).gameObject:SetActive(true)
                    UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect.transform, 22);
                end
            else
                if newscale == nil then
                    newscale = sprContainer.transform.parent.transform.localScale.x
                end
                if ComponentData.Get(sprContainer.gameObject).Id == 1 then
                    return
                end 
                ComponentData.Get(sprContainer.gameObject).Id = 1
                effect = addEffect(sprContainer.transform, "biangkuang01", {
                scale = 1,
                loop = true,
                complete = function(obj)
                    local gObj = obj.EffectGameObj
                    gObj.transform.name = "effect_vip";
                    gObj.transform.localScale = UnityEngine.Vector3(1,1,1)
                    findGo(gObj.transform, "v8").gameObject:SetActive(false)
                    findGo(gObj.transform, "v9").gameObject:SetActive(false)
                    findGo(gObj.transform, "v10").gameObject:SetActive(false)
                    if parent ~= nil then 
                            local ef = findGo(gObj.transform, "v10").gameObject
                            if vip == 10 then
                                ef:SetActive(true)
                            end
                            ef.transform.localScale = UnityEngine.Vector3(ef.transform.localScale.x/newscale,ef.transform.transform.localScale.y/newscale,ef.transform.transform.localScale.z/newscale)
                            local effect1 = findGo(gObj.transform, "v10/kuang")
                            local effect2 = findGo(gObj.transform, "v10/v101")
                            effect1.transform.localScale = UnityEngine.Vector3(effect1.transform.localScale.x*newscale,effect1.transform.transform.localScale.y*newscale,effect1.transform.transform.localScale.z*newscale)
                            effect2.transform.localScale = UnityEngine.Vector3(effect2.transform.localScale.x*newscale,effect2.transform.transform.localScale.y*newscale,effect2.transform.transform.localScale.z*newscale)
                            UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect1.transform, 20);
                            UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect2.transform, 22);
                            local child=nil
                            for i =1,effect1.transform.childCount do
                                child = effect1.transform:GetChild(i-1)
                                child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*newscale,child.transform.localScale.y*newscale,child.transform.localScale.z*newscale)
                            end
                            for i =1,effect2.transform.childCount do
                                child = effect2.transform:GetChild(i-1)
                                child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*newscale,child.transform.localScale.y*newscale,child.transform.localScale.z*newscale)
                            end
                            
                            effect1 = findGo(gObj.transform, "v9")
                            if vip == 9 then
                                effect1.gameObject:SetActive(true)
                            end
                            effect2 = effect1.transform:Find("guang")
                            effect2.transform.localPosition = UnityEngine.Vector3(0,0,1/newscale * -100)

                            effect1.transform.localScale = UnityEngine.Vector3(effect1.transform.localScale.x*newscale,effect1.transform.transform.localScale.y*newscale,effect1.transform.transform.localScale.z*newscale)
                            UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect1.transform, 22);
                            UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect2.transform, 23);
                            for i =1,effect1.transform.childCount do
                                child = effect1.transform:GetChild(i-1)
                                child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*newscale,child.transform.localScale.y*newscale,child.transform.localScale.z*newscale)
                            end

                            effect1 = findGo(gObj.transform, "v8")
                            if vip == 8 then
                                effect1.gameObject:SetActive(true)
                            end
                            effect1.transform.localScale = UnityEngine.Vector3(effect1.transform.localScale.x*newscale,effect1.transform.transform.localScale.y*newscale,effect1.transform.transform.localScale.z*newscale)
                            UtilTools.SetEffectRenderQueueByUIParent(parent.transform, effect1.transform, 22);
                            
                            for i =1,effect1.transform.childCount do
                                child = effect1.transform:GetChild(i-1)
                                child.transform.localScale = UnityEngine.Vector3(child.transform.localScale.x*newscale,child.transform.localScale.y*newscale,child.transform.localScale.z*newscale)
                            end
                    end                    
                end
            })
            end
            spVip.enabled = false
        else
            local effect = findGo(sprContainer.transform, "effect_vip");
            if effect ~= nil then
                effect.gameObject:SetActive(false)
            end
            spVip.enabled = true
        end
end
--- desc:设置Vip头像特效
-- YQ.Qu:2017/3/21 0021
-- @param sprContainer Vip的父窗口（vip,crown）
-- @param vip vip等级
-- @return
local function SetVipBox(sprContainer, vip, setEffectRenderQ)
    local _platformMgr = IMPORT_MODULE("PlatformMgr");
    if sprContainer == nil then
        return
    end

    if vip == nil then
        vip = 0;
    end
    local vipSpr = findCo(sprContainer.transform, "UISprite", "vip");
    local crown = findGo(sprContainer.transform, "crown");

    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        vipSpr.gameObject:SetActive(false);
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽VIP显示")
        return
    end

    --绿色包不显示Vip
    if _platformMgr.config_vip == false then
        if vipSpr ~= nil then
            vipSpr.gameObject:SetActive(false);
        end

        if crown ~= nil then
            crown:SetActive(false);
        end
        return;
    end

    if vipSpr ~= nil then
        vipSpr.gameObject:SetActive(vip > 0);
        vipSpr.spriteName = "v" .. vip;
    end

    if crown ~= nil then
        crown:SetActive(vip > 8);
    end

    local effect = findGo(sprContainer.transform, "effect_vip");
    if effect ~= nil then
        if vip == 10 then
            --            effect.transform.SetScale
            effect.gameObject:SetActive(true);
        else
            effect.gameObject:SetActive(false);
        end
    else
        if vip == 10 then
            local effect = addEffect(sprContainer.transform, "effect_vip", {
                scale = vipSpr.width / 253,
                loop = true,
                complete = function(obj)
                    local gObj = obj.EffectGameObj
                    gObj.transform.name = "effect_vip";
                    local setSale = 320 * (vipSpr.width / 253);
                    gObj.transform.localScale = UnityEngine.Vector3(setSale, setSale, setSale)
                    if setEffectRenderQ ~= nil then
                        setEffectRenderQ(sprContainer.transform, gObj.transform);
                    else
                        UtilTools.SetEffectRenderQueueByUIParent(sprContainer.transform, gObj.transform, 1);
                    end
                end
            })
            --            effect.transform.name = "effect_vip";
        end
    end
end

local _debugSound = true
local _soundCnt = 0
local _soundList = {}
setmetatable(_soundList, { __mode = "kv" })
gTimer.registerRepeatTimer(800, function()
    _soundCnt = 0
end)
-- args = {delTime, loop, perTime, target, type, delay}
local function playSound(path, args)
    args = args or {}
    
    if _soundCnt > 35 or _debugSound == false then return nil end
    local autoDelTime = args.delTime or 7
    local loop = args.loop or false
    local perTime = args.perTime or 0
    local target = args.target or nil
    local type = args.type or 1
    local delay = args.delay or 0
    local soundObj = UtilTools.PlaySoundEffect(path, autoDelTime, loop, perTime, target, type, delay)
    if soundObj ~= nil then
        _soundCnt = _soundCnt + 1
        _soundList[soundObj] = path
    end

    return soundObj
end

local function removeAllSound()
    for k, v in pairs(_soundList) do
        if k ~= nil then
            k:Stop()
        end
    end
end

local function getLongNumber(num)
    local strNum = tostring(num)
    local newStr = ""
    local numTb = {}
    for k, v in string.gmatch(strNum, "%d") do
        if 0 ~= string.len(k) then
            numTb[#numTb + 1] = k
        end
    end

    local index = 1
    for i = #numTb, 1, -1 do
        local str = numTb[i]

        if index == 3 and i ~= 1 then
            newStr = "," .. str .. newStr
            index = 0
        else
            newStr = str .. newStr
        end
        index = index + 1
    end
    return newStr
end

local function RGB(r, g, b, a)
    a = a or 255
    return { r / 255, g / 255, b / 255, a / 255 }
end

local _lastPlayingBgm = nil
local function setBGM(bgm)
    if bgm == nil then
        if _lastPlayingBgm == nil then
            return nil
        end
        bgm = _lastPlayingBgm
    end
    _lastPlayingBgm = bgm
    local _platformMgr = IMPORT_MODULE("PlatformMgr");
    if _platformMgr.GetMusic() > 0 then
        UtilTools.SetBgm(_lastPlayingBgm)
    end
end

g_openGuassBlur = true

local function setGuassBlur(wName, isShow)
    if isShow == nil then isShow = true end
    local exceptWin = {
        RoomLvSelectWin = 1,
        NormalCowMain = 1,
        HundredCowMain = 1,
        MainWin = 1,
        MainCenterWin = 1,
        GuideWin = 1,
        FruitWin = 1,
        FruitAwardWin = 1,
        MonthCardHelpWin = 1,
        AwardWin = 1,
        ChatMainWin = 1
    }
    local CamObj = UnityEngine.GameObject.Find("Scene/Cameras/SceneCamera")
    local blurEff = CamObj:GetComponent("BlurOptimized") --RapidBlurEffect
    if blurEff == nil then
        return;
    end
    if exceptWin[wName] == nil then  
        if isShow == true then
            _needBlurCnt = _needBlurCnt + 1
        else
            _needBlurCnt = _needBlurCnt - 1
        end 
    end
    if g_openGuassBlur == false then return nil end
    if _needBlurCnt > 0 then
        if blurEff.enabled == false then
            blurEff.enabled = true
            -- LogError(wName .. "need blur")
        end
    else 
        blurEff.enabled = false
    end
end

---重置模糊计数器
local function ResetBlurCnt()
    _needBlurCnt = 0;
 end

local function ChangeLogin()
    _needBlurCnt = 0;
    UtilTools.ChangeLogin();
end

-- local _faManager = nil
-- local function setFastActionManager(manager)
--     _faManager = manager
-- end

-- local function fastMove(obj, time, world)
--     obj.Duration = time
--     obj.IsWorld = world
--     _faManager:Begin(obj)
-- end

-- local function actionPos(obj, time, args, eCall, delay, wName)
--     fastMove(obj, time, args.world)
--     -- local aFun = function() 
--     --     -- if obj == nil then
--     --     --     local creator = args.creator
--     --     --     if creator ~= nil then
--     --     --         obj = creator(args.creatorArgs)
--     --     --     end
--     --     -- end
--     --     local world = args.world or false
--     --     -- local from = args.from
--     --     -- local to = args.to or UnityEngine.Vector3.zero
--     --     -- if from ~= nil then
--     --     --     if world == true then
--     --     --         obj.transform.position = from
--     --     --     else
--     --     --         obj.transform.localPosition = from
--     --     --     end
--     --     -- end
--     --     obj.Duration = time
--     --     -- obj.IsWorld = world
        
--     --     -- if eCall ~= nil then
--     --     --     obj.IsOtherEvent = true
--     --     --     obj:SetOtherCall(function(o)
--     --     --         eCall(o)
--     --     --     end)
--     --     -- else
--     --     --     obj.IsOtherEvent = false
--     --     -- end
--     --     _faManager:Begin(obj)
--     --     return obj
--     -- end
--     if delay > 0 then 
--         local timer = gTimer.registerOnceTimer(delay, fastMove, obj, time, args.world)
--         gTimer.setRecycler(wName, timer)
--         return timer
--     else
--         return fastMove(obj, time, args.world)
--     end
-- end

local _deactiveList = {}
-- setmetatable(_deactiveList, { __mode = "k" })
local function setActive(object, active)
    local obj = object.gameObject
    if obj == nil then return end
    local aObj = _deactiveList[object.gameObject]

    if active == false and aObj == nil then
        _deactiveList[object.gameObject] = obj.transform.localPosition
        obj.transform.localPosition = UnityEngine.Vector3(-10000,-10000,-10000)
    elseif active == true and aObj ~= nil then
        obj.transform.localPosition = aObj
        _deactiveList[object.gameObject] = nil
    end
end

local function removeDeactive(object)
    _deactiveList[object.gameObject] = nil
end

local function removeDeactiveList()
    _deactiveList = nil
    _deactiveList = {}
end

local function collect()
    -- gTimer.registerOnceTimer(200, function() 
        
    -- end)
    asset.AssetManager.getInstance().regularlyClearAssets()
    CollectLua()
end

local _loadingWin = nil
local function callLoadingWin(show)
    if _loadingWin == nil then
        local root = UnityEngine.GameObject.Find("UIRoot")
        _loadingWin = findGo(root.transform, "LoadingWin")
    end
    _loadingWin:SetActive(show)
end

local _openWinTimer = 0
local function recordOpenTime(over)
    if over == false then
        _openWinTimer = UtilTools.GetCurrentTime()
    else
        LogWarn("cost :" .. (UtilTools.GetCurrentTime() - _openWinTimer))
    end
end 
local function setHead(head,icon,vip,isSelf,sex)
    local headImg = head.transform:Find("headImg"):GetComponent("UISprite")
    if headImg == nil then
        return
    end
    if icon~=nil and icon ~="" then
        local headtex= headImg.transform:Find("Texture"):GetComponent("UITexture")
        if headtex == nil then
            return
        end
        headtex.mainTexture = nil
        SetPlayerHead(icon,headtex,isSelf)
    else
        local headtex= headImg.transform:Find("Texture"):GetComponent("UITexture")
        if headtex ~= nil then
            headtex.mainTexture = nil
        end
        local _platformMgr = IMPORT_MODULE("PlatformMgr");
        headImg.spriteName = _platformMgr.PlayerDefaultHead(sex)
    end
    SetVipBox(headImg,vip)
end
local function startRecord(sAudioName)
    local jarUtilTools = UnityEngine.GameObject.Find("UIRoot/UICamera"):GetComponent("JARUtilTools")
    if jarUtilTools ~= nil then
        return jarUtilTools:startRecord(sAudioName);
    end
    return false
end

local function stopRecord()
    local jarUtilTools = UnityEngine.GameObject.Find("UIRoot/UICamera"):GetComponent("JARUtilTools")
    if jarUtilTools ~= nil then
        jarUtilTools:stopRecord();
    end
end

local function deleteAudio(strAudioName)
	local jarUtilTools = UnityEngine.GameObject.Find("UIRoot/UICamera"):GetComponent("JARUtilTools")
    if jarUtilTools ~= nil then
        jarUtilTools:deleteAudio(strAudioName);
    end
end

local function playAudio(strAudioName)
	local jarUtilTools = UnityEngine.GameObject.Find("UIRoot/UICamera"):GetComponent("JARUtilTools")
    if jarUtilTools ~= nil then
        jarUtilTools:playAudio(strAudioName);
    end
end
M.playAudio = playAudio
M.deleteAudio = deleteAudio
M.startRecord = startRecord
M.stopRecord = stopRecord
M.RecordOpenTime = recordOpenTime
M.CallLoadingWin = callLoadingWin
-- M.FastMove = fastMove
-- M.SetFastActionManager = setFastActionManager
M.RemoveDeactive = removeDeactive
M.RemoveDeactiveList = removeDeactiveList
M.Collect = collect
M.SetActive = setActive
-- M.ActionPos = actionPos
M.SetGuassBlur = setGuassBlur
M.SetBGM = setBGM
M.RGB = RGB
M.GetLongNumber = getLongNumber
M.PlaySound = playSound
M.RemoveAllSound = removeAllSound
M.UIManager = UIManager
M.Destroy = destroy
M.GetAllComponents = getAllComponents
M.RemoveObjectByName = removeObjectByName
M.GetUIController = getUIController
M.DestroyWin = destroyWin
M.CreateWinByAction = createWinByAction
M.CreateWin = createWin
M.CreateLuaWin = createLuaWin
M.OpenAction = openAction
M.CloseByAction = closeByAction
M.HasObj = hasObj
M.PrintTable = printTable
M.FindTf = findTf
M.FindGo = findGo
M.FindCo = findCo
M.FindCoInChild = findCoInChild
M.AddOnClick = addOnClick
M.CallFuncInChildren = callFuncInChildren
M.MessageDialog = messageDialog
M.AddTipPress = addTipPress
M.AddEffect = addEffect
M.CheckMsg = checkMsg
M.GetShortNum = GetShortNum
M.ReturnToMainCity = ReturnToMainCity
M.CreatePool = createPool
M.GetPoolObj = getPoolObj
M.DelPoolAllObj = delPoolAllObj
M.DelPools = delPools
M.ReleasePoolObj = releasePoolObj
M.GetPool = getPool
M.GetPoolCnt = getPoolCnt
M.ShowMessage = ShowMessage
M.Update = update
M.ShowPlayerInfoItemTip = ShowPlayerInfoItemTip
M.MatchPhone = MatchPhone
M.SetPlayerHead = SetPlayerHead
M.IsWinShow = isWinShow
M.SetCostIcon = SetCostIcon
M.SetVipBox = SetVipBox
M.SetNewVipBox = SetNewVipBox 
M.ChangeLogin = ChangeLogin
M.ResetBlurCnt = ResetBlurCnt
M.SetHead = setHead
M.GetShortNum2 =GetShortNum2
return M