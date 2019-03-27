using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_iTween.reg,
				Lua_iTween_EaseType.reg,
				Lua_PoolManager_STUB.reg,
				Lua_BarcodeCam_STUB.reg,
				Lua_ComponentData.reg,
				Lua_CooldownUpdate.reg,
				Lua_LabelClick.reg,
				Lua_UtilTools.reg,
				Lua_CardAnimation.reg,
				Lua_GameText.reg,
				Lua_effect_EffectManager_STUB.reg,
				Lua_effect_EffectObject.reg,
				Lua_ConfigDataMgr_STUB.reg,
				Lua_GameDataMgr.reg,
				Lua_PlayerData.reg,
				Lua_ShopData.reg,
				Lua_EventManager_DelegateType.reg,
				Lua_EventManager_EventSystem.reg,
				Lua_EventManager_EventID.reg,
				Lua_EventManager_EventMultiArgs.reg,
				Lua_EventManager_UIEventCachePool.reg,
				Lua_EventManager_UIEventID.reg,
				Lua_FastAction_FastMove.reg,
				Lua_FastAction_FastMoveManager.reg,
				Lua_version_VersionData.reg,
				Lua_asset_AssetManager_STUB.reg,
				Lua_player_PlayerDelegate.reg,
				Lua_player_PlayerManager.reg,
				Lua_PokerBase_ePOKER_TYPE.reg,
				Lua_PokerBase_ePOKER_COUNT.reg,
				Lua_PokerBase_Poker.reg,
				Lua_PokerBase_PokerBag.reg,
				Lua_UI_Controller_ControllerBase.reg,
				Lua_MainScrollViewEffect.reg,
				Lua_ShopRechargeOtherController.reg,
				Lua_UI_Controller_UILevel.reg,
				Lua_UI_Controller_UIDepth.reg,
				Lua_UI_Controller_UIManager.reg,
				Lua_sluaAux_luaMonoBehaviour.reg,
				Lua_sluaAux_luaProtobuf.reg,
				Lua_sluaAux_luaSvrManager_STUB.reg,
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_System_Collections_Generic_Dictionary_2_int_string.reg,
				Lua_System_String.reg,
			};
			return list;
		}
	}
}
