-- -----------------------------------------------------------------


-- *
-- * Filename:    OpenBoxWinController.lua
-- * Summary:     打开宝箱界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 4:28:20 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("OpenBoxWinController")



-- 界面名称
local wName = "OpenBoxWin"

-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();
M.LeftTimes={}
M.LeftTimes[1]=0
M.LeftTimes[2]=0
M.LeftTimes[3]=0
M.LeftTimes[4]=0
M.WinType = 0
M.Process = 0  
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.WinType = 0
    M.Process = 0 
end
function OnChestTimesUpdate(msgId, tMsgData)
    if tMsgData ~= nil then
        if tMsgData.update_type == 0 then
            M.LeftTimes[1] = tMsgData.times_niu
            M.LeftTimes[2] = tMsgData.times_hundred
            M.LeftTimes[3] = tMsgData.times_laba
        else
            M.LeftTimes[tMsgData.update_type] = tMsgData.times_niu
        end
       
    end
end
protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_room_chest_times_update, "OnChestTimesUpdate")


UI.Controller.UIManager.RegisterLuaWinFunc("OpenBoxWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
