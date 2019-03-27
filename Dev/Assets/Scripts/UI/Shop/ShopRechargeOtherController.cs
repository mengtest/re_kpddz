/***************************************************************


 *
 *
 * Filename:  	ShopRechargeController.cs	
 * Summary: 	商店充值界面
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2015/03/24 17:46
 ***************************************************************/

using System.Collections.Generic;
using System.Text.RegularExpressions;
using network;
using network.protobuf;
using UI.Controller;
using UnityEngine;
using Scene;
[SLua.CustomLuaClass]
public class ShopRechargeOtherController : ControllerBase
{
    private ShopRechargeOtherMono _mono;
    public int _itemID;
    public pb_shop_item _shopData;
    public bool VX_recharge = true;
    public bool ZFB_recharge = true;
    private List<pb_shop_item> _list = new List<pb_shop_item>();

    public ShopRechargeOtherController(string uiName)
    {
        sName = uiName;
        _itemID = 0;
        ELevel = UILevel.HIGHT;
        prefabsPath = new string[] {UIPrefabPath.SHOP_RECHARGE_OTHER_WIN};
        MsgCallManager.AddCallback(ProtoID.sc_shop_all_item_base_config, OnShowAllItemBaseConfig); //登录回调
    }


    /// <summary>
    /// 销毁前处理
    /// </summary>
    protected override void UIDestroyCallback()
    {
        if (_mono != null){
            UnityEngine.Object.DestroyImmediate(_mono);
            _mono = null;
        }
    }

    /// <summary>
    /// 界面加载完成后调用
    /// </summary>
    protected override void UICreateCallback()
    {
        _shopData = GetShopItem(_itemID);
        _mono = winObject.AddComponent<ShopRechargeOtherMono>();
    }

    public void GoBack(GameObject go)
    {
        UIManager.DestroyWinByAction(sName);
    }

    public void toBack()
    {
        UIManager.DestroyWinByAction(sName);
    }


    private void OnShowAllItemBaseConfig(object proto)
    {
        if (proto == null) return;
        var msg = proto as sc_shop_all_item_base_config;
        _list = msg.item_list;
    }

    public pb_shop_item GetShopItem(int id)
    {
        for (int i = 0; i < _list.Count; i++){
            if (_list[i].id == id){
                return _list[i];
            }
        }

        return null;
    }
	public void wxBuy(){
		_shopData = GetShopItem(_itemID);
		if (_shopData == null)
			return;
		int payTag = 1;
		if (payTag == 1 && VX_recharge == false) {//微信支付先关闭
			UtilTools.ShowMessage ("微信支付通道暂时关闭", TextColor.RED);
			return;
		}
		GameObject sceneObj = GameObject.Find("Scene");
		if (sceneObj)
		{
			StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
			if (startUpMono != null) 
			{
				startUpMono.startIOSWxPay (payTag,(int) _shopData.cost_list[0].cost_num,(int)_shopData.id,TextUtils.GetString(_shopData.name),TextUtils.GetString(_shopData.name));
			}
		}
    }
    /// <summary>
    /// 苹果IAP支付
    /// </summary>
    /// <param name="id"></param>
    public void buyItemIAPImpl(int id)
    {
#if UNITY_IOS && !UNITY_EDITOR
        pb_shop_item shopData = GetShopItem(id);
        string productID = GameDataMgr.PLAYER_DATA.GetProductIDByKey((int)shopData.id);
        object_c.ObjectCInterface.buyItem(productID);
        UtilTools.ShowWaitWin(WaitFlag.AppStorePay, 20f);
#endif
    }
}