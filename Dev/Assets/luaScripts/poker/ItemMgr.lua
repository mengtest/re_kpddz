-- -----------------------------------------------------------------

-- User: EQ
-- Date: 2017/2/24 0024
-- Time: 下午 4:45
-- To change this template use File | Settings | File Templates.
-- Summary:     物品管理类
-- Version:     1.0.0
-- -----------------------------------------------------------------


-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ItemMgr")
local UnityTools = IMPORT_MODULE("UnityTools")

local _list = {}
local _bagBaseList = {}
local _shopItemList = {}
local _sysMails = {}

---清理数据
local function ClearAll()
    _list = {}
    _bagBaseList ={}
    _shopItemList = {}
    _sysMails = {}
 end



local function getList()

    return _list;
end

-------------------------------- 【物品】


--- 属于货币的物品，会抛出更新
local function ItemResourceUpadte(baseId)
    baseId = baseId or "109";
    local id = baseId .. "";

    if id == "109" then --- 红包
        triggerScriptEvent(EVENT_RESCOURCE_UDPATE, 109)
    end
end

--desc:物品更新
--YQ.Qu:2017/2/24 0024
local function ItemUpdate(tMsgData)
    if tMsgData ~= nil and tMsgData.upd_list ~= nil then
        for k, v in pairs(tMsgData.upd_list) do
            --            LogWarn("[ItemMgr.ItemUpdate]" .. "更新的Uuid = " .. v.uuid .. "    " .. v.base_id .. "  count = " .. v.count);
            _list[v.uuid] = v;
            ItemResourceUpadte(v.base_id)
        end
    end
end



--desc:物品添加
--YQ.Qu:2017/2/24 0024
local function ItemAdd(tMsgData)
    if tMsgData ~= nil and tMsgData.add_list ~= nil then
        for k, v in pairs(tMsgData.add_list) do
            _list[v.uuid] = v;
            ItemResourceUpadte(v.base_id)
        end
    end
end

--desc:删除操作
--YQ.Qu:2017/2/24 0024
local function ItemDel(tMsgData)
    if tMsgData ~= nil and tMsgData.del_list ~= nil then
        for k, v in pairs(tMsgData.del_list) do
            LogWarn("[ItemMgr.ItemDel]被删除的物品uuID === " .. v);
            if _list[v] ~= nil then
                _list[v] = nil;
                ItemResourceUpadte(v.base_id)
            end
        end
    end
end

--desc:物品初始化
--YQ.Qu:2017/2/24 0024
local function ItemInit(tMsgData)
    if tMsgData ~= nil and tMsgData.all_list ~= nil then
        _list = {}
        for k, v in pairs(tMsgData.all_list) do
            _list[v.uuid] = v;
        end
    end
end

--desc:物品数量
--YQ.Qu:2017/2/24 0024
local function GetItemNum(baseId)
    local count = 0;

    for k, v in pairs(_list) do
        if v.base_id == baseId then
            count = count + v.count;
        end
    end

    return count;
end

local function GetBagBaseList()
    if #_bagBaseList ~= 0 then
        return _bagBaseList
    end
    for k, v in pairs(LuaConfigMgr.ItemBaseConfig) do
        if tonumber(v.cls) ~= 0 then
            _bagBaseList[#_bagBaseList + 1] = v;
        end
    end
    table.sort(_bagBaseList, function(a, b)
        return tonumber(a.key) < tonumber(b.key)
    end)

    return _bagBaseList
end

local function GetItemByKey(key)
    local baseId = tonumber(key);
    for k, v in pairs(_list) do
        if v.base_id == baseId then
            return v
        end
    end
    return nil
end

----------------------- 【邮件】


local function MailSort(a, b)
    if a.read ~= b.read then
        return a.read == false;
    else
        return a.receive_date > b.receive_date;
    end
end

--desc:邮件初始化更新
--YQ.Qu:2017/2/28 0028
local function MailsInitUpdate(tMsgData)
    if tMsgData == nil then return end
    _sysMails = tMsgData.sys_mails
    table.sort(_sysMails, MailSort);
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"mail")
end

local function SysMails()
    return _sysMails;
end


local function MailAdd(tMsgData)
    if tMsgData == nil then return end
    _sysMails[#_sysMails + 1] = tMsgData.add_sys_mail;
    table.sort(_sysMails, MailSort);
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"mail")
end

local function MailDel(mail_id)
    --[[if _sysMails[mail_id] ~= nil then
        _sysMails[mail_id] = nil;
    end]]
    for i = 1, #_sysMails do
        if _sysMails[i].mail_id == mail_id then
            table.remove(_sysMails, i);
            udpateRed = true
            triggerScriptEvent(UPDATE_MAIN_WIN_RED,"mail")
            return true
        end
    end
    return false;
end

--- 邮件红点数量
function M.mailRedCount()
    local count = 0;
    for i = 1, #_sysMails do
        if _sysMails[i].read == false or (_sysMails[i].reward_list ~= nil and #_sysMails[i].reward_list > 0) then
            count = count + 1
        end
    end
    return count
end


local function MailRead(mail_id)
    --[[if _sysMails[mail_id] ~= nil then
        _sysMails[mail_id].read = true;
    end]]
    local udpateRed = false
    for i = 1, #_sysMails do
        if _sysMails[i].mail_id == mail_id then
            _sysMails[i].read = true;
            udpateRed = true
            break
        end
    end

    table.sort(_sysMails, MailSort);
    if udpateRed then
        triggerScriptEvent(UPDATE_MAIN_WIN_RED,"mail")
    end
end

----------------------- 【商城】

--- desc:初始化商店物品表
-- YQ.Qu:2017/3/8 0008
local function SetShopList(tMsgData)
    --    LogWarn("[ItemMgr.SetShopList]---->item_list = "..#tMsgData.item_list);
    --    PrintTable(tMsgData);
    if tMsgData == nil then return end
    if tMsgData.item_list == nil then return end;

    _shopItemList = {}
    local len = #tMsgData.item_list;
    for i = 1, len do
        local v = tMsgData.item_list[i]
        if v ~= nil then
            if _shopItemList[v.shop_type] == nil then
                _shopItemList[v.shop_type] = {}
            end
            if v.shop_type < 4 or v.shop_type==8 then --商城
                _shopItemList[v.shop_type][v.sort] = v;
            else
                local key = v.id % 100;
                _shopItemList[v.shop_type][key] = v;
            end
        end
--        LogError("[ItemMgr.SetShopList]shop_type = "..v.shop_type.. " shop_id = "..v.id.." leftTime  = "..v.left_times);
    end
    

    if _shopItemList[5] ~= nil and #_shopItemList[5] > 0 then --- 首充
    -- LogWarn("[ItemMgr.SetShopList]首充的次数：".._shopItemList[5][1].left_times);
        triggerScriptEvent(EVENT_FIRST_PAY, _shopItemList[5][1].left_times);
    end
    if _shopItemList[7] ~= nil and #_shopItemList[7] > 0 then
        local diamondBagCtrl = IMPORT_MODULE("DiamondBagWinController")
        if diamondBagCtrl ~= nil and diamondBagCtrl.data.isInit then
            diamondBagCtrl.data:Update()
        end 
    end
end

--- desc:获取某一商店的数据列表
-- YQ.Qu:2017/3/8 0008
local function GetShopListByType(type)
    if _shopItemList[type] == nil then return {} end;
    return _shopItemList[type];
end
local function GetShopListByTypeWithoutZero(type)
    if _shopItemList[type] == nil then return {} end;
    local list ={}
    local index = 1
    for i=1,#_shopItemList[type] do
        if _shopItemList[type][i].id ~= 30013 or _shopItemList[type][i].left_times ~=0 then
            list[index] = _shopItemList[type][i]
            index=index+1
        end
    end 
    return list;
end
--- desc:更新商店里物品的限购次数
local function UpdateShopItem(tMsgData)
    if tMsgData == nil then
        return;
    end
    
    for k, v in pairs(_shopItemList) do
        for m, value in pairs(_shopItemList[k]) do
            if value.id == tMsgData.id then
                _shopItemList[k][m].left_times = tMsgData.left_times;
                if k == 5 and #_shopItemList[k] > 0 then --- 首充
                -- LogWarn("[ItemMgr.UpdateShopItem]首充。。。。。".._shopItemList[k][1].left_times);
                    triggerScriptEvent(EVENT_FIRST_PAY, _shopItemList[k][1].left_times);
                end
                return;
            end
        end
    end
end

--- 游戏中的快速获取商城数据
local function GetGameShopItem(key)
    local index = (key + 0) % 100;
    if _shopItemList[4] == nil then
        return nil
    end
    return _shopItemList[4][index] or nil;
end

------------------- 任务数据

local _taskList = {};
local function UpdateOneMissionList(key, mData)

    local list = _taskList[key];
    for i = 1, #list do
        if list[i].id == mData.id then
            list[i] = mData;
            break;
        end
    end
end

local function CreateOneMissionList(key, mData)
    if _taskList[key] == nil then _taskList[key] = {} end;
    --    local k = mData.id % 100;
    _taskList[key][#_taskList[key] + 1] = mData;
end
local function GetCurrentConfig(_tabIndex)
    if _tabIndex == 2 then
        return LuaConfigMgr.DailyTaskConfig;
    elseif _tabIndex == 3 then
        return LuaConfigMgr.WeekTaskConfig;
    elseif _tabIndex == 4 then
        return LuaConfigMgr.TimeTaskConfig;
    end
    return LuaConfigMgr.NewTaskConfig;
end
local function CheckIsDiamondMission(mData,dataindex,doId)
    if dataindex <=4 and dataindex >=1 then
        local missionCfg = GetCurrentConfig(dataindex)
        local missionData = missionCfg[tostring(mData.id)]
        if missionData ~= nil then
            if missionData.diamond_mark == "1" then
                mData.order_mark = missionData.order_mark
                if doId == 1 then
                    CreateOneMissionList(6, mData);
                elseif doId == 2 then
                    UpdateOneMissionList(6, mData);
                elseif doId == 3 then
                    if _taskList[6] ~= nil then
                        for i = 1, #_taskList[6] do
                            if _taskList[6][i].id == mData.id then
                                table.remove(_taskList[6], i);
                                break;
                            end
                        end
                    end
                end
            end
        end
    end
end

--- 初始化任务列表
local function InitMission(tMsgData)
    if tMsgData == nil or tMsgData.missions == nil then
        return;
    end
    _taskList = {}
    local redConditionCtrl = IMPORT_MODULE("RedConditionWinController")
    if redConditionCtrl~=nil and redConditionCtrl.Data.isInit== false then
        redConditionCtrl.Data:Init()
    end
    for i = 1, #tMsgData.missions do
        local mData = tMsgData.missions[i];
        if redConditionCtrl~=nil and mData.id>710000 then--红包牌局免费复活任务
            redConditionCtrl.Data:Update(mData)
            
        elseif mData.id < 510000 then
            if mData.id < 505000 then
                CreateOneMissionList(1, mData);
            end
            CheckIsDiamondMission(mData,1,1)
        elseif mData.id < 520000 then
            if mData.id > 511000 then
                CreateOneMissionList(5, mData);
            else
                CreateOneMissionList(2, mData);
                CheckIsDiamondMission(mData,2,1)
            end
        elseif mData.id < 530000 then
            CreateOneMissionList(3, mData);
            CheckIsDiamondMission(mData,3,1)
        elseif mData.id < 540000 then
            CreateOneMissionList(4, mData);
            CheckIsDiamondMission(mData,4,1)
        end
    end
    
    for i = 1, #_taskList do
        if _taskList[i] ~= nil and #_taskList[i] > 1 then
            if i==6 then
                table.sort(_taskList[i], function(a, b)
                    if a.state ==1 and b.state ~=1 then
                            return true
                    elseif a.state ~=1 and b.state == 1 then
                            return false
                    elseif  a.state ~= b.state then
                        return a.state < b.state
                    else
                        local idA = a.order_mark + 0;
                        local idB = b.order_mark + 0;
                        if idA == idB then
                            idA = a.id + 0;
                            idB = b.id + 0;
                            return idA < idB;
                        else
                            return idA > idB;
                        end
                    end
                end)
            else
                table.sort(_taskList[i], function(a, b)
                    if a.state ==1 and b.state ~=1 then
                            return true
                    elseif a.state ~=1 and b.state == 1 then
                            return false
                    elseif  a.state ~= b.state then
                        return a.state < b.state
                    else
                        local idA = a.id + 0;
                        local idB = b.id + 0;
                        return idA < idB;
                    end
                end)
            end
        end
    end
end


--- 更新单条任务
local function UpdateOneMission(tMsgData)
    if tMsgData == nil or tMsgData.mission_ == nil then
        return;
    end
    local mData = tMsgData.mission_;
    local redConditionCtrl = IMPORT_MODULE("RedConditionWinController")
    if redConditionCtrl~=nil and mData.id>710000 then--红包牌局免费复活任务
        
        redConditionCtrl.Data:Update(mData)
    elseif mData.id < 510000 then
        if mData.id < 505000 then
            UpdateOneMissionList(1, mData);
        end
        CheckIsDiamondMission(mData,1,2)
    elseif mData.id < 520000 then
        if mData.id > 511000 then
            UpdateOneMissionList(5, mData);
        else
            UpdateOneMissionList(2, mData);
            CheckIsDiamondMission(mData,2,2)
        end
    elseif mData.id < 530000 then
        UpdateOneMissionList(3, mData);
        CheckIsDiamondMission(mData,3,2)
    elseif mData.id < 540000 then
        UpdateOneMissionList(4, mData);
        CheckIsDiamondMission(mData,4,2)
    end
    for i = 1, #_taskList do
        if _taskList[i] ~= nil and #_taskList[i] > 1 then
            if i==6 then
                table.sort(_taskList[i], function(a, b)
                    if a.state ==1 and b.state ~=1 then
                            return true
                    elseif a.state ~=1 and b.state == 1 then
                            return false
                    elseif  a.state ~= b.state then
                        return a.state < b.state
                    else
                        local idA = a.order_mark + 0;
                        local idB = b.order_mark + 0;
                        if idA == idB then
                            idA = a.id + 0;
                            idB = b.id + 0;
                            return idA < idB;
                        else
                            return idA > idB;
                        end
                    end
                end)
            else
                table.sort(_taskList[i], function(a, b)
                    if a.state ==1 and b.state ~=1 then
                            return true
                    elseif a.state ~=1 and b.state == 1 then
                            return false
                    elseif  a.state ~= b.state then
                        return a.state < b.state
                    else
                        local idA = a.id + 0;
                        local idB = b.id + 0;
                        return idA < idB;
                    end
                end)
            end
        end
    end
end

--- 添加一条任务
local function AddOneMission(tMsgData)
    if tMsgData == nil or tMsgData.mission_ == nil then
        return;
    end
    local mData = tMsgData.mission_;
    if mData.id < 510000 then
        if mData.id < 505000 then
            CreateOneMissionList(1, mData);
        end
        CheckIsDiamondMission(mData,1,1)
    elseif mData.id < 520000 then
        if mData.id > 511000 then
            CreateOneMissionList(5, mData);
        else
            CreateOneMissionList(2, mData);
            CheckIsDiamondMission(mData,2,1)
        end
    elseif mData.id < 530000 then
        CreateOneMissionList(3, mData);
        CheckIsDiamondMission(mData,3,1)
    elseif mData.id < 540000 then
        CreateOneMissionList(4, mData);
        CheckIsDiamondMission(mData,4,1)
    end
end



--- desc:
-- YQ.Qu:2017/3/13 0013
local function DelOneMission(tMsgData)
    if tMsgData == nil or tMsgData.id == nil then
        return;
    end
    local id = tMsgData.id;
    local k = id % 100;
    local list;
    local dataIndex = 1
    if id < 510000 then
        if id >= 505000 then
            if _taskList[6] ~= nil then
                for i = 1, #_taskList[6] do
                    if _taskList[6][i].id == id then
                        table.remove(_taskList[6], i);
                        break;
                    end
                end
            end  
            return
        end
        list = _taskList[1];
        dataIndex = 1
    elseif id < 520000 then
        if id > 511000 then
            list = _taskList[5];
            dataIndex=5
        else
            list = _taskList[2];
            dataIndex=2
        end
    elseif id < 530000 then
        list = _taskList[3];
        dataIndex=3
    elseif id < 540000 then
        list = _taskList[4];
        dataIndex=4
    end

    if list ~= nil then
        for i = 1, #list do
            if list[i].id == id then
                CheckIsDiamondMission(list[i],dataIndex,3)
                table.remove(list, i);
                break;
            end
        end
    end
end

local function GetMissionListByType(type)
    return _taskList[type];
end


--- 某一类型的任务已经完成可领取有几个
local function GetTaskCompleteByType(type)
    local taskList = _taskList[type] or {}
    if #taskList == 0 then return 0 end
    local completeNum = 0;

    local currDate = os.date("*t", UtilTools.GetServerTime());


    for i = 1, #taskList do
        if type ~= 4 then
            if taskList[i] ~= nil and taskList[i].state == 1 then
                completeNum = completeNum + 1;
            end
        else
            --            LogWarn("[ItemMgr.GetTaskCompleteByType]id ===== >>>>" .. taskList[i].id);
            if taskList[i] ~= nil then
                local cfgData = LuaConfigMgr.TimeTaskConfig[taskList[i].id .. ""];
                if cfgData ~= nil then
                    --                LogWarn("[ItemMgr.GetTaskCompleteByType]taskList[i].id = " .. cfgData.title.."   "..cfgData.parameter1 .."   hour = "..currDate.hour);
                    local min = cfgData.parameter1 + 0;
                    local max = cfgData.parameter2 + 0;
                    if (currDate.hour >= min and currDate.hour < max) and taskList[i].state < 2 then
                        --                    LogWarn("[ItemMgr.GetTaskCompleteByType]"..cfgData.title);
                        completeNum = completeNum + 1;
                    end
                end
            end
        end
    end
    return completeNum;
end

--- 所有任务已经完成可领取有几个
local function GetTaskComplete()
    local num = 0;
    for i = 1, #_taskList do
        num = num + GetTaskCompleteByType(i);
    end

    return num;
end

M.ItemUpdate = ItemUpdate
M.ItemAdd = ItemAdd
M.ItemDel = ItemDel
M.ItemInit = ItemInit
M.getList = getList
M.GetItemNum = GetItemNum;
M.GetBagBaseList = GetBagBaseList;
M.GetItemByKey = GetItemByKey;


M.MailsInitUpdate = MailsInitUpdate
M.MailAdd = MailAdd
M.MailDel = MailDel
M.MailRead = MailRead
M.SysMails = SysMails


M.SetShopList = SetShopList;
M.GetShopListByTypeWithoutZero= GetShopListByTypeWithoutZero;
M.GetShopListByType = GetShopListByType;
M.UpdateShopItem = UpdateShopItem;


M.InitMission = InitMission;
M.GetMissionListByType = GetMissionListByType;
M.UpdateOneMission = UpdateOneMission;
M.AddOneMission = AddOneMission;
M.DelOneMission = DelOneMission;
M.GetTaskCompleteByType = GetTaskCompleteByType;
M.GetTaskComplete = GetTaskComplete;
M.GetGameShopItem = GetGameShopItem;


M.ClearAll = ClearAll
return M