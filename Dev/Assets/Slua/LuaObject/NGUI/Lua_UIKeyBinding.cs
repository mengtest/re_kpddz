using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UIKeyBinding : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_keyCode(IntPtr l) {
		try {
			UIKeyBinding self=(UIKeyBinding)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.keyCode);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_keyCode(IntPtr l) {
		try {
			UIKeyBinding self=(UIKeyBinding)checkSelf(l);
			UnityEngine.KeyCode v;
			checkEnum(l,2,out v);
			self.keyCode=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_modifier(IntPtr l) {
		try {
			UIKeyBinding self=(UIKeyBinding)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.modifier);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_modifier(IntPtr l) {
		try {
			UIKeyBinding self=(UIKeyBinding)checkSelf(l);
			UIKeyBinding.Modifier v;
			checkEnum(l,2,out v);
			self.modifier=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_action(IntPtr l) {
		try {
			UIKeyBinding self=(UIKeyBinding)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.action);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_action(IntPtr l) {
		try {
			UIKeyBinding self=(UIKeyBinding)checkSelf(l);
			UIKeyBinding.Action v;
			checkEnum(l,2,out v);
			self.action=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIKeyBinding");
		addMember(l,"keyCode",get_keyCode,set_keyCode,true);
		addMember(l,"modifier",get_modifier,set_modifier,true);
		addMember(l,"action",get_action,set_action,true);
		createTypeMetatable(l,null, typeof(UIKeyBinding),typeof(UnityEngine.MonoBehaviour));
	}
}
