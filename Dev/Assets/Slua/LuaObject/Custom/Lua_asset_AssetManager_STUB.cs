using System;
using UnityEngine;
using LuaInterface;
using SLua;
using System.Collections.Generic;

namespace asset
{
    public class AssetManager_STUB : MonoBehaviour
    {
        public static AssetManager getInstance()
        {
            return AssetManager.getInstance();
        }
    }

}

public class Lua_asset_AssetManager_STUB : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getInstance_s(IntPtr l) {
		try {
			var ret = asset.AssetManager_STUB.getInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"asset.AssetManager");
		addMember(l,getInstance_s);
		createTypeMetatable(l,null, typeof(asset.AssetManager_STUB),typeof(UnityEngine.MonoBehaviour));
	}

}
