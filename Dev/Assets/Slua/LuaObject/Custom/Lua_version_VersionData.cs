using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_version_VersionData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			version.VersionData o;
			o=new version.VersionData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetUpdateVersionType(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			var ret=self.GetUpdateVersionType();
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetUpdateVersionSize(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			var ret=self.GetUpdateVersionSize();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StartUpdateVersion(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			self.StartUpdateVersion();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetUpdatePercent(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			var ret=self.GetUpdatePercent();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetDownloadingVersion(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			var ret=self.GetDownloadingVersion();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SaveProgressVersion_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			version.VersionData.SaveProgressVersion(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GeProgressVersion_s(IntPtr l) {
		try {
			var ret=version.VersionData.GeProgressVersion();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SaveLocalVersion_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			version.VersionData.SaveLocalVersion(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLocalVersion_s(IntPtr l) {
		try {
			var ret=version.VersionData.GetLocalVersion();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsReviewingVersion_s(IntPtr l) {
		try {
			var ret=version.VersionData.IsReviewingVersion();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int isAppStoreVersion_s(IntPtr l) {
		try {
			var ret=version.VersionData.isAppStoreVersion();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__versionChecked(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData._versionChecked);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__versionChecked(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			version.VersionData._versionChecked=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__strShowVersion(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData._strShowVersion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__strShowVersion(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			version.VersionData._strShowVersion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__strReViewingVersion(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData._strReViewingVersion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__strReViewingVersion(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			version.VersionData._strReViewingVersion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HealthTips(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData.HealthTips);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HealthTips(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			version.VersionData.HealthTips=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ShowTipWhenInToGame(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData.ShowTipWhenInToGame);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ShowTipWhenInToGame(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			version.VersionData.ShowTipWhenInToGame=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__strPackageSize(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData._strPackageSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__strPackageSize(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			version.VersionData._strPackageSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__toLoadProgramVersion(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,version.VersionData._toLoadProgramVersion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__toLoadProgramVersion(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			version.VersionData._toLoadProgramVersion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_VersionTypeToUpdate(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.VersionTypeToUpdate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LoadNewVersionPath(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LoadNewVersionPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LoadPackagePath(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LoadPackagePath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ErrorCodeData(IntPtr l) {
		try {
			version.VersionData self=(version.VersionData)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.ErrorCodeData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"version.VersionData");
		addMember(l,Init);
		addMember(l,GetUpdateVersionType);
		addMember(l,GetUpdateVersionSize);
		addMember(l,StartUpdateVersion);
		addMember(l,GetUpdatePercent);
		addMember(l,GetDownloadingVersion);
		addMember(l,SaveProgressVersion_s);
		addMember(l,GeProgressVersion_s);
		addMember(l,SaveLocalVersion_s);
		addMember(l,GetLocalVersion_s);
		addMember(l,IsReviewingVersion_s);
		addMember(l,isAppStoreVersion_s);
		addMember(l,"_versionChecked",get__versionChecked,set__versionChecked,false);
		addMember(l,"_strShowVersion",get__strShowVersion,set__strShowVersion,false);
		addMember(l,"_strReViewingVersion",get__strReViewingVersion,set__strReViewingVersion,false);
		addMember(l,"HealthTips",get_HealthTips,set_HealthTips,false);
		addMember(l,"ShowTipWhenInToGame",get_ShowTipWhenInToGame,set_ShowTipWhenInToGame,false);
		addMember(l,"_strPackageSize",get__strPackageSize,set__strPackageSize,false);
		addMember(l,"_toLoadProgramVersion",get__toLoadProgramVersion,set__toLoadProgramVersion,false);
		addMember(l,"VersionTypeToUpdate",get_VersionTypeToUpdate,null,true);
		addMember(l,"LoadNewVersionPath",get_LoadNewVersionPath,null,true);
		addMember(l,"LoadPackagePath",get_LoadPackagePath,null,true);
		addMember(l,"ErrorCodeData",get_ErrorCodeData,null,true);
		createTypeMetatable(l,constructor, typeof(version.VersionData));
	}
}
