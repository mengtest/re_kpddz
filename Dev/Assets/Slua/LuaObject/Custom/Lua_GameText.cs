using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameText : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameText o;
			o=new GameText();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitData(IntPtr l) {
		try {
			GameText self=(GameText)checkSelf(l);
			self.InitData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StrCheck(IntPtr l) {
		try {
			GameText self=(GameText)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.StrCheck(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StrFilter(IntPtr l) {
		try {
			GameText self=(GameText)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Char a2;
			checkType(l,3,out a2);
			var ret=self.StrFilter(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStr_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameText.GetStr(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Format_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.String a1;
				checkType(l,1,out a1);
				var ret=GameText.Format(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(object[]))){
				System.String a1;
				checkType(l,1,out a1);
				System.Object[] a2;
				checkParams(l,2,out a2);
				var ret=GameText.Format(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.Object))){
				System.String a1;
				checkType(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				var ret=GameText.Format(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.String a1;
				checkType(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				System.Object a3;
				checkType(l,3,out a3);
				var ret=GameText.Format(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				System.String a1;
				checkType(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				System.Object a3;
				checkType(l,3,out a3);
				System.Object a4;
				checkType(l,4,out a4);
				var ret=GameText.Format(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameText.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameText");
		addMember(l,InitData);
		addMember(l,StrCheck);
		addMember(l,StrFilter);
		addMember(l,GetStr_s);
		addMember(l,Format_s);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(GameText));
	}
}
