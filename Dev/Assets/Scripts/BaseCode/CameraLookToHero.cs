/***************************************************************

 *
 *
 * Filename:  	CameraLookToHero.cs	
 * Summary: 	镜头追随英雄。(需要在编辑战斗的unity场景时，将该组件挂在场景镜头上，并调好_cameraDir)
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/12/08 20:11
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System;


 //[RequireComponent(typeof(UIWidget))]
public class CameraLookToHero : MonoBehaviour
{
    public Transform _tfLookAtTarget;
    public float _cameraDir = 0f;//0~360表示镜头在以英雄为中心的360度的哪个方向上。(每个场景都会不同)
    public bool _useDefaultAngle = true;
    public float _depressionAngles = 45f;//俯视角;
    public bool _useDefaultHeight = true;
    public float _height = 6f;//镜头与英雄的高度差;


    private float _defaultAngle = 45f;
    private float _defaultHeight = 7.5f;
    private bool _pause = false;//暂停追随英雄
    private float Pi = 3.1415926f;
    private bool _bInitRotation = false;

    //是否锁定
    private bool _bLocked = false;

    /// <summary>
    /// 锁定
    /// </summary>
    public bool Lock
    {
        get { return _bLocked; }
        set { _bLocked = value; }
    }

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    /// <param name="tfTarget"></param>
    public void setLookAtTarget(Transform tfTarget)
    {
        if (_bLocked) return;
        if (tfTarget == null) return;

        _tfLookAtTarget = tfTarget;
    }


    void Awake()
    {
        if (_useDefaultAngle)
        {
            _depressionAngles = _defaultAngle;
        }
        if (_useDefaultHeight)
        {
            _height = _defaultHeight;
        }
       
    }

    void CalculateCameraPos()
    {
        if (_tfLookAtTarget == null)
        {
        }
    }

    void LateUpdate()
    {
        if (_tfLookAtTarget == null ) return;
        if (_pause) return;

        Vector3 newPos = CalculatePosByTarget(_tfLookAtTarget);
        transform.position = Vector3.Lerp(transform.position, newPos, 5f * Time.deltaTime);
        if (!_bInitRotation)
        {
            _bInitRotation = true;
            Vector3 lookToPos = CalculateLookAtPos();
            transform.rotation = Quaternion.LookRotation(lookToPos - transform.position);//Quaternion.Euler(new Vector3(_depressionAngles, _cameraDir, 0f));
        }

#if UNITY_EDITOR
        _bInitRotation = false;//为方便调试，每帧刷新朝向
#endif
    }

    //镜头看向该目标时应该所处的位置
    public Vector3 CalculatePosByTarget(Transform tfTarget)
    {

        float r = _height * Mathf.Tan(Pi * (90f - _depressionAngles) / 180f);//r： 镜头与英雄的水平距离
        Vector3 newPos = Vector3.zero;
        newPos.y = tfTarget.position.y + _height;
        newPos.x = tfTarget.position.x + r * Mathf.Cos(Pi * -_cameraDir / 180f);
        newPos.z = tfTarget.position.z + r * Mathf.Sin(Pi * -_cameraDir / 180f);
        return newPos;
    }

    public Vector3 CalculateLookAtPos()
    {
        float r = _height * Mathf.Tan(Pi * (90f - _depressionAngles) / 180f);//r： 镜头与英雄的水平距离
        Vector3 lookToPos = Vector3.zero;
        lookToPos.y = transform.position.y - _height;
        lookToPos.x = transform.position.x - r * Mathf.Cos(Pi * -_cameraDir / 180f);
        lookToPos.z = transform.position.z - r * Mathf.Sin(Pi * -_cameraDir / 180f);
        return lookToPos;
        
    }

    public void Pause()
    {
        _pause = true;
    }
    public void Consume()
    {
        _pause = false;
    }
 
 }
