using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_FastAction_FastMoveManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ResetRender(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			self.ResetRender();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Begin(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			FastAction.FastMove a1;
			checkType(l,2,out a1);
			self.Begin(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int actionBegin(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			FastAction.FastMove a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Single a7;
			checkType(l,8,out a7);
			System.Single a8;
			checkType(l,9,out a8);
			System.Int32 a9;
			checkType(l,10,out a9);
			System.Int32 a10;
			checkType(l,11,out a10);
			System.Boolean a11;
			checkType(l,12,out a11);
			System.Boolean a12;
			checkType(l,13,out a12);
			self.actionBegin(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int groupActionBegin(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			SLua.LuaTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.groupActionBegin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_FrameHandleCount(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FrameHandleCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_FrameHandleCount(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.FrameHandleCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__listCount(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._listCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__listCount(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self._listCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RenderIndex(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FastAction.FastMoveManager.RenderIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RenderIndex(IntPtr l) {
		try {
			System.Int32 v;
			checkType(l,2,out v);
			FastAction.FastMoveManager.RenderIndex=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HandleFrame(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HandleFrame);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HandleFrame(IntPtr l) {
		try {
			FastAction.FastMoveManager self=(FastAction.FastMoveManager)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.HandleFrame=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"FastAction.FastMoveManager");
		addMember(l,ResetRender);
		addMember(l,Begin);
		addMember(l,actionBegin);
		addMember(l,groupActionBegin);
		addMember(l,"FrameHandleCount",get_FrameHandleCount,set_FrameHandleCount,true);
		addMember(l,"_listCount",get__listCount,set__listCount,true);
		addMember(l,"RenderIndex",get_RenderIndex,set_RenderIndex,false);
		addMember(l,"HandleFrame",get_HandleFrame,set_HandleFrame,true);
		createTypeMetatable(l,null, typeof(FastAction.FastMoveManager),typeof(UnityEngine.MonoBehaviour));
	}
}
