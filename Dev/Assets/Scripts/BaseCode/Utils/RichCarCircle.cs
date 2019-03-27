using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using System.Collections.Generic;
using Msg;
using network;
using network.protobuf;
public class RichCarCircle : MonoBehaviour {

	// Use this for initialization
    List<TweenAlpha> _tweenList = new List<TweenAlpha>();
    List<UISprite> _spList = new List<UISprite>();
    List<Vector2> _posList = new List<Vector2>();
    public GameObject _effect;
    public List<EventDelegate> onComplete = new List<EventDelegate>();
    private int _startIndex = 1;
    private int _toIndex = 10;
    private int _totalCount = 5;

    private bool _start = false;
    private int _moveCount = 0;
    private int _nowSelect = 0;
    private float _passTime = 0f;
    private float _dur = 0.3f;
    float _initDur = 0.3f;
    private int _count = 0;
    private Vector3 _effectPos = new Vector3(0, 0, 0);
    private int _winRender = 0;
    private bool _isStop = false;
    private int _flashCount = 0;
    private int _lastIndex = -1;
    private List<Transform> _trList = new List<Transform>();
    bool _isColor = false;
    float[] _angleArr = new float[] { -90f, -115.5f, -136.9f, -158.4f, -179.9f, 155.4f, 133f, 112.1f, 90.1f, 65.4f, 42.4f, 21.79f, 0f, -24.8f, -46.3f, -67.8f };
    bool isPlaySound = false;
	void Start () {
        if(_effect!=null){
            _effectPos = _effect.transform.localPosition;
        }
	}
    private void SetEffect(int index)
    {

//        Vector2 v2 = new Vector2(0, 0) - pos;
//        float angle = (float)(180 * Math.Atan2(v2.y, v2.x) / Math.PI);

        _effect.transform.localEulerAngles = new Vector3(0, 0, _angleArr[index]);
    }
    void Update()
    {
        if(!_start)
            return;
        _passTime += Time.deltaTime;
        if (_flashCount>0)
        {
            if (isPlaySound)
            {
                isPlaySound = false;
                UtilTools.PlaySoundEffect("Sounds/RichCar/roulette20f", 0.3f * _flashCount);
            }
            if(_passTime>= 0.3f)
            {
                if (_flashCount % 2 == 0)
                {
                    for (int i = 0; i < _spList.Count; i++)
                    {
                        _spList[i].alpha = 1f;
                    }
                }
                else
                {
                    for (int i = 0; i < _spList.Count; i++)
                    {
                        _spList[i].alpha = 0f;
                    }
                }
                _passTime = 0f;
                _flashCount--;
                if (_flashCount == 3)
                {
                    UtilTools.SetBgm("Sounds/RichCar/roulette20");
                }
            }
            return;
        }
        if (_passTime >= _dur)
        {
            if (_moveCount == 0)
            {
                _start = false;
                _spList[_toIndex].alpha = 1f;
                if (_lastIndex<_trList.Count && _lastIndex>=0)
                {
                    _trList[_lastIndex].localScale = new Vector3(1f, 1f, 1f);
                    _lastIndex = _toIndex;
                    _trList[_toIndex].localScale = new Vector3(1.1f, 1.1f, 1.1f);
                }
                _passTime = 0f;
                if (_effect != null)
                    _effect.transform.localPosition = new Vector3(10000, 0, 0);
                if (onComplete.Count > 0)
                {
                    EventDelegate.Execute(onComplete);
                }
                
                return;
            }
            _passTime = _passTime - _dur;
            StartTweenAlpha(_nowSelect);
            _nowSelect += 1;
            _moveCount--;
            _count++;
            if (_count <= 2)
            {
                
            }
            else
            {
                if (_count == 16)
                {
                    ResetEffect();
                }
                if (_moveCount <= 20)
                {
                    if (_moveCount <= 6 && !_isStop)
                    {
                        _effect.transform.localPosition = new Vector3(10000, 0, 0);
                        _isStop = true;
                    }
                    _dur = _dur * 1.1f;
                    if (_dur >= 0.5f)
                    {
                        _dur = 0.5f;
                    }
                    if (_moveCount <= 5)
                    {
                        _dur *= 1.2f;
                    }
                }
                else
                {
                    _dur = _dur / 1.1f;
                    if (_dur <= 0.05f)
                    {
                        _dur = 0.05f;
                    }
                }
            }
        }
    }

    private int GetPlayIndex(int index)
    {
        if (index < 0)
        {
            return (index + 16) % 16;
        }
        else
        {
            return index%16;
        }
    }
    private void StartTweenAlpha(int playIndex)
    {
        playIndex = GetPlayIndex(playIndex);
        if (_lastIndex == -1)
        {
            _lastIndex = playIndex;
        }
        else
        {
            _trList[_lastIndex].localScale = new Vector3(1f, 1f, 1f);
            _lastIndex = playIndex;
        }

        if (playIndex < _spList.Count && playIndex < _tweenList.Count)
        {
            if (!_isStop && _isColor)
            {
                SetEffect(playIndex);
            }
            _tweenList[playIndex].from = 1;
            _tweenList[playIndex].to = 0;
            _tweenList[playIndex].delay = 0;
            _tweenList[playIndex].duration = 0.8f;
            _tweenList[playIndex].style = 0;
            _tweenList[playIndex].ResetToBeginning();
            _tweenList[playIndex].value = 1;
            //_tweenList[playIndex].enabled = true;
            _tweenList[playIndex].PlayForward();
            _trList[playIndex].localScale = new Vector3(1.1f, 1.1f, 1.1f);
            UtilTools.PlaySoundEffect("Sounds/RichCar/circle", taget: _tweenList[playIndex].gameObject);
        }
    }
	public void AddTweenAlpha(TweenAlpha tween,UISprite sp,GameObject go)
    {
        _tweenList.Add(tween);
        _spList.Add(sp);
        _posList.Add(transform.InverseTransformVector(sp.transform.position));
        _trList.Add(go.transform);
    }
    public void SetParam(float dur,int total)
    {
        _initDur = dur;
        _totalCount = total;
    }
    public void SetEffectRender(Transform parent,int value)
    {
        if (_effect != null)
            UtilTools.SetEffectRenderQueueByUIParent(parent, _effect.transform, value);
    }
    private void HideEffect()
    {
        if (_effect != null)
        {
            _effect.transform.localPosition = new Vector3(10000, 0, 0);
        }
        
    }
    private void ResetEffect()
    {
        if (_effect != null && _isColor)
        {
            _effect.transform.localPosition = _effectPos;
        }
    }
    public void StartCircle(int stSel,int toSel,int flashCount)
    {
        _startIndex = stSel;
        _nowSelect = stSel;
        _toIndex = toSel;
        _dur = _initDur;
        SetEffect(_startIndex);
        HideEffect();
        _isStop = false;
        _start=true;
        _count = 0;
        _moveCount = _totalCount * 16 + toSel - stSel;
        _flashCount = flashCount;
        if (flashCount > 0)
        {
            isPlaySound = true;
            //_tweenList[_nowSelect].enabled = false;
            _spList[_nowSelect].alpha = 0f;
        }
    }
    public void StartColorCircle(int stSel,int toSel)
    {
        _startIndex = stSel;
        _nowSelect = stSel;
        _toIndex = toSel;
        _dur = _initDur;
        SetEffect(_startIndex);
        HideEffect();
        _isStop = false;
        _start = true;
        _count = 0;
        _moveCount = _totalCount * 16 + toSel - stSel;
        _flashCount = 6;
        _isColor = true;
        for (int i = 0; i < _spList.Count; i++) {
            _spList[i].spriteName = "bigselect";
        }
        //_tweenList[_nowSelect].enabled = false;
        _spList[_nowSelect].alpha = 0f;
        isPlaySound = true;

    }
    public void ResetSelect()
    {
        if (_isColor)
        {
            _isColor = false;
            for (int i = 0; i < _spList.Count; i++)
            {
                _spList[i].spriteName = "carselect";
            }
        }
    }
}
