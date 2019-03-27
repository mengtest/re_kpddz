using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_sluaAux_luaProtobuf : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int registerMessageScriptHandler(IntPtr l) {
		try {
			sluaAux.luaProtobuf self=(sluaAux.luaProtobuf)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.registerMessageScriptHandler(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int removeMessageHandler(IntPtr l) {
		try {
			sluaAux.luaProtobuf self=(sluaAux.luaProtobuf)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.removeMessageHandler(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret=sluaAux.luaProtobuf.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"sluaAux.luaProtobuf");
		addMember(l,registerMessageScriptHandler);
		addMember(l,removeMessageHandler);
		addMember(l,getInstance_s);
		addMember(l,sluaAux.luaProtobuf.sendMessage,true);
		createTypeMetatable(l,null, typeof(sluaAux.luaProtobuf));
	}
}
