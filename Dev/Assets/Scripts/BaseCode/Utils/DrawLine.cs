using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Utils;
using effect;
class DrawLine : MonoBehaviour
{
    private UISprite _lineCell;
    Material _mat = null;
    List<Vector3> _posList = new List<Vector3>();
    float length = 0f;
    int RIBBON_MAX = 9;
    public GameObject effectObj = null;
    public int _render = 0;

    
    List<Vector3> _movePosList = new List<Vector3>();
    void OnEffectLoadComplete(EffectObject efo)
    {
        effectObj = efo.EffectGameObj;
        effectObj.SetActive(false);
        SetEffectRender();
    }
    void SetEffectRender()
    {
        if (_render != 0 && effectObj != null) { UtilTools.SetEffectRenderQueue(effectObj.transform, _render); }
    }
    void Awake()
    {
        _lineCell = transform.Find("line").GetComponent<UISprite>();
        _lineCell.gameObject.SetActive(false);
        EffectObject efo = EffectManager.getInstance().addEffect(transform, UIPrefabPath.EFFECT_FRUIT_BALL, isAutoDestroy: false);
        efo._loadComplete = OnEffectLoadComplete;
    }
    

    IEnumerator ShowEffect(int index, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        Move(index, duration);

    }
    void Move(int index,float duration)
    {
        if (index == 0 || index >= _movePosList.Count)
        {
            effectObj.SetActive(false);
            return;
        }
        if (effectObj == null)
            return;
        TweenPosition tweenPosition = effectObj.GetComponent<TweenPosition>();
        if (tweenPosition == null)
        {
            tweenPosition = effectObj.AddComponent<TweenPosition>();
        }
        tweenPosition.ResetToBeginning();
        effectObj.transform.localPosition = _movePosList[index - 1];
        tweenPosition.enabled = true;
        tweenPosition.from=_movePosList[index-1];
        tweenPosition.to = _movePosList[index];
        tweenPosition.PlayForward();
        tweenPosition.duration = duration;
    }
    public void MoveEffect(float nTime)
    {
        if (effectObj == null)
            return;
        float speed = length / nTime;
        float delay = 0f;
        for (int i = 0; i < _movePosList.Count; i++)
        {
            if (i == 0)
            {
                effectObj.transform.localPosition = _movePosList[i];
                effectObj.SetActive(true);
            }
            else
            {
                float distance = Vector3.Distance(_movePosList[i - 1], _movePosList[i]);
                float duration = distance / speed;
                StartCoroutine(ShowEffect(i, delay, duration));
                delay += duration;
            } 
        }
        StartCoroutine(ShowEffect(_movePosList.Count, nTime+0.2f, 0f));
    }
    /*
    public void SetPosition(Vector3[] posArray)
    {
        for (int i = 0; i < posArray.Length; i++)
        {
            _posList.Add(posArray[i]);
        }
    }*/
    public void SetLineColor(int colorIndex,int render)
    {   
        _lineCell.spriteName = "l" + colorIndex;
        _render = render;
        SetEffectRender();
    }

    public void SetPosition(Vector3 pos)
    {
        _posList.Add(transform.InverseTransformVector(pos));
    }
    public void DrawRenderLine()
    {
        Vector3 prePoint = Vector3.zero;
        int direct = 0;
        int nowDirect = 0;
        GameObject go = null;
        Vector3 lastStartPoint = new Vector3();
        float lineLenth=0;
        Vector2 v2;
        float angle = 0f;
        Vector3 pos;
        UISprite sp;
        for (int i = 0; i < _posList.Count; i++)
        {
            if (i == 0)
            {
                go = NGUITools.AddChild(gameObject, _lineCell.gameObject);
                lastStartPoint=_posList[i];
                go.SetActive(true);
                _movePosList.Add(_posList[i]);
            }
            else
            {
                float dis = Vector3.Distance(prePoint, _posList[i]);
                if (prePoint.y > _posList[i].y)
                {
                    nowDirect = 1;
                }
                else if (prePoint.y < _posList[i].y)
                {
                    nowDirect = -1;
                }
                else
                {
                    nowDirect = 0;
                }
                if (i !=1 && direct != nowDirect)
                {
                    v2 = new Vector2(prePoint.x, prePoint.y) - new Vector2(lastStartPoint.x, lastStartPoint.y);
                    angle = (float)(180 * Math.Atan2(v2.y, v2.x) / Math.PI);
                    pos = Vector3.Lerp(lastStartPoint, prePoint, 0.5f);
                    sp = go.GetComponent<UISprite>();
                    sp.transform.localEulerAngles = new Vector3(0, 0, angle);
                    sp.transform.localPosition = new Vector3(pos.x,pos.y,0);
                    sp.width = (int)lineLenth+3;
                    lineLenth = dis;
                    lastStartPoint = prePoint;
                    _movePosList.Add(lastStartPoint);
                    go = NGUITools.AddChild(gameObject, _lineCell.gameObject);
                    go.SetActive(true);
                    direct = nowDirect;
                }
                else
                {
                    direct = nowDirect;
                    lineLenth += dis;
                }
                length += dis;
            }
            prePoint = _posList[i];
            
        }
        v2 = new Vector2(prePoint.x, prePoint.y) - new Vector2(lastStartPoint.x, lastStartPoint.y);
        angle = (float)(180 * Math.Atan2(v2.y, v2.x) / Math.PI);
        pos = Vector3.Lerp(lastStartPoint, prePoint, 0.5f);
        sp = go.GetComponent<UISprite>();
        sp.transform.localEulerAngles = new Vector3(0, 0, angle);
        sp.transform.localPosition = new Vector3(pos.x, pos.y, 0); 
        sp.width = (int)lineLenth;
        _movePosList.Add(prePoint);
        /*
        lineRenderer.SetVertexCount(posArray.Length);
        lineRenderer.SetPositions(posArray);*/
    }



}