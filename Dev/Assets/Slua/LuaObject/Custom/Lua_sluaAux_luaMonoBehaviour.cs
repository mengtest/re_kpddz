using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_sluaAux_luaMonoBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int loadLuaScript(IntPtr l) {
		try {
			sluaAux.luaMonoBehaviour self=(sluaAux.luaMonoBehaviour)checkSelf(l);
			self.loadLuaScript();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_bindScript(IntPtr l) {
		try {
			sluaAux.luaMonoBehaviour self=(sluaAux.luaMonoBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bindScript);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_bindScript(IntPtr l) {
		try {
			sluaAux.luaMonoBehaviour self=(sluaAux.luaMonoBehaviour)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.bindScript=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"sluaAux.luaMonoBehaviour");
		addMember(l,loadLuaScript);
		addMember(l,"bindScript",get_bindScript,set_bindScript,true);
		createTypeMetatable(l,null, typeof(sluaAux.luaMonoBehaviour),typeof(UnityEngine.MonoBehaviour));
	}
}
