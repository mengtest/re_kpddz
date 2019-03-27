using System;
using UnityEngine;
using LuaInterface;
using SLua;
using System.Collections.Generic;

public class PoolManager_STUB : MonoBehaviour
{
    public static PoolManager getInstance()
    {
        return PoolManager.getInstance();
    }
}

public class Lua_PoolManager_STUB : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret = PoolManager_STUB.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PoolManager");
		addMember(l,getInstance_s);
		createTypeMetatable(l,null, typeof(PoolManager_STUB),typeof(UnityEngine.MonoBehaviour));
	}

}
