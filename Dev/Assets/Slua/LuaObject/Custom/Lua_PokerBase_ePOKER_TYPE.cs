using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PokerBase_ePOKER_TYPE : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PokerBase.ePOKER_TYPE");
		addMember(l,0,"none");
		addMember(l,1,"spade");
		addMember(l,2,"heart");
		addMember(l,3,"club");
		addMember(l,4,"diamond");
		addMember(l,5,"king");
		LuaDLL.lua_pop(l, 1);
	}
}
