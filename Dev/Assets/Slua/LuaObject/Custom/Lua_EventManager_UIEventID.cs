using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EventManager_UIEventID : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HERO_LIST_UPDATE_HEROS(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.HERO_LIST_UPDATE_HEROS);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HERO_LIST_UPDATE_HEROS(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.HERO_LIST_UPDATE_HEROS=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MESSAGE_DIALOG_SET_TEXT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.MESSAGE_DIALOG_SET_TEXT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MESSAGE_DIALOG_SET_TEXT(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.MESSAGE_DIALOG_SET_TEXT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LIFE_POINT_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.LIFE_POINT_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LIFE_POINT_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.LIFE_POINT_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SHOP_BUY_RESULT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.SHOP_BUY_RESULT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SHOP_BUY_RESULT(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.SHOP_BUY_RESULT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LOTTERY_GET_HERO(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.LOTTERY_GET_HERO);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LOTTERY_GET_HERO(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.LOTTERY_GET_HERO=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_STAR_UPGRADE_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.STAR_UPGRADE_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_STAR_UPGRADE_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.STAR_UPGRADE_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PHASE_UPGRADE_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.PHASE_UPGRADE_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PHASE_UPGRADE_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.PHASE_UPGRADE_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SKILL_TIP_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.SKILL_TIP_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SKILL_TIP_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.SKILL_TIP_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SKILL_TIP_CLOSE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.SKILL_TIP_CLOSE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SKILL_TIP_CLOSE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.SKILL_TIP_CLOSE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HERO_STONE_WIN_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.HERO_STONE_WIN_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HERO_STONE_WIN_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.HERO_STONE_WIN_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TIPS(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.TIPS);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_TIPS(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.TIPS=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MESSAGE_WIN_SET_TEXT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.MESSAGE_WIN_SET_TEXT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MESSAGE_WIN_SET_TEXT(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.MESSAGE_WIN_SET_TEXT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CREATE_WIN_ACTION(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.CREATE_WIN_ACTION);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CREATE_WIN_ACTION(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.CREATE_WIN_ACTION=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DESTROY_WIN_ACTION(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.DESTROY_WIN_ACTION);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DESTROY_WIN_ACTION(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.DESTROY_WIN_ACTION=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ARENA_CLICK_RIVAL(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.ARENA_CLICK_RIVAL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ARENA_CLICK_RIVAL(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.ARENA_CLICK_RIVAL=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ARENA_LONG_PRESS_RIVAL(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.ARENA_LONG_PRESS_RIVAL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ARENA_LONG_PRESS_RIVAL(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.ARENA_LONG_PRESS_RIVAL=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ARENA_PRESS_CANCEL_RIVAL(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.ARENA_PRESS_CANCEL_RIVAL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ARENA_PRESS_CANCEL_RIVAL(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.ARENA_PRESS_CANCEL_RIVAL=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ACTIVITY_WIN_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.ACTIVITY_WIN_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ACTIVITY_WIN_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.ACTIVITY_WIN_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MAIN_TOP_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.MAIN_TOP_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MAIN_TOP_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.MAIN_TOP_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LOTTERY_CUP_BOOM_BEGIN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.LOTTERY_CUP_BOOM_BEGIN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LOTTERY_CUP_BOOM_BEGIN(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.LOTTERY_CUP_BOOM_BEGIN=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RESULT_WIN_BACK_TO_LOTERRY(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.RESULT_WIN_BACK_TO_LOTERRY);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RESULT_WIN_BACK_TO_LOTERRY(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.RESULT_WIN_BACK_TO_LOTERRY=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HIDE_WIN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.HIDE_WIN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HIDE_WIN(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.HIDE_WIN=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SHOW_WIN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.SHOW_WIN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SHOW_WIN(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.SHOW_WIN=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LOTTERY_HERO_RUN_TO_TARGET(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.LOTTERY_HERO_RUN_TO_TARGET);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LOTTERY_HERO_RUN_TO_TARGET(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.LOTTERY_HERO_RUN_TO_TARGET=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_COPY_STARS_ACTION_START(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.COPY_STARS_ACTION_START);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_COPY_STARS_ACTION_START(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.COPY_STARS_ACTION_START=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UPDATE_COPY_MAP(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.UPDATE_COPY_MAP);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_UPDATE_COPY_MAP(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.UPDATE_COPY_MAP=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_COPY_CLICK_BIG_NODE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.COPY_CLICK_BIG_NODE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_COPY_CLICK_BIG_NODE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.COPY_CLICK_BIG_NODE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_COPY_PVP_UPDATE_ALL_AREA(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.COPY_PVP_UPDATE_ALL_AREA);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_COPY_PVP_UPDATE_ALL_AREA(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.COPY_PVP_UPDATE_ALL_AREA=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_COPY_PVP_UPDATE_ONE_AREA(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.COPY_PVP_UPDATE_ONE_AREA);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_COPY_PVP_UPDATE_ONE_AREA(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.COPY_PVP_UPDATE_ONE_AREA=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_COPY_PVP_UPDATE_ONE_JOB(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.COPY_PVP_UPDATE_ONE_JOB);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_COPY_PVP_UPDATE_ONE_JOB(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.COPY_PVP_UPDATE_ONE_JOB=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UPDATE_GUIDE_DATA(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.UPDATE_GUIDE_DATA);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_UPDATE_GUIDE_DATA(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.UPDATE_GUIDE_DATA=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PHASE_EQUIP_UPGRADE_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.PHASE_EQUIP_UPGRADE_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PHASE_EQUIP_UPGRADE_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.PHASE_EQUIP_UPGRADE_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SET_MESSAGE_WITH_ICON(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.SET_MESSAGE_WITH_ICON);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SET_MESSAGE_WITH_ICON(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.SET_MESSAGE_WITH_ICON=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LIMIT_BUY_RESULT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.LIMIT_BUY_RESULT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LIMIT_BUY_RESULT(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.LIMIT_BUY_RESULT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LIMIT_HERO_BUY_RESULT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.LIMIT_HERO_BUY_RESULT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LIMIT_HERO_BUY_RESULT(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.LIMIT_HERO_BUY_RESULT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SHOW_HELP_SURE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.SHOW_HELP_SURE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SHOW_HELP_SURE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.SHOW_HELP_SURE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MAIN_CITY_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.UIEventID.MAIN_CITY_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MAIN_CITY_UPDATE(IntPtr l) {
		try {
			System.Int16 v;
			checkType(l,2,out v);
			EventManager.UIEventID.MAIN_CITY_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventManager.UIEventID");
		addMember(l,"HERO_LIST_UPDATE_HEROS",get_HERO_LIST_UPDATE_HEROS,set_HERO_LIST_UPDATE_HEROS,false);
		addMember(l,"MESSAGE_DIALOG_SET_TEXT",get_MESSAGE_DIALOG_SET_TEXT,set_MESSAGE_DIALOG_SET_TEXT,false);
		addMember(l,"LIFE_POINT_UPDATE",get_LIFE_POINT_UPDATE,set_LIFE_POINT_UPDATE,false);
		addMember(l,"SHOP_BUY_RESULT",get_SHOP_BUY_RESULT,set_SHOP_BUY_RESULT,false);
		addMember(l,"LOTTERY_GET_HERO",get_LOTTERY_GET_HERO,set_LOTTERY_GET_HERO,false);
		addMember(l,"STAR_UPGRADE_UPDATE",get_STAR_UPGRADE_UPDATE,set_STAR_UPGRADE_UPDATE,false);
		addMember(l,"PHASE_UPGRADE_UPDATE",get_PHASE_UPGRADE_UPDATE,set_PHASE_UPGRADE_UPDATE,false);
		addMember(l,"SKILL_TIP_UPDATE",get_SKILL_TIP_UPDATE,set_SKILL_TIP_UPDATE,false);
		addMember(l,"SKILL_TIP_CLOSE",get_SKILL_TIP_CLOSE,set_SKILL_TIP_CLOSE,false);
		addMember(l,"HERO_STONE_WIN_UPDATE",get_HERO_STONE_WIN_UPDATE,set_HERO_STONE_WIN_UPDATE,false);
		addMember(l,"TIPS",get_TIPS,set_TIPS,false);
		addMember(l,"MESSAGE_WIN_SET_TEXT",get_MESSAGE_WIN_SET_TEXT,set_MESSAGE_WIN_SET_TEXT,false);
		addMember(l,"CREATE_WIN_ACTION",get_CREATE_WIN_ACTION,set_CREATE_WIN_ACTION,false);
		addMember(l,"DESTROY_WIN_ACTION",get_DESTROY_WIN_ACTION,set_DESTROY_WIN_ACTION,false);
		addMember(l,"ARENA_CLICK_RIVAL",get_ARENA_CLICK_RIVAL,set_ARENA_CLICK_RIVAL,false);
		addMember(l,"ARENA_LONG_PRESS_RIVAL",get_ARENA_LONG_PRESS_RIVAL,set_ARENA_LONG_PRESS_RIVAL,false);
		addMember(l,"ARENA_PRESS_CANCEL_RIVAL",get_ARENA_PRESS_CANCEL_RIVAL,set_ARENA_PRESS_CANCEL_RIVAL,false);
		addMember(l,"ACTIVITY_WIN_UPDATE",get_ACTIVITY_WIN_UPDATE,set_ACTIVITY_WIN_UPDATE,false);
		addMember(l,"MAIN_TOP_UPDATE",get_MAIN_TOP_UPDATE,set_MAIN_TOP_UPDATE,false);
		addMember(l,"LOTTERY_CUP_BOOM_BEGIN",get_LOTTERY_CUP_BOOM_BEGIN,set_LOTTERY_CUP_BOOM_BEGIN,false);
		addMember(l,"RESULT_WIN_BACK_TO_LOTERRY",get_RESULT_WIN_BACK_TO_LOTERRY,set_RESULT_WIN_BACK_TO_LOTERRY,false);
		addMember(l,"HIDE_WIN",get_HIDE_WIN,set_HIDE_WIN,false);
		addMember(l,"SHOW_WIN",get_SHOW_WIN,set_SHOW_WIN,false);
		addMember(l,"LOTTERY_HERO_RUN_TO_TARGET",get_LOTTERY_HERO_RUN_TO_TARGET,set_LOTTERY_HERO_RUN_TO_TARGET,false);
		addMember(l,"COPY_STARS_ACTION_START",get_COPY_STARS_ACTION_START,set_COPY_STARS_ACTION_START,false);
		addMember(l,"UPDATE_COPY_MAP",get_UPDATE_COPY_MAP,set_UPDATE_COPY_MAP,false);
		addMember(l,"COPY_CLICK_BIG_NODE",get_COPY_CLICK_BIG_NODE,set_COPY_CLICK_BIG_NODE,false);
		addMember(l,"COPY_PVP_UPDATE_ALL_AREA",get_COPY_PVP_UPDATE_ALL_AREA,set_COPY_PVP_UPDATE_ALL_AREA,false);
		addMember(l,"COPY_PVP_UPDATE_ONE_AREA",get_COPY_PVP_UPDATE_ONE_AREA,set_COPY_PVP_UPDATE_ONE_AREA,false);
		addMember(l,"COPY_PVP_UPDATE_ONE_JOB",get_COPY_PVP_UPDATE_ONE_JOB,set_COPY_PVP_UPDATE_ONE_JOB,false);
		addMember(l,"UPDATE_GUIDE_DATA",get_UPDATE_GUIDE_DATA,set_UPDATE_GUIDE_DATA,false);
		addMember(l,"PHASE_EQUIP_UPGRADE_UPDATE",get_PHASE_EQUIP_UPGRADE_UPDATE,set_PHASE_EQUIP_UPGRADE_UPDATE,false);
		addMember(l,"SET_MESSAGE_WITH_ICON",get_SET_MESSAGE_WITH_ICON,set_SET_MESSAGE_WITH_ICON,false);
		addMember(l,"LIMIT_BUY_RESULT",get_LIMIT_BUY_RESULT,set_LIMIT_BUY_RESULT,false);
		addMember(l,"LIMIT_HERO_BUY_RESULT",get_LIMIT_HERO_BUY_RESULT,set_LIMIT_HERO_BUY_RESULT,false);
		addMember(l,"SHOW_HELP_SURE",get_SHOW_HELP_SURE,set_SHOW_HELP_SURE,false);
		addMember(l,"MAIN_CITY_UPDATE",get_MAIN_CITY_UPDATE,set_MAIN_CITY_UPDATE,false);
		createTypeMetatable(l,null, typeof(EventManager.UIEventID));
	}
}
