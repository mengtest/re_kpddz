-- --------------------------------------------------


-- *
-- * Summary: 	动态LuaMono配置表，用于代码中动态绑定lua脚本
-- * Version: 	1.0.0
-- * Author: 	WP.Chu
-- --------------------------------------------------


-- *************** 配置说明 *****************
--
-- 格式: { [key] = script }
-- key： 脚本绑定的GameObject名字，注意要唯一
-- script： 绑定的脚本，同make
--
-- *****************************************


-- 配置表（全局唯一，C#中使用）
___dynamic_lua_mono_table___ = {
	["MyDynamicLuaWindow"] = "luaScripts/dynamicLuaMono.lua",
}