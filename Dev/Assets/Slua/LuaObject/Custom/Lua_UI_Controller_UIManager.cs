using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UI_Controller_UIManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UI.Controller.UIManager o;
			o=new UI.Controller.UIManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RecordShowingWin_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UILevel a2;
			checkEnum(l,2,out a2);
			UI.Controller.UIManager.RecordShowingWin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnRecordShowingWin_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UILevel a2;
			checkEnum(l,2,out a2);
			UI.Controller.UIManager.UnRecordShowingWin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetWinRenderQueue_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			UI.Controller.UILevel a2;
			checkEnum(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			UI.Controller.UIManager.SetWinRenderQueue(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetScrollViewRenderQueue_s(IntPtr l) {
		try {
			UI.Controller.ControllerBase a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UI.Controller.UILevel a3;
			checkEnum(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			UI.Controller.UIManager.SetScrollViewRenderQueue(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int autoResetAllRenderQueue_s(IntPtr l) {
		try {
			UI.Controller.UIManager.autoResetAllRenderQueue();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int changeToTopInUILevel_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UIManager.changeToTopInUILevel(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddCurDepth_s(IntPtr l) {
		try {
			UI.Controller.UILevel a1;
			checkEnum(l,1,out a1);
			var ret=UI.Controller.UIManager.AddCurDepth(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetNextDepth_s(IntPtr l) {
		try {
			UI.Controller.UILevel a1;
			checkEnum(l,1,out a1);
			var ret=UI.Controller.UIManager.GetNextDepth(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CleanCurDepth_s(IntPtr l) {
		try {
			UI.Controller.UILevel a1;
			checkEnum(l,1,out a1);
			UI.Controller.UIManager.CleanCurDepth(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReInit_s(IntPtr l) {
		try {
			UI.Controller.UIManager.ReInit();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitAllControllers_s(IntPtr l) {
		try {
			UI.Controller.UIManager.InitAllControllers();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetControler_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UI.Controller.UIManager.GetControler(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateLuaWin_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			EventManager.EventMultiArgs a3;
			checkType(l,3,out a3);
			var ret=UI.Controller.UIManager.CreateLuaWin(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateWin_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			EventManager.EventMultiArgs a3;
			checkType(l,3,out a3);
			var ret=UI.Controller.UIManager.CreateWin(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DestroyWin_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			EventManager.EventMultiArgs a3;
			checkType(l,3,out a3);
			UI.Controller.UIManager.DestroyWin(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateWinByAction_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			EventManager.EventMultiArgs a2;
			checkType(l,2,out a2);
			var ret=UI.Controller.UIManager.CreateWinByAction(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DestroyWinByAction_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			EventManager.EventMultiArgs a2;
			checkType(l,2,out a2);
			UI.Controller.UIManager.DestroyWinByAction(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveAllWinExpect_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==0){
				UI.Controller.UIManager.RemoveAllWinExpect();
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(System.String[]))){
				System.String[] a1;
				checkArray(l,1,out a1);
				UI.Controller.UIManager.RemoveAllWinExpect(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				UI.Controller.UIManager.RemoveAllWinExpect(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AutoSceneBlur_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UIManager.AutoSceneBlur(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AutoSceneHide_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UIManager.AutoSceneHide(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsWinShow_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UI.Controller.UIManager.IsWinShow(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int isWinLevelGuideUnLess_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UI.Controller.UIManager.isWinLevelGuideUnLess(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterLuaWinFunc_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UIManager.onLuaFuncEventHandle a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			UI.Controller.UIManager.onLuaFuncEventHandle a3;
			LuaDelegation.checkDelegate(l,3,out a3);
			UI.Controller.UIManager.RegisterLuaWinFunc(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterLuaWinRenderFunc_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UIManager.onLuaFuncEventHandle a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			UI.Controller.UIManager.RegisterLuaWinRenderFunc(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallLuaWinRenderFunc_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UI.Controller.UIManager.CallLuaWinRenderFunc(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallLuaWinOnCreateFunc_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UI.Controller.UIManager.CallLuaWinOnCreateFunc(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallLuaWinOnDestoryFunc_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UI.Controller.UIManager.CallLuaWinOnDestoryFunc(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterLuaFuncCall_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UI.Controller.UIManager.onLuaFuncEventHandle a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			UI.Controller.UIManager.RegisterLuaFuncCall(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallLuaFuncCall_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UI.Controller.UIManager.CallLuaFuncCall(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TopDepth(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UI.Controller.UIManager.TopDepth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UI.Controller.UIManager");
		addMember(l,RecordShowingWin_s);
		addMember(l,UnRecordShowingWin_s);
		addMember(l,SetWinRenderQueue_s);
		addMember(l,SetScrollViewRenderQueue_s);
		addMember(l,autoResetAllRenderQueue_s);
		addMember(l,changeToTopInUILevel_s);
		addMember(l,AddCurDepth_s);
		addMember(l,GetNextDepth_s);
		addMember(l,CleanCurDepth_s);
		addMember(l,ReInit_s);
		addMember(l,InitAllControllers_s);
		addMember(l,GetControler_s);
		addMember(l,CreateLuaWin_s);
		addMember(l,CreateWin_s);
		addMember(l,DestroyWin_s);
		addMember(l,CreateWinByAction_s);
		addMember(l,DestroyWinByAction_s);
		addMember(l,RemoveAllWinExpect_s);
		addMember(l,AutoSceneBlur_s);
		addMember(l,AutoSceneHide_s);
		addMember(l,IsWinShow_s);
		addMember(l,isWinLevelGuideUnLess_s);
		addMember(l,RegisterLuaWinFunc_s);
		addMember(l,RegisterLuaWinRenderFunc_s);
		addMember(l,CallLuaWinRenderFunc_s);
		addMember(l,CallLuaWinOnCreateFunc_s);
		addMember(l,CallLuaWinOnDestoryFunc_s);
		addMember(l,RegisterLuaFuncCall_s);
		addMember(l,CallLuaFuncCall_s);
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"TopDepth",get_TopDepth,null,false);
		createTypeMetatable(l,constructor, typeof(UI.Controller.UIManager));
	}
}
