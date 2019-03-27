
using System;
using System.Collections.Generic;
using LuaInterface;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out FastAction.FastMove.OnMoveEvent ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (FastAction.FastMove.OnMoveEvent)checkObj(l, p);
                return op;
            }
            LuaDelegate ld;
            checkType(l, -1, out ld);
            if(ld.d!=null)
            {
                ua = (FastAction.FastMove.OnMoveEvent)ld.d;
                return op;
            }
			LuaDLL.lua_pop(l,1);
			
			l = LuaState.get(l).L;
            ua = (FastAction.FastMove a1) =>
            {
                int error = pushTry(l);

				pushValue(l,a1);
				ld.pcall(1, error);
				LuaDLL.lua_settop(l, error-1);
			};
			ld.d=ua;
			return op;
		}
	}
}
