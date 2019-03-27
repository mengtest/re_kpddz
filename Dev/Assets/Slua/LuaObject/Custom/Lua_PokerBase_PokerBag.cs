using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PokerBase_PokerBag : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			PokerBase.PokerBag o;
			o=new PokerBase.PokerBag();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FinalPokers(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			var ret=self.FinalPokers();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AllCombination(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.AllCombination(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Combinations(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.Combinations(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CleanPokers(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			self.CleanPokers();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddPublicPoker(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.AddPublicPoker(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(PokerBase.Poker))){
				PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
				PokerBase.Poker a1;
				checkType(l,2,out a1);
				var ret=self.AddPublicPoker(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
				PokerBase.ePOKER_TYPE a1;
				checkEnum(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.AddPublicPoker(a1,a2);
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
	static public int AddOwnPoker(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.AddOwnPoker(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(PokerBase.Poker))){
				PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
				PokerBase.Poker a1;
				checkType(l,2,out a1);
				var ret=self.AddOwnPoker(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
				PokerBase.ePOKER_TYPE a1;
				checkEnum(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.AddOwnPoker(a1,a2);
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
	static public int PrintPokers(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			System.Collections.Generic.List<PokerBase.Poker> a1;
			checkType(l,2,out a1);
			self.PrintPokers(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PublicPokers(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PublicPokers);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_OwnPokers(IntPtr l) {
		try {
			PokerBase.PokerBag self=(PokerBase.PokerBag)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OwnPokers);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PokerBase.PokerBag");
		addMember(l,FinalPokers);
		addMember(l,AllCombination);
		addMember(l,Combinations);
		addMember(l,CleanPokers);
		addMember(l,AddPublicPoker);
		addMember(l,AddOwnPoker);
		addMember(l,PrintPokers);
		addMember(l,"PublicPokers",get_PublicPokers,null,true);
		addMember(l,"OwnPokers",get_OwnPokers,null,true);
		createTypeMetatable(l,constructor, typeof(PokerBase.PokerBag));
	}
}
