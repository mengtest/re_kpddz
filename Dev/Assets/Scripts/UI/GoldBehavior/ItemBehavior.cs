/***************************************************************


 *
 *
 * Filename:  	ItemBehavior.cs	
 * Summary:     物品表现
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2016/11/8 11:08
 ***************************************************************/
using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Utils;
using Scene;
using Msg;
using network.protobuf;

class ItemBehavior : MonoBehaviour
{
    int _nTimes = 1;//弹出次数
    float _nTime = 0.0f;
    bool _bClickItem = false;
    bool _bStart = false;
    bool _bAuto = false;
    Vector3 _startPos = Vector3.zero;
    Vector3 _endPos = Vector3.zero;
    Transform _ItemIcon;
    int _nItemID;



    void Awake()
    {
        _ItemIcon = transform.Find("img_bg");
        Transform effect = transform.Find("effect_wupindaoju");
        Transform effectPlane = effect.Find("plane");
        effectPlane.localScale = new Vector3(50, 50, 50);
    }

    void Start()
    {

        Transform effect = transform.Find("effect_wupindaoju");
        UtilTools.SetEffectRenderQueueByUIParent(transform, effect, -715);
    }


    void Update()
    {
        if (!_bStart)
            return;
        if (_bClickItem)
            return;

        _nTime += Time.deltaTime;
        if(_nTime/5 >= _nTimes)
        {
            _nTimes++;

            Destroy(_ItemIcon.GetComponent<TweenScale>());

            TweenScale scaleAction = _ItemIcon.gameObject.AddComponent<TweenScale>();
            
            /*if(_nTimes <= 3 && !NewbieGuideManager.getInstance().isNewbieGuide())
            {
                scaleAction.from = new Vector3(1.5f, 1.5f, 1.5f);
                scaleAction.to = new Vector3(2f, 2f, 2f);
                scaleAction.duration = 0.3f;
                scaleAction.enabled = true;
                EventDelegate.Add(scaleAction.onFinished, ScaleBehaviorFinfished, true);
            }
            else if(!NewbieGuideManager.getInstance().isNewbieGuide())
            {
                Destroy(transform.GetComponent<TweenPosition>());
                scaleAction.from = new Vector3(1.5f, 1.5f, 1.5f);
                scaleAction.to = new Vector3(0, 0, 0);
                scaleAction.duration = 0.3f;
                scaleAction.enabled = true;
                EventDelegate.Add(scaleAction.onFinished, ScaleBahaviorReadyToDestory, true);
            }*/
        }
    }

    public void setItemInfo(int nItemID, int nItemType, int nItemCount, Vector3 startpos,bool bAuto = false)
    {
        ItemBaseConfigItem itembasedata = ConfigDataMgr.getInstance().ItemBaseConfig.GetDataByKey(nItemType);
        UtilTools.SetIcon(_ItemIcon, itembasedata.icon);
        _bAuto = bAuto;
        _nItemID = nItemID;


        transform.position = new Vector3(startpos.x,startpos.y,transform.position.z);
        _startPos = transform.position;

        TweenScale scaleAction = _ItemIcon.gameObject.AddComponent<TweenScale>();
        scaleAction.from = new Vector3(0, 0, 0);

        /*if (!NewbieGuideManager.getInstance().isNewbieGuide())
        {
            scaleAction.to = new Vector3(1.5f, 1.5f, 1.5f);
            TweenPosition posAction = transform.gameObject.AddComponent<TweenPosition>();
            posAction.from = transform.localPosition;
            posAction.to = new Vector3(transform.localPosition.x, transform.localPosition.y + 40f, transform.localPosition.z);
            posAction.duration = 1.5f;
            posAction.style = UITweener.Style.PingPong;
        }
        else
        {
            scaleAction.to = new Vector3(1f, 1f, 1f);
        }*/
        scaleAction.duration = 0.5f;
        EventDelegate.Add(scaleAction.onFinished, ScaleBahaviorShowFinish, true);

        UIEventListener.Get(gameObject).onClick = catchItemBehavior;


    }

    void ScaleBahaviorShowFinish()
    {
        Destroy(_ItemIcon.GetComponent<TweenScale>());
        if (!_bAuto)
            _bStart = true;
        else
        {
            TweenScale scaleAction = _ItemIcon.gameObject.AddComponent<TweenScale>();
            scaleAction.from = new Vector3(1.5f, 1.5f, 1.5f);
            scaleAction.to = new Vector3(1.7f, 1.7f, 1.7f);
            scaleAction.duration = 0.5f;
            scaleAction.enabled = true;
            EventDelegate.Add(scaleAction.onFinished, ScaleBehaviorFinfished, true);
        }
    }

    void ScaleBehaviorFinfished()
    {
        if(!_bAuto)
        {
            TweenScale scaleAction = _ItemIcon.GetComponent<TweenScale>();
            scaleAction.PlayReverse();
        }
        else
            catchItemBehavior(null);
    }

    void ScaleBahaviorReadyToDestory()
    {
        _bStart = false;
        Destroy(gameObject);
    }

    void catchItemBehavior(GameObject go)
    {
        if (_bClickItem)
            return;

        _bClickItem = true;

        Destroy(transform.GetComponent<TweenScale>());
        Destroy(transform.GetComponent<TweenPosition>());

        TweenScale scaleAction = _ItemIcon.gameObject.AddComponent<TweenScale>();
        scaleAction.from = new Vector3(1.5f, 1.5f, 1.5f);
        scaleAction.to = new Vector3(0, 0, 0);
        scaleAction.duration = 0.5f;
        EventDelegate.Add(scaleAction.onFinished, ScaleBahaviorEndToDestory, true);



        Transform tf_camera = StartUpScene._uiCamera;
        Camera ui_cam = tf_camera.GetComponent<Camera>();
        Transform tf_scene_camera = StartUpScene._fishCamera;
        Camera scene_cam = tf_scene_camera.GetComponent<Camera>();
//        Vector3 ownerpos = BattleManager.getInstance().GetPlayerPos(GameDataMgr.PLAYER_DATA.UserId.ToString());
//        Vector3 ownerScreenPoint = scene_cam.WorldToScreenPoint(ownerpos);
//        Vector3 pFinalOwner = ui_cam.ScreenToWorldPoint(ownerScreenPoint);
//        Vector3 endPos = new Vector3(pFinalOwner.x, pFinalOwner.y, 0);
//        _endPos = new Vector3(pFinalOwner.x, pFinalOwner.y, transform.position.z);

        transform.position = _endPos;
        Vector3 _endLocalPos = transform.localPosition;
        transform.position = _startPos;

        TweenPosition posAction = transform.gameObject.AddComponent<TweenPosition>();
        posAction.from = transform.localPosition;
        posAction.to = _endLocalPos;
        posAction.duration = 0.5f;
    }

    void ScaleBahaviorEndToDestory()
    {
        /*if (!NewbieGuideManager.getInstance().isNewbieGuide())
        {
            if(_nItemID != 102)
            {
                CS_CBattlePickupRequest req = new CS_CBattlePickupRequest();
                req.itemId = _nItemID;
                ClientNetwork.Instance.SendMsg(ProtoID.CS_CBattlePickupRequest, req);
            }
            Destroy(gameObject);
        }
        else
        {
            SC_CItemAddResponse msg = new SC_CItemAddResponse();
            msg.info = new SC_CItemInfoResponse();
            msg.info.itemId = _nItemID;
            msg.info.count = 1;
            GameDataMgr.ITEM_DATA.ItemAdd(msg);
            NewbieGuideManager.getInstance().goonGuide();
            Destroy(gameObject);
        }*/
    }

}