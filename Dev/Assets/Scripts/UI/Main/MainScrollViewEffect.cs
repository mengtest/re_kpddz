using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyExtensionMethod;
using SLua;
using UI.Controller;
using Utils;

[CustomLuaClass]
public class MainScrollViewEffect : BaseMono
{
    [DoNotToLua] private GameObject _container;
    [DoNotToLua] private UIGridCellMgr _gridMgr;
    [DoNotToLua] private int _index = 0;
    [DoNotToLua] private Dictionary<int, float> _scaleDic = new Dictionary<int, float>();
    [DoNotToLua] private Transform _center;
    [DoNotToLua] private UIScrollView _scrollView;
    [DoNotToLua] private bool isDrag = false;
    [DoNotToLua] private UIPanel _scrollPanel;
    [DoNotToLua] public float itemWight = 250f;

    public int CenterIndex
    {
        get { return _index; }
    }

    [DoNotToLua]
    protected override void ReplaceAwake()
    {
        _container = FindGameObject("Container/right/ScrollView/Container");
        _gridMgr = FindComponent<UIGridCellMgr>("Container/right/ScrollView/Container");
        _center = FindTransform("Container/right/center");
        _scrollView = FindComponent<UIScrollView>("Container/right/ScrollView");
        _scrollPanel = _scrollView.transform.GetComponent<UIPanel>();
        if (_scrollView != null){
            if (_scrollPanel != null){
                _scrollPanel.onClipMove += OnMoveUpdate;
            }
            _scrollView.onStoppedMoving += OnMoveStop;
        }
    }


    public void TweenTo(int index)
    {
        _index = index;
//        InitScaleDic();
        iTween.Stop(_container);
        var args = new Hashtable();
        args.Add("time", 0.5f);
        args.Add("islocal", true);
//        args.Add("easetype",iTween.EaseType.spring);
        args.Add("x", -index * itemWight);
        args.Add("onupdate", "OnUpdateHandler");
        args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "OnComplete");
        args.Add("oncompletetarget", gameObject);
        iTween.MoveTo(_container, args);
    }

    public void InitScaleDic()
    {
        for (int i = 0; i < _container.transform.childCount; i++){
            var uiW = _container.transform.Find<UISprite>("cell" + (i+1));
            var t = _container.transform.Find("cell" + (i+1));
            if (t != null){
                float dis = Vector3.Distance(t.position, _center.position);
                float temScale = dis >= 1.5f ? .5f : (1f - .5f * (dis / 1.5f));
                if (uiW != null){
                    uiW.alpha = temScale;
                }
                t.localScale = new Vector3(temScale, temScale,temScale);
            }
        }
    }

    [DoNotToLua]
    private void OnComplete()
    {
//        LogSys.LogWarning("-----> complete");
    }

    [DoNotToLua]
    private void OnUpdateHandler()
    {
        float tmp = 1f / _container.transform.childCount;
        for (int i = 0; i < _container.transform.childCount; i++){
            var uiW = _container.transform.Find<UIWidget>("cell" + (i+1));
            var t = _container.transform.Find("cell" + (i+1));
            if (t != null){
                float dis = Vector3.Distance(t.position, _center.position);
                float temScale = dis >= 1.5f ? .5f : (1f - .5f * (dis / 1.5f));
                if (uiW != null){
                    uiW.alpha = temScale;
                }
                t.localScale = new Vector3(temScale, temScale,temScale);
            }
        }
    }

    [DoNotToLua]
    private void OnMoveUpdate(UIPanel panel)
    {
        if (_scrollView.isDragging == false) return;
//        LogSys.LogWarning("------> panelMove");
        iTween.Stop(_container);
        isDrag = true;
        OnUpdateHandler();
    }

    [DoNotToLua]
    private void OnMoveStop()
    {
        if (!isDrag) return;
        iTween.Stop(_container);
//        LogSys.LogWarning("......>>>>> OnMoveStop");
        if (!_scrollView.isDragging) isDrag = false;
        var index = 0;
        float tmp = _container.transform.childCount * 0.5f;
        for (int i = 0; i < _container.transform.childCount; i++){
            var t = _container.transform.Find("cell" + (i+1));
            if (t == null) continue;
            float dis = Vector3.Distance(t.position, _center.position);
            float absDic = Mathf.Abs(dis);
            if (i == 0){
                tmp = absDic;
            }
            else if (absDic < tmp){
                tmp = absDic;
                index = i;
            }
        }
        if (Mathf.Abs(_scrollView.transform.localPosition.x) > 0.001f){
            _container.transform.localPosition = new Vector3(
                _scrollView.transform.localPosition.x + _container.transform.localPosition.x, 0f);
            _scrollView.transform.localPosition = Vector3.zero;
            _scrollPanel.clipOffset = Vector2.zero;
        }
        _index = index;
        float time = tmp <= 0f ? 0f : tmp / 0.5f;
        if (tmp > 0.1f){
            var args = new Hashtable();
            args.Add("time", time);
            args.Add("islocal", true);
            args.Add("x", -index * itemWight);
            args.Add("onupdate", "OnUpdateHandler");
            args.Add("onupdatetarget", gameObject);
            args.Add("oncomplete", "OnComplete");
            args.Add("oncompletetarget", gameObject);
            iTween.MoveTo(_container, args);
        }
        else{
            OnComplete();
        }


        UIManager.CallLuaFuncCall("MainWinMono:OnSetIndex", gameObject);
    }
}