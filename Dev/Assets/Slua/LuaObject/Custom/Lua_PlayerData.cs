using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PlayerData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			PlayerData o;
			o=new PlayerData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoginGameServer(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			self.LoginGameServer();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearData(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			self.ClearData();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int initAppStorePurchaseItem(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			self.initAppStorePurchaseItem();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddAppStorePurchase(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.AddAppStorePurchase(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RequestPurchase(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			self.RequestPurchase();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPriceByProductID(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetPriceByProductID(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetKeyByProductID(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetKeyByProductID(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetProductIDByKey(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetProductIDByKey(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PlayerHead(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PlayerHead);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PlayerHead(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			UnityEngine.Texture2D v;
			checkType(l,2,out v);
			self.PlayerHead=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PicCount(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PicCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PicCount(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.PicCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsTouris(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsTouris);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsTouris(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsTouris=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Uuid(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Uuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Account(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Account);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Rmb(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Rmb);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Rmb(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.Rmb=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Icon(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Icon);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Icon(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Icon=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Sex(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Sex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Sex(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.Sex=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Cash(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Cash);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsTriggerRelife(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsTriggerRelife);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RelifeLeftCount(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RelifeLeftCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UserId(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.UserId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Password(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Password);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UserName(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.UserName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Diamond(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Diamond);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Gold(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Gold);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Exp(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Exp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Level(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_VipLevel(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.VipLevel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RenameCount(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RenameCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RenameCount(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.RenameCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_VipExp(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.VipExp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_VipExp(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.VipExp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NewbieguideStep(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NewbieguideStep);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NewbieguideStep(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.NewbieguideStep=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareStr1(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareStr1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareStr1(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareStr1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareStr2(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareStr2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareStr2(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareStr2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareStr3(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareStr3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareStr3(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareStr3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareStr4(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareStr4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareStr4(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareStr4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareStr5(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareStr5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareStr5(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareStr5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareStr6(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareStr6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareStr6(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareStr6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShareURL(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ShareURL);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShareURL(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ShareURL=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SharePicUrl(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SharePicUrl);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SharePicUrl(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.SharePicUrl=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Block(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Block);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Block(IntPtr l) {
		try {
			PlayerData self=(PlayerData)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Block=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PlayerData");
		addMember(l,LoginGameServer);
		addMember(l,ClearData);
		addMember(l,initAppStorePurchaseItem);
		addMember(l,AddAppStorePurchase);
		addMember(l,RequestPurchase);
		addMember(l,GetPriceByProductID);
		addMember(l,GetKeyByProductID);
		addMember(l,GetProductIDByKey);
		addMember(l,"PlayerHead",get_PlayerHead,set_PlayerHead,true);
		addMember(l,"PicCount",get_PicCount,set_PicCount,true);
		addMember(l,"IsTouris",get_IsTouris,set_IsTouris,true);
		addMember(l,"Uuid",get_Uuid,null,true);
		addMember(l,"Account",get_Account,null,true);
		addMember(l,"Rmb",get_Rmb,set_Rmb,true);
		addMember(l,"Icon",get_Icon,set_Icon,true);
		addMember(l,"Sex",get_Sex,set_Sex,true);
		addMember(l,"Cash",get_Cash,null,true);
		addMember(l,"IsTriggerRelife",get_IsTriggerRelife,null,true);
		addMember(l,"RelifeLeftCount",get_RelifeLeftCount,null,true);
		addMember(l,"UserId",get_UserId,null,true);
		addMember(l,"Password",get_Password,null,true);
		addMember(l,"UserName",get_UserName,null,true);
		addMember(l,"Diamond",get_Diamond,null,true);
		addMember(l,"Gold",get_Gold,null,true);
		addMember(l,"Exp",get_Exp,null,true);
		addMember(l,"Level",get_Level,null,true);
		addMember(l,"VipLevel",get_VipLevel,null,true);
		addMember(l,"RenameCount",get_RenameCount,set_RenameCount,true);
		addMember(l,"VipExp",get_VipExp,set_VipExp,true);
		addMember(l,"NewbieguideStep",get_NewbieguideStep,set_NewbieguideStep,true);
		addMember(l,"ShareStr1",get_ShareStr1,set_ShareStr1,true);
		addMember(l,"ShareStr2",get_ShareStr2,set_ShareStr2,true);
		addMember(l,"ShareStr3",get_ShareStr3,set_ShareStr3,true);
		addMember(l,"ShareStr4",get_ShareStr4,set_ShareStr4,true);
		addMember(l,"ShareStr5",get_ShareStr5,set_ShareStr5,true);
		addMember(l,"ShareStr6",get_ShareStr6,set_ShareStr6,true);
		addMember(l,"ShareURL",get_ShareURL,set_ShareURL,true);
		addMember(l,"SharePicUrl",get_SharePicUrl,set_SharePicUrl,true);
		addMember(l,"Block",get_Block,set_Block,true);
		createTypeMetatable(l,constructor, typeof(PlayerData));
	}
}
