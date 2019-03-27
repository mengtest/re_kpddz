/***************************************************************


 *
 *
 * Filename:  	DetialTipsWinMono.cs	
 * Summary: 	
 *
 * Version:   	1.0.0
 * Author: 		HR.Chen
 * Date:   		2016/3/14 17:42:18
 ***************************************************************/

#region Using
using Scene;
using EventManager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UI.Controller;
using HeroData;
using network;

using System.Collections;
#endregion


internal class SystemTipsWinMono : BaseMono
{
    #region Variable

    private SystemTipsWinController _controller;
    private Transform _position;
    private GameObject _tipCell;

    private float _appearTime = 0.5f;
    private float _move = 0;
    private float _delay = 5f;
    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void ReplaceAwake()
    {
        findObject(transform.Find("Container"));
        _move = _position.GetComponent<UIWidget>().localSize.y;

    }

    private void findObject(Transform tr)
    {
        _controller = UIManager.GetControler(UIName.SYSTEM_TIPS_WIN) as SystemTipsWinController;
        _position = tr.Find("TipsWin");
        _tipCell = tr.Find("Sprite").gameObject;
    }
    void Start()
    {
        if (_controller._message.Count > 0)
        {
            AddItem(_controller._message[0]);
        }
    }
    private void AddItem(string msg)
    {
        GameObject go = NGUITools.AddChild(_position.gameObject, _tipCell);
        go.transform.Find("tip").GetComponent<UILabel>().text = msg;
        ShowEffect(go);

    }
    private void ShowEffect(GameObject go)
    {
        Hashtable args = new Hashtable();
        TweenAlpha tweenAlphabg = TweenAlpha.Begin(go, _appearTime, 1);
        tweenAlphabg.from = 0f;
        args.Clear();
        args.Add("easeType", iTween.EaseType.linear);
        args.Add("time", _appearTime);
        args.Add("y", go.transform.localPosition.y - _move);
        args.Add("islocal", true);
        args.Add("oncomplete", "OnMoveComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("oncompleteparams", go);
        iTween.MoveTo(go, args);

        TweenAlpha tweenAlpha = TweenAlpha.Begin(go, _appearTime, 0);
        tweenAlpha.delay = _delay;
        tweenAlpha.from = 1f;


        args.Clear();
        args.Add("easeType", iTween.EaseType.linear);
        args.Add("time", _appearTime);
        args.Add("y", go.transform.localPosition.y);
        args.Add("delay", _delay);
        args.Add("islocal", true);
        args.Add("oncomplete", "OnActionComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("oncompleteparams", go);
        iTween.MoveTo(go, args);
    }
    private void OnActionComplete(GameObject go)
    {
        Destroy(go);
        _controller.RemoveMessage();
        if (_controller._message.Count > 0)
        {
            AddItem(_controller._message[0]);
        }
        else
        {
            _controller.GoBack();
        }
        
    }
}