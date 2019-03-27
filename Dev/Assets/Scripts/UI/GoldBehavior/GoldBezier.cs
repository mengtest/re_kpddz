/***************************************************************


 *
 *
 * Filename:  	GoldBezier.cs	
 * Summary: 	金币贝塞尔移动
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

class GoldBezier : MonoBehaviour
{

    bool _bStart = false;
    //PathBezier path_bezier;
    Vector3 _startpos;
    Vector3 _endpos;
    float _nTime = 0.0f;
    float _nJourney = 0.0f;
    float _nTotalTime = 0.0f;
    float _nStayTime = 0.0f;
    public bool _bLastCoin = false;
    void Awake()
    {

    }

    public void setBezier(Vector3 startPos,Vector3 endPos,int nIndex,float nTime,float nStayTime)
    {

        Vector3 midPos = new Vector3(endPos.x, startPos.y, startPos.z);

        _startpos = startPos;
        _endpos = endPos;

        //path_bezier = new PathBezier(startPos, midPos, midPos, endPos);

        Vector3 point_1 = _startpos;
        point_1.z = 0f;
        Vector3 point_2 = _endpos;
        point_2.z = 0f;
        _nJourney = Vector3.Distance(point_1, point_2);
        _nTotalTime = _nJourney / 3f;
        _nStayTime = nStayTime;

        //Utils.LogSys.Log("Distance-------------->" + _nJourney);
        Invoke("StartPlayAnimation", nIndex * nTime);
        //StartPlayAnimation();
    }

    void StartPlayAnimation()
    {
        transform.Find("Icon").localPosition = new Vector3(Vector3.zero.x, Vector3.zero.y + 30, Vector3.zero.z);
        transform.position = _startpos;
        
        PListMono plistMono = transform.Find("Icon").GetComponent<PListMono>();
        int nFrame = Random.Range(0,plistMono.frameCount - 1);
        plistMono.nCurFrame = nFrame;
        plistMono.enabled = true;
        plistMono.frameSpeed = 0.04f;
        Animator animator = transform.Find("Icon").GetComponent<Animator>();
        animator.enabled = true;
        animator.Rebind();
        animator.Play("jinbidiaoluoAnimation");

        Invoke("changeFreameSpeed", 0.4f);
        Invoke("StartBezier", _nStayTime);

        //StartBezier();
    }

    void changeFreameSpeed()
    {
        PListMono plistMono = transform.Find("Icon").GetComponent<PListMono>();
        plistMono.frameSpeed = 0.055f;
    }

    void StartBezier()
    {
        _bStart = true;
        //PListMono plistMono = transform.Find("Icon").GetComponent<PListMono>();
        //plistMono.enabled = false;
    }

    void Update()
    {
        if(_bStart)
        {
            Vector3 curPoint = Vector3.zero;
            //path_bezier.GetPointAtTime((float)_nCount / (float)70, out curPoint);
            //transform.localPosition = curPoint;
            //_nCount++;
            //if(curPoint == path_bezier.p3)
            //{
            //    transform.localPosition = new Vector3(1000, 1000, transform.localPosition.z);
            //    clearData();
            //    GoldBehavior.getInstance().setCoinInList(gameObject, false);
            //}


            //float timeX = Mathf.Abs(path_bezier.p0.x - path_bezier.p3.x) / 0.01f;
            //float timeY = Mathf.Abs(path_bezier.p0.y - path_bezier.p3.y) / 0.01f;

            _nTime += Time.deltaTime;
            Vector3 pos = Vector3.Lerp(_startpos, _endpos, _nTime / _nTotalTime);
//             float curPointX = 0.0f;
//             float curPointY = 0.0f;
//             if (path_bezier.p0.x > path_bezier.p3.x)
//                 curPointX = path_bezier.p0.x - (vX * _nTime);
//             else
//                 curPointX = path_bezier.p0.x + (vX * _nTime);
// 
//             if (path_bezier.p0.y > path_bezier.p3.y)
//                 curPointY = path_bezier.p0.y - (vY * _nTime);
//             else
//                 curPointY = path_bezier.p0.y + (vY * _nTime);

            //curPoint = new Vector3(curPointX, curPointY, path_bezier.p0.z);
            transform.position = pos;
            if (Mathf.Abs(pos.x - _endpos.x) <= 0.01 && Mathf.Abs(pos.y - _endpos.y) <= 0.01)
            {
                clearCoinBezierData();
                transform.position = new Vector3(1000, 1000, transform.position.z);

                PListMono plistMono = transform.Find("Icon").GetComponent<PListMono>();
                plistMono.enabled = false;
                Animator animator = transform.Find("Icon").GetComponent<Animator>();
                animator.enabled = false;

                GoldBehavior.getInstance().setCoinInList(gameObject, false);
            }

        }
    }

    void clearCoinBezierData()
    {

        /*BattleMainWinController battleMainController = UIManager.GetControler<BattleMainWinController>();
        if (battleMainController != null && _bLastCoin)
        {
            battleMainController._nBehaviorMoney = GameDataMgr.PLAYER_DATA.Money;
            battleMainController.refreshScoreBehavior(0);
        }*/

        _startpos = Vector3.zero;
        _endpos = Vector3.zero;
        _bStart = false;
        _nTime = 0.0f;
        _nStayTime = 0.0f;
        _bLastCoin = false;
        CancelInvoke();
    }
}