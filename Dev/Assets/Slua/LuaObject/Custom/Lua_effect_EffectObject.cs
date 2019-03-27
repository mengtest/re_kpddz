using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_effect_EffectObject : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			effect.EffectObject o;
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			effect.EEffectBehaviour a3;
			checkEnum(l,4,out a3);
			o=new effect.EffectObject(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int isAutoDestroy(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			var ret=self.isAutoDestroy();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int stop(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			self.stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int disablePlayOnAwake(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.disablePlayOnAwake(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setEffectBehaviourType(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			effect.EEffectBehaviour a1;
			checkEnum(l,2,out a1);
			self.setEffectBehaviourType(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setTarget(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Transform a1;
			checkType(l,2,out a1);
			self.setTarget(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setParent(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Transform a1;
			checkType(l,2,out a1);
			self.setParent(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetScale(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			self.SetScale(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetSpeed(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			self.SetSpeed(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setPosition(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			self.setPosition(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setLocalPosition(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			self.setLocalPosition(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setLocalRotation(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Quaternion a1;
			checkType(l,2,out a1);
			self.setLocalRotation(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setLocalScale(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			self.setLocalScale(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int isStopped(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			var ret=self.isStopped();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int play(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			self.play();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int destroy(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			self.destroy();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setLayer(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.setLayer(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__loadComplete(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			effect.EffectObject.LoadComplete v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self._loadComplete=v;
			else if(op==1) self._loadComplete+=v;
			else if(op==2) self._loadComplete-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__removeComplete(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			effect.EffectObject.LoadComplete v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self._removeComplete=v;
			else if(op==1) self._removeComplete+=v;
			else if(op==2) self._removeComplete-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_param_int(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param_int);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_param_int(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param_int=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_param_transform(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param_transform);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_param_transform(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.param_transform=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_param_vector3(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param_vector3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_param_vector3(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.param_vector3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AutoDestroyDelay(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.AutoDestroyDelay=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Loop(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Loop);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Loop(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.Loop=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_TransformName(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.TransformName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_GameobjectActive(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.GameobjectActive=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Offset(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Offset=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EffectGameObj(IntPtr l) {
		try {
			effect.EffectObject self=(effect.EffectObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EffectGameObj);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"effect.EffectObject");
		addMember(l,isAutoDestroy);
		addMember(l,stop);
		addMember(l,disablePlayOnAwake);
		addMember(l,setEffectBehaviourType);
		addMember(l,setTarget);
		addMember(l,setParent);
		addMember(l,SetScale);
		addMember(l,SetSpeed);
		addMember(l,setPosition);
		addMember(l,setLocalPosition);
		addMember(l,setLocalRotation);
		addMember(l,setLocalScale);
		addMember(l,isStopped);
		addMember(l,play);
		addMember(l,destroy);
		addMember(l,setLayer);
		addMember(l,"_loadComplete",null,set__loadComplete,true);
		addMember(l,"_removeComplete",null,set__removeComplete,true);
		addMember(l,"param_int",get_param_int,set_param_int,true);
		addMember(l,"param_transform",get_param_transform,set_param_transform,true);
		addMember(l,"param_vector3",get_param_vector3,set_param_vector3,true);
		addMember(l,"AutoDestroyDelay",null,set_AutoDestroyDelay,true);
		addMember(l,"Loop",get_Loop,set_Loop,true);
		addMember(l,"TransformName",null,set_TransformName,true);
		addMember(l,"GameobjectActive",null,set_GameobjectActive,true);
		addMember(l,"Offset",null,set_Offset,true);
		addMember(l,"EffectGameObj",get_EffectGameObj,null,true);
		createTypeMetatable(l,constructor, typeof(effect.EffectObject));
	}
}
