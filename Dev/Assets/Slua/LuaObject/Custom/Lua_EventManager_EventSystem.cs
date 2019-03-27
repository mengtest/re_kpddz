using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EventManager_EventSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int update_s(IntPtr l) {
		try {
			EventManager.EventSystem.update();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterEvent_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			EventManager.DelegateType.EventCallback a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			EventManager.EventSystem.RegisterEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveEvent_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			EventManager.DelegateType.EventCallback a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			EventManager.EventSystem.RemoveEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallEvent_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			EventManager.EventMultiArgs a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			EventManager.EventSystem.CallEvent(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Dispose_s(IntPtr l) {
		try {
			EventManager.EventSystem.Dispose();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventManager.EventSystem");
		addMember(l,update_s);
		addMember(l,RegisterEvent_s);
		addMember(l,RemoveEvent_s);
		addMember(l,CallEvent_s);
		addMember(l,Dispose_s);
		createTypeMetatable(l,null, typeof(EventManager.EventSystem));
	}
}
