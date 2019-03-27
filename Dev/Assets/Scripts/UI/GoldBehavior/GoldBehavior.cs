/***************************************************************


 *
 *
 * Filename:  	GoldBehavior.cs	
 * Summary: 	金币表现
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2016/11/3 14:55
 * 
 * 
 * 外部使用调用函数
 * GoldBehavior.getInstance().setGoldBehaviorInfo(int nLevel, Vector3 pos, Vector3 endPos, int nScore,string playerid);
 * 参数1：关卡 参数2：起始位置(localposition) 参数3：结束位置(localposition) 参数4：分数 参数5：玩家ID
 ***************************************************************/
using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Utils;
using Random = UnityEngine.Random;
using effect;
using System;

class CoinObjInfo
{
    private bool bActive;
    public bool Active
    {
        get { return bActive; }
        set { bActive = value; }
    }

    private GameObject pCoinObj;
    public GameObject CoinObj
    {
        get { return pCoinObj; }
        set { pCoinObj = value; }
    }

}

class GoldBehavior : Singleton<GoldBehavior>
{
    BetterList<CoinObjInfo> _goldList;
    BetterList<CoinObjInfo> _silverList;
    int nGoldIndex = 0;
    int nSilverIndex = 0;



    void Awake()
    {
        _goldList = new BetterList<CoinObjInfo>();
        _goldList.Clear();
        _silverList = new BetterList<CoinObjInfo>();
        _silverList.Clear();
    }

    public void insertGameObj(GameObject coinObj, bool bGold, bool bActive,int nIndex = 9999999)
    {
        CoinObjInfo coin = new CoinObjInfo();
        coin.CoinObj = coinObj;
        coin.Active = bActive;

        Animator animator = coinObj.transform.Find("Icon").GetComponent<Animator>();
        animator.enabled = false;

        PListMono plistMono = coinObj.transform.Find("Icon").GetComponent<PListMono>();
        plistMono.enabled = false;

        coin.CoinObj.transform.position = new Vector3(1000, 1000, coin.CoinObj.transform.position.z);

        if(bGold)
        {
            if (nIndex == 9999999)
            {
                _goldList.Add(coin);
            }
            else
            {
                _goldList.Insert(nIndex, coin);
            }
        }
        else
        {
            if (nIndex == 9999999)
            {
                _silverList.Add(coin);
            }
            else
            {
                _silverList.Insert(nIndex, coin);
            }
        }
    }

    public void clearGameObj()
    {
        _goldList.Clear();
        _silverList.Clear();
    }

    public void setCoinBehaviorInfo(int nLevel, Vector3 pos, Vector3 endPos, int nScore,string playerid)
    {

        int nCoinCount = getCoinCountWithLevelScore(nLevel, nScore);
        bool bBoom = isShowBoomCoin(nLevel, nScore);
        float nStayTime = 1.0f;

        bool bSelf = false;
        if (playerid == GameDataMgr.PLAYER_DATA.UserId.ToString())
        {
            bSelf = true;
        }

        if(bBoom)
        {
            Transform parent = _goldList[0].CoinObj.transform.parent;
            EffectObject effect = EffectManager.getInstance().addEffect(parent, UIPrefabPath.EFFECT_JINBI);
            effect.setPosition(new Vector3(pos.x, pos.y));
            nStayTime = 1.1f;

            /*BattleMainWinController battlController = UIManager.GetControler<BattleMainWinController>();
            int nPos = BattleManager.getInstance().GetPlayerIndex(playerid);
            if (battlController != null)
                battlController.showCircularBehavior(nScore, nPos);*/

            if (bSelf)
                UtilTools.PlaySoundEffect("Sounds/jinbizhuanpan");
        }

        /*BattleMainWinController battleMainController = UIManager.GetControler<BattleMainWinController>();
        if (battleMainController != null && bSelf)
            battleMainController.shownScoreBehavior(nScore, pos);

        if (battleMainController != null && bSelf)
        {
            endPos = battleMainController.getPlaySlefGoldPosition();
        }*/

        if(bSelf)
            UtilTools.PlaySoundEffect("Sounds/jinbiwofang");
        else
            UtilTools.PlaySoundEffect("Sounds/jinbiqitawanjia");


        coinBehavior(pos, endPos, nCoinCount, bSelf, nStayTime);

    }

    private void coinBehavior(Vector2 pos, Vector2 endPos, int nCount, bool bGold,float staytime)
    {

        float randomX = nCount * 0.03f;
        float randomY = nCount * 0.03f;
        for (int i = 0; i < nCount; i++)
        {
            CoinObjInfo coin = getUnActiveCoinInfo(bGold);

            if(coin != null)
            {
                Transform goldTf = coin.CoinObj.transform;
                float nX = Random.Range(-randomX, randomX);
                float nY = Random.Range(-randomY, randomY);

                Vector3 startPos = new Vector3(pos.x + nX, pos.y + nY, coin.CoinObj.transform.position.z);
                GoldBezier gb = coin.CoinObj.GetComponent<GoldBezier>();
                float nEveryTime = 0.5f / nCount;
                nEveryTime = Mathf.Min(0.08f, nEveryTime);
                if (i >= nCount - 1 && bGold)
                    gb._bLastCoin = true;
                else
                    gb._bLastCoin = false;
                gb.setBezier(startPos, endPos, i, nEveryTime, staytime);
                coin.Active = true;
            }
        }
    }

    public void setCoinInList(GameObject gameObj, bool bActive)
    {
        for (int i = 0; i < _goldList.size; i++)
        {
            if (_goldList[i].CoinObj == gameObj)
            {
                _goldList[i].Active = bActive;
                break;
            }
        }

        for (int i = 0; i < _silverList.size; i++)
        {
            if (_silverList[i].CoinObj == gameObj)
            {
                _silverList[i].Active = bActive;
                break;
            }
        }
    }

    private CoinObjInfo getUnActiveCoinInfo(bool bGold)
    {
        CoinObjInfo coin = new CoinObjInfo();
        coin.Active = true;
        coin.CoinObj = null;

        bool bLast = false;

        Transform parent = _goldList[0].CoinObj.transform.parent;

//         int nIndex = 0;
//         while ((coin.Active || coin.CoinObj == null) && !bLast)
//         {
//             if (bGold)
//             {
//                 coin = _goldList[nIndex];
//             }
//             else
//             {
//                 coin = _silverList[nIndex];
//             }
//             nIndex++;
//             if (bGold)
//             {
//                 if (nIndex >= _goldList.size)
//                 {
//                     bLast = true;
//                 }
//             }
//             else
//             {
//                 if (nIndex >= _silverList.size)
//                 {
//                     bLast = true;
//                 }
//             }
//         }


        if(bGold)
        {
            coin = _goldList[nGoldIndex];
        }
        else
        {
            coin = _silverList[nSilverIndex];
        }

        if (coin.Active || coin.CoinObj == null)
        {
            int nIndex = 0;
            coin = null;
            coin = new CoinObjInfo();
            GameObject singleCoinObj = null;
            if (bGold)
            {
                nIndex = nGoldIndex + 1;
                if (_goldList[0].CoinObj != null)
                {
                    singleCoinObj = NGUITools.AddChild(parent.gameObject, _goldList[0].CoinObj);
                }
                nGoldIndex += 2;
            }
            else
            {
                nIndex = nSilverIndex + 1;
                if (_silverList[0].CoinObj != null)
                {
                    singleCoinObj = NGUITools.AddChild(parent.gameObject, _silverList[0].CoinObj);
                }
                nSilverIndex += 2;
            }

            if (singleCoinObj != null)
            {
                coin.CoinObj = singleCoinObj;
                coin.Active = true;

                insertGameObj(singleCoinObj, bGold, true,nIndex + 1);
                //Utils.LogSys.LogError("create new coin");
            }
        }
        else
        {
            if (bGold)
            {
                nGoldIndex++;
            }
            else
            {
                nSilverIndex++;
            }
        }

        if(nGoldIndex >= _goldList.size)
        {
            nGoldIndex = 0;
        }

        if (nSilverIndex >= _silverList.size)
        {
            nSilverIndex = 0;
        }


        if(coin.CoinObj != null)
        {
            return coin;
        }
        return null;
    }

    int getCoinCountWithLevelScore(int nLevel,int nScore)
    {
        int nCount = 0;
        if(nLevel == 1)
        {
            if(nScore < 400)
            {
                nCount = Mathf.CeilToInt((float)nScore / 40);
            }
            else
            {
                nCount = Mathf.CeilToInt((float)nScore/400);
            }
        }
        else if(nLevel == 2)
        {
            if (nScore < 2000)
            {
                nCount = Mathf.CeilToInt((float)nScore / 200);
            }
            else
            {
                nCount = Mathf.CeilToInt((float)nScore / 2000);
            }
        }
        else if (nLevel == 3)
        {
            if (nScore < 4000)
            {
                nCount = Mathf.CeilToInt((float)nScore / 400);
            }
            else
            {
                nCount = Mathf.CeilToInt((float)nScore / 4000);
            }
        }
        else if (nLevel == 4)
        {
            if (nScore < 8000)
            {
                nCount = Mathf.CeilToInt((float)nScore / 800);
            }
            else
            {
                nCount = Mathf.CeilToInt((float)nScore / 8000);
            }
        }
        else if (nLevel == 5)
        {
            if (nScore < 20000)
            {
                nCount = Mathf.CeilToInt((float)nScore / 2000);
            }
            else
            {
                nCount = Mathf.CeilToInt((float)nScore / 20000);
            }
        }
        return nCount;
    }
    bool isShowBoomCoin(int nLevel, int nScore)
    {
        bool bBoom = false;
        if(nLevel == 1)
        {
            if(nScore >= 600)
            {
                bBoom = true;
            }
        }
        else if(nLevel == 2)
        {
            if (nScore >= 3000)
            {
                bBoom = true;
            }
        }
        else if (nLevel == 3)
        {
            if (nScore >= 6000)
            {
                bBoom = true;
            }
        }
        else if (nLevel == 4)
        {
            if (nScore >= 12000)
            {
                bBoom = true;
            }
        }
        else if (nLevel == 5)
        {
            if (nScore >= 30000)
            {
                bBoom = true;
            }
        }
        return bBoom;
    }

}