

using Scene;
using System.Collections.Generic;
using UnityEngine;

[SLua.CustomLuaClass]
public class ShopData : IGameData
{
    public static Dictionary<string, string> _dicIOSPayOrderNum = new Dictionary<string, string>();//ios<订单号,单据信息>（处理漏单） 
    public static Dictionary<string, string> _dicIOSPayProductID = new Dictionary<string, string>();//ios<订单号,档位编号>（处理漏单） 

    public ShopData()//构造函数,进行初始化
    {
    }
    public void ClearData()
    {
    }


    //添加订单号
    public static void IOSPay_AddOrderNum(string sOrderID, string detailInfo, string sProductID)
    {
        if (string.IsNullOrEmpty(sOrderID) || _dicIOSPayOrderNum.ContainsKey(sOrderID))
        {
            return;
        }
        _dicIOSPayOrderNum.Add(sOrderID, detailInfo);
        _dicIOSPayProductID.Add(sOrderID, sProductID);
        IOSPay_SaveOrder(true);
    }

    //充值完成后删除订单号
    public static void IOSPay_DelOrderNum(string sOrderID)
    {
        if (!_dicIOSPayOrderNum.ContainsKey(sOrderID))
        {
            return;
        }
        PlayerPrefs.DeleteKey(sOrderID);
        PlayerPrefs.DeleteKey(sOrderID + "_ProductID");
        _dicIOSPayOrderNum.Remove(sOrderID);
        _dicIOSPayProductID.Remove(sOrderID);
        IOSPay_SaveOrder(false);
    }


    /// <summary>
    /// 保存订单列表（在充值成功能后，会从该列表移除）
    /// bWrite=true会立即写硬盘，会造成卡顿，需要合适位置调用。
    /// bWrite=false是等游戏退出时写硬盘。
    /// </summary>
    /// <param name="bWrite"></param>
    public static void IOSPay_SaveOrder(bool bWrite)
    {
        if (_dicIOSPayOrderNum.Count > 0)
        {
            string sPlayerID = GameDataMgr.PLAYER_DATA.UserId.ToString();
            string sListOrder = "";
            foreach (KeyValuePair<string, string> item in _dicIOSPayOrderNum)
            {
                string sOrder = item.Key;
                PlayerPrefs.SetString(sOrder, item.Value);
                PlayerPrefs.SetString(sOrder + "_ProductID", _dicIOSPayProductID[sOrder]);
                if (string.IsNullOrEmpty(sListOrder))
                {
                    sListOrder = sOrder;
                }
                else
                {
                    sListOrder = sListOrder + "," + sOrder;
                }
            }
            PlayerPrefs.SetString("TransactionList" + sPlayerID, sListOrder);
            if (bWrite)
            {
                PlayerPrefs.Save();
            }
        }
    }

    //漏单读到内存,并处理漏单
    public static void IOSPay_RetryLostOrder()
    {
        string sPlayerID = GameDataMgr.PLAYER_DATA.UserId.ToString();
        string sListOrder = PlayerPrefs.GetString("TransactionList" + sPlayerID);
        string[] tOrders = sListOrder.Split(new char[] { ',' });
        _dicIOSPayOrderNum.Clear();
        _dicIOSPayProductID.Clear();
        for (int i = 0; i < tOrders.Length; i++)
        {
            string sOrder = tOrders[i];
            string sProductIndex = sOrder + "_ProductID";
            if (PlayerPrefs.HasKey(sOrder) && PlayerPrefs.HasKey(sProductIndex))
            {
                string detailInfo = PlayerPrefs.GetString(sOrder);
                string sProductID = PlayerPrefs.GetString(sProductIndex);
                _dicIOSPayOrderNum.Add(sOrder, detailInfo);
                _dicIOSPayProductID.Add(sOrder, sProductID);
            }
        }
        GameObject sceneObj = GameObject.Find("Scene");
        if (sceneObj)
        {
            StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
            if (startUpMono != null)
            {
                startUpMono.ApppStoreRechargeLost();
            }
        }
    }
}
