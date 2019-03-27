using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ShopData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ShopData o;
			o=new ShopData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearData(IntPtr l) {
		try {
			ShopData self=(ShopData)checkSelf(l);
			self.ClearData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IOSPay_AddOrderNum_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			ShopData.IOSPay_AddOrderNum(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IOSPay_DelOrderNum_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			ShopData.IOSPay_DelOrderNum(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IOSPay_SaveOrder_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			ShopData.IOSPay_SaveOrder(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IOSPay_RetryLostOrder_s(IntPtr l) {
		try {
			ShopData.IOSPay_RetryLostOrder();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__dicIOSPayOrderNum(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ShopData._dicIOSPayOrderNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__dicIOSPayOrderNum(IntPtr l) {
		try {
			System.Collections.Generic.Dictionary<System.String,System.String> v;
			checkType(l,2,out v);
			ShopData._dicIOSPayOrderNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__dicIOSPayProductID(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ShopData._dicIOSPayProductID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__dicIOSPayProductID(IntPtr l) {
		try {
			System.Collections.Generic.Dictionary<System.String,System.String> v;
			checkType(l,2,out v);
			ShopData._dicIOSPayProductID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ShopData");
		addMember(l,ClearData);
		addMember(l,IOSPay_AddOrderNum_s);
		addMember(l,IOSPay_DelOrderNum_s);
		addMember(l,IOSPay_SaveOrder_s);
		addMember(l,IOSPay_RetryLostOrder_s);
		addMember(l,"_dicIOSPayOrderNum",get__dicIOSPayOrderNum,set__dicIOSPayOrderNum,false);
		addMember(l,"_dicIOSPayProductID",get__dicIOSPayProductID,set__dicIOSPayProductID,false);
		createTypeMetatable(l,constructor, typeof(ShopData));
	}
}
