-- -----------------------------------------------------------------


-- *
-- * Filename:    ShopWinController.lua
-- * Summary:     商城界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/8/2017 9:58:22 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShopWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local _itemMgr = IMPORT_MODULE("ItemMgr");
local UnityTools = IMPORT_MODULE("UnityTools");


-- 界面名称
local wName = "ShopWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local closeOpenOtherWin;
local ctrlData = {
    startTab = 1
}

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

--- desc:
-- YQ.Qu:2017/3/8 0008
function OnShopAllItemBaseConfig(msgId, tMsgData)
    --    LogWarn("[ShopWinController.OnShopAllItemBaseConfig]商店数据更新下来了。。。。");
    _itemMgr.SetShopList(tMsgData);
    if UI.Controller.UIManager.IsWinShow("ShopWin") then
        local mono = IMPORT_MODULE("ShopWinMono");
        if mono ~= nil then
            mono.UpdateWin();
        end
    end
    
    local diamondBagCtrl = IMPORT_MODULE("DiamondBagWinController")
    if diamondBagCtrl ~= nil and diamondBagCtrl.data.isInit==false then
        diamondBagCtrl.data:Init()
    end
end

function OnShopBuyReply(msgId, tMsgData)
    if tMsgData ~= nil then
        if tMsgData.result == 0 then
            if #tMsgData.rewards > 0 then
                ShowAwardWin(tMsgData.rewards);
                _itemMgr.UpdateShopItem(tMsgData)
                if tMsgData.id == 50001 and tMsgData.left_times == 0 then
                    UnityTools.DestroyWin("FirstPayWin");
                    triggerScriptEvent(EVENT_FIRST_PAY, tMsgData.left_times);
                end

                if tMsgData.id == 70001 or tMsgData.id == 70002 then --钻石福袋
                    local diamondBagCtrl = IMPORT_MODULE("DiamondBagWinController")
                    if diamondBagCtrl ~= nil then
                        diamondBagCtrl.data:BuySucc()
                    end
                end
                triggerScriptEvent(EVENT_SHOP_BUY_UPDATE, {});
            else
                UnityTools.ShowMessage(LuaText.GetString("shop_buy_succ"));
            end

        else
            UnityTools.ShowMessage(tMsgData.err_msg);
        end
        if UI.Controller.UIManager.IsWinShow("ShopWin") then
            local mono = IMPORT_MODULE("ShopWinMono");
            if mono ~= nil then
                mono.UpdateWin();
            end
        end
    end
end

--- desc:打开商城
-- @param tab 打开对应商城的Tab（默认为1）
local function OpenShop(tab)
    tab = tab or 1;
    ctrlData.startTab = tab;
    UnityTools.CreateLuaWin(wName);
end


UI.Controller.UIManager.RegisterLuaWinFunc("ShopWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_shop_all_item_base_config, "OnShopAllItemBaseConfig")
protobuf:registerMessageScriptHandler(protoIdSet.sc_shop_buy_reply, "OnShopBuyReply");

M.OpenShop = OpenShop;
M.CtrlData = ctrlData;
M.closeOpenOtherWin = closeOpenOtherWin;
-- 返回当前模块
return M
