using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UIGridCellMgr : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int resetCellMgr(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			self.resetCellMgr();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NewCellsBox(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.NewCellsBox(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				var ret=self.NewCellsBox(a1,a2,a3);
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
	static public int ClearCells(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			self.ClearCells();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateCells(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			self.UpdateCells();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RecoverSpawn(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.RecoverSpawn(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DelteLastNode(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			self.DelteLastNode();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddNewNode(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			self.AddNewNode();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ScrollToPosition(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UnityEngine.Vector2 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.ScrollToPosition(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ScrollCellToIndex(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.ScrollCellToIndex(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ScrollCellToTop(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject),typeof(int))){
				UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.ScrollCellToTop(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.ScrollCellToTop(a1,a2);
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
	static public int get_cellTemplate(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cellTemplate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cellTemplate(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.cellTemplate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_preloadAmount(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.preloadAmount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_preloadAmount(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.preloadAmount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Go(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Go);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Go(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.Go=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Grid(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Grid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Grid(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UIGrid v;
			checkType(l,2,out v);
			self.Grid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onShowItem(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UIGridCellMgr.OnInitializeItem v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onShowItem=v;
			else if(op==1) self.onShowItem+=v;
			else if(op==2) self.onShowItem-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_onHideItem(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UIGridCellMgr.OnInitializeItem v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onHideItem=v;
			else if(op==1) self.onHideItem+=v;
			else if(op==2) self.onHideItem-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_getCellRealSize(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			UIGridCellMgr.GetItemSize v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.getCellRealSize=v;
			else if(op==1) self.getCellRealSize+=v;
			else if(op==2) self.getCellRealSize-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CellCount(IntPtr l) {
		try {
			UIGridCellMgr self=(UIGridCellMgr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CellCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UIGridCellMgr");
		addMember(l,resetCellMgr);
		addMember(l,NewCellsBox);
		addMember(l,ClearCells);
		addMember(l,UpdateCells);
		addMember(l,RecoverSpawn);
		addMember(l,DelteLastNode);
		addMember(l,AddNewNode);
		addMember(l,ScrollToPosition);
		addMember(l,ScrollCellToIndex);
		addMember(l,ScrollCellToTop);
		addMember(l,"cellTemplate",get_cellTemplate,set_cellTemplate,true);
		addMember(l,"preloadAmount",get_preloadAmount,set_preloadAmount,true);
		addMember(l,"Go",get_Go,set_Go,true);
		addMember(l,"Grid",get_Grid,set_Grid,true);
		addMember(l,"onShowItem",null,set_onShowItem,true);
		addMember(l,"onHideItem",null,set_onHideItem,true);
		addMember(l,"getCellRealSize",null,set_getCellRealSize,true);
		addMember(l,"CellCount",get_CellCount,null,true);
		createTypeMetatable(l,null, typeof(UIGridCellMgr),typeof(UnityEngine.MonoBehaviour));
	}
}
