/***************************************************************


 *
 *
 * Filename:  	ShopMono.cs	
 * Summary: 	商店界面
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
using asset;
using effect;
using Utils;
using System.Collections.Generic;

public class BarrageMono : MonoBehaviour
{
    private BarrageController _ctrl;
    private Transform _bg;
    private GameObject _barrageContent;

    private GameObject _normalBarrage;
    private GameObject _highBarrage;

    private int nRow = 5;
    private float nTotalTime = 20.0f;
    private Dictionary<int, bool> _rowStatus = new Dictionary<int,bool>();

    // Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
    void Update()
    {
        for (int i = 0; i < nRow; i++)
        {
            if (_rowStatus[i])
            {
                showBarrage(i);
            }
        }
	}

    void Awake()
    {
        _ctrl = (BarrageController)UIManager.GetControler(UIName.BARRAGE_WIN);
        initUI();
        _ctrl.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        _ctrl.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);
        
    }
    private void initUI()
    {
        _bg = transform.Find("Container");
        _barrageContent = _bg.Find("BarrageContent").gameObject;
        _normalBarrage = _barrageContent.transform.Find("NormalBarrage").gameObject;
        _highBarrage = _barrageContent.transform.Find("HighBarrage").gameObject;


        PoolManager.getInstance().CreatePool(_normalBarrage, _normalBarrage.transform.parent.gameObject, 25, false, 0, new Vector3(1000, 1000, _normalBarrage.transform.position.z));
        PoolManager.getInstance().CreatePool(_highBarrage, _highBarrage.transform.parent.gameObject, 25, false, 0, new Vector3(1000, 1000, _highBarrage.transform.position.z));
        
        for(int i = 0;i < nRow;i++)
        {
            _rowStatus[i] = true;
        }
    }
    private void UIDestroyAction(EventMultiArgs args)
    {
        clearGameObj();
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
        UIManager.DestroyWin(UIName.BARRAGE_WIN);
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
        openArg.Add("oncomplete", "OnCreateActoinComplete");
        openArg.Add("oncompletetarget", gameObject);
        iTween.ScaleFrom(_bg.gameObject, openArg);
    }
    public void OnCreateActoinComplete()
    {
    }

    public void clearGameObj()
    {
        if (_normalBarrage != null)
        {
            PoolManager.getInstance().DestroyPool(_normalBarrage);
            Destroy(_normalBarrage);
        }
        if (_highBarrage != null)
        {
            PoolManager.getInstance().DestroyPool(_highBarrage);
            Destroy(_highBarrage);
        }
        _normalBarrage = null;
        _highBarrage = null;

    }

    private void showBarrage(int nRowIndex)
    {
        
        BetterList<SC_CBarrageResponse> tempBarrageList = null;
        tempBarrageList = _ctrl._richCarBarrageList;
        if (tempBarrageList != null && tempBarrageList.size > 0)
        {
            GameObject curBarrageObj = null;
            SC_CBarrageResponse curBarrage = tempBarrageList[0];
            if (curBarrage != null)
            {
                float backWidth = 0;
                float startPos = 0;
                int nType = getBarradgeType(curBarrage.itemId);
                if (nType == 2)
                {
                    backWidth = 165.0f;
                    startPos = 80.0f;
                    curBarrageObj = PoolManager.getInstance().Spawn(_highBarrage);
                }
                else
                {
                    backWidth = 35.0f;
                    startPos = 15.0f;
                    curBarrageObj = PoolManager.getInstance().Spawn(_normalBarrage);
                }

                if (curBarrageObj != null)
                {
                    UIWidget back = curBarrageObj.transform.Find("Back").GetComponent<UIWidget>();

                    UISprite vipIcon = back.transform.Find("VipIcon").GetComponent<UISprite>();
                    vipIcon.spriteName = curBarrage.vipLv.ToString();
                    UILabel senderName = back.transform.Find("SenderName").GetComponent<UILabel>();
                    senderName.overflowMethod = UILabel.Overflow.ResizeFreely;
                    if (_ctrl.nShownType == 1)
                        senderName.text = string.Format("{0}:",curBarrage.name);
                    else if (_ctrl.nShownType == 2)
                        senderName.text = string.Format("{0}", curBarrage.name);


                    UILabel content = back.transform.Find("Content").GetComponent<UILabel>();
                    content.overflowMethod = UILabel.Overflow.ResizeFreely;
                    content.text = GameText.Instance.StrFilter(curBarrage.content);
                    senderName.overflowMethod = UILabel.Overflow.ClampContent;
                    content.overflowMethod = UILabel.Overflow.ClampContent;

                    backWidth = backWidth + senderName.width + content.width + vipIcon.width;
                    back.width = Mathf.CeilToInt(backWidth);

                    vipIcon.transform.localPosition = new Vector3(startPos, vipIcon.transform.localPosition.y, vipIcon.transform.localPosition.z);
                    senderName.transform.localPosition = new Vector3(vipIcon.transform.localPosition.x + vipIcon.width + 5.0f, senderName.transform.localPosition.y, senderName.transform.localPosition.z);
                    content.transform.localPosition = new Vector3(senderName.transform.localPosition.x + senderName.width + 5.0f, content.transform.localPosition.y, content.transform.localPosition.z);
                    
                    UIWidget barrageWidget = _barrageContent.GetComponent<UIWidget>();
                    curBarrageObj.transform.localPosition = new Vector3(barrageWidget.width, -(barrageWidget.height / nRow) * nRowIndex);
                    
                    float distance = barrageWidget.width + backWidth;
                    float speed = distance / nTotalTime;
                    float nCostTime = (backWidth - 100) / speed;


                    _rowStatus[nRowIndex] = false;

                    TweenPosition twPos = curBarrageObj.GetComponent<TweenPosition>();
                    twPos.duration = nTotalTime;
                    twPos.from = curBarrageObj.transform.localPosition;
                    twPos.to = new Vector3(-distance, curBarrageObj.transform.localPosition.y);
                    twPos.ResetToBeginning();
                    twPos.enabled = true;
                    StartCoroutine(changeBarrageLineStatus(nRowIndex, nCostTime, curBarrageObj));

                    tempBarrageList.RemoveAt(0);
                }

            }
        }
    }

    int getBarradgeType(int nID)
    {
        int nType = 1;
        if(nID == 1500002)
        {
            nType = 2;
        }
        return nType;
    }

    IEnumerator changeBarrageLineStatus(int nRowIndex,float nTime,GameObject curBarrageObj)
    {
        yield return new WaitForSeconds(nTime);
        if(_rowStatus.ContainsKey(nRowIndex))
        {
            _rowStatus[nRowIndex] = true;
        }
        yield return new WaitForSeconds(nTotalTime - nTime);
        curBarrageObj.transform.position = new Vector3(1000, 1000, curBarrageObj.transform.position.z);
        TweenPosition twPos = curBarrageObj.GetComponent<TweenPosition>();
        twPos.enabled = false;
        PoolManager.getInstance().Despawn(curBarrageObj);
    }
}
