//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using Scene;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Works together with UIDragCamera script, allowing you to drag a secondary camera while keeping it constrained to a certain area.
/// </summary>

[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Interaction/MoveCameraByPath")]
public class DragCameraByPath : MonoBehaviour
{
    public enum EDragType
    {
        NONE,//纯拖动，无效果
        MOMENTUM,//惯性效果
        PAGEVIEW,//PageView效果（需要指定节点数组）
    }

    public delegate void EventOnDrag(float value);
    public EventOnDrag _onDragEvent = null;
    bool _disableDrag = false;

    private MovePathMono _movePath = null;
    private bool _bPressed = false;
    private Vector3 _pressedMousePos;//点下去时鼠标的位置
    private float _pressedCameraPos;//点下去时镜头的位置(0~1)
    private Camera uiCamera;
    private Vector2 _mMomentum = Vector2.zero;
    private Vector2 _deltaDrag = Vector2.zero;
    private bool _bDrag = false;//true时不能点击场景中的物体
    public bool _bDragDirReverse = false;//反向拖动
    public Vector2 _scale = Vector2.one;
    public EDragType _dragType = EDragType.MOMENTUM;
    public float _curCameraPos;//当前镜头的位置(0~1)

    public List<float> _pageViewPoints = new List<float>();
    float _pageViewTargetPoint = -1f;
    public float _pageViewBeginSlowDistance = 0.03f;
    /// <summary>
    /// How much momentum gets applied when the press is released after dragging.
    /// </summary>
    public float momentumAmount = 65f;
    private Vector3 _lastFrameMousePos = Vector3.zero;
    float _pageMomentum = 60f;
    private EventDelegate.Callback _moveCameraXCallback;

    public bool DisableDrag
    {
        get { return _disableDrag; }
        set { _disableDrag = value; }
    }

    void Awake()
    {
        GameObject camera2 = GameSceneManager.uiCameraObj;
        if (camera2)
            uiCamera = camera2.GetComponent<Camera>();
    }

    void Update()
    {
        if (_disableDrag)
            return;

        if (_movePath == null)
            return;

        if (_bPressed)
        {
            Vector3 mousePose = Input.mousePosition;//cam.ScreenToWorldPoint(Input.mousePosition);//hitt.transform.position; //
            float offsetX = mousePose.x - _pressedMousePos.x;
            if (Mathf.Abs(offsetX) >= 10f)
            {
                if (_bDragDirReverse)
                    _curCameraPos = _pressedCameraPos + offsetX / 2000f;
                else
                    _curCameraPos = _pressedCameraPos - offsetX / 2000f;


                _curCameraPos = Mathf.Clamp(_curCameraPos, 0f, 1f);
                transform.position = _movePath.GetPointAtTime(_curCameraPos);
                _movePath.UpdateRotation();
                if (_onDragEvent != null)
                    _onDragEvent(_curCameraPos);

                
                if (_lastFrameMousePos != Vector3.zero)
                {
                    _deltaDrag.x = _lastFrameMousePos.x - mousePose.x;
                    _deltaDrag.y = _lastFrameMousePos.y - mousePose.y;
                    Vector2 offset = Vector2.Scale(_deltaDrag, -_scale);
                    // Adjust the momentum
                    if (_bDragDirReverse)
                        _mMomentum = Vector2.Lerp(_mMomentum, _mMomentum + offset * (0.01f * momentumAmount), 0.67f);
                    else
                        _mMomentum = Vector2.Lerp(_mMomentum, _mMomentum - offset * (0.01f * momentumAmount), 0.67f);

                   // Utils.LogSys.Log("mMomentum" + _mMomentum.ToString() + ", " + offset.ToString());
                }
                 _lastFrameMousePos = mousePose;
                 _bDrag = true;
            }
            NGUIMath.SpringDampen(ref _mMomentum, 9f, RealTime.deltaTime);
        }
        else
        {
            if (_dragType == EDragType.MOMENTUM)
            {
                //惯性运动
                if (_mMomentum.magnitude > 0.01f)
                {
                    // Apply the momentum
                    Vector3 offsetPos = (Vector3)NGUIMath.SpringDampen(ref _mMomentum, 10f, RealTime.deltaTime);
                    _curCameraPos += offsetPos.x / 2000f;
                    _curCameraPos = Mathf.Clamp(_curCameraPos, 0f, 1f);
                    transform.position = _movePath.GetPointAtTime(_curCameraPos);
                    _movePath.UpdateRotation();
                    if (_onDragEvent != null)
                        _onDragEvent(_curCameraPos);
                }
                else
                {
                    NGUIMath.SpringDampen(ref _mMomentum, 9f, RealTime.deltaTime);
                }
            }
            else if (_dragType == EDragType.PAGEVIEW)
            {
                //滑到指定点
                if (_pageViewTargetPoint != -1f)
                {
                    bool bStop = false;
                    if (_mMomentum.magnitude > 0.01f)//正在滑动
                    {
                        float distance = _pageViewTargetPoint - _curCameraPos;
                        if (Mathf.Abs(distance) > 0.01f)
                        {
                            float dir = _bDragDirReverse?-1f:1f;
                            if (Mathf.Abs(distance) < _pageViewBeginSlowDistance)
                            {
                                Vector3 offsetPos = (Vector3)NGUIMath.SpringDampen(ref _mMomentum, 10f, RealTime.deltaTime);
                                _curCameraPos -= dir*offsetPos.x / 2000f;
                            }
                            else
                            {
                                _curCameraPos -= dir*_mMomentum.x / 2000f;
                            }
                            _curCameraPos = Mathf.Clamp(_curCameraPos, 0f, 1f);
                            float newDistance = _pageViewTargetPoint - _curCameraPos;
                            if (newDistance * distance > 0f)//还没到目标
                            {
                                transform.position = _movePath.GetPointAtTime(_curCameraPos);
                                _movePath.UpdateRotation();
                            }
                            else//超过目标点了
                            {
                                bStop = true;
                            }
                        }
                        else//已接近目标
                        {
                            bStop = true;
                        }
                    }
                    else//停止滑动
                    {
                        bStop = true;
                    }

                    if (bStop)
                    {
                        _curCameraPos = _pageViewTargetPoint;
                        transform.position = _movePath.GetPointAtTime(_curCameraPos);
                        _movePath.UpdateRotation();
                        if (_onDragEvent != null)
                            _onDragEvent(_curCameraPos);
                        _mMomentum = Vector2.zero;
                        _pageViewTargetPoint = -1f;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))//点下去
        {
            if (!UtilTools.ClickUI() && UtilTools.ClickInValidArea()) //没点到UI && 点的是有效区
            {
                if (_dragType != EDragType.NONE)
                {
                    //还在滑动
                    if (_mMomentum.magnitude > 1f)  _bDrag = true;
                    else _bDrag = false;
                }
                else
                    _bDrag = false;

                _bPressed = true;
                _pressedMousePos = Input.mousePosition;
                _pressedCameraPos = _curCameraPos;
                _mMomentum = Vector2.zero;
                _deltaDrag = Vector2.zero;
                _lastFrameMousePos = Vector3.zero;
            }
        }
        else if (Input.GetMouseButtonUp(0))//弹起
        {
            _bPressed = false;
            if (_dragType == EDragType.PAGEVIEW)
            {
                if (_pageViewPoints.Count == 0)
                {
                    _pageViewTargetPoint = -1;
                }
                else
                {
                    int dir = 1;
                    Vector3 offsetPos = (Vector3)NGUIMath.SpringDampen(ref _mMomentum, 10f, RealTime.deltaTime);
                    if (Mathf.Abs(offsetPos.x) > 1f) {//拖动时，翻到拖动方向的那一页
                    
                        int offPage = -1;//-1表示前一页，1表示后一页
                        if (_mMomentum.x > 0f)
                        {
                            offPage = 1;
                        }
                        _pageViewTargetPoint = GetTargetPage(_curCameraPos, offPage, ref dir);
                    }
                    else//原地弹起时，靠近哪页就滚动到哪一页
                    {
                        _pageViewTargetPoint = GetTargetPage(_curCameraPos, 0, ref dir);
                    }
                    if (_pageViewTargetPoint == -1f || Mathf.Abs(_curCameraPos - _pageViewTargetPoint) <= 0.01f)
                    {
                        _mMomentum = Vector2.zero;
                    }
                    else
                    {
                        _mMomentum = new Vector2(dir * _pageMomentum, 0f);
                    }
                }
            }
            //Utils.LogSys.Log("camera path value " + _curCameraPos);
        }
    }

    /// <summary>
    /// 移动镜头到
    /// </summary>
    /// <param name="t">时间</param>
    /// <param name="to">值：0-1f</param>
    /// <param name="cb">回调</param>
    public void MoveCameraTo(float t, float to, EventDelegate.Callback cb) {
        _moveCameraXCallback = cb;
        Hashtable args = new Hashtable();
        args.Add("time", t);
        args.Add("from", _curCameraPos);
        args.Add("to", to);
        args.Add("onupdate", "onMoveCameraUpdate");
        args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "onMoveCameraComplete");
        args.Add("oncompletetarget", gameObject);
        iTween.ValueTo(gameObject, args);
    }

    void onMoveCameraUpdate(float value) {
        _curCameraPos = value;
        transform.position = _movePath.GetPointAtTime(value);
        _movePath.UpdateRotation();
        if (_onDragEvent != null)
            _onDragEvent(_curCameraPos);
    }

    void onMoveCameraComplete() {
        if (_moveCameraXCallback != null)
            _moveCameraXCallback();
    }

    //offsetPage-1表示前一页，1表示后一页，0时表示靠近哪页就滚动到哪一页
    private float GetTargetPage(float cruPos, int offsetPage, ref int dir)
    {
        if (_movePath == null)
            return -1f;//表示没有找到目标

        if (_pageViewPoints.Count == 0)
            return -1f;//表示没有找到目标

        if (_pageViewPoints.Count == 1)
            return _pageViewPoints[0];//只有一个目标

        //2个以上页时
        for (int i = 0; i < _pageViewPoints.Count-1; i++ )
        {
            float perPage = _pageViewPoints[i];
            float nextPage = _pageViewPoints[i+1];
            if (perPage < cruPos && cruPos < nextPage)
            {
                if (offsetPage == 0)
                {
                    dir = (cruPos - perPage) < (nextPage - cruPos) ? -1 : 1;
                    return (cruPos - perPage) < (nextPage - cruPos) ? perPage : nextPage;
                }
                dir = offsetPage == -1 ? -1 : 1;
                return offsetPage == -1 ? perPage : nextPage;
            }
        }

        return -1f;
    }

    public void SetMovePath(MovePathMono path)
    {
        _movePath = path;
    }

    /// <summary>
    /// 判断是否触发了拖动事件
    /// </summary>
    /// <returns></returns>
    public bool IsDraged()
    {
        if (_movePath == null)
            return false;

        return _bDrag;
    }
    
}
