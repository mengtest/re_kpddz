using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EventManager_DelegateType : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			EventManager.DelegateType o;
			o=new EventManager.DelegateType();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventManager.DelegateType");
		createTypeMetatable(l,constructor, typeof(EventManager.DelegateType));
	}
}
