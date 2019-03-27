-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitBoxWinMono.lua
-- * Summary:     FruitBoxWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        4/7/2017 3:44:18 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitBoxWinMono")



-- 界面名称
local wName = "FruitBoxWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _platformMgr = IMPORT_MODULE("PlatformMgr")

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local FruitCTRL = IMPORT_MODULE("FruitWinController")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _taskObj = {}
local _btnClose
--- [ALD END]



--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local function OnClickGet(gameObject)
    local protobuf = sluaAux.luaProtobuf.getInstance()
    local req ={}
    req.game_type = 2
    req.box = gameObject:GetComponent("ComponentData").Id
    protobuf:sendMessage(protoIdSet.cs_game_task_box_draw_req,req)
end
local function UpdateWin()
    local boxConfig = LuaConfigMgr.FruitChestConfig[tostring(FruitCTRL.BoxTable.vip_level)]
    if boxConfig ==nil or FruitCTRL.BoxTable.boxStart==nil then
        return
    end
    for i=1,3 do
        _taskObj[i].num.text = boxConfig["reward_gold"..i]
        LogError(i.."="..FruitCTRL.BoxTable.boxStatus[i])
        if i <= #FruitCTRL.BoxTable.boxStatus then
            if FruitCTRL.BoxTable.boxStatus[i] == 0 then
                _taskObj[i].lbbtn.text = LuaText.GetString("mail_desc2")
                UtilTools.SetGray(_taskObj[i].btn.gameObject,true,false)
                _taskObj[i].desc.text = LuaText.Format("fruit_box_tip1",FruitCTRL.BoxTable.boxProcess.."/"..(i*2+1))
                _taskObj[i].col.enabled=false
            elseif FruitCTRL.BoxTable.boxStatus[i] == 1 then
                _taskObj[i].lbbtn.text = LuaText.GetString("mail_desc2")
                UtilTools.RevertGray(_taskObj[i].btn.gameObject,true,false)
                _taskObj[i].desc.text = LuaText.Format("fruit_box_tip1",(i*2+1))
                _taskObj[i].col.enabled=true
            else
                _taskObj[i].lbbtn.text = LuaText.GetString("fruit_box_tip7")
                UtilTools.SetGray(_taskObj[i].btn.gameObject,true,false)
                _taskObj[i].desc.text = LuaText.Format("fruit_box_tip1",(i*2+1))
                _taskObj[i].col.enabled=false
            end
        end
    end
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
   
    for i=1,3 do
        _taskObj[i]={}
        _taskObj[i].btn = UnityTools.FindGo(gameObject.transform, "WinBox/task"..i.."/OKButton")
        _taskObj[i].lbbtn = UnityTools.FindGo(gameObject.transform, "WinBox/task"..i.."/OKButton/Label"):GetComponent("UILabel")
        _taskObj[i].desc = UnityTools.FindGo(gameObject.transform, "WinBox/task"..i.."/desc"):GetComponent("UILabel")
        _taskObj[i].num = UnityTools.FindGo(gameObject.transform, "WinBox/task"..i.."/num"):GetComponent("UILabel")
        _taskObj[i].col = _taskObj[i].btn:GetComponent("BoxCollider") 
        UnityTools.AddOnClick(_taskObj[i].btn.gameObject, OnClickGet)
        local comData = _taskObj[i].btn.gameObject:AddComponent("ComponentData")
        comData.Id=i
    end

    _btnClose = UnityTools.FindGo(gameObject.transform, "WinBox/CloseButton")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

--- [ALB END]


end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end
function FruitTaskUpdate()
    UpdateWin()
end

local function Start(gameObject)
    UpdateWin()
    registerScriptEvent(EVENT_LABA_GOLD_UPDATE, "FruitTaskUpdate")
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_LABA_GOLD_UPDATE, "FruitTaskUpdate")
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
