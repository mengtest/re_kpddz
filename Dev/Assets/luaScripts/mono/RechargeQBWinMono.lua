-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargeQBWinMono.lua
-- * Summary:     RechargeQBWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/5/2018 2:03:17 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeQBWinMono")



-- 界面名称
local wName = "RechargeQBWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnSure
local _phoneInput
local _btnClose
local _nameLb
local _descLb
--- [ALD END]





local function OnClickSure(gameObject)
    if _phoneInput.value == "" then
        UnityTools.ShowMessage(LuaText.GetString("exchange_desc2"));
        return;
    end
    local req = {}
    req.phone_number = _phoneInput.value
    req.obj_id = CTRL.obj_id;
    req.phone_card_charge_type = 1; --1表示直充
    req.address_id = 1;
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_prize_exchange_req, req);
end

--- [ALF END]




local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, OnClickSure)
    _phoneInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/phone/inputBg")
    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _nameLb = UnityTools.FindGo(gameObject.transform, "Container/name"):GetComponent("UILabel")

    _descLb = UnityTools.FindGo(gameObject.transform, "Container/desc"):GetComponent("UILabel")


--- [ALB END]




end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    _phoneInput.defaultText = LuaText.GetString("exchange_desc2");
    if CTRL.saveData ~= nil then
        _nameLb.text = CTRL.saveData.name
        _descLb.text = CTRL.saveData.dsc
    end
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("RechargeQBWinMono")
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
