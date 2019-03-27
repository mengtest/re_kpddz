-- -----------------------------------------------------------------


-- *
-- * Filename:    UserIDCardBindWinMono.lua
-- * Summary:     UserIDCardBindWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/14/2018 3:54:27 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("UserIDCardBindWinMono")



-- 界面名称
local wName = "UserIDCardBindWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnClose
local _btnSure
local _inputName
local _inputID
--- [ALD END]


local string_len = string.len
local tonumber = tonumber

-- // wi =2(n-1)(mod 11) 
local wi = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 }; 
-- // verify digit 
local vi= { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' }; 

local function isBirthDate(date)
    local year = tonumber(date:sub(1,4))
    local month = tonumber(date:sub(5,6))
    local day = tonumber(date:sub(7,8))
    if year < 1900 or year > 2100 or month >12 or month < 1 then
        return false
    end
    -- //月份天数表
    local month_days = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    local bLeapYear = (year % 4 == 0 and year % 100 ~= 0) or (year % 400 == 0)
    if bLeapYear  then
        month_days[2] = 29;
    end

    if day > month_days[month] or day < 1 then
        return false
    end

    return true
end

local function isAllNumberOrWithXInEnd( str )
    local ret = str:match("%d+X?") 
    return ret == str 
end


local function checkSum(idcard)
    local nums = {}
    local _idcard = idcard:sub(1,17)
    for ch in _idcard:gmatch"." do
        table.insert(nums,tonumber(ch))
    end
    local sum = 0
    for i,k in ipairs(nums) do
        sum = sum + k * wi[i]
    end

    return vi [sum % 11+1] == idcard:sub(18,18)
end


local err_success = 0
local err_length = 1
local err_province = 2
local err_birth_date = 3
local err_code_sum = 4
local err_unknow_charactor = 5

local function verifyIDCard(idcard)
    if string_len(idcard) ~= 18 then
        return err_length
    end

    if not isAllNumberOrWithXInEnd(idcard) then
        return err_unknow_charactor
    end
    -- //第1-2位为省级行政区划代码，[11, 65] (第一位华北区1，东北区2，华东区3，中南区4，西南区5，西北区6)
    local nProvince = tonumber(idcard:sub(1, 2))
    if( nProvince < 11 or nProvince > 65 ) then
        return err_province
    end

    -- //第3-4为为地级行政区划代码，第5-6位为县级行政区划代码因为经常有调整，这块就不做校验

    -- //第7-10位为出生年份；//第11-12位为出生月份 //第13-14为出生日期
    if not isBirthDate(idcard:sub(7,14)) then
        return err_birth_date
    end

    if not checkSum(idcard) then
        return err_code_sum
    end

    return err_success
end

local function isHanZi(str)
    if str == nil or str == "" then
        return
    end

    for ch in string.gmatch(str, "[%z\1-\x7F\xC2-\xF4][\x80-\xBF]*") do
        if #ch == 1 then 
            return false
        end
    end

    return true
end


local function OnClickSureBtn(gameObject)
    local NameText = _inputName.value
    local IDCardText = _inputID.value
    if NameText == nil or NameText == "" or not isHanZi(NameText) then
        UnityTools.MessageDialog(GameText.GetStr("请输入中文名称！"))
        return
    end

    if IDCardText == nil or IDCardText == "" or verifyIDCard(IDCardText) ~= err_success then
        UnityTools.MessageDialog(GameText.GetStr("请输入18位身份证号码！"))
        return
    end

    UtilTools.ShowWaitFlag()

    local protobuf = sluaAux.luaProtobuf.getInstance()
    local tMsg = {}
    tMsg.id_card_num = IDCardText
    tMsg.name = NameText
    protobuf:sendMessage(protoIdSet.cs_real_name_update, tMsg)
end

--- [ALF END]




local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

function OnUpdateRealNameWinClose()
    UnityTools.DestroyWin(wName)
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnClickSureBtn)

    _inputName = UnityTools.FindGo(gameObject.transform, "Container/phone/Sprite"):GetComponent("UIInput")

    _inputID = UnityTools.FindGo(gameObject.transform, "Container/password/Sprite"):GetComponent("UIInput")

--- [ALB END]
    _inputName.defaultText = "请输入中文名称！"
    _inputID.defaultText = "请输入18位身份证号码！"
    registerScriptEvent(EVENT_UPDATE_PLAYER_WIN_INFO, "OnUpdateRealNameWinClose");

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)

end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_UPDATE_PLAYER_WIN_INFO, "OnUpdateRealNameWinClose");
    CLEAN_MODULE("UserIDCardBindWinMono")
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
