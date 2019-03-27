using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UI_Controller_UILevel : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UI.Controller.UILevel");
		addMember(l,0,"BACKGROUND");
		addMember(l,1,"NORMAL");
		addMember(l,2,"HIGHT");
		addMember(l,3,"TOP");
		addMember(l,4,"TOP_HIGHT");
		addMember(l,5,"TOP_TOP");
		LuaDLL.lua_pop(l, 1);
	}
}
