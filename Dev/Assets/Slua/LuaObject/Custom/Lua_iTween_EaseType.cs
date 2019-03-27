using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_iTween_EaseType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"iTween.EaseType");
		addMember(l,0,"easeInQuad");
		addMember(l,1,"easeOutQuad");
		addMember(l,2,"easeInOutQuad");
		addMember(l,3,"easeInCubic");
		addMember(l,4,"easeOutCubic");
		addMember(l,5,"easeInOutCubic");
		addMember(l,6,"easeInQuart");
		addMember(l,7,"easeOutQuart");
		addMember(l,8,"easeInOutQuart");
		addMember(l,9,"easeInQuint");
		addMember(l,10,"easeOutQuint");
		addMember(l,11,"easeInOutQuint");
		addMember(l,12,"easeInSine");
		addMember(l,13,"easeOutSine");
		addMember(l,14,"easeInOutSine");
		addMember(l,15,"easeInExpo");
		addMember(l,16,"easeOutExpo");
		addMember(l,17,"easeInOutExpo");
		addMember(l,18,"easeInCirc");
		addMember(l,19,"easeOutCirc");
		addMember(l,20,"easeInOutCirc");
		addMember(l,21,"linear");
		addMember(l,22,"spring");
		addMember(l,23,"easeInBounce");
		addMember(l,24,"easeOutBounce");
		addMember(l,25,"easeInOutBounce");
		addMember(l,26,"easeInBack");
		addMember(l,27,"easeOutBack");
		addMember(l,28,"easeInOutBack");
		addMember(l,29,"easeInElastic");
		addMember(l,30,"easeOutElastic");
		addMember(l,31,"easeInOutElastic");
		addMember(l,32,"punch");
		LuaDLL.lua_pop(l, 1);
	}
}
