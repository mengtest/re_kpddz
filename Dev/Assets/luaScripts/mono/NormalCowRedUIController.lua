-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowRedUIController.lua
-- * Summary:     看牌红包UI
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/9/2017 11:23:30 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowRedUIController")

local protobuf = sluaAux.luaProtobuf.getInstance();

-- 界面名称
local wName = "NormalCowRedUI"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
M.poolNum = 0


local function OnCreateCallBack(gameObject)
    -- gameObject:SetActive(true)
end


local function OnDestoryCallBack(gameObject)

end

function OnFuDaiUpdateReply(msgId, tMsgData)
    if tMsgData ~= nil then
        if tMsgData.next_open_redpack_second == 99 then 
            M.poolNum  = tMsgData.close_draw_second
            triggerScriptEvent(COW_RED_PACK_UPDATE);
        end
    end
end
protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_room_redpack_notice_update, "OnFuDaiUpdateReply")
UI.Controller.UIManager.RegisterLuaWinFunc("NormalCowRedUI", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
