-- --------------------------------------------------


-- *
-- * Summary: 	脚本加载和编译
-- * Version: 	1.0.0
-- * Author: 	WP.Chu
-- --------------------------------------------------


-- 所有的非绑定脚本
-- 加载顺序：按照表中的脚本顺序加载

___all_unbind_scripts___ = {

    --脚本设置
    "luaScripts/luaConfig.lua",

	-- 配置模块
    "luaScripts/config/luaTextConfig.lua",
    "luaScripts/config/eventConfig.lua",
	"luaScripts/config/dynamicLuaMonoConfig.lua",
	"luaScripts/config/protoMsgDef.lua",
    "luaScripts/config/LuaWinConfig.lua",
    "luaScripts/config/luaConfigMgr.lua",
    "luaScripts/config/activityAndAnnouncementConfig.lua",

    -- 基础模块
    "luaScripts/basicModules/basicFunc.lua",
    "luaScripts/extension.lua",
    "luaScripts/basicModules/timer.lua",
    "luaScripts/basicModules/unityTools.lua",
    "luaScripts/basicModules/eventManager.lua",
    
	-- 物品
	--"luaScripts/item.lua",
	"luaScripts/poker/ItemMgr.lua",

    -- 平台管理器
    "luaScripts/poker/PlatformMgr.lua",
    -- 房间管理器
    "luaScripts/poker/roomMgr.lua",
    -- 扑克管理器
    "luaScripts/poker/PokerMgr.lua",
    -- 游戏管理器
    "luaScripts/poker/GameMgr.lua",

    -- 垃圾回收
    "luaScripts/basicModules/collectMem.lua",

    -- 主函数
    "luaScripts/main.lua",
}
