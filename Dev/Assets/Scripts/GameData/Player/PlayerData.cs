using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using UI.Controller;
using network;
using network.protobuf;
using Msg;
using Utils;

//using NewbieGuide;

[SLua.CustomLuaClass]
public class PlayerData : IGameData
{
    private string playerUuid = ""; //玩家uuid
    private string _account = ""; //账号ID
    int _userId = 0;
    string _password = "";
    string _userName = "";
    uint _diamond = 0;
    private uint _cash = 0;
    ulong _gold = 0;
    uint _lv = 0;
    uint _exp = 0;
    uint _vipLevel = 0;
    int _relifeLeftCount = 0;
    bool _isTriggerRelife = false;
    int _newbieguideStep = 0;
    private string _picCount = "0";

    private bool isTouris = false;
    private Texture2D _playerHead = null;

    public Texture2D PlayerHead
    {
        get { return _playerHead; }
        set { _playerHead = value; }
    }


    public string PicCount
    {
        get { return _picCount; }
        set { _picCount = value; }
    }

    /// <summary>
    /// 是否游客
    /// </summary>
    public bool IsTouris
    {
        get { return isTouris; }
        set
        {
            var isUpdatePlayerData = value != isTouris;
            isTouris = value;
            if (isUpdatePlayerData){
                UIManager.GetControler<LoginInputController>().UpdatePlayerInfo();
            }
        }
    }

    public string Uuid
    {
        get { return playerUuid; }
    }

    public string Account
    {
        get { return _account; }
    }

    private string _icon;
    private uint _sex;
    private uint _rmb;

    public uint Rmb
    {
        get { return _rmb; }
        set { _rmb = value; }
    }

    public string Icon
    {
        get
        {
/*#if UNITY_EDITOR
//            return "ftp://192.168.1.220/NiuNiuHeadImg/test.png";
            return "http://121.40.223.122:5000/uploads/001.png";
//            return "http://121.40.223.122:5000/uploads/01.png";
#endif*/
            return _icon;
        }
        set
        {
            if (!value.Equals("") && value.Length>6){
                _picCount = value.Substring(value.Length-5,1);
            }else{
                _picCount = "0";
            }

            _icon = value;
        }
    }

    public uint Sex
    {
        get { return _sex; }
        set { _sex = value; }
    }

    public uint Cash
    {
        get { return _cash; }
    }

    public bool IsTriggerRelife
    {
        get { return _isTriggerRelife; }
    }

    public int RelifeLeftCount
    {
        get { return _relifeLeftCount; }
    }

    public int UserId
    {
        get { return _userId; }
    }

    public string Password
    {
        get { return _password; }
    }

    public string UserName
    {
        get { return _userName; }
    }

    public uint Diamond
    {
        get { return _diamond; }
    } //宝石

    public ulong Gold
    {
        get { return _gold; }
    } //元宝

    public uint Exp
    {
        get { return _exp; }
    }

    public uint Level
    {
        get { return _lv; }
    }

    public uint VipLevel
    {
        get { return _vipLevel; }
    }

    public int RenameCount { get; set; }
    public int VipExp { get; set; }

    public int NewbieguideStep
    {
        get { return _newbieguideStep; }
        set { _newbieguideStep = value; }
    }
    public string ShareStr1 { get; set; }
    public string ShareStr2 { get; set; }
    public string ShareStr3 { get; set; }
    public string ShareStr4 { get; set; }
    public string ShareStr5 { get; set; }
    public string ShareStr6 { get; set; }
    public string ShareURL { get; set; }

    public string SharePicUrl { get; set; }
    public int Block { get; set; }
    public PlayerData() //构造函数,进行初始化
    {
//        MsgCallManager.AddCallback(ProtoID.SC_CReliefInfoResponse, RelifeInfoUpdate);
//        MsgCallManager.AddCallback(ProtoID.SC_CRoleDataResponse, OnRoleData);
//        MsgCallManager.AddCallback(ProtoID.SC_CHeadListResponse, OnHeadListUpdate);
//        MsgCallManager.AddCallback(ProtoID.SC_CHeadFrameListResponse, OnMaskListUpdate);
//        MsgCallManager.AddCallback(ProtoID.SC_AchievementInfoResponse, OnTitleInfoUpdate);
        MsgCallManager.AddCallback(ProtoID.sc_player_base_info, OnPlayerBaseInfo);
        //MsgCallManager.AddCallback(ProtoID.SC_CShopInfoResponse, OnVipExpUpdate);

// 初始化苹果支付数据

#if UNITY_IOS && !UNITY_EDITOR
        if (!sdk.SDKManager.IsOfficialPay())
        {
            initAppStorePurchaseItem();
        }
#endif
    }

    public void LoginGameServer()
    {
        if (!GameDataMgr.LOGIN_DATA.IsLoginGameServer){
            GameDataMgr.LOGIN_DATA.IsLoginGameServer = true;
        }
    }


    private void OnPlayerBaseInfo(object proto)
    {
        if (proto == null) return;

        var msg = proto as sc_player_base_info;
        if (playerUuid != msg.player_uuid)
        {
            if (!sdk.SDKManager.isAppStoreVersion())
            {
                UtilTools.GetAvmpSign(msg.account, 1);
            }
            //UtilTools.GetAvmpSign(msg.account, 1);
			//LoginInputController.startUpMono.VerificationLoginSign (playerUuid, "sldfjosdjfosidflsjdlfsj");
            UtilTools.InitSharePic(msg.account);
        }
        playerUuid = msg.player_uuid;
        _account = msg.account;
        _userName = TextUtils.GetString(msg.name);
        _gold = msg.gold;
        _diamond = msg.diamond;
        _exp = msg.exp;
        _lv = msg.level;
        Icon = msg.icon;
        _cash = msg.cash;
        _sex = msg.sex;
        _vipLevel = msg.vip_level;
        _rmb = msg.rmb;
        Block = msg.block;
        
//        LogSys.LogWarning("----->   icon === "+msg.icon);
        UIManager.GetControler<LoginInputController>().UpdatePlayerInfo();
    }

    public void ClearData()
    {
        _isTriggerRelife = false;
        _relifeLeftCount = 0;

        playerUuid = "";
        _account = "";
        _userName = "";
        _gold = 0;
        _diamond = 0;
        _exp = 0;
        _lv = 1;
        _icon = "";
        _cash = 0;
        _sex = 0;
        isTouris = false;
        _playerHead = null;
    }


#region 苹果支付相关接口

    private Dictionary<int, string> _purchaseList = new Dictionary<int, string>();


    public void initAppStorePurchaseItem()
    {
        AddAppStorePurchase(10001, "CNY6GOLD");
        AddAppStorePurchase(10002, "CNY30GOLD");
        AddAppStorePurchase(20001, "CNY6DIAMOND");
        AddAppStorePurchase(20002, "CNY30DIAMOND");
		RequestPurchase();
    }

    public void AddAppStorePurchase(int id, string purchaseID)
    {
        _purchaseList.Add(id, purchaseID);
    }

    public void RequestPurchase()
    {
        //app store 数据初始化
        if (sdk.SDKManager.isAppStoreVersion())
        {
            bool bFirst = true;
            string sList = "";
            foreach (KeyValuePair<int, string> item in _purchaseList)
            {
                if (bFirst)
                {
                    sList = item.Value;
                    bFirst = false;
                }
                else
                {
                    sList += ",";
                    sList += item.Value;
                }
            }
            object_c.ObjectCInterface.requestItemInfo(sList);
        }
    }

    public int GetPriceByProductID(string sID)
    {
        foreach (KeyValuePair<int, string> item in _purchaseList)
        {
            if (item.Value.Equals(sID))
            {
                BaseShopItemConfigItem shopitem = ConfigDataMgr.getInstance().BaseShopItemConfig.GetDataByKey(item.Key);
                if (shopitem != null)
                {
                    string[] const_list = shopitem.cost_list.Split(new char[] { ',' });
                    if (const_list.Length >= 2)
                    {
                        int price = 0;
                        int.TryParse(const_list[1], out price);
                        return price;
                    }
                }
            }
        }
        return 0;
    }

    public int GetKeyByProductID(string sID)
    {
        foreach (KeyValuePair<int, string> item in _purchaseList)
        {
            if (item.Value.Equals(sID))
            {
                return item.Key;
            }
        }
        return 1;
    }

    public string GetProductIDByKey(int key)
    {
        if (_purchaseList.ContainsKey(key))
        {
            return _purchaseList[key];
        }
        return null;
    }

#endregion //苹果支付相关接口
}