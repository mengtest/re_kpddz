using UnityEngine;
using System.Collections;
using EventManager;
using UI.Controller;
using Utils;

public class QueueLoginMono : MonoBehaviour
{
    private QueueLoginController _ctrl;
    //private LoginInputController _ctrlLog;
    private GameObject _bg;
    private GameObject cancelButton;
    private UILabel serverNumber;
    private UILabel descLabel;
    private UILabel timeLabel;
    private UILabel buttonLabel;
    private uint currNum = 0;
    private uint _speed = 5;

    void Awake()
    {
       // _ctrlLog = UIManager.GetControler<LoginInputController>();
        _ctrl = UIManager.GetControler<QueueLoginController>();
        _bg = transform.Find("bg").gameObject;
        cancelButton = transform.Find("bg/cancelButton").gameObject;
        serverNumber = transform.Find("bg/serverBg/serverNumberLabel").GetComponent<UILabel>();
        descLabel = transform.Find("bg/descLabel").GetComponent<UILabel>();
        buttonLabel = transform.Find("bg/cancelButton/buttonLabel").GetComponent<UILabel>();
        timeLabel = transform.Find("bg/timeLabel").GetComponent<UILabel>();

        _ctrl.RegisterUIEvent(QueueLoginController.UPDATE_SHOW,OnUpdateShow);
    }
	
	void Start ()
	{
	    UIEventListener.Get(cancelButton).onClick = OnCancelWait;
        SetCenterMsg(currNum);
        //服务器名称
        if (GameDataMgr.LOGIN_DATA != null)
        {
	        serverNumber.text = GameDataMgr.LOGIN_DATA.serverName;
	    }
	    else
	    {
	        serverNumber.text = "";
	    }

        WaitTime(currNum);
	}
    /// <summary>
    /// 显示人数
    /// </summary>
    private void Repeat()
    {
        if (currNum <= _speed){
            currNum = 0;
        }else{
            currNum -= _speed;
        }
        SetCenterMsg(currNum);
        WaitTime(currNum);
        if (currNum > 0)
        {
            Invoke("Repeat", 1f);
        }else{
 //           _ctrlLog.LoginAgain();
            
        }
    }
    private void OnCancelWait(GameObject go)
    {
//         var loginCtrl = UIManager.GetControler<LoginInputController>();
//         if (loginCtrl != null)
//         {
//             loginCtrl.QuitQueueLogin();
//         }
        UIManager.DestroyWin(UIName.QUEUE_LOGIN_WIN);
    }

    /// <summary>
    /// 中间信息
    /// </summary>
    /// <param name="totalRank">排队人数</param>
    private void SetCenterMsg(uint total)
    {
        descLabel.text = GameText.Format("queueCenterLabel", total);
    }
    /// <summary>
    /// 排队时间
    /// </summary>
    /// <param name="currNum">排队总人数</param>
    private void WaitTime(uint currNum)
    {
        if(currNum<=0) return;
        uint totalSec =( currNum+300)/_speed;
        uint hour = totalSec/3600;
        if (hour <= 0)
        {
            hour = 0;
        }
        uint minute = (totalSec - hour * 3600) / 60;
        timeLabel.text = GameText.Format("queueTimeLabel", hour, minute);
    }

    public void UpdateQueue(uint queue, uint Speed)
    {
        currNum = queue;
        _speed = Speed;
        if (currNum>0)
        {
            Invoke("Repeat", 1f);
            SetCenterMsg(currNum);
        }      
    }

    private void OnUpdateShow(EventMultiArgs args)
    {
        uint queue = args.GetArg<uint>("queue");
        uint enterSpeed = args.GetArg<uint>("enterSpeed");
        CancelInvoke("Repeat");
        UpdateQueue(queue,enterSpeed);
    }
}
