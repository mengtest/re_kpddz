-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerInfoTipsWinMono.lua
-- * Summary:     人物界面上的物品Tip
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/27/2017 10:52:36 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerInfoTipsWinMono")



-- 界面名称
local wName = "PlayerInfoTipsWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnClose
local _item
local _tittleLb
local _content
--- [ALD END]


local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnEventSetContent(args)
    if args.go ~= nil then
        local posX = args.posX or 0;
        local posY = args.posY or 0;
        local itemPosition = args.go.transform.position;
        _item.transform.position = UnityEngine.Vector3(itemPosition.x+posX, itemPosition.y +posY, 0);

        local isShowTitle  = args.isShowTitle or false;
        _tittleLb.gameObject:SetActive(isShowTitle)
        if isShowTitle then
            _tittleLb.text = args.title or "";
        else
            _content.transform.localPosition = UnityEngine.Vector3(_content.transform.localPosition.x,_tittleLb.transform.localPosition.y,0)
        end
        local contentColor = args.contentColor or "";
        _content.text = contentColor..args.desc.."[-]" or "";
    end
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnClose = UnityTools.FindGo(gameObject.transform, "BtnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _item = UnityTools.FindGo(gameObject.transform, "Container")

        _tittleLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/tittle")

    _content = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/content")

--- [ALB END]


end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    --    _controller.RegisterUIEvent(1, OnEventSetContent);
end


local function Start(gameObject)
end


local function OnDestroy(gameObject)
    CTRL.ClearAll()
    CLEAN_MODULE(wName .. "Mono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEventSetContent = OnEventSetContent


-- 返回当前模块
return M
