using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameDataMgr : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameDataMgr o;
			o=new GameDataMgr();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitData_s(IntPtr l) {
		try {
			GameDataMgr.InitData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearAll_s(IntPtr l) {
		try {
			GameDataMgr.ClearAll();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearAllData_s(IntPtr l) {
		try {
			GameDataMgr.ClearAllData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LOGIN_DATA(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameDataMgr.LOGIN_DATA);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LOGIN_DATA(IntPtr l) {
		try {
			LoginData v;
			checkType(l,2,out v);
			GameDataMgr.LOGIN_DATA=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PLAYER_DATA(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameDataMgr.PLAYER_DATA);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PLAYER_DATA(IntPtr l) {
		try {
			PlayerData v;
			checkType(l,2,out v);
			GameDataMgr.PLAYER_DATA=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MyPlayer(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameDataMgr.MyPlayer);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameDataMgr");
		addMember(l,InitData_s);
		addMember(l,ClearAll_s);
		addMember(l,ClearAllData_s);
		addMember(l,"LOGIN_DATA",get_LOGIN_DATA,set_LOGIN_DATA,false);
		addMember(l,"PLAYER_DATA",get_PLAYER_DATA,set_PLAYER_DATA,false);
		addMember(l,"MyPlayer",get_MyPlayer,null,false);
		createTypeMetatable(l,constructor, typeof(GameDataMgr));
	}
}
