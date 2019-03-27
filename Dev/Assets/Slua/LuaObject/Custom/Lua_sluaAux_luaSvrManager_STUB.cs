using System;
using UnityEngine;
using LuaInterface;
using SLua;
using System.Collections.Generic;

namespace sluaAux
{
    public class luaSvrManager_STUB : MonoBehaviour
    {
        public static luaSvrManager getInstance()
        {
            return luaSvrManager.getInstance();
        }
    }

}

public class Lua_sluaAux_luaSvrManager_STUB : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret = sluaAux.luaSvrManager_STUB.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"sluaAux.luaSvrManager");
		addMember(l,getInstance_s);
		createTypeMetatable(l,null, typeof(sluaAux.luaSvrManager_STUB),typeof(UnityEngine.MonoBehaviour));
	}

}
