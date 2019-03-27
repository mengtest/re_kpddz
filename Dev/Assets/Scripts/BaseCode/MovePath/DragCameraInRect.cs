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


public class DragCameraInRect : MonoBehaviour
{
    public enum EDragType
    {
        NONE,//纯拖动，无效果
        MOMENTUM,//惯性效果
    }
    public Vector3 _minPoint;
    public Vector3 _maxPoint;
    public Vector3 _dragSpeedVec = Vector3.zero;//拖动速度
    public float momentumAmount = 150f;//惯性速度
    public delegate void EventOnDrag(Vector3 value);
    public EventOnDrag _onDragEvent = null;
    bool _disableDrag = false;

    private bool _bPressed = false;
    private Vector3 _pressedMousePos;//点下去时鼠标的位置
    private Vector3 _pressedCameraPos;//点下去时镜头的位置
    private Camera uiCamera;
    private Vector2 _mMomentum = Vector2.zero;
    private Vector2 _deltaDrag = Vector2.zero;
    private bool _bDrag = false;//true时不能点击场景中的物体
    public bool _bDragDirReverse = true;//镜头移动方向与拖动方向相反时
    public Vector2 _scale = Vector2.one;
    public EDragType _dragType = EDragType.MOMENTUM;
    public Vector3 _curCameraPos;//当前镜头的位置(0~1)
    public bool _bIgnorMyChildrenUI = false;//true时不被UI遮挡(仅限该组件所挂载的UI以及它的孩子，之外的UI还是会遮拦)
    /// <summary>
    /// How much momentum gets applied when the press is released after dragging.
    /// </summary>
    private Vector3 _lastFrameMousePos = Vector3.zero;

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

        if (_bPressed)
        {
            Vector3 mousePose = Input.mousePosition;//cam.ScreenToWorldPoint(Input.mousePosition);//hitt.transform.position; //
            float offsetX = mousePose.x - _pressedMousePos.x;
            float offsetY = mousePose.y - _pressedMousePos.y;
            if (Mathf.Abs(offsetX) >= 10f || Mathf.Abs(offsetY) >= 10f)
            {
                if (_bDragDirReverse)
                {
                    _curCameraPos.x = _pressedCameraPos.x + offsetX * _dragSpeedVec.x / 2000f;
                    _curCameraPos.y = _pressedCameraPos.y + offsetY * _dragSpeedVec.y / 2000f;
                    _curCameraPos.z = _pressedCameraPos.z + offsetY * _dragSpeedVec.z / 2000f;
                }
                else
                {
                    _curCameraPos.x = _pressedCameraPos.x - offsetX * _dragSpeedVec.x / 2000f;
                    _curCameraPos.y = _pressedCameraPos.y - offsetX * _dragSpeedVec.y / 2000f;
                    _curCameraPos.z = _pressedCameraPos.z - offsetY * _dragSpeedVec.z / 2000f;
                }
                    
                _curCameraPos = ClampPoint(_curCameraPos, _minPoint, _maxPoint);
                transform.position = _curCameraPos;
//                _movePath.UpdateRotation();
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
                    _curCameraPos.x += offsetPos.x * _dragSpeedVec.x / 2000f;
                    _curCameraPos.z += offsetPos.y * _dragSpeedVec.z / 2000f;
                    _curCameraPos.y += offsetPos.y * _dragSpeedVec.y / 2000f;
                    _curCameraPos = ClampPoint(_curCameraPos, _minPoint, _maxPoint);
                    transform.position = _curCameraPos;
                   // _movePath.UpdateRotation();
                    if (_onDragEvent != null)
                        _onDragEvent(_curCameraPos);
                }
                else
                {
                    NGUIMath.SpringDampen(ref _mMomentum, 9f, RealTime.deltaTime);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))//点下去
        {
            if (UtilTools.ClickInValidArea())//点的是有效区
            {
                if (!HasUICoverSelf() || !UtilTools.ClickUI()) //忽略UI || 没点到UI
                {
                    if (_dragType != EDragType.NONE)
                    {
                        //还在滑动
                        if (_mMomentum.magnitude > 1f)  _bDrag = true;
                        else _bDrag = false;
                    }
                    else
                        _bDrag = false;

                    _curCameraPos = transform.position;
                    _bPressed = true;
                    _pressedMousePos = Input.mousePosition;
                    _pressedCameraPos = _curCameraPos;
                    _mMomentum = Vector2.zero;
                    _deltaDrag = Vector2.zero;
                    _lastFrameMousePos = Vector3.zero;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))//弹起
        {
            _bPressed = false;
        }
    }

    //判断当前点击位置是否被其他控件挡住
    bool HasUICoverSelf()
    {
        if (_bIgnorMyChildrenUI)
        {
            Transform touchTarget = null;
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000, 1 << LayerMask.NameToLayer("UI"));
            bool hasCoverSelf = false;
            for (int i = 0; i < hits.Length; i++ )
            {
                touchTarget = hits[i].collider.transform;
                if (touchTarget != null && touchTarget != transform  && !touchTarget.IsChildOf(transform))//有点到东西 && 不是自己 && 不是自己的孩子
                {
                    return true;
                }
            }

            return hasCoverSelf;
        }
        else
        {
            return true;
        }
    }


    Vector3 ClampPoint(Vector3 paramPoint, Vector3 minPoint, Vector3 maxPoint)
    {
        if (Mathf.Abs(minPoint.x - maxPoint.x) > float.Epsilon)
        {
            paramPoint.x = Mathf.Clamp(paramPoint.x, minPoint.x, maxPoint.x);
        }
        if (Mathf.Abs(minPoint.y - maxPoint.y) > float.Epsilon)
        {
            paramPoint.y = Mathf.Clamp(paramPoint.y, minPoint.y, maxPoint.y);
        }
        if (Mathf.Abs(minPoint.z - maxPoint.z) > float.Epsilon)
        {
            paramPoint.z = Mathf.Clamp(paramPoint.z, minPoint.z, maxPoint.z);
        }
        if (Mathf.Abs(minPoint.y - maxPoint.y) > float.Epsilon)
        {
            paramPoint.y = Mathf.Clamp(paramPoint.y, minPoint.y, maxPoint.y);
        }
        return paramPoint;
    }
    
    /// <summary>
    /// 判断是否触发了拖动事件
    /// </summary>
    /// <returns></returns>
    public bool IsDraged()
    {
        return _bDrag;
    }
    
}
