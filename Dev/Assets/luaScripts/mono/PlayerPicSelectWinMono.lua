-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerPicSelectWinMono.lua
-- * Summary:     选择头像来源
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/3/2017 5:07:40 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerPicSelectWinMono")



-- 界面名称
local wName = "PlayerPicSelectWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnAlbum
local _btnPhotograph
local _btnCancel
--- [ALD END]



--- 选择手机相册
local function OnSelectAlbumHandler(gameObject)
    UtilTools.PickPhotoFormAlbum("png",true,120,120,"UIRoot");
end

--- 选择拍照
local function OnPhotoGraphHandler(gameObject)
    UtilTools.pickPhotoFromCamera("png",true,120,120,"UIRoot");
end

--- 取消
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName);
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnAlbum = UnityTools.FindGo(gameObject.transform, "Container/btnalbum")
    UnityTools.AddOnClick(_btnAlbum.gameObject, OnSelectAlbumHandler)

    _btnPhotograph = UnityTools.FindGo(gameObject.transform, "Container/btnPhotograph")
    UnityTools.AddOnClick(_btnPhotograph.gameObject, OnPhotoGraphHandler)

    _btnCancel = UnityTools.FindGo(gameObject.transform, "Container/btnCancel")
    UnityTools.AddOnClick(_btnCancel.gameObject, OnCloseHandler)

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("PlayerPicSelectWinMono")
end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
