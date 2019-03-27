using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EventManager_UIEventCachePool : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			EventManager.UIEventCachePool o;
			o=new EventManager.UIEventCachePool();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SaveUIEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			EventManager.EventMultiArgs a1;
			checkType(l,2,out a1);
			self.SaveUIEvent(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallAllCacheEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			self.CallAllCacheEvent();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearAllCacheEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			self.ClearAllCacheEvent();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterUIEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			System.Int16 a1;
			checkType(l,2,out a1);
			EventManager.DelegateType.UIEventCallback a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.RegisterUIEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnRegisterUIEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			System.Int16 a1;
			checkType(l,2,out a1);
			EventManager.DelegateType.UIEventCallback a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.UnRegisterUIEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnRegisterAllUIEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			self.UnRegisterAllUIEvent();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RunUIEvent(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			EventManager.EventMultiArgs a1;
			checkType(l,2,out a1);
			self.RunUIEvent(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IdEventCallback(IntPtr l) {
		try {
			EventManager.UIEventCachePool self=(EventManager.UIEventCachePool)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IdEventCallback);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EventManager.UIEventCachePool");
		addMember(l,SaveUIEvent);
		addMember(l,CallAllCacheEvent);
		addMember(l,ClearAllCacheEvent);
		addMember(l,RegisterUIEvent);
		addMember(l,UnRegisterUIEvent);
		addMember(l,UnRegisterAllUIEvent);
		addMember(l,RunUIEvent);
		addMember(l,"IdEventCallback",get_IdEventCallback,null,true);
		createTypeMetatable(l,constructor, typeof(EventManager.UIEventCachePool));
	}
}
