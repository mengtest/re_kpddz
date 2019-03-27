using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_CooldownUpdate : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetEndTime(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.UInt32 a1;
			checkType(l,2,out a1);
			self.SetEndTime(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetStartTime(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.UInt32 a1;
			checkType(l,2,out a1);
			self.SetStartTime(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnComplete(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			CooldownUpdate.TimeOverCallback v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnComplete=v;
			else if(op==1) self.OnComplete+=v;
			else if(op==2) self.OnComplete-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnUpdate(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.Action v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnUpdate=v;
			else if(op==1) self.OnUpdate+=v;
			else if(op==2) self.OnUpdate-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isCooldown(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isCooldown);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_isCooldown(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.isCooldown=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isServerTime(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isServerTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_isServerTime(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.isServerTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isNoHour(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isNoHour);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_isNoHour(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.isNoHour=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isTicket(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isTicket);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_isTicket(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.isTicket=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cooldownSound(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cooldownSound);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cooldownSound(IntPtr l) {
		try {
			CooldownUpdate self=(CooldownUpdate)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.cooldownSound=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"CooldownUpdate");
		addMember(l,SetEndTime);
		addMember(l,SetStartTime);
		addMember(l,"OnComplete",null,set_OnComplete,true);
		addMember(l,"OnUpdate",null,set_OnUpdate,true);
		addMember(l,"isCooldown",get_isCooldown,set_isCooldown,true);
		addMember(l,"isServerTime",get_isServerTime,set_isServerTime,true);
		addMember(l,"isNoHour",get_isNoHour,set_isNoHour,true);
		addMember(l,"isTicket",get_isTicket,set_isTicket,true);
		addMember(l,"cooldownSound",get_cooldownSound,set_cooldownSound,true);
		createTypeMetatable(l,null, typeof(CooldownUpdate),typeof(UnityEngine.MonoBehaviour));
	}
}
