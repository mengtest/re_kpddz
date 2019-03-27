using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_MainScrollViewEffect : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			MainScrollViewEffect o;
			o=new MainScrollViewEffect();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TweenTo(IntPtr l) {
		try {
			MainScrollViewEffect self=(MainScrollViewEffect)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.TweenTo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitScaleDic(IntPtr l) {
		try {
			MainScrollViewEffect self=(MainScrollViewEffect)checkSelf(l);
			self.InitScaleDic();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CenterIndex(IntPtr l) {
		try {
			MainScrollViewEffect self=(MainScrollViewEffect)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CenterIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"MainScrollViewEffect");
		addMember(l,TweenTo);
		addMember(l,InitScaleDic);
		addMember(l,"CenterIndex",get_CenterIndex,null,true);
		createTypeMetatable(l,constructor, typeof(MainScrollViewEffect),typeof(BaseMono));
	}
}
