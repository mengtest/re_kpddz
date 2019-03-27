using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UIDragDropItem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StopDragging(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.StopDragging(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_restriction(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.restriction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_restriction(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			UIDragDropItem.Restriction v;
			checkEnum(l,2,out v);
			self.restriction=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cloneOnDrag(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cloneOnDrag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cloneOnDrag(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.cloneOnDrag=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_pressAndHoldDelay(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pressAndHoldDelay);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_pressAndHoldDelay(IntPtr l) {
		try {
			UIDragDropItem self=(UIDragDropItem)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.pressAndHoldDelay=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIDragDropItem");
		addMember(l,StopDragging);
		addMember(l,"restriction",get_restriction,set_restriction,true);
		addMember(l,"cloneOnDrag",get_cloneOnDrag,set_cloneOnDrag,true);
		addMember(l,"pressAndHoldDelay",get_pressAndHoldDelay,set_pressAndHoldDelay,true);
		createTypeMetatable(l,null, typeof(UIDragDropItem),typeof(UnityEngine.MonoBehaviour));
	}
}
