/***************************************************************


 *
 *
 * Filename:  	ShopRechargeMono.cs	
 * Summary: 	商店充值界面
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2015/03/24 17:46
 ***************************************************************/

using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Msg;
using network;
using network.protobuf;
using sdk;
using Utils;
using Scene;

public class ShopRechargeOtherMono : MonoBehaviour
{
    private ShopRechargeOtherController _ctrl;

    private Transform _bg;
    int _nType; //0 人民币 1 元宝 2 钻石
    int _nCost;
    int _nCurKey; //當前購買商品ID
    string _sCurName = "";
    string _sCurDes = "";
    private UILabel _nameLb;
    private UISprite _iconSpr;
    private UILabel _priceLb;

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }

    void Awake()
    {
        _ctrl = (ShopRechargeOtherController) UIManager.GetControler(UIName.SHOP_RECHARGE_OTHER_WIN);
        initUI();
        _ctrl.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        _ctrl.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);
    }

    private void initUI()
    {
        _bg = transform.Find("Container");

        GameObject backGo = transform.Find("Container/closeBtn").gameObject;
        UIEventListener.Get(backGo).onClick = _ctrl.GoBack;

        GameObject weixinBtn = transform.Find("Container/bg/Sprite/WeiXin").gameObject;
        UIEventListener.Get(weixinBtn).onClick = buyItem;

        GameObject zhifubaoBtn = transform.Find("Container/bg/Sprite/ZhiFuBao").gameObject;
        UIEventListener.Get(zhifubaoBtn).onClick = buyItem;

        _nameLb = transform.Find<UILabel>("Container/bg/name");
        _iconSpr = transform.Find<UISprite>("Container/bg/icon");
        _priceLb = transform.Find<UILabel>("Container/bg/price");

        if (_ctrl._shopData != null && _ctrl._shopData.cost_list.Count > 0 &&
            _ctrl._shopData.cost_list[0].cost_type != 999){
            pb_shop_item shopitem = _ctrl._shopData;
            SetShow(shopitem);
            _sCurName = TextUtils.GetString(shopitem.name);
            _nType = 0;
            _nCost = (int) shopitem.cost_list[0].cost_num;
            _nCurKey = (int) shopitem.id;
            _sCurDes = TextUtils.GetString(shopitem.name);
        }
        else if (_ctrl._shopData != null && _ctrl._shopData.cost_list.Count > 0 &&
                 _ctrl._shopData.cost_list[0].cost_type == 999){
            pb_shop_item shopitem = _ctrl._shopData;
            SetShow(shopitem);
            ItemBaseConfigItem itembasedata = ConfigDataMgr.getInstance().ItemBaseConfig.GetDataByKey(shopitem.item_id);

            string sItemCount = "";
            if (shopitem.item_num > 0){
                if (shopitem.item_id != 101)
                    sItemCount = string.Format(GameText.GetStr("shop_count"), shopitem.item_num);
                else sItemCount = shopitem.item_num.ToString();
            }


            _sCurName = string.Format("{0}{1}", sItemCount, itembasedata.name);

            _nCost = (int) shopitem.cost_list[0].cost_num;
            string sPrice = "";
            if (shopitem.cost_list[0].cost_type == 999){
                sPrice = string.Format(GameText.GetStr("shop_rmb"), shopitem.cost_list[0].cost_num);
                _nType = 0;
            }
            else if (shopitem.cost_list[0].cost_type == 102){
                sPrice = string.Format(GameText.GetStr("shop_yuanbao"), shopitem.cost_list[0].cost_num);
                _nType = 1;
            }
            else if (shopitem.cost_list[0].cost_type == 101){
                sPrice = string.Format(GameText.GetStr("shop_jinbi"), shopitem.cost_list[0].cost_num);
                _nType = 2;
            }
            else if (shopitem.cost_list[0].cost_type == 103){
                sPrice = string.Format(GameText.GetStr("shop_zuanshi"), shopitem.cost_list[0].cost_num);
                _nType = 3;
            }

            _nCurKey = (int) shopitem.id;
            _sCurDes = TextUtils.GetString(shopitem.name);
        }
    }


    private void UIDestroyAction(EventMultiArgs args)
    {
        _bg.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable closeArg = new Hashtable();
        closeArg.Add("time", 0.3f);
        closeArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        closeArg.Add("easetype", iTween.EaseType.easeInBack);
        closeArg.Add("oncomplete", "OnDestroyActoinComplete");
        closeArg.Add("oncompletetarget", gameObject);

        iTween.ScaleTo(_bg.gameObject, closeArg);
    }

    public void OnDestroyActoinComplete()
    {
        UIManager.DestroyWin(UIName.SHOP_RECHARGE_OTHER_WIN);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="args"></param>
    private void UICreateAction(EventMultiArgs args)
    {
        _bg.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable openArg = new Hashtable();
        openArg.Add("time", 0.3f);
        openArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        openArg.Add("easetype", iTween.EaseType.easeOutBack);
        iTween.ScaleFrom(_bg.gameObject, openArg);
    }

    private void buyItem(GameObject go)
    {
        PrintNeed();
#if UNITY_EDITOR
        UtilTools.ShowMessage("PC版不提供充值服务", TextColor.RED);
        return;
#endif


		if (SDKManager.IsOfficialPay ()) {

			ComponentData componentdata = ComponentData.Get (go);
			int payTag = (int)componentdata.Tag;
			if (payTag == 1 && _ctrl.VX_recharge == false) {//微信支付先关闭
				UtilTools.ShowMessage ("微信支付通道暂时关闭", TextColor.RED);
				return;
			}
			if (payTag == 2 && _ctrl.ZFB_recharge == false) {//微信支付先关闭
				UtilTools.ShowMessage ("支付宝支付通道暂时关闭", TextColor.RED);
				return;
			}
			StartCoroutine ("startAndroidOfficialPay", componentdata.Tag);
		} 
		else 
		{
			if (version.VersionData.IsReviewingVersion())
			{
				return;
			}
			else
			{
				
				ComponentData componentdata = ComponentData.Get (go);
				int payTag = (int)componentdata.Tag;
				if (payTag == 1 && _ctrl.VX_recharge == false) {//微信支付先关闭
					UtilTools.ShowMessage ("微信支付通道暂时关闭", TextColor.RED);
					return;
				}
				if (payTag == 2 && _ctrl.ZFB_recharge == false) {//微信支付先关闭
					UtilTools.ShowMessage ("支付宝支付通道暂时关闭", TextColor.RED);
					return;
				}
				GameObject sceneObj = GameObject.Find("Scene");
				if (sceneObj)
				{
					StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
					if (startUpMono != null) 
					{
						startUpMono.startIOSWxPay (payTag,_nCost,_nCurKey,_sCurName,_sCurDes);
					}
				}

			}
		}
        
    }

    /// <summary>
    /// 官方安卓支付接口
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public IEnumerator startAndroidOfficialPay(object tag)
    {
        //UtilTools.ShowWaitWin(WaitFlag.AppStorePay);
        int payTag = (int) tag;
        string extraInfo = "1";
        int payType = 0;
        if (payTag == 1) //微信支付
        {
            extraInfo = "2";
            payType = 10;
        }

        else if (payTag == 2) // 支付宝支付
        {
            extraInfo = "1";
            payType = 2;
        }

        Debug.Log(">>> payType = " + payType);
        string strUserID = GameDataMgr.PLAYER_DATA.Account;
        string strInfoFormat = SDKManager.PlatformPayUrl;
        string strUrlInfo = string.Format(strInfoFormat, "android", _nCost.ToString(), GameDataMgr.PLAYER_DATA.UserName,
            GameDataMgr.LOGIN_DATA.serverId, strUserID, payType, _nCurKey);
        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> WWW: " + strUrlInfo);
        //向后台请求订单号
        WWW www = new WWW(strUrlInfo);
        yield return www;

        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> WWW: " + www.text);

        if (www.isDone && string.IsNullOrEmpty(www.error)){
            var orderInfo = new CommonOrderInfo();
            orderInfo.extrasParams = extraInfo;
            orderInfo.cpOrderID = www.text;
            orderInfo.goodsName = _sCurName;
            orderInfo.amount = _nCost;
            orderInfo.goodsDesc = _sCurDes;
            orderInfo.extrasParams = extraInfo;
            orderInfo.callbackUrl = SDKManager.PayCallbackUrl;

            var roleInfo = new CommonGameRoleInfo();
            roleInfo.gameRoleName = GameDataMgr.PLAYER_DATA.UserName;
            roleInfo.gameRoleID = GameDataMgr.PLAYER_DATA.Account.ToString();
            roleInfo.gameRoleLevel = GameDataMgr.PLAYER_DATA.Level.ToString();
            roleInfo.gameRoleBalance = GameDataMgr.PLAYER_DATA.Gold.ToString();
            roleInfo.vipLevel = GameDataMgr.PLAYER_DATA.VipLevel.ToString();
            SDKManager.getInstance().CommonSDKInterface.pay(orderInfo, roleInfo);
            //UtilTools.HideWaitFlag();
            _ctrl.GoBack(null);
        }
    }

	/// <summary>
	/// 官方IOS支付接口
	/// </summary>
	/// <returns>The IOS official pay.</returns>
	/// <param name="tag">Tag.</param>
	public IEnumerator startIOSOfficialPay(object tag)
	{
		//UtilTools.ShowWaitWin(WaitFlag.AppStorePay);
		int payTag = (int) tag;
		string extraInfo = "1";
		int payType = 0;
		if (payTag == 1) //微信支付
		{
			extraInfo = "2";
			payType = 10;
		}

		else if (payTag == 2) // 支付宝支付
		{
			extraInfo = "1";
			payType = 2;
		}

		string strUserID = GameDataMgr.PLAYER_DATA.Account;
		string strUrlInfo = SDKManager.PlatformPayUrlForAlipayAndWX;
		Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> WWW: " + strUrlInfo);
        WWWForm form = new WWWForm();
        form.AddField("d", "iOS");
        form.AddField("m", _nCost.ToString());
        form.AddField("uname", GameDataMgr.PLAYER_DATA.UserName);
        form.AddField("server", GameDataMgr.LOGIN_DATA.serverId);
        form.AddField("id", strUserID);
        form.AddField("type", payType.ToString());
        form.AddField("saletype", _nCurKey.ToString());

        //向后台请求订单号
        WWW www = new WWW(strUrlInfo, form);
		yield return www;

		Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> WWW: " + www.text);
		Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> ERROR WWW: " + www.error);

		if (www.isDone && string.IsNullOrEmpty(www.error)){
			var orderInfo = new CommonOrderInfo();
			orderInfo.extrasParams = extraInfo;
			orderInfo.cpOrderID = www.text;
			orderInfo.goodsName = _sCurName;
			orderInfo.amount = _nCost;
			orderInfo.goodsDesc = _sCurDes;
			orderInfo.extrasParams = extraInfo;
			orderInfo.callbackUrl = SDKManager.PayCallbackUrl;

			var roleInfo = new CommonGameRoleInfo();
			roleInfo.gameRoleName = GameDataMgr.PLAYER_DATA.UserName;
			roleInfo.gameRoleID = GameDataMgr.PLAYER_DATA.Account.ToString();
			roleInfo.gameRoleLevel = GameDataMgr.PLAYER_DATA.Level.ToString();
			roleInfo.gameRoleBalance = GameDataMgr.PLAYER_DATA.Gold.ToString();
			roleInfo.vipLevel = GameDataMgr.PLAYER_DATA.VipLevel.ToString();
			SDKManager.getInstance().CommonSDKInterface.pay(orderInfo, roleInfo);
			//UtilTools.HideWaitFlag();
			_ctrl.GoBack(null);
		}
	}


    private void SetShow(pb_shop_item pbData)
    {
        if (pbData == null) return;
        if (_nameLb != null){
            _nameLb.text = TextUtils.GetString(pbData.name);
        }

        if (_iconSpr != null){
            _iconSpr.spriteName = pbData.icon;
        }

        if (_priceLb != null){
            _priceLb.text = "￥" + pbData.cost_list[0].cost_num;
        }
    }


//    #if UNITY_EDITOR

    private void PrintNeed()
    {
        LogSys.LogWarning("sCurName = " + _sCurName + "   nType = " + _nType + "   nCost = " + _nCost +
                          "   _nCurKey = " + _nCurKey + "   sCurDes=" + _sCurDes);
    }

//    #endif
}