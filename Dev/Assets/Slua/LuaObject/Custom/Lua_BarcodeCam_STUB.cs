using System;
using UnityEngine;
using LuaInterface;
using SLua;
using System.Collections.Generic;

public class BarcodeCam_STUB : MonoBehaviour
{
    public static BarcodeCam getInstance()
    {
        return BarcodeCam.getInstance();
    }
}

public class Lua_BarcodeCam_STUB : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret = BarcodeCam_STUB.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"BarcodeCam");
		addMember(l,getInstance_s);
		createTypeMetatable(l,null, typeof(BarcodeCam_STUB),typeof(UnityEngine.MonoBehaviour));
	}

}
