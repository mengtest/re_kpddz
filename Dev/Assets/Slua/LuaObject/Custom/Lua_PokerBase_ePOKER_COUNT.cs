using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PokerBase_ePOKER_COUNT : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PokerBase.ePOKER_COUNT");
		addMember(l,0,"none");
		addMember(l,13,"suit");
		addMember(l,52,"except");
		addMember(l,54,"both");
		LuaDLL.lua_pop(l, 1);
	}
}
