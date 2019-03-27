using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ShopRechargeOtherController : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ShopRechargeOtherController o;
			System.String a1;
			checkType(l,2,out a1);
			o=new ShopRechargeOtherController(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GoBack(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.GoBack(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int toBack(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			self.toBack();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetShopItem(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetShopItem(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int wxBuy(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			self.wxBuy();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int buyItemIAPImpl(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.buyItemIAPImpl(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__itemID(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._itemID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__itemID(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self._itemID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__shopData(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._shopData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__shopData(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			network.pb_shop_item v;
			checkType(l,2,out v);
			self._shopData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_VX_recharge(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.VX_recharge);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_VX_recharge(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.VX_recharge=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ZFB_recharge(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ZFB_recharge);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ZFB_recharge(IntPtr l) {
		try {
			ShopRechargeOtherController self=(ShopRechargeOtherController)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.ZFB_recharge=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ShopRechargeOtherController");
		addMember(l,GoBack);
		addMember(l,toBack);
		addMember(l,GetShopItem);
		addMember(l,wxBuy);
		addMember(l,buyItemIAPImpl);
		addMember(l,"_itemID",get__itemID,set__itemID,true);
		addMember(l,"_shopData",get__shopData,set__shopData,true);
		addMember(l,"VX_recharge",get_VX_recharge,set_VX_recharge,true);
		addMember(l,"ZFB_recharge",get_ZFB_recharge,set_ZFB_recharge,true);
		createTypeMetatable(l,constructor, typeof(ShopRechargeOtherController),typeof(UI.Controller.ControllerBase));
	}
}
