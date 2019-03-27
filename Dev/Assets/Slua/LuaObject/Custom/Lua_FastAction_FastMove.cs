using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_FastAction_FastMove : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.Start();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Stop(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.Stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clean(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.Clean();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Prepare(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.Prepare();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetParams(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.SetParams(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetRenderQ(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetRenderQ(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetPositionToStartPos(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.SetPositionToStartPos();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetDefaultCall(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			FastAction.FastMove.OnMoveEvent a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.SetDefaultCall(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetOtherCall(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			FastAction.FastMove.OnMoveEvent a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.SetOtherCall(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetStartPos(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
				self.SetStartPos();
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				self.SetStartPos(a1,a2,a3);
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
	static public int SetEndPos(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
				self.SetEndPos();
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				self.SetEndPos(a1,a2,a3);
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
	static public int Doing(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			var ret=self.Doing(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int pause(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.pause();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int end(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			self.end();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Previous(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Previous);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Previous(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			FastAction.FastMove v;
			checkType(l,2,out v);
			self.Previous=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Next(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Next);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Next(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			FastAction.FastMove v;
			checkType(l,2,out v);
			self.Next=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnDefaultEvent(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			FastAction.FastMove.OnMoveEvent v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnDefaultEvent=v;
			else if(op==1) self.OnDefaultEvent+=v;
			else if(op==2) self.OnDefaultEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnOtherEvent(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			FastAction.FastMove.OnMoveEvent v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnOtherEvent=v;
			else if(op==1) self.OnOtherEvent+=v;
			else if(op==2) self.OnOtherEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsOtherEvent(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsOtherEvent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsOtherEvent(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsOtherEvent=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NoEvent(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NoEvent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NoEvent(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.NoEvent=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Duration(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Duration);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Duration(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Duration=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Delay(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Delay);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Delay(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Delay=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StartPos(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StartPos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_StartPos(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.StartPos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EndPos(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EndPos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_EndPos(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.EndPos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsWorld(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsWorld);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsWorld(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsWorld=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__doingAction(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._doingAction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__doingAction(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self._doingAction=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__renderQueue(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._renderQueue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__renderQueue(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self._renderQueue=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_name(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_name(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_time(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.time);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_time(IntPtr l) {
		try {
			FastAction.FastMove self=(FastAction.FastMove)checkSelf(l);
			System.Double v;
			checkType(l,2,out v);
			self.time=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"FastAction.FastMove");
		addMember(l,Start);
		addMember(l,Stop);
		addMember(l,Clean);
		addMember(l,Clear);
		addMember(l,Prepare);
		addMember(l,SetParams);
		addMember(l,SetRenderQ);
		addMember(l,SetPositionToStartPos);
		addMember(l,SetDefaultCall);
		addMember(l,SetOtherCall);
		addMember(l,SetStartPos);
		addMember(l,SetEndPos);
		addMember(l,Doing);
		addMember(l,pause);
		addMember(l,end);
		addMember(l,"Previous",get_Previous,set_Previous,true);
		addMember(l,"Next",get_Next,set_Next,true);
		addMember(l,"OnDefaultEvent",null,set_OnDefaultEvent,true);
		addMember(l,"OnOtherEvent",null,set_OnOtherEvent,true);
		addMember(l,"IsOtherEvent",get_IsOtherEvent,set_IsOtherEvent,true);
		addMember(l,"NoEvent",get_NoEvent,set_NoEvent,true);
		addMember(l,"Duration",get_Duration,set_Duration,true);
		addMember(l,"Delay",get_Delay,set_Delay,true);
		addMember(l,"StartPos",get_StartPos,set_StartPos,true);
		addMember(l,"EndPos",get_EndPos,set_EndPos,true);
		addMember(l,"IsWorld",get_IsWorld,set_IsWorld,true);
		addMember(l,"_doingAction",get__doingAction,set__doingAction,true);
		addMember(l,"_renderQueue",get__renderQueue,set__renderQueue,true);
		addMember(l,"name",get_name,set_name,true);
		addMember(l,"time",get_time,set_time,true);
		createTypeMetatable(l,null, typeof(FastAction.FastMove),typeof(UnityEngine.MonoBehaviour));
	}
}
