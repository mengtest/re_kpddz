-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitSuperAwardWinMono.lua
-- * Summary:     超级奖励
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/26/2018 10:20:45 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitSuperAwardWinMono")



-- 界面名称
local wName = "FruitSuperAwardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _btnClose
local _tab1
local _tab2
local _itemTable={}
local _sortList={9,10,11,1,2,3,4,5,6,7,8}
local _newdesc1
--- [ALD END]

local _tabTable={}
local nowIndex=0


local function OnClickTab(gameObject)
    local comData=gameObject:GetComponent("ComponentData")
    if comData==nil then
        return
    end
    if nowIndex == comData.Id then
        _tabTable[nowIndex].value=true
        return
    end
    nowIndex = comData.Id
    _tabTable[nowIndex].value=true
end
local function GetContent(objTable,itemconfig)
    local showCount=0
    if tonumber(itemconfig.power2)~=0 then
        showCount=showCount+1
        objTable.countLabel[showCount].text = "2"
        objTable.valueLabel[showCount].text = itemconfig.power2
    end
    if tonumber(itemconfig.power3)~=0 then
        showCount=showCount+1
        objTable.countLabel[showCount].text = "3"
        objTable.valueLabel[showCount].text = itemconfig.power3
    end
    if tonumber(itemconfig.power4)~=0 then
        showCount=showCount+1
        objTable.countLabel[showCount].text = "4"
        objTable.valueLabel[showCount].text = itemconfig.power4
    end
    if tonumber(itemconfig.power5)~=0 then
        showCount=showCount+1
        objTable.countLabel[showCount].text = "5"
        objTable.valueLabel[showCount].text = itemconfig.power5
    end
    if showCount == 0 then
        objTable.grid.gameObject:SetActive(false)
    else
        objTable.grid.gameObject:SetActive(true)
        objTable.grid.maxPerLine = showCount
        for i=showCount+1,#objTable.countLabel do
            objTable.countLabel[i].gameObject:SetActive(false)
            objTable.valueLabel[i].gameObject:SetActive(false)
        end
    end
end
--- [ALF END]

local function UpdateAwardInfo()
    for i=1,#_itemTable do
        if i > #_sortList then
            return
        end
        local configData = LuaConfigMgr.SuperFruitConfig[tostring(_sortList[i])]
        if configData ~=nil then
            _itemTable[i].icon.spriteName=configData.icon
            GetContent(_itemTable[i],configData)
        end
        
    end
end


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/close")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _tabTable[1] = UnityTools.FindGo(gameObject.transform, "Container/tab/tab1"):GetComponent("UIToggle")
    UnityTools.AddOnClick(_tabTable[1].gameObject, OnClickTab)
    local data = _tabTable[1].gameObject:AddComponent("ComponentData")
    data.Id=1
    _tabTable[2] = UnityTools.FindGo(gameObject.transform, "Container/tab/tab2"):GetComponent("UIToggle")
    UnityTools.AddOnClick(_tabTable[2].gameObject, OnClickTab)
    local data = _tabTable[2].gameObject:AddComponent("ComponentData")
    data.Id=2
    for i=1,11 do
        _itemTable[i] ={}
        _itemTable[i].item = UnityTools.FindGo(gameObject.transform, "Container/awardinfo/item"..i)
        _itemTable[i].icon = UnityTools.FindGo(_itemTable[i].item.transform,"icon"):GetComponent("UISprite")
        _itemTable[i].grid = UnityTools.FindGo(_itemTable[i].item.transform, "nums"):GetComponent("UIGrid")
        _itemTable[i].countLabel={}
        _itemTable[i].valueLabel={}
        for j=1,4 do
            _itemTable[i].countLabel[j]=UnityTools.FindGo(_itemTable[i].grid.transform, "num"..j):GetComponent("UILabel")
            _itemTable[i].valueLabel[j]=UnityTools.FindGo(_itemTable[i].grid.transform, "value"..j):GetComponent("UILabel")
        end
    end
    
    _newdesc1 = UnityTools.FindGo(gameObject.transform, "Container/awardinfo/item3/icon/desc1"):GetComponent("UILabel")

--- [ALB END]




end

local function Awake(gameObject)
       -- Lua Editor 自动绑定
       AutoLuaBind(gameObject)
       UpdateAwardInfo()
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg)
    OnClickTab(_tabTable[1].gameObject)
    _newdesc1.text = LuaText.GetString("fruit_award_info7")
    
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("FruitSuperAwardWinMono")
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
