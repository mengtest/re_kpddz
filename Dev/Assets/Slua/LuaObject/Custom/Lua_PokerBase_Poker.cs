using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PokerBase_Poker : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			PokerBase.Poker o;
			if(argc==2){
				System.Int32 a1;
				checkType(l,2,out a1);
				o=new PokerBase.Poker(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				PokerBase.ePOKER_TYPE a1;
				checkEnum(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				o=new PokerBase.Poker(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int toString(IntPtr l) {
		try {
			PokerBase.Poker self=(PokerBase.Poker)checkSelf(l);
			var ret=self.toString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IndexToType_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=PokerBase.Poker.IndexToType(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IndexToNum_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=PokerBase.Poker.IndexToNum(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NumTypeToIndex_s(IntPtr l) {
		try {
			PokerBase.ePOKER_TYPE a1;
			checkEnum(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=PokerBase.Poker.NumTypeToIndex(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_BagIndex(IntPtr l) {
		try {
			PokerBase.Poker self=(PokerBase.Poker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BagIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_BagIndex(IntPtr l) {
		try {
			PokerBase.Poker self=(PokerBase.Poker)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.BagIndex=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Num(IntPtr l) {
		try {
			PokerBase.Poker self=(PokerBase.Poker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Num);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Type(IntPtr l) {
		try {
			PokerBase.Poker self=(PokerBase.Poker)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Index(IntPtr l) {
		try {
			PokerBase.Poker self=(PokerBase.Poker)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Index);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PokerBase.Poker");
		addMember(l,toString);
		addMember(l,IndexToType_s);
		addMember(l,IndexToNum_s);
		addMember(l,NumTypeToIndex_s);
		addMember(l,"BagIndex",get_BagIndex,set_BagIndex,true);
		addMember(l,"Num",get_Num,null,true);
		addMember(l,"Type",get_Type,null,true);
		addMember(l,"Index",get_Index,null,true);
		createTypeMetatable(l,constructor, typeof(PokerBase.Poker));
	}
}
