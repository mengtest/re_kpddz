using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ComponentData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Get_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			var ret=ComponentData.Get(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Tag(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Tag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Tag(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Tag=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Id(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Id(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Text(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Text(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Value(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Value(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_color(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.color);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_color(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			UnityEngine.Color v;
			checkType(l,2,out v);
			self.color=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_eColor(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.eColor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_eColor(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			UnityEngine.Color v;
			checkType(l,2,out v);
			self.eColor=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_effColor(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.effColor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_effColor(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			UILabel.Effect v;
			checkEnum(l,2,out v);
			self.effColor=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Params(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Params);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Params(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			System.String[] v;
			checkArray(l,2,out v);
			self.Params=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Name(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Name(IntPtr l) {
		try {
			ComponentData self=(ComponentData)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ComponentData");
		addMember(l,Get_s);
		addMember(l,"Tag",get_Tag,set_Tag,true);
		addMember(l,"Id",get_Id,set_Id,true);
		addMember(l,"Text",get_Text,set_Text,true);
		addMember(l,"Value",get_Value,set_Value,true);
		addMember(l,"color",get_color,set_color,true);
		addMember(l,"eColor",get_eColor,set_eColor,true);
		addMember(l,"effColor",get_effColor,set_effColor,true);
		addMember(l,"Params",get_Params,set_Params,true);
		addMember(l,"Name",get_Name,set_Name,true);
		createTypeMetatable(l,null, typeof(ComponentData),typeof(UnityEngine.MonoBehaviour));
	}
}
