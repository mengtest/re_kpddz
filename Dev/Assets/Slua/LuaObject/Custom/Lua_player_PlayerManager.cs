using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_player_PlayerManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			player.PlayerManager o;
			o=new player.PlayerManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int init_s(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			self.init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int createModel_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				player.PlayerManager self=player.PlayerManager.getInstance();
				System.Int32 a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				player.PlayerDelegate.onPlayerEvent a3;
				LuaDelegation.checkDelegate(l,4,out a3);
				UnityEngine.Vector3 a4;
				checkType(l,5,out a4);
				var ret=self.createModel(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				player.PlayerManager self=player.PlayerManager.getInstance();
				System.String a1;
				checkType(l,2,out a1);
				player.PlayerDelegate.onPlayerEvent a2;
				LuaDelegation.checkDelegate(l,3,out a2);
				UnityEngine.Vector3 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Boolean a5;
				checkType(l,6,out a5);
				System.String[] a6;
				checkParams(l,7,out a6);
				var ret=self.createModel(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int createModelTest_s(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			player.PlayerDelegate.onPlayerEvent a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.createModelTest(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int createModelTests_s(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			System.Int32 a1;
			checkType(l,2,out a1);
			self.createModelTests(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int onModelCreateSuccessEvent_s(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			player.Model a1;
			checkType(l,2,out a1);
			self.onModelCreateSuccessEvent(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int isModelExist_s(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.isModelExist(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadFinishedCallback_s(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			System.Boolean a1;
			checkType(l,2,out a1);
			task.TaskBase a2;
			checkType(l,3,out a2);
			self.LoadFinishedCallback(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onModelCreateEvent(IntPtr l) {
		try {
			player.PlayerManager self=player.PlayerManager.getInstance();
			player.PlayerDelegate.onPlayerEvent v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onModelCreateEvent=v;
			else if(op==1) self.onModelCreateEvent+=v;
			else if(op==2) self.onModelCreateEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__pAllModelData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,player.PlayerManager._pAllModelData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__pAllModelData(IntPtr l) {
		try {
			GameModelDataHolder v;
			checkType(l,2,out v);
			player.PlayerManager._pAllModelData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ModelData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,player.PlayerManager.ModelData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"player.PlayerManager");
		addMember(l,init_s);
		addMember(l,createModel_s);
		addMember(l,createModelTest_s);
		addMember(l,createModelTests_s);
		addMember(l,onModelCreateSuccessEvent_s);
		addMember(l,isModelExist_s);
		addMember(l,LoadFinishedCallback_s);
		addMember(l,"onModelCreateEvent",null,set_onModelCreateEvent,true);
		addMember(l,"_pAllModelData",get__pAllModelData,set__pAllModelData,false);
		addMember(l,"ModelData",get_ModelData,null,false);
		createTypeMetatable(l,constructor, typeof(player.PlayerManager),typeof(Utils.Singleton<player.PlayerManager>));
	}
}
