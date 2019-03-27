using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UtilTools : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MessageDialog_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			EventManager.DelegateType.MessageDialogCallback a4;
			LuaDelegation.checkDelegate(l,4,out a4);
			EventManager.DelegateType.MessageDialogCallback a5;
			LuaDelegation.checkDelegate(l,5,out a5);
			System.Boolean a6;
			checkType(l,6,out a6);
			System.String a7;
			checkType(l,7,out a7);
			System.Int32 a8;
			checkType(l,8,out a8);
			System.Boolean a9;
			checkType(l,9,out a9);
			UtilTools.MessageDialog(a1,a2,a3,a4,a5,a6,a7,a8,a9);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MessageDialogWithTwoSelect_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			EventManager.DelegateType.MessageDialogCallback a4;
			LuaDelegation.checkDelegate(l,4,out a4);
			EventManager.DelegateType.MessageDialogCallback a5;
			LuaDelegation.checkDelegate(l,5,out a5);
			UtilTools.MessageDialogWithTwoSelect(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ErrorMessageDialog_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			EventManager.DelegateType.MessageDialogCallback a4;
			LuaDelegation.checkDelegate(l,4,out a4);
			EventManager.DelegateType.MessageDialogCallback a5;
			LuaDelegation.checkDelegate(l,5,out a5);
			System.Boolean a6;
			checkType(l,6,out a6);
			UtilTools.ErrorMessageDialog(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int applicationExitDialog_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			EventManager.DelegateType.MessageDialogCallback a4;
			LuaDelegation.checkDelegate(l,4,out a4);
			EventManager.DelegateType.MessageDialogCallback a5;
			LuaDelegation.checkDelegate(l,5,out a5);
			System.Boolean a6;
			checkType(l,6,out a6);
			UtilTools.applicationExitDialog(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MessageDialogUseMoney_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			MoneyType a2;
			checkEnum(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			UseType a5;
			checkEnum(l,5,out a5);
			EventManager.DelegateType.MessageDialogUseMoneyCallBack a6;
			LuaDelegation.checkDelegate(l,6,out a6);
			EventManager.DelegateType.MessageDialogUseMoneyCallBack a7;
			LuaDelegation.checkDelegate(l,7,out a7);
			UtilTools.MessageDialogUseMoney(a1,a2,a3,a4,a5,a6,a7);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CancelMessageDialog_s(IntPtr l) {
		try {
			UtilTools.CancelMessageDialog();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowMessage_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UtilTools.ShowMessage(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(System.Byte[]),typeof(string))){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UtilTools.ShowMessage(a1,a2);
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
	static public int ShowMessageByCode_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Byte[]),typeof(string))){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UtilTools.ShowMessageByCode(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UtilTools.ShowMessageByCode(a1,a2);
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
	static public int SetGray_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UISprite))){
				UISprite a1;
				checkType(l,1,out a1);
				UtilTools.SetGray(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UITexture))){
				UITexture a1;
				checkType(l,1,out a1);
				UtilTools.SetGray(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==2){
				UnityEngine.Transform a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				UtilTools.SetGray(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UtilTools.SetGray(a1,a2,a3);
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
	static public int RevertGray_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UITexture))){
				UITexture a1;
				checkType(l,1,out a1);
				UtilTools.RevertGray(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UISprite))){
				UISprite a1;
				checkType(l,1,out a1);
				UtilTools.RevertGray(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GameObject),typeof(bool),typeof(bool))){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UtilTools.RevertGray(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Transform),typeof(bool),typeof(bool))){
				UnityEngine.Transform a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UtilTools.RevertGray(a1,a2,a3);
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
	static public int SetIcon_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.String a5;
			checkType(l,5,out a5);
			System.Int32 a6;
			checkType(l,6,out a6);
			System.UInt32 a7;
			checkType(l,7,out a7);
			System.Boolean a8;
			checkType(l,8,out a8);
			UtilTools.SetIcon(a1,a2,a3,a4,a5,a6,a7,a8);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetQualityIcon_s(IntPtr l) {
		try {
			UISprite a1;
			checkType(l,1,out a1);
			EObjectType a2;
			checkEnum(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			UtilTools.SetQualityIcon(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetQualityColor_s(IntPtr l) {
		try {
			EObjectType a1;
			checkEnum(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=UtilTools.GetQualityColor(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetQualityEffectColor_s(IntPtr l) {
		try {
			EObjectType a1;
			checkEnum(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=UtilTools.GetQualityEffectColor(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetLabelQuilityColor_s(IntPtr l) {
		try {
			UILabel a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			EObjectType a3;
			checkEnum(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			UtilTools.SetLabelQuilityColor(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowTips_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				EventManager.EventMultiArgs a1;
				checkType(l,1,out a1);
				UnityEngine.GameObject a2;
				checkType(l,2,out a2);
				UtilTools.ShowTips(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				EventManager.EventMultiArgs a1;
				checkType(l,1,out a1);
				UnityEngine.GameObject a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UtilTools.ShowTips(a1,a2,a3);
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
	static public int AddChildNodeLayer_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			UtilTools.AddChildNodeLayer(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddChild_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				UnityEngine.GameObject a2;
				checkType(l,2,out a2);
				var ret=UtilTools.AddChild(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				UnityEngine.GameObject a2;
				checkType(l,2,out a2);
				UnityEngine.Vector3 a3;
				checkType(l,3,out a3);
				var ret=UtilTools.AddChild(a1,a2,a3);
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
	static public int ProgressRunTo_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.Collections.Hashtable a2;
				checkType(l,2,out a2);
				UtilTools.ProgressRunTo(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UtilTools.ProgressRunTo(a1,a2,a3,a4);
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
	static public int GetPlayerExpAddPercent_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			var ret=UtilTools.GetPlayerExpAddPercent(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetHeroExpAddPercent_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			var ret=UtilTools.GetHeroExpAddPercent(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FadeIn_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			EventDelegate a3;
			checkType(l,3,out a3);
			UtilTools.FadeIn(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateModel_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			player.PlayerDelegate.onPlayerEvent a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			UtilTools.CreateModel(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetFileMD5_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			var ret=UtilTools.GetFileMD5(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStringMD5_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Text.Encoding a2;
			checkType(l,2,out a2);
			var ret=UtilTools.GetStringMD5(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTimeStamp_s(IntPtr l) {
		try {
			var ret=UtilTools.GetTimeStamp();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetClientTime_s(IntPtr l) {
		try {
			var ret=UtilTools.GetClientTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTimeStampWithHMS_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Int32 a5;
			checkType(l,5,out a5);
			System.Int32 a6;
			checkType(l,6,out a6);
			var ret=UtilTools.GetTimeStampWithHMS(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetCurrentTime_s(IntPtr l) {
		try {
			var ret=UtilTools.GetCurrentTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetServerTime_s(IntPtr l) {
		try {
			var ret=UtilTools.GetServerTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TimeStampToDateTime_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			var ret=UtilTools.TimeStampToDateTime(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TimeStampToString_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=UtilTools.TimeStampToString(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.UInt32 a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=UtilTools.TimeStampToString(a1,a2);
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
	static public int GetTimeSpan_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			var ret=UtilTools.GetTimeSpan(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ConvertDateTimeInt_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			var ret=UtilTools.ConvertDateTimeInt(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowWaitWin_s(IntPtr l) {
		try {
			WaitFlag a1;
			checkEnum(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Action a3;
			LuaDelegation.checkDelegate(l,3,out a3);
			UtilTools.ShowWaitWin(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HideWaitWin_s(IntPtr l) {
		try {
			WaitFlag a1;
			checkEnum(l,1,out a1);
			UtilTools.HideWaitWin(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsWaitShowing_s(IntPtr l) {
		try {
			WaitFlag a1;
			checkEnum(l,1,out a1);
			var ret=UtilTools.IsWaitShowing(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ConvertIntDateTime_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			var ret=UtilTools.ConvertIntDateTime(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Wrap_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UtilTools.Wrap(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetLayerRecursive_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.GameObject),typeof(int))){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UtilTools.SetLayerRecursive(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GameObject),typeof(string))){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UtilTools.SetLayerRecursive(a1,a2);
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
	static public int SetActiveRecursive_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			UtilTools.SetActiveRecursive(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReverseSubstring_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=UtilTools.ReverseSubstring(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CallUIEvent_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Int16 a2;
			checkType(l,2,out a2);
			EventManager.EventMultiArgs a3;
			checkType(l,3,out a3);
			UtilTools.CallUIEvent(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetModelOutLine_s(IntPtr l) {
		try {
			player.Model a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			UtilTools.SetModelOutLine(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddShadowObject_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			UtilTools.AddShadowObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReturnToLoginScene_s(IntPtr l) {
		try {
			UtilTools.ReturnToLoginScene();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ChangeLogin_s(IntPtr l) {
		try {
			UtilTools.ChangeLogin();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetModelRenderQueueByUIParent_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			UnityEngine.Transform a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			UtilTools.SetModelRenderQueueByUIParent(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetModelRenderQueue_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			UtilTools.SetModelRenderQueue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetEffectRenderQueueByUIParent_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			UnityEngine.Transform a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			UtilTools.SetEffectRenderQueueByUIParent(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetEffectRenderQueue_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			UtilTools.SetEffectRenderQueue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReplayEffect_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			UtilTools.ReplayEffect(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetScale_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			UtilTools.SetScale(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PlaySoundEffect_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			UnityEngine.GameObject a5;
			checkType(l,5,out a5);
			System.Int32 a6;
			checkType(l,6,out a6);
			System.Single a7;
			checkType(l,7,out a7);
			var ret=UtilTools.PlaySoundEffect(a1,a2,a3,a4,a5,a6,a7);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StopBGM_s(IntPtr l) {
		try {
			UtilTools.StopBGM();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetServerListTip_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			UtilTools.SetServerListTip(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsMoneyEnough_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.UInt64 a2;
			checkType(l,2,out a2);
			var ret=UtilTools.IsMoneyEnough(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClickInValidArea_s(IntPtr l) {
		try {
			var ret=UtilTools.ClickInValidArea();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClickUI_s(IntPtr l) {
		try {
			var ret=UtilTools.ClickUI();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetTag_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			UtilTools.SetTag(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTag_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			var ret=UtilTools.GetTag(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetId_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			UtilTools.SetId(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetId_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			var ret=UtilTools.GetId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowScreenshot_s(IntPtr l) {
		try {
			UtilTools.ShowScreenshot();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HideScreenshot_s(IntPtr l) {
		try {
			UtilTools.HideScreenshot();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveAllWinExpect_s(IntPtr l) {
		try {
			UtilTools.RemoveAllWinExpect();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SaveScreenshot_s(IntPtr l) {
		try {
			UtilTools.SaveScreenshot();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyString2Clipboard_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UtilTools.CopyString2Clipboard(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStringFromClipboard_s(IntPtr l) {
		try {
			var ret=UtilTools.GetStringFromClipboard();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PlayeVideo_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Int32 a5;
			checkType(l,5,out a5);
			UtilTools.onPlayVideoComplete a6;
			LuaDelegation.checkDelegate(l,6,out a6);
			UtilTools.PlayeVideo(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetItemTemplate_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			NGUIText.Alignment a4;
			checkEnum(l,4,out a4);
			UtilTools.SetItemTemplate(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PathCheck_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UtilTools.PathCheck(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PrefabPathCheck_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UtilTools.PrefabPathCheck(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PngPathCheck_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UtilTools.PngPathCheck(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateShaders_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			UtilTools.UpdateShaders(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MultiTouchSwitch_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			UtilTools.MultiTouchSwitch(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetCurrentNetworkType_s(IntPtr l) {
		try {
			var ret=UtilTools.GetCurrentNetworkType();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetCurViewHeight_s(IntPtr l) {
		try {
			var ret=UtilTools.GetCurViewHeight();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetBgm_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UtilTools.SetBgm(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PlayeMovie_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			UtilTools.PlayeMovie(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ResetMessageWithIconCount_s(IntPtr l) {
		try {
			UtilTools.ResetMessageWithIconCount();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowMessageWithIcon_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,3,out a3);
			MessageWinType a4;
			checkEnum(l,4,out a4);
			UtilTools.ShowMessageWithIcon(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ArrayHeadIsWoDong_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			var ret=UtilTools.ArrayHeadIsWoDong(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetFPS_s(IntPtr l) {
		try {
			FPSLevel a1;
			checkEnum(l,1,out a1);
			UtilTools.SetFPS(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoginFailedAndShowLoginWin_s(IntPtr l) {
		try {
			UtilTools.LoginFailedAndShowLoginWin();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetFishRenderQueueByDepth_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=UtilTools.GetFishRenderQueueByDepth(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadHead_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UITexture a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			UtilTools.LoadHead(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PickPhotoFormAlbum_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.String a5;
			checkType(l,5,out a5);
			UtilTools.PickPhotoFormAlbum(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int pickPhotoFromCamera_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.String a5;
			checkType(l,5,out a5);
			UtilTools.pickPhotoFromCamera(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int asyncHttpUploadFile_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			UtilTools.asyncHttpUploadFile(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BindingPhone_s(IntPtr l) {
		try {
			UtilTools.BindingPhone();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyTextToPhone_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UtilTools.CopyTextToPhone(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowWaitFlag_s(IntPtr l) {
		try {
			UtilTools.ShowWaitFlag();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HideWaitFlag_s(IntPtr l) {
		try {
			UtilTools.HideWaitFlag();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int loadTexture_s(IntPtr l) {
		try {
			UITexture a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			UtilTools.loadTexture(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterCount_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UtilTools.RegisterCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnUmSdkInit_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			UtilTools.OnUmSdkInit(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAvmpSign_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			UtilTools.GetAvmpSign(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShareWeChat_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			UtilTools.ShareWeChat(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitSharePic_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UtilTools.InitSharePic(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShareWeChatPic_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			System.String a5;
			checkType(l,5,out a5);
			System.String a6;
			checkType(l,6,out a6);
			System.String a7;
			checkType(l,7,out a7);
			System.String a8;
			checkType(l,8,out a8);
			System.String a9;
			checkType(l,9,out a9);
			UtilTools.ShareWeChatPic(a1,a2,a3,a4,a5,a6,a7,a8,a9);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShareWeChatMoments_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			UtilTools.ShareWeChatMoments(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShareImageToWeChat_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			UtilTools.ShareImageToWeChat(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateAudioFile_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UtilTools.UpdateAudioFile(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UploadAudioFileAndToText_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UtilTools.UploadAudioFileAndToText(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAudioFile_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityEngine.GameObject a2;
			checkType(l,2,out a2);
			UtilTools.LoadAudioFile(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int clearAllAudio_s(IntPtr l) {
		try {
			UtilTools.clearAllAudio();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CanTouchButton(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UtilTools.CanTouchButton);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CanTouchButton(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			UtilTools.CanTouchButton=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UtilTools");
		addMember(l,MessageDialog_s);
		addMember(l,MessageDialogWithTwoSelect_s);
		addMember(l,ErrorMessageDialog_s);
		addMember(l,applicationExitDialog_s);
		addMember(l,MessageDialogUseMoney_s);
		addMember(l,CancelMessageDialog_s);
		addMember(l,ShowMessage_s);
		addMember(l,ShowMessageByCode_s);
		addMember(l,SetGray_s);
		addMember(l,RevertGray_s);
		addMember(l,SetIcon_s);
		addMember(l,SetQualityIcon_s);
		addMember(l,GetQualityColor_s);
		addMember(l,GetQualityEffectColor_s);
		addMember(l,SetLabelQuilityColor_s);
		addMember(l,ShowTips_s);
		addMember(l,AddChildNodeLayer_s);
		addMember(l,AddChild_s);
		addMember(l,ProgressRunTo_s);
		addMember(l,GetPlayerExpAddPercent_s);
		addMember(l,GetHeroExpAddPercent_s);
		addMember(l,FadeIn_s);
		addMember(l,CreateModel_s);
		addMember(l,GetFileMD5_s);
		addMember(l,GetStringMD5_s);
		addMember(l,GetTimeStamp_s);
		addMember(l,GetClientTime_s);
		addMember(l,GetTimeStampWithHMS_s);
		addMember(l,GetCurrentTime_s);
		addMember(l,GetServerTime_s);
		addMember(l,TimeStampToDateTime_s);
		addMember(l,TimeStampToString_s);
		addMember(l,GetTimeSpan_s);
		addMember(l,ConvertDateTimeInt_s);
		addMember(l,ShowWaitWin_s);
		addMember(l,HideWaitWin_s);
		addMember(l,IsWaitShowing_s);
		addMember(l,ConvertIntDateTime_s);
		addMember(l,Wrap_s);
		addMember(l,SetLayerRecursive_s);
		addMember(l,SetActiveRecursive_s);
		addMember(l,ReverseSubstring_s);
		addMember(l,CallUIEvent_s);
		addMember(l,SetModelOutLine_s);
		addMember(l,AddShadowObject_s);
		addMember(l,ReturnToLoginScene_s);
		addMember(l,ChangeLogin_s);
		addMember(l,SetModelRenderQueueByUIParent_s);
		addMember(l,SetModelRenderQueue_s);
		addMember(l,SetEffectRenderQueueByUIParent_s);
		addMember(l,SetEffectRenderQueue_s);
		addMember(l,ReplayEffect_s);
		addMember(l,SetScale_s);
		addMember(l,PlaySoundEffect_s);
		addMember(l,StopBGM_s);
		addMember(l,SetServerListTip_s);
		addMember(l,IsMoneyEnough_s);
		addMember(l,ClickInValidArea_s);
		addMember(l,ClickUI_s);
		addMember(l,SetTag_s);
		addMember(l,GetTag_s);
		addMember(l,SetId_s);
		addMember(l,GetId_s);
		addMember(l,ShowScreenshot_s);
		addMember(l,HideScreenshot_s);
		addMember(l,RemoveAllWinExpect_s);
		addMember(l,SaveScreenshot_s);
		addMember(l,CopyString2Clipboard_s);
		addMember(l,GetStringFromClipboard_s);
		addMember(l,PlayeVideo_s);
		addMember(l,SetItemTemplate_s);
		addMember(l,PathCheck_s);
		addMember(l,PrefabPathCheck_s);
		addMember(l,PngPathCheck_s);
		addMember(l,UpdateShaders_s);
		addMember(l,MultiTouchSwitch_s);
		addMember(l,GetCurrentNetworkType_s);
		addMember(l,GetCurViewHeight_s);
		addMember(l,SetBgm_s);
		addMember(l,PlayeMovie_s);
		addMember(l,ResetMessageWithIconCount_s);
		addMember(l,ShowMessageWithIcon_s);
		addMember(l,ArrayHeadIsWoDong_s);
		addMember(l,SetFPS_s);
		addMember(l,LoginFailedAndShowLoginWin_s);
		addMember(l,GetFishRenderQueueByDepth_s);
		addMember(l,LoadHead_s);
		addMember(l,PickPhotoFormAlbum_s);
		addMember(l,pickPhotoFromCamera_s);
		addMember(l,asyncHttpUploadFile_s);
		addMember(l,BindingPhone_s);
		addMember(l,CopyTextToPhone_s);
		addMember(l,ShowWaitFlag_s);
		addMember(l,HideWaitFlag_s);
		addMember(l,loadTexture_s);
		addMember(l,RegisterCount_s);
		addMember(l,OnUmSdkInit_s);
		addMember(l,GetAvmpSign_s);
		addMember(l,ShareWeChat_s);
		addMember(l,InitSharePic_s);
		addMember(l,ShareWeChatPic_s);
		addMember(l,ShareWeChatMoments_s);
		addMember(l,ShareImageToWeChat_s);
		addMember(l,UpdateAudioFile_s);
		addMember(l,UploadAudioFileAndToText_s);
		addMember(l,LoadAudioFile_s);
		addMember(l,clearAllAudio_s);
		addMember(l,"CanTouchButton",get_CanTouchButton,set_CanTouchButton,false);
		createTypeMetatable(l,null, typeof(UtilTools));
	}
}
