using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Utils;

public class AllServerBroadcastMono : MonoBehaviour
{
    private AllServerBroadcastController controller;
    private Transform sNotice;
    private GameObject noticeBg;
    private Vector3 _startPoint;

    private bool _bMoving = false;

    // Use this for initialization
    void Start()
    {
        SetBroadcastContent();
    }

    void Awake()
    {
        controller = UIManager.GetControler(UIName.BROADCAST_WIN) as AllServerBroadcastController;

        noticeBg = transform.Find("Container/Sprite").gameObject;
        sNotice = transform.Find("Container/Sprite/Panel/Label");
        _startPoint = sNotice.localPosition;
        var panel = transform.Find<GameObject>("Container/Sprite/Panel");
        var gamePanel = transform.GetComponent<UIPanel>();
        if (gamePanel != null){
            controller.SetUIPanelRenderQueue(panel, 1);
        }
    }

    #region 设置广播内容

    public void SetBroadcastContent()
    {
        if (_bMoving){
            return;
        }
        if (controller.broadcastContent.Count > 0){
            _bMoving = true;
            var lab = sNotice.GetComponent<UILabel>();
            lab.text = controller.broadcastContent[0];
            sNotice.localPosition = new Vector3(_startPoint.x + lab.width, 0f, 0f);
            Invoke("NoticeMove", 0.5f);
            controller.broadcastContent.RemoveAt(0);
        }
    }

    #endregion

    #region 文字飘动

    private void NoticeMove()
    {
        noticeBg.SetActive(true);
        if (_startPoint == null){
            FinishAndDestory();
        }
        float currentWidth = sNotice.GetComponent<UILabel>().width;
        float currentX = _startPoint.x + currentWidth/2;
        Hashtable moveArg = new Hashtable();
        moveArg.Add("speed", 100);
        moveArg.Add("islocal", true);
        moveArg.Add("x", currentX * (-1.0f));
        moveArg.Add("easetype", iTween.EaseType.linear);
        moveArg.Add("oncomplete", "FinishAndDestory");
        moveArg.Add("oncompletetarget", gameObject);

        iTween.MoveTo(sNotice.gameObject, moveArg);
    }

    #endregion

    #region 显示完成后销毁界面

    private void FinishAndDestory()
    {
        if (controller.broadcastContent.Count == 0){
            _bMoving = false;
            UIManager.DestroyWin(UIName.BROADCAST_WIN);
        }
        else{
            _bMoving = false;
//            noticeBg.SetActive(false);
            SetBroadcastContent();
        }
    }

    #endregion
}