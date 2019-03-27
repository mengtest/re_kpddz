using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EventManager_EventMultiArgs : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			EventManager.EventMultiArgs o;
			o=new EventManager.EventMultiArgs();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddArg(IntPtr l) {
		try {
			EventManager.EventMultiArgs self=(EventManager.EventMultiArgs)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.AddArg(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ContainsKey(IntPtr l) {
		try {
			EventManager.EventMultiArgs self=(EventManager.EventMultiArgs)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.ContainsKey(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventManager.EventMultiArgs");
		addMember(l,AddArg);
		addMember(l,ContainsKey);
		createTypeMetatable(l,constructor, typeof(EventManager.EventMultiArgs),typeof(System.EventArgs));
	}
}
