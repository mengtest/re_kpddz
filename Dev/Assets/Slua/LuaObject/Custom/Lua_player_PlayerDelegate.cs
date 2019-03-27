using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_player_PlayerDelegate : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			player.PlayerDelegate o;
			o=new player.PlayerDelegate();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"player.PlayerDelegate");
		createTypeMetatable(l,constructor, typeof(player.PlayerDelegate));
	}
}
