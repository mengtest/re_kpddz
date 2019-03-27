-- -----------------------------------------------------------------


-- *
-- * Filename:    MainCenterWinController.lua
-- * Summary:     游戏主界面场景
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/13/2017 4:15:17 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainCenterWinController")
local PlatformMg = IMPORT_MODULE("PlatformMgr");
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 界面名称
local wName = "MainCenterWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

--desc:更新玩家数据
--YQ.Qu:2017/2/17 0017
local function OnPlayerInfoUpdate(gameoObject)
--    PlatformMg
--    LogWarn("   player data update = ".. GameDataMgr.PLAYER_DATA.UserName)
    local updateFree = false;
    if GameDataMgr.PLAYER_DATA.IsTouris ~= PlatformMg.IsTouris() then
        updateFree = true;
    end
    PlatformMg.UpdatePlayerInfo();
    local va = 1
    triggerScriptEvent(EVENT_RESCOURCE_UDPATE,va)

    if updateFree then
        triggerScriptEvent(UPDATE_MAIN_WIN_RED,"free")
    end
end

---desc:
---YQ.Qu
function OnGuideInfoUpdateMainCenter(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    PlatformMg.SetGuideStep(tMsgData.step_id);
end

--desc:清理所有数据
--YQ.Qu:2017/2/17 0017
local function OnClearAllData(gameObject)
    PlatformMg.Config:ResetBlurCnt();
    PlatformMg.ClearAll();
end

local function OnOpenGuide(go)
    PlatformMg.Config.isOpenGuide = true;
 end

 function MainCenterRenderQ(go)
     local mono = IMPORT_MODULE("MainCenterWinMono")
     if mono ~= nil then
         mono.ResetEffectRenderQ(go)
     end
  end


UI.Controller.UIManager.RegisterLuaWinFunc("MainCenterWin", OnCreateCallBack, OnDestoryCallBack)
UI.Controller.UIManager.RegisterLuaFuncCall("event_resource_update_from_csharp", OnPlayerInfoUpdate)
UI.Controller.UIManager.RegisterLuaFuncCall("event_clear_all_data_from_csharp", OnClearAllData)
UI.Controller.UIManager.RegisterLuaFuncCall("event_open_guide", OnOpenGuide)
UI.Controller.UIManager.RegisterLuaWinRenderFunc("MainCenterWin", MainCenterRenderQ)

protobuf:registerMessageScriptHandler(protoIdSet.sc_guide_info_update,"OnGuideInfoUpdateMainCenter")

-- 返回当前模块
return M
