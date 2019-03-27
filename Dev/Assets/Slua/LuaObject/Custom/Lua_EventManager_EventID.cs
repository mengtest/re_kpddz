using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EventManager_EventID : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			EventManager.EventID o;
			o=new EventManager.EventID();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SOKECT_CONNECT_RESULT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.SOKECT_CONNECT_RESULT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SOKECT_CONNECT_RESULT(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.SOKECT_CONNECT_RESULT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_GAME_RECONNECT_RESULT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.GAME_RECONNECT_RESULT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_GAME_RECONNECT_RESULT(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.GAME_RECONNECT_RESULT=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SERVER_MSG_DLSN_TEN_MUL(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.SERVER_MSG_DLSN_TEN_MUL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SERVER_MSG_DLSN_TEN_MUL(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.SERVER_MSG_DLSN_TEN_MUL=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SERVER_MSG_MISSING(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.SERVER_MSG_MISSING);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SERVER_MSG_MISSING(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.SERVER_MSG_MISSING=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LOGIN_COMPLETE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.LOGIN_COMPLETE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LOGIN_COMPLETE(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.LOGIN_COMPLETE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CLICK_SCENE_TARGET(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.CLICK_SCENE_TARGET);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CLICK_SCENE_TARGET(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.CLICK_SCENE_TARGET=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LOGINWIN_UPDATE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.LOGINWIN_UPDATE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LOGINWIN_UPDATE(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.LOGINWIN_UPDATE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PRESS_SCENE_TARGET(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.PRESS_SCENE_TARGET);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PRESS_SCENE_TARGET(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.PRESS_SCENE_TARGET=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PRESS_CANCEL_SCENE_TARGET(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.PRESS_CANCEL_SCENE_TARGET);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PRESS_CANCEL_SCENE_TARGET(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.PRESS_CANCEL_SCENE_TARGET=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PRESS_CANCEL_PRESS(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.PRESS_CANCEL_PRESS);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PRESS_CANCEL_PRESS(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.PRESS_CANCEL_PRESS=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PRESS_MOVE_PRESS(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.PRESS_MOVE_PRESS);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PRESS_MOVE_PRESS(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.PRESS_MOVE_PRESS=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PRESS_REBOUND_PRESS(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.PRESS_REBOUND_PRESS);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PRESS_REBOUND_PRESS(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.PRESS_REBOUND_PRESS=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_BATTLEITEM_REFRESH(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.BATTLEITEM_REFRESH);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_BATTLEITEM_REFRESH(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.BATTLEITEM_REFRESH=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SHOW_LOGIN_BTN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.SHOW_LOGIN_BTN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SHOW_LOGIN_BTN(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.SHOW_LOGIN_BTN=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NEWBIEGUIDE_FIRECOUNTCHANGE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.NEWBIEGUIDE_FIRECOUNTCHANGE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NEWBIEGUIDE_FIRECOUNTCHANGE(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.NEWBIEGUIDE_FIRECOUNTCHANGE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NEWBIEGUIDE_KILLCOUNTCHANGE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.NEWBIEGUIDE_KILLCOUNTCHANGE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NEWBIEGUIDE_KILLCOUNTCHANGE(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.NEWBIEGUIDE_KILLCOUNTCHANGE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,EventManager.EventID.NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE(IntPtr l) {
		try {
			System.UInt32 v;
			checkType(l,2,out v);
			EventManager.EventID.NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventManager.EventID");
		addMember(l,"SOKECT_CONNECT_RESULT",get_SOKECT_CONNECT_RESULT,set_SOKECT_CONNECT_RESULT,false);
		addMember(l,"GAME_RECONNECT_RESULT",get_GAME_RECONNECT_RESULT,set_GAME_RECONNECT_RESULT,false);
		addMember(l,"SERVER_MSG_DLSN_TEN_MUL",get_SERVER_MSG_DLSN_TEN_MUL,set_SERVER_MSG_DLSN_TEN_MUL,false);
		addMember(l,"SERVER_MSG_MISSING",get_SERVER_MSG_MISSING,set_SERVER_MSG_MISSING,false);
		addMember(l,"LOGIN_COMPLETE",get_LOGIN_COMPLETE,set_LOGIN_COMPLETE,false);
		addMember(l,"CLICK_SCENE_TARGET",get_CLICK_SCENE_TARGET,set_CLICK_SCENE_TARGET,false);
		addMember(l,"LOGINWIN_UPDATE",get_LOGINWIN_UPDATE,set_LOGINWIN_UPDATE,false);
		addMember(l,"PRESS_SCENE_TARGET",get_PRESS_SCENE_TARGET,set_PRESS_SCENE_TARGET,false);
		addMember(l,"PRESS_CANCEL_SCENE_TARGET",get_PRESS_CANCEL_SCENE_TARGET,set_PRESS_CANCEL_SCENE_TARGET,false);
		addMember(l,"PRESS_CANCEL_PRESS",get_PRESS_CANCEL_PRESS,set_PRESS_CANCEL_PRESS,false);
		addMember(l,"PRESS_MOVE_PRESS",get_PRESS_MOVE_PRESS,set_PRESS_MOVE_PRESS,false);
		addMember(l,"PRESS_REBOUND_PRESS",get_PRESS_REBOUND_PRESS,set_PRESS_REBOUND_PRESS,false);
		addMember(l,"BATTLEITEM_REFRESH",get_BATTLEITEM_REFRESH,set_BATTLEITEM_REFRESH,false);
		addMember(l,"SHOW_LOGIN_BTN",get_SHOW_LOGIN_BTN,set_SHOW_LOGIN_BTN,false);
		addMember(l,"NEWBIEGUIDE_FIRECOUNTCHANGE",get_NEWBIEGUIDE_FIRECOUNTCHANGE,set_NEWBIEGUIDE_FIRECOUNTCHANGE,false);
		addMember(l,"NEWBIEGUIDE_KILLCOUNTCHANGE",get_NEWBIEGUIDE_KILLCOUNTCHANGE,set_NEWBIEGUIDE_KILLCOUNTCHANGE,false);
		addMember(l,"NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE",get_NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE,set_NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE,false);
		createTypeMetatable(l,constructor, typeof(EventManager.EventID));
	}
}
