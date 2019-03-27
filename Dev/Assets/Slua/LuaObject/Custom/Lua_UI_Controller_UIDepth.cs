using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UI_Controller_UIDepth : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UI.Controller.UIDepth o;
			o=new UI.Controller.UIDepth();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_BACKGROUND(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIDepth.BACKGROUND);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NORMAL(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIDepth.NORMAL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HIGHT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIDepth.HIGHT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TOP(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIDepth.TOP);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TOP_HIGHT(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIDepth.TOP_HIGHT);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TOP_TOP(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIDepth.TOP_TOP);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UI.Controller.UIDepth");
		addMember(l,"BACKGROUND",get_BACKGROUND,null,false);
		addMember(l,"NORMAL",get_NORMAL,null,false);
		addMember(l,"HIGHT",get_HIGHT,null,false);
		addMember(l,"TOP",get_TOP,null,false);
		addMember(l,"TOP_HIGHT",get_TOP_HIGHT,null,false);
		addMember(l,"TOP_TOP",get_TOP_TOP,null,false);
		createTypeMetatable(l,constructor, typeof(UI.Controller.UIDepth));
	}
}
