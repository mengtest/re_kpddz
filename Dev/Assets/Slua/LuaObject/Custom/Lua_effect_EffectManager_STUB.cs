using System;
using UnityEngine;
using LuaInterface;
using SLua;
using System.Collections.Generic;

namespace effect
{
    public class EffectManager_STUB : MonoBehaviour
    {
        public static EffectManager getInstance()
        {
            return EffectManager.getInstance();
        }
    }

}

public class Lua_effect_EffectManager_STUB : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret = effect.EffectManager_STUB.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"effect.EffectManager");
		addMember(l,getInstance_s);
		createTypeMetatable(l,null, typeof(effect.EffectManager_STUB),typeof(UnityEngine.MonoBehaviour));
	}

}
