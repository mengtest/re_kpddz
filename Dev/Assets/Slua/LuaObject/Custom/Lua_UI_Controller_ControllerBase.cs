using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UI_Controller_ControllerBase : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UI.Controller.ControllerBase o;
			o=new UI.Controller.ControllerBase();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddShowingScrollView(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.AddShowingScrollView(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearShowingScrollView(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.ClearShowingScrollView();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int autoResetScrollViewRenderQueue(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.autoResetScrollViewRenderQueue();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getWinName(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			var ret=self.getWinName();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ChangeWindowRenderQueue(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.ChangeWindowRenderQueue();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateWin(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			var ret=self.CreateWin(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAppendResourceComplete(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.LoadAppendResourceComplete(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DestroyWin(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			var ret=self.DestroyWin(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetWinRenderQueue(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetWinRenderQueue(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetScrollViewRenderQueue(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.SetScrollViewRenderQueue(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetScrollViewRenderQueueOffset(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetScrollViewRenderQueueOffset(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetUIPanelRenderQueue(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SetUIPanelRenderQueue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterUIEvent(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Int16 a1;
			checkType(l,2,out a1);
			EventManager.DelegateType.UIEventCallback a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.RegisterUIEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnRegisterAllUIEvent(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.UnRegisterAllUIEvent();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnRegisterUIEvent(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Int16 a1;
			checkType(l,2,out a1);
			EventManager.DelegateType.UIEventCallback a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.UnRegisterUIEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallUIEvent(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Int16 a1;
			checkType(l,2,out a1);
			EventManager.EventMultiArgs a2;
			checkType(l,3,out a2);
			self.CallUIEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallAllCacheEvent(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.CallAllCacheEvent();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearAllCacheEvent(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.ClearAllCacheEvent();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnloadUnusedResources(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			self.UnloadUnusedResources();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_winObject(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.winObject);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_winObject(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.winObject=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_windDepth(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.windDepth);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_windDepth(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.windDepth=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__needSceneBlur(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._needSceneBlur);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__needSceneBlur(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self._needSceneBlur=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__needSceneHide(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._needSceneHide);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__needSceneHide(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self._needSceneHide=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__bAddToScene(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._bAddToScene);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__bAddToScene(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self._bAddToScene=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__createByActionArgs(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._createByActionArgs);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__createByActionArgs(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			EventManager.EventMultiArgs v;
			checkType(l,2,out v);
			self._createByActionArgs=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__destroyByActionArgs(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._destroyByActionArgs);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__destroyByActionArgs(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			EventManager.EventMultiArgs v;
			checkType(l,2,out v);
			self._destroyByActionArgs=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsClose(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsClose);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsShow(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsShow);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsActive(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsActive);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ELevel(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.ELevel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ELevel(IntPtr l) {
		try {
			UI.Controller.ControllerBase self=(UI.Controller.ControllerBase)checkSelf(l);
			UI.Controller.UILevel v;
			checkEnum(l,2,out v);
			self.ELevel=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UI.Controller.ControllerBase");
		addMember(l,AddShowingScrollView);
		addMember(l,ClearShowingScrollView);
		addMember(l,autoResetScrollViewRenderQueue);
		addMember(l,getWinName);
		addMember(l,ChangeWindowRenderQueue);
		addMember(l,CreateWin);
		addMember(l,LoadAppendResourceComplete);
		addMember(l,DestroyWin);
		addMember(l,SetWinRenderQueue);
		addMember(l,SetScrollViewRenderQueue);
		addMember(l,GetScrollViewRenderQueueOffset);
		addMember(l,SetUIPanelRenderQueue);
		addMember(l,RegisterUIEvent);
		addMember(l,UnRegisterAllUIEvent);
		addMember(l,UnRegisterUIEvent);
		addMember(l,CallUIEvent);
		addMember(l,CallAllCacheEvent);
		addMember(l,ClearAllCacheEvent);
		addMember(l,UnloadUnusedResources);
		addMember(l,"winObject",get_winObject,set_winObject,true);
		addMember(l,"windDepth",get_windDepth,set_windDepth,true);
		addMember(l,"_needSceneBlur",get__needSceneBlur,set__needSceneBlur,true);
		addMember(l,"_needSceneHide",get__needSceneHide,set__needSceneHide,true);
		addMember(l,"_bAddToScene",get__bAddToScene,set__bAddToScene,true);
		addMember(l,"_createByActionArgs",get__createByActionArgs,set__createByActionArgs,true);
		addMember(l,"_destroyByActionArgs",get__destroyByActionArgs,set__destroyByActionArgs,true);
		addMember(l,"IsClose",get_IsClose,null,true);
		addMember(l,"IsShow",get_IsShow,null,true);
		addMember(l,"IsActive",get_IsActive,null,true);
		addMember(l,"ELevel",get_ELevel,set_ELevel,true);
		createTypeMetatable(l,constructor, typeof(UI.Controller.ControllerBase));
	}
}
