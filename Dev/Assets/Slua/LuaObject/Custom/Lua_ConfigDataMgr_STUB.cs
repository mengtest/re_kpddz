using System;
using UnityEngine;
using LuaInterface;
using SLua;
using System.Collections.Generic;

public class ConfigDataMgr_STUB : MonoBehaviour
{
    public static ConfigDataMgr getInstance()
    {
        return ConfigDataMgr.getInstance();
    }
}

public class Lua_ConfigDataMgr_STUB : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret = ConfigDataMgr_STUB.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ConfigDataMgr");
		addMember(l,getInstance_s);
		createTypeMetatable(l,null, typeof(ConfigDataMgr_STUB),typeof(UnityEngine.MonoBehaviour));
	}

}
