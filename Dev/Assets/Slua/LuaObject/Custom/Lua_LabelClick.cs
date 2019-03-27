using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LabelClick : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_goToTask(IntPtr l) {
		try {
			LabelClick self=(LabelClick)checkSelf(l);
			LabelClick.VoidDelegate v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.goToTask=v;
			else if(op==1) self.goToTask+=v;
			else if(op==2) self.goToTask-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LabelClick");
		addMember(l,"goToTask",null,set_goToTask,true);
		createTypeMetatable(l,null, typeof(LabelClick),typeof(UnityEngine.MonoBehaviour));
	}
}
