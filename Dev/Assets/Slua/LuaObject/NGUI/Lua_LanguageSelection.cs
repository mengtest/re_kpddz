using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LanguageSelection : LuaObject {
	static public void reg(IntPtr l) {
		getTypeTable(l,"LanguageSelection");
		createTypeMetatable(l,null, typeof(LanguageSelection),typeof(UnityEngine.MonoBehaviour));
	}
}
