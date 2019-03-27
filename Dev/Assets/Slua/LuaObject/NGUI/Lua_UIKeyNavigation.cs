using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UIKeyNavigation : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_list(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UIKeyNavigation.list);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_list(IntPtr l) {
		try {
			BetterList<UIKeyNavigation> v;
			checkType(l,2,out v);
			UIKeyNavigation.list=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_constraint(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.constraint);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_constraint(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			UIKeyNavigation.Constraint v;
			checkEnum(l,2,out v);
			self.constraint=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_onUp(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onUp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onUp(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.onUp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_onDown(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onDown);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onDown(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.onDown=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_onLeft(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onLeft);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onLeft(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.onLeft=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_onRight(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onRight);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onRight(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.onRight=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_onClick(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onClick);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onClick(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.onClick=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_startsSelected(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.startsSelected);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_startsSelected(IntPtr l) {
		try {
			UIKeyNavigation self=(UIKeyNavigation)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.startsSelected=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIKeyNavigation");
		addMember(l,"list",get_list,set_list,false);
		addMember(l,"constraint",get_constraint,set_constraint,true);
		addMember(l,"onUp",get_onUp,set_onUp,true);
		addMember(l,"onDown",get_onDown,set_onDown,true);
		addMember(l,"onLeft",get_onLeft,set_onLeft,true);
		addMember(l,"onRight",get_onRight,set_onRight,true);
		addMember(l,"onClick",get_onClick,set_onClick,true);
		addMember(l,"startsSelected",get_startsSelected,set_startsSelected,true);
		createTypeMetatable(l,null, typeof(UIKeyNavigation),typeof(UnityEngine.MonoBehaviour));
	}
}
